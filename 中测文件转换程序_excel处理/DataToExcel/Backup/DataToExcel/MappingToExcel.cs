using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using System.Reflection;
using System.Collections;
using System.Net.Mail;

//using Jcap.MappingConverter;

namespace DataToExcel
{
    public partial class MappingToExcel : Form
    {
        // Fields
        private IMappingFile _currFile;
        private FieldsProp Field;
        private ArrayList FieldsArray;
        private string FilePath = Application.StartupPath;
        private string LotNo;
        private string ResultFileName;
        private string TskFile;
        private string Device;
        private int count;

        // Methods
        public MappingToExcel()
        {
            this.InitializeComponent();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (this.lsvItems.Items.Count <= 0)
            {
                MessageBox.Show("There is no TSK file!Please Load TSK files first!");
            }
            else
            {
                if (!Directory.Exists(this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo))
                {
                    Directory.CreateDirectory(this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo);
                }
                else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                this.progressBar1.Maximum = this.lsvItems.Items.Count;
                this.progressBar1.Value = 0;
                this.ExpDataToExcel();
                if (MessageBox.Show("Export EXCEL File Success!Would you like to open it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(this.ResultFileName);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.lsvItems.Items.Count <= 0)
            {
                MessageBox.Show("There is no TSK file!Please Load TSK files first!");
            }
            else
            {
                int num2;
                StreamWriter writer;
                if (!Directory.Exists(this.textBox1.Text + @"\TxtOutFile\" + this.LotNo))
                {
                    Directory.CreateDirectory(this.textBox1.Text + @"\TxtOutFile\" + this.LotNo);
                }
                else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                int count = this.lsvItems.Items.Count;
                this.progressBar1.Maximum = count;
                this.progressBar1.Value = 0;
                ToCountDie._ToCountDie = new Hashtable();
                CmdTxt.InitTotal();
                for (num2 = 0; num2 <= (count - 1); num2++)
                {
                    new CMDTskToTxt().Convert(this.lsvItems.Items[num2].SubItems[1].Text.Trim(), this.textBox1.Text + @"\TxtOutFile\" + this.LotNo + @"\" + this.lsvItems.Items[num2].Text.Trim() + ".txt");
                    this.progressBar1.Value++;
                }
                string path = this.textBox1.Text + @"\TxtOutFile\" + this.LotNo + @"\Total.txt"; //建立的Total.txt
                if (File.Exists(path))
                {
                    writer = File.AppendText(path);
                }
                else
                {
                    writer = File.CreateText(path);
                }
                writer.WriteLine("============ Total Wafer Information () ===========");
                writer.WriteLine("  Device: " + CmdTxt._Device);
                writer.WriteLine("  Lot NO: " + CmdTxt._LotNo);
                writer.WriteLine("  Total Die: " + CmdTxt._TotalDie);
                writer.WriteLine("  Total Pass Die: " + CmdTxt._TotalPassDie);
                writer.WriteLine("  Total Fail Die: " + CmdTxt._TotalFailDie);
                writer.WriteLine("  Total Yield: " + CmdTxt._TotalYield);
                writer.WriteLine("=============================================");
                int num3 = this.FieldListBox1.CheckedItems.Count;
                int num4 = 0;
                for (num2 = 0; num2 <= (num3 - 1); num2++)
                {
                    string s = this.FieldListBox1.CheckedItems[num2].ToString().Trim();
                    if (s.Substring(0, 3) == "BIN")
                    {
                        int num5;
                        string[] strArray;
                        s = s.Substring(s.LastIndexOf(" ")).Trim();
                        if (ToCountDie._ToCountDie[int.Parse(s)] != null)
                        {
                            num5 = Convert.ToInt32(ToCountDie._ToCountDie[int.Parse(s)]);
                        }
                        else
                        {
                            num5 = 0;
                        }
                        if (s != "1")
                        {
                            strArray = new string[5];
                            strArray[0] = this.FieldListBox1.CheckedItems[num2].ToString().Trim();
                            strArray[1] = "   ";
                            strArray[2] = num5.ToString("00000");
                            strArray[3] = "   ";
                            double num6 = Convert.ToDouble(num5) / ((double)CmdTxt._TotalDie);
                            strArray[4] = num6.ToString("0.00%");
                            writer.WriteLine(string.Concat(strArray));
                        }
                        else
                        {
                            strArray = new string[] { this.FieldListBox1.CheckedItems[num2].ToString().Trim(), "   ", CmdTxt._TotalPassDie.ToString("00000"), "   ", (Convert.ToDouble(CmdTxt._TotalPassDie) / ((double)CmdTxt._TotalDie)).ToString("0.00%") };
                            writer.WriteLine(string.Concat(strArray));
                        }
                        num4++;
                    }
                }
                writer.Close();
                if (MessageBox.Show("Export TXT File Success!Would you like to open it?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(this.textBox1.Text + @"\TxtOutFile\" + this.LotNo + @"\");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.SelectedPath;
                this.SavePath();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.lsvItems.Items.Count <= 0)
            {
                MessageBox.Show("There is no TSK file!Please Load TSK files first!");
            }
            else
            {
                int num2;
                StreamWriter writer;

                string outpath = this.textBox1.Text + @"\TmaOutFile\" + this.LotNo;

                if (!Directory.Exists(outpath))
                {
                    Directory.CreateDirectory(outpath);
                }
                else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                int count = this.lsvItems.Items.Count;
                this.progressBar1.Maximum = count;
                this.progressBar1.Value = 0;
                ToCountDie._ToCountDie = new Hashtable();
                CmdTxt.InitTotal();

                string source = "";
                TskToTma converter = new TskToTma();
                Tma tma = null;

                for (num2 = 0; num2 <= (count - 1); num2++)
                {
                    // 来源 tsk 文件
                    source = this.lsvItems.Items[num2].SubItems[1].Text.Trim();

                    // 截取文件名
                    string str = source.Substring(source.LastIndexOf(@"\") + 1).Substring(1).Replace(".", "");

                    // 执行文件格式转换
                    converter.Convert(source, outpath + @"\" + str + "_1.tma");

                    // 读取来源文件
                    tma = new Tma(outpath + @"\" + str + "_1.tma");
                    tma.Read();

                    // 去空白行和空白列
                    this.Trim(tma);

                    // 平边向下
                    tma.DeasilRotate(180 - Int32.Parse(tma.FlatDir));
                    tma.FlatDir = "180";

                    // 写平边标记
                    this.MarkNouch(tma);

                    // 构建文件名，保存
                    tma.FileName = str + ".tma";
                    tma.Save();

                    File.Delete(outpath + @"\" + str + "_1.tma");

                    // 修改进度条
                    this.progressBar1.Value++;
                }

                string path = this.textBox1.Text + @"\TmaOutFile\" + this.LotNo + @"\Total.txt";
                if (File.Exists(path))
                {
                    writer = File.AppendText(path);
                }
                else
                {
                    writer = File.CreateText(path);
                }

                writer.WriteLine("============ Total Wafer Information () ===========");
                writer.WriteLine("  Device: " + CmdTxt._Device);
                writer.WriteLine("  Lot NO: " + CmdTxt._LotNo);
                writer.WriteLine("  Total Die: " + CmdTxt._TotalDie);
                writer.WriteLine("  Total Pass Die: " + CmdTxt._TotalPassDie);
                writer.WriteLine("  Total Fail Die: " + CmdTxt._TotalFailDie);
                writer.WriteLine("  Total Yield: " + CmdTxt._TotalYield);
                writer.WriteLine("=============================================");

                  int  num3 = this.FieldListBox1.CheckedItems.Count;
                int num4 = 0;
                for (num2 = 0; num2 <= (num3 - 1); num2++)
                {
                    string s = this.FieldListBox1.CheckedItems[num2].ToString().Trim();
                    if (s.Substring(0, 3) == "BIN")
                    {
                        int num5;
                        string[] strArray;
                        s = s.Substring(s.LastIndexOf(" ")).Trim();
                        if (ToCountDie._ToCountDie[int.Parse(s)] != null)
                        {
                            num5 = Convert.ToInt32(ToCountDie._ToCountDie[int.Parse(s)]);
                        }
                        else
                        {
                            num5 = 0;
                        }
                        if (s != "1")
                        {
                            strArray = new string[5];
                            strArray[0] = this.FieldListBox1.CheckedItems[num2].ToString().Trim();
                            strArray[1] = "   ";
                            strArray[2] = num5.ToString("00000");
                            strArray[3] = "   ";
                            double num6 = Convert.ToDouble(num5) / ((double)CmdTxt._TotalDie);
                            strArray[4] = num6.ToString("0.00%");
                            writer.WriteLine(string.Concat(strArray));
                        }
                        else
                        {
                            strArray = new string[] { this.FieldListBox1.CheckedItems[num2].ToString().Trim(), "   ", CmdTxt._TotalPassDie.ToString("00000"), "   ", (Convert.ToDouble(CmdTxt._TotalPassDie) / ((double)CmdTxt._TotalDie)).ToString("0.00%") };
                            writer.WriteLine(string.Concat(strArray));
                        }
                        num4++;
                    }
                }

                writer.Close();

                if (MessageBox.Show("Export TMA File Success!Would you like to open it?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(this.textBox1.Text + @"\TmaOutFile\" + this.LotNo + @"\");
                }
            }
        }

        // 去除空白行或空白列
        private void Trim(IMappingFile mapping)
        {
            // 去除图谱左边的空白
            this.TrimL(mapping);

            // 去除图谱上边的空白
            this.TrimU(mapping);

            // 去除图谱右边的空白
            this.TrimR(mapping);

            // 去除图谱下边的空白
            this.TrimD(mapping);
        }

        // 获取 mapping 矩阵左侧空白行
        private int TrimL(IMappingFile mapping)
        {
            int cnt = 0;

            for (int i = 0; i < mapping.DieMatrix.XMax; i++)
            {
                for (int j = 0; j < mapping.DieMatrix.YMax; j++)
                {
                    if (!this.IsEmptyDie(mapping.DieMatrix[i, j]))
                    {
                        cnt = i;
                        goto trimFlag;
                    }
                }
            }

            cnt = mapping.DieMatrix.XMax;

        trimFlag:
            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Left, cnt);

            return cnt;
        }

        // 获取 mapping 矩阵右侧空白行
        private int TrimR(IMappingFile mapping)
        {
            int cnt = 0;

            int x = mapping.DieMatrix.XMax - 1;
            int y = mapping.DieMatrix.YMax - 1;

            for (int i = x; i >= 0; i--)
            {
                for (int j = y; j >= 0; j--)
                {
                    if (!this.IsEmptyDie(mapping.DieMatrix[i, j]))
                    {
                        cnt = mapping.DieMatrix.XMax - i - 1;
                        goto trimFlag;
                    }
                }
            }

            cnt = mapping.DieMatrix.XMax;

        trimFlag:
            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Right, cnt);

            return cnt;
        }

        // 获取 mapping 矩阵上方空白行
        private int TrimU(IMappingFile mapping)
        {
            int cnt = 0;

            for (int i = 0; i < mapping.DieMatrix.YMax; i++)
            {
                for (int j = 0; j < mapping.DieMatrix.XMax; j++)
                {
                    if (!this.IsEmptyDie(mapping.DieMatrix[j, i]))
                    {
                        cnt = i;
                        goto trimFlag;
                    }
                }
            }

            cnt = mapping.DieMatrix.YMax;

        trimFlag:
            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Up, cnt);

            return cnt;
        }

        // 获取 mapping 矩阵下方空白行
        private int TrimD(IMappingFile mapping)
        {
            int cnt = 0;

            int x = mapping.DieMatrix.XMax - 1;
            int y = mapping.DieMatrix.YMax - 1;

            for (int i = y; i >= 0; i--)
            {
                for (int j = x; j >= 0; j--)
                {
                    if (!this.IsEmptyDie(mapping.DieMatrix[j, i]))
                    {
                        cnt = mapping.DieMatrix.YMax - i - 1;
                        goto trimFlag;
                    }
                }
            }

            cnt = mapping.DieMatrix.YMax;

        trimFlag:
            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Down, cnt);

            return cnt;
        }

        // 写平边标记
        private void MarkNouch(IMappingFile map)
        {
            DieMatrix matrix = map.DieMatrix;
            int y = matrix.YMax - 1;

            for (int i = 0; i < matrix.XMax; i++)
            {
                if (
                    matrix[i, y].Attribute != DieCategory.PassDie && 
                    matrix[i, y].Attribute != DieCategory.FailDie && 
                    matrix[i, y].Attribute != DieCategory.TIRefFail && 
                    matrix[i, y].Attribute != DieCategory.TIRefPass
                    )
                    matrix[i, y].Attribute = DieCategory.MarkDie;
            }
        }

        // 判断是否为空 die
        private bool IsEmptyDie(DieData die)
        {
            bool r = false;

            switch (die.Attribute)
            {
                case DieCategory.PassDie:
                case DieCategory.FailDie:
                case DieCategory.TIRefFail:
                case DieCategory.TIRefPass:
                    r = false;
                    break;
                default:
                    r = true;
                    break;
            }

            return r;
        }

        private void clearFileMenuItem_Click(object sender, EventArgs e)
        {
            this._currFile = null;
            this.lsvItems.Columns[0].Text = "mapping file";
            this.lsvItems.Items.Clear();
        }

        private void Draw(Excel.Worksheet sheet)
        {
            if (this._currFile != null)
            {
                this.DrawMatrix(sheet);
            }
        }

        private void DrawMatrix(Excel.Worksheet sheet)
        {
            this._currFile.DieMatrix.Paint(sheet, false);
        }

        private bool ExpDataToExcel()
        {
            int num2;
            Excel.Application application = new Excel.ApplicationClass();
            application.Visible = false;
            object updateLinks = Missing.Value;
            DateTime now = DateTime.Now;
            Excel.Workbook workbook = application.Workbooks._Open(this.FilePath + @"\Sample.xls", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            OperateExcel excel = new OperateExcel(workbook);
           
           

             count = this.lsvItems.Items.Count;
            for (num2 = 0; num2 <= (count - 2); num2++)
            {
                excel.Copy("Sheet1", "aa");
                excel.Rename("Sheet1 (2)", this.lsvItems.Items[num2 + 1].Text.Trim());
                
            }
            excel.Rename("Sheet1", this.lsvItems.Items[0].Text.Trim());

            int num3 = this.FieldListBox1.CheckedItems.Count;
            object[] objArray = new object[num3];


            for (num2 = 0; num2 <= (count - 1); num2++)
            {
                this._currFile = (IMappingFile)this.lsvItems.Items[num2].Tag;
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[this.lsvItems.Items[num2].Text.Trim()];
                sheet.Columns.ColumnWidth = 1.25;
                sheet.Rows.RowHeight = (12.5 * this._currFile.DieMatrix.XMax) / ((double)this._currFile.DieMatrix.YMax);
                this.WriteSheet(sheet);
                Excel.Worksheet worksheet2 = (Excel.Worksheet)workbook.Sheets["Statistics"];

                object[,] aryTP = (object[,])(sheet.get_Range("A1:IV500", Missing.Value).Value2);
                sheet = null;



                object[] objArray2 = new object[num3];
                object[] objArray3 = new object[num3];
                Device = ((Tsk)this._currFile).Device;

                for (int i = 0; i <= (num3 - 1); i++)
                {
                    string str;
                    objArray2[i] = this.FieldListBox1.CheckedItems[i].ToString();

                    switch (this.FieldListBox1.CheckedItems[i].ToString())
                    {
                        case "LotNo":
                            {
                                objArray3[i] = ((Tsk)this._currFile).LotNo;
                                objArray[i] = "Total";
                                continue;
                            }

                        case "WaferID":
                            {
                                objArray3[i] = ((Tsk)this._currFile).WaferID;
                                objArray[i] = "";
                                continue;
                            }


                        case "Device":
                            {
                                objArray3[i] = ((Tsk)this._currFile).Device;
                                objArray[i] = "";
                                continue;
                            }


                        case "Total":
                            {
                                objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow | DieCategory.FailDie | DieCategory.PassDie);
                                if (objArray[i] == null)
                                {
                                    break;
                                }
                                if (objArray3[i] != null)
                                {
                                    objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                                }
                                continue;
                            }
                        case "Pass":
                            {
                                objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefPass | DieCategory.PassDie);
                                if (objArray[i] == null)
                                {
                                    goto Label_0458;
                                }
                                if (objArray3[i] != null)
                                {
                                    objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                                }
                                continue;
                            }
                        case "Fail":
                            {
                                objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.FailDie);
                                if (objArray[i] == null)
                                {
                                    goto Label_04C5;
                                }
                                if (objArray3[i] != null)
                                {
                                    objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                                }
                                continue;
                            }
                        case "Yield":
                            if ((objArray3[i - 2] == null) || (objArray3[i - 3] == null))
                            {
                                goto Label_0527;
                            }
                            objArray3[i] = Math.Round((double)(Convert.ToDouble(objArray3[i - 2]) / ((double)Convert.ToInt32(objArray3[i - 3]))), 4).ToString("0.00%");
                            if (objArray3[i].ToString() == "100.00%")
                            {
                                MessageBox.Show("TSK良率100%,请检查图谱是否有问题");

                            }
                            goto Label_0531;

                        case "Index X":
                            {
                                objArray3[i] = ((Tsk)this._currFile).IndexSizeX;
                                objArray[i] = "";
                                continue;
                            }
                        case "Index Y":
                            {
                                objArray3[i] = ((Tsk)this._currFile).IndexSizeY;
                                objArray[i] = "";
                                continue;
                            }
                        case "Wafer Size":
                            {
                                try
                                {
                                    objArray3[i] = ((Convert.ToInt32(((Tsk)this._currFile).WaferSize) / 10)).ToString() + "inch";
                                }
                                catch
                                {
                                    objArray3[i] = "";
                                }
                                objArray[i] = "";
                                continue;
                            }
                        case "OF Direction":
                            {
                                objArray3[i] = ((Tsk)this._currFile).FlatDir;
                                objArray[i] = "";
                                continue;
                            }
                        case "LoadTime":
                            {
                                objArray3[i] = ((Tsk)this._currFile).LoadTime.ToString();
                                objArray[i] = "";
                                continue;
                            }
                        case "UnloadTime":
                            {
                                objArray3[i] = ((Tsk)this._currFile).UnloadTime.ToString();
                                objArray[i] = "";
                                continue;
                            }
                        case "UsedTime":
                            {
                                objArray3[i] = ((TimeSpan)(((Tsk)this._currFile).UnloadTime - ((Tsk)this._currFile).LoadTime)).ToString();
                                objArray[i] = "";
                                continue;
                            }
                        case "BIN 0":
                            {
                                // objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefPass | DieCategory.PassDie);
                                objArray3[i] = ToCountDie._ToCountDie[0];
                                if (objArray[i] == null)
                                {
                                    goto Label_076F;
                                }
                                if (objArray3[i] != null)
                                {
                                    objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                                }
                                continue;
                            }
                        default:
                            goto Label_077E;
                    }
                    objArray[i] = objArray3[i];
                    continue;
                Label_0458:
                    objArray[i] = objArray3[i];
                    continue;
                Label_04C5:
                    objArray[i] = objArray3[i];
                    continue;
                Label_0527:
                    objArray3[i] = "";
                Label_0531:
                    if ((objArray[i - 2] != null) && (objArray[i - 3] != null))
                    {
                        objArray[i] = Math.Round(Convert.ToDouble((double)(Convert.ToDouble(objArray[i - 2]) / ((double)((int)objArray[i - 3])))), 4).ToString("0.00%");


                    }
                    else
                    {
                        objArray[i] = "";
                    }
                    continue;
                Label_076F:
                    objArray[i] = objArray3[i];
                    continue;
                Label_077E:
                    str = this.FieldListBox1.CheckedItems[i].ToString().Trim();
                    if (str.Substring(0, str.LastIndexOf(" ")).Trim() == "BIN")
                    {
                        str = str.Substring(str.LastIndexOf(" ")).Trim();
                        objArray3[i] = ToCountDie._ToCountDie[int.Parse(str)];
                        if (objArray[i] != null)
                        {
                            if (objArray3[i] != null)
                            {
                                objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                            }
                        }
                        else
                        {
                            objArray[i] = ToCountDie._ToCountDie[int.Parse(str)];
                        }
                    }
                    else
                    {
                        objArray3[i] = "??";
                        objArray[i] = "??";
                    }
                }

                worksheet2.get_Range(worksheet2.Cells[5, 1], worksheet2.Cells[5, num3]).Value2 = objArray2;
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 5, 1], worksheet2.Cells[(num2 + 1) + 5, num3]).Value2 = objArray3;
                worksheet2.get_Range(worksheet2.Cells[(num2 + 2) + 5, 1], worksheet2.Cells[(num2 + 2) + 5, num3]).Value2 = objArray;
                worksheet2 = null;
                this.progressBar1.Value++;
                /*

                              int [] Mnum = new int[9];
                              int[] MMnum = new int[9];

                                for(int i=3;i<500;i++)
                                    for (int j = 3; j < 255; j++)
                                    {      int Tnum = 0;

                                    if (aryTP[i, j ] != null)
                                    {
                                        if (aryTP[i, j].ToString() == "M")
                                        {

                                            if (aryTP[i, j + 1] != null && aryTP[i, j + 1].ToString() != "M") { Tnum++; }
                                            if (aryTP[i, j - 1] != null && aryTP[i, j - 1].ToString() != "M") { Tnum++; }
                                            if (aryTP[i + 1, j + 1] != null && aryTP[i + 1, j + 1].ToString() != "M") { Tnum++; }
                                            if (aryTP[i + 1, j - 1] != null && aryTP[i + 1, j - 1].ToString() != "M") { Tnum++; }
                                            if (aryTP[i - 1, j + 1] != null && aryTP[i - 1, j + 1].ToString() != "M") { Tnum++; }
                                            if (aryTP[i - 1, j - 1] != null && aryTP[i - 1, j - 1].ToString() != "M") { Tnum++; }
                                            if (aryTP[i - 1, j] != null && aryTP[i - 1, j].ToString() != "M") { Tnum++; }
                                            if (aryTP[i + 1, j] != null && aryTP[i + 1, j].ToString() != "M") { Tnum++; }

                                        }
                                    }

                                        if (Tnum == 8)
                                        {
                                            Mnum[0]++;

                                            if (aryTP[i, j + 1] != null&&aryTP[i, j + 1].ToString() != "M" && (Convert.ToInt32(aryTP[i, j + 1]) != 0 && Convert.ToInt32(aryTP[i, j + 1]) != 1))
                                            {
                                                Mnum[1]++;
                            
                                            }

                                            if (aryTP[i, j - 1] != null && aryTP[i, j- 1].ToString() != "M" && (Convert.ToInt32(aryTP[i, j -1]) != 0 && Convert.ToInt32(aryTP[i, j- 1]) != 1))
                                            {
                                                Mnum[2]++;

                                            }

                                            if (aryTP[i+1, j + 1] != null && aryTP[i+1, j + 1].ToString() != "M" && (Convert.ToInt32(aryTP[i+1, j + 1]) != 0 &&Convert.ToInt32(aryTP[i+1, j +1]) != 1))
                                            {
                                                Mnum[3]++;

                                            }

                                            if (aryTP[i + 1, j -1] != null && aryTP[i + 1, j - 1].ToString() != "M" && (Convert.ToInt32(aryTP[i + 1, j -1]) != 0 && Convert.ToInt32(aryTP[i + 1, j- 1]) != 1))
                                            {
                                                Mnum[4]++;

                                            }

                                            if (aryTP[i - 1, j + 1] != null && aryTP[i - 1, j + 1].ToString() != "M" && (Convert.ToInt32(aryTP[i - 1, j + 1]) != 0 &&Convert.ToInt32(aryTP[i -1, j + 1]) != 1))
                                            {
                                                Mnum[5]++;

                                            }

                                            if (aryTP[i - 1, j - 1] != null && aryTP[i - 1, j - 1].ToString() != "M" && (Convert.ToInt32(aryTP[i - 1, j- 1]) != 0 && Convert.ToInt32(aryTP[i - 1, j-1]) != 1))
                                            {
                                                Mnum[6]++;

                                            }

                                            if (aryTP[i - 1, j ] != null && aryTP[i - 1, j ].ToString() != "M" && (Convert.ToInt32(aryTP[i - 1, j ]) != 0 && Convert.ToInt32(aryTP[i - 1, j]) != 1))
                                            {
                                                Mnum[7]++;

                                            }

                                            if (aryTP[i + 1, j ] != null && aryTP[i + 1, j ].ToString() != "M" && (Convert.ToInt32(aryTP[i + 1, j ]) != 0 && Convert.ToInt32(aryTP[i + 1, j]) != 1))
                                            {
                                                Mnum[8]++;

                                            }

                        
                                        }



                                      else  if (Tnum >=5&&Tnum<8)
                                        {
                                            MMnum[0]++;

                                            if (aryTP[i, j + 1] != null && aryTP[i, j + 1].ToString() != "M" && (Convert.ToInt32(aryTP[i, j + 1]) != 0 && Convert.ToInt32(aryTP[i, j + 1]) != 1))
                                            {
                                                MMnum[1]++;

                                            }

                                            if (aryTP[i, j - 1] != null && aryTP[i, j - 1].ToString() != "M" && (Convert.ToInt32(aryTP[i, j - 1]) != 0 && Convert.ToInt32(aryTP[i, j - 1]) != 1))
                                            {
                                                MMnum[2]++;

                                            }

                                            if (aryTP[i + 1, j + 1] != null && aryTP[i + 1, j + 1].ToString() != "M" && (Convert.ToInt32(aryTP[i + 1, j + 1]) != 0 && Convert.ToInt32(aryTP[i + 1, j + 1]) != 1))
                                            {
                                                MMnum[3]++;

                                            }

                                            if (aryTP[i + 1, j - 1] != null && aryTP[i + 1, j - 1].ToString() != "M" && (Convert.ToInt32(aryTP[i + 1, j - 1]) != 0 && Convert.ToInt32(aryTP[i + 1, j - 1]) != 1))
                                            {
                                                MMnum[4]++;

                                            }

                                            if (aryTP[i - 1, j + 1] != null && aryTP[i - 1, j + 1].ToString() != "M" && (Convert.ToInt32(aryTP[i - 1, j + 1]) != 0 && Convert.ToInt32(aryTP[i - 1, j + 1]) != 1))
                                            {
                                                MMnum[5]++;

                                            }

                                            if (aryTP[i - 1, j - 1] != null && aryTP[i - 1, j - 1].ToString() != "M" && (Convert.ToInt32(aryTP[i - 1, j - 1]) != 0 && Convert.ToInt32(aryTP[i - 1, j - 1]) != 1))
                                            {
                                                MMnum[6]++;

                                            }

                                            if (aryTP[i - 1, j] != null && aryTP[i - 1, j].ToString() != "M" && (Convert.ToInt32(aryTP[i - 1, j]) != 0 && Convert.ToInt32(aryTP[i - 1, j]) != 1))
                                            {
                                                MMnum[7]++;

                                            }

                                            if (aryTP[i + 1, j] != null && aryTP[i + 1, j].ToString() != "M" && (Convert.ToInt32(aryTP[i + 1, j]) != 0 && Convert.ToInt32(aryTP[i + 1, j]) != 1))
                                            {
                                                MMnum[8]++;

                                            }


                                        }
                    
                    
                    
                                    }
                                int flag11 = 0;

                                for (int k = 1; k < 9; k++)
                                {
                                    double ff = (double)Mnum[k] / (double)Mnum[0];
                                    if ( ff>0.98)

                                    {
                                        flag11 = 1;
                                        break;
                    
                                    }
                
                                }


                                for (int kK = 1; kK < 9; kK++)
                                {
                                    double Fff = (double)Mnum[kK] / (double)Mnum[0];
                                    if (Fff > 0.8)
                                    {
                                        flag11 = 1;

                                        break;

                                    }

                                }

                                if (flag11 == 1)
                                {

                                    MailAddress MessageFrom = new MailAddress("daniel_huang@jcap.com.cn");
                                    string MessageTo = "daniel_huang@jcap.com.cn,ronnie_hu@jcap.com.cn,yong_qian@jcap.com.cn";
                                    string MessageSubject = "图谱偏移提醒";         //邮件主题
                                    string MessageBody = "您好，: " + this.LotNo + "-" + this.lsvItems.Items[num2].Text.Trim() + "图谱偏移，请知晓!!!!";
                                    Send(MessageFrom, MessageTo, MessageSubject, MessageBody);
                                    MessageBox.Show(this.lsvItems.Items[num2].Text.Trim() + "图谱偏移，请检查图谱是否有问题!!!!");
                
                                }

                            }
                    */
            }
            /////////////////////SBL BIN 确认////////////////////////////////////////////////////
            Device=ReturnName2(Device,0);

            if ( Device == "MPS3312GC")
            {
                Excel.Worksheet worksheet3 = (Excel.Worksheet)workbook.Sheets["Statistics"];
                object[,] arystring = (object[,])(worksheet3.get_Range("P6:AB200", Missing.Value).Value2);

                for(int i=6;i<count+6;i++)
                {

                    for (int j = 16; j < 29; j++)
                    {
                        Excel.Range rng3 = (Excel.Range)worksheet3.Cells[i, j];
                        if (arystring[i - 5, j - 15] != null)
                        {

                            rng3.Value2 = arystring[i - 5, j - 15].ToString() + "(" + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(arystring[i - 5, j - 15]) / 14433)), 4).ToString("0.00%") + ")";
                            if (j == 16 && Convert.ToDouble(arystring[i - 5, j - 15]) > 72)//BIN5
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 17 && Convert.ToDouble(arystring[i - 5, j - 15]) > 72)//BIN6
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 18 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN7
                            {
                                rng3.Interior.ColorIndex = 3;
                            }

                            if (j == 19 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN8
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 20 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN9
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 21 && Convert.ToDouble(arystring[i - 5, j - 15]) > 432)//BIN10
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 22 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN11
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 23 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN12
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 24 && Convert.ToDouble(arystring[i - 5, j - 15]) > 432)//BIN13
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 25 && Convert.ToDouble(arystring[i - 5, j - 15]) > 72)//BIN14
                            {
                                rng3.Interior.ColorIndex = 3;
                            }
                            if (j == 26 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN15
                            {
                                rng3.Interior.ColorIndex = 3;
                            }

                            if (j == 27 && Convert.ToDouble(arystring[i - 5, j - 15]) > 1154)//BIN16
                            {
                                rng3.Interior.ColorIndex = 3;
                            }

                            if (j == 28 && Convert.ToDouble(arystring[i - 5, j - 15]) > 144)//BIN17
                            {
                                rng3.Interior.ColorIndex = 3;
                            }



                        }
                    }

                }

                }
        


            ///-----------------------------------MPS3331--------------------------------------------------------/////


                if (Device == "MPS3331GC")
                {
                    Excel.Worksheet worksheet3 = (Excel.Worksheet)workbook.Sheets["Statistics"];
                    object[,] arystring = (object[,])(worksheet3.get_Range("P6:AB200", Missing.Value).Value2);

                    for (int i = 6; i < count + 6; i++)
                    {

                        for (int j = 16; j < 29; j++)
                        {
                            Excel.Range rng3 = (Excel.Range)worksheet3.Cells[i, j];
                            if (arystring[i - 5, j - 15] != null)
                            {

                                rng3.Value2 = arystring[i - 5, j - 15].ToString() + "(" + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(arystring[i - 5, j - 15]) /9079)), 4).ToString("0.00%") + ")";
                                if (j == 16 && Convert.ToDouble(arystring[i - 5, j - 15]) > 46)//BIN5
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 17 && Convert.ToDouble(arystring[i - 5, j - 15]) > 46)//BIN6
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 18 && Convert.ToDouble(arystring[i - 5, j - 15]) > 182)//BIN7
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }

                                if (j == 19 && Convert.ToDouble(arystring[i - 5, j - 15]) > 46)//BIN8
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 20 && Convert.ToDouble(arystring[i - 5, j - 15]) > 91)//BIN9
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 21 && Convert.ToDouble(arystring[i - 5, j - 15]) > 273)//BIN10
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 22 && Convert.ToDouble(arystring[i - 5, j - 15]) > 91)//BIN11
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 23 && Convert.ToDouble(arystring[i - 5, j - 15]) > 91)//BIN12
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 24 && Convert.ToDouble(arystring[i - 5, j - 15]) > 273)//BIN13
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 25 && Convert.ToDouble(arystring[i - 5, j - 15]) > 46)//BIN14
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }
                                if (j == 26 && Convert.ToDouble(arystring[i - 5, j - 15]) > 46)//BIN15
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }

                                if (j == 27 && Convert.ToDouble(arystring[i - 5, j - 15]) >182)//BIN16
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }

                                if (j == 28 && Convert.ToDouble(arystring[i - 5, j - 15]) > 453)//BIN17
                                {
                                    rng3.Interior.ColorIndex = 3;
                                }



                            }
                        }

                    }

                }
        
             
         
           
  
         
            
            

            this.ResultFileName = this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo + @"\" + this.LotNo + ".xls";
            workbook.SaveAs(this.ResultFileName, Excel.XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, Excel.XlSaveAsAccessMode.xlNoChange, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            
            excel = null;
            workbook = null;
            application.Quit();
            application = null;
            return true;
        }

        public string ReturnName2(string a, int n)
        {
            string[] b = a.Split(new char[] { '-' }, StringSplitOptions.None);

            return b[n];

        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadMappingFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }



        private void LoadMappingFile()
        {
            this.LoadTsk();
        }

        private void LoadTsk()
        {
           
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                

                TskFile = dialog.SelectedPath;


                DirectoryInfo TheFolder = new DirectoryInfo(TskFile);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                   
                    Tsk tsk = new Tsk(str.FullName);
                    tsk.Read();
                    this.LotNo = tsk.LotNo.Trim();
                    ListViewItem item = new ListViewItem(tsk.WaferID);
                    item.Tag = tsk;
                    this.lsvItems.Items.Add(item);
                    item.SubItems.Add(str.FullName);

                }
            }




        }

        private void lsvItems_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem itemAt = this.lsvItems.GetItemAt(e.X, e.Y);
            if (itemAt != null)
            {
                this.toolTip1.SetToolTip(this.lsvItems, itemAt.SubItems[1].Text);
                this.toolTip1.AutoPopDelay = 0x1388;
            }
            else
            {
                this.toolTip1.SetToolTip(this.lsvItems, "");
            }
        }

        private void lsvItems_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void MappingToExcel_Load(object sender, EventArgs e)
        {
            this.FieldsArray = new ArrayList();
            XmlDocument document = new XmlDocument();
            document.Load(this.FilePath + @"\Config.xml");
            XmlNodeList childNodes = document.SelectSingleNode("DataToExcel").ChildNodes;
            foreach (XmlNode node2 in childNodes)
            {
                XmlElement element = (XmlElement)node2;
                XmlNodeList list2 = element.ChildNodes;
                foreach (XmlNode node3 in list2)
                {
                    XmlElement element2 = (XmlElement)node3;
                    if (element2.Name == "Name")
                    {
                        this.Field = new FieldsProp();
                        this.Field.Name = element2.InnerText;
                        this.Field.Checked = element2.GetAttribute("checked");
                        this.FieldsArray.Add(this.Field);
                    }
                    else if (element2.Name == "Path")
                    {
                        this.textBox1.Text = element2.InnerText;
                    }
                }
            }
            foreach (FieldsProp prop in this.FieldsArray)
            {
                this.FieldListBox1.Items.Add(prop.Name, Convert.ToBoolean(prop.Checked));
            }
        }

        private void SavePath()
        {
            XmlDocument document = new XmlDocument();
            document.Load(this.FilePath + @"\Config.xml");
            XmlNodeList childNodes = document.SelectSingleNode("DataToExcel").ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = (XmlElement)node;
                XmlNodeList list2 = element.ChildNodes;
                foreach (XmlNode node2 in list2)
                {
                    XmlElement element2 = (XmlElement)node2;
                    if (element2.Name == "Path")
                    {
                        element2.InnerText = this.textBox1.Text.Trim();
                        break;
                    }
                }
            }
            document.Save(this.FilePath + @"\Config.xml");
        }

        private void ShowTsk(Excel.Worksheet sheet)
        {
            this.Draw(sheet);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.SavePath();
        }

        private void WriteSheet(Excel.Worksheet sheet)
        {
            this.ShowTsk(sheet);
        }


        public bool Send(MailAddress MessageFrom, string MessageTo, string MessageSubject, string MessageBody)
        {
            MailMessage message = new MailMessage();

            message.From = MessageFrom;
            message.To.Add(MessageTo);              //收件人邮箱地址可以是多个以实现群发
            message.Subject = MessageSubject;
            message.Body = MessageBody;
            message.IsBodyHtml = true;              //是否为html格式
            message.Priority = MailPriority.High;   //发送邮件的优先等级

            SmtpClient sc = new SmtpClient();
            // sc.Host = "smtp.163.com";    //指定发送邮件的服务器地址或IP
            sc.Host = "mail.jcap.com.cn";

            sc.Port = 25;                           //指定发送邮件端口
            // sc.Credentials = new System.Net.NetworkCredential("676537916@163.com", "852456123"); //指定登录服务器的用户名和密码

            sc.Credentials = new System.Net.NetworkCredential("daniel_huang@jcap.com.cn", "123456");

            try
            {
                sc.Send(message);       //发送邮件
            }
            catch
            {
                return false;
            }
            return true;
        }




       
     
    }
}
