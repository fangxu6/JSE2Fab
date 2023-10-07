
namespace DataToExcel
{
    using System;
    using System.Drawing;
    using System.Collections;

    public class Tma : MappingBase
    {
        private ArrayList _diesBuffer;
        // 保存 reference die 在线性表中的指针
        private int _refDiePoint;

        public override string DeviceName
        {
            get { return this._properties["Device"].ToString(); }
            set { this._properties["Device"] = value; }
        }

        public override string LotNo
        {
            get { return this._properties["LotNo"].ToString(); }
            set { this._properties["LotNo"] = value; }
        }

        public string SlotNo
        {
            get { return this._properties["SlotNo"].ToString(); }
            set { this._properties["SlotNo"] = value; }
        }

        public override string WaferID
        {
            get { return this._properties["WaferID"].ToString(); }
            set { this._properties["WaferID"] = value; }
        }

        public string Operater
        {
            get { return this._properties["Operater"].ToString(); }
            set { this._properties["Operater"] = value; }
        }

        public string WaferSize
        {
            get { return this._properties["WaferSize"].ToString(); }
            set { this._properties["WaferSize"] = value; }
        }

        public string FlatDir
        {
            get { return this._properties["FlatDir"].ToString(); }
            set { this._properties["FlatDir"] = value; }
        }

        public DateTime TestStartTime
        {
            get { return (DateTime)this._properties["TestStartTime"]; }
            set { this._properties["TestStartTime"] = value; }
        }

        public DateTime TestFinishTime
        {
            get { return (DateTime)this._properties["TestFinishTime"]; }
            set { this._properties["TestFinishTime"] = value; }
        }

        public DateTime LoadTime
        {
            get { return (DateTime)this._properties["LoadTime"]; }
            set { this._properties["LoadTime"] = value; }
        }

        public DateTime UnloadTime
        {
            get { return (DateTime)this._properties["UnloadTime"]; }
            set { this._properties["UnloadTime"] = value; }
        }

        public int TotalDie
        {
            get { return (int)this._properties["TotalDie"]; }
            set { this._properties["TotalDie"] = value; }
        }

        public int PassDie
        {
            get { return (int)this._properties["PassDie"]; }
            set { this._properties["PassDie"] = value; }
        }

        public int FailDie
        {
            get { return (int)this._properties["FailDie"]; }
            set { this._properties["FailDie"] = value; }
        }

        public decimal Yield
        {
            get { return (decimal)this._properties["Yield"]; }
            set { this._properties["Yield"] = value; }
        }

       

        public Point[] Marking
        {
            get { return (Point[])this._properties["Marking"]; }
            set { this._properties["Marking"] = value; }
        }

        public int RowCount
        {
            get { return (int)this._properties["RowCount"]; }
            set { this._properties["RowCount"] = value; }
        }

        public int ColCount
        {
            get { return (int)this._properties["ColCount"]; }
            set { this._properties["ColCount"] = value; }
        }

        public int Refpx
        {
            get { return (int)this._properties["Refpx"]; }
            set { this._properties["Refpx"] = value; }
        }

        public int Refpy
        {
            get { return (int)this._properties["Refpy"]; }
            set { this._properties["Refpy"] = value; }
        }

        public Tma(string file)
            : base(ConstDefine.FileType_TMA, file)
        {
            this._refDiePoint = -1;
        }

        protected override void InitialProperties()
        {
            this._keys.Add("Device");
            this._keys.Add("LotNo");
            this._keys.Add("SlotNo");
            this._keys.Add("WaferID");
            this._keys.Add("Operater");
            this._keys.Add("WaferSize");
            this._keys.Add("FlatDir");

            this._keys.Add("TestStartTime");
            this._keys.Add("TestFinishTime");
            this._keys.Add("LoadTime");
            this._keys.Add("UnloadTime");

            this._keys.Add("TotalDie");
            this._keys.Add("PassDie");
            this._keys.Add("FailDie");

            this._keys.Add("Yield");

            this._keys.Add("Marking");

            this._keys.Add("RowCount");
            this._keys.Add("ColCount");

            this._properties.Add("Device", "");
            this._properties.Add("LotNo", "");
            this._properties.Add("SlotNo", "");
            this._properties.Add("WaferID", "");
            this._properties.Add("Operater", "");
            this._properties.Add("WaferSize", "");
            this._properties.Add("FlatDir", "");

            this._properties.Add("TestStartTime", new DateTime(1900, 1, 1));
            this._properties.Add("TestFinishTime", new DateTime(1900, 1, 1));
            this._properties.Add("LoadTime", new DateTime(1900, 1, 1));
            this._properties.Add("UnloadTime", new DateTime(1900, 1, 1));

            this._properties.Add("TotalDie", 0);
            this._properties.Add("PassDie", 0);
            this._properties.Add("FailDie", 0);

            this._properties.Add("Yield", 0.0m);
            this._properties.Add("Marking", null);

            this._properties.Add("RowCount", 0);
            this._properties.Add("ColCount", 0);

            this._keys.Add("Refpx");
            this._properties.Add("Refpx", 0);

            this._keys.Add("Refpy");
            this._properties.Add("Refpy", 0);
        }

        public override void Read()
        {
            try
            {
                // 打开读取流
                this.OpenReader();

                if (this._diesBuffer == null)
                    this._diesBuffer = new ArrayList();
                else
                    this._diesBuffer.Clear();

                this.ColCount = 0;
                this.RowCount = 0;

                while (true)
                {
                    string line = this.ReadLine();

                    if (line == null)
                        break;

                    this.Parse(line);
                }

                /*
                 * 读取原始 tma 文件时需要执行此代码 ?
                 * 
                // tma 最后补一个 null die
                DieData die = new DieData();
                die.Attribute = DieCategory.NoneDie;
                this._diesBuffer.Add(die);
                 * 
                */

                this._dieMatrix = new DieMatrix(this._diesBuffer, this.ColCount, this.RowCount);

            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                // 关闭读取流
                this.CloseReader();
            }
        }

        public void ReadHeader()
        {
            try
            {
                // 打开读取流
                this.OpenReader();

                if (this._diesBuffer == null)
                    this._diesBuffer = new ArrayList();
                else
                    this._diesBuffer.Clear();

                this.ColCount = 0;
                this.RowCount = 0;

                while (true)
                {
                    string line = this.ReadLine();

                    if (line == null)
                        break;

                    if (line.IndexOf('|') >= 0)
                        continue;

                    this.Parse(line);
                }

            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                // 关闭读取流
                this.CloseReader();
            }
        }

        // 按行解析文件内容
        private void Parse(string line)
        {
            try
            {
                if (line.IndexOf(':') >= 0)
                {
                    string[] strs = line.Split(new char[] { ':' });

                    switch (strs[0].ToLower())
                    {
                        case "device":
                            this.DeviceName = strs[1].Trim();
                            break;
                        case "lot no":
                            this.LotNo = strs[1].Trim();
                            break;
                        case "slot no":
                            this.SlotNo = strs[1].Trim();
                            break;
                        case "wafer id":
                            this.WaferID = strs[1].Trim();
                            break;
                        case "operater":
                            this.Operater = strs[1].Trim();
                            break;
                        case "wafer size":
                            this.WaferSize = strs[1].Trim();
                            break;
                        case "flat dir":
                            this.FlatDir = strs[1].Trim();
                            break;
                        case "wafer test start time":
                            this.TestStartTime = this.ReadDate(strs[1].Trim());
                            break;
                        case "wafer test finish time":
                            this.TestFinishTime = this.ReadDate(strs[1].Trim());
                            break;
                        case "wafer load time":
                            this.LoadTime = this.ReadDate(strs[1].Trim());
                            break;
                        case "wafer unload time":
                            this.UnloadTime = this.ReadDate(strs[1].Trim());
                            break;
                        case "total test die":
                            this.TotalDie = Int32.Parse(strs[1].Trim());
                            break;
                        case "pass die":
                            this.PassDie = Int32.Parse(strs[1].Trim());
                            break;
                        case "fail die":
                            this.FailDie = Int32.Parse(strs[1].Trim());
                            break;
                        case "yield":
                            string s = strs[1].Trim();
                            this.Yield = Decimal.Parse(s.Substring(0, s.Length - 1));
                            break;
                    }
                }
                else if (line.IndexOf('|') >= 0)
                {
                    this.ParseDies(line.Split(new char[] { '|' })[1]);

                    // 从 reference die 在线性表指针值计算 refX 和 refY 值
                    if (this._refDiePoint != -1)
                    {
                        this.Refpx = this._refDiePoint % this.ColCount + 1;
                        this.Refpy = this._refDiePoint / this.ColCount + 1;
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        // 解析每行 die 数据
        private void ParseDies(string s)
        {
            string[] dies = s.Split(new char[] { ' ' });

            this.RowCount += 1;

            // 清除用于分隔的空隔
            ArrayList arr = new ArrayList();
            foreach (string str in dies)
            {
                if (str != "" && str != " ")
                    arr.Add(str);
            }

            if (this.ColCount <= 0)
                this.ColCount = arr.Count;

            foreach (string d in arr)
            {
                DieData die = new DieData();

                switch (d.ToUpper())
                {
                    case ".":
                        die.Attribute = DieCategory.NoneDie;
                        break;
                    case "M":
                        die.Attribute = DieCategory.MarkDie;
                        break;
                    case "F":
                        die.Attribute = DieCategory.FailDie;
                        break;
                    case "P":
                        die.Attribute = DieCategory.PassDie;
                        break;
                    case "S":
                        die.Attribute = DieCategory.SkipDie;
                        break;
                    case "Y":
                        die.Attribute = DieCategory.TIRefPass;
                        // 记录 reference die 在线性表中的指针
                        this._refDiePoint = this._diesBuffer.Count;
                        break;
                    case "N":
                        die.Attribute = DieCategory.TIRefFail;
                        // 记录 reference die 在线性表中的指针
                        this._refDiePoint = this._diesBuffer.Count;
                        break;
                    default:
                        die.Attribute = DieCategory.Unknow;
                        break;
                }

                this._diesBuffer.Add(die);
            }
        }

        // 从 tma 文件中的日期格式中解析出时间对象
        private DateTime ReadDate(string txt)
        {
            try
            {
                string str = "20";

                // 年
                str += txt.Substring(0, 2) + "-";
                // 月
                str += txt.Substring(2, 2) + "-";
                // 日
                str += txt.Substring(4, 2) + " ";
                // 时
                str += txt.Substring(6, 2) + ":";
                // 分
                str += txt.Substring(8, 2) + ":";
                // 秒
                str += "00";

                return DateTime.Parse(str);
            }
            catch
            {
                return this.EmpDate;
            }
        }

        /// <summary>
        /// 将数据保存为 tma 文件
        /// </summary>
        public override void Save()
        {
            try
            {
                // 打开或创建文件
                this.OpenWriter();
                string[] b = this.DeviceName.Split(new char[] { '-' }, StringSplitOptions.None);

                this.WriteString("<Header>"+"\r\n");
                this.WriteString("Skyworks Solutions, Inc.: JCAP" +"\r\n");
                this.WriteString("PRODUCT: " + "DIE-01290"+ "\r\n");
                this.WriteString("PROBE_PROGRAM: " + this.DeviceName +"\r\n");
                this.WriteString("LOT: " + this.LotNo + "\r\n");
                this.WriteString("WAFER: " + this.LotNo+"-"+this.WaferID +"\r\n");
                if (this.WaferSize == "60")
                {
                    this.WaferSize = "6 inch";
                }
                else if (this.WaferSize == "80")
                {
                    this.WaferSize = "8 inch";
                
                }

                this.WriteString("Wafer_Size: " + this.WaferSize + "\r\n");
                this.WriteString("START_TIME: " + (this.TestStartTime <= this.EmpDate ? "" : this.TestStartTime.ToString("yy/MM/dd HH:mm")) + "\r\n");
                this.WriteString("FINISH_TIME: " + (this.TestFinishTime <= this.EmpDate ? "" : this.TestFinishTime.ToString("yy/MM/dd HH:mm")) + "\r\n");
                this.WriteString("MAP_FILENAME: " +this.LotNo+"-"+ this.WaferID.Trim('\0') + ".ASC" + "\r\n");
                this.WriteString("GROSS_DIE_QTY: " + (this.PassDie + this.FailDie) + "\r\n");
                this.WriteString("GOOD_DIE_QTY: " + this.PassDie + "\r\n");
                this.WriteString("ROWS: " + this.RowCount + "\r\n");
                this.WriteString("COLUMNS: " + this.ColCount + "\r\n");
                if (this.FlatDir == "90")
                {
                    this.FlatDir = "R";
                }

                else if (this.FlatDir == "180")
                {
                    this.FlatDir = "B";
                }
                else if (this.FlatDir == "270")
                {
                    this.FlatDir = "L";
                }
                else if (this.FlatDir == "0")
                {
                    this.FlatDir = "T";
                }


                this.WriteString("NOTCH: " + this.FlatDir + "\r\n");
                this.WriteString("</Header>" + "\r\n");
                this.WriteString("\r\n");
                this.WriteString("<Bin_Map>");


                // 写入 Die 数据
                for (int y = 0; y < this.DieMatrix.YMax; y++)
                {
                    this.WriteString("\r\n");

             
                    for (int x = 0; x < this.DieMatrix.XMax; x++)
                    {
  
                       switch (this.DieMatrix[x, y].Attribute)
                       {
                       
                           case DieCategory.PassDie:
                           case DieCategory.MarkDie:
                           case DieCategory.NoneDie:
                           case DieCategory.SkipDie:
                               {

                                   this.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(this.DieMatrix[x, y].Attribute)));
                                   break;
                               }
                           
                           case DieCategory.FailDie:
                               {


                               //    this.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(this.DieMatrix[x, y].Attribute)));
                               //    break;
                                   switch (this.DieMatrix[x, y].Bin)
                                   {
                                   
                                       case 2:
                                       case 3:
                                       case 4:
                                       case 5:
                                       case 6:
                                       case 7:
                                       case 8:
                                       case 9:

                                           this.WriteString(string.Format("{0,1:G}", this.DieMatrix[x, y].Bin));
                                           break;


                                       case 10:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "A"));
                                               break;

                                           }
                                       case 11:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "B"));
                                               break;

                                           }
                                       case 12:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "C"));
                                               break;

                                           }
                                       case 13:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "D"));
                                               break;

                                           }
                                       case 14:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "E"));
                                               break;

                                           }
                                       case 15:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "F"));
                                               break;

                                           }
                                       case 16:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "G"));
                                               break;

                                           }
                                       case 17:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "H"));
                                               break;

                                           }
                                       case 18:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "I"));
                                               break;

                                           }
                                       case 19:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "J"));
                                               break;

                                           }
                                       case 20:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "K"));
                                               break;

                                           }
                                       case 21:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "L"));
                                               break;

                                           }
                                       case 22:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "M"));
                                               break;


                                           }
                                       case 23:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "N"));
                                               break;

                                           }
                                       case 24:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "O"));
                                               break;

                                           }
                                       case 25:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "P"));
                                               break;

                                           }
                                       case 26:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "Q"));
                                               break;

                                           }
                                       case 27:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "R"));
                                               break;

                                           }
                                       case 28:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "S"));
                                               break;

                                           }
                                       case 29:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "T"));
                                               break;

                                           }
                                       case 30:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "U"));
                                               break;

                                           }
                                       case 31:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "V"));
                                               break;

                                           }
                                       case 32:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "W"));
                                               break;

                                           }

                                       case 33:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "X"));
                                               break;

                                           }
                                       case 34:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "Y"));
                                               break;

                                           }
                                       case 35:
                                           {
                                               this.WriteString(string.Format("{0,1:G}", "Z"));
                                               break;

                                           }

                                       default:
                                           {

                                               this.WriteString(string.Format("{0,1:G}", "F"));
                                               break;
                                           
                                           }


                                   }





                           break;
                               }

                    }
                       

                    }
                }
                this.WriteString("\r\n" + "</Bin_Map>" + "\r\n");


            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                this.CloseWriter();
            }
        }

        private string MarkText()
        {
            string str = "";

            Point[] mark = this.Marking;

            if (mark == null)
                return "";

            for (int i = 0; i < mark.Length; i++)
            {
                str += (i + 1) + ".X" + mark[i].X + ",Y" + mark[i].Y + ";";
            }

            if (str == "")
                return "";

            return str.Substring(0, str.Length - 1);
        }

        // 合并 tma 文件
        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            if (!(map is Tma))
                throw new Exception("tma 类型文件只能和 tma 类型文件合并。");

            Tma tma = new Tma(newfile);

            // 合并 die 矩阵
            tma._dieMatrix = this._dieMatrix + map.DieMatrix;

            // 合并基本信息

            if (this.DeviceName != "")
                tma.DeviceName = this.DeviceName;
            else
                tma.DeviceName = map.Properties["Device"].ToString();

            if (this.LotNo != "")
                tma.LotNo = this.LotNo;
            else
                tma.LotNo = map.Properties["LotNo"].ToString();

            if (this.SlotNo != "")
                tma.SlotNo = this.SlotNo;
            else
                tma.SlotNo = map.Properties["SlotNo"].ToString();

            if (this.WaferID != "")
                tma.WaferID = this.WaferID;
            else
                tma.WaferID = map.Properties["WaferID"].ToString();

            if (this.Operater != "")
                tma.Operater = this.Operater;
            else
                tma.Operater = map.Properties["Operater"].ToString();

            if (this.WaferSize != "")
                tma.WaferSize = this.WaferSize;
            else
                tma.WaferSize = map.Properties["WaferSize"].ToString();

            if (this.FlatDir != "")
                tma.FlatDir = this.FlatDir;
            else
                tma.FlatDir = map.Properties["FlatDir"].ToString();

            if (this.TestStartTime > this.EmpDate)
                tma.TestStartTime = this.TestStartTime;
            else
                tma.TestStartTime = (DateTime)map.Properties["TestStartTime"];

            if (this.TestFinishTime > this.EmpDate)
                tma.TestFinishTime = this.TestFinishTime;
            else
                tma.TestFinishTime = (DateTime)map.Properties["TestFinishTime"];

            if (this.LoadTime > this.EmpDate)
                tma.LoadTime = this.LoadTime;
            else
                tma.LoadTime = (DateTime)map.Properties["LoadTime"];

            if (this.UnloadTime > this.UnloadTime)
                tma.UnloadTime = this.UnloadTime;
            else
                tma.UnloadTime = (DateTime)map.Properties["UnloadTime"];


            // 重新计算统计数据
            tma.TotalDie = 0;
            tma.PassDie = 0;
            tma.FailDie = 0;
            tma.Yield = 0.0m;

            foreach (DieData die in tma._dieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    tma.FailDie += 1;
                else if (die.Attribute == DieCategory.PassDie)
                    tma.PassDie += 1;
            }

            tma.TotalDie = tma.PassDie + tma.FailDie;
            tma.Yield = (decimal)tma.PassDie / (decimal)tma.TotalDie;
         

            // 创建打印五个对位点
            tma.Marking = tma.CreatePCP();

            return tma;
        }

        // create the post check point
        public Point[] CreatePCP()
        {
            Point[] points = new Point[5];
            int ptop = 0, pleft = 1, pcenter = 2, pright = 3, pbottom = 4;

            // 中心点
            int x = this._dieMatrix.XMax / 2;
            int y = this._dieMatrix.YMax / 2;

            int limit = x > y ? y : x;

            Point[] ps = new Point[] { 
                new Point(1, 0), 
                new Point(1, -1) , 
                new Point(0, -1) , 
                new Point(-1, -1) , 
                new Point(-1, 0) , 
                new Point(-1, 1) , 
                new Point(0, 1) , 
                new Point(1, 1) 
            };

            for (int i = 0; i < limit; i++)
            {
                foreach (Point p in ps)
                {
                    if (this.DieMatrix[x + i * p.X, y + i * p.Y].Attribute == DieCategory.PassDie)
                    {
                        // 找到
                        points[pcenter].X = x + i * p.X;
                        points[pcenter].Y = y + i * p.Y;
                        goto top;
                    }
                }
            }

            // 找不到
            points[pcenter].X = -1;
            points[pcenter].Y = -1;

            // 上
        top:
            ps = new Point[] {  
                new Point(0, 1) , 
                new Point(-1, 1) ,
                new Point(1, 1) ,
                new Point(1, 0),
                new Point(-1, 0) 
            };

            int x1 = x, y1 = y;

            x = x1;
            y = 1;

            int i1 = 0;
            for (int i = 0; i < limit; i++)
            {
                foreach (Point p in ps)
                {
                    if (this.DieMatrix[x + i * p.X, y + i * p.Y].Attribute == DieCategory.PassDie)
                    {
                        // 找到
                        points[ptop].X = x + i * p.X;
                        points[ptop].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // 找到第一个，继续找
                            break;
                        }
                        else
                        {
                            // 找到第二个点
                            goto left;
                        }
                    }
                }
            }

            // 找不到
            if (i1 <= 0)
            {
                points[ptop].X = -1;
                points[ptop].Y = -1;
            }

            // 左
        left:
            ps = new Point[] { 
                new Point(1, 0), 
                new Point(1, 1) , 
                new Point(1, -1) , 
                new Point(0, 1) , 
                new Point(0, -1) 
            };

            x = 1;
            y = y1;

            i1 = 0;
            for (int i = 0; i < limit; i++)
            {
                foreach (Point p in ps)
                {
                    if (this.DieMatrix[x + i * p.X, y + i * p.Y].Attribute == DieCategory.PassDie)
                    {
                        // 找到
                        points[pleft].X = x + i * p.X;
                        points[pleft].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // 找到第一个，继续找
                            break;
                        }
                        else
                        {
                            // 找到第二个点
                            goto right;
                        }
                    }
                }
            }

            // 找不到
            if (i1 <= 0)
            {
                points[pleft].X = -1;
                points[pleft].Y = -1;
            }

            // 右
        right:
            ps = new Point[] { 
                new Point(-1, 0), 
                new Point(-1, 1) , 
                new Point(-1, -1) , 
                new Point(0, 1) , 
                new Point(0, -1) 
            };

            x = this._dieMatrix.XMax - 2;
            y = y1;

            i1 = 0;
            for (int i = 0; i < limit; i++)
            {
                foreach (Point p in ps)
                {
                    if (this.DieMatrix[x + i * p.X, y + i * p.Y].Attribute == DieCategory.PassDie)
                    {
                        // 找到
                        points[pright].X = x + i * p.X;
                        points[pright].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // 找到第一个，继续找
                            break;
                        }
                        else
                        {
                            // 找到第二个点
                            goto bottom;
                        }
                    }
                }
            }

            // 找不到
            if (i1 <= 0)
            {
                points[pright].X = -1;
                points[pright].Y = -1;
            }


            // 下
        bottom:
            ps = new Point[] { 
                new Point(0, -1), 
                new Point(1, -1) , 
                new Point(-1, -1) , 
                new Point(-1, 0) , 
                new Point(1, 0) 
            };

            x = x1;
            y = this._dieMatrix.YMax - 2;

            i1 = 0;
            for (int i = 0; i < limit; i++)
            {
                foreach (Point p in ps)
                {
                    if (this.DieMatrix[x + i * p.X, y + i * p.Y].Attribute == DieCategory.PassDie)
                    {
                        // 找到
                        points[pbottom].X = x + i * p.X;
                        points[pbottom].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // 找到第一个，继续找
                            break;
                        }
                        else
                        {
                            // 找到第二个点
                            goto exit;
                        }
                    }
                }
            }

            // 找不到
            if (i1 <= 0)
            {
                points[pbottom].X = -1;
                points[pbottom].Y = -1;
            }

        exit:
            return points;
        }

        /// <summary>
        /// 判断 mapping 文件的 die 矩阵中的一个 die 是否为空 die
        /// </summary>
        /// <param name="die"></param>
        /// <returns></returns>
        public override bool IsEmptyDie(DieData die)
        {
            if (die.Attribute == DieCategory.NoneDie ||
                die.Attribute == DieCategory.MarkDie ||
                die.Attribute == DieCategory.SkipDie)
                return true;
            else
                return false;
        }
    }
}
