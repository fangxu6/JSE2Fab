

/*
 * 作者：sky
 * 时间：2008-06-25
 * 作用：用于描述 CMD 的 txt 格式的 mapping 文件
 */

namespace DataToExcel
{
    using System;
    using System.Drawing;
    using System.Collections;
    using DataToExcel;
    using System.IO;
    using System.Text.RegularExpressions;

    public class CmdTxt : MappingBase
    {
        // Fields
        public static string _Device = "";
        public static string _LotNo = "";
        public static int _singleTotalDie = 0;
        public static int _TotalDie = 0;
        public static int _TotalFailDie = 0;
        public static int _TotalPassDie = 0;
        public static string _TotalYield = "";

        // Methods
        public CmdTxt(string file)
            : base("cmdtxt", file)
        {
        }

        protected override void InitialProperties()
        {

            base._keys.Add("PassDie");
            base._keys.Add("FailDie");
            base._keys.Add("RowCount");
            base._keys.Add("ColCount");
            base._properties.Add("PassDie", 0);
            base._properties.Add("FailDie", 0);
            base._properties.Add("RowCount", 0);
            base._properties.Add("ColCount", 0);
        }

        public static bool InitTotal()
        {
            _TotalDie = 0;
            _TotalPassDie = 0;
            _TotalFailDie = 0;
            _singleTotalDie = 0;
            _TotalYield = "";
            return true;
        }

        public override bool IsEmptyDie(DieData die)
        {
            return (((die.Attribute == DieCategory.NoneDie) || (die.Attribute == DieCategory.MarkDie)) || (die.Attribute == DieCategory.SkipDie));
        }

        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            return null;
        }

        public override void Read()
        {
            throw new Exception("此类型不支持文件读取。");
        }

        public override void Save()
        {
            try
            {
                try
                {

                    if (File.Exists(base.FullName))
                    {
                        File.Delete(base.FullName);
                    }
                    base.OpenWriter();
                    this.WriteString("[BOF]" + this.Enter);

                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "PRODUCT ID", this.Device));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "LOT ID", this.LotNo));
                    this.WaferID = Regex.Replace(this.WaferID, @"\0", string.Empty);
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "WAFER ID", this.WaferID));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "FLOW ID", ""));//根据批次号可以解析

                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "START TIME", this.StartTime));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "STOP TIME", this.EndTime));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "SUBCON", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "TESTER NAME", ""));

                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "TEST PROGRAM", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "LOAD BOARD ID", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "PROBE CARD ID", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "SITE NUM", ""));

                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "DUT ID", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "DUT DIFF NUM", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "OPERATOR ID", this.Operator));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "TESTED DIE", this.PassDie + this.FailDie));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "PASS BIN", "Bin1"));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "PASS DIE", this.PassDie));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "YIELD", Math.Round(Convert.ToDouble((double)(this.PassDie / ((double)(this.PassDie + this.FailDie)))), 6).ToString("0.00%")));

                    string FlatDir1 = "";

                    if (this.FlatDir == 90)
                    {
                        FlatDir1 = "Right";
                    }

                    else if (this.FlatDir == 180)
                    {
                        FlatDir1 = "Down";
                    }
                    else if (this.FlatDir == 270)
                    {
                        FlatDir1 = "Left";
                    }
                    else if (this.FlatDir == 0)
                    {
                        FlatDir1 = "Up";
                    }

                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "SOURCE NOTCH", FlatDir1));

                    int flagbin = 0;

                    int ymin = 1000, xmin = 1000, ymax = 0, xmax = 0;
                    {
                        for (int y = 0; y < base.DieMatrix.YMax; y++)
                        {

                            for (int x = 0; x < base.DieMatrix.XMax; x++)
                            {
                                switch (base.DieMatrix[x, y].Attribute)
                                {
                                    case DieCategory.PassDie:
                                    case DieCategory.FailDie:
                                    case DieCategory.SkipDie2:
                                        if (xmin > x) { xmin = x; }
                                        if (ymin > y) { ymin = y; }
                                        if (ymax < y) { ymax = y; }
                                        if (xmax < x) { xmax = x; }
                                        int xxx = this.DieMatrix[x, y].Bin;
                                        if (xxx == 2)
                                        {
                                            flagbin = 1;
                                        }
                                        break;
                                }

                            }
                        }
                    }
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "MAP ROW", (ymax - ymin + 1)));

                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "MAP COLUMN", (xmax - xmin + 1)));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "MAPB INLENGTH", ""));
                    this.WriteString(string.Format("{0,-16}" + ": " + "{1}" + this.Enter, "SHIP", ""));

                    this.WriteString(this.Enter);

                    //Calculate bin number and yield
                    int[] binCount = new int[33]; //bin1-bin32 0不用
                    for (int i = 0; i <= 32; i++)
                    {
                        binCount[i] = 0;
                    }

                    for (int y = ymin; y < ymax + 1; y++)
                    {
                        for (int x = xmin; x < xmax + 1; x++)
                        {

                            switch (base.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        binCount[this.DieMatrix[x, y].Bin - 1]++;
                                        break;
                                    }
                                case DieCategory.FailDie:
                                    {
                                        //this.DieMatrix[x, y].Bin = this.DieMatrix[x, y].Bin - 1;  //BIN-1
                                        if (this.DieMatrix[x, y].Bin - 1 < 32)
                                        {
                                            binCount[this.DieMatrix[x, y].Bin - 1]++;
                                        }
                                        else
                                        {
                                            binCount[32]++;
                                        }
                                        break;
                                    }
                            }
                        }
                    }

                    this.WriteString("[SOFT BIN]" + this.Enter);
                    this.WriteString(string.Format("{0,7}" + "," + "{1,7}" + "," + "{2,7}" + "," + "{3,7}" + ", " + "{4,-20}" + this.Enter, "", "BINNAME", "DIENUM", "YIELD", "DESCRIPTION"));
                    char symbol = '0';
                    for (int i = 1; i < 32; i++)
                    {
                        if (i < 10)
                        {
                            symbol = (char)('0' + i);
                        }
                        else
                        {
                            symbol = (char)('0' + 7 + i);
                        }
                        this.WriteString(string.Format("{0,7}" + "," + "{1,7}" + "," + "{2,7}" + "," + "{3,7}" + ", " + "{4,-20}" + this.Enter, "BIN", i, binCount[i], Math.Round(Convert.ToDouble((double)(binCount[i] / ((double)(this.PassDie + this.FailDie)))), 6).ToString("0.00%"), "Symbol " + symbol));
                    }
                    this.WriteString(string.Format("{0,7}" + "," + "{1,7}" + "," + "{2,7}" + "," + "{3,7}" + ", " + "{4,-20}" + this.Enter, "BIN", "32", binCount[32], Math.Round(Convert.ToDouble((double)(binCount[32] / ((double)(this.PassDie + this.FailDie)))), 6).ToString("0.00%"), "Symbol X"));

                    this.WriteString(this.Enter);
                    this.WriteString("[SOFT BIN MAP]" + this.Enter);
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = xmin; x <= xmax + 1; x++)
                        {
                            if (x == xmin)
                            {
                                this.WriteString("    ");
                            }
                            else
                            {
                                if (y == 0)
                                {
                                    this.WriteString(String.Format("{0}", (x - xmin) / 100));
                                }
                                else if (y == 1)
                                {
                                    this.WriteString(String.Format("{0}", (x - xmin) / 10 % 10));
                                }
                                else
                                {
                                    this.WriteString(String.Format("{0}", (x - xmin) % 10));
                                }
                            }
                        }
                        this.WriteString(this.Enter);
                    }
                    this.WriteString(this.Enter);

                    for (int y = ymin; y < ymax + 1; y++)
                    //  for (int y = 0; y < base.DieMatrix.YMax-1; y++)
                    {
                        this.WriteString(String.Format("{0:d3}" + " ", y - ymin));
                        for (int x = xmin; x < xmax + 1; x++)
                        //  for (int x = 0; x < base.DieMatrix.XMax; x++)
                        {

                            switch (base.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        int xxx = this.DieMatrix[x, y].Bin;
                                        this.WriteString(string.Format("{0,1:G}", this.DieMatrix[x, y].Bin - 1)); //BIN-1
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                case DieCategory.SkipDie2:
                                    {

                                        this.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(base.DieMatrix[x, y].Attribute)));
                                        break;
                                    }

                                case DieCategory.FailDie:
                                    {
                                        this.DieMatrix[x, y].Bin = this.DieMatrix[x, y].Bin - 1;  //BIN-1
                                                                                                  //this.WriteString(string.Format("{0,1:G}", "X"));
                                        if (this.DieMatrix[x, y].Bin < 10)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", this.DieMatrix[x, y].Bin));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 10)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "A"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 11)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "B"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 12)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "C"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 13)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "D"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 14)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "E"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 15)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "F"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 16)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "G"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 17)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "H"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 18)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "I"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 19)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "J"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 20)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "K"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 21)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "L"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 22)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "M"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 23)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "N"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 24)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "O"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 25)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "P"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 26)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "Q"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 27)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "R"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 28)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "S"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 29)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "T"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 30)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "U"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin == 31)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "V"));
                                        }
                                        else if (this.DieMatrix[x, y].Bin >= 32)
                                        {
                                            this.WriteString(string.Format("{0,1:G}", "X"));
                                        }
                                        break;

                                    }

                            }
                        }
                        this.WriteString(this.Enter);


                    }

                    this.WriteString("[EXTENSION]" + this.Enter);
                    this.WriteString(this.Enter);
                    this.WriteString("[EOF]" + this.Enter);



                    /*

                    this.WriteString("     ");
                    for (int i = 0; i < base.DieMatrix.XMax; i++)
                    {
                        int num5 = i + 1;
                        this.WriteString(num5.ToString("00") + " ");
                    }
                    this.WriteString(base.Enter + "     ");
                    for (int j = 0; j < base.DieMatrix.XMax; j++)
                    {
                        this.WriteString("++-");
                    }
                    ToCountDie die = new ToCountDie();
                    for (int k = 0; k < base.DieMatrix.YMax; k++)
                    {
                        this.WriteString(base.Enter + ((k + 1)).ToString("000") + "| ");
                        for (int m = 0; m < base.DieMatrix.XMax; m++)
                        {
                            if (base.DieMatrix[m, k].Attribute == DieCategory.FailDie)
                            {
                                die.CountDie(base.DieMatrix[m, k].Bin);
                            }
                            this.WriteString(UtilFunction.DieCategoryCaption(base.DieMatrix[m, k].Attribute) + " ");
                        }
                    }
                    _singleTotalDie = base.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow | DieCategory.FailDie | DieCategory.PassDie);
                    this.WriteString(base.Enter + base.Enter);
                    this.WriteString("============ Wafer Information () ===========" + base.Enter);
                    this.WriteString("  Device: " + this.Device + base.Enter);
                    this.WriteString("  Lot NO: " + this.LotNo + base.Enter);
                    this.WriteString("  Slot NO: " + this.SlotNo + base.Enter);
                    this.WriteString("  Wafer ID: " + this.WaferID + base.Enter);
                    this.WriteString("  Operater: " + base.Enter);
                    this.WriteString("  Wafer Size: " + ((this.WaferSize / 10)).ToString() + "inch" + base.Enter);
                    this.WriteString("  Flat Dir: " + this.FlatDir + base.Enter);
                    this.WriteString("  Wafer Test Start Time: " + this.StartTime + base.Enter);
                    this.WriteString("  Wafer Test Finish Time: " + this.EndTime + base.Enter);
                    this.WriteString("  Wafer Load Time: " + this.LoadTime + base.Enter);
                    this.WriteString("  Wafer Unload Time: " + this.UnloadTime + base.Enter);
                    this.WriteString("  Total Test Die: " + _singleTotalDie + base.Enter);
                    this.WriteString("  Pass Die: " + this.PassDie + base.Enter);
                    this.WriteString("  Fail Die: " + this.FailDie + base.Enter);
                    this.WriteString("  Yield: " + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(this.PassDie) / ((double)_singleTotalDie))), 4).ToString("0.00%") + base.Enter);
                    this.WriteString("  Rows: " + this.RowCount + base.Enter);
                    this.WriteString("  Cols: " + this.ColCount + base.Enter);
                    string path = base.FullName.Substring(0, base.FullName.LastIndexOf(@"\")) + @"\Total.txt";
                    if (File.Exists(path))
                    {   
                        writer = File.AppendText(path);
                    }
                    else
                    {
                        writer = File.CreateText(path);
                    }
                    _Device = this.Device;
                    _LotNo = this.LotNo;
                    _TotalDie += _singleTotalDie;
                    _TotalPassDie += this.PassDie;
                    _TotalFailDie += this.FailDie;
                    _TotalYield = Math.Round(Convert.ToDouble((double)(Convert.ToDouble(_TotalPassDie) / ((double)_TotalDie))), 4).ToString("0.00%");
                    writer.WriteLine("============ Wafer Information () ===========");
                    writer.WriteLine("  Device: " + this.Device);
                    writer.WriteLine("  Lot NO: " + this.LotNo);
                    writer.WriteLine("  Slot NO: " + this.SlotNo);
                    writer.WriteLine("  Wafer ID: " + this.WaferID);
                    writer.WriteLine("  Operater: ");
                    writer.WriteLine("  Wafer Size: " + ((this.WaferSize / 10)).ToString() + "inch");
                    writer.WriteLine("  Flat Dir: " + this.FlatDir);
                    writer.WriteLine("  Wafer Test Start Time: " + this.StartTime);
                    writer.WriteLine("  Wafer Test Finish Time: " + this.EndTime);
                    writer.WriteLine("  Wafer Load Time: " + this.LoadTime);
                    writer.WriteLine("  Wafer Unload Time: " + this.UnloadTime);
                    writer.WriteLine("  Total Test Die: " + _singleTotalDie);
                    writer.WriteLine("  Pass Die: " + this.PassDie);
                    writer.WriteLine("  Fail Die: " + this.FailDie);
                    writer.WriteLine("  Yield: " + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(this.PassDie) / ((double)_singleTotalDie))), 4).ToString("0.00%"));
                    writer.WriteLine("  Rows: " + this.RowCount);
                    writer.WriteLine("  Cols: " + this.ColCount);
                    writer.WriteLine("=============================================");
                    writer.WriteLine(base.Enter);

                    writer.Close();
                     */



                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                base.CloseWriter();
            }
        }

        // Properties
        public int CassetteNo
        {
            get
            {
                return (int)base._properties["CassetteNo"];
            }
            set
            {
                base._properties["CassetteNo"] = value;
            }
        }

        public int ColCount
        {
            get
            {
                return (int)base._properties["ColCount"];
            }
            set
            {
                base._properties["ColCount"] = value;
            }
        }

        public string Device
        {
            get
            {
                return base._properties["Device"].ToString();
            }
            set
            {
                base._properties["Device"] = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return (DateTime)base._properties["EndTime"];
            }
            set
            {
                base._properties["EndTime"] = value;
            }
        }

        public int FailDie
        {
            get
            {
                return (int)base._properties["FailDie"];
            }
            set
            {
                base._properties["FailDie"] = value;
            }
        }

        public int FlatDir
        {
            get
            {
                return (int)base._properties["FlatDir"];
            }
            set
            {
                base._properties["FlatDir"] = value;
            }
        }

        public int IndexSizeX
        {
            get
            {
                return (int)base._properties["IndexSizeX"];
            }
            set
            {
                base._properties["IndexSizeX"] = value;
            }
        }

        public int IndexSizeY
        {
            get
            {
                return (int)base._properties["IndexSizeY"];
            }
            set
            {
                base._properties["IndexSizeY"] = value;
            }
        }

        public DateTime LoadTime
        {
            get
            {
                return (DateTime)base._properties["LoadTime"];
            }
            set
            {
                base._properties["LoadTime"] = value;
            }
        }

        public override string LotNo
        {
            get
            {
                return base._properties["LotNo"].ToString();
            }
            set
            {
                base._properties["LotNo"] = value;
            }
        }

        public int MachineNo
        {
            get
            {
                return (int)base._properties["MachineNo"];
            }
            set
            {
                base._properties["MachineNo"] = value;
            }
        }

        public byte MachineType
        {
            get
            {
                return (byte)base._properties["MachineType"];
            }
            set
            {
                base._properties["MachineType"] = value;
            }
        }

        public int MapDataForm
        {
            get
            {
                return (int)base._properties["MapDataForm"];
            }
            set
            {
                base._properties["MapDataForm"] = value;
            }
        }

        public byte MapVersion
        {
            get
            {
                return (byte)base._properties["MapVersion"];
            }
            set
            {
                base._properties["MapVersion"] = value;
            }
        }

        public string Operator
        {
            get
            {
                return base._properties["Operator"].ToString();
            }
            set
            {
                base._properties["Operator"] = value;
            }
        }

        public int PassDie
        {
            get
            {
                return (int)base._properties["PassDie"];
            }
            set
            {
                base._properties["PassDie"] = value;
            }
        }

        public byte ProbingNo
        {
            get
            {
                return (byte)base._properties["ProbingNo"];
            }
            set
            {
                base._properties["ProbingNo"] = value;
            }
        }

        public int Refpx
        {
            get
            {
                return (int)base._properties["Refpx"];
            }
            set
            {
                base._properties["Refpx"] = value;
            }
        }

        public int Refpy
        {
            get
            {
                return (int)base._properties["Refpy"];
            }
            set
            {
                base._properties["Refpy"] = value;
            }
        }

        public int RowCount
        {
            get
            {
                return (int)base._properties["RowCount"];
            }
            set
            {
                base._properties["RowCount"] = value;
            }
        }

        public int SlotNo
        {
            get
            {
                return (int)base._properties["SlotNo"];
            }
            set
            {
                base._properties["SlotNo"] = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return (DateTime)base._properties["StartTime"];
            }
            set
            {
                base._properties["StartTime"] = value;
            }
        }

        public int TotalDie
        {
            get
            {
                return (int)base._properties["TotalDie"];
            }
            set
            {
                base._properties["TotalDie"] = value;
            }
        }

        public DateTime UnloadTime
        {
            get
            {
                return (DateTime)base._properties["UnloadTime"];
            }
            set
            {
                base._properties["UnloadTime"] = value;
            }
        }

        public override string WaferID
        {
            get
            {
                return base._properties["WaferID"].ToString();
            }
            set
            {
                base._properties["WaferID"] = value;
            }
        }

        public int WaferSize
        {
            get
            {
                return (int)base._properties["WaferSize"];
            }
            set
            {
                base._properties["WaferSize"] = value;
            }
        }
    }
}
