
namespace DataToExcel
{
    using System;
    using System.Drawing;
    using System.Collections;

    public class Tma : MappingBase
    {
        private ArrayList _diesBuffer;
        // ���� reference die �����Ա��е�ָ��
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

            this._properties.Add("TestStartTime", new DateTime(1900, 01, 01));
            this._properties.Add("TestFinishTime", new DateTime(1900, 01, 01));
            this._properties.Add("LoadTime", new DateTime(1900, 01, 01));
            this._properties.Add("UnloadTime", new DateTime(1900, 01, 01));

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
                // �򿪶�ȡ��
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
                 * ��ȡԭʼ tma �ļ�ʱ��Ҫִ�д˴��� ?
                 * 
                // tma ���һ�� null die
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
                // �رն�ȡ��
                this.CloseReader();
            }
        }

        public void ReadHeader()
        {
            try
            {
                // �򿪶�ȡ��
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
                // �رն�ȡ��
                this.CloseReader();
            }
        }

        // ���н����ļ�����
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

                    // �� reference die �����Ա�ָ��ֵ���� refX �� refY ֵ
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

        // ����ÿ�� die ����
        private void ParseDies(string s)
        {
            string[] dies = s.Split(new char[] { ' ' });

            this.RowCount += 1;

            // ������ڷָ��Ŀո�
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
                        // ��¼ reference die �����Ա��е�ָ��
                        this._refDiePoint = this._diesBuffer.Count;
                        break;
                    case "N":
                        die.Attribute = DieCategory.TIRefFail;
                        // ��¼ reference die �����Ա��е�ָ��
                        this._refDiePoint = this._diesBuffer.Count;
                        break;
                    default:
                        die.Attribute = DieCategory.Unknow;
                        break;
                }

                this._diesBuffer.Add(die);
            }
        }

        // �� tma �ļ��е����ڸ�ʽ�н�����ʱ�����
        private DateTime ReadDate(string txt)
        {
            try
            {
                string str = "20";

                // ��
                str += txt.Substring(0, 2) + "-";
                // ��
                str += txt.Substring(2, 2) + "-";
                // ��
                str += txt.Substring(4, 2) + " ";
                // ʱ
                str += txt.Substring(6, 2) + ":";
                // ��
                str += txt.Substring(8, 2) + ":";
                // ��
                str += "00";

                return DateTime.Parse(str);
            }
            catch
            {
                return this.EmpDate;
            }
        }

        /// <summary>
        /// �����ݱ���Ϊ tma �ļ�
        /// </summary>
        public override void Save()
        {
            try
            {

                int BIN1 = 0, BIN2 = 0, BIN3 = 0, BIN4 = 0, BIN5 = 0, BIN6 = 0, BIN7 = 0, BIN8 = 0, BIN9 = 0, BIN10 = 0, BIN11 = 0, BIN12 = 0, BIN13 = 0, BIN14 = 0, BIN15 = 0, BIN16 = 0, BIN17 = 0, BIN18 = 0,
                   BIN19 = 0, BIN20 = 0, BIN21 = 0, BIN22 = 0, BIN23 = 0, BIN24 = 0, BIN25 = 0, BIN26 = 0, BIN27 = 0, BIN28 = 0, BIN29 = 0, BIN30 = 0, BIN31 = 0, BIN32 = 0;

                for (int y = 0; y < this.DieMatrix.YMax; y++)
                {
                    for (int x = 0; x < this.DieMatrix.XMax; x++)
                    {
                        switch (this.DieMatrix[x, y].Attribute)
                        {

                            case DieCategory.PassDie:
                                {
                                    switch (this.DieMatrix[x, y].Bin)
                                    {
                                        case 1: { BIN1++; break; }
                                        case 2: { BIN2++; break; }
                                        case 3: { BIN3++; break; }
                                        case 4: { BIN4++; break; }
                                       
                                    }
                                    break;
                                }

                        }


                        switch (this.DieMatrix[x, y].Attribute)
                        {

                           case DieCategory.FailDie:
                                {
                                    switch (this.DieMatrix[x, y].Bin)
                                    {

                                        case 2: { BIN2++; break; }
                                        case 3: { BIN3++; break; }
                                        case 4: { BIN4++; break; }
                                        case 5: { BIN5++; break; }
                                        case 6: { BIN6++; break; }
                                        case 7: { BIN7++; break; }
                                        case 8: { BIN8++; break; }
                                        case 9: { BIN9++; break; }
                                        case 10: { BIN10++; break; }
                                        case 11: { BIN11++; break; }
                                        case 12: { BIN12++; break; }
                                        case 13: { BIN13++; break; }
                                        case 14: { BIN14++; break; }
                                        case 15: { BIN15++; break; }
                                        case 16: { BIN16++; break; }
                                        case 17: { BIN17++; break; }
                                        case 18: { BIN18++; break; }
                                        case 19: { BIN19++; break; }
                                        case 20: { BIN20++; break; }
                                        case 21: { BIN21++; break; }
                                        case 22: { BIN22++; break; }
                                        case 23: { BIN23++; break; }
                                        case 24: { BIN24++; break; }
                                        case 25: { BIN25++; break; }
                                        case 26: { BIN26++; break; }
                                        case 27: { BIN27++; break; }
                                        case 28: { BIN28++; break; }
                                        case 29: { BIN29++; break; }
                                        case 30: { BIN30++; break; }
                                        case 31: { BIN31++; break; }
                                        case 32: { BIN32++; break; }
                                    }
                                    break;
                                }
                               
                        }
                    }
                }


                // �򿪻򴴽��ļ�
                this.OpenWriter();
                this.WriteString("DEVICE:" + this.DeviceName + "\r\n");
                this.WriteString("LOT:" + this.LotNo.Substring(0,this.LotNo.Length-3) + "\r\n");
                this.WriteString("WAFER:" +Convert.ToInt32(this.SlotNo).ToString("00")+ "\r\n");
                /*
                if (this.FlatDir == "90")
                {
                    this.WriteString("FNLOC:" + "RIGHT" + "\r\n");
                }
                if (this.FlatDir == "0")
                {
                    this.WriteString("FNLOC:" + "UP" + "\r\n");
                }
                if (this.FlatDir == "180")
                {
                    this.WriteString("FNLOC:" + "DOWN" + "\r\n");
                }
                if (this.FlatDir == "270")
                {
                    this.WriteString("FNLOC:" + "LEFT" + "\r\n");
                }
                  */
                this.WriteString("FNLOC:" + this.FlatDir + "\r\n");
                this.WriteString("ROWCT:" + this.RowCount + "\r\n");
                this.WriteString("COLCT:" + this.ColCount + "\r\n");
                this.WriteString("BCEQU:01" + "\r\n");
                this.WriteString("REFPX:9" + "\r\n");
                this.WriteString("REFPY:54" + "\r\n");
                this.WriteString("DUTMS:mm" + "\r\n");
                this.WriteString("XDIES:1.2599" + "\r\n");
                this.WriteString("YDIES:1.69");
              
                // д�� Die ����
                for (int y = 0; y < this.DieMatrix.YMax; y++)
                {
                    this.WriteString("\r\n");
                    this.WriteString("RowData:");
             
                    for (int x = 0; x < this.DieMatrix.XMax; x++)
                    {
  
                       switch (this.DieMatrix[x, y].Attribute)
                       {
                       
                           case DieCategory.PassDie:
                               {

                                   this.WriteString(string.Format("{0,1:G}", " 01"));
                                   break;
                               }
                           case DieCategory.MarkDie:
                           case DieCategory.NoneDie:
                           case DieCategory.SkipDie:
                           case DieCategory.FailDie:
                               {

                                   this.WriteString(string.Format("{0,1:G}", " __"));
                                   break;
                               }
                           
                         
                        

                    }
                       

                    }
                }

                this.WriteString("\r\n");
              

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

        // �ϲ� tma �ļ�
        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            if (!(map is Tma))
                throw new Exception("tma �����ļ�ֻ�ܺ� tma �����ļ��ϲ���");

            Tma tma = new Tma(newfile);

            // �ϲ� die ����
            tma._dieMatrix = this._dieMatrix + map.DieMatrix;

            // �ϲ�������Ϣ

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


            // ���¼���ͳ������
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
         

            // ������ӡ�����λ��
            tma.Marking = tma.CreatePCP();

            return tma;
        }

        // create the post check point
        public Point[] CreatePCP()
        {
            Point[] points = new Point[5];
            int ptop = 0, pleft = 1, pcenter = 2, pright = 3, pbottom = 4;

            // ���ĵ�
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
                        // �ҵ�
                        points[pcenter].X = x + i * p.X;
                        points[pcenter].Y = y + i * p.Y;
                        goto top;
                    }
                }
            }

            // �Ҳ���
            points[pcenter].X = -1;
            points[pcenter].Y = -1;

            // ��
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
                        // �ҵ�
                        points[ptop].X = x + i * p.X;
                        points[ptop].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // �ҵ���һ����������
                            break;
                        }
                        else
                        {
                            // �ҵ��ڶ�����
                            goto left;
                        }
                    }
                }
            }

            // �Ҳ���
            if (i1 <= 0)
            {
                points[ptop].X = -1;
                points[ptop].Y = -1;
            }

            // ��
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
                        // �ҵ�
                        points[pleft].X = x + i * p.X;
                        points[pleft].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // �ҵ���һ����������
                            break;
                        }
                        else
                        {
                            // �ҵ��ڶ�����
                            goto right;
                        }
                    }
                }
            }

            // �Ҳ���
            if (i1 <= 0)
            {
                points[pleft].X = -1;
                points[pleft].Y = -1;
            }

            // ��
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
                        // �ҵ�
                        points[pright].X = x + i * p.X;
                        points[pright].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // �ҵ���һ����������
                            break;
                        }
                        else
                        {
                            // �ҵ��ڶ�����
                            goto bottom;
                        }
                    }
                }
            }

            // �Ҳ���
            if (i1 <= 0)
            {
                points[pright].X = -1;
                points[pright].Y = -1;
            }


            // ��
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
                        // �ҵ�
                        points[pbottom].X = x + i * p.X;
                        points[pbottom].Y = y + i * p.Y;

                        if (i1 <= 2)
                        {
                            i1 += 1;
                            // �ҵ���һ����������
                            break;
                        }
                        else
                        {
                            // �ҵ��ڶ�����
                            goto exit;
                        }
                    }
                }
            }

            // �Ҳ���
            if (i1 <= 0)
            {
                points[pbottom].X = -1;
                points[pbottom].Y = -1;
            }

        exit:
            return points;
        }

        /// <summary>
        /// �ж� mapping �ļ��� die �����е�һ�� die �Ƿ�Ϊ�� die
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
