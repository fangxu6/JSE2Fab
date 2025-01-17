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

            if (comboBox1.SelectedIndex == 0)
                ToMapping();
            else if (comboBox1.SelectedIndex == 1)
            {
                for (int i = 0; i < firstFileList.Capacity; i++)
                {
                    ExcelFilePath = firstFileList[i];
                    TSKFilePath = secondFileList[i];
                    MergeTsk();
                }
            }
            if (MessageBox.Show("TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);//TODO 两个地方都写了
                Process.Start(Path.GetDirectoryName(newTskFilePath));
            }
        }

        private void MergeTsk()
        {
            if (string.IsNullOrWhiteSpace(ExcelFilePath))
            {
                MessageBox.Show("请先选择 TSK初始图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 待合并TSK图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateRichTextBox("开始合并TSK图谱\n");
            UpdateRichTextBox("开始解析TSK原始图谱\n");
            Tsk originalTsk = LoadTsk(ExcelFilePath);
            UpdateRichTextBox("解析初始TSK空图谱结束\n");
            UpdateRichTextBox("开始解析TSK原始图谱\n");
            Tsk mergeTsk = LoadTsk(TSKFilePath);
            UpdateRichTextBox("解析待合并TSK图谱结束\n");
            //TSK比对，以防不能合并
            if (originalTsk.Rows != mergeTsk.Rows || originalTsk.Cols != mergeTsk.Cols)
            {
                MessageBox.Show("TSK图谱尺寸不一致，无法合并", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //originalTsk.LotNo替换CP1或者CP2或者CP3为空格
            string originalLotNo = Regex.Replace(originalTsk.LotNo, "CP[1-3]", "").Trim();
            string mergeLotNo = Regex.Replace(mergeTsk.LotNo, "CP[1-3]", "").Trim();
            if (originalLotNo != mergeLotNo || originalTsk.SlotNo != mergeTsk.SlotNo)
            {
                MessageBox.Show("TSK图谱WaferID不一致，无法合并", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            UpdateRichTextBox("开始合并TSK图谱\n");
            string newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);
            UpdateRichTextBox("生成图谱路径" + newTskFilePath + "\n");

            mergeTsk.Merge(originalTsk, newTskFilePath);

        }

        private void ToMapping()
        {
            if (string.IsNullOrWhiteSpace(ExcelFilePath))
            {
                MessageBox.Show("请先选择 Excel文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 TSK初始图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateRichTextBox("开始恢复TSK图谱\n");
            DataTable table = MiniExcel.QueryAsDataTable(ExcelFilePath, useHeaderRow: false);

            UpdateRichTextBox("解析Excel信息成功\n");
            ///////-------------------------------TSK读取-------------------------//////
            UpdateRichTextBox("开始解析初始TSK图谱\n");
            Tsk tsk = LoadTsk(TSKFilePath);
            UpdateRichTextBox("解析初始TSK空图谱结束\n");


            UpdateRichTextBox("开始写入TSK空图谱\n");
            string newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);
            UpdateRichTextBox("生成图谱路径" + newTskFilePath + "\n");


            this.progressBar1.Maximum = tsk.Rows * tsk.Cols;
            this.progressBar1.Value = 0;


            // 创建一个字典来加速查找
            var binNoMap = new Dictionary<(int, int), int>();
            foreach (DataRow row in table.Rows)
            {
                if (row[0] is DBNull || row[1] is DBNull || row[2] is DBNull)//Excel跳过空行
                    continue;
                if (row[0] != null && row[1] != null && row[2] != null)
                {
                    int x = Convert.ToInt32(row[0]);
                    int y = Convert.ToInt32(row[1]);
                    int binNo = Convert.ToInt32(row[2]);
                    binNoMap[(x, y)] = binNo;
                }
            }

            for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
            {
                this.progressBar1.Value++;

                DieData die = tsk.DieMatrix[k];
                if (binNoMap.TryGetValue((die.X, die.Y), out int binNo))
                {
                    die.Bin = binNo;
                    die.Attribute = binNo == 1 ? DieCategory.PassDie : DieCategory.FailDie;
                }
            }

            tsk.PassDie = 0;
            tsk.FailDie = 0;
            for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
            {
                if (tsk.DieMatrix[k].Attribute == DieCategory.PassDie)
                {
                    tsk.PassDie++;
                }
                else if (tsk.DieMatrix[k].Attribute == DieCategory.FailDie)
                {
                    tsk.FailDie++;
                }
            }
            tsk.TotalDie = tsk.PassDie + tsk.FailDie;


            UpdateRichTextBox("开始生成新TSK图谱\n");
            tsk.FullName = newTskFilePath;
            tsk.Save();

            if (MessageBox.Show("TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(Path.GetDirectoryName(newTskFilePath));
            }
        }
        private void Reverse(ref byte[] target)
        {
            int n1 = 0, n2 = target.Length - 1;
            byte temp;
            while (n1 < n2)
            {
                temp = target[n1];
                target[n1] = target[n2];
                target[n2] = temp;

                n1++;
                n2--;
            }
        }
        private short ByteToInt16(ref byte[] target)
        {
            this.Reverse(ref target);
            return BitConverter.ToInt16(target, 0);
        }

        private int ByteToInt32(ref byte[] target)
        {
            this.Reverse(ref target);
            return BitConverter.ToInt32(target, 0);
        }
        //更新RichTextBox
        private void UpdateRichTextBox(string message)
        {
            richTextBox1.Text += message;
            Application.DoEvents();
        }

        private Tsk LoadTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read();
            //this.LotNo = tsk.LotNo.Trim();
            return tsk;
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
                default:
                    button6.Text = "未定义的功能";
                    break;
            }
        }

    }

}
