
/*
 * 作者：sky
 * 时间：2008-01-09
 * 作用：用于描述 Mapping 中的 Dat 文件格式
 */

namespace DataToExcel
{
    using System;
    using System.Text;
    using System.Collections;
    using DataToExcel;

    public class Dat : MappingBase
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

        public Dat(string file)
            : base(ConstDefine.FileType_DAT, file)
        {
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
            this._keys.Add("MapDataForm");

            this._keys.Add("WaferID");
            this._keys.Add("ProbingNo");
            this._keys.Add("LotNo");
            this._keys.Add("CassetteNo");
            this._keys.Add("SlotNo");

            this._keys.Add("Refpx");
            this._keys.Add("Refpy");

            this._keys.Add("StartTime");
            this._keys.Add("EndTime");
            this._keys.Add("LoadTime");
            this._keys.Add("UnloadTime");

            this._keys.Add("TotalDie");
            this._keys.Add("PassDie");
            this._keys.Add("FailDie");

            this._properties.Add("Operator", "");
            this._properties.Add("Device", "");
            this._properties.Add("WaferSize", 0);
            this._properties.Add("MachineNo", 0);
            this._properties.Add("IndexSizeX", 0);
            this._properties.Add("IndexSizeY", 0);
            this._properties.Add("FlatDir", 0);

            this._properties.Add("MachineType", 0);
            this._properties.Add("MapVersion", 0);
            this._properties.Add("MapDataForm", 0);

            this._properties.Add("WaferID", "");
            this._properties.Add("ProbingNo", 0);
            this._properties.Add("LotNo", "");
            this._properties.Add("CassetteNo", 0);
            this._properties.Add("SlotNo", 0);

            this._properties.Add("Refpx", 0);
            this._properties.Add("Refpy", 0);

            this._properties.Add("StartTime", new DateTime(1900, 1, 1));
            this._properties.Add("EndTime", new DateTime(1900, 1, 1));
            this._properties.Add("LoadTime", new DateTime(1900, 1, 1));
            this._properties.Add("UnloadTime", new DateTime(1900, 1, 1));

            this._properties.Add("TotalDie", 0);
            this._properties.Add("PassDie", 0);
            this._properties.Add("FailDie", 0);
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

                int rows = this.ReadToInt16();// 记录行数
                int cols = this.ReadToInt16();// 记录列数

                this.MapDataForm = this.ReadToInt32();
                this.WaferID = this.ReadToString(21);
                this.ProbingNo = this.ReadToByte();
                this.LotNo = this.ReadToString(18);
                this.CassetteNo = this.ReadToInt16();
                this.SlotNo = this.ReadToInt16();

                // X coordinates increase direction
                this._reader.BaseStream.Position += 1;
                // Y coordinates increase direction
                this._reader.BaseStream.Position += 1;
                // Reference dir setting procedures
                this._reader.BaseStream.Position += 1;
                // (Reserved)
                this._reader.BaseStream.Position += 1;
                // Target die position X
                this._reader.BaseStream.Position += 4;
                // Target die position Y
                this._reader.BaseStream.Position += 4;

                this.Refpx = this.ReadToInt16();
                this.Refpy = this.ReadToInt16();

                //// Refrence die coordinator X
                //this._reader.BaseStream.Position += 2;
                //// Refrence die coordinator Y
                //this._reader.BaseStream.Position += 2;
                // Probing start position
                this._reader.BaseStream.Position += 1;
                // Probing direction
                this._reader.BaseStream.Position += 1;
                // (Reserved)
                this._reader.BaseStream.Position += 2;
                // Distance X to wafer center die origin
                this._reader.BaseStream.Position += 4;
                // Distance Y to wafer center die origin
                this._reader.BaseStream.Position += 4;
                // Coordinator X of wafer center die
                this._reader.BaseStream.Position += 4;
                // Coordinator Y of wafer center die
                this._reader.BaseStream.Position += 4;
                // First dir coordinator X
                this._reader.BaseStream.Position += 4;
                // First dir coordinator Y
                this._reader.BaseStream.Position += 4;

                this.StartTime = this.ReadToDate();
                this.EndTime = this.ReadToDate();
                this.LoadTime = this.ReadToDate();
                this.UnloadTime = this.ReadToDate();

                // Machine No.
                this._reader.BaseStream.Position += 4;
                // Machine No.
                this._reader.BaseStream.Position += 4;
                // Special characters
                this._reader.BaseStream.Position += 4;
                // Testing end information
                this._reader.BaseStream.Position += 1;
                // (Reserved)
                this._reader.BaseStream.Position += 1;

                this.TotalDie = this.ReadToInt16();
                this.PassDie = this.ReadToInt16();
                this.FailDie = this.ReadToInt16();

                // 记录 die 测试数据起始指针
                int dieSP = this.ReadToInt32();

                // Number of line category data
                this._reader.ReadBytes(4);
                // Line category address
                this._reader.ReadBytes(4);
                // Map file configuration
                this._reader.ReadBytes(2);
                // Max. multi site
                this._reader.ReadBytes(2);
                // Max. categories
                this._reader.ReadBytes(2);
                // Do not use,reserved
                this._reader.ReadBytes(2);

                // 设置流的起始指针为 die 测试数据起始指针
                this._reader.BaseStream.Position = dieSP;

                int total = rows * cols;
                ArrayList arry = new ArrayList();

                for (int i = 0; i < total; i++)
                {
                    arry.Add(this.ReadDie());
                }

                this._dieMatrix = new DieMatrix(arry, rows, cols);
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

        private DieData ReadDie()
        {
            /*
             * First word
             */
            byte[] buffer = this._reader.ReadBytes(2);

            // needle mark inspection result(added jan/23'96)(not handled)
            int f5 = (buffer[0] >> 1) & 0x1;
            // re-probing result
            int f4 = (buffer[0] >> 2) & 0x3;
            // fail mark inspection
            int f3 = (buffer[0] >> 4) & 0x1;
            // marking
            int f2 = (buffer[0] >> 5) & 0x1;
            // die test result
            int f1 = (buffer[0] >> 6) & 0x3;

            // die coordinator values * (0 to 511)
            buffer[0] = (byte)(buffer[0] & 0x1);
            this.Reverse(ref buffer);
            int f6 = BitConverter.ToInt16(buffer, 0);

            /*
             * Second word
             */
            buffer = this._reader.ReadBytes(2);

            // Dummy data(excerpt warfer)
            int s6 = (buffer[0] >> 1) & 0x1;
            // code bit of coordinator value x
            int s5 = (buffer[0] >> 2) & 0x1;
            // code bit of coordinator value y
            int s4 = (buffer[0] >> 3) & 0x1;
            // sampling die
            int s3 = (buffer[0] >> 4) & 0x1;
            // needle marking inspection execution die selection
            int s2 = (buffer[0] >> 5) & 0x1;
            // die property
            int s1 = (buffer[0] >> 6) & 0x3;

            // die coordinator value Y
            buffer[0] = (byte)(buffer[0] & 0x1);
            this.Reverse(ref buffer);
            int s7 = BitConverter.ToInt16(buffer, 0);

            /*
             * Third word
             */
            buffer = this._reader.ReadBytes(2);

            // test execution site no.(0 to 63)
            int t3 = buffer[0] & 0x3f;
            // reject chip flag
            int t2 = (buffer[0] >> 6) & 0x1;
            // measurement finish flag at "No-Over-Travel" probing
            int t1 = (buffer[0] >> 7) & 0x1;

            // According to user special,8-bit area may be used.
            int t6 = buffer[1];
            // category data (0 to 63)
            int t5 = buffer[1] & 0x3f;
            // block area judgement function
            int t4 = (buffer[0] >> 6) & 0x3;

            DieData die = new DieData();

            switch (s1)
            {
                case 0:
                    //  die.Attribute = DieCategory.SkipDie;
                    //  break;
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
                    switch (f1)
                    {
                        case 0:
                            die.Attribute = DieCategory.NoneDie;
                            break;
                        case 1:
                            die.Attribute = DieCategory.PassDie;
                            die.Bin = t5 + 1;    //aegon--2019.3.25
                            break;
                        case 2:
                        case 3:
                            die.Attribute = DieCategory.FailDie;
                            die.Bin = t5 + 1;    //zjf 2008.09.27
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

            die.X = s4 == 0 ? f6 : f6 * (-1);
            die.Y = s5 == 0 ? s7 : s7 * (-1);

            return die;
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

                this._writer.Write(Encoding.ASCII.GetBytes(this.Operator), 0, 20);
                this._writer.Write(Encoding.ASCII.GetBytes(this.Device), 0, 16);
                this._writer.Write(BitConverter.GetBytes(this.WaferSize), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.MachineNo), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.IndexSizeX), 0, 4);
                this._writer.Write(BitConverter.GetBytes(this.IndexSizeY), 0, 4);
                this._writer.Write(BitConverter.GetBytes(this.FlatDir), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.MachineType), 0, 1);
                this._writer.Write(BitConverter.GetBytes(this.MapVersion), 0, 1);

                this._writer.Write(BitConverter.GetBytes(this.DieMatrix.YMax), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.DieMatrix.XMax), 0, 2);

                this._writer.Write(BitConverter.GetBytes(this.MapDataForm), 0, 4);
                this._writer.Write(Encoding.ASCII.GetBytes(this.WaferID), 0, 21);
                this._writer.Write(BitConverter.GetBytes(this.ProbingNo), 0, 1);
                this._writer.Write(Encoding.ASCII.GetBytes(this.LotNo), 0, 18);
                this._writer.Write(BitConverter.GetBytes(this.CassetteNo), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.SlotNo), 0, 2);

                // X coordinates increase direction
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // Y coordinates increase direction
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // Reference dir setting procedures
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // (Reserved)
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // Target die position X
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Target die position Y
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);

                this._writer.Write(BitConverter.GetBytes(this.Refpx), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.Refpy), 0, 2);

                // Probing start position
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // Probing direction
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // (Reserved)
                this._writer.Write(BitConverter.GetBytes(0), 0, 2);
                // Distance X to wafer center die origin
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Distance Y to wafer center die origin
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Coordinator X of wafer center die
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Coordinator Y of wafer center die
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // First dir coordinator X
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // First dir coordinator Y
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);

                // start time
                this._writer.Write(BitConverter.GetBytes(this.StartTime.Year - 2000), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.StartTime.Month), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.StartTime.Day), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.StartTime.Hour), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.StartTime.Minute), 0, 2);

                // end time
                this._writer.Write(BitConverter.GetBytes(this.EndTime.Year - 2000), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.EndTime.Month), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.EndTime.Day), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.EndTime.Hour), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.EndTime.Minute), 0, 2);

                // load time
                this._writer.Write(BitConverter.GetBytes(this.LoadTime.Year - 2000), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.LoadTime.Month), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.LoadTime.Day), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.LoadTime.Hour), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.LoadTime.Minute), 0, 2);

                // unload time
                this._writer.Write(BitConverter.GetBytes(this.UnloadTime.Year - 2000), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.UnloadTime.Month), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.UnloadTime.Day), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.UnloadTime.Hour), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.UnloadTime.Minute), 0, 2);

                // Machine No.
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Machine No.
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Special characters
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Testing end information
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);
                // (Reserved)
                this._writer.Write(BitConverter.GetBytes(0), 0, 1);

                this._writer.Write(BitConverter.GetBytes(this.TotalDie), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.PassDie), 0, 2);
                this._writer.Write(BitConverter.GetBytes(this.FailDie), 0, 2);

                // 记录 die 测试数据起始指针
                this._writer.Write(BitConverter.GetBytes(236), 0, 4);

                // Number of line category data
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Line category address
                this._writer.Write(BitConverter.GetBytes(0), 0, 4);
                // Map file configuration
                this._writer.Write(BitConverter.GetBytes(0), 0, 2);
                // Max. multi site
                this._writer.Write(BitConverter.GetBytes(0), 0, 2);
                // Max. categories
                this._writer.Write(BitConverter.GetBytes(0), 0, 2);
                // Do not use,reserved
                this._writer.Write(BitConverter.GetBytes(0), 0, 2);

                // 写入 die 数据
                foreach (DieData d in this.DieMatrix.Items)
                {
                    this.WriteDie(d);
                }

                /*

                int total = rows * cols;
                ArrayList arry = new ArrayList();

                for (int i = 0; i < total; i++)
                {
                    arry.Add(this.ReadDie());
                }
                 */
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
            /*
             * First word
             */
            byte[] buffer = this._reader.ReadBytes(2);

            // needle mark inspection result(added jan/23'96)(not handled)
            int f5 = (buffer[0] >> 1) & 0x1;
            // re-probing result
            int f4 = (buffer[0] >> 2) & 0x3;
            // fail mark inspection
            int f3 = (buffer[0] >> 4) & 0x1;
            // marking
            int f2 = (buffer[0] >> 5) & 0x1;
            // die test result
            int f1 = (buffer[0] >> 6) & 0x3;

            // die coordinator values * (0 to 511)
            buffer[0] = (byte)(buffer[0] & 0x1);
            this.Reverse(ref buffer);
            int f6 = BitConverter.ToInt16(buffer, 0);

            /*
             * Second word
             */
            buffer = this._reader.ReadBytes(2);

            // Dummy data(excerpt warfer)
            int s6 = (buffer[0] >> 1) & 0x1;
            // code bit of coordinator value x
            int s5 = (buffer[0] >> 2) & 0x1;
            // code bit of coordinator value y
            int s4 = (buffer[0] >> 3) & 0x1;
            // sampling die
            int s3 = (buffer[0] >> 4) & 0x1;
            // needle marking inspection execution die selection
            int s2 = (buffer[0] >> 5) & 0x1;
            // die property
            int s1 = (buffer[0] >> 6) & 0x3;

            // die coordinator value Y
            buffer[0] = (byte)(buffer[0] & 0x1);
            this.Reverse(ref buffer);
            int s7 = BitConverter.ToInt16(buffer, 0);

            /*
             * Third word
             */
            buffer = this._reader.ReadBytes(2);

            // test execution site no.(0 to 63)
            int t3 = buffer[0] & 0x3f;
            // reject chip flag
            int t2 = (buffer[0] >> 6) & 0x1;
            // measurement finish flag at "No-Over-Travel" probing
            int t1 = (buffer[0] >> 7) & 0x1;

            // According to user special,8-bit area may be used.
            int t6 = buffer[1];
            // category data (0 to 63)
            int t5 = buffer[1] & 0x3f;
            // block area judgement function
            int t4 = (buffer[0] >> 6) & 0x3;

            DieData die = new DieData();

            switch (s1)
            {
                case 0:
                    die.Attribute = DieCategory.SkipDie;
                    break;
                case 1:
                    switch (f1)
                    {
                        case 0:
                            die.Attribute = DieCategory.NoneDie;
                            break;
                        case 1:
                            die.Attribute = DieCategory.PassDie;
                            break;
                        case 2:
                        case 3:
                            die.Attribute = DieCategory.FailDie;
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

        // 合并 Dat 文件
        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            if (!(map is Dat))
                throw new Exception("Dat 类型文件只能和 Dat 类型文件合并。");

            return null;
        }
    }
}
