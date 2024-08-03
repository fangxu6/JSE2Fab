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
using Excel;
using System.Reflection;
using System.Collections;
using MiniExcelLibs;
using System.Linq;
using DataTable = System.Data.DataTable;

//using Jcap.MappingConverter;

namespace DataToExcel
{
    public partial class MappingToExcel : Form
    {
        // Fields
        private IMappingFile _currFile;
        private FieldsProp Field;
        private ArrayList FieldsArray;
        private string FilePath = System.Windows.Forms.Application.StartupPath;
        private string LotNo;
        private string ResultFileName;
        private string FileName;
        private string EWaferID;
        private string ELotNo;
        private string Enter1 = new string((char)13, 1) + new string((char)10, 1);
        //private FileStream _writer;


        //private readonly string Enter1 = new string((char)13, 1) + new string((char)10, 1);

        //protected virtual void WriteString(string str)
        //{
        //    if (!this._writer.CanWrite)
        //        throw new Exception( " can't be writen.");

        //    this._writer.Write(System.Text.Encoding.ASCII.GetBytes(str), 0, str.Length);
        //}

        // Methods
        public MappingToExcel()
        {
            this.InitializeComponent();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }



        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (this.lsvItems.Items.Count <= 0)
        //    {
        //        MessageBox.Show("There is no TSK file!Please Load TSK files first!");
        //    }
        //    else
        //    {
        //        if (!Directory.Exists(this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo))
        //        {
        //            Directory.CreateDirectory(this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo);
        //        }
        //        else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
        //        {
        //            return;
        //        }
        //        this.progressBar1.Maximum = this.lsvItems.Items.Count;
        //        this.progressBar1.Value = 0;
        //        this.ExpDataToExcel();
        //        if (MessageBox.Show("Export EXCEL File Success!Would you like to open it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            Process.Start(this.ResultFileName);
        //        }
        //    }
        //}

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

        //private bool ExpDataToExcel()
        //{
        //    int num2;
        //    Excel.Application application = new Excel.ApplicationClass();
        //    application.Visible = false;
        //    object updateLinks = Missing.Value;
        //    DateTime now = DateTime.Now;
        //    Excel.Workbook workbook = application.Workbooks._Open(this.FilePath + @"\Sample.xls", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
        //    OperateExcel excel = new OperateExcel(workbook);



        //    int count = this.lsvItems.Items.Count;
        //    for (num2 = 0; num2 <= (count - 2); num2++)
        //    {
        //        excel.Copy("Sheet1", "aa");
        //        excel.Rename("Sheet1 (2)", this.lsvItems.Items[num2 + 1].Text.Trim());

        //    }
        //    excel.Rename("Sheet1", this.lsvItems.Items[0].Text.Trim());

        //    int num3 = this.FieldListBox1.CheckedItems.Count;
        //    object[] objArray = new object[num3];

        //    for (num2 = 0; num2 <= (count - 1); num2++)
        //    {
        //        this._currFile = (IMappingFile)this.lsvItems.Items[num2].Tag;
        //        Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[this.lsvItems.Items[num2].Text.Trim()];
        //        sheet.Columns.ColumnWidth = 1.25;
        //        sheet.Rows.RowHeight = (12.5 * this._currFile.DieMatrix.XMax) / ((double)this._currFile.DieMatrix.YMax);
        //        this.WriteSheet(sheet);
        //        sheet = null;

        //        Excel.Worksheet worksheet2 = (Excel.Worksheet)workbook.Sheets["Statistics"];

        //        object[] objArray2 = new object[num3];
        //        object[] objArray3 = new object[num3];

        //        for (int i = 0; i <= (num3 - 1); i++)
        //        {
        //            string str;
        //            objArray2[i] = this.FieldListBox1.CheckedItems[i].ToString();

        //            switch (this.FieldListBox1.CheckedItems[i].ToString())
        //            {
        //                case "LotNo":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).LotNo;
        //                        objArray[i] = "Total";
        //                        continue;
        //                    }
        //                case "WaferID":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).WaferID;
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "Total":
        //                    {
        //                        objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow | DieCategory.FailDie | DieCategory.PassDie);
        //                        if (objArray[i] == null)
        //                        {
        //                            break;
        //                        }
        //                        if (objArray3[i] != null)
        //                        {
        //                            objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
        //                        }
        //                        continue;
        //                    }
        //                case "Pass":
        //                    {
        //                        objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefPass | DieCategory.PassDie);
        //                        if (objArray[i] == null)
        //                        {
        //                            goto Label_0458;
        //                        }
        //                        if (objArray3[i] != null)
        //                        {
        //                            objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
        //                        }
        //                        continue;
        //                    }
        //                case "Fail":
        //                    {
        //                        objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.FailDie);
        //                        if (objArray[i] == null)
        //                        {
        //                            goto Label_04C5;
        //                        }
        //                        if (objArray3[i] != null)
        //                        {
        //                            objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
        //                        }
        //                        continue;
        //                    }
        //                case "Yield":
        //                    if ((objArray3[i - 2] == null) || (objArray3[i - 3] == null))
        //                    {
        //                        goto Label_0527;
        //                    }
        //                    objArray3[i] = Math.Round((double)(Convert.ToDouble(objArray3[i - 2]) / ((double)Convert.ToInt32(objArray3[i - 3]))), 4).ToString("0.00%");
        //                    goto Label_0531;

        //                case "Index X":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).IndexSizeX;
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "Index Y":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).IndexSizeY;
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "Wafer Size":
        //                    {
        //                        try
        //                        {
        //                            objArray3[i] = ((Convert.ToInt32(((Tsk)this._currFile).WaferSize) / 10)).ToString() + "inch";
        //                        }
        //                        catch
        //                        {
        //                            objArray3[i] = "";
        //                        }
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "OF Direction":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).FlatDir;
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "LoadTime":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).LoadTime.ToString();
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "UnloadTime":
        //                    {
        //                        objArray3[i] = ((Tsk)this._currFile).UnloadTime.ToString();
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "UsedTime":
        //                    {
        //                        objArray3[i] = ((TimeSpan)(((Tsk)this._currFile).UnloadTime - ((Tsk)this._currFile).LoadTime)).ToString();
        //                        objArray[i] = "";
        //                        continue;
        //                    }
        //                case "BIN 0":
        //                    {
        //                        objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefPass | DieCategory.PassDie);
        //                        if (objArray[i] == null)
        //                        {
        //                            goto Label_076F;
        //                        }
        //                        if (objArray3[i] != null)
        //                        {
        //                            objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
        //                        }
        //                        continue;
        //                    }
        //                default:
        //                    goto Label_077E;
        //            }
        //            objArray[i] = objArray3[i];
        //            continue;
        //        Label_0458:
        //            objArray[i] = objArray3[i];
        //            continue;
        //        Label_04C5:
        //            objArray[i] = objArray3[i];
        //            continue;
        //        Label_0527:
        //            objArray3[i] = "";
        //        Label_0531:
        //            if ((objArray[i - 2] != null) && (objArray[i - 3] != null))
        //            {
        //                objArray[i] = Math.Round(Convert.ToDouble((double)(Convert.ToDouble(objArray[i - 2]) / ((double)((int)objArray[i - 3])))), 4).ToString("0.00%");
        //            }
        //            else
        //            {
        //                objArray[i] = "";
        //            }
        //            continue;
        //        Label_076F:
        //            objArray[i] = objArray3[i];
        //            continue;
        //        Label_077E:
        //            str = this.FieldListBox1.CheckedItems[i].ToString().Trim();
        //            if (str.Substring(0, str.LastIndexOf(" ")).Trim() == "BIN")
        //            {
        //                str = str.Substring(str.LastIndexOf(" ")).Trim();
        //                objArray3[i] = ToCountDie._ToCountDie[int.Parse(str)];
        //                if (objArray[i] != null)
        //                {
        //                    if (objArray3[i] != null)
        //                    {
        //                        objArray[i] = ((int)objArray[i]) + ((int)objArray3[i]);
        //                    }
        //                }
        //                else
        //                {
        //                    objArray[i] = ToCountDie._ToCountDie[int.Parse(str)];
        //                }
        //            }
        //            else
        //            {
        //                objArray3[i] = "??";
        //                objArray[i] = "??";
        //            }
        //        }

        //        worksheet2.get_Range(worksheet2.Cells[5, 1], worksheet2.Cells[5, num3]).Value2 = objArray2;
        //        worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 5, 1], worksheet2.Cells[(num2 + 1) + 5, num3]).Value2 = objArray3;
        //        worksheet2.get_Range(worksheet2.Cells[(num2 + 2) + 5, 1], worksheet2.Cells[(num2 + 2) + 5, num3]).Value2 = objArray;
        //        worksheet2 = null;

        //        this.progressBar1.Value++;
        //    }

        //    this.ResultFileName = this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo + @"\" + this.LotNo + ".xls";
        //    workbook.SaveAs(this.ResultFileName, Excel.XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, Excel.XlSaveAsAccessMode.xlNoChange, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);

        //    excel = null;
        //    workbook = null;
        //    application.Quit();
        //    application = null;
        //    return true;
        //}

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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.lsvItems.Items.Clear();
                foreach (string str in dialog.FileNames)
                {
                    this.textBox1.Text = Path.GetDirectoryName(str);
                    Tsk tsk = new Tsk(str);
                    tsk.Read();
                    this.LotNo = tsk.LotNo.Trim();
                    ListViewItem item = new ListViewItem(tsk.WaferID);
                    item.Tag = tsk;
                    this.lsvItems.Items.Add(item);
                    item.SubItems.Add(str);


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




        #region  ==============================================EXCEL TO Mapping ===============================

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadExcelFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
        private void LoadExcelFile()
        {
            this.LoadExcel();
        }



        private void LoadExcel()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.lsvItems.Items.Clear();


                foreach (string str in dialog.FileNames)
                {
                    this.textBox1.Text = Path.GetDirectoryName(str);
                    //this.FileName = str;  // 打开文件的路径

                    //this.WaferID = Path.GetFileNameWithoutExtension(str); //不带后缀显示文件名称

                    ListViewItem item = new ListViewItem((Path.GetFileNameWithoutExtension(str)));//显示文件名称  dialog.FileName是文件路径

                    this.lsvItems.Items.Add(item);
                    item.SubItems.Add(str);

                }
            }
        }




        private void button8_Click(object sender, EventArgs e)
        {
            this.ELotNo = Path.GetFileName(this.textBox1.Text); //读出LotNo，为目标文件夹名称
            this.ELotNo = "aaa";
            this.textBox1.Text = "C:\\Users\\fangx\\Desktop\\待恢复数据文件";
            if (this.lsvItems.Items.Count < 0)
            {
                MessageBox.Show("there is no Excel file!Please load Excel files first!!");
            }
            else
            {
                if (!Directory.Exists(this.textBox1.Text + @"\ExcelOutFile\" + this.ELotNo))
                {
                    Directory.CreateDirectory(this.textBox1.Text + @"\ExcelOutFile\" + this.ELotNo);


                }
                if (!Directory.Exists(this.textBox1.Text + @"\TxtOutFile\" + this.ELotNo))
                {
                    Directory.CreateDirectory(this.textBox1.Text + @"\TxtOutFile\" + this.ELotNo);

                }

                else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                this.progressBar1.Maximum = this.lsvItems.Items.Count;
                this.progressBar1.Value = 0;

                this.ToMapping();
                MessageBox.Show("ok");
            }

        }

        
        private bool ToMapping()
        {


            //创建Application对象

            Excel.Application xAPP = new Excel.ApplicationClass();

            xAPP.Visible = false; // 是否前台运行

            //得到WorkBook对象，可以用两种方式之一：下面是打开已有文件
            int num1 = this.lsvItems.Items.Count;
            num1 = 1;

            for (int count = 2; count < num1 + 2; count++)
            {

                //this.FileName = this.lsvItems.Items[count - 2].SubItems[1].Text; //文件的路径
                string path = @"C:\Users\fangx\Desktop\图谱恢复\待恢复.xlsx";
                this.FileName =  path;

                Excel.Workbook xBook = xAPP.Workbooks._Open(this.FileName,
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                // 打开指定路径的文件 this.FileName

                var table = MiniExcel.QueryAsDataTable(path, useHeaderRow: false);





                Excel.Worksheet xSheet1 = (Excel.Worksheet)xBook.Sheets[1];//源文件的Sheet


                object[,] aryX = (object[,])(xSheet1.get_Range("F1:F150", Missing.Value).Value2);
                object[,] aryY = (object[,])(xSheet1.get_Range("G1:G150", Missing.Value).Value2);
                object[,] aryBIN = (object[,])(xSheet1.get_Range("D1:D150", Missing.Value).Value2);


                //------------TSK READ--------------------------------------------------//


                FileStream fs;
                // fs = new FileStream(arrayFilepath[i].ToString(), FileMode.Open);
                string tskFileName = "022.UQ6889-22";
                fs = new FileStream(@"C:\Users\fangx\Desktop\huangyan\UQ6889-tsk\UQ6889-1\" + tskFileName, FileMode.Open,FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                ///头文件-------------------------------------------------------//

                //Operator Size 20
                string Operator = Encoding.ASCII.GetString(br.ReadBytes(20)).Trim();
                //Device Size 16
                string Device = Encoding.ASCII.GetString(br.ReadBytes(16)).Trim();
                //WaferSize Size 2
                byte[] WaferSize = br.ReadBytes(2);
                //MachineNo Size2
                byte[] MachineNo = br.ReadBytes(2);
                //IndexSizeX Size4
                byte[] IndexSizeX = br.ReadBytes(4);
                //IndexSizeY Size4
                byte[] IndexSizeY = br.ReadBytes(4);
                //FlatDir Size2
                byte[] FlatDir = br.ReadBytes(2);
                //MachineType Size1
                byte MachineType = br.ReadByte();
                //MapVersion Size1
                byte MapVersion = br.ReadByte();
                //row Size2
                byte[] row = br.ReadBytes(2);
                //col Size2
                byte[] col = br.ReadBytes(2);
                //MapDataForm Size4
                byte[] MapDataForm = br.ReadBytes(4);
                //WaferID Size21
                string WaferID = Encoding.ASCII.GetString(br.ReadBytes(21)).Trim();
                //ProbingNo Size1
                byte ProbingNo = br.ReadByte();
                //LotNo Size18
                string LotNo = Encoding.ASCII.GetString(br.ReadBytes(18)).Trim();
                //CassetteNo Size2
                byte[] CN = br.ReadBytes(2);
                this.Reverse(ref CN);
                int CassetteNo = BitConverter.ToInt16(CN, 0);

                //SlotNo Size2
                byte[] SN = br.ReadBytes(2);
                this.Reverse(ref SN);
                int SlotNo = BitConverter.ToInt16(SN, 0);
                //X axis coordinates increase direction Size1
                byte IdeX = br.ReadByte();
                //Y axis coordinates increase direction Size1
                byte IdeY = br.ReadByte();
                //Reference die setting procedures Size1
                byte Rdsp = br.ReadByte();
                //Reserved1 Size1
                byte Reserved1 = br.ReadByte();
                //Target die position X Size4
                byte[] Tdpx = br.ReadBytes(4);
                //Target die position Y Size4
                byte[] Tdpy = br.ReadBytes(4);
                //Reference die coordinator X Size2
                byte[] Rdcx = br.ReadBytes(2);
                //Reference die coordinator Y
                byte[] Rdcy = br.ReadBytes(2);
                // Probing start position Size1
                byte Psps = br.ReadByte();
                //Probing direction Size1
                byte Pds = br.ReadByte();
                //Reserved2 Size2
                byte[] Reserved2 = br.ReadBytes(2);
                //Distance X to wafer center die origin Szie4
                byte[] DistanceX = br.ReadBytes(4);
                //Distance Y to wafer center die origin Size4
                byte[] DistanceY = br.ReadBytes(4);
                //Coordinator X of wafer center die Size4
                byte[] CoordinatorX = br.ReadBytes(4);
                //Coordinator Y of wafer center die Size4
                byte[] CoordinatorY = br.ReadBytes(4);
                //First Die Coordinator X Size4
                byte[] FdcX = br.ReadBytes(4);
                //First Die Coordinator Y Size4
                byte[] FdcY = br.ReadBytes(4);
                //Wafer Testing Start Time Data Size12
                byte[] WTSTime = br.ReadBytes(12);
                //Wafer Testing End Time Data Size12
                byte[] WTETime = br.ReadBytes(12);
                //Wafer Loading Time Data Size 12
                byte[] WLTime = br.ReadBytes(12);
                //Wafer Unloading Time Data Size12
                byte[] WULT = br.ReadBytes(12);
                //Machine No1 Size4
                byte[] MachineNo1 = br.ReadBytes(4);
                //Machine No2 Size4
                byte[] MachineNo2 = br.ReadBytes(4);

                // Special Characters Size4
                byte[] SpecialChar = br.ReadBytes(4);
                //Testing End Information Size1
                byte TestEndInfo = br.ReadByte();
                //Reserved3 Size1
                byte Reserved3 = br.ReadByte();
                //Total tested dice Size2
                byte[] Totaldice = br.ReadBytes(2);
                //Total pass dice Size2
                byte[] TotalPdice = br.ReadBytes(2);
                //Total fail dice Size2
                byte[] TotalFdice = br.ReadBytes(2);
                //Test Die Information Address Size4
                byte[] TDIAdress = br.ReadBytes(4);
                //Number of line category data Size4
                byte[] NumberCategory = br.ReadBytes(4);
                //Line category address Size4
                byte[] LineCategory = br.ReadBytes(4);
                // Map File Configuration Size2
                byte[] MapConfig = br.ReadBytes(2);
                // Max. Multi Site Size2
                byte[] MMSite = br.ReadBytes(2);
                //Max. Categories Size2
                byte[] MCategory = br.ReadBytes(2);
                //Do not use,Reserved4 Size2
                byte[] Reserved4 = br.ReadBytes(2);
                ////////Die 信息/////////////////////

                int row1 = ByteToInt16(ref row);
                int col1 = ByteToInt16(ref col);


                ArrayList arryfirstbyte1 = new ArrayList();
                ArrayList arryfirstbyte2 = new ArrayList();
                ArrayList arrysecondbyte1 = new ArrayList();
                ArrayList arrysecondbyte2 = new ArrayList();
                ArrayList arrythirdbyte1 = new ArrayList();
                ArrayList arrythirdbyte2 = new ArrayList();

                for (int k = 0; k < row1 * col1; k++)
                {
                    arryfirstbyte1.Add(br.ReadByte());
                    arryfirstbyte2.Add(br.ReadByte());
                    arrysecondbyte1.Add(br.ReadByte());
                    arrysecondbyte2.Add(br.ReadByte());
                    arrythirdbyte1.Add(br.ReadByte());
                    arrythirdbyte2.Add(br.ReadByte());

                }


                ArrayList arry = new ArrayList();


                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    arry.Add(br.ReadByte());
                }

                br.Close();
                fs.Close();
                //------------------------------TSK模板Read 结束------------------------------//

                //-------------------------------------------------------写TSK MAP--------------------------------------
                FileStream fw;
                int flag2 = 0;


                fw = new FileStream("D:\\New-Tsk\\" + tskFileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fw);

                byte[] firstbyte1 = (byte[])arryfirstbyte1.ToArray(typeof(byte));
                byte[] firstbyte2 = (byte[])arryfirstbyte2.ToArray(typeof(byte));

                byte[] secondbyte1 = (byte[])arrysecondbyte1.ToArray(typeof(byte));
                byte[] secondbyte2 = (byte[])arrysecondbyte2.ToArray(typeof(byte));

                byte[] thirdbyte1 = (byte[])arrythirdbyte1.ToArray(typeof(byte));
                byte[] thirdbyte2 = (byte[])arrythirdbyte2.ToArray(typeof(byte));

                /////--------------------TSK修改BIN信息代码----------------------------------------------------

                for (int k = 0; k < row1 * col1; k++)
                {

                    if ((secondbyte1[k] & 192) == 0)//Skip Die
                    {
                        continue;

                    }

                    if ((secondbyte1[k] & 192) == 128)//Mark Die
                    {
                        continue;

                    }



                    if ((secondbyte1[k] & 192) == 64)//Probe Die
                    {

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            var aaa = table.Rows[i];
                            var x = aaa[0];
                            var y = aaa[1];
                            var binNo = aaa[2];

                            short first = (short)((firstbyte1[k] << 8) | firstbyte2[k]);//合并2位 x坐标
                            short second = (short)((secondbyte1[k] << 8) | secondbyte2[k]);//合并2位 y坐标

                            // if (((firstbyte2[k] & 255) == Convert.ToInt32(aryX[m, 1])) && ((secondbyte2[k] & 255) == Convert.ToInt32(aryY[m, 1])))
                            if (((first & 511) == Convert.ToInt32(x)) && ((second & 511) == Convert.ToInt32(y)))  //数据文件 x坐标 y坐标
                            {
                                firstbyte1[k] = Convert.ToByte((firstbyte1[k] & 1));
                                if(Convert.ToInt32(binNo) == 1)
                                {
                                    firstbyte1[k] = Convert.ToByte(firstbyte1[k] | 64); //标记为Pass
                                } else
                                {
                                    firstbyte1[k] = Convert.ToByte(firstbyte1[k] | 128); //标记为Fail
                                }
                                

                                thirdbyte2[k] = Convert.ToByte((thirdbyte2[k] & 192));
                                thirdbyte2[k] = Convert.ToByte((thirdbyte2[k] | Convert.ToInt32(binNo)));  //换category,全部换成4

                            }

                        }
                        



                    }


                }//----------------------------TSK修改BIN信息-----------------------------------------------------

                //Operator Size20
                string str = string.Format("{0,-20:G}", Operator);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 20);

                //Device Size16
                str = string.Format("{0,-16:G}", Device);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 16);

                byte[] buf;
                //WaferSize
                bw.Write(WaferSize);
                //MachineNo
                bw.Write(MachineNo);
                //IndexSizeX
                bw.Write(IndexSizeX);
                //IndexSizeY
                bw.Write(IndexSizeY);
                //FlatDir
                bw.Write(FlatDir);
                //MachineType
                bw.Write(MachineType);
                //MapVersion
                bw.Write(MapVersion);
                //Row
                bw.Write(row[1]);
                bw.Write(row[0]);
                //Col
                bw.Write(col[1]);
                bw.Write(col[0]);
                //MapDataForm
                bw.Write(MapDataForm);

                //NewWaferID
                str = string.Format("{0,-21:G}", WaferID);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 21);


                //ProbingNo
                bw.Write(BitConverter.GetBytes(ProbingNo), 0, 1);

                //NewLotNo
                str = string.Format("{0,-18:G}", LotNo);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 18);

                //CN
                buf = BitConverter.GetBytes((short)CassetteNo);
                this.Reverse(ref buf);
                bw.Write(buf, 0, 2);
                //SN
                buf = BitConverter.GetBytes((short)SlotNo);
                this.Reverse(ref buf);
                bw.Write(buf, 0, 2);
                //Idex
                bw.Write(IdeX);
                //Idey
                bw.Write(IdeY);
                //Rdsp
                bw.Write(Rdsp);
                //Reserved1
                bw.Write(Reserved1);
                //Tdpx
                bw.Write(Tdpx);
                //Tdpy
                bw.Write(Tdpy);

                //Rdcx
                bw.Write(Rdcx);
                //Rdcy
                bw.Write(Rdcy);
                //Psps
                bw.Write(Psps);
                //Pds
                bw.Write(Pds);
                //Reserved2
                bw.Write(Reserved2);
                //DistanceX
                bw.Write(DistanceX);
                //DistanceY
                bw.Write(DistanceY);

                //CoordinatorX
                bw.Write(CoordinatorX);
                //CoordinatorY
                bw.Write(CoordinatorY);
                //Fdcx
                bw.Write(FdcX);
                //Fdxy
                bw.Write(FdcY);
                //WTSTIME
                bw.Write(WTSTime);
                //WTETIME
                bw.Write(WTETime);
                //WLTIME
                bw.Write(WLTime);
                //WULT
                bw.Write(WULT);

                //MachineNo1
                bw.Write(MachineNo1);
                //MachineNo2
                bw.Write(MachineNo2);
                //Specialchar
                bw.Write(SpecialChar);
                //TestEndInfo
                bw.Write(TestEndInfo);
                //Reserved3
                bw.Write(Reserved3);
                //Totaldice
                bw.Write(Totaldice);
                //TotalPdice
                bw.Write(TotalPdice);
                //TotalFdice
                bw.Write(TotalFdice);
                //DIAdress
                bw.Write(TDIAdress);
                //Numbercategory
                bw.Write(NumberCategory);
                //Linecategory
                bw.Write(LineCategory);
                //mapconfig
                bw.Write(MapConfig);
                //mmsite
                bw.Write(MMSite);
                //mcategory
                bw.Write(MCategory);
                //Reserved4
                bw.Write(Reserved4);

                for (int k = 0; k < row1 * col1; k++)
                {
                    bw.Write(firstbyte1[k]);
                    bw.Write(firstbyte2[k]);
                    bw.Write(secondbyte1[k]);
                    bw.Write(secondbyte2[k]);
                    bw.Write(thirdbyte1[k]);
                    bw.Write(thirdbyte2[k]);


                }

                foreach (byte obj in arry)
                {
                    bw.Write(obj);

                }

                bw.Flush();
                bw.Close();
                fw.Close();



                xBook.Close();



              //  this.progressBar1.Value++;//进度条进度
                xBook = null;
                xAPP.Quit();
                xAPP = null;

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

        private void button1_Click(object sender, EventArgs e)
        {

        }


    }

}

#endregion