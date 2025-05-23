
/*
 * 作者：sky
 * 时间：2008-01-09
 * 作用：用于描述 Mapping 中的 Tsk 文件格式
 */

namespace DataToExcel
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Reflection;

    public class Tsk : MappingBase
    {
        public string Operator
        {
            get { return this._properties["Operator"].ToString(); }
            set { this._properties["Operator"] = value; }
        }

        public string Device
        {
            get { return this._properties["Device"].ToString(); }
            set { this._properties["Device"] = value; }
        }

        public int WaferSize
        {
            get { return (int)this._properties["WaferSize"]; }
            set { this._properties["WaferSize"] = value; }
        }

        public int MachineNo
        {
            get { return (int)this._properties["MachineNo"]; }
            set { this._properties["MachineNo"] = value; }
        }

        public int IndexSizeX
        {
            get { return (int)this._properties["IndexSizeX"]; }
            set { this._properties["IndexSizeX"] = value; }
        }

        public int IndexSizeY
        {
            get { return (int)this._properties["IndexSizeY"]; }
            set { this._properties["IndexSizeY"] = value; }
        }

        public int FlatDir
        {
            get { return (int)this._properties["FlatDir"]; }
            set { this._properties["FlatDir"] = value; }
        }

        public byte MachineType
        {
            get { return (byte)this._properties["MachineType"]; }
            set { this._properties["MachineType"] = value; }
        }

        public byte MapVersion
        {
            get { return (byte)this._properties["MapVersion"]; }
            set { this._properties["MapVersion"] = value; }
        }

        // 用于标识是否有扩展数据 扩展头信息
        public bool ExtendHeadFlag
        {
            get { return (bool)this._properties["ExtendHeadFlag"]; }
            set { this._properties["ExtendHeadFlag"] = value; }
        }

        public bool ExtendFlag
        {
            get { return (bool)this._properties["ExtendFlag"]; }
            set { this._properties["ExtendFlag"] = value; }
        }

        public bool ExtendFlag2
        {
            get { return (bool)this._properties["ExtendFlag2"]; }
            set { this._properties["ExtendFlag2"] = value; }

        }

        public ArrayList ExtendList
        {
            get { return (ArrayList)this._properties["ExtendList"]; }
            set { this._properties["ExtendList"] = value; }
        }

        public int Rows
        {
            get { return (int)this._properties["Rows"]; }
            set { this._properties["Rows"] = value; }
        }

        public int Cols
        {
            get { return (int)this._properties["Cols"]; }
            set { this._properties["Cols"] = value; }
        }

        public int MapDataForm
        {
            get { return (int)this._properties["MapDataForm"]; }
            set { this._properties["MapDataForm"] = value; }
        }

        public override string WaferID
        {
            get { return this._properties["WaferID"].ToString(); }
            set { this._properties["WaferID"] = value; }
        }

        public byte ProbingNo
        {
            get { return (byte)this._properties["ProbingNo"]; }
            set { this._properties["ProbingNo"] = value; }
        }

        public override string LotNo
        {
            get { return this._properties["LotNo"].ToString(); }
            set { this._properties["LotNo"] = value; }
        }

        public int CassetteNo
        {
            get { return (int)this._properties["CassetteNo"]; }
            set { this._properties["CassetteNo"] = value; }
        }

        public int SlotNo
        {
            get { return (int)this._properties["SlotNo"]; }
            set { this._properties["SlotNo"] = value; }
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

        public DateTime StartTime
        {
            get { return (DateTime)this._properties["StartTime"]; }
            set { this._properties["StartTime"] = value; }
        }

        public DateTime EndTime
        {
            get { return (DateTime)this._properties["EndTime"]; }
            set { this._properties["EndTime"] = value; }
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

        public Tsk(string file)
            : base(ConstDefine.FileType_TSK, file)
        {
            this.ExtendHeadFlag = false;
            this.ExtendFlag = false;
            this.ExtendFlag2 = false;
            this.ExtendList = new ArrayList();
        }

        // 从 mapping 文件完整文件名中解析出文件名
        protected override string GetFileName(string str)
        {
            try
            {
                return str.Substring(str.LastIndexOf('\\') + 1);
            }
            catch
            {
                return str;
            }
        }

        protected override void InitialProperties()
        {

            this._keys.Add("Operator");
            this._keys.Add("Device");
            this._keys.Add("WaferSize");
            this._keys.Add("MachineNo");
            this._keys.Add("IndexSizeX");
            this._keys.Add("IndexSizeY");
            this._keys.Add("FlatDir");

            this._keys.Add("MachineType");
            this._keys.Add("MapVersion");
            this._keys.Add("Rows");
            this._keys.Add("Cols");
            this._keys.Add("MapDataForm");

            this._keys.Add("WaferID");
            this._keys.Add("ProbingNo");
            this._keys.Add("LotNo");
            this._keys.Add("CassetteNo");
            this._keys.Add("SlotNo");

            this._keys.Add("XCoordinates");
            this._keys.Add("YCoordinates");
            this._keys.Add("RefeDir");
            this._keys.Add("Reserved0");
            this._keys.Add("TargetX");
            this._keys.Add("TargetY");

            this._keys.Add("Refpx");
            this._keys.Add("Refpy");

            this._keys.Add("ProbingSP");
            this._keys.Add("ProbingDir");
            this._keys.Add("Reserved1");
            this._keys.Add("DistanceX");
            this._keys.Add("DistanceY");
            this._keys.Add("CoordinatorX");
            this._keys.Add("CoordinatorY");
            this._keys.Add("FirstDirX");
            this._keys.Add("FirstDirY");

            this._keys.Add("StartTime");
            this._keys.Add("EndTime");
            this._keys.Add("LoadTime");
            this._keys.Add("UnloadTime");

            this._keys.Add("MachineNo1");
            this._keys.Add("MachineNo2");
            this._keys.Add("SpecialChar");
            this._keys.Add("TestingEnd");
            this._keys.Add("Reserved2");

            this._keys.Add("TotalDie");
            this._keys.Add("PassDie");
            this._keys.Add("FailDie");

            this._keys.Add("DieStartPosition");

            this._keys.Add("LineCategoryNo");
            this._keys.Add("LineCategoryAddr");
            this._keys.Add("Configuration");
            this._keys.Add("MaxMultiSite");
            this._keys.Add("MaxCategories");
            this._keys.Add("Reserved3");

            // 用于标识是否有扩展数据
            this._keys.Add("ExtendHeadFlag");
            this._keys.Add("ExtendFlag");
            this._keys.Add("ExtensionHead_20");
            this._keys.Add("ExtensionHead_32");
            this._keys.Add("ExtensionHead_total");
            this._keys.Add("ExtensionHead_pass");
            this._keys.Add("ExtensionHead_fail");
            this._keys.Add("ExtensionHead_44");
            this._keys.Add("ExtensionHead_64");
            // 用于标识是否有额外扩展数据，只用于保存，不做修改
            this._keys.Add("ExtendFlag2");
            this._keys.Add("ExtendList");

            this._properties.Add("Operator", "");
            this._properties.Add("Device", "");
            this._properties.Add("WaferSize", 0);
            this._properties.Add("MachineNo", 0);
            this._properties.Add("IndexSizeX", 0);
            this._properties.Add("IndexSizeY", 0);
            this._properties.Add("FlatDir", 0);

            this._properties.Add("MachineType", (byte)0);
            this._properties.Add("MapVersion", (byte)0);
            this._properties.Add("Rows", 0);
            this._properties.Add("Cols", 0);
            this._properties.Add("MapDataForm", 0);

            this._properties.Add("WaferID", "");
            this._properties.Add("ProbingNo", (byte)0);
            this._properties.Add("LotNo", "");
            this._properties.Add("CassetteNo", 0);
            this._properties.Add("SlotNo", 0);

            this._properties.Add("XCoordinates", (byte)0);
            this._properties.Add("YCoordinates", (byte)0);
            this._properties.Add("RefeDir", (byte)0);
            this._properties.Add("Reserved0", (byte)0);
            this._properties.Add("TargetX", 0);
            this._properties.Add("TargetY", 0);

            this._properties.Add("Refpx", 0);
            this._properties.Add("Refpy", 0);

            this._properties.Add("ProbingSP", (byte)0);
            this._properties.Add("ProbingDir", (byte)0);
            this._properties.Add("Reserved1", (short)0);
            this._properties.Add("DistanceX", 0);
            this._properties.Add("DistanceY", 0);
            this._properties.Add("CoordinatorX", 0);
            this._properties.Add("CoordinatorY", 0);
            this._properties.Add("FirstDirX", 0);
            this._properties.Add("FirstDirY", 0);

            this._properties.Add("StartTime", new DateTime(1900, 1, 1));
            this._properties.Add("EndTime", new DateTime(1900, 1, 1));
            this._properties.Add("LoadTime", new DateTime(1900, 1, 1));
            this._properties.Add("UnloadTime", new DateTime(1900, 1, 1));

            this._properties.Add("MachineNo1", 0);
            this._properties.Add("MachineNo2", 0);
            this._properties.Add("SpecialChar", 0);
            this._properties.Add("TestingEnd", (byte)0);
            this._properties.Add("Reserved2", (byte)0);

            this._properties.Add("TotalDie", 0);
            this._properties.Add("PassDie", 0);
            this._properties.Add("FailDie", 0);

            this._properties.Add("LineCategoryNo", 0);
            this._properties.Add("LineCategoryAddr", 0);
            this._properties.Add("Configuration", (short)0);
            this._properties.Add("MaxMultiSite", (short)0);
            this._properties.Add("MaxCategories", (short)0);
            this._properties.Add("Reserved3", (short)0);

            this._properties.Add("ExtendHeadFlag", false);
            this._properties.Add("ExtendFlag", false);

            this._properties.Add("ExtensionHead_20", new byte[20]);
            this._properties.Add("ExtensionHead_32", new byte[32]);
            this._properties.Add("ExtensionHead_total", (int)0);
            this._properties.Add("ExtensionHead_pass", (int)0);
            this._properties.Add("ExtensionHead_fail", (int)0);
            this._properties.Add("ExtensionHead_44", new byte[44]);
            this._properties.Add("ExtensionHead_64", new byte[64]);
        }

        public override void Read()
        {
            try
            {
                // 打开读取流
                this.OpenReader();

                this.Operator = this.ReadToString(20);
                this.Device = this.ReadToString(16);
                this.WaferSize = this.ReadToInt16();
                this.MachineNo = this.ReadToInt16();
                this.IndexSizeX = this.ReadToInt32();
                this.IndexSizeY = this.ReadToInt32();
                this.FlatDir = this.ReadToInt16();
                this.MachineType = this.ReadToByte();
                this.MapVersion = this.ReadToByte();

                int rows = this.ReadToInt16();// 记录列数 x轴的最大值
                int cols = this.ReadToInt16();// 记录行数 y轴的最大值
                this.Rows = rows;
                this.Cols = cols;

                this.MapDataForm = this.ReadToInt32();
                this.WaferID = this.ReadToString(21).TrimEnd('\0');

                //  this.WaferID = WaferID.Trim("\0".ToCharArray());  

                this.ProbingNo = this.ReadToByte();
                this.LotNo = this.ReadToString(18);
                this.CassetteNo = this.ReadToInt16();
                this.SlotNo = this.ReadToInt16();

                // X coordinates increase direction   XCoordinates 1 leftforward 负, 2 rightforward 正
                this._properties["XCoordinates"] = this.ReadToByte();
                // Y coordinates increase direction   YCoordinates 1 forward 正, 2 backforward 负
                this._properties["YCoordinates"] = this.ReadToByte();
                // Reference dir setting procedures
                this._properties["RefeDir"] = this.ReadToByte();
                // (Reserved)
                this._properties["Reserved0"] = this.ReadToByte();
                // Target die position X
                this._properties["TargetX"] = this.ReadToInt32();
                // Target die position Y
                this._properties["TargetY"] = this.ReadToInt32();

                /*
                 * **********************************
                 */
                this.Refpx = this.ReadToInt16();
                this.Refpy = this.ReadToInt16();

                //// Refrence die coordinator X
                //this._reader.BaseStream.Position += 2;
                //// Refrence die coordinator Y
                //this._reader.BaseStream.Position += 2;
                /*
                 * **********************************
                 */

                // Probing start position
                this._properties["ProbingSP"] = this.ReadToByte();
                // Probing direction
                this._properties["ProbingDir"] = this.ReadToByte();
                // (Reserved)
                this._properties["Reserved1"] = this.ReadToInt16();
                // Distance X to wafer center die origin
                this._properties["DistanceX"] = this.ReadToInt32();
                // Distance Y to wafer center die origin
                this._properties["DistanceY"] = this.ReadToInt32();
                // Coordinator X of wafer center die
                this._properties["CoordinatorX"] = this.ReadToInt32();
                // Coordinator Y of wafer center die
                this._properties["CoordinatorY"] = this.ReadToInt32();
                // First dir coordinator X
                this._properties["FirstDirX"] = this.ReadToInt32();
                // First dir coordinator Y
                this._properties["FirstDirY"] = this.ReadToInt32();

                this.StartTime = this.ReadToDate();
                this.EndTime = this.ReadToDate();
                this.LoadTime = this.ReadToDate();
                this.UnloadTime = this.ReadToDate();

                // Machine No.
                this._properties["MachineNo1"] = this.ReadToInt32();
                // Machine No.
                this._properties["MachineNo2"] = this.ReadToInt32();
                // Special characters
                this._properties["SpecialChar"] = this.ReadToInt32();
                // Testing end information
                this._properties["TestingEnd"] = this.ReadToByte();
                // (Reserved)
                this._properties["Reserved2"] = this.ReadToByte();

                this.TotalDie = this.ReadToInt16();
                this.PassDie = this.ReadToInt16();
                this.FailDie = this.ReadToInt16();

                // 记录 die 测试数据起始指针
                int dieStartPosition = this.ReadToInt32();
                this._properties["DieStartPosition"] = dieStartPosition;

                // Number of line category data
                this._properties["LineCategoryNo"] = this.ReadToInt32();
                // Line category address
                this._properties["LineCategoryAddr"] = this.ReadToInt32();
                // Map file configuration
                this._properties["Configuration"] = this.ReadToInt16();
                // Max. multi site
                this._properties["MaxMultiSite"] = this.ReadToInt16();
                // Max. categories
                this._properties["MaxCategories"] = this.ReadToInt16();
                // Do not use,reserved
                this._properties["Reserved3"] = this.ReadToInt16();

                // 设置流的起始指针为 die 测试数据起始指针
                this._reader.BaseStream.Position = dieStartPosition;

                int total = rows * cols;
                ArrayList arry = new ArrayList();


                for (int i = 0; i < total; i++)
                {
                    arry.Add(this.ReadDie(i));
                }

                this._dieMatrix = new DieMatrix(arry, rows, cols);

                if (this._reader.BaseStream.Position < this._reader.BaseStream.Length)
                {
                    this.ExtendHeadFlag = true;

                    this._properties["ExtensionHead_20"] = this._reader.ReadBytes(20);
                    this._properties["ExtensionHead_32"] = this._reader.ReadBytes(32);
                    this._properties["ExtensionHead_total"] = this.ReadToInt32();
                    this._properties["ExtensionHead_pass"] = this.ReadToInt32();
                    this._properties["ExtensionHead_fail"] = this.ReadToInt32();
                    this._properties["ExtensionHead_44"] = this._reader.ReadBytes(44);
                    this._properties["ExtensionHead_64"] = this._reader.ReadBytes(64);
                }




                while (this._reader.BaseStream.Position < this._reader.BaseStream.Length)
                {
                    this.ExtendFlag = true;

                    for (int k = 0; k < arry.Count; k++)
                    {
                        byte[] buffer = this._reader.ReadBytes(2);
                        byte[] buffer2 = this._reader.ReadBytes(2);
                        int extSite;
                        int extCategory;
                        if (this.DieMatrix[k].Attribute == DieCategory.FailDie || this.DieMatrix[k].Attribute == DieCategory.PassDie)
                        {
                            if (this.MapVersion == 4 || this.MapVersion == 7)
                            {
                                extSite = buffer[1];
                                extCategory = buffer2[1];
                            }
                            else
                            {
                                extSite = buffer[0];
                                extCategory = buffer[1];
                            }
                            if (this.DieMatrix[k].Attribute == DieCategory.FailDie)
                            {
                                if (extCategory == 0 || extCategory == 1)
                                {
                                    Console.WriteLine("error");
                                    continue;//只要bin不是超过64的，可以跳过
                                }
                            }

                            if (this.DieMatrix[k].Attribute == DieCategory.PassDie)
                            {
                                if (extCategory != 1)
                                {
                                    Console.WriteLine("error");
                                    continue;//只要bin不是超过64的，可以跳过
                                }
                            }
                            this.DieMatrix[k].Bin = extCategory;
                            this.DieMatrix[k].Site = extSite;
                        }
                        //Debug
                        //if (die.Attribute == DieCategory.FailDie || die.Attribute == DieCategory.PassDie)
                        //{
                        //    if (extCategory == 0)
                        //    {
                        //        Console.WriteLine("error");
                        //    }
                        //}
                    }
                    break; //可以注释
                }

                while (this._reader.BaseStream.Position < this._reader.BaseStream.Length)
                {
                    this.ExtendFlag2 = true;
                    //继续读取余下的数据并保存到list中
                    ExtendList.Add(this._reader.ReadByte());
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

        private DieData ReadDie(int index)
        {
            /*
             * First word
             */
            byte[] buffer = this._reader.ReadBytes(2);
            int f7 = buffer[0];
            int f8 = buffer[1];

            // needle mark inspection result(added jan/23'96)(not handled)
            int f5 = (buffer[0] >> 1) & 0x1;
            // re-probing result
            int f4 = (buffer[0] >> 2) & 0x3;
            // fail mark inspection
            int f3 = (buffer[0] >> 4) & 0x1;
            // marking
            int f2 = (buffer[0] >> 5) & 0x1;
            // die test result
            int dieTestResult = (buffer[0] >> 6) & 0x3;

            // die coordinator values * (0 to 511)
            buffer[0] = (byte)(buffer[0] & 0x1);
            this.Reverse(ref buffer);
            int f6 = BitConverter.ToInt16(buffer, 0);


            /*
             * Second word
             */
            buffer = this._reader.ReadBytes(2);
            int s8 = buffer[0];
            int s9 = buffer[1];

            // Dummy data(excerpt warfer)
            int s6 = (buffer[0] >> 1) & 0x1;//Dummy Data (except wafer) 1 skip 0 test die
            // code bit of coordinator value x
            int s5 = (buffer[0] >> 2) & 0x1;
            // code bit of coordinator value y
            int s4 = (buffer[0] >> 3) & 0x1;
            // sampling die
            int s3 = (buffer[0] >> 4) & 0x1;
            // needle marking inspection execution die selection
            int s2 = (buffer[0] >> 5) & 0x1;
            // die property
            int dieProperty = (buffer[0] >> 6) & 0x3;

            // die coordinator value Y
            buffer[0] = (byte)(buffer[0] & 0x1);
            this.Reverse(ref buffer);
            int s7 = BitConverter.ToInt16(buffer, 0);



            /*
             * Third word
             */
            buffer = this._reader.ReadBytes(2);
            int t8 = buffer[0];
            int t9 = buffer[1];

            // test execution site no.(0 to 63)
            int t3 = buffer[0] & 0x3f;
            // reject chip flag
            int t2 = (buffer[0] >> 6) & 0x1;
            // measurement finish flag at "No-Over-Travel" probing
            int t1 = (buffer[0] >> 7) & 0x1;

            // According to user special,8-bit area may be used.
            int t6 = buffer[1];
            // category data (0 to 63)
            int binNum = buffer[1] & 0x3f;
            int t7 = buffer[0];

            // block area judgement function
            int t4 = (buffer[0] >> 6) & 0x3;

            DieData die = new DieData();

            switch (dieProperty)
            {
                case 0:
                    // die.Attribute = DieCategory.SkipDie;
                    // break;
                    if (s6 == 1)
                    {
                        die.Attribute = DieCategory.SkipDie;
                        break;
                    }
                    else if (s6 == 0)
                    {
                        die.Attribute = DieCategory.SkipDie2;
                        break;
                    }
                    break;

                case 1:
                    switch (dieTestResult)
                    {
                        case 0:
                            die.Attribute = DieCategory.NoneDie;
                            break;
                        case 1:
                            //die.Attribute = DieCategory.PassDie;
                            die.Attribute = DieCategory.PassDie;
                            die.Bin = binNum;
                            die.Site = t3;
                            if (binNum != 1)
                            {
                                Console.WriteLine("error");
                            }
                            break;
                        case 2:
                        case 3:
                            die.Attribute = DieCategory.FailDie;
                            die.Bin = binNum;
                            die.Site = t3;
                            if (binNum == 0 || binNum == 1)
                            {
                                Console.WriteLine("error");
                            }
                            break;
                        default:
                            die.Attribute = DieCategory.Unknow;
                            break;
                    }
                    break;
                case 2:
                    die.Attribute = DieCategory.MarkDie;
                    break;
                default:
                    die.Attribute = DieCategory.Unknow;
                    break;
            }

            ////原来计算x和y坐标的方法
            //die.X = s4 == 0 ? f6 : f6 * (-1);
            //die.Y = s5 == 0 ? s7 : s7 * (-1);
            // X coordinates increase direction   XCoordinates 1 leftforward 负, 2 rightforward 正
            die.X = Convert.ToInt32(this._properties["FirstDirX"]) +
                (Convert.ToInt32(this._properties["XCoordinates"]).Equals(2) ? index % this.Rows : -index % this.Rows);

            // Y coordinates increase direction   YCoordinates 1 forward 正, 2 backforward 负
            die.Y = Convert.ToInt32(this._properties["FirstDirY"]) +
                (Convert.ToInt32(this._properties["YCoordinates"]).Equals(1) ? index / this.Rows : -index / this.Rows);

            return die;
        }

        /// <summary>
        /// 将数据保存为 tma 文件
        /// </summary>
        public override void Save()
        {
            try
            {
                byte[] buf;

                // 打开或创建文件
                this.OpenWriter();

                string str = string.Format("{0,-20:G}", this.Operator);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 20);

                str = string.Format("{0,-16:G}", this.Device);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 16);

                buf = BitConverter.GetBytes((short)this.WaferSize);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes((short)this.MachineNo);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes(this.IndexSizeX);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                buf = BitConverter.GetBytes(this.IndexSizeY);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                buf = BitConverter.GetBytes((short)this.FlatDir);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.MachineType), 0, 1);
                this._writer.Write(BitConverter.GetBytes(this.MapVersion), 0, 1);

                buf = BitConverter.GetBytes((short)this.DieMatrix.XMax);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes((short)this.DieMatrix.YMax);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);

                buf = BitConverter.GetBytes(this.MapDataForm);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);

                str = string.Format("{0,-21:G}", this.WaferID);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 21);
                this._writer.Write(BitConverter.GetBytes(this.ProbingNo), 0, 1);

                str = string.Format("{0,-18:G}", this.LotNo);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 18);

                buf = BitConverter.GetBytes((short)this.CassetteNo);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes((short)this.SlotNo);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);

                // X coordinates increase direction
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["XCoordinates"]), 0, 1);
                // Y coordinates increase direction
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["YCoordinates"]), 0, 1);
                // Reference dir setting procedures
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["RefeDir"]), 0, 1);
                // (Reserved)
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["Reserved0"]), 0, 1);

                // Target die position X
                buf = BitConverter.GetBytes((int)this._properties["TargetX"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);

                // Target die position Y
                buf = BitConverter.GetBytes((int)this._properties["TargetY"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);

                buf = BitConverter.GetBytes((short)this.Refpx);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes((short)this.Refpy);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);

                // Probing start position
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["ProbingSP"]), 0, 1);
                // Probing direction
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["ProbingDir"]), 0, 1);

                // (Reserved)
                buf = BitConverter.GetBytes((short)this._properties["Reserved1"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);

                // Distance X to wafer center die origin
                buf = BitConverter.GetBytes((int)this._properties["DistanceX"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Distance Y to wafer center die origin
                buf = BitConverter.GetBytes((int)this._properties["DistanceY"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Coordinator X of wafer center die
                buf = BitConverter.GetBytes((int)this._properties["CoordinatorX"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Coordinator Y of wafer center die
                buf = BitConverter.GetBytes((int)this._properties["CoordinatorY"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // First dir coordinator X
                buf = BitConverter.GetBytes((int)this._properties["FirstDirX"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // First dir coordinator Y
                buf = BitConverter.GetBytes((int)this._properties["FirstDirY"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);

                // start time
                str = this.StartTime.Year.ToString().Substring(2);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.StartTime.Month.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.StartTime.Day.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.StartTime.Hour.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.StartTime.Minute.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                // (Reserved)
                this._writer.Write(Encoding.ASCII.GetBytes("00"), 0, 2);

                // end time
                str = this.EndTime.Year.ToString().Substring(2);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.EndTime.Month.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.EndTime.Day.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.EndTime.Hour.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.EndTime.Minute.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                // (Reserved)
                this._writer.Write(Encoding.ASCII.GetBytes("00"), 0, 2);

                // load time
                str = this.LoadTime.Year.ToString().Substring(2);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.LoadTime.Month.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.LoadTime.Day.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.LoadTime.Hour.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.LoadTime.Minute.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                // (Reserved)
                this._writer.Write(Encoding.ASCII.GetBytes("00"), 0, 2);

                // unload time
                str = this.UnloadTime.Year.ToString().Substring(2);
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.UnloadTime.Month.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.UnloadTime.Day.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.UnloadTime.Hour.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                str = String.Format("{0,2:G}", this.UnloadTime.Minute.ToString());
                this._writer.Write(Encoding.ASCII.GetBytes(str), 0, 2);
                // (Reserved)
                this._writer.Write(Encoding.ASCII.GetBytes("00"), 0, 2);

                // Machine No.
                buf = BitConverter.GetBytes((int)this._properties["MachineNo1"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Machine No.
                buf = BitConverter.GetBytes((int)this._properties["MachineNo2"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Special characters
                buf = BitConverter.GetBytes((int)this._properties["SpecialChar"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Testing end information
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["TestingEnd"]), 0, 1);
                // (Reserved)
                this._writer.Write(BitConverter.GetBytes((byte)this._properties["Reserved2"]), 0, 1);

                buf = BitConverter.GetBytes((short)this.TotalDie);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes((short)this.PassDie);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                buf = BitConverter.GetBytes((short)this.FailDie);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);

                // 记录 die 测试数据起始指针
                buf = BitConverter.GetBytes((int)this._properties["DieStartPosition"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);

                // Number of line category data
                buf = BitConverter.GetBytes((int)this._properties["LineCategoryNo"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Line category address
                buf = BitConverter.GetBytes((int)this._properties["LineCategoryAddr"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 4);
                // Map file configuration
                buf = BitConverter.GetBytes((short)this._properties["Configuration"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                // Max. multi site
                buf = BitConverter.GetBytes((short)this._properties["MaxMultiSite"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                // Max. categories
                buf = BitConverter.GetBytes((short)this._properties["MaxCategories"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);
                // Do not use,reserved
                buf = BitConverter.GetBytes((short)this._properties["Reserved3"]);
                this.Reverse(ref buf);
                this._writer.Write(buf, 0, 2);

                // write die data
                for (int i = 0; i < this.DieMatrix.YMax; i++)
                {
                    for (int j = 0; j < this.DieMatrix.XMax; j++)
                    {
                        this.WriteDie(this.DieMatrix[j, i]);
                    }
                }
                if ((bool)this._properties["ExtendHeadFlag"])
                {
                    // Extension head 20
                    this._writer.Write((byte[])this._properties["ExtensionHead_20"], 0, 20);
                    // Extension head 32
                    this._writer.Write((byte[])this._properties["ExtensionHead_32"], 0, 32);
                    // Extension head total
                    buf = BitConverter.GetBytes((int)this.TotalDie);
                    this.Reverse(ref buf);
                    this._writer.Write(buf, 0, 4);
                    // Extension head pass
                    buf = BitConverter.GetBytes((int)this.PassDie);
                    this.Reverse(ref buf);
                    this._writer.Write(buf, 0, 4);
                    // Extension head fail
                    buf = BitConverter.GetBytes((int)this.FailDie);
                    this.Reverse(ref buf);
                    this._writer.Write(buf, 0, 4);
                    // Extension head 44
                    this._writer.Write((byte[])this._properties["ExtensionHead_44"], 0, 44);
                    // Extension head 64
                    this._writer.Write((byte[])this._properties["ExtensionHead_64"], 0, 64);
                }
                if ((bool)this._properties["ExtendFlag"])
                {
                    // write extend data
                    for (int i = 0; i < this.DieMatrix.YMax; i++)
                    {
                        for (int j = 0; j < this.DieMatrix.XMax; j++)
                        {
                            this.WriteDieExtention(this.DieMatrix[j, i]);
                        }
                    }
                }
                if ((bool)this._properties["ExtendFlag2"])
                {
                    ArrayList extendList = (ArrayList)this._properties["ExtendList"];
                    foreach (byte extendByte in extendList)
                    {
                        this._writer.WriteByte(extendByte);
                    }
                }
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

        private void WriteDie(DieData d)
        {
            ushort f = (ushort)Math.Abs(d.X);
            f = (ushort)(f & (ushort)0x01ff);// x cord
            ushort s = (ushort)Math.Abs(d.Y);
            s = (ushort)(s & (ushort)0x01ff);// y cord

            ushort t = (ushort)d.Bin;

            switch (d.Attribute)
            {
                case DieCategory.FailDie:
                case DieCategory.TIRefFail:
                    f = (ushort)(f | (ushort)0x8000);//Fail
                    s = (ushort)(s | (ushort)0x4000);//01 000000 probing
                    break;
                case DieCategory.MarkDie:
                    s = (ushort)(s | (ushort)0x8000);//10 000000 mark
                    break;
                case DieCategory.NoneDie:
                    f = (ushort)(f | (ushort)0x0000);//None
                    s = (ushort)(s | (ushort)0x4000);//01 000000 none probing
                    break;
                case DieCategory.PassDie:
                case DieCategory.TIRefPass:
                    f = (ushort)(f | (ushort)0x4000);//Pass
                    s = (ushort)(s | (ushort)0x4000);//01 000000 probing
                    break;
                case DieCategory.SkipDie:
                    s = (ushort)(s | (ushort)0x0000);//00 000000 skip
                    s = (ushort)(s | (ushort)0x0200);//000000 1 0 skip2
                    break;
                case DieCategory.SkipDie2:
                    s = (ushort)(s | (ushort)0x0000);//00 000000 skip
                    s = (ushort)(s | (ushort)0x0000);//000000 0 0 skip
                    break;
                case DieCategory.Unknow:
                    s = (ushort)(s | (ushort)0xc000);
                    break;
            }

            if (d.X < 0)
                s = (ushort)(s | (ushort)0x0800);

            if (d.Y < 0)
                s = (ushort)(s | (ushort)0x0400);

            byte[] fb = BitConverter.GetBytes(f);
            byte[] sb = BitConverter.GetBytes(s);
            byte[] tb = BitConverter.GetBytes(t);

            // first word
            this.Reverse(ref fb);
            this._writer.Write(fb, 0, 2);

            // second word
            this.Reverse(ref sb);
            this._writer.Write(sb, 0, 2);

            // third word
            this.Reverse(ref tb);
            this._writer.Write(tb, 0, 2);
        }

        private void WriteDieExtention(DieData d)
        {
            ushort binNo = (ushort)d.Bin;
            if (this.MapVersion == 2)
            {
                byte[] extentionFirstWord = BitConverter.GetBytes(binNo);
                byte[] extentionSecondWord = BitConverter.GetBytes(0);
                this.Reverse(ref extentionFirstWord);
                this._writer.Write(extentionFirstWord, 0, 2);

                this._writer.Write(extentionSecondWord, 0, 2);
            }
            else
            {
                byte[] extentionFirstWord = BitConverter.GetBytes(0);
                byte[] extentionSecondWord = BitConverter.GetBytes(binNo);

                this._writer.Write(extentionFirstWord, 0, 2);

                this.Reverse(ref extentionSecondWord);
                this._writer.Write(extentionSecondWord, 0, 2);
            }

        }


        /// <summary>
        /// 判断 mapping 文件的 die 矩阵中的一个 die 是否为空 die
        /// </summary>
        /// <param name="die"></param>
        /// <returns></returns>
        public override bool IsEmptyDie(DieData die)
        {
            if (die.Attribute == DieCategory.NoneDie)
                return true;
            else
                return false;
        }

        // 合并 Tsk 文件
        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            if (!(map is Tsk))
                throw new Exception("Tsk 类型文件只能和 Tsk 类型文件合并。");

            for (int i = 0; i < this.Rows * this.Cols; i++)
            {
                DieData die = map.DieMatrix[i];
                DieData mergeDie = this.DieMatrix[i];
                if (die.Attribute == DieCategory.FailDie)
                {
                    mergeDie.Attribute = DieCategory.FailDie;
                    mergeDie.Bin = die.Bin;
                }
            }

            this.PassDie = 0;
            this.FailDie = 0;
            for (int k = 0; k < this.Rows * this.Cols; k++)
            {
                if (this.DieMatrix[k].Attribute == DieCategory.PassDie)
                {
                    this.PassDie++;
                }
                else if (this.DieMatrix[k].Attribute == DieCategory.FailDie)
                {
                    this.FailDie++;
                }
            }
            this.TotalDie = this.PassDie + this.FailDie;

            this.FullName = newfile;
            this.Save();
            return this;
        }
    }
}