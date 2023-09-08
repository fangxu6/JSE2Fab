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
using DataToExcel.ExpDataToExcelFactory;

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
            Excel.Workbook workbook = application.Workbooks._Open(this.FilePath + @"\Sample.xlsx", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            OperateExcel excel = new OperateExcel(workbook);



            count = this.lsvItems.Items.Count;
            for (num2 = 0; num2 <= (count - 2); num2++)
            {
                excel.Copy("Sheet1", "aa");
                excel.Rename("Sheet1 (2)", this.lsvItems.Items[num2 + 1].Text.Trim());

            }
            excel.Rename("Sheet1", this.lsvItems.Items[0].Text.Trim());

            int num3 = this.FieldListBox1.CheckedItems.Count;
            object[] objArray = new object[num3];//Total 信息
            object[] objArray4 = new object[num3];//平均值信息

            int flag11 = 0;

            for (num2 = 0; num2 <= (count - 1); num2++)
            {
                this._currFile = (IMappingFile)this.lsvItems.Items[num2].Tag;
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[this.lsvItems.Items[num2].Text.Trim()];
                sheet.Columns.ColumnWidth = 3.25;
                sheet.Rows.RowHeight = (22.5 * this._currFile.DieMatrix.XMax) / ((double)this._currFile.DieMatrix.YMax);
                this.WriteSheet(sheet);
                Excel.Worksheet worksheet2 = (Excel.Worksheet)workbook.Sheets["统计信息"];

                object[,] aryTP = (object[,])(sheet.get_Range("A1:IV500", Missing.Value).Value2);
                sheet = null;


                Excel.Range rngdevice = (Excel.Range)worksheet2.Cells[3, 2];
                rngdevice.Value2 = ((Tsk)this._currFile).Device;

                Excel.Range rnglot = (Excel.Range)worksheet2.Cells[4, 2];
                rnglot.Value2 = ((Tsk)this._currFile).LotNo;

                Excel.Range rnginch = (Excel.Range)worksheet2.Cells[5, 2];
                rnginch.Value2 = ((Convert.ToInt32(((Tsk)this._currFile).WaferSize) / 10)).ToString("0.0") + " inch";

                Excel.Range rngnum = (Excel.Range)worksheet2.Cells[6, 2];
                rngnum.Value2 = count.ToString() + " pcs";


                object[] objArray2 = new object[num3];//头信息文件
                object[] objArray3 = new object[num3];//每片Wafer信息
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
                                objArray[i] = "Total:";
                                objArray4[i] = "Average:";
                                continue;
                            }

                        case "Wafer ID":
                            {
                                objArray3[i] = ((Tsk)this._currFile).WaferID;
                                objArray[i] = "Total:";
                                objArray4[i] = "Average:";
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
                                // objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow | DieCategory.FailDie | DieCategory.PassDie);
                                objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie);
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
                                flag11 = i;
                                objArray3[i] = ToCountDie._ToCountDie[0];
                                ////add-2017.12.4///////////////////////
                                if (objArray3[i] == null)
                                {
                                    objArray3[i] = 0;
                                }
                                ///////////////////////////////////////
                                if (objArray[i] == null)
                                {

                                    goto Label_076F;
                                }


                                if (objArray3[i] != null)
                                {
                                    objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                                    //////////////////////////////////增加百分比////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    objArray3[i] = objArray3[i].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray3[i]) / ((double)this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie))), 4).ToString("0.00%") + ")";

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
                    //////////////////////////////////增加百分比////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    objArray3[i] = objArray3[i].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray3[i]) / ((double)this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie))), 4).ToString("0.00%") + ")";
                    continue;
                Label_077E:
                    str = this.FieldListBox1.CheckedItems[i].ToString().Trim();
                    if (str.Substring(0, str.LastIndexOf(" ")).Trim() == "BIN")
                    {
                        str = str.Substring(str.LastIndexOf(" ")).Trim();
                        objArray3[i] = ToCountDie._ToCountDie[int.Parse(str)];
                        /////////为0则显示为0-2017.12.4/////////////////////////////////
                        if (objArray3[i] == null)
                        {
                            objArray3[i] = 0;
                        }

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

                            /////////////////////为0则显示为0////////////////////////////////
                            if (objArray[i] == null)
                            {
                                objArray[i] = 0;
                            }
                        }


                        ////////////////////////////////增加百分比///////////////////////////
                        if (objArray3[i] != null)
                        {
                            objArray3[i] = objArray3[i].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray3[i]) / ((double)this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie))), 4).ToString("0.00%") + ")";

                        }
                        //////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        objArray3[i] = "??";
                        objArray[i] = "??";
                    }
                }

                worksheet2.get_Range(worksheet2.Cells[8, 1], worksheet2.Cells[8, num3]).Value2 = objArray2;
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, num3]).Value2 = objArray3;
                //  worksheet2.get_Range(worksheet2.Cells[(num2 + 2) + 8, 1], worksheet2.Cells[(num2 + 2) + 8, num3]).Value2 = objArray;
                worksheet2 = null;
                this.progressBar1.Value++;

            }

            ////////////////////////////////////////add total and average////////////////////////////////
            Excel.Worksheet worksheet3 = (Excel.Worksheet)workbook.Sheets["统计信息"];
            objArray4[1] = (int)objArray[1] / num2;
            objArray4[2] = (int)objArray[2] / num2;
            objArray4[3] = (int)objArray[3] / num2;
            objArray4[4] = objArray[4];
            for (int m = flag11; m < num3; m++)
            {
                if (objArray[m] != null)
                {
                    objArray4[m] = (int)objArray[m] / num2;
                }

            }
            for (int m = flag11; m < num3; m++)
            {
                objArray4[m] = objArray4[m].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray4[m]) / ((double)Convert.ToDouble(objArray4[1]))), 4).ToString("0.00%") + ")"; ;
                objArray[m] = objArray[m].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray[m]) / ((double)Convert.ToDouble(objArray[1]))), 4).ToString("0.00%") + ")"; ;
            }


            worksheet3.get_Range(worksheet3.Cells[(num2 + 2) + 8, 1], worksheet3.Cells[(num2 + 2) + 8, num3]).Value2 = objArray4;
            worksheet3.get_Range(worksheet3.Cells[(num2 + 3) + 8, 1], worksheet3.Cells[(num2 + 3) + 8, num3]).Value2 = objArray;
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.ResultFileName = this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo + @"\" + this.LotNo + ".xlsx";
            worksheet3.Activate();
            // workbook.SaveAs(this.ResultFileName, Excel.XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, Excel.XlSaveAsAccessMode.xlNoChange, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            workbook.SaveAs(this.ResultFileName, 51);
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

        private void button7_Click(object sender, EventArgs e)
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
                this.ExpDataToExcelAW();
                if (MessageBox.Show("Export EXCEL File Success!Would you like to open it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(this.ResultFileName);
                }
            }
        }

        private bool ExpDataToExcelAW()
        {
            int num2;
            Excel.Application application = new Excel.ApplicationClass();
            application.Visible = false;
            object updateLinks = Missing.Value;
            DateTime now = DateTime.Now;
            // Excel.Workbook workbook = application.Workbooks._Open(this.FilePath + @"\SampleAW.xls", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            Excel.Workbook workbook = application.Workbooks._Open(this.FilePath + @"\SampleAW.xlsx", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            OperateExcel excel = new OperateExcel(workbook);


            count = this.lsvItems.Items.Count;
            for (num2 = 0; num2 <= (count - 2); num2++)
            {
                excel.Copy("Sheet1", "aa");
                excel.Rename("Sheet1 (2)", this.lsvItems.Items[num2 + 1].Text.Trim());

            }
            excel.Rename("Sheet1", this.lsvItems.Items[0].Text.Trim());

            int num3 = this.FieldListBox1.CheckedItems.Count;
            object[] objArray = new object[num3];//Total 信息
            object[] objArray4 = new object[num3];//平均值信息

            int flag11 = 0;
            // TODO 

            for (num2 = 0; num2 <= (count - 1); num2++)
            {
                this._currFile = (IMappingFile)this.lsvItems.Items[num2].Tag;
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[this.lsvItems.Items[num2].Text.Trim()];
                sheet.Columns.ColumnWidth = 3.25;
                sheet.Rows.RowHeight = (22.5 * this._currFile.DieMatrix.XMax) / ((double)this._currFile.DieMatrix.YMax);
                this.WriteSheet(sheet);
                Excel.Worksheet worksheet2 = (Excel.Worksheet)workbook.Sheets["统计信息"];

                object[,] aryTP = (object[,])(sheet.get_Range("A1:IV500", Missing.Value).Value2);
                sheet = null;


                Excel.Range rngdevice = (Excel.Range)worksheet2.Cells[3, 2];
                rngdevice.Value2 = ((Tsk)this._currFile).Device;

                Excel.Range rnglot = (Excel.Range)worksheet2.Cells[4, 2];
                rnglot.Value2 = ((Tsk)this._currFile).LotNo;

                Excel.Range rnginch = (Excel.Range)worksheet2.Cells[5, 2];
                rnginch.Value2 = ((Convert.ToInt32(((Tsk)this._currFile).WaferSize) / 10)).ToString("0.0") + " inch";

                Excel.Range rngnum = (Excel.Range)worksheet2.Cells[6, 2];
                rngnum.Value2 = count.ToString() + " pcs";

                String deviceName = ((Tsk)this._currFile).Device;
                ExpToExcelSoftBin expToExcelSoftBin = ExpToExcelSoftBinFactory.GetExpToExcelSoft(deviceName);
                if (expToExcelSoftBin != null)
                {
                    expToExcelSoftBin.expToExcel(worksheet2);
                }

                if ((((Tsk)this._currFile).Device == "ICND2056-8-4-CP2") || (((Tsk)this._currFile).Device == "ICND2056-8-4-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP2_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUNC_PLL_ATE";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:leakage1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUNC_nor";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUNC_Mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:IOUT";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:SKEW_IOUT";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:TRIM";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:IOUT_1";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:SKEW_IOUT_1";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_AVE_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP2_Bin17:OS_VDD";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP2_Bin18:FUNC_PLL_ATE";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19:leakage1";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20:FUNC_nor";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21:FUNC_Mbist";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP2_Bin22:IOUT";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:SKEW_IOUT";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "CP2_Bin24:IOUT_AVE";


                }

                if ((((Tsk)this._currFile).Device == "2019WCA-8-8-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OS_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:Leakage2";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:FUN_test";

                }

                if ((((Tsk)this._currFile).Device == "2019WCA-8-16-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OS_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:Leakage2";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:FUN_test";

                }

                if ((((Tsk)this._currFile).Device == "2018WCA-8-32-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT_FAIL";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:fun_FAIL";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:LEAKAGE_in_FAIL";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:LEAKAGE_out_FAIL";
                }


                if ((((Tsk)this._currFile).Device == "2025WMA-8-8-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:OPEN_SHORT";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:FUN_all_3V";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:IOUT_1P5K";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:IOUT_1P5K_AVE";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_all_6V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:LEAKAGE_1";


                }

                if ((((Tsk)this._currFile).Device == "2025WNA-8-8-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:OPEN_SHORT";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:FUN_all_3V";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:IOUT_1P5K";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:IOUT_1P5K_AVE";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_all_6V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:LEAKAGE_1";


                }

                if ((((Tsk)this._currFile).Device == "2025WNA-8-16-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:OPEN_SHORT";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:FUN_all_3V";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:IOUT_1P5K";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:IOUT_1P5K_AVE";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_all_6V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:LEAKAGE_1";


                }


                if ((((Tsk)this._currFile).Device == "2047WAA-8-8-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin3 = (Excel.Range)worksheet2.Cells[7, 9];
                    rngbin3.Value2 = "Bin3:OS_PMU";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:OS_PMU_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:FUN_sdo";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:IOUT_12K";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:SKEW_12K";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:IOUT_12K_AVE";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:ft_open_debug";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:LEAKAGE_1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "Bin13:OS_PMU_1";

                }

                if ((((Tsk)this._currFile).Device == "2047WBA-8-8-04P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:OS_PMU";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OS_PMU_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:FUN_sdo";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:IOUT_12K";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:SKEW_12K";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:IOUT_12K_AVE";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:LEAKAGE_1";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:ft_open_debug";


                }
                if ((((Tsk)this._currFile).Device == "2053WIA-8-8-CP1"))
                {
                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "CP1_Bin4:OS_PMU";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:FUN_sdo";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_mbist";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:FUN_mbist2";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_mbist1";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:IOUT_12K";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:SKEW_12K";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:LEAKAGE_1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K_AVE";


                }

                if ((((Tsk)this._currFile).Device == "2053WIA-8-8-CP2"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP2_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "CP1_Bin4:OS_PMU";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU_VDD";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:FUN_sdo";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_mbist";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:FUN_mbist2";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_mbist1";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:IOUT_12K";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:SKEW_12K";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:LEAKAGE_1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K_AVE";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP2_Bin14:OS_PMU";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP2_Bin15:OS_PMU_VDD";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP2_Bin16:FUN_mbist";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP2_Bin17:FUN_mbist2";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP2_Bin18:FUN_mbist1";

                }


                if ((((Tsk)this._currFile).Device == "2065WCB-8-8-CP1"))
                {

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_PMU_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_sdo";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_norm";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUN_mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_mbist2";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_mbist1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                }

                if ((((Tsk)this._currFile).Device == "2065WCB-8-8-CP2"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP2_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_PMU_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_sdo";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_norm";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUN_mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_mbist2";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_mbist1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19:OS_PMU";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20:OS_PMU_VDD";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21:FUN_mbist";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP2_Bin22:FUN_mbist2";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:FUN_mbist1";
                }

                if ((((Tsk)this._currFile).Device == "2065WCB-8-16-CP1"))
                {

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_PMU_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_sdo";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_norm";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUN_mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_mbist2";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_mbist1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                }

                if ((((Tsk)this._currFile).Device == "2065WCB-8-16-CP2"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP2_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_PMU_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_sdo";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_norm";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUN_mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_mbist2";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_mbist1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19:OS_PMU";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20:OS_PMU_VDD";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21:FUN_mbist";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP2_Bin22:FUN_mbist2";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:FUN_mbist1";
                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-8-CP1"))
                {

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_PMU_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_sdo";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_norm";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUN_mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_mbist2";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_mbist1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-8-CP2"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP2_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS_PMU";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_PMU_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_sdo";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_norm";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUN_mbist";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_mbist2";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_mbist1";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19:OS_PMU";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20:OS_PMU_VDD";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21:FUN_mbist";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP2_Bin22:FUN_mbist2";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:FUN_mbist1";
                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-16-CP1"))
                {

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_SDO";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:Leakage_1";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_NORM";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_Mbist";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:FUN_Mbist2";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:FUN_Mbist1";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IDD_REXT_12K";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IDD_LDO_REGU";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:IOUT_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:SKEW_12K";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP1_Bin19:IOUT_12K_AVE";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP1_Bin20:IOUT_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP1_Bin21:SKEW_12K_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP1_Bin22:IOUT_12K_AVE_1";


                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-Y16-P1"))
                {

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_SDO";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:Leakage_1";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_NORM";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_Mbist";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:FUN_Mbist2";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:FUN_Mbist1";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IDD_REXT_12K";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IDD_LDO_REGU";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:IOUT_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:SKEW_12K";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP1_Bin19:IOUT_12K_AVE";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP1_Bin20:IOUT_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP1_Bin21:SKEW_12K_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP1_Bin22:IOUT_12K_AVE_1";


                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-16-CP2"))
                {
                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_SDO";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:Leakage_1";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_NORM";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_Mbist";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:FUN_Mbist2";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:FUN_Mbist1";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IDD_REXT_12K";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IDD_LDO_REGU";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:IOUT_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:SKEW_12K";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP1_Bin19:IOUT_12K_AVE";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP1_Bin20:IOUT_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP1_Bin21:SKEW_12K_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP1_Bin22:IOUT_12K_AVE_1";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:OS";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "CP2_Bin24:OS_VDD";

                    Excel.Range rngbin25 = (Excel.Range)worksheet2.Cells[7, 31];
                    rngbin25.Value2 = "CP2_Bin25:IDD_REXT_12K";

                    Excel.Range rngbin26 = (Excel.Range)worksheet2.Cells[7, 32];
                    rngbin26.Value2 = "CP2_Bin26:R_CLK";

                    Excel.Range rngbin27 = (Excel.Range)worksheet2.Cells[7, 33];
                    rngbin27.Value2 = "CP2_Bin27:IIH";

                    Excel.Range rngbin28 = (Excel.Range)worksheet2.Cells[7, 34];
                    rngbin28.Value2 = "CP2_Bin28:FUN_SDO";

                    Excel.Range rngbin29 = (Excel.Range)worksheet2.Cells[7, 35];
                    rngbin29.Value2 = "CP2_Bin29:LEAKAGE_ad1";

                    Excel.Range rngbin30 = (Excel.Range)worksheet2.Cells[7, 36];
                    rngbin30.Value2 = "CP2_Bin30:FUN_NORM";

                    Excel.Range rngbin31 = (Excel.Range)worksheet2.Cells[7, 37];
                    rngbin31.Value2 = "CP2_Bin31:FUN_Mbist";

                    Excel.Range rngbin32 = (Excel.Range)worksheet2.Cells[7, 38];
                    rngbin32.Value2 = "CP2_Bin32:FUN_Mbist2";

                    Excel.Range rngbin33 = (Excel.Range)worksheet2.Cells[7, 39];
                    rngbin33.Value2 = "CP2_Bin33:FUN_Mbist1";

                    Excel.Range rngbin34 = (Excel.Range)worksheet2.Cells[7, 40];
                    rngbin34.Value2 = "CP2_Bin34:IDD_LDO_REGU";

                    Excel.Range rngbin35 = (Excel.Range)worksheet2.Cells[7, 41];
                    rngbin35.Value2 = "CP2_Bin35:IDD_REXT_12K_0P1S";

                    Excel.Range rngbin36 = (Excel.Range)worksheet2.Cells[7, 42];
                    rngbin36.Value2 = "CP2_Bin36:R_CLK_0P1S";

                    Excel.Range rngbin37 = (Excel.Range)worksheet2.Cells[7, 43];
                    rngbin37.Value2 = "CP2_Bin37:IIH_0P1S";

                    Excel.Range rngbin38 = (Excel.Range)worksheet2.Cells[7, 44];
                    rngbin38.Value2 = "CP2_Bin38:FUN_SDO_0P1S";

                    Excel.Range rngbin39 = (Excel.Range)worksheet2.Cells[7, 45];
                    rngbin39.Value2 = "CP2_Bin39:LEAKAGE_ad1_0P1S";

                    Excel.Range rngbin40 = (Excel.Range)worksheet2.Cells[7, 46];
                    rngbin40.Value2 = "CP2_Bin40:FUN_NORM_0P1S";

                    Excel.Range rngbin41 = (Excel.Range)worksheet2.Cells[7, 47];
                    rngbin41.Value2 = "CP2_Bin41:FUN_Mbist_0P1S";

                    Excel.Range rngbin42 = (Excel.Range)worksheet2.Cells[7, 48];
                    rngbin42.Value2 = "CP2_Bin42:FUN_Mbist2_0P1S";

                    Excel.Range rngbin43 = (Excel.Range)worksheet2.Cells[7, 49];
                    rngbin43.Value2 = "CP2_Bin43:FUN_Mbist1_0P1S";

                    Excel.Range rngbin44 = (Excel.Range)worksheet2.Cells[7, 50];
                    rngbin44.Value2 = "CP2_Bin44:IDD_LDO_REGU_0P1S";




                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-Y16-P2"))
                {
                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OS";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OS_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUN_SDO";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:Leakage_1";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUN_NORM";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUN_Mbist";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:FUN_Mbist2";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:FUN_Mbist1";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IDD_REXT_12K";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IDD_LDO_REGU";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:IOUT_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:SKEW_12K";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP1_Bin19:IOUT_12K_AVE";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP1_Bin20:IOUT_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP1_Bin21:SKEW_12K_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP1_Bin22:IOUT_12K_AVE_1";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:OS";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "CP2_Bin24:OS_VDD";

                    Excel.Range rngbin25 = (Excel.Range)worksheet2.Cells[7, 31];
                    rngbin25.Value2 = "CP2_Bin25:IDD_REXT_12K";

                    Excel.Range rngbin26 = (Excel.Range)worksheet2.Cells[7, 32];
                    rngbin26.Value2 = "CP2_Bin26:R_CLK";

                    Excel.Range rngbin27 = (Excel.Range)worksheet2.Cells[7, 33];
                    rngbin27.Value2 = "CP2_Bin27:IIH";

                    Excel.Range rngbin28 = (Excel.Range)worksheet2.Cells[7, 34];
                    rngbin28.Value2 = "CP2_Bin28:FUN_SDO";

                    Excel.Range rngbin29 = (Excel.Range)worksheet2.Cells[7, 35];
                    rngbin29.Value2 = "CP2_Bin29:LEAKAGE_ad1";

                    Excel.Range rngbin30 = (Excel.Range)worksheet2.Cells[7, 36];
                    rngbin30.Value2 = "CP2_Bin30:FUN_NORM";

                    Excel.Range rngbin31 = (Excel.Range)worksheet2.Cells[7, 37];
                    rngbin31.Value2 = "CP2_Bin31:FUN_Mbist";

                    Excel.Range rngbin32 = (Excel.Range)worksheet2.Cells[7, 38];
                    rngbin32.Value2 = "CP2_Bin32:FUN_Mbist2";

                    Excel.Range rngbin33 = (Excel.Range)worksheet2.Cells[7, 39];
                    rngbin33.Value2 = "CP2_Bin33:FUN_Mbist1";

                    Excel.Range rngbin34 = (Excel.Range)worksheet2.Cells[7, 40];
                    rngbin34.Value2 = "CP2_Bin34:IDD_LDO_REGU";

                    Excel.Range rngbin35 = (Excel.Range)worksheet2.Cells[7, 41];
                    rngbin35.Value2 = "CP2_Bin35:IDD_REXT_12K_0P1S";

                    Excel.Range rngbin36 = (Excel.Range)worksheet2.Cells[7, 42];
                    rngbin36.Value2 = "CP2_Bin36:R_CLK_0P1S";

                    Excel.Range rngbin37 = (Excel.Range)worksheet2.Cells[7, 43];
                    rngbin37.Value2 = "CP2_Bin37:IIH_0P1S";

                    Excel.Range rngbin38 = (Excel.Range)worksheet2.Cells[7, 44];
                    rngbin38.Value2 = "CP2_Bin38:FUN_SDO_0P1S";

                    Excel.Range rngbin39 = (Excel.Range)worksheet2.Cells[7, 45];
                    rngbin39.Value2 = "CP2_Bin39:LEAKAGE_ad1_0P1S";

                    Excel.Range rngbin40 = (Excel.Range)worksheet2.Cells[7, 46];
                    rngbin40.Value2 = "CP2_Bin40:FUN_NORM_0P1S";

                    Excel.Range rngbin41 = (Excel.Range)worksheet2.Cells[7, 47];
                    rngbin41.Value2 = "CP2_Bin41:FUN_Mbist_0P1S";

                    Excel.Range rngbin42 = (Excel.Range)worksheet2.Cells[7, 48];
                    rngbin42.Value2 = "CP2_Bin42:FUN_Mbist2_0P1S";

                    Excel.Range rngbin43 = (Excel.Range)worksheet2.Cells[7, 49];
                    rngbin43.Value2 = "CP2_Bin43:FUN_Mbist1_0P1S";

                    Excel.Range rngbin44 = (Excel.Range)worksheet2.Cells[7, 50];
                    rngbin44.Value2 = "CP2_Bin44:IDD_LDO_REGU_0P1S";
                }

                if ((((Tsk)this._currFile).Device == "2065WEB-12-16-00"))
                {
                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OPEN_SHORT_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_SDO";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE_AD1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUNC_NOR";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUNC_mbist_1P8V";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUNC_mbist_2V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUNC_mbist_1P4V";


                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                }

                if ((((Tsk)this._currFile).Device == "2065WEB-12-16-01"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP2_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:OPEN_SHORT_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:FUN_SDO";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:LEAKAGE_AD1";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:FUNC_NOR";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP1_Bin10:FUNC_mbist_1P8V";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:FUNC_mbist_2V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP1_Bin12:FUNC_mbist_1P4V";


                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP1_Bin13:IOUT_12K";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP1_Bin14:SKEW_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP1_Bin15:IOUT_12K_AVE";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP1_Bin16:IOUT_12K_1";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:SKEW_12K_1";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP1_Bin18:IOUT_12K_AVE_1";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19:OPEN_SHORT";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20:OPEN_SHORT_VDD";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21:FUNC_mbist_1P8V";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP2_Bin22:FUNC_mbist_2V";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23:FUNC_mbist_1P4V";



                }

                if ((((Tsk)this._currFile).Device == "THEMIS-8-8-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "Bin2:SCAN_test";

                    Excel.Range rngbin3 = (Excel.Range)worksheet2.Cells[7, 9];
                    rngbin3.Value2 = "Bin3:Bist_test";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:ICC";


                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:os_down_test";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:eFlash_CHIP_Init";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:eFlash_Isb";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:eFlash_Erase_Ref_Cell";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:eFlash_DC_Stress";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:eFlash_Mass_Progam";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:eFlash_Mass_Erase";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:eFlash_Program_First_6Rows";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "Bin13:eFlash_Mass_Progam_1";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "Bin14:eFlash_Mass_Erase_1";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "Bin15:eFlash_Write_Disturb";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "Bin16:eFlash_Cycling_10x";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "Bin17:eFlash_Verify";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "Bin18:eFlash_Weak_Erase";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "Bin19：eFlash_Tox_Stress;";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "Bin20:eFlash_Mass_Erase_2;";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "Bin21:eFlash_Program_Full_Diagonal";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "Bin22:eFlash_Verify_Diagonal";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "Bin23：eFlash_Mass_Progam_2";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "Bin24:eFlash_Page_Erase";

                    Excel.Range rngbin25 = (Excel.Range)worksheet2.Cells[7, 31];
                    rngbin25.Value2 = "Bin25:eFlash_Verify_MRG_1_Read_info";

                    Excel.Range rngbin26 = (Excel.Range)worksheet2.Cells[7, 32];
                    rngbin26.Value2 = "Bin26:eFlash_Page_Erase_1";

                    Excel.Range rngbin27 = (Excel.Range)worksheet2.Cells[7, 33];
                    rngbin27.Value2 = "Bin27:eFlash_Verify_MRG_1_Read_main";


                    Excel.Range rngbin28 = (Excel.Range)worksheet2.Cells[7, 34];
                    rngbin28.Value2 = "Bin28:eFlash_Mass_Erase_3";

                    Excel.Range rngbin29 = (Excel.Range)worksheet2.Cells[7, 35];
                    rngbin29.Value2 = "Bin29:eFlash_Punch_Through";

                    Excel.Range rngbin30 = (Excel.Range)worksheet2.Cells[7, 36];
                    rngbin30.Value2 = "Bin30:eFlash_Mass_Erase_4";

                    Excel.Range rngbin31 = (Excel.Range)worksheet2.Cells[7, 37];
                    rngbin31.Value2 = "Bin31:eFlash_Program_0xFFFF";

                    Excel.Range rngbin32 = (Excel.Range)worksheet2.Cells[7, 38];
                    rngbin32.Value2 = "Bin32:eFlash_Mass_Erase_5";

                    Excel.Range rngbin33 = (Excel.Range)worksheet2.Cells[7, 39];
                    rngbin33.Value2 = "Bin33:eFlash_Program_Check_Board";

                    Excel.Range rngbin34 = (Excel.Range)worksheet2.Cells[7, 40];
                    rngbin34.Value2 = "Bin34:eFlash_Mass_Erase_6";

                    Excel.Range rngbin35 = (Excel.Range)worksheet2.Cells[7, 41];
                    rngbin35.Value2 = "Bin35:eFlash_Program_Check_Board_1;";

                    Excel.Range rngbin36 = (Excel.Range)worksheet2.Cells[7, 42];
                    rngbin36.Value2 = "Bin36:eFlash_Thin_Oxide_Leak;";

                    Excel.Range rngbin37 = (Excel.Range)worksheet2.Cells[7, 43];
                    rngbin37.Value2 = "Bin37:eFlash_Read_Disturb";

                    Excel.Range rngbin38 = (Excel.Range)worksheet2.Cells[7, 44];
                    rngbin38.Value2 = "Bin38:eFlash_Mass_Erase_7";


                    Excel.Range rngbin39 = (Excel.Range)worksheet2.Cells[7, 45];
                    rngbin39.Value2 = "Bin39:eFlash_Mass_Program_Single_FFFF";

                    Excel.Range rngbin40 = (Excel.Range)worksheet2.Cells[7, 46];
                    rngbin40.Value2 = "Bin40:eFlash_Mass_Progam_3";

                    Excel.Range rngbin41 = (Excel.Range)worksheet2.Cells[7, 47];
                    rngbin41.Value2 = "Bin41:eFlash_Mass_Erase_8";

                    Excel.Range rngbin42 = (Excel.Range)worksheet2.Cells[7, 48];
                    rngbin42.Value2 = "Bin42:eFlash_Verify_MRG_1_Read";

                    /* Excel.Range rngbin43 = (Excel.Range)worksheet2.Cells[7, 49];
                     rngbin43.Value2 = "Bin43:eFlash_Good_Die_Record";

                     Excel.Range rngbin44 = (Excel.Range)worksheet2.Cells[7, 50];
                     rngbin44.Value2 = "Bin44:eFlash_Bake_Write_Verify";

                     Excel.Range rngbin45 = (Excel.Range)worksheet2.Cells[7, 51];
                     rngbin45.Value2 = "Bin45:eFlash_CHIP_Init";

                     Excel.Range rngbin46 = (Excel.Range)worksheet2.Cells[7, 52];
                     rngbin46.Value2 = "Bin46:eFlash_Check_Intf_Mode";

                     Excel.Range rngbin47 = (Excel.Range)worksheet2.Cells[7, 53];
                     rngbin47.Value2 = "Bin47:eFlash_Verify_MRG1_Read_before";

                     Excel.Range rngbin48 = (Excel.Range)worksheet2.Cells[7, 54];
                     rngbin48.Value2 = "Bin48:eFlash_Erase_Ref_Cell ";

                     Excel.Range rngbin49 = (Excel.Range)worksheet2.Cells[7, 55];
                     rngbin49.Value2 = "Bin49:eFlash_Verify_MRG1_Read ";

                     Excel.Range rngbin50 = (Excel.Range)worksheet2.Cells[7, 56];
                     rngbin50.Value2 = "Bin50:eFlash_Mass_Erase ";

                     Excel.Range rngbin51 = (Excel.Range)worksheet2.Cells[7, 57];
                     rngbin51.Value2 = "Bin51:eFlash_Verify_MRG1_Read_2nd ";*/

                    Excel.Range rngbin52 = (Excel.Range)worksheet2.Cells[7, 58];
                    rngbin52.Value2 = "Bin52: eFlash_Bake_Write_Verify_ff";

                    Excel.Range rngbin55 = (Excel.Range)worksheet2.Cells[7, 61];
                    rngbin55.Value2 = "Bin55: eFlash_CHIP_Init";

                    Excel.Range rngbin56 = (Excel.Range)worksheet2.Cells[7, 62];
                    rngbin56.Value2 = "Bin56: eFlash_Check_Intf_Mode";

                    Excel.Range rngbin57 = (Excel.Range)worksheet2.Cells[7, 63];
                    rngbin57.Value2 = "Bin57: eFlash_Verify_MRG1_Read_i2c";

                    Excel.Range rngbin58 = (Excel.Range)worksheet2.Cells[7, 64];
                    rngbin58.Value2 = "Bin58: eFlash_Erase_Ref_Cell";

                    Excel.Range rngbin59 = (Excel.Range)worksheet2.Cells[7, 65];
                    rngbin59.Value2 = "Bin59: eFlash_Verify_MRG1_Read_i2c_1";

                    Excel.Range rngbin60 = (Excel.Range)worksheet2.Cells[7, 66];
                    rngbin60.Value2 = "Bin60: eFlash_Mass_Erase";

                    Excel.Range rngbin61 = (Excel.Range)worksheet2.Cells[7, 67];
                    rngbin61.Value2 = "Bin61: eFlash_Verify_MRG1_Read_2nd";

                    Excel.Range rngbin62 = (Excel.Range)worksheet2.Cells[7, 68];
                    rngbin62.Value2 = "Bin62: eFlash_Good_Die_Record";



                }

                if ((((Tsk)this._currFile).Device == "ICNC66-12-8-01P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "Bin2:OS_PMU";

                    Excel.Range rngbin3 = (Excel.Range)worksheet2.Cells[7, 9];
                    rngbin3.Value2 = "Bin3:OS_PWR";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "Bin4:LEAK_TEST";


                    /*   Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                       rngbin5.Value2 = "Bin5:VIHL_TEST";*/

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:VOHL_TEST ";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:VBG_TEST";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:IDDQ_TEST";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:ATPG_TEST ";


                }

                if ((((Tsk)this._currFile).Device == "2012WRA-12-16-00"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";



                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:FUN_pulldown";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:RIN/LEAKAGE_in";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:LEAKAGE_out";


                }

                if ((((Tsk)this._currFile).Device == "2053WMA-8-16-CP1"))
                {
                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:FUN_MBIST_2V";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_MBIST_1P8V";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:FUN_MBIST_1P5V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:FUN_ATPG";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "Bin13:FUN_NORM";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "Bin14:IDD_REXT_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "Bin15:LEAKAGE_AD1";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "Bin16:IOUT_12K";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "Bin17:SKEW_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "Bin18:IOUT_12K_AVE";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "Bin19:IOUT_12K_1";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "Bin20:SKEW_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "Bin21:IOUT_12K_AVE_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "Bin22:IOUT_2P2K";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "Bin23:SKEW_2P2K";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "Bin24:IOUT_2P2K_AVE";




                }

                if ((((Tsk)this._currFile).Device == "2053WMA-8-Y16-P1"))
                {
                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "Bin2:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:FUN_MBIST_2V";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_MBIST_1P8V";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:FUN_MBIST_1P5V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:FUN_ATPG";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "Bin13:FUN_NORM";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "Bin14:IDD_REXT_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "Bin15:LEAKAGE_AD1";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "Bin16:IOUT_12K";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "Bin17:SKEW_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "Bin18:IOUT_12K_AVE";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "Bin19:IOUT_12K_1";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "Bin20:SKEW_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "Bin21:IOUT_12K_AVE_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "Bin22:IOUT_2P2K";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "Bin23:SKEW_2P2K";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "Bin24:IOUT_2P2K_AVE";



                }

                if ((((Tsk)this._currFile).Device == "2053WMA-8-16-CP2"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:FUN_MBIST_2V";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_MBIST_1P8V";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:FUN_MBIST_1P5V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:FUN_ATPG";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "Bin13:FUN_NORM";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "Bin14:IDD_REXT_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "Bin15:LEAKAGE_AD1";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "Bin16:IOUT_12K";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "Bin17:SKEW_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "Bin18:IOUT_12K_AVE";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "Bin19:IOUT_12K_1";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "Bin20:SKEW_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "Bin21:IOUT_12K_AVE_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "Bin22:IOUT_2P2K";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "Bin23:SKEW_2P2K";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "Bin24:IOUT_2P2K_AVE";


                    Excel.Range rngbin25 = (Excel.Range)worksheet2.Cells[7, 31];
                    rngbin25.Value2 = "Bin25:OPEN_SHORT";

                    Excel.Range rngbin26 = (Excel.Range)worksheet2.Cells[7, 32];
                    rngbin26.Value2 = "Bin26:OPEN_SHORT_VDD";

                    Excel.Range rngbin27 = (Excel.Range)worksheet2.Cells[7, 33];
                    rngbin27.Value2 = "Bin27:R_CLK";

                    Excel.Range rngbin28 = (Excel.Range)worksheet2.Cells[7, 34];
                    rngbin28.Value2 = "Bin28:IIH";

                    Excel.Range rngbin29 = (Excel.Range)worksheet2.Cells[7, 35];
                    rngbin29.Value2 = "Bin29:FUN_MBIST_2V";

                    Excel.Range rngbin30 = (Excel.Range)worksheet2.Cells[7, 36];
                    rngbin30.Value2 = "Bin30:FUN_MBIST_1P8V";

                    Excel.Range rngbin31 = (Excel.Range)worksheet2.Cells[7, 37];
                    rngbin31.Value2 = "Bin31:FUN_MBIST_1P5V";

                    Excel.Range rngbin32 = (Excel.Range)worksheet2.Cells[7, 38];
                    rngbin32.Value2 = "Bin32:FUN_ATPG";

                    Excel.Range rngbin33 = (Excel.Range)worksheet2.Cells[7, 39];
                    rngbin33.Value2 = "Bin33:FUN_NORM";

                    Excel.Range rngbin34 = (Excel.Range)worksheet2.Cells[7, 40];
                    rngbin34.Value2 = "Bin34:IDD_REXT_12K";

                    Excel.Range rngbin35 = (Excel.Range)worksheet2.Cells[7, 41];
                    rngbin35.Value2 = "Bin35:LEAKAGE_AD1";



                }

                if ((((Tsk)this._currFile).Device == "2053WMA-8-Y16-P2"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OPEN_SHORT";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:R_CLK";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:IIH";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:FUN_MBIST_2V";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:FUN_MBIST_1P8V";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "Bin11:FUN_MBIST_1P5V";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "Bin12:FUN_ATPG";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "Bin13:FUN_NORM";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "Bin14:IDD_REXT_12K";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "Bin15:LEAKAGE_AD1";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "Bin16:IOUT_12K";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "Bin17:SKEW_12K";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "Bin18:IOUT_12K_AVE";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "Bin19:IOUT_12K_1";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "Bin20:SKEW_12K_1";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "Bin21:IOUT_12K_AVE_1";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "Bin22:IOUT_2P2K";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "Bin23:SKEW_2P2K";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "Bin24:IOUT_2P2K_AVE";


                    Excel.Range rngbin25 = (Excel.Range)worksheet2.Cells[7, 31];
                    rngbin25.Value2 = "Bin25:OPEN_SHORT";

                    Excel.Range rngbin26 = (Excel.Range)worksheet2.Cells[7, 32];
                    rngbin26.Value2 = "Bin26:OPEN_SHORT_VDD";

                    Excel.Range rngbin27 = (Excel.Range)worksheet2.Cells[7, 33];
                    rngbin27.Value2 = "Bin27:R_CLK";

                    Excel.Range rngbin28 = (Excel.Range)worksheet2.Cells[7, 34];
                    rngbin28.Value2 = "Bin28:IIH";

                    Excel.Range rngbin29 = (Excel.Range)worksheet2.Cells[7, 35];
                    rngbin29.Value2 = "Bin29:FUN_MBIST_2V";

                    Excel.Range rngbin30 = (Excel.Range)worksheet2.Cells[7, 36];
                    rngbin30.Value2 = "Bin30:FUN_MBIST_1P8V";

                    Excel.Range rngbin31 = (Excel.Range)worksheet2.Cells[7, 37];
                    rngbin31.Value2 = "Bin31:FUN_MBIST_1P5V";

                    Excel.Range rngbin32 = (Excel.Range)worksheet2.Cells[7, 38];
                    rngbin32.Value2 = "Bin32:FUN_ATPG";

                    Excel.Range rngbin33 = (Excel.Range)worksheet2.Cells[7, 39];
                    rngbin33.Value2 = "Bin33:FUN_NORM";

                    Excel.Range rngbin34 = (Excel.Range)worksheet2.Cells[7, 40];
                    rngbin34.Value2 = "Bin34:IDD_REXT_12K";

                    Excel.Range rngbin35 = (Excel.Range)worksheet2.Cells[7, 41];
                    rngbin35.Value2 = "Bin35:LEAKAGE_AD1";





                }

                if ((((Tsk)this._currFile).Device == "HS5154-8-8-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "Bin1:Pass";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "Bin5:OS_fail";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "Bin6:Leakage_fail";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "Bin7:IDDQ_ICC_fail";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "Bin8:PulseWidth_fail";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "Bin9:function1_fail";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "Bin10:function2_fail";


                }

                if ((((Tsk)this._currFile).Device == "CPS4038-8-32-00P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP3_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:OS";

                    Excel.Range rngbin3 = (Excel.Range)worksheet2.Cells[7, 9];
                    rngbin3.Value2 = "CP1_Bin3: ";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "CP1_Bin4: ";


                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5: ";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP2_Bin6:PASS";

                    Excel.Range rngbin10 = (Excel.Range)worksheet2.Cells[7, 16];
                    rngbin10.Value2 = "CP3_Bin10:OS ";

                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP3_Bin11: ";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP3_Bin12: ";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP3_Bin13: ";

                    /* Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                     rngbin14.Value2 = "Bin14:eFlash_Mass_Erase_1";

                     Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                     rngbin15.Value2 = "Bin15:eFlash_Write_Disturb";

                     Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                     rngbin16.Value2 = "Bin16:eFlash_Cycling_10x";
                     */
                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP1_Bin17:PASS";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP2_Bin18: OS";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19： ;";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20: ;";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21: ";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP2_Bin22: ";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "CP2_Bin24: ";

                }

                if ((((Tsk)this._currFile).Device == "CPS4019-8-32-01P"))
                {
                    Excel.Range rngbin1 = (Excel.Range)worksheet2.Cells[7, 7];
                    rngbin1.Value2 = "CP3_Bin1:Pass";

                    Excel.Range rngbin2 = (Excel.Range)worksheet2.Cells[7, 8];
                    rngbin2.Value2 = "CP1_Bin2:OS";

                    Excel.Range rngbin3 = (Excel.Range)worksheet2.Cells[7, 9];
                    rngbin3.Value2 = "CP1_Bin3: ";

                    Excel.Range rngbin4 = (Excel.Range)worksheet2.Cells[7, 10];
                    rngbin4.Value2 = "CP1_Bin4: ";

                    Excel.Range rngbin5 = (Excel.Range)worksheet2.Cells[7, 11];
                    rngbin5.Value2 = "CP1_Bin5: ";

                    Excel.Range rngbin6 = (Excel.Range)worksheet2.Cells[7, 12];
                    rngbin6.Value2 = "CP1_Bin6:";

                    Excel.Range rngbin7 = (Excel.Range)worksheet2.Cells[7, 13];
                    rngbin7.Value2 = "CP1_Bin7:";

                    Excel.Range rngbin8 = (Excel.Range)worksheet2.Cells[7, 14];
                    rngbin8.Value2 = "CP1_Bin8:";

                    Excel.Range rngbin9 = (Excel.Range)worksheet2.Cells[7, 15];
                    rngbin9.Value2 = "CP1_Bin9:";



                    Excel.Range rngbin11 = (Excel.Range)worksheet2.Cells[7, 17];
                    rngbin11.Value2 = "CP1_Bin11:Pass ";

                    Excel.Range rngbin12 = (Excel.Range)worksheet2.Cells[7, 18];
                    rngbin12.Value2 = "CP2_Bin12:OS";

                    Excel.Range rngbin13 = (Excel.Range)worksheet2.Cells[7, 19];
                    rngbin13.Value2 = "CP2_Bin13: ";

                    Excel.Range rngbin14 = (Excel.Range)worksheet2.Cells[7, 20];
                    rngbin14.Value2 = "CP2_Bin14:";

                    Excel.Range rngbin15 = (Excel.Range)worksheet2.Cells[7, 21];
                    rngbin15.Value2 = "CP2_Bin15:";

                    Excel.Range rngbin16 = (Excel.Range)worksheet2.Cells[7, 22];
                    rngbin16.Value2 = "CP2_Bin16:";

                    Excel.Range rngbin17 = (Excel.Range)worksheet2.Cells[7, 23];
                    rngbin17.Value2 = "CP2_Bin17:Pass";

                    Excel.Range rngbin18 = (Excel.Range)worksheet2.Cells[7, 24];
                    rngbin18.Value2 = "CP2_Bin18: ";

                    Excel.Range rngbin19 = (Excel.Range)worksheet2.Cells[7, 25];
                    rngbin19.Value2 = "CP2_Bin19： ;";

                    Excel.Range rngbin20 = (Excel.Range)worksheet2.Cells[7, 26];
                    rngbin20.Value2 = "CP2_Bin20: ;";

                    Excel.Range rngbin21 = (Excel.Range)worksheet2.Cells[7, 27];
                    rngbin21.Value2 = "CP2_Bin21: ";

                    Excel.Range rngbin22 = (Excel.Range)worksheet2.Cells[7, 28];
                    rngbin22.Value2 = "CP3_Bin22: os";

                    Excel.Range rngbin23 = (Excel.Range)worksheet2.Cells[7, 29];
                    rngbin23.Value2 = "CP2_Bin23: ";

                    Excel.Range rngbin24 = (Excel.Range)worksheet2.Cells[7, 30];
                    rngbin24.Value2 = "CP2_Bin24: ";

                    Excel.Range rngbin25 = (Excel.Range)worksheet2.Cells[7, 31];
                    rngbin25.Value2 = "CP2_Bin25: ";

                    Excel.Range rngbin26 = (Excel.Range)worksheet2.Cells[7, 32];
                    rngbin26.Value2 = "CP2_Bin26: ";

                    Excel.Range rngbin27 = (Excel.Range)worksheet2.Cells[7, 33];
                    rngbin27.Value2 = "CP2_Bin27: ";


                    Excel.Range rngbin28 = (Excel.Range)worksheet2.Cells[7, 34];
                    rngbin28.Value2 = "CP2_Bin28: ";

                }

                object[] objArray2 = new object[num3];//头信息文件
                object[] objArray3 = new object[num3];//每片Wafer信息
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
                                objArray[i] = "Total:";
                                objArray4[i] = "Average:";
                                continue;
                            }

                        case "Wafer ID":
                            {
                                objArray3[i] = ((Tsk)this._currFile).WaferID;
                                objArray[i] = "Total:";
                                objArray4[i] = "Average:";
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
                                // objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow | DieCategory.FailDie | DieCategory.PassDie);
                                objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie);
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
                                flag11 = i;
                                objArray3[i] = ToCountDie._ToCountDie[0];
                                ////add-2017.12.4///////////////////////
                                if (objArray3[i] == null)
                                {
                                    objArray3[i] = 0;
                                }
                                ///////////////////////////////////////
                                if (objArray[i] == null)
                                {

                                    goto Label_076F;
                                }


                                if (objArray3[i] != null)
                                {
                                    objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
                                    //////////////////////////////////增加百分比////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    //  objArray3[i] = objArray3[i].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray3[i]) / ((double)this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie))), 4).ToString("0.00%") + ")";

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
                    //////////////////////////////////增加百分比////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // objArray3[i] = objArray3[i].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray3[i]) / ((double)this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie))), 4).ToString("0.00%") + ")";
                    continue;
                Label_077E:
                    str = this.FieldListBox1.CheckedItems[i].ToString().Trim();
                    if (str.Substring(0, str.LastIndexOf(" ")).Trim() == "BIN")
                    {
                        str = str.Substring(str.LastIndexOf(" ")).Trim();
                        objArray3[i] = ToCountDie._ToCountDie[int.Parse(str)];
                        /////////为0则显示为0-2017.12.4/////////////////////////////////
                        if (objArray3[i] == null)
                        {
                            objArray3[i] = 0;
                        }

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

                            /////////////////////为0则显示为0////////////////////////////////
                            if (objArray[i] == null)
                            {
                                objArray[i] = 0;
                            }
                        }


                        ////////////////////////////////增加百分比///////////////////////////
                        if (objArray3[i] != null)
                        {
                            //   objArray3[i] = objArray3[i].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray3[i]) / ((double)this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.FailDie | DieCategory.PassDie))), 4).ToString("0.00%") + ")";

                        }
                        //////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        objArray3[i] = "??";
                        objArray[i] = "??";
                    }
                }

                worksheet2.get_Range(worksheet2.Cells[8, 1], worksheet2.Cells[8, num3]).Value2 = objArray2;
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, num3]).Value2 = objArray3;
                if ((((Tsk)this._currFile).Device == "2053WMA-8-Y16-P2"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[30]) > 114) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 31], worksheet2.Cells[(num2 + 1) + 8, 31]).Interior.ColorIndex = 7; flagbin++; }//bin25
                    if (Convert.ToInt32(objArray3[31]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 32], worksheet2.Cells[(num2 + 1) + 8, 32]).Interior.ColorIndex = 7; flagbin++; }//bin26
                    if (Convert.ToInt32(objArray3[32]) > 85) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 33], worksheet2.Cells[(num2 + 1) + 8, 33]).Interior.ColorIndex = 7; flagbin++; }//bin27
                    if (Convert.ToInt32(objArray3[33]) > 156) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 34], worksheet2.Cells[(num2 + 1) + 8, 34]).Interior.ColorIndex = 7; flagbin++; }//bin28
                    if (Convert.ToInt32(objArray3[34]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 35], worksheet2.Cells[(num2 + 1) + 8, 35]).Interior.ColorIndex = 7; flagbin++; }//bin29
                    if (Convert.ToInt32(objArray3[35]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 36], worksheet2.Cells[(num2 + 1) + 8, 36]).Interior.ColorIndex = 7; flagbin++; }//bin30
                    if (Convert.ToInt32(objArray3[36]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 37], worksheet2.Cells[(num2 + 1) + 8, 37]).Interior.ColorIndex = 7; flagbin++; }//bin31
                    if (Convert.ToInt32(objArray3[37]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 38], worksheet2.Cells[(num2 + 1) + 8, 38]).Interior.ColorIndex = 7; flagbin++; }//bin32
                    if (Convert.ToInt32(objArray3[38]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 39], worksheet2.Cells[(num2 + 1) + 8, 39]).Interior.ColorIndex = 7; flagbin++; }//bin33
                    if (Convert.ToInt32(objArray3[39]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 40], worksheet2.Cells[(num2 + 1) + 8, 40]).Interior.ColorIndex = 7; flagbin++; }//bin34
                    if (Convert.ToInt32(objArray3[40]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 41], worksheet2.Cells[(num2 + 1) + 8, 41]).Interior.ColorIndex = 7; flagbin++; }//bin35
                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }
                if ((((Tsk)this._currFile).Device == "2053WMA-8-Y16-P1"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[26]) > 10) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 27], worksheet2.Cells[(num2 + 1) + 8, 27]).Interior.ColorIndex = 7; flagbin++; }//bin21

                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2053WMA-8-16-CP1"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[26]) > 10) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 27], worksheet2.Cells[(num2 + 1) + 8, 27]).Interior.ColorIndex = 7; flagbin++; }//bin21

                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2053WMA-8-16-CP2"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[30]) > 114) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 31], worksheet2.Cells[(num2 + 1) + 8, 31]).Interior.ColorIndex = 7; flagbin++; }//bin25
                    if (Convert.ToInt32(objArray3[31]) > 114) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 32], worksheet2.Cells[(num2 + 1) + 8, 32]).Interior.ColorIndex = 7; flagbin++; }//bin26
                    if (Convert.ToInt32(objArray3[32]) > 85) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 33], worksheet2.Cells[(num2 + 1) + 8, 33]).Interior.ColorIndex = 7; flagbin++; }//bin27
                    if (Convert.ToInt32(objArray3[33]) > 156) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 34], worksheet2.Cells[(num2 + 1) + 8, 34]).Interior.ColorIndex = 7; flagbin++; }//bin28
                    if (Convert.ToInt32(objArray3[34]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 35], worksheet2.Cells[(num2 + 1) + 8, 35]).Interior.ColorIndex = 7; flagbin++; }//bin29
                    if (Convert.ToInt32(objArray3[35]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 36], worksheet2.Cells[(num2 + 1) + 8, 36]).Interior.ColorIndex = 7; flagbin++; }//bin30
                    if (Convert.ToInt32(objArray3[36]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 37], worksheet2.Cells[(num2 + 1) + 8, 37]).Interior.ColorIndex = 7; flagbin++; }//bin31
                    if (Convert.ToInt32(objArray3[37]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 38], worksheet2.Cells[(num2 + 1) + 8, 38]).Interior.ColorIndex = 7; flagbin++; }//bin32
                    if (Convert.ToInt32(objArray3[38]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 39], worksheet2.Cells[(num2 + 1) + 8, 39]).Interior.ColorIndex = 7; flagbin++; }//bin33
                    if (Convert.ToInt32(objArray3[39]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 40], worksheet2.Cells[(num2 + 1) + 8, 40]).Interior.ColorIndex = 7; flagbin++; }//bin34
                    if (Convert.ToInt32(objArray3[40]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 41], worksheet2.Cells[(num2 + 1) + 8, 41]).Interior.ColorIndex = 7; flagbin++; }//bin35
                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-16-CP2"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[28]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 29], worksheet2.Cells[(num2 + 1) + 8, 29]).Interior.ColorIndex = 7; flagbin++; }//bin23
                    if (Convert.ToInt32(objArray3[29]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 30], worksheet2.Cells[(num2 + 1) + 8, 30]).Interior.ColorIndex = 7; flagbin++; }//bin24
                    if (Convert.ToInt32(objArray3[30]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 31], worksheet2.Cells[(num2 + 1) + 8, 31]).Interior.ColorIndex = 7; flagbin++; }//bin25
                    if (Convert.ToInt32(objArray3[31]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 32], worksheet2.Cells[(num2 + 1) + 8, 32]).Interior.ColorIndex = 7; flagbin++; }//bin26
                    if (Convert.ToInt32(objArray3[32]) > 20) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 33], worksheet2.Cells[(num2 + 1) + 8, 33]).Interior.ColorIndex = 7; flagbin++; }//bin27
                    if (Convert.ToInt32(objArray3[33]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 34], worksheet2.Cells[(num2 + 1) + 8, 34]).Interior.ColorIndex = 7; flagbin++; }//bin28
                    if (Convert.ToInt32(objArray3[34]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 35], worksheet2.Cells[(num2 + 1) + 8, 35]).Interior.ColorIndex = 7; flagbin++; }//bin29
                    if (Convert.ToInt32(objArray3[35]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 36], worksheet2.Cells[(num2 + 1) + 8, 36]).Interior.ColorIndex = 7; flagbin++; }//bin30
                    if (Convert.ToInt32(objArray3[36]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 37], worksheet2.Cells[(num2 + 1) + 8, 37]).Interior.ColorIndex = 7; flagbin++; }//bin31
                    if (Convert.ToInt32(objArray3[37]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 38], worksheet2.Cells[(num2 + 1) + 8, 38]).Interior.ColorIndex = 7; flagbin++; }//bin32
                    if (Convert.ToInt32(objArray3[38]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 39], worksheet2.Cells[(num2 + 1) + 8, 39]).Interior.ColorIndex = 7; flagbin++; }//bin33
                    if (Convert.ToInt32(objArray3[39]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 40], worksheet2.Cells[(num2 + 1) + 8, 40]).Interior.ColorIndex = 7; flagbin++; }//bin34

                    if (Convert.ToInt32(objArray3[40]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 41], worksheet2.Cells[(num2 + 1) + 8, 41]).Interior.ColorIndex = 7; flagbin++; }//bin35
                    if (Convert.ToInt32(objArray3[41]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 42], worksheet2.Cells[(num2 + 1) + 8, 42]).Interior.ColorIndex = 7; flagbin++; }//bin36
                    if (Convert.ToInt32(objArray3[42]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 43], worksheet2.Cells[(num2 + 1) + 8, 43]).Interior.ColorIndex = 7; flagbin++; }//bin37
                    if (Convert.ToInt32(objArray3[43]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 44], worksheet2.Cells[(num2 + 1) + 8, 44]).Interior.ColorIndex = 7; flagbin++; }//bin38
                    if (Convert.ToInt32(objArray3[44]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 45], worksheet2.Cells[(num2 + 1) + 8, 45]).Interior.ColorIndex = 7; flagbin++; }//bin39
                    if (Convert.ToInt32(objArray3[45]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 46], worksheet2.Cells[(num2 + 1) + 8, 46]).Interior.ColorIndex = 7; flagbin++; }//bin40
                    if (Convert.ToInt32(objArray3[46]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 47], worksheet2.Cells[(num2 + 1) + 8, 47]).Interior.ColorIndex = 7; flagbin++; }//bin41
                    if (Convert.ToInt32(objArray3[47]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 48], worksheet2.Cells[(num2 + 1) + 8, 48]).Interior.ColorIndex = 7; flagbin++; }//bin42
                    if (Convert.ToInt32(objArray3[48]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 49], worksheet2.Cells[(num2 + 1) + 8, 49]).Interior.ColorIndex = 7; flagbin++; }//bin43
                    if (Convert.ToInt32(objArray3[49]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 50], worksheet2.Cells[(num2 + 1) + 8, 50]).Interior.ColorIndex = 7; flagbin++; }//bin44

                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-16-CP1"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[27]) > 10) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 28], worksheet2.Cells[(num2 + 1) + 8, 28]).Interior.ColorIndex = 7; flagbin++; }//bin22

                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-Y16-P1"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[27]) > 10) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 28], worksheet2.Cells[(num2 + 1) + 8, 28]).Interior.ColorIndex = 7; flagbin++; }//bin22

                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2065WAA-8-Y16-P2"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[28]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 29], worksheet2.Cells[(num2 + 1) + 8, 29]).Interior.ColorIndex = 7; flagbin++; }//bin23
                    if (Convert.ToInt32(objArray3[29]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 30], worksheet2.Cells[(num2 + 1) + 8, 30]).Interior.ColorIndex = 7; flagbin++; }//bin24
                    if (Convert.ToInt32(objArray3[30]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 31], worksheet2.Cells[(num2 + 1) + 8, 31]).Interior.ColorIndex = 7; flagbin++; }//bin25
                    if (Convert.ToInt32(objArray3[31]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 32], worksheet2.Cells[(num2 + 1) + 8, 32]).Interior.ColorIndex = 7; flagbin++; }//bin26
                    if (Convert.ToInt32(objArray3[32]) > 20) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 33], worksheet2.Cells[(num2 + 1) + 8, 33]).Interior.ColorIndex = 7; flagbin++; }//bin27
                    if (Convert.ToInt32(objArray3[33]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 34], worksheet2.Cells[(num2 + 1) + 8, 34]).Interior.ColorIndex = 7; flagbin++; }//bin28
                    if (Convert.ToInt32(objArray3[34]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 35], worksheet2.Cells[(num2 + 1) + 8, 35]).Interior.ColorIndex = 7; flagbin++; }//bin29
                    if (Convert.ToInt32(objArray3[35]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 36], worksheet2.Cells[(num2 + 1) + 8, 36]).Interior.ColorIndex = 7; flagbin++; }//bin30
                    if (Convert.ToInt32(objArray3[36]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 37], worksheet2.Cells[(num2 + 1) + 8, 37]).Interior.ColorIndex = 7; flagbin++; }//bin31
                    if (Convert.ToInt32(objArray3[37]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 38], worksheet2.Cells[(num2 + 1) + 8, 38]).Interior.ColorIndex = 7; flagbin++; }//bin32
                    if (Convert.ToInt32(objArray3[38]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 39], worksheet2.Cells[(num2 + 1) + 8, 39]).Interior.ColorIndex = 7; flagbin++; }//bin33
                    if (Convert.ToInt32(objArray3[39]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 40], worksheet2.Cells[(num2 + 1) + 8, 40]).Interior.ColorIndex = 7; flagbin++; }//bin34

                    if (Convert.ToInt32(objArray3[40]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 41], worksheet2.Cells[(num2 + 1) + 8, 41]).Interior.ColorIndex = 7; flagbin++; }//bin35
                    if (Convert.ToInt32(objArray3[41]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 42], worksheet2.Cells[(num2 + 1) + 8, 42]).Interior.ColorIndex = 7; flagbin++; }//bin36
                    if (Convert.ToInt32(objArray3[42]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 43], worksheet2.Cells[(num2 + 1) + 8, 43]).Interior.ColorIndex = 7; flagbin++; }//bin37
                    if (Convert.ToInt32(objArray3[43]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 44], worksheet2.Cells[(num2 + 1) + 8, 44]).Interior.ColorIndex = 7; flagbin++; }//bin38
                    if (Convert.ToInt32(objArray3[44]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 45], worksheet2.Cells[(num2 + 1) + 8, 45]).Interior.ColorIndex = 7; flagbin++; }//bin39
                    if (Convert.ToInt32(objArray3[45]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 46], worksheet2.Cells[(num2 + 1) + 8, 46]).Interior.ColorIndex = 7; flagbin++; }//bin40
                    if (Convert.ToInt32(objArray3[46]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 47], worksheet2.Cells[(num2 + 1) + 8, 47]).Interior.ColorIndex = 7; flagbin++; }//bin41
                    if (Convert.ToInt32(objArray3[47]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 48], worksheet2.Cells[(num2 + 1) + 8, 48]).Interior.ColorIndex = 7; flagbin++; }//bin42
                    if (Convert.ToInt32(objArray3[48]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 49], worksheet2.Cells[(num2 + 1) + 8, 49]).Interior.ColorIndex = 7; flagbin++; }//bin43
                    if (Convert.ToInt32(objArray3[49]) > 9) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 50], worksheet2.Cells[(num2 + 1) + 8, 50]).Interior.ColorIndex = 7; flagbin++; }//bin44



                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "2053WFA-8-16-CP2"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[23]) > 15) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 24], worksheet2.Cells[(num2 + 1) + 8, 24]).Interior.ColorIndex = 7; flagbin++; }//bin18
                    if (Convert.ToInt32(objArray3[24]) > 15) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 25], worksheet2.Cells[(num2 + 1) + 8, 25]).Interior.ColorIndex = 7; flagbin++; }//bin19
                    if (Convert.ToInt32(objArray3[25]) > 15) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 26], worksheet2.Cells[(num2 + 1) + 8, 26]).Interior.ColorIndex = 7; flagbin++; }//bin20
                    if (Convert.ToInt32(objArray3[26]) > 15) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 27], worksheet2.Cells[(num2 + 1) + 8, 27]).Interior.ColorIndex = 7; flagbin++; }//bin21
                    if (Convert.ToInt32(objArray3[27]) > 15) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 28], worksheet2.Cells[(num2 + 1) + 8, 28]).Interior.ColorIndex = 7; flagbin++; }//bin22
                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }

                if ((((Tsk)this._currFile).Device == "CPS4019-8-32-01P"))
                {
                    int flagbin = 0;
                    if (Convert.ToInt32(objArray3[7]) > 7) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 8], worksheet2.Cells[(num2 + 1) + 8, 8]).Interior.ColorIndex = 7; flagbin++; }//bin2
                    if (Convert.ToInt32(objArray3[17]) > 4) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 18], worksheet2.Cells[(num2 + 1) + 8, 18]).Interior.ColorIndex = 7; flagbin++; }//bin12
                    if (Convert.ToInt32(objArray3[27]) > 4) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 28], worksheet2.Cells[(num2 + 1) + 8, 28]).Interior.ColorIndex = 7; flagbin++; }//bin22
                    if (flagbin > 0) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7; MessageBox.Show(objArray3[0].ToString() + "--SBL超标,请检查图谱是否有问题"); }

                }








                // worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, num3]).Interior.ColorIndex = 7;
                //  worksheet2.get_Range(worksheet2.Cells[(num2 + 2) + 8, 1], worksheet2.Cells[(num2 + 2) + 8, num3]).Value2 = objArray;
                worksheet2 = null;
                this.progressBar1.Value++;

            }

            ////////////////////////////////////////add total and average////////////////////////////////
            Excel.Worksheet worksheet3 = (Excel.Worksheet)workbook.Sheets["统计信息"];
            objArray4[1] = (int)objArray[1] / num2;
            objArray4[2] = (int)objArray[2] / num2;
            objArray4[3] = (int)objArray[3] / num2;
            objArray4[4] = objArray[4];
            for (int m = flag11; m < num3; m++)
            {
                if (objArray[m] != null)
                {
                    objArray4[m] = (int)objArray[m] / num2;
                }

            }
            for (int m = flag11; m < num3; m++)
            {
                objArray4[m] = objArray4[m].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray4[m]) / ((double)Convert.ToDouble(objArray4[1]))), 4).ToString("0.00%") + ")"; ;
                objArray[m] = objArray[m].ToString() + " (" + Math.Round((double)(Convert.ToDouble(objArray[m]) / ((double)Convert.ToDouble(objArray[1]))), 4).ToString("0.00%") + ")"; ;
            }


            worksheet3.get_Range(worksheet3.Cells[(num2 + 2) + 8, 1], worksheet3.Cells[(num2 + 2) + 8, num3]).Value2 = objArray4;
            worksheet3.get_Range(worksheet3.Cells[(num2 + 3) + 8, 1], worksheet3.Cells[(num2 + 3) + 8, num3]).Value2 = objArray;
            ////////////////////////////////////////////////////////////////////////////////////////////

            this.ResultFileName = this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo + @"\" + this.LotNo + ".xlsx";
            worksheet3.Activate();
            // workbook.SaveAs(this.ResultFileName, Excel.XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, Excel.XlSaveAsAccessMode.xlNoChange, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            workbook.SaveAs(this.ResultFileName, 51);
            excel = null;
            workbook = null;
            application.Quit();
            application = null;
            return true;

        }

        private void sINF合并TSKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SINF_MERGE_TSK().ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            if (this.lsvItems.Items.Count <= 0)
            {
                MessageBox.Show("There is no TSK file!Please Load TSK files first!");
            }
            else
            {
                if (!Directory.Exists(this.textBox1.Text + @"\MapMergeFile\" + this.LotNo))
                {
                    Directory.CreateDirectory(this.textBox1.Text + @"\MapMergeFile\" + this.LotNo);
                }
                else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                this.progressBar1.Maximum = this.lsvItems.Items.Count;
                this.progressBar1.Value = 0;
                this.ExpAllMapMerge();
                if (MessageBox.Show("Export EXCEL File Success!Would you like to open it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(this.ResultFileName);
                }
            }


        }

        private bool ExpAllMapMerge()
        {

            int num2;
            Excel.Application application = new Excel.ApplicationClass();
            application.Visible = false;
            object updateLinks = Missing.Value;
            DateTime now = DateTime.Now;
            Excel.Workbook workbook = application.Workbooks._Open(this.FilePath + @"\AllMerge.xlsx", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            OperateExcel excel = new OperateExcel(workbook);

            Excel.Worksheet MapSheet = (Excel.Worksheet)workbook.Sheets[1];
            MapSheet.Columns.ColumnWidth = 3.25;
            MapSheet.Rows.RowHeight = 22.5;
            object[,] aryTP = (object[,])(MapSheet.get_Range("A1:SR512", Missing.Value).Value2);

            count = this.lsvItems.Items.Count;
            for (num2 = 0; num2 <= (count - 1); num2++)
            {
                string str = this.lsvItems.Items[num2].SubItems[1].Text; //文件的路径
                ///////-------------------------------TSK读取-------------------------//////

                FileStream fs_1;
                fs_1 = new FileStream(str, FileMode.Open);
                BinaryReader br_1 = new BinaryReader(fs_1);

                ///TSK1头文件-------------------------------------------------------//

                //Operator Size 20
                string Operator_1 = Encoding.ASCII.GetString(br_1.ReadBytes(20)).Trim();
                //Device Size 16
                string Device_1 = Encoding.ASCII.GetString(br_1.ReadBytes(16)).Trim();
                //WaferSize Size 2
                byte[] WaferSize_1 = br_1.ReadBytes(2);
                this.Reverse(ref WaferSize_1);
                int TSKWafersize1 = BitConverter.ToInt16(WaferSize_1, 0);
                //MachineNo Size2
                byte[] MachineNo_1 = br_1.ReadBytes(2);
                //IndexSizeX Size4
                byte[] IndexSizeX_1 = br_1.ReadBytes(4);
                //IndexSizeY Size4
                byte[] IndexSizeY_1 = br_1.ReadBytes(4);
                //FlatDir Size2
                byte[] FlatDir_1 = br_1.ReadBytes(2);
                this.Reverse(ref FlatDir_1);
                int TSKFlat1 = BitConverter.ToInt16(FlatDir_1, 0);
                //MachineType Size1
                byte MachineType_1 = br_1.ReadByte();
                //MapVersion Size1
                byte MapVersion_1 = br_1.ReadByte();
                //row Size2
                byte[] row_1 = br_1.ReadBytes(2);
                //col Size2
                byte[] col_1 = br_1.ReadBytes(2);
                //MapDataForm Size4
                byte[] MapDataForm_1 = br_1.ReadBytes(4);
                //WaferID Size21
                string WaferID_1 = Encoding.ASCII.GetString(br_1.ReadBytes(21)).Trim();
                //ProbingNo Size1
                byte ProbingNo_1 = br_1.ReadByte();
                //LotNo Size18
                string LotNo_1 = Encoding.ASCII.GetString(br_1.ReadBytes(18)).Trim();
                //CassetteNo Size2
                byte[] CN_1 = br_1.ReadBytes(2);
                this.Reverse(ref CN_1);
                int CassetteNo_1 = BitConverter.ToInt16(CN_1, 0);

                //SlotNo Size2
                byte[] SN_1 = br_1.ReadBytes(2);
                this.Reverse(ref SN_1);
                int SlotNo_1 = BitConverter.ToInt16(SN_1, 0);
                //X axis coordinates increase direction Size1
                byte IdeX_1 = br_1.ReadByte();
                //Y axis coordinates increase direction Size1
                byte IdeY_1 = br_1.ReadByte();
                //Reference die setting procedures Size1
                byte Rdsp_1 = br_1.ReadByte();
                //Reserved1 Size1
                byte Reserved1_1 = br_1.ReadByte();
                //Target die position X Size4
                byte[] Tdpx_1 = br_1.ReadBytes(4);
                //Target die position Y Size4
                byte[] Tdpy_1 = br_1.ReadBytes(4);
                //Reference die coordinator X Size2
                byte[] Rdcx_1 = br_1.ReadBytes(2);
                //Reference die coordinator Y
                byte[] Rdcy_1 = br_1.ReadBytes(2);
                // Probing start position Size1
                byte Psps_1 = br_1.ReadByte();
                //Probing direction Size1
                byte Pds_1 = br_1.ReadByte();
                //Reserved2 Size2
                byte[] Reserved2_1 = br_1.ReadBytes(2);
                //Distance X to wafer center die origin Szie4
                byte[] DistanceX_1 = br_1.ReadBytes(4);
                //Distance Y to wafer center die origin Size4
                byte[] DistanceY_1 = br_1.ReadBytes(4);
                //Coordinator X of wafer center die Size4
                byte[] CoordinatorX_1 = br_1.ReadBytes(4);
                //Coordinator Y of wafer center die Size4
                byte[] CoordinatorY_1 = br_1.ReadBytes(4);
                //First Die Coordinator X Size4
                byte[] FdcX_1 = br_1.ReadBytes(4);
                //First Die Coordinator Y Size4
                byte[] FdcY_1 = br_1.ReadBytes(4);
                //Wafer Testing Start Time Data Size12
                byte[] WTSTime_1 = br_1.ReadBytes(12);
                //Wafer Testing End Time Data Size12
                byte[] WTETime_1 = br_1.ReadBytes(12);
                //Wafer Loading Time Data Size 12
                byte[] WLTime_1 = br_1.ReadBytes(12);
                //Wafer Unloading Time Data Size12
                byte[] WULT_1 = br_1.ReadBytes(12);
                //Machine No1 Size4
                byte[] MachineNo1_1 = br_1.ReadBytes(4);
                //Machine No2 Size4
                byte[] MachineNo2_1 = br_1.ReadBytes(4);

                // Special Characters Size4
                byte[] SpecialChar_1 = br_1.ReadBytes(4);
                //Testing End Information Size1
                byte TestEndInfo_1 = br_1.ReadByte();
                //Reserved3 Size1
                byte Reserved3_1 = br_1.ReadByte();
                //Total tested dice Size2
                byte[] Totaldice_1 = br_1.ReadBytes(2);
                //Total pass dice Size2
                byte[] TotalPdice_1 = br_1.ReadBytes(2);
                //Total fail dice Size2
                byte[] TotalFdice_1 = br_1.ReadBytes(2);
                //Test Die Information Address Size4
                byte[] TDIAdress_1 = br_1.ReadBytes(4);
                //Number of line category data Size4
                byte[] NumberCategory_1 = br_1.ReadBytes(4);
                //Line category address Size4
                byte[] LineCategory_1 = br_1.ReadBytes(4);
                // Map File Configuration Size2
                byte[] MapConfig_1 = br_1.ReadBytes(2);
                // Max. Multi Site Size2
                byte[] MMSite_1 = br_1.ReadBytes(2);
                //Max. Categories Size2
                byte[] MCategory_1 = br_1.ReadBytes(2);
                //Do not use,Reserved4 Size2
                byte[] Reserved4_1 = br_1.ReadBytes(2);
                ////////Die 信息/////////////////////

                int row1_1 = ByteToInt16(ref row_1);
                int col1_1 = ByteToInt16(ref col_1);

                ArrayList arryfirstbyte1_1 = new ArrayList();
                ArrayList arryfirstbyte2_1 = new ArrayList();
                ArrayList arrysecondbyte1_1 = new ArrayList();
                ArrayList arrysecondbyte2_1 = new ArrayList();
                ArrayList arrythirdbyte1_1 = new ArrayList();
                ArrayList arrythirdbyte2_1 = new ArrayList();

                for (int k = 0; k < row1_1 * col1_1; k++)
                {
                    arryfirstbyte1_1.Add(br_1.ReadByte());
                    arryfirstbyte2_1.Add(br_1.ReadByte());
                    arrysecondbyte1_1.Add(br_1.ReadByte());
                    arrysecondbyte2_1.Add(br_1.ReadByte());
                    arrythirdbyte1_1.Add(br_1.ReadByte());
                    arrythirdbyte2_1.Add(br_1.ReadByte());

                }


                ArrayList arry_1 = new ArrayList();


                while (br_1.BaseStream.Position < br_1.BaseStream.Length)
                {
                    arry_1.Add(br_1.ReadByte());
                }

                br_1.Close();
                fs_1.Close();

                byte[] firstbyte1_1 = (byte[])arryfirstbyte1_1.ToArray(typeof(byte));
                byte[] firstbyte2_1 = (byte[])arryfirstbyte2_1.ToArray(typeof(byte));

                byte[] secondbyte1_1 = (byte[])arrysecondbyte1_1.ToArray(typeof(byte));
                byte[] secondbyte2_1 = (byte[])arrysecondbyte2_1.ToArray(typeof(byte));

                byte[] thirdbyte1_1 = (byte[])arrythirdbyte1_1.ToArray(typeof(byte));
                byte[] thirdbyte2_1 = (byte[])arrythirdbyte2_1.ToArray(typeof(byte));

                for (int i = 0; i < col1_1; i++)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        if ((secondbyte1_1[j + i * row1_1] & 192) == 0)//Skip Die
                        {
                            if (Convert.ToInt32(secondbyte1_1[j + i * row1_1]) == 2)
                            {
                                aryTP[i + 1, j + 1] = null;
                            }

                            if (Convert.ToInt32(secondbyte1_1[j + i * row1_1]) == 0)
                            {
                                aryTP[i + 1, j + 1] = "S";
                            }

                        }

                        if ((secondbyte1_1[j + i * row1_1] & 192) == 128)//Mark Die
                        {
                            aryTP[i + 1, j + 1] = "M";

                        }

                        if ((secondbyte1_1[j + i * row1_1] & 192) == 64)//Probe Die
                        {

                            if ((firstbyte1_1[j + i * row1_1] & 64) == 64)//PASS
                            {
                                if (aryTP[i + 1, j + 1] == null)
                                {
                                    aryTP[i + 1, j + 1] = 0;
                                }
                                else
                                {
                                    aryTP[i + 1, j + 1] = Convert.ToInt32(aryTP[i + 1, j + 1]) + 0;
                                }
                            }

                            if ((firstbyte1_1[j + i * row1_1] & 128) == 128)//FAIL
                            {
                                if (aryTP[i + 1, j + 1] == null)
                                {
                                    aryTP[i + 1, j + 1] = 1;
                                }
                                else
                                {
                                    aryTP[i + 1, j + 1] = Convert.ToInt32(aryTP[i + 1, j + 1]) + 1;
                                }
                            }

                        }

                    }
                }
                //------------------------------TSK1模板Read 结束------------------------------//
            }

            MapSheet.get_Range("A1:SR512", Missing.Value).Value2 = aryTP;

            this.ResultFileName = this.textBox1.Text + @"\MapMergeFile\" + this.LotNo + @"\" + this.LotNo + "堆叠" + ".xlsx";
            MapSheet.Activate();
            // workbook.SaveAs(this.ResultFileName, Excel.XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, Excel.XlSaveAsAccessMode.xlNoChange, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
            workbook.SaveAs(this.ResultFileName, 51);

            excel = null;
            workbook = null;
            application.Quit();
            application = null;


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








    }
}
