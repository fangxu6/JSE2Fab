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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static DataToExcel.DieMatrix;

namespace DataToExcel
{
    public partial class Form1 : Form
    {
        public string TSKFilePath;
        public Form1()
        {
            InitializeComponent();
        }

        

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "选择 TSK 图谱文件";
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


        private void ToMapping()
        {
            
            if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 TSK初始图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ///////-------------------------------TSK读取-------------------------//////
            UpdateRichTextBox("开始解析初始TSK图谱\n");
            Tsk tsk = LoadTsk(TSKFilePath);
            UpdateRichTextBox("解析TSK图谱结束\n");

            DieMatrix moveDieMatrix = tsk.DieMatrix.Clone();
            //开始移动moveDieMatrix
            UpdateRichTextBox("开始移动DieMatrix\n");
            int moveX = Int32.Parse(comboBox1.Text);
            int moveY = Int32.Parse(comboBox2.Text);

            OffsetDir offsetDir = OffsetDir.X;
            moveDieMatrix.Offset(offsetDir,moveX);
            offsetDir = OffsetDir.Y;
            moveDieMatrix.Offset(offsetDir, moveY);

            tsk.DieMatrix = tsk.DieMatrix + moveDieMatrix;

            UpdateRichTextBox("开始写入新TSK图谱\n");
            string newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);
            UpdateRichTextBox("生成图谱路径" + newTskFilePath + "\n");


            this.progressBar1.Maximum = tsk.Rows * tsk.Cols;
            this.progressBar1.Value = 0;



            for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
            {
                this.progressBar1.Value++;

                
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
    }

}
