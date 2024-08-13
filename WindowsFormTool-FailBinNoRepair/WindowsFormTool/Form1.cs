using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DataToExcel;

namespace WindowsFormTool
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
            if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 TSK空图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string originFailBinNo = comboBox1.SelectedItem?.ToString();
            string newFailBinNo = comboBox2.SelectedItem?.ToString();

            if (newFailBinNo.Equals(originFailBinNo))
            {
                MessageBox.Show("新Fail Bin No 与 原Fail Bin No 不能相同", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            UpdateRichTextBox("开始恢复TSK图谱\n");


            
            //------------TSK READ--------------------------------------------------//
            Tsk tsk = LoadTsk(TSKFilePath);
            UpdateRichTextBox("打开TSk空图谱成功\n");
            //-------------------------------------------------------写TSK MAP--------------------------------------

            UpdateRichTextBox("开始生产新TSk图谱\n");
            string newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);
            tsk.FullName = newTskFilePath;
            UpdateRichTextBox("生成图谱路径：" + newTskFilePath + "\n");


            UpdateRichTextBox("原Fail Bin No ：" + comboBox1.SelectedItem?.ToString() + "更改为新Fail Bin No ：" + newFailBinNo + "\n");

            /////--------------------Map版本为2，且无扩展信息TSK修改BIN信息代码-------------------////
            if (!tsk.ExtendFlag && ((Convert.ToInt32(tsk.MapVersion) == 2)))
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    if (tsk.DieMatrix[k].Attribute.Equals(DieCategory.FailDie) && tsk.DieMatrix[k].Bin.Equals(originFailBinNo))
                    {
                        tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                        tsk.DieMatrix[k].Bin = Convert.ToInt32(newFailBinNo);
                    }

                }
            }

            /////--------------------Map版本为2，且有扩展信息TSK修改BIN信息代码-------------------////
            if (tsk.ExtendFlag)
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    //if (Convert.ToInt32(tsk.MapVersion) == 2)
                    //{

                        if (tsk.DieMatrix[k].Attribute.Equals(DieCategory.FailDie) && tsk.DieMatrix[k].Bin.Equals(Convert.ToInt32(originFailBinNo)))
                        {
                            tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                            tsk.DieMatrix[k].Bin = Convert.ToInt32(newFailBinNo);
                        }

                    //}
                    //else if (Convert.ToInt32(tsk.MapVersion) == 4)
                    //{
                    //    if (tsk.DieMatrix[k].Attribute.Equals(DieCategory.FailDie) && tsk.DieMatrix[k].Bin.Equals(originFailBinNo))
                    //    {
                    //        tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                    //        tsk.DieMatrix[k].Bin = Convert.ToInt32(newFailBinNo);
                    //    }
                    //}
                }
            }


            /////--------------------Map版本为4，且有扩展信息TSK修改BIN信息代码-------------------////



            //----------------------------TSK修改BIN信息-----------------------------------------------------
            tsk.Save(); //只有基本信息


            if (MessageBox.Show("TSk新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(Path.GetDirectoryName(newTskFilePath));
            }

            return true;
        }

        private Tsk LoadTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read(); //版本2和4的拓展还是没有体现进binNo
            //this.LotNo = tsk.LotNo.Trim();
            return tsk;
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
    }
}
