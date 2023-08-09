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
        private string WaferID;
        private string ELotNo;
        private string EWaferID;
     

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
              //  StreamWriter writer;

                string outpath = this.textBox1.Text + @"\ASCOutFile\" + this.LotNo;

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
               // Tma tma = null;

                for (num2 = 0; num2 <= (count - 1); num2++)
                {
                    // 来源 tsk 文件
                    source = this.lsvItems.Items[num2].SubItems[1].Text.Trim();

                    // 截取文件名
                   // string str = source.Substring(source.LastIndexOf(@"\") + 1).Substring(1).Replace(".", "");
                    string str = this.lsvItems.Items[num2].Text.Trim();

                    //// 执行文件格式转换
                    converter.Convert(source, outpath + @"\" +this.LotNo+"-"+str+ ".ASC");

              
                    this.progressBar1.Value++;
                }

                MessageBox.Show("OK");

             
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
                    this.WaferID = tsk.WaferID.Trim();
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
            if (this.lsvItems.Items.Count <= 0)
            {
                MessageBox.Show("there is no Excel file!Please load Excel files first!!");
            }
            else
            {
                if (!Directory.Exists(this.textBox1.Text + @"\ExcelOutFile\" + this.ELotNo))
                {
                    Directory.CreateDirectory(this.textBox1.Text + @"\ExcelOutFile\" + this.ELotNo);
                    //Directory.CreateDirectory(this.textBox1.Text + @"\TxtOutFile\" + this.ELotNo);

                }
                else if (MessageBox.Show("The folder is Existed!Do you want to cover it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                this.progressBar1.Maximum = this.lsvItems.Items.Count;
                this.progressBar1.Value = 0;

                this.ToMapping();
                if (MessageBox.Show("Export EXCEL File Success!Would you like to open it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(this.ResultFileName);
                }
            }

        }

        private bool ToMapping()
        {


            //创建Application对象

            Excel.Application xAPP = new Excel.ApplicationClass();

            xAPP.Visible = false; // 是否前台运行

            //得到WorkBook对象，可以用两种方式之一：下面是打开已有文件

            //FilePath 是程序debug目录
            Excel.Workbook xBook2 = xAPP.Workbooks._Open(this.FilePath + @"\Sample2.xls",
               Missing.Value, Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            // xBook = xApp.Workbooks.Add(Missing.Value);//新建文件的代码


            int num1 = this.lsvItems.Items.Count;

            if (num1 > 2)
            {
                for (int j = 0; j < num1 - 2; j++)
                {
                    Excel.Worksheet newWorksheet = (Excel.Worksheet)xBook2.Worksheets.Add(Missing.Value, (Excel.Worksheet)xBook2.Sheets[1], Missing.Value, Missing.Value);
                }

            }

            //指定要操作的Sheet：
            Excel.Worksheet xSheet3 = (Excel.Worksheet)xBook2.Sheets[1];//Sample2文件的Sheet1


            int Tpass = 0, Tfail = 0;

            int TBIN0 = 0, TBIN1 = 0, TBIN2 = 0, TBIN3 = 0, TBIN4 = 0, TBIN5 = 0, TBIN6 = 0, TBIN7 = 0, TBIN8 = 0, TBIN9 = 0, TBIN10 = 0, TBIN11 = 0, TBIN12 = 0, TBIN13 = 0, TBIN14 = 0, TBIN15 = 0, TBIN16 = 0, TBIN17 = 0, TBIN18 = 0,
                    TBIN19 = 0, TBIN20 = 0, TBIN21 = 0, TBIN22 = 0, TBIN23 = 0, TBIN24 = 0, TBIN25 = 0, TBIN26 = 0, TBIN27 = 0, TBIN28 = 0, TBIN29 = 0, TBIN30 = 0, TBIN31 = 0;
                

            for (int count = 2; count < num1 + 2; count++)
            {
                int pass = 0, fail = 0;

                int BIN0 = 0, BIN1 = 0, BIN2 = 0, BIN3 = 0, BIN4 = 0, BIN5 = 0, BIN6 = 0, BIN7 = 0, BIN8 = 0, BIN9 = 0, BIN10 = 0, BIN11 = 0, BIN12 = 0, BIN13 = 0, BIN14 = 0, BIN15 = 0, BIN16 = 0, BIN17 = 0, BIN18 = 0,
                    BIN19 = 0, BIN20 = 0, BIN21 = 0, BIN22 = 0, BIN23 = 0, BIN24 = 0, BIN25 = 0, BIN26 = 0, BIN27 = 0, BIN28 = 0, BIN29 = 0, BIN30 = 0, BIN31 = 0;
                
                int YMin = 200, XMin = 200;//图的边界
                Excel.Worksheet xSheet2 = (Excel.Worksheet)xBook2.Sheets[count];//Sample2文件的Sheet2

                //改变Excel宽度
                xSheet2.Columns.ColumnWidth = 1.25;

                this.FileName = this.lsvItems.Items[count - 2].SubItems[1].Text; //文件的路径
                this.EWaferID = this.lsvItems.Items[count - 2].Text.Trim(); // WaferID为文件名
                xSheet2.Name = this.lsvItems.Items[count - 2].Text.Trim(); // Sheet的名称为文件名

                Excel.Workbook xBook = xAPP.Workbooks._Open(this.FileName,
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                   Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                // 打开指定路径的文件 this.FileName


                Excel.Worksheet xSheet1 = (Excel.Worksheet)xBook.Sheets[1];//源文件的Sheet

                //Sample Sheet1工作表的内容
                Excel.Range rngLotNo = (Excel.Range)xSheet3.Cells[6 + count - 2, 1];
                rngLotNo.Value2 = Convert.ToString(this.ELotNo); //LotNo

                Excel.Range rngWaferID = (Excel.Range)xSheet3.Cells[6 + count - 2, 2];
                rngWaferID.Value2 = Convert.ToString(this.EWaferID); // WaferID



                for (int i = 13; i < 100000; i++)
                {
                    Excel.Range rng2 = (Excel.Range)xSheet1.Cells[i, 3]; //X坐标
                    Excel.Range rngX1 = (Excel.Range)xSheet1.Cells[13,3];

                    Excel.Range rng1 = (Excel.Range)xSheet1.Cells[i,4];  //Y坐标
                    Excel.Range rngY1 = (Excel.Range)xSheet1.Cells[13, 4];

                    Excel.Range rng4 = (Excel.Range)xSheet1.Cells[i,2]; //BIN值

                    if (rng1.Value2 != null)
                    {

                        Excel.Range rng3 = (Excel.Range)xSheet2.Cells[Convert.ToInt32(rngY1.Value2) + 10 - Convert.ToInt32(rng1.Value2), Convert.ToInt32(rngX1.Value2) + 70 - Convert.ToInt32(rng2.Value2)];//图的初始位置，有时需要调整

                        if (Convert.ToInt32(rng4.Value2) == 1.0)
                        {
                            rng3.Value2 = Convert.ToInt32(rng4.Value2) - 1;
                            rng3.Interior.ColorIndex = 4;

                        }
                        else
                        {
                            rng3.Value2 = Convert.ToInt32(rng4.Value2) - 1;
                            rng3.Interior.ColorIndex = 3;

                        }

                    }

                    else break;

                }


                for (int x = 1; x < 150; x++)
                {
                    for (int y = 1; y < 200; y++)
                    {

                        Excel.Range rngBIN = (Excel.Range)xSheet2.Cells[y, x];
                        if (rngBIN.Value2 != null)
                        {
                            if (YMin > y) YMin = y;
                            if (XMin > x) XMin = x;

                            switch (Convert.ToInt32(rngBIN.Value2))
                            {
                                case 0: { pass++; BIN0++; Tpass++; TBIN0++; break; }
                                case 1: { fail++; BIN1++; Tfail++; TBIN1++; break; }
                                case 2: { fail++; BIN2++; Tfail++; TBIN2++; break; }
                                case 3: { fail++; BIN3++; Tfail++; TBIN3++; break; }
                                case 4: { fail++; BIN4++; Tfail++; TBIN4++; break; }
                                case 5: { fail++; BIN5++; Tfail++; TBIN5++; break; }
                                case 6: { fail++; BIN6++; Tfail++; TBIN6++; break; }
                                case 7: { fail++; BIN7++; Tfail++; TBIN7++; break; }
                                case 8: { fail++; BIN8++; Tfail++; TBIN8++; break; }
                                case 9: { fail++; BIN9++; Tfail++; TBIN9++; break; }
                                case 10: { fail++; BIN10++; Tfail++; TBIN10++; break; }
                                case 11: { fail++; BIN11++; Tfail++; TBIN11++; break; }
                                case 12: { fail++; BIN12++; Tfail++; TBIN12++; break; }
                                case 13: { fail++; BIN13++; Tfail++; TBIN13++; break; }
                                case 14: { fail++; BIN14++; Tfail++; TBIN14++; break; }
                                case 15: { fail++; BIN15++; Tfail++; TBIN15++; break; }
                                case 16: { fail++; BIN16++; Tfail++; TBIN16++; break; }
                                case 17: { fail++; BIN17++; Tfail++; TBIN17++; break; }
                                case 18: { fail++; BIN18++; Tfail++; TBIN18++; break; }
                                case 19: { fail++; BIN19++; Tfail++; TBIN19++; break; }
                                case 20: { fail++; BIN20++; Tfail++; TBIN20++; break; }
                                case 21: { fail++; BIN21++; Tfail++; TBIN21++; break; }
                                case 22: { fail++; BIN22++; Tfail++; TBIN22++; break; }
                                case 23: { fail++; BIN23++; Tfail++; TBIN23++; break; }
                                case 24: { fail++; BIN24++; Tfail++; TBIN24++; break; }
                                case 25: { fail++; BIN25++; Tfail++; TBIN25++; break; }
                                case 26: { fail++; BIN26++; Tfail++; TBIN26++; break; }
                                case 27: { fail++; BIN27++; Tfail++; TBIN27++; break; }
                                case 28: { fail++; BIN28++; Tfail++; TBIN28++; break; }
                                case 29: { fail++; BIN29++; Tfail++; TBIN29++; break; }
                                case 30: { fail++; BIN30++; Tfail++; TBIN30++; break; }
                                case 31: { fail++; BIN31++; Tfail++; TBIN31++; break; }
                                
                            }


                        }
                    }
                }

                // 重新读图，统计信息
                #region =====================================读图，写入统计信息==============================================

                Excel.Range rngTotal = (Excel.Range)xSheet3.Cells[6 + count - 2, 3];
                rngTotal.Value2 = pass + fail;
                Excel.Range rngPass = (Excel.Range)xSheet3.Cells[6 + count - 2, 4];
                rngPass.Value2 = pass; //pass
                Excel.Range rngFail = (Excel.Range)xSheet3.Cells[6 + count - 2, 5];
                rngFail.Value2 = fail;//fail
                Excel.Range rngYield = (Excel.Range)xSheet3.Cells[6 + count - 2, 6];
                rngYield.Value2 = Math.Round(((double)pass / (double)(pass + fail)), 4).ToString("0.00%");

                Excel.Range rngBIN0 = (Excel.Range)xSheet3.Cells[6 + count - 2, 7];
                rngBIN0.Value2 = BIN0;
                Excel.Range rngBIN1 = (Excel.Range)xSheet3.Cells[6 + count - 2, 8];
                rngBIN1.Value2 = BIN1;
                Excel.Range rngBIN2 = (Excel.Range)xSheet3.Cells[6 + count - 2, 9];
                rngBIN2.Value2 = BIN2;
                Excel.Range rngBIN3 = (Excel.Range)xSheet3.Cells[6 + count - 2, 10];
                rngBIN3.Value2 = BIN3;
                Excel.Range rngBIN4 = (Excel.Range)xSheet3.Cells[6 + count - 2, 11];
                rngBIN4.Value2 = BIN4;
                Excel.Range rngBIN5 = (Excel.Range)xSheet3.Cells[6 + count - 2, 12];
                rngBIN5.Value2 = BIN5;
                Excel.Range rngBIN6 = (Excel.Range)xSheet3.Cells[6 + count - 2, 13];
                rngBIN6.Value2 = BIN6;
                Excel.Range rngBIN7 = (Excel.Range)xSheet3.Cells[6 + count - 2, 14];
                rngBIN7.Value2 = BIN7;
                Excel.Range rngBIN8 = (Excel.Range)xSheet3.Cells[6 + count - 2, 15];
                rngBIN8.Value2 = BIN8;
                Excel.Range rngBIN9 = (Excel.Range)xSheet3.Cells[6 + count - 2, 16];
                rngBIN9.Value2 = BIN9;
                Excel.Range rngBIN10 = (Excel.Range)xSheet3.Cells[6 + count - 2, 17];
                rngBIN10.Value2 = BIN10;
                Excel.Range rngBIN11 = (Excel.Range)xSheet3.Cells[6 + count - 2, 18];
                rngBIN11.Value2 = BIN11;
                Excel.Range rngBIN12 = (Excel.Range)xSheet3.Cells[6 + count - 2, 19];
                rngBIN12.Value2 = BIN12;
                Excel.Range rngBIN13 = (Excel.Range)xSheet3.Cells[6 + count - 2, 20];
                rngBIN13.Value2 = BIN13;
                Excel.Range rngBIN14 = (Excel.Range)xSheet3.Cells[6 + count - 2, 21];
                rngBIN14.Value2 = BIN14;
                Excel.Range rngBIN15 = (Excel.Range)xSheet3.Cells[6 + count - 2, 22];
                rngBIN15.Value2 = BIN15;
                Excel.Range rngBIN16 = (Excel.Range)xSheet3.Cells[6 + count - 2, 23];
                rngBIN16.Value2 = BIN16;
                Excel.Range rngBIN17 = (Excel.Range)xSheet3.Cells[6 + count - 2, 24];
                rngBIN17.Value2 = BIN17;
                Excel.Range rngBIN18 = (Excel.Range)xSheet3.Cells[6 + count - 2, 25];
                rngBIN18.Value2 = BIN18;
                Excel.Range rngBIN19 = (Excel.Range)xSheet3.Cells[6 + count - 2,26];
                rngBIN19.Value2 = BIN19;
                Excel.Range rngBIN20 = (Excel.Range)xSheet3.Cells[6 + count - 2,27];
                rngBIN20.Value2 = BIN20;
                Excel.Range rngBIN21 = (Excel.Range)xSheet3.Cells[6 + count - 2, 28];
                rngBIN21.Value2 = BIN21;
                Excel.Range rngBIN22 = (Excel.Range)xSheet3.Cells[6 + count - 2, 29];
                rngBIN22.Value2 = BIN22;
                Excel.Range rngBIN23 = (Excel.Range)xSheet3.Cells[6 + count - 2, 30];
                rngBIN23.Value2 = BIN23;
                Excel.Range rngBIN24 = (Excel.Range)xSheet3.Cells[6 + count - 2, 31];
                rngBIN24.Value2 = BIN24;
                Excel.Range rngBIN25 = (Excel.Range)xSheet3.Cells[6 + count - 2, 32];
                rngBIN25.Value2 = BIN25;
                Excel.Range rngBIN26 = (Excel.Range)xSheet3.Cells[6 + count - 2, 33];
                rngBIN26.Value2 = BIN26;
                Excel.Range rngBIN27 = (Excel.Range)xSheet3.Cells[6 + count - 2, 34];
                rngBIN27.Value2 = BIN27;
                Excel.Range rngBIN28 = (Excel.Range)xSheet3.Cells[6 + count - 2, 35];
                rngBIN28.Value2 = BIN28;
                Excel.Range rngBIN29 = (Excel.Range)xSheet3.Cells[6 + count - 2, 36];
                rngBIN29.Value2 = BIN29;
                Excel.Range rngBIN30 = (Excel.Range)xSheet3.Cells[6 + count - 2, 37];
                rngBIN30.Value2 = BIN30;
                Excel.Range rngBIN31 = (Excel.Range)xSheet3.Cells[6 + count - 2, 38];
                rngBIN31.Value2 = BIN31;

                #endregion


                for (int m = 0; m < YMin - 2; m++)
                {
                    ((Range)xSheet2.Rows[1, Missing.Value]).Delete(XlDeleteShiftDirection.xlShiftUp);
                } //删除行

                for (int n = 0; n < XMin - 2; n++)
                {
                    ((Range)xSheet2.Columns[1, Missing.Value]).Delete(XlDeleteShiftDirection.xlShiftToLeft);

                } //删除列


                #region =====================================将Mapping图转换成TXT格式部分====================================
                //    StreamWriter writer;
                //    writer = File.CreateText(this.textBox1.Text+@"\TxtOutFile\"+this.LotNo+@"\"+xSheet2.Name+".txt");
                //     writer.Write("     ");
                //for (int i = 0; i < 150; i++)
                //{
                //    int num5 = i + 1;
                //    writer.Write(num5.ToString("00") + " ");
                //}
                //writer.WriteLine("");
                //writer.Write("     ");
                //for (int j = 0; j < 150; j++)
                //{
                //    writer.Write("++-");
                //}
                //for (int k = 0; k < 150; k++)
                //{
                //    writer.WriteLine("");
                //    writer.Write((k + 1).ToString("000") + "|");
                //    for (int m = 0; m < 150; m++)
                //    {
                //        Excel.Range rng = (Excel.Range)xSheet2.Cells[k + 1, m + 1];
                //        if (rng.Value2 == null) { writer.Write(".  "); }
                //        else if (Convert.ToString(rng.Value2) == "M") { writer.Write("M  "); }
                //        else if (Convert.ToString(rng.Value2) == "S") { writer.Write("S  "); }
                //        else if (Convert.ToInt32(rng.Value2) == 0) { writer.Write("P  "); }
                //        else writer.Write("F  ");

                //    }

                //}

                //writer.WriteLine("   ");
                //writer.WriteLine("   ");
                //writer.WriteLine("   ");
                //writer.WriteLine("===================Wafer Information()================");
                //writer.WriteLine("   Lot NO: " + this.LotNo);
                //writer.WriteLine("   Slot NO: " );
                //writer.WriteLine("   Wafer ID: " + this.WaferID);
                //writer.WriteLine("   Operater: " );
                //writer.WriteLine("   Wafer Size: ");
                //writer.WriteLine("   Flat Dir: " + 180);
                //writer.WriteLine("   Wafer Test Start Time " );
                //writer.WriteLine("   Wafer Test Finish Time " );
                //writer.WriteLine("   Wafer Load Time " );
                //writer.WriteLine("   Wafer Unload Time " );
                //writer.WriteLine("   Toatl Test Die: " + (pass+fail));
                //writer.WriteLine("   Pass Die: " +pass);
                //writer.WriteLine("   Fail Die: "+fail);
                //writer.WriteLine("   Yield: " + Math.Round(((double)pass / (double)(pass + fail)), 4).ToString("0.00%") );
                //writer.WriteLine("   Sample marking:");
                //    writer.Close();

                #endregion

                this.progressBar1.Value++;//进度条进度

            }

            #region  ========================================== 统计批良率TOTAL===============================================
            Excel.Range rngTT = (Excel.Range)xSheet3.Cells[6 + num1, 1];
            rngTT.Value2 = "TOTAL";

            Excel.Range rngTTotal = (Excel.Range)xSheet3.Cells[6 +num1, 3];
            rngTTotal.Value2 = Tpass + Tfail;
            Excel.Range rngTPass = (Excel.Range)xSheet3.Cells[6 + num1, 4];
            rngTPass.Value2 = Tpass; //pass
            Excel.Range rngTFail = (Excel.Range)xSheet3.Cells[6 + num1, 5];
            rngTFail.Value2 = Tfail;//fail
            Excel.Range rngTYield = (Excel.Range)xSheet3.Cells[6 + num1, 6];
            rngTYield.Value2 = Math.Round(((double)Tpass / (double)(Tpass + Tfail)), 4).ToString("0.00%");

            Excel.Range rngTBIN0 = (Excel.Range)xSheet3.Cells[6 + num1, 7];
            rngTBIN0.Value2 = TBIN0;
            Excel.Range rngTBIN1 = (Excel.Range)xSheet3.Cells[6 + num1, 8];
            rngTBIN1.Value2 = TBIN1;
            Excel.Range rngTBIN2 = (Excel.Range)xSheet3.Cells[6 + num1, 9];
            rngTBIN2.Value2 = TBIN2;
            Excel.Range rngTBIN3 = (Excel.Range)xSheet3.Cells[6 + num1, 10];
            rngTBIN3.Value2 = TBIN3;
            Excel.Range rngTBIN4 = (Excel.Range)xSheet3.Cells[6 + num1, 11];
            rngTBIN4.Value2 = TBIN4;
            Excel.Range rngTBIN5 = (Excel.Range)xSheet3.Cells[6 + num1, 12];
            rngTBIN5.Value2 = TBIN5;
            Excel.Range rngTBIN6 = (Excel.Range)xSheet3.Cells[6 + num1, 13];
            rngTBIN6.Value2 = TBIN6;
            Excel.Range rngTBIN7 = (Excel.Range)xSheet3.Cells[6 + num1, 14];
            rngTBIN7.Value2 = TBIN7;
            Excel.Range rngTBIN8 = (Excel.Range)xSheet3.Cells[6 + num1, 15];
            rngTBIN8.Value2 = TBIN8;
            Excel.Range rngTBIN9 = (Excel.Range)xSheet3.Cells[6 + num1, 16];
            rngTBIN9.Value2 = TBIN9;
            Excel.Range rngTBIN10 = (Excel.Range)xSheet3.Cells[6 + num1, 17];
            rngTBIN10.Value2 = TBIN10;
            Excel.Range rngTBIN11 = (Excel.Range)xSheet3.Cells[6 + num1, 18];
            rngTBIN11.Value2 = TBIN11;
            Excel.Range rngTBIN12 = (Excel.Range)xSheet3.Cells[6 + num1, 19];
            rngTBIN12.Value2 = TBIN12;
            Excel.Range rngTBIN13 = (Excel.Range)xSheet3.Cells[6 + num1, 20];
            rngTBIN13.Value2 = TBIN13;
            Excel.Range rngTBIN14 = (Excel.Range)xSheet3.Cells[6 + num1, 21];
            rngTBIN14.Value2 = TBIN14;

            Excel.Range rngTBIN15 = (Excel.Range)xSheet3.Cells[6 + num1, 22];
            rngTBIN15.Value2 = TBIN15;
            Excel.Range rngTBIN16 = (Excel.Range)xSheet3.Cells[6 + num1, 23];
            rngTBIN16.Value2 = TBIN16;
            Excel.Range rngTBIN17 = (Excel.Range)xSheet3.Cells[6 + num1, 24];
            rngTBIN17.Value2 = TBIN17;
            Excel.Range rngTBIN18 = (Excel.Range)xSheet3.Cells[6 + num1, 25];
            rngTBIN18.Value2 = TBIN18;
            Excel.Range rngTBIN19 = (Excel.Range)xSheet3.Cells[6 + num1, 26];
            rngTBIN19.Value2 = TBIN19;
            Excel.Range rngTBIN20 = (Excel.Range)xSheet3.Cells[6 + num1, 27];
            rngTBIN20.Value2 = TBIN20;
            Excel.Range rngTBIN21 = (Excel.Range)xSheet3.Cells[6 + num1, 28];
            rngTBIN21.Value2 = TBIN21;
            Excel.Range rngTBIN22 = (Excel.Range)xSheet3.Cells[6 + num1, 29];
            rngTBIN22.Value2 = TBIN22;
            Excel.Range rngTBIN23 = (Excel.Range)xSheet3.Cells[6 + num1, 30];
            rngTBIN23.Value2 = TBIN23;
            Excel.Range rngTBIN24 = (Excel.Range)xSheet3.Cells[6 + num1, 31];
            rngTBIN24.Value2 = TBIN24;
            Excel.Range rngTBIN25 = (Excel.Range)xSheet3.Cells[6 + num1, 32];
            rngTBIN25.Value2 = TBIN25;
            Excel.Range rngTBIN26 = (Excel.Range)xSheet3.Cells[6 + num1, 33];
            rngTBIN26.Value2 = TBIN26;
            Excel.Range rngTBIN27 = (Excel.Range)xSheet3.Cells[6 + num1, 34];
            rngTBIN27.Value2 = TBIN27;
            Excel.Range rngTBIN28 = (Excel.Range)xSheet3.Cells[6 + num1, 35];
            rngTBIN28.Value2 = TBIN28;
            Excel.Range rngTBIN29 = (Excel.Range)xSheet3.Cells[6 + num1, 36];
            rngTBIN29.Value2 = TBIN29;
            Excel.Range rngTBIN30 = (Excel.Range)xSheet3.Cells[6 + num1, 37];
            rngTBIN30.Value2 = TBIN30;
            Excel.Range rngTBIN31 = (Excel.Range)xSheet3.Cells[6 + num1, 38];
            rngTBIN31.Value2 = TBIN31;

           

            #endregion


            this.ResultFileName = this.textBox1.Text + @"\ExcelOutFile\" + this.ELotNo + @"\" + this.ELotNo + ".xls";//以ELotID命名Excel

            //xSheet3.Name = "统计信息";

            //xSheet1.Delete();



            //保存方式一： 保存WorkBook
            xBook2.SaveAs(this.ResultFileName,
              Excel.XlFileFormat.xlWorkbookNormal, Missing.Value, Missing.Value, Missing.Value,
               Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
               Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            xBook2 = null;
            xAPP.Quit();
            xAPP = null;

            return true;

        }

        #endregion

    }
}
