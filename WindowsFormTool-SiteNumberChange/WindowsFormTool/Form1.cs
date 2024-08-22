using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DataToExcel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WindowsFormTool
{
    public partial class Form1 : Form
    {
        public string TSKFilePath;
        public ArrayList tskFileList = new ArrayList();
        string newTskFilePath;
        public Form1()
        {
            InitializeComponent();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "选择TSK所在文件夹";
                folderDialog.ShowNewFolderButton = false; // 不允许新建文件夹

                // 显示文件浏览对话框，并获取用户选择
                DialogResult result = folderDialog.ShowDialog();

                //确认后打开所选多个文件或者文件夹所在文件
                if (result == DialogResult.OK)
                {
                    this.button2.Text = folderDialog.SelectedPath;
                    string selectedFolderPath = folderDialog.SelectedPath;

                    // 获取文件夹中的所有文件
                    string[] files = Directory.GetFiles(selectedFolderPath);

                    // 打开每个文件
                    foreach (string file in files)
                    {
                        try
                        {
                            tskFileList.Add(file);
                            Process.Start(file);
                            UpdateRichTextBox(file + "\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"无法打开文件: {file}\n错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tskFileList.Count == 0)
            {
                MessageBox.Show("请先选择TSK文件", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (string file in tskFileList)
            {
                TSKFilePath = file;
                ToMapping();
            }

            if (MessageBox.Show("TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(Path.GetDirectoryName(newTskFilePath));
            }
        }

        private bool ToMapping()
        {
            if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 TSK图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string originSiteNumber = comboBox1.SelectedItem?.ToString();
            string newFailSiteNumber = comboBox2.SelectedItem?.ToString();

            if (newFailSiteNumber.Equals(originSiteNumber))
            {
                MessageBox.Show("site number 不能相同", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            UpdateRichTextBox("开始恢复TSK图谱\n");



            //------------TSK READ--------------------------------------------------//
            Tsk tsk = LoadTsk(TSKFilePath);
            UpdateRichTextBox("加载TSK图谱成功\n");
            //-------------------------------------------------------写TSK MAP--------------------------------------

            UpdateRichTextBox("开始生成新TSk图谱\n");
            newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);
            tsk.FullName = newTskFilePath;
            UpdateRichTextBox("生成新图谱路径：" + newTskFilePath + "\n");


            UpdateRichTextBox("开始相互交换：site nubmer " + originSiteNumber + "和site nubmer " + newFailSiteNumber + "\n");


            FileStream fw;
            //fw = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\" + SlotNo_1.ToString("000") + WaferID_1.TrimEnd('\0') + ".txt", FileMode.Create);
            fw = new FileStream(newTskFilePath + "-summary.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fw);
            sw.WriteLine("异常site 3明细：");
            /////--------------------Map版本为2，且无扩展信息TSK修改BIN信息代码-------------------////
            if (!tsk.ExtendFlag && ((Convert.ToInt32(tsk.MapVersion) == 2)))
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    //site =3 和site = 4的时候，对应的属性互相切换
                    if (tsk.DieMatrix[k].Site == 3)
                    {
                        DieData dieDataSite3 = tsk.DieMatrix[k];
                        DieData dieDataSite4 = tsk.DieMatrix[k + 1];
                        if ((dieDataSite3.Attribute.Equals(DieCategory.PassDie) && dieDataSite4.Attribute.Equals(DieCategory.FailDie)) || (dieDataSite3.Attribute.Equals(DieCategory.FailDie) && dieDataSite4.Attribute.Equals(DieCategory.PassDie)))
                        {
                            int tempBin = dieDataSite3.Bin;
                            dieDataSite3.Bin = dieDataSite4.Bin;
                            dieDataSite4.Bin = tempBin;
                            DieCategory tempAttribute = dieDataSite3.Attribute;
                            dieDataSite3.Attribute = dieDataSite4.Attribute;
                            dieDataSite4.Attribute = tempAttribute;
                            int tempSite = dieDataSite3.Site;
                            dieDataSite3.Site = dieDataSite4.Site;
                            dieDataSite4.Site = tempSite;
                            sw.WriteLine(tsk.DieMatrix[k + 1].ToString());
                            sw.WriteLine(tsk.DieMatrix[k].ToString());

                            sw.WriteLine();
                        }
                        k++;
                    }

                }
            }

            /////--------------------Map版本为2，且有扩展信息TSK修改BIN信息代码-------------------////
            if (tsk.ExtendFlag)
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {

                    if (tsk.DieMatrix[k].Site == 3)
                    {
                        DieData dieDataSite3 = tsk.DieMatrix[k];
                        DieData dieDataSite4 = tsk.DieMatrix[k + 1];
                        if ((dieDataSite3.Attribute.Equals(DieCategory.PassDie) && dieDataSite4.Attribute.Equals(DieCategory.FailDie)) || (dieDataSite3.Attribute.Equals(DieCategory.FailDie) && dieDataSite4.Attribute.Equals(DieCategory.PassDie)))
                        {
                            int tempBin = dieDataSite3.Bin;
                            dieDataSite3.Bin = dieDataSite4.Bin;
                            dieDataSite4.Bin = tempBin;
                            DieCategory tempAttribute = dieDataSite3.Attribute;
                            dieDataSite3.Attribute = dieDataSite4.Attribute;
                            dieDataSite4.Attribute = tempAttribute;
                            int tempSite = dieDataSite3.Site;
                            dieDataSite3.Site = dieDataSite4.Site;
                            dieDataSite4.Site = tempSite;
                            sw.WriteLine(tsk.DieMatrix[k + 1].ToString());
                            sw.WriteLine(tsk.DieMatrix[k].ToString());

                            sw.WriteLine();
                        }
                        k++;

                    }


                }
            }
            sw.Close();
            fw.Close();

            //----------------------------TSK修改BIN信息-----------------------------------------------------
            tsk.Save(); //只有基本信息









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
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
            Application.DoEvents();
        }
    }
}
