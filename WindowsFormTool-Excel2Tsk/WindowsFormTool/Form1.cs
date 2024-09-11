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
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "选择 Excel 文件";
                FileDialog.RestoreDirectory = true; // 记住上次打开的目录

                // 显示文件浏览对话框，并获取用户选择
                DialogResult result = FileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ExcelFilePath = FileDialog.FileName;
                    button6.Text = ExcelFilePath;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "选择 TSK 空图谱文件";
                FileDialog.RestoreDirectory = true; // 记住上次打开的目录

                // 显示文件浏览对话框，并获取用户选择
                DialogResult result = FileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    TSKFilePath = FileDialog.FileName;
                    button2.Text = TSKFilePath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToMapping();
        }

        private bool ToMapping()
        {
            if (string.IsNullOrWhiteSpace(ExcelFilePath))
            {
                MessageBox.Show("请先选择 Excel文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 TSK初始图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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

                var die = tsk.DieMatrix[k];
                if (binNoMap.TryGetValue((die.X, die.Y), out int binNo))
                {
                    die.Bin = binNo;
                    die.Attribute = binNo == 1 ? DieCategory.PassDie : DieCategory.FailDie;
                }
            }


            UpdateRichTextBox("开始生成新TSK图谱\n");
            tsk.FullName = newTskFilePath;
            tsk.Save();

            if (MessageBox.Show("TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(Path.GetDirectoryName(newTskFilePath));
            }

            return true;
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
    }

}
