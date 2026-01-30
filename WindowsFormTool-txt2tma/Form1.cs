using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace txt2tma
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "TXT文件|*.txt|所有文件|*.*";
            dialog.Title = "选择txt文件";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = dialog.FileName;
                Log("已选择文件: " + dialog.FileName);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择包含txt文件的文件夹";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = dialog.SelectedPath;
                Log("已选择文件夹: " + dialog.SelectedPath);
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            string path = txtFilePath.Text.Trim();

            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请先选择文件或文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (File.Exists(path))
            {
                ConvertSingleFile(path);
            }
            else if (Directory.Exists(path))
            {
                ConvertFolder(path);
            }
            else
            {
                MessageBox.Show("路径不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBatchConvert_Click(object sender, EventArgs e)
        {
            string path = txtFilePath.Text.Trim();

            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请先选择文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(path))
            {
                MessageBox.Show("文件夹不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConvertFolder(path);
        }

        private void ConvertSingleFile(string txtPath)
        {
            try
            {
                Log("开始转换: " + txtPath);

                var (waferInfo, dataRows, rowCount, colCount) = ParseTxt(txtPath);

                if (dataRows.Count == 0)
                {
                    MessageBox.Show("未找到RowData数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string tmaPath = Path.ChangeExtension(txtPath, ".tma");
                List<string> tmaLines = ConvertToTma(waferInfo, dataRows, rowCount, colCount);

                File.WriteAllLines(tmaPath, tmaLines, Encoding.UTF8);

                Log("转换完成: " + tmaPath);
                lblStatus.Text = "转换完成";
            }
            catch (Exception ex)
            {
                Log("转换失败: " + ex.Message);
                MessageBox.Show("转换失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConvertFolder(string folderPath)
        {
            try
            {
                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

                if (txtFiles.Length == 0)
                {
                    MessageBox.Show("文件夹中没有txt文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                progressBar.Maximum = txtFiles.Length;
                progressBar.Value = 0;
                int successCount = 0;

                Log("找到 " + txtFiles.Length + " 个txt文件");

                foreach (string txtPath in txtFiles)
                {
                    try
                    {
                        var (waferInfo, dataRows, rowCount, colCount) = ParseTxt(txtPath);

                        if (dataRows.Count > 0)
                        {
                            string tmaPath = Path.ChangeExtension(txtPath, ".tma");
                            List<string> tmaLines = ConvertToTma(waferInfo, dataRows, rowCount, colCount);
                            File.WriteAllLines(tmaPath, tmaLines, Encoding.UTF8);
                            Log("转换成功: " + Path.GetFileName(txtPath));
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("转换失败 " + Path.GetFileName(txtPath) + ": " + ex.Message);
                    }

                    progressBar.Value++;
                    Application.DoEvents();
                }

                lblStatus.Text = string.Format("完成: {0}/{1}", successCount, txtFiles.Length);
                Log("批量转换完成，成功 " + successCount + " 个文件");
                MessageBox.Show(string.Format("转换完成！\n成功: {0}/{1}", successCount, txtFiles.Length),
                    "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("批量转换失败: " + ex.Message);
                MessageBox.Show("批量转换失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (Dictionary<string, string> waferInfo, List<string[]> dataRows, int rowCount, int colCount) ParseTxt(string txtPath)
        {
            Dictionary<string, string> waferInfo = new Dictionary<string, string>();
            List<string[]> dataRows = new List<string[]>();
            int rowCount = 0;
            int colCount = 0;

            foreach (string line in File.ReadAllLines(txtPath, Encoding.UTF8))
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                if (trimmedLine.StartsWith("RowData:"))
                {
                    // 解析数据行
                    string dataPart = trimmedLine.Substring(8).Trim();
                    string[] parts = dataPart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> rowData = new List<string>();

                    foreach (string part in parts)
                    {
                        if (part == "__")
                            rowData.Add("__");
                        else
                            rowData.Add(part);
                    }

                    dataRows.Add(rowData.ToArray());

                    if (colCount == 0)
                        colCount = rowData.Count;
                }
                else
                {
                    // 解析wafer基本信息
                    int colonIndex = trimmedLine.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        string key = trimmedLine.Substring(0, colonIndex).Trim();
                        string value = trimmedLine.Substring(colonIndex + 1).Trim();
                        waferInfo[key] = value;
                    }

                    // 从ROWCT和COLCT获取行列数
                    if (trimmedLine.StartsWith("ROWCT:"))
                    {
                        if (int.TryParse(trimmedLine.Substring(6).Trim(), out int rc))
                            rowCount = rc;
                    }
                    if (trimmedLine.StartsWith("COLCT:"))
                    {
                        if (int.TryParse(trimmedLine.Substring(6).Trim(), out int cc))
                            colCount = cc;
                    }
                }
            }

            return (waferInfo, dataRows, rowCount, colCount);
        }

        private List<string> ConvertToTma(Dictionary<string, string> waferInfo, List<string[]> dataRows, int rowCount, int colCount)
        {
            List<string> tmaLines = new List<string>();

            // 根据行列数确定坐标位数
            int yDigits = rowCount >= 100 ? 3 : 2;
            int xDigits = colCount >= 100 ? 3 : 2;

            // 生成x坐标行（列号）
            StringBuilder xCoords = new StringBuilder();
            xCoords.Append(' ', yDigits + 1); // 第一行的空格数等于y坐标的位数+1

            for (int i = 1; i <= colCount; i++)
            {
                xCoords.Append(i.ToString().PadLeft(xDigits));
            }
            tmaLines.Add(xCoords.ToString());

            // 生成分隔线
            // 第二行的空格数等于y坐标的位数，然后补一个+号
            // 每个x坐标对应"-+-+"格式
            StringBuilder separator = new StringBuilder();
            separator.Append(' ', yDigits);
            separator.Append('+');
            for (int i = 0; i < colCount; i++)
            {
                separator.Append("-+-");
            }
            separator.Append('+');
            tmaLines.Add(separator.ToString());

            // 生成数据行
            for (int y = 0; y < dataRows.Count; y++)
            {
                string[] row = dataRows[y];
                bool isLastRow = (y == dataRows.Count - 1);

                StringBuilder line = new StringBuilder();
                line.Append((y + 1).ToString().PadLeft(yDigits, '0'));
                line.Append('|');

                foreach (string cell in row)
                {
                    if (cell == "__")
                    {
                        line.Append(isLastRow ? "  M" : "  .");
                    }
                    else if (cell == "01")
                    {
                        line.Append("  P");
                    }
                    else
                    {
                        line.Append("  F");
                    }
                }
                tmaLines.Add(line.ToString());
            }

            // 生成wafer基本信息
            tmaLines.Add("");
            tmaLines.Add("============ Wafer Information () ===========");

            // Device
            tmaLines.Add("  Device: " + GetValue(waferInfo, "DEVICE"));

            // Lot NO
            tmaLines.Add("  Lot NO: " + GetValue(waferInfo, "Lot"));

            // Slot NO
            tmaLines.Add("  Slot NO: ");

            // Wafer ID
            tmaLines.Add("  Wafer ID: " + GetValue(waferInfo, "Wafer"));

            // Operater
            tmaLines.Add("  Operater: ");

            // Wafer Size
            tmaLines.Add("  Wafer Size: ");

            // Flat Dir
            tmaLines.Add("  Flat Dir: 180");

            // Wafer Test Start Time
            tmaLines.Add("  Wafer Test Start Time: ");

            // Wafer Test Finish Time
            tmaLines.Add("  Wafer Test Finish Time: ");

            // Wafer Load Time
            tmaLines.Add("  Wafer Load Time: ");

            // Wafer Unload Time
            tmaLines.Add("  Wafer Unload Time: ");

            // Total test die
            tmaLines.Add("  Total test die: " + GetValue(waferInfo, "Total Tested"));

            // Pass Die
            string totalPass = GetValue(waferInfo, "Total Pass");
            tmaLines.Add("  Pass Die: " + totalPass);

            // Fail Die
            string totalTested = GetValue(waferInfo, "Total Tested");
            int failDie = 0;
            if (int.TryParse(totalTested, out int totalT) && int.TryParse(totalPass, out int totalP))
            {
                failDie = totalT - totalP;
            }
            tmaLines.Add("  Fail Die: " + failDie);

            // Yield
            if (int.TryParse(totalTested, out totalT) && totalT > 0)
            {
                double yieldPct = (double)failDie / totalT * 100;
                tmaLines.Add("  Yield: " + yieldPct.ToString("F1") + "%");
            }
            else
            {
                tmaLines.Add("  Yield: 0%");
            }

            // Sample marking
            tmaLines.Add("  Sample marking:");
            tmaLines.Add("");

            return tmaLines;
        }

        private string GetValue(Dictionary<string, string> dict, string key)
        {
            return dict.ContainsKey(key) ? dict[key] : "";
        }

        private void Log(string message)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText("[" + time + "] " + message + Environment.NewLine);
        }
    }
}
