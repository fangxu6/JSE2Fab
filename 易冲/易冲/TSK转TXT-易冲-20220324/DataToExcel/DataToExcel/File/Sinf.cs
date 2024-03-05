
/*
 * 作者：sky
 * 时间：2008-04-17
 * 作用：用于描述 Mapping 中的 sinf 文件格式
 */

namespace DataToExcel
{
    using System;
    using System.Collections;

    using DataToExcel;

    public class Sinf : MappingBase
    {
        private ArrayList _diesBuffer;

        public string Device
        {
            get { return this._properties["Device"].ToString(); }
            set { this._properties["Device"] = value; }
        }

        public string LotNo
        {
            get { return this._properties["LotNo"].ToString(); }
            set { this._properties["LotNo"] = value; }
        }

        public string Wafer
        {
            get { return this._properties["Wafer"].ToString(); }
            set { this._properties["Wafer"] = value; }
        }

        public string Fnloc
        {
            get { return this._properties["Fnloc"].ToString(); }
            set { this._properties["Fnloc"] = value; }
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

        public string Bcequ
        {
            get { return this._properties["Bcequ"].ToString(); }
            set { this._properties["Bcequ"] = value; }
        }

        public string Dutms
        {
            get { return this._properties["Dutms"].ToString(); }
            set { this._properties["Dutms"] = value; }
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

        public Sinf(string file)
            : base(ConstDefine.FileType_SINF, file)
        {
        }

        protected override void InitialProperties()
        {
            this._keys.Add("Device");
            this._keys.Add("LotNo");
            this._keys.Add("Wafer");
            this._keys.Add("Fnloc");
            this._keys.Add("Bcequ");
            this._keys.Add("RowCount");
            this._keys.Add("ColCount");
            this._keys.Add("Dutms");

            this._keys.Add("Refpx");
            this._keys.Add("Refpy");

            this._keys.Add("IndexSizeX");
            this._keys.Add("IndexSizeY");

            this._properties.Add("Device", "");
            this._properties.Add("Lot", "");
            this._properties.Add("Wafer", "");
            this._properties.Add("Fnloc", "");
            this._properties.Add("RowCount", 0);
            this._properties.Add("ColCount", 0);
            this._properties.Add("Bcequ", "");
            this._properties.Add("Dutms", "");

            this._properties.Add("Refpx", 0);
            this._properties.Add("Refpy", 0);

            this._properties.Add("IndexSizeX", 0.0m);
            this._properties.Add("IndexSizeY", 0.0m);
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

                while (true)
                {
                    string line = this.ReadLine();

                    if (line == null)
                        break;

                    this.Parse(line);
                }

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

        public override void DeasilRotate(int degree)
        {
            base.DeasilRotate(degree);

            this.Fnloc = (Int32.Parse(this.Fnloc) + degree).ToString();
        }

        // 按行解析文件内容
        private void Parse(string line)
        {
            try
            {
                string[] strs = line.Split(new char[] { ':' });

                switch (strs[0].ToUpper())
                {
                    case "DEVICE":
                        this.Device = strs[1].Trim();
                        break;
                    case "LOT":
                        this.LotNo = strs[1].Trim();
                        break;
                    case "WAFER":
                        this.Wafer = strs[1].Trim();
                        break;
                    case "FNLOC":
                        this.Fnloc = strs[1].Trim();
                        break;
                    case "ROWCT":
                        this.RowCount = Int32.Parse(strs[1].Trim());
                        break;
                    case "COLCT":
                        this.ColCount = Int32.Parse(strs[1].Trim());
                        break;
                    case "BCEQU":
                        this.Bcequ = strs[1].Trim();
                        break;
                    case "REFPX":
                        this.Refpx = Int32.Parse(strs[1].Trim());
                        break;
                    case "REFPY":
                        this.Refpy = Int32.Parse(strs[1].Trim());
                        break;
                    case "DUTMS":
                        this.Dutms = strs[1].Trim();
                        break;
                    case "XDIES":
                        this.IndexSizeX = Int32.Parse(strs[1].Trim());
                        break;
                    case "YDIES":
                        this.IndexSizeY = Int32.Parse(strs[1].Trim());
                        break;
                    case "ROWDATA":
                        this.ParseDies(strs[1]);
                        break;
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

            foreach (string d in dies)
            {
                DieData die = new DieData();

                try
                {
                    die.Bin = byte.Parse(d);
                }
                catch { }

                if (d == "__")
                {
                    die.Attribute = DieCategory.NoneDie;
                }
                else if (die.Bin == 0)
                {
                    die.Attribute = DieCategory.PassDie;
                }
                else if (die.Bin > 0)
                {
                    die.Attribute = DieCategory.FailDie;
                }
                else
                {
                    die.Attribute = DieCategory.Unknow;
                }

                this._diesBuffer.Add(die);
            }
        }

        /// <summary>
        /// 将数据保存为 inf 文件
        /// </summary>
        public override void Save()
        {
            try
            {

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
                                    break;
                            }

                        }
                    }


                }



                // 打开或创建文件
                this.OpenWriter();
                this.WriteString("DEVICE:" + this.Device + Enter);
                this.WriteString("LOT:" + this.LotNo.Substring(0, this.LotNo.Length - 3) + Enter);
                this.WriteString("WAFER:" + this.Wafer + Enter);
                this.WriteString("FNLOC:" + this.Fnloc + Enter);
                this.WriteString("ROWCT:" + (ymax - ymin + 1) + Enter);
                this.WriteString("COLCT:" + (xmax - xmin + 1) + Enter);
                this.WriteString("BCEQU:00" + this.Bcequ + Enter);
                this.WriteString("REFPX:7" + Enter);
                this.WriteString("REFPY:10" + Enter);
                this.WriteString("DUTMS:MM" + this.Dutms + Enter);
                this.WriteString("XDIES:" + (((double)this.IndexSizeX/100000)).ToString("0.000000") + Enter);
                this.WriteString("YDIES:" + (((double)this.IndexSizeY/100000)).ToString("0.000000") + Enter);

                /*
                 * 此功能在正式版本中启用，在测试版本中不启用
                 * 

                // 将边缘的 mark die 转换为 fail die
                for (int y = 0; y < this._dieMatrix.YMax; y++)
                {
                    int x = 0;

                    for (x = 0; x < this._dieMatrix.XMax - 1; x++)
                    {
                        if (this.DieMatrix[x, y].Attribute == DieCategory.MarkDie)
                            this.DieMatrix[x, y].Attribute = DieCategory.FailDie;

                        if (this.DieMatrix[x + 1, y].Attribute == DieCategory.FailDie ||
                            this.DieMatrix[x + 1, y].Attribute == DieCategory.PassDie ||
                            this.DieMatrix[x + 1, y].Attribute == DieCategory.TIRefFail ||
                            this.DieMatrix[x + 1, y].Attribute == DieCategory.TIRefPass)
                        {
                            break;
                        }
                    }

                    for (x = this._dieMatrix.XMax - 1; x > 0; x--)
                    {
                        if (this.DieMatrix[x, y].Attribute == DieCategory.MarkDie)
                            this.DieMatrix[x, y].Attribute = DieCategory.FailDie;

                        if (this.DieMatrix[x - 1, y].Attribute == DieCategory.FailDie ||
                            this.DieMatrix[x - 1, y].Attribute == DieCategory.PassDie ||
                            this.DieMatrix[x - 1, y].Attribute == DieCategory.TIRefFail ||
                            this.DieMatrix[x - 1, y].Attribute == DieCategory.TIRefPass)
                        {
                            break;
                        }
                    }

                }
                 * 
                 */

                // 写入 Die 数据
               // for (int y = 0; y < this._dieMatrix.YMax; y++)
                for (int y = ymin; y < ymax+1; y++)
                {
                    this.WriteString("RowData:");

                  //  for (int x = 0; x < this._dieMatrix.XMax; x++)
                    for (int x = xmin; x < xmax+1; x++)
                    {
                        this.WriteString(this.DieCategoryCaption(this.DieMatrix[x, y].Attribute, this.DieMatrix[x, y].Bin));

                       // if (x != xmax+1)
                       // if (x != this._dieMatrix.XMax - 1)
                            this.WriteString(" ");
                    }

                    this.WriteString(Enter);
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

        private string DieCategoryCaption(DieCategory attr, int bin)
        {
            string str = "?? ";

            switch (attr)
            {
                case DieCategory.PassDie:
                    str = "00";
                    break;
                case DieCategory.FailDie:
                    //if (bin > 0)
                    //    str = bin.ToString("X2");
                    //else
                        str = "01";
                    break;
                case DieCategory.SkipDie2:
                    str = "@@";
                    break;
                default:
                    str = "__";
                    break;
            }

            return str;
        }

        // 合并 inf 文件
        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            if (!(map is Sinf))
                throw new Exception("Inf 类型文件只能和 Inf 类型文件合并。");

            return null;
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
