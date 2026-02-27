using DataToExcel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormTool.Forms;

namespace WindowsFormTool.TskUtil
{
    public class TskDpatInkProcessor : ITskProcessor
    {
        // CSV 格式参考 docs/Csv格式.csv（Test Name 行 + Site,Serial,Sbin,Bin,X,Y 表头）
        private const int MaxBatchCount = 25;

        public void ProcessSingle(string tskPath, string csvPath, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (string.IsNullOrEmpty(tskPath) || string.IsNullOrEmpty(csvPath))
            {
                MessageBox.Show(@"请先选择TSK文件和CSV文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProcessBatch(new List<string> { tskPath }, new List<string> { csvPath }, updateStatus, progressBar);
        }

        public void ProcessBatch(List<string> tskFiles, List<string> csvFiles, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (tskFiles == null || tskFiles.Count == 0)
            {
                MessageBox.Show(@"请先选择TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (csvFiles == null || csvFiles.Count == 0)
            {
                MessageBox.Show(@"请先选择CSV文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tskFiles.Count > MaxBatchCount)
            {
                MessageBox.Show(@"一次最多处理25个TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var mappingErrors = new List<string>();
            var mapping = MatchCsvFiles(tskFiles, csvFiles, mappingErrors);
            if (mappingErrors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", mappingErrors), "CSV匹配失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                updateStatus($"CSV匹配失败：{string.Join("；", mappingErrors)}\n");
                return;
            }

            var firstCsv = mapping.Values.FirstOrDefault();
            if (string.IsNullOrEmpty(firstCsv))
            {
                MessageBox.Show(@"未找到可用的CSV文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryParseLayout(firstCsv, out var layout, out var lines, out var layoutError))
            {
                MessageBox.Show(layoutError, "CSV解析失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dialog = new DpatInkDialog(layout.TestNames))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                var testConfigs = dialog.SelectedTests;
                if (testConfigs == null || testConfigs.Count == 0)
                    return;

                ProcessMappedFiles(mapping, testConfigs, dialog.AllowMissingTestName, updateStatus, progressBar);
            }
        }

        private void ProcessMappedFiles(Dictionary<string, string> mapping, IReadOnlyList<DpatInkTestConfig> testConfigs,
            bool allowMissingTestName, Action<string> updateStatus, ProgressBar progressBar)
        {
            var contexts = new Dictionary<string, CsvContext>();
            var errors = new List<string>();

            foreach (var pair in mapping)
            {
                var tskPath = pair.Key;
                var csvPath = pair.Value;

                if (!TryParseLayout(csvPath, out var layout, out var lines, out var layoutError))
                {
                    errors.Add($"{Path.GetFileName(csvPath)}：{layoutError}");
                    continue;
                }

                var missingTests = new List<string>();
                foreach (var config in testConfigs)
                {
                    if (!layout.TestNameToColumn.ContainsKey(config.TestName))
                        missingTests.Add(config.TestName);
                }

                if (missingTests.Count > 0)
                    errors.Add($"{Path.GetFileName(csvPath)} 缺少测试项：{string.Join(", ", missingTests)}");

                contexts[tskPath] = new CsvContext
                {
                    CsvPath = csvPath,
                    Layout = layout,
                    Lines = lines
                };
            }

            if (errors.Count > 0 && !allowMissingTestName)
            {
                MessageBox.Show(string.Join("\n", errors), "CSV测试项校验失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                updateStatus($"CSV测试项校验失败：{string.Join("；", errors)}\n");
                return;
            }

            if (errors.Count > 0 && allowMissingTestName)
            {
                updateStatus($"CSV测试项缺失，已跳过：{string.Join("；", errors)}\n");
            }

            if (contexts.Count == 0)
            {
                MessageBox.Show(@"没有可处理的CSV文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (progressBar != null)
            {
                progressBar.Maximum = contexts.Count;
                progressBar.Value = 0;
            }

            foreach (var pair in contexts)
            {
                var tskPath = pair.Key;
                var csvPath = pair.Value.CsvPath;
                updateStatus($"开始处理：{Path.GetFileName(tskPath)}\n");

                var layout = pair.Value.Layout;
                var lines = pair.Value.Lines;
                var availableConfigs = testConfigs
                    .Where(c => layout.TestNameToColumn.ContainsKey(c.TestName))
                    .ToList();

                if (availableConfigs.Count == 0)
                {
                    updateStatus("当前CSV缺少所有已配置测试项，跳过。\n");
                    continue;
                }

                Tsk tsk = null;
                var totalInked = 0;

                foreach (var config in availableConfigs)
                {
                    var testColumnIndex = layout.TestNameToColumn[config.TestName];
                    var dataPoints = ReadDataPoints(lines, layout, testColumnIndex);
                    var lowerLimit = config.LowerLimit;
                    var upperLimit = config.UpperLimit;
                    if (lowerLimit > upperLimit)
                    {
                        var temp = lowerLimit;
                        lowerLimit = upperLimit;
                        upperLimit = temp;
                        updateStatus($"[{config.TestName}] 检测到上下限输入反向，已自动纠正。\n");
                    }

                    var filtered = FilterByBinAndLimit(dataPoints, lowerLimit, upperLimit);
                    updateStatus($"[{config.TestName}] 筛选后样本数：{filtered.Count}\n");

                    if (filtered.Count == 0)
                    {
                        updateStatus($"[{config.TestName}] 筛选后无有效数据，跳过。\n");
                        continue;
                    }

                    var limits = config.UseFormula2
                        ? CalculateIqrLimits(filtered.Select(d => d.Value).ToList(), config.Sigma)
                        : CalculateStdLimits(filtered.Select(d => d.Value).ToList(), config.Sigma);

                    updateStatus($"[{config.TestName}] 新下限={limits.Lower:F6}, 新上限={limits.Upper:F6}\n");

                    var outliers = GetOutlierCoordinates(filtered, limits.Lower, limits.Upper);
                    updateStatus($"[{config.TestName}] 越界点数量：{outliers.Count}\n");

                    if (outliers.Count == 0)
                    {
                        updateStatus($"[{config.TestName}] 无越界点，跳过写回。\n");
                        continue;
                    }

                    try
                    {
                        if (tsk == null)
                            tsk = TskFileLoader.LoadTsk(tskPath);

                        var inkedCount = ApplyInk(tsk, outliers, config.InkBin);
                        totalInked += inkedCount;
                    }
                    catch (Exception ex)
                    {
                        updateStatus($"[{config.TestName}] 处理失败：{ex.Message}\n");
                    }
                }

                if (tsk == null || totalInked == 0)
                {
                    updateStatus("无越界点，跳过写回。\n");
                    continue;
                }

                try
                {
                    RecalculateStats(tsk);
                    var outputPath = BuildOutputPath(tskPath);
                    tsk.Save(outputPath);
                    updateStatus($"已保存：{outputPath}，INK数量：{totalInked}\n");
                }
                catch (Exception ex)
                {
                    updateStatus($"处理失败：{ex.Message}\n");
                }

                if (progressBar != null && progressBar.Value < progressBar.Maximum)
                    progressBar.Value++;
            }
        }

        private static Dictionary<string, string> MatchCsvFiles(IEnumerable<string> tskFiles, IEnumerable<string> csvFiles, List<string> errors)
        {
            var mapping = new Dictionary<string, string>();
            var csvList = csvFiles
                .Select(path => new { Path = path, Name = Path.GetFileNameWithoutExtension(path) })
                .ToList();

            foreach (var tskPath in tskFiles)
            {
                var tskName = Path.GetFileName(tskPath);
                string[] tskNames = tskName.Split('.');
                var matches = csvList
                    .Where(c => c.Name.IndexOf(tskNames[1], StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                if (matches.Count == 0)
                {
                    errors.Add($"未找到匹配CSV：{Path.GetFileName(tskPath)}");
                    continue;
                }

                if (matches.Count > 1)
                {
                    errors.Add($"CSV匹配不唯一：{Path.GetFileName(tskPath)}");
                    continue;
                }

                mapping[tskPath] = matches[0].Path;
            }

            return mapping;
        }

        // 解析 CSV 布局（定位 Test Name 行与数据区表头），并返回原始行用于后续取值。
        private static bool TryParseLayout(string csvPath, out CsvLayout layout, out List<string> lines, out string error)
        {
            layout = new CsvLayout();
            lines = new List<string>();
            error = null;
            List<string> testNameFields = null;

            if (!File.Exists(csvPath))
            {
                error = $"CSV不存在：{csvPath}";
                return false;
            }

            // 按 UTF-8 优先读取（CSV 可能包含非 ASCII）
            using (var reader = new StreamReader(csvPath, Encoding.UTF8, true))
            {
                string line;
                var index = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                    var fields = SplitCsvLine(line);

                    if (layout.TestNameRowIndex < 0 && fields.Count > 0 &&
                        fields[0].Trim().Equals("Test Name", StringComparison.OrdinalIgnoreCase))
                    {
                        layout.TestNameRowIndex = index;
                        testNameFields = fields;
                    }

                    if (layout.DataHeaderIndex < 0 && fields.Count >= 6)
                    {
                        if (TryLocateDataHeader(fields, out var binIndex, out var xIndex, out var yIndex))
                        {
                            layout.DataHeaderIndex = index;
                            layout.BinIndex = binIndex;
                            layout.XIndex = xIndex;
                            layout.YIndex = yIndex;
                            layout.TestStartIndex = yIndex + 1;
                        }
                    }

                    index++;
                }
            }

            if (layout.TestNameRowIndex < 0)
            {
                error = "未找到 Test Name 行";
                return false;
            }

            if (layout.DataHeaderIndex < 0)
            {
                error = "未找到数据区表头（Site,Serial,Sbin,Bin,X,Y）";
                return false;
            }

            var testStartIndex = layout.TestStartIndex >= 0 ? layout.TestStartIndex : 6;
            if (testNameFields != null)
            {
                for (int i = testStartIndex; i < testNameFields.Count; i++)
                {
                    var name = testNameFields[i].Trim();
                    if (!string.IsNullOrEmpty(name) && !layout.TestNameToColumn.ContainsKey(name))
                    {
                        layout.TestNameToColumn[name] = i;
                        layout.TestNames.Add(name);
                    }
                }
            }

            if (layout.TestNames.Count == 0)
            {
                error = "Test Name 行为空";
                return false;
            }

            return true;
        }

        private static List<DpatDataPoint> ReadDataPoints(List<string> lines, CsvLayout layout, int testColumnIndex)
        {
            var data = new List<DpatDataPoint>();

            for (int i = layout.DataHeaderIndex + 1; i < lines.Count; i++)
            {
                var fields = SplitCsvLine(lines[i]);
                if (fields.Count <= testColumnIndex || layout.BinIndex < 0 || layout.XIndex < 0 || layout.YIndex < 0)
                    continue;

                var requiredMax = Math.Max(testColumnIndex, Math.Max(layout.XIndex, layout.YIndex));
                if (fields.Count <= requiredMax)
                    continue;

                // Bin / X / Y / 测试值任一不可解析则跳过该行
                if (!TryParseInt(fields[layout.BinIndex], out var bin))
                    continue;

                if (!TryParseInt(fields[layout.XIndex], out var x) || !TryParseInt(fields[layout.YIndex], out var y))
                    continue;

                if (!TryParseDouble(fields[testColumnIndex], out var value))
                    continue;

                data.Add(new DpatDataPoint
                {
                    X = x,
                    Y = y,
                    Bin = bin,
                    Value = value
                });
            }

            return data;
        }

        // 仅使用 Bin=1 且落在初始上下限内的数据作为统计样本
        private static List<DpatDataPoint> FilterByBinAndLimit(List<DpatDataPoint> data, double lower, double upper)
        {
            return data
                .Where(d => d.Bin == 1 && d.Value >= lower && d.Value <= upper)
                .ToList();
        }

        // 公式1：均值/标准差
        private static (double Lower, double Upper) CalculateStdLimits(List<double> values, double sigma)
        {
            var mean = values.Average();
            var variance = values.Sum(v => (v - mean) * (v - mean)) / values.Count;
            var std = Math.Sqrt(variance);
            return (mean - sigma * std, mean + sigma * std);
        }

        // 公式2：中位数/IQR
        private static (double Lower, double Upper) CalculateIqrLimits(List<double> values, double sigma)
        {
            values.Sort();
            var median = Percentile(values, 0.5);
            var p25 = Percentile(values, 0.25);
            var p75 = Percentile(values, 0.75);
            var specialSigma = (p75 - p25) / 1.35;
            return (median - sigma * specialSigma, median + sigma * specialSigma);
        }

        // 线性插值的百分位计算，输入需已排序
        private static double Percentile(List<double> sortedValues, double percentile)
        {
            if (sortedValues.Count == 1)
                return sortedValues[0];

            var position = (sortedValues.Count - 1) * percentile;
            var lowerIndex = (int)Math.Floor(position);
            var upperIndex = (int)Math.Ceiling(position);
            if (lowerIndex == upperIndex)
                return sortedValues[lowerIndex];

            var weight = position - lowerIndex;
            return sortedValues[lowerIndex] + (sortedValues[upperIndex] - sortedValues[lowerIndex]) * weight;
        }

        private static HashSet<(int X, int Y)> GetOutlierCoordinates(IEnumerable<DpatDataPoint> data, double lower, double upper)
        {
            var set = new HashSet<(int, int)>();
            foreach (var item in data)
            {
                if (item.Value < lower || item.Value > upper)
                    set.Add((item.X, item.Y));
            }
            return set;
        }

        // 通过矩阵索引回写 INK（坐标异常时跳过）
        private static int ApplyInk(Tsk tsk, IEnumerable<(int X, int Y)> coords, int inkBin)
        {
            var count = 0;
            foreach (var coord in coords)
            {
                if (coord.X < 0 || coord.Y < 0 || coord.X >= tsk.DieMatrix.XMax || coord.Y >= tsk.DieMatrix.YMax)
                    continue;

                var die = tsk.DieMatrix[coord.X, coord.Y];
                die.Attribute = DieCategory.FailDie;
                die.Bin = inkBin;
                count++;
            }

            return count;
        }

        // 回写后重新统计 Pass/Fail/Total
        private static void RecalculateStats(Tsk tsk)
        {
            tsk.PassDie = 0;
            tsk.FailDie = 0;
            for (int i = 0; i < tsk.Rows * tsk.Cols; i++)
            {
                if (tsk.DieMatrix[i].Attribute == DieCategory.PassDie)
                    tsk.PassDie++;
                else if (tsk.DieMatrix[i].Attribute == DieCategory.FailDie)
                    tsk.FailDie++;
            }
            tsk.TotalDie = tsk.PassDie + tsk.FailDie;
        }

        // 输出目录：SavePath/DPAT_INK，文件名追加 _DPAT
        private static string BuildOutputPath(string tskPath)
        {
            var basePath = TskFileHelper.SavePath;
            if (string.IsNullOrWhiteSpace(basePath))
                basePath = @"D:\New-Tsk\";

            var outputDir = Path.Combine(basePath, "DPAT_INK");
            Directory.CreateDirectory(outputDir);

            var fileName = Path.GetFileNameWithoutExtension(tskPath) + "_DPAT" + Path.GetExtension(tskPath);
            return Path.Combine(outputDir, fileName);
        }

        private static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            if (line == null)
                return result;

            var sb = new StringBuilder();
            var inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }

            result.Add(sb.ToString());
            return result;
        }

        private static bool TryLocateDataHeader(List<string> fields, out int binIndex, out int xIndex, out int yIndex)
        {
            binIndex = FindHeaderIndex(fields, "Bin");
            xIndex = FindHeaderIndex(fields, "X");
            yIndex = FindHeaderIndex(fields, "Y");

            var siteIndex = FindHeaderIndex(fields, "Site");
            var serialIndex = FindHeaderIndex(fields, "Serial");
            var sbinIndex = FindHeaderIndex(fields, "Sbin");

            if (siteIndex < 0 || serialIndex < 0 || sbinIndex < 0 || binIndex < 0 || xIndex < 0 || yIndex < 0)
                return false;

            return siteIndex < serialIndex &&
                   serialIndex < sbinIndex &&
                   sbinIndex < binIndex &&
                   binIndex < xIndex &&
                   xIndex < yIndex;
        }

        private static int FindHeaderIndex(List<string> fields, string expected)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].Trim().Equals(expected, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        private static bool TryParseInt(string value, out int result)
        {
            return int.TryParse(value?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        private static bool TryParseDouble(string value, out double result)
        {
            return double.TryParse(value?.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        private sealed class CsvLayout
        {
            public int TestNameRowIndex = -1;
            public int DataHeaderIndex = -1;
            public int BinIndex = -1;
            public int XIndex = -1;
            public int YIndex = -1;
            public int TestStartIndex = -1;
            public List<string> TestNames = new List<string>();
            public Dictionary<string, int> TestNameToColumn = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        }

        private sealed class CsvContext
        {
            public string CsvPath;
            public CsvLayout Layout;
            public List<string> Lines;
        }

        private sealed class DpatDataPoint
        {
            public int X;
            public int Y;
            public int Bin;
            public double Value;
        }

    }
}
