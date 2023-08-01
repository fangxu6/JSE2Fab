public class MappingToExcel : Form
{
    // Fields
    private IMapingFile _currFile;
    private FieldsProp Field;
    private ArrayList FieldsArray;
    private string FilePath = Application.StartupPath;
    private string LotNo;
    private string ResultFileName;

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
            string path = this.textBox1.Text + @"\TxtOutFile\" + this.LotNo + @"\Total.txt";
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
            if (!Directory.Exists(this.textBox1.Text + @"\TmaOutFile\" + this.LotNo))
            {
                Directory.CreateDirectory(this.textBox1.Text + @"\TmaOutFile\" + this.LotNo);
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
                CMDTskToTxt txt = new CMDTskToTxt();
                string str = this.lsvItems.Items[num2].SubItems[1].Text.Trim();
                str = str.Substring(str.LastIndexOf(@"\") + 1).Substring(1).Replace(".", "");
                txt.Convert(this.lsvItems.Items[num2].SubItems[1].Text.Trim(), this.textBox1.Text + @"\TmaOutFile\" + this.LotNo + @"\" + str + ".tma");
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

    private void clearFileMenuItem_Click(object sender, EventArgs e)
    {
        this._currFile = null;
        this.lsvItems.Columns[0].Text = "mapping file";
        this.lsvItems.Items.Clear();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.components != null))
        {
            this.components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void Draw(Worksheet sheet)
    {
        if (this._currFile != null)
        {
            this.DrawMatrix(sheet);
        }
    }

    private void DrawMatrix(Worksheet sheet)
    {
        this._currFile.DieMatrix.Paint(sheet, false);
    }

    private bool ExpDataToExcel()
    {
        int num2;
        Application application = new ApplicationClass();
        application.Visible = false;
        object updateLinks = Missing.Value;
        DateTime now = DateTime.Now;
        Workbook workbook = application.Workbooks._Open(this.FilePath + @"\Sample.xls", updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
        OperateExcel excel = new OperateExcel(workbook);
        int count = this.lsvItems.Items.Count;
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
            this._currFile = (IMapingFile)this.lsvItems.Items[num2].Tag;
            Worksheet sheet = (Worksheet)workbook.Sheets[this.lsvItems.Items[num2].Text.Trim()];
            sheet.Columns.ColumnWidth = 1.25;
            sheet.Rows.RowHeight = (12.5 * this._currFile.DieMatrix.XMax) / ((double)this._currFile.DieMatrix.YMax);
            this.WriteSheet(sheet);
            sheet = null;
            Worksheet worksheet2 = (Worksheet)workbook.Sheets["Statistics"];
            object[] objArray2 = new object[num3];
            object[] objArray3 = new object[num3];
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
                            objArray3[i] = this._currFile.DieMatrix.DieAttributeStat(DieCategory.TIRefPass | DieCategory.PassDie);
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
        }
        this.ResultFileName = this.textBox1.Text + @"\ExcelOutFile\" + this.LotNo + @"\" + this.LotNo + ".xls";
        workbook.SaveAs(this.ResultFileName, XlFileFormat.xlWorkbookNormal, updateLinks, updateLinks, updateLinks, updateLinks, XlSaveAsAccessMode.xlNoChange, updateLinks, updateLinks, updateLinks, updateLinks, updateLinks);
        excel = null;
        workbook = null;
        application.Quit();
        application = null;
        return true;
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
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.RestoreDirectory = false;
        dialog.Multiselect = true;
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            this.lsvItems.Items.Clear();
            foreach (string str in dialog.FileNames)
            {
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

    private void ShowTsk(Worksheet sheet)
    {
        this.Draw(sheet);
    }

    private void textBox1_Leave(object sender, EventArgs e)
    {
        this.SavePath();
    }

    private void WriteSheet(Worksheet sheet)
    {
        this.ShowTsk(sheet);
    }
}

