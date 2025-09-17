using DataToExcel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button5_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {


                string TskFile = dialog.SelectedPath;


                DirectoryInfo TheFolder = new DirectoryInfo(TskFile);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    ToRotating(str);
                }
            }

           
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    ToRotating();
        //}

        private void ToRotating(FileInfo txtFile)
        {
            if (txtFile == null || txtFile.Length == 0)
            {
                MessageBox.Show("请先选择 TXT空图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateRichTextBox("开始旋转TXT图谱\n");

            try
            {
                // 1. 配置文件路径（可根据实际情况修改）
                string inputFilePath = txtFile.FullName; // 原始文件路径
                string outputFilePath = @"D:\New-TXT\" + txtFile.Name; // 旋转后输出路径

                // 2. 读取原始文件内容，过滤空行（避免旋转后出现无效空行）
                List<string> originalLines = ReadNonEmptyLines(inputFilePath);
                if (originalLines.Count == 0)
                {
                    UpdateRichTextBox("错误：原始文件无有效内容（或文件不存在）！");
                    return;
                }

                // 3. 处理行长度一致性（以最长行为基准，短行补空格，确保旋转后格式整齐）
                int maxLineLength = originalLines.Max(line => line.Length);
                List<string> normalizedLines = NormalizeLineLengths(originalLines, maxLineLength);

                // 4. 执行90度顺时针旋转
                List<string> rotatedLines = Rotate90DegreesClockwise(normalizedLines, maxLineLength);

                // 5. 保存旋转后的内容到新文件
                SaveRotatedContent(rotatedLines, outputFilePath);

                // 6. 提示操作成功
                UpdateRichTextBox($"文件旋转完成！");
                UpdateRichTextBox($"原始文件：{inputFilePath}");
                UpdateRichTextBox($"输出文件：{outputFilePath}");
            }
            catch (Exception ex)
            {
                // 捕获并显示异常信息（如文件读写权限不足、路径错误等）
                UpdateRichTextBox($"程序执行出错：{ex.Message}");
            }

        }


        static void Rotete(string[] args)
        {
            
        }

        /// <summary>
        /// 读取文件中所有非空行
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>非空行列表</returns>
        private static List<string> ReadNonEmptyLines(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("指定的原始文件不存在", filePath);
            }

            // 读取所有行，并过滤掉空行（包括仅含空格的行）
            return File.ReadAllLines(filePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToList();
        }

        /// <summary>
        /// 标准化所有行的长度（短行末尾补空格，与最长行长度一致）
        /// </summary>
        /// <param name="lines">原始行列表</param>
        /// <param name="maxLength">最长行长度</param>
        /// <returns>长度统一的行列表</returns>
        private static List<string> NormalizeLineLengths(List<string> lines, int maxLength)
        {
            List<string> normalized = new List<string>();
            foreach (string line in lines)
            {
                // 若行长度不足，末尾补空格；若超出（理论上不会，因maxLength是最大值），则截断
                string normalizedLine = line.PadRight(maxLength).Substring(0, maxLength);
                normalized.Add(normalizedLine);
            }
            return normalized;
        }

        /// <summary>
        /// 将文本内容按顺时针方向旋转90度
        /// 原理：原始矩阵（行x列）转置后，反转每一行的顺序
        /// </summary>
        /// <param name="normalizedLines">长度统一的原始行列表</param>
        /// <param name="maxLineLength">每行长度（列数）</param>
        /// <returns>旋转后的行列表</returns>
        private static List<string> Rotate90DegreesClockwise(List<string> normalizedLines, int maxLineLength)
        {
            List<string> rotated = new List<string>();
            int rowCount = normalizedLines.Count; // 原始行数（旋转后变为列数）

            // 遍历原始矩阵的列（旋转后变为行）
            for (int col = 0; col < maxLineLength; col++)
            {
                char[] rotatedRow = new char[rowCount];
                // 遍历原始矩阵的行（从最后一行到第一行，实现反转）
                for (int row = 0; row < rowCount; row++)
                {
                    // 原始位置 (row, col) → 旋转后位置 (col, rowCount - 1 - row)
                    rotatedRow[row] = normalizedLines[rowCount - 1 - row][col];
                }
                rotated.Add(new string(rotatedRow));
            }

            return rotated;
        }

        /// <summary>
        /// 将旋转后的内容保存到文件
        /// </summary>
        /// <param name="rotatedLines">旋转后的行列表</param>
        /// <param name="outputFilePath">输出文件路径</param>
        private static void SaveRotatedContent(List<string> rotatedLines, string outputFilePath)
        {
            // 若输出目录不存在，自动创建
            string outputDir = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // 写入文件（使用UTF-8编码，避免中文等特殊字符乱码）
            File.WriteAllLines(outputFilePath, rotatedLines, System.Text.Encoding.UTF8);
        }


        //更新RichTextBox
        private void UpdateRichTextBox(string message)
        {
            richTextBox1.Text += message;
            Application.DoEvents();
        }
    }
}
