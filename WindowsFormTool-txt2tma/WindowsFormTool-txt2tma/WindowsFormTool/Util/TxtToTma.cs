/*
 * 作者：sky
 * 时间：2008-06-25
 * 作用：用于描述 CMD 的 txt 格式的 mapping 文件
 */

namespace DataToExcel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class TxtToTma
    {
        void ConvertFile(string txtPath, Action<string> updateStatus)
        {
            var (waferInfo, dataRows, rowCount, colCount) = ParseTxt(txtPath);
            if (dataRows.Count == 0)
            {
                updateStatus("未找到RowData数据");
                return;
            }

            string tmaPath = Path.ChangeExtension(txtPath, ".tma");
            List<string> tmaLines = ConvertToTma(waferInfo, dataRows, rowCount, colCount);
            File.WriteAllLines(tmaPath, tmaLines, Encoding.UTF8);

            updateStatus($"转换完成: {txtPath} -> {tmaPath}");
            updateStatus($"数据行数: {dataRows.Count}, 每行数据数: {dataRows[0].Length}");
        }

        public void BatchConvert(string folderPath, Action<string> updateStatus)
        {
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
            if (txtFiles.Length == 0)
            {
                updateStatus("文件夹中没有txt文件");
                return;
            }

            updateStatus($"找到 {txtFiles.Length} 个txt文件");
            int count = 0;

            foreach (string txtPath in txtFiles)
            {
                try
                {
                    ConvertFile(txtPath, updateStatus);
                    count++;
                }
                catch (Exception ex)
                {
                    updateStatus($"转换失败 {Path.GetFileName(txtPath)}: {ex.Message}");
                }
            }

            updateStatus($"完成: {count}/{txtFiles.Length}");
        }

        (Dictionary<string, string> waferInfo, List<string[]> dataRows, int rowCount, int colCount) ParseTxt(
            string txtPath)
        {
            var waferInfo = new Dictionary<string, string>();
            var dataRows = new List<string[]>();
            int rowCount = 0, colCount = 0;

            foreach (string line in File.ReadAllLines(txtPath, Encoding.UTF8))
            {
                string lineTrim = line.Trim();
                if (string.IsNullOrEmpty(lineTrim)) continue;

                if (lineTrim.StartsWith("RowData:"))
                {
                    string dataPart = lineTrim.Substring(8).Trim();
                    string[] parts = dataPart.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var rowData = new List<string>();
                    foreach (string part in parts)
                        rowData.Add(part == "__" ? "__" : part);
                    dataRows.Add(rowData.ToArray());
                    if (colCount == 0) colCount = rowData.Count;
                }
                else
                {
                    int idx = lineTrim.IndexOf(':');
                    if (idx > 0)
                    {
                        string key = lineTrim.Substring(0, idx).Trim();
                        string value = lineTrim.Substring(idx + 1).Trim();
                        waferInfo[key] = value;
                    }

                    if (lineTrim.StartsWith("ROWCT:") && int.TryParse(lineTrim.Substring(6).Trim(), out int rc))
                        rowCount = rc;
                    if (lineTrim.StartsWith("COLCT:") && int.TryParse(lineTrim.Substring(6).Trim(), out int cc))
                        colCount = cc;
                }
            }

            return (waferInfo, dataRows, rowCount, colCount);
        }

        List<string> ConvertToTma(Dictionary<string, string> waferInfo, List<string[]> dataRows, int rowCount,
            int colCount)
        {
            var tmaLines = new List<string>();

            int yDigits = rowCount >= 100 ? 3 : 2;
            int xDigits = colCount >= 100 ? 3 : 2;

            // 第一行：x坐标
            var sb = new StringBuilder();
            sb.Append(' ', yDigits + 1);
            for (int i = 1; i <= colCount; i++)
                sb.Append(i.ToString().PadLeft(xDigits, '0'));
            tmaLines.Add(sb.ToString());

            // 第二行：分隔线
            sb = new StringBuilder();
            sb.Append(' ', yDigits).Append('+');
            for (int i = 0; i < colCount; i++) sb.Append("+-+");
            tmaLines.Add(sb.ToString());

            // 数据行
            for (int y = 0; y < dataRows.Count; y++)
            {
                bool isLast = y == dataRows.Count - 1;
                sb = new StringBuilder();
                sb.Append((y + 1).ToString().PadLeft(yDigits, '0')).Append('|');
                foreach (string cell in dataRows[y])
                {
                    if (cell == "__") sb.Append(isLast ? "  M" : "  .");
                    else if (cell == "01") sb.Append("  P");
                    else sb.Append("  F");
                }

                tmaLines.Add(sb.ToString());
            }

            // Wafer信息
            tmaLines.Add("");
            tmaLines.Add("============ Wafer Information () ===========");
            tmaLines.Add("  Device: " + Get(waferInfo, "DEVICE"));
            tmaLines.Add("  Lot NO: " + Get(waferInfo, "Lot"));
            tmaLines.Add("  Slot NO: ");
            tmaLines.Add("  Wafer ID: " + Get(waferInfo, "Wafer"));
            tmaLines.Add("  Operater: ");
            tmaLines.Add("  Wafer Size: ");
            tmaLines.Add("  Flat Dir: 180");
            tmaLines.Add("  Wafer Test Start Time: ");
            tmaLines.Add("  Wafer Test Finish Time: ");
            tmaLines.Add("  Wafer Load Time: ");
            tmaLines.Add("  Wafer Unload Time: ");
            int totalDieCounts = Int32.Parse(Get(waferInfo, "Total Tested"));
            int passDieCounts = Int32.Parse(Get(waferInfo, "Total Pass"));
            int failDeiCount = totalDieCounts - passDieCounts;
            tmaLines.Add("  Total test die: " + Get(waferInfo, "Total Tested"));
            tmaLines.Add("  Pass Die: " + Get(waferInfo, "Total Pass"));
            tmaLines.Add("  Fail Die: " + failDeiCount.ToString());
            string yield = totalDieCounts > 0 ? (passDieCounts * 100.0 / totalDieCounts).ToString("F2") + "%" : "0%";
            tmaLines.Add("  Yield: " + yield);
            tmaLines.Add("  Sample marking:");
            tmaLines.Add("");

            return tmaLines;
        }

        string Get(Dictionary<string, string> dict, string key) => dict.ContainsKey(key) ? dict[key] : "";
    }
}