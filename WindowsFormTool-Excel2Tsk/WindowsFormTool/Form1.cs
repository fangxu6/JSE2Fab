////using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniExcelLibs;
using WindowsFormTool.TskUtil;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DataToExcel
{

    public partial class Form1 : Form
    {
        public string ExcelFilePath;
        public string TSKFilePath;

        private List<string> firstFileList;
        private List<string> secondFileList;

        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 加载第一个文件
        /// 1. Excel文件
        /// 2. 待合并的TSK文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadFirstFile_Click(object sender, EventArgs e)
        {
            firstFileList = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo TheFolder = new DirectoryInfo(dialog.SelectedPath);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    firstFileList.Add(str.FullName);
                }
                button6.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 加载第二个文件
        /// 1. TSK文件
        /// 2. 待合并到的TSK文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSecondFile_Click(object sender, EventArgs e)
        {
            secondFileList = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo TheFolder = new DirectoryInfo(dialog.SelectedPath);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    secondFileList.Add(str.FullName);
                }
                button2.Text = dialog.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var processor = new TskProcessor();
            TskFileHelper.SavePath = SaveFileTo.Text.Trim();
            string newTskPath = TskFileHelper.SavePath;

            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0: // Excel数据 to TSK - 支持单个和批量
                        if (firstFileList != null && firstFileList.Count > 0)
                        {
                            // 批量处理
                            processor.ProcessBatch(firstFileList, secondFileList, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else if (!string.IsNullOrEmpty(ExcelFilePath) && !string.IsNullOrEmpty(TSKFilePath))
                        {
                            // 单文件处理
                            processor.ProcessSingle(ExcelFilePath, TSKFilePath, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        break;

                    case 1: // TSK合并 - 支持单个和批量
                        if (firstFileList != null && firstFileList.Count > 0)
                        {
                            // 批量处理
                            processor.ProcessBatch(firstFileList, secondFileList, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else if (!string.IsNullOrEmpty(ExcelFilePath) && !string.IsNullOrEmpty(TSKFilePath))
                        {
                            // 单文件处理
                            processor.ProcessSingle(ExcelFilePath, TSKFilePath, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        break;

                    case 2: // Excel图 to TSK - 支持单个和批量
                        if (firstFileList != null && firstFileList.Count > 0)
                        {
                            // 批量处理
                            processor.ProcessBatch(firstFileList, secondFileList, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else if (!string.IsNullOrEmpty(ExcelFilePath) && !string.IsNullOrEmpty(TSKFilePath))
                        {
                            // 单文件处理
                            processor.ProcessSingle(ExcelFilePath, TSKFilePath, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        break;

                    default:
                        MessageBox.Show(@"未选择处理方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                // 处理完成后询问是否打开文件夹
                if (MessageBox.Show(@"TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(newTskPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"处理过程中出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateRichTextBox($"错误：{ex.Message}\n");
            }
        }

        //更新RichTextBox
        private void UpdateRichTextBox(string message)
        {
            richTextBox1.AppendText(message);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();;
            Application.DoEvents();
        }

  

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 根据 ComboBox 的选择更改 Button 的文本
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    button6.Text = "XLSL文件路径";
                    button2.Text = "TSK空图谱文件路径";
                    button4.Text = "说明：\r\nxlsx或者csv文件的开始三列分别是x坐标、y坐标和binNo\r\n把对应数据的文件x坐标、y坐标和binNo分别黏贴到对应列\r\n\r\n";
                    break;
                case 1:
                    button6.Text = "待合并TSK 1（模板）";
                    button2.Text = "待合并到TSK 2";
                    button4.Text = "说明：\r\n将tsk1的fail合并到tsk2\r\n";
                    break;
                case 2:
                    button6.Text = "XLSL文件路径";
                    button2.Text = "待合并到TSK文件路径";
                    button4.Text = "说明：\r\n将XLSL的fail合并到TSK\r\n";
                    break;
                default:
                    button6.Text = "未定义的功能";
                    break;
            }
        }

    }

}
