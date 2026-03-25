using DataToExcel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormTool.Forms;

namespace WindowsFormTool.TskUtil
{
    public class TskDpatInkProcessor : ITskProcessor
    {
        private const int MaxBatchCount = 25;
        private static readonly Regex NumberRegex = new Regex(
            @"[-+]?(?:\d+\.?\d*|\.\d+)(?:[eE][-+]?\d+)?",
            RegexOptions.Compiled);

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

        private void ProcessMappedFiles(
            Dictionary<string, string> mapping,
            IReadOnlyList<DpatInkTestConfig> testConfigs,
            bool allowMissingTestName,
            Action<string> updateStatus,
            ProgressBar progressBar)
        {
            var contexts = new Dictionary<string, CsvContext>();
            var errors = new List<string>();

            foreach (var pair in mapping)
            {
                var tskPath = pair.Key;
                var csvPath = pair.Value;

                if (!TryParseLayout(csvPath, out var layout, out var lines, out var layoutError))
                {
                    errors.Add($"{Path.GetFileName(csvPath)}: {layoutError}");
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
                updateStatus($"CSV缺少测试项，已按设置跳过：{string.Join("；", errors)}\n");
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
                updateStatus($"开始处理：{Path.GetFileName(tskPath)}\n");

                var layout = pair.Value.Layout;
                var lines = pair.Value.Lines;
                var availableConfigs = testConfigs
                    .Where(config => layout.TestNameToColumn.ContainsKey(config.TestName))
                    .ToList();

                if (availableConfigs.Count == 0)
                {
                    updateStatus("当前CSV缺少已配置测试项，跳过。\n");
                    continue;
                }

                Tsk tsk = null;
                var totalInked = 0;

                foreach (var config in availableConfigs)
                {
                    var testColumnIndex = layout.TestNameToColumn[config.TestName];
                    if (!layout.TestLimits.TryGetValue(testColumnIndex, out var limitsFromCsv))
                    {
                        updateStatus($"[{config.TestName}] CSV中缺少 MIN/MAX，跳过。\n");
                        continue;
                    }

                    var dataPoints = ReadDataPoints(lines, layout, testColumnIndex);
                    var lowerLimit = limitsFromCsv.Lower;
                    var upperLimit = limitsFromCsv.Upper;
                    if (lowerLimit > upperLimit)
                    {
                        var temp = lowerLimit;
                        lowerLimit = upperLimit;
                        upperLimit = temp;
                        updateStatus($"[{config.TestName}] CSV中的 MIN/MAX 顺序反向，已自动纠正。\n");
                    }

                    updateStatus($"[{config.TestName}] CSV上下限：MIN={lowerLimit:F6}, MAX={upperLimit:F6}\n");

                    var filtered = FilterByBinAndLimit(dataPoints, lowerLimit, upperLimit);
                    updateStatus($"[{config.TestName}] 筛选后样本数：{filtered.Count}\n");

                    if (filtered.Count == 0)
                    {
                        updateStatus($"[{config.TestName}] 无可用 Bin=1 样本，跳过。\n");
                        continue;
                    }

                    var limits = config.UseFormula2
                        ? CalculateIqrLimits(filtered.Select(point => point.Value).ToList(), config.Sigma)
                        : CalculateStdLimits(filtered.Select(point => point.Value).ToList(), config.Sigma);

                    updateStatus($"[{config.TestName}] 统计上下限：下限={limits.Lower:F6}, 上限={limits.Upper:F6}\n");

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

                        var inkResult = ApplyInk(tsk, outliers, config.InkBin);
                        totalInked += inkResult.AppliedCount;
                        if (inkResult.MissedCount > 0)
                            updateStatus($"[{config.TestName}] 坐标未命中TSK：{inkResult.MissedCount}（已跳过）\n");
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

        private static Dictionary<string, string> MatchCsvFiles(
            IEnumerable<string> tskFiles,
            IEnumerable<string> csvFiles,
            List<string> errors)
        {
            var mapping = new Dictionary<string, string>();
            var csvList = csvFiles
                .Select(path => new { Path = path, Name = Path.GetFileNameWithoutExtension(path) })
                .ToList();

            foreach (var tskPath in tskFiles)
            {
                var tskName = Path.GetFileName(tskPath);
                var tskNameParts = tskName.Split('.');
                var matchKey = tskNameParts.Length > 1
                    ? tskNameParts[1]
                    : Path.GetFileNameWithoutExtension(tskPath);

                var matches = csvList
                    .Where(csv => csv.Name.IndexOf(matchKey, StringComparison.OrdinalIgnoreCase) >= 0)
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

        private static bool TryParseLayout(string csvPath, out CsvLayout layout, out List<string> lines, out string error)
        {
            layout = new CsvLayout();
            lines = new List<string>();
            error = null;
            List<string> dataHeaderFields = null;
            List<string> minFields = null;
            List<string> maxFields = null;

            if (!File.Exists(csvPath))
            {
                error = $"CSV不存在：{csvPath}";
                return false;
            }

            using (var reader = new StreamReader(csvPath, Encoding.UTF8, true))
            {
                string line;
                var index = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                    var fields = SplitCsvLine(line);

                    if (layout.DataHeaderIndex < 0 && fields.Count >= 6)
                    {
                        if (TryLocateDataHeader(fields, out var binIndex, out var xIndex, out var yIndex))
                        {
                            layout.DataHeaderIndex = index;
                            layout.BinIndex = binIndex;
                            layout.XIndex = xIndex;
                            layout.YIndex = yIndex;
                            layout.TestStartIndex = yIndex + 1;
                            dataHeaderFields = fields;
                        }
                    }
                    else if (layout.DataHeaderIndex >= 0 && fields.Count > 0)
                    {
                        var marker = fields[0].Trim();
                        if (minFields == null && marker.Equals("MIN", StringComparison.OrdinalIgnoreCase))
                        {
                            minFields = fields;
                        }
                        else if (maxFields == null && marker.Equals("MAX", StringComparison.OrdinalIgnoreCase))
                        {
                            maxFields = fields;
                        }
                    }

                    index++;
                }
            }

            if (layout.DataHeaderIndex < 0)
            {
                error = "未找到数据区表头（TestNo,SiteNo,Bin,Time/mS,X,Y）";
                return false;
            }

            if (dataHeaderFields == null)
            {
                error = "未读取到数据区表头字段";
                return false;
            }

            var testStartIndex = layout.TestStartIndex >= 0 ? layout.TestStartIndex : 6;
            for (int columnIndex = testStartIndex; columnIndex < dataHeaderFields.Count; columnIndex++)
            {
                var testName = dataHeaderFields[columnIndex].Trim();
                if (string.IsNullOrEmpty(testName))
                    continue;

                if (!layout.TestNameToColumn.ContainsKey(testName))
                {
                    layout.TestNameToColumn[testName] = columnIndex;
                    layout.TestNames.Add(testName);
                }
            }

            if (layout.TestNames.Count == 0)
            {
                error = "数据表头中 Y 后未找到测试项";
                return false;
            }

            if (minFields == null || maxFields == null)
            {
                error = "未找到 MIN/MAX 行";
                return false;
            }

            foreach (var pair in layout.TestNameToColumn)
            {
                if (TryReadLimit(minFields, maxFields, pair.Value, out var lower, out var upper))
                {
                    layout.TestLimits[pair.Value] = (lower, upper);
                }
            }

            if (layout.TestLimits.Count == 0)
            {
                error = "CSV中测试项MIN/MAX解析失败";
                return false;
            }

            return true;
        }

        private static List<DpatDataPoint> ReadDataPoints(List<string> lines, CsvLayout layout, int testColumnIndex)
        {
            var data = new List<DpatDataPoint>();

            for (int lineIndex = layout.DataHeaderIndex + 1; lineIndex < lines.Count; lineIndex++)
            {
                var fields = SplitCsvLine(lines[lineIndex]);
                if (fields.Count <= testColumnIndex || layout.BinIndex < 0 || layout.XIndex < 0 || layout.YIndex < 0)
                    continue;

                var requiredMax = Math.Max(testColumnIndex, Math.Max(layout.XIndex, layout.YIndex));
                if (fields.Count <= requiredMax)
                    continue;

                if (!TryParseInt(fields[layout.BinIndex], out var bin))
                    continue;

                if (!TryParseInt(fields[layout.XIndex], out var x) || !TryParseInt(fields[layout.YIndex], out var y))
                    continue;

                if (!TryParseMeasurementValue(fields[testColumnIndex], out var value))
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

        private static List<DpatDataPoint> FilterByBinAndLimit(List<DpatDataPoint> data, double lower, double upper)
        {
            return data
                .Where(point => point.Bin == 1 && point.Value >= lower && point.Value <= upper)
                .ToList();
        }

        private static (double Lower, double Upper) CalculateStdLimits(List<double> values, double sigma)
        {
            var mean = values.Average();
            var variance = values.Sum(value => (value - mean) * (value - mean)) / values.Count;
            var std = Math.Sqrt(variance);
            return (mean - sigma * std, mean + sigma * std);
        }

        private static (double Lower, double Upper) CalculateIqrLimits(List<double> values, double sigma)
        {
            values.Sort();
            var median = Percentile(values, 0.5);
            var p25 = Percentile(values, 0.25);
            var p75 = Percentile(values, 0.75);
            var specialSigma = (p75 - p25) / 1.35;
            return (median - sigma * specialSigma, median + sigma * specialSigma);
        }

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
            var set = new HashSet<(int X, int Y)>();
            foreach (var point in data)
            {
                if (point.Value < lower || point.Value > upper)
                    set.Add((point.X, point.Y));
            }

            return set;
        }

        private static (int AppliedCount, int MissedCount) ApplyInk(Tsk tsk, IEnumerable<(int X, int Y)> coords, int inkBin)
        {
            var appliedCount = 0;
            var missedCount = 0;

            var dieByCoordinate = new Dictionary<(int X, int Y), DieData>();
            for (int index = 0; index < tsk.DieMatrix.Count; index++)
            {
                var die = tsk.DieMatrix[index];
                var key = (die.X, die.Y);
                if (!dieByCoordinate.ContainsKey(key))
                    dieByCoordinate[key] = die;
            }

            foreach (var coord in coords)
            {
                if (dieByCoordinate.TryGetValue((coord.X, coord.Y), out var dieByCoordinateMatch))
                {
                    dieByCoordinateMatch.Attribute = DieCategory.FailDie;
                    dieByCoordinateMatch.Bin = inkBin;
                    appliedCount++;
                    continue;
                }

                if (coord.X >= 0 && coord.Y >= 0 && coord.X < tsk.DieMatrix.XMax && coord.Y < tsk.DieMatrix.YMax)
                {
                    var dieByIndex = tsk.DieMatrix[coord.X, coord.Y];
                    dieByIndex.Attribute = DieCategory.FailDie;
                    dieByIndex.Bin = inkBin;
                    appliedCount++;
                    continue;
                }

                missedCount++;
            }

            return (appliedCount, missedCount);
        }

        private static void RecalculateStats(Tsk tsk)
        {
            tsk.PassDie = 0;
            tsk.FailDie = 0;
            for (int index = 0; index < tsk.Rows * tsk.Cols; index++)
            {
                if (tsk.DieMatrix[index].Attribute == DieCategory.PassDie)
                    tsk.PassDie++;
                else if (tsk.DieMatrix[index].Attribute == DieCategory.FailDie)
                    tsk.FailDie++;
            }

            tsk.TotalDie = tsk.PassDie + tsk.FailDie;
        }

        private static string BuildOutputPath(string tskPath)
        {
            var basePath = TskFileHelper.SavePath;
            if (string.IsNullOrWhiteSpace(basePath))
                basePath = @"D:\New-Tsk\";

            var outputDir = Path.Combine(basePath, "DPAT_INK");
            Directory.CreateDirectory(outputDir);

            var fileName = Path.GetFileName(tskPath) + "_DPAT";
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

            if (binIndex < 0 || xIndex < 0 || yIndex < 0)
                return false;

            if (!(binIndex < xIndex && xIndex < yIndex))
                return false;

            var testNoIndex = FindHeaderIndex(fields, "TestNo");
            var siteNoIndex = FindHeaderIndex(fields, "SiteNo");
            var timeIndex = FindHeaderIndex(fields, "Time/mS");
            var isNewHeader = testNoIndex >= 0 &&
                              siteNoIndex >= 0 &&
                              timeIndex >= 0 &&
                              testNoIndex < siteNoIndex &&
                              siteNoIndex < binIndex &&
                              binIndex < timeIndex &&
                              timeIndex < xIndex;

            if (isNewHeader)
                return true;

            var siteIndex = FindHeaderIndex(fields, "Site");
            var serialIndex = FindHeaderIndex(fields, "Serial");
            var sbinIndex = FindHeaderIndex(fields, "Sbin");
            var isOldHeader = siteIndex >= 0 &&
                              serialIndex >= 0 &&
                              sbinIndex >= 0 &&
                              siteIndex < serialIndex &&
                              serialIndex < sbinIndex &&
                              sbinIndex < binIndex;

            return isOldHeader;
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

        private static bool TryReadLimit(List<string> minFields, List<string> maxFields, int columnIndex, out double lower, out double upper)
        {
            lower = 0;
            upper = 0;

            if (columnIndex < 0 || columnIndex >= minFields.Count || columnIndex >= maxFields.Count)
                return false;

            if (!TryParseMeasurementValue(minFields[columnIndex], out lower))
                return false;

            if (!TryParseMeasurementValue(maxFields[columnIndex], out upper))
                return false;

            return true;
        }

        private static bool TryParseInt(string value, out int result)
        {
            return int.TryParse(value?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        private static bool TryParseDouble(string value, out double result)
        {
            return double.TryParse(value?.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        private static bool TryParseMeasurementValue(string value, out double result)
        {
            if (TryParseDouble(value, out result))
                return true;

            result = 0;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var match = NumberRegex.Match(value.Trim());
            if (!match.Success)
                return false;

            return double.TryParse(match.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        private sealed class CsvLayout
        {
            public int DataHeaderIndex = -1;
            public int BinIndex = -1;
            public int XIndex = -1;
            public int YIndex = -1;
            public int TestStartIndex = -1;
            public List<string> TestNames = new List<string>();
            public Dictionary<string, int> TestNameToColumn = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            public Dictionary<int, (double Lower, double Upper)> TestLimits = new Dictionary<int, (double Lower, double Upper)>();
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
