

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

                    this.WriteString("Device:" + this.Device + this.Enter);
                    this.WriteString("Lot NO:" + this.LotNo + this.Enter);
                  //  this.WriteString("Slot No:" + this.SlotNo.ToString("00") + this.Enter);
                    this.WriteString("Wafer ID:" + this.SlotNo.ToString("00") + this.Enter);
                    string WaferSize1="";

                    if (this.WaferSize == 60)
                    {
                        WaferSize1 = "6 Inch";
                    }
                    else if (this.WaferSize == 80)
                    {
                        WaferSize1 = "8 ";

                    }

                    else if (this.WaferSize == 120)
                    {
                        WaferSize1 = "12 Inch";

                    }
                    this.WriteString("Wafer Size:" + WaferSize1 + this.Enter);

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

                    this.WriteString("Flat Dir:" + FlatDir1 + this.Enter);
                    int flagbin = 0;

                    int ymin = 1000, xmin = 1000, ymax = 0, xmax = 0 ,bin58=0;
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
                                            if (xxx == 58)
                                            {
                                                bin58++;
                                            }
                                        break;
                                }

                            }
                        }


                    }

                    this.WriteString("ROWCT:" + (ymax - ymin +1) + this.Enter);
                    this.WriteString("COLCT:" + (xmax - xmin + 1) + this.Enter);
                    if (flagbin == 0)
                    {
                        this.WriteString("PASS BIN:1" + this.Enter);
                    }
                    else
                    {
                        this.WriteString("PASS BIN:1" + this.Enter);
                    }
                    this.WriteString("Test Start Time:" + this.LoadTime.ToString("yy/MM/dd HH:mm:ss") + this.Enter);
                    this.WriteString("Test End Time:" + this.EndTime.ToString("yy/MM/dd HH:mm:ss") + this.Enter);
                   /* this.WriteString("Test Program:;" + this.Enter);
                    this.WriteString("Tester ID:" + this.Enter);
                    this.WriteString("Operator ID:" + this.Enter);
                    this.WriteString("Sort ID:" + this.Enter);
                    this.WriteString("Test site: " + this.Enter);
                    this.WriteString("Probe Card ID:" + this.Enter);
                    this.WriteString("Load Board ID:" + this.Enter);*/

                   
                    this.WriteString("Gross die:" + (this.PassDie + this.FailDie-bin58) + this.Enter);
                    this.WriteString("Pass Die:" + this.PassDie + this.Enter);
                    this.WriteString("Fail Die:" + this.FailDie + this.Enter);
                    this.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)(this.PassDie / ((double)(this.PassDie + this.FailDie-bin58)))), 6).ToString("0.0000%") + this.Enter);
                  //  this.WriteString("StrBin:2,2;3,3;4,4;5,5;6,6;7,7;8,8;9,9;10,A;11,B;12,C;13,D;14,E;15,F;16,G;17,H;18,I;19,J;20,K;21,L;22,M;63,X;" + this.Enter);
                    //this.WriteString("StrBin:1,A;57,X;" + this.Enter);
                   

                   for (int y = ymin; y < ymax+1; y++)
                  //  for (int y = 0; y < base.DieMatrix.YMax-1; y++)
                   {

                       for (int x = xmin; x < xmax+1; x++)
                      //  for (int x = 0; x < base.DieMatrix.XMax; x++)
                        {

                            switch (base.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        int xxx = this.DieMatrix[x, y].Bin;
                                        this.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(base.DieMatrix[x, y].Attribute)));
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
                                        //this.DieMatrix[x, y].Bin = this.DieMatrix[x, y].Bin - 1;  //BIN-1
                                        this.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(base.DieMatrix[x, y].Attribute)));
                                        
                                        break;

                                    }

                            }
                        }
                        this.WriteString(this.Enter);


                    }


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
