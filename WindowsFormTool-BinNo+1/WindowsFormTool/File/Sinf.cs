
/*
 * ���ߣ�sky
 * ʱ�䣺2008-04-17
 * ���ã��������� Mapping �е� sinf �ļ���ʽ
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

        public string Lot
        {
            get { return this._properties["Lot"].ToString(); }
            set { this._properties["Lot"] = value; }
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

        public decimal XDies
        {
            get { return (decimal)this._properties["XDies"]; }
            set { this._properties["XDies"] = value; }
        }

        public decimal YDies
        {
            get { return (decimal)this._properties["YDies"]; }
            set { this._properties["YDies"] = value; }
        }

        public Sinf(string file)
            : base(ConstDefine.FileType_SINF, file)
        {
        }

        protected override void InitialProperties()
        {
            this._keys.Add("Device");
            this._keys.Add("Lot");
            this._keys.Add("Wafer");
            this._keys.Add("Fnloc");
            this._keys.Add("Bcequ");
            this._keys.Add("RowCount");
            this._keys.Add("ColCount");
            this._keys.Add("Dutms");

            this._keys.Add("Refpx");
            this._keys.Add("Refpy");

            this._keys.Add("XDies");
            this._keys.Add("YDies");

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

            this._properties.Add("XDies", 0.0m);
            this._properties.Add("YDies", 0.0m);
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
                // �رն�ȡ��
                this.CloseReader();
            }
        }

        public override void DeasilRotate(int degree)
        {
            base.DeasilRotate(degree);

            this.Fnloc = (Int32.Parse(this.Fnloc) + degree).ToString();
        }

        // ���н����ļ�����
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
                        this.Lot = strs[1].Trim();
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
                        this.XDies = Decimal.Parse(strs[1].Trim());
                        break;
                    case "YDIES":
                        this.YDies = Decimal.Parse(strs[1].Trim());
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

        // ����ÿ�� die ����
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
        /// �����ݱ���Ϊ inf �ļ�
        /// </summary>
        public override void Save()
        {
            try
            {
                // �򿪻򴴽��ļ�
                this.OpenWriter();

                this.WriteString("DEVICE:" + this.Device + Enter);
                this.WriteString("LOT:" + this.Lot + Enter);
                this.WriteString("WAFER:" + this.Wafer + Enter);
                this.WriteString("FNLOC:" + this.Fnloc + Enter);
                this.WriteString("ROWCT:" + this._dieMatrix.YMax + Enter);
                this.WriteString("COLCT:" + this._dieMatrix.XMax + Enter);
                this.WriteString("BCEQU:" + this.Bcequ + Enter);
                this.WriteString("REFPX:" + this.Refpx + Enter);
                this.WriteString("REFPY:" + this.Refpy + Enter);
                this.WriteString("DUTMS:" + this.Dutms + Enter);
                this.WriteString("XDIES:" + this.XDies + Enter);
                this.WriteString("YDIES:" + this.YDies + Enter);

                /*
                 * �˹�������ʽ�汾�����ã��ڲ��԰汾�в�����
                 * 

                // ����Ե�� mark die ת��Ϊ fail die
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

                // д�� Die ����
                for (int y = 0; y < this._dieMatrix.YMax; y++)
                {
                    this.WriteString("RowData:");

                    for (int x = 0; x < this._dieMatrix.XMax; x++)
                    {
                        this.WriteString(this.DieCategoryCaption(this.DieMatrix[x, y].Attribute, this.DieMatrix[x, y].Bin));

                        if (x != this._dieMatrix.XMax - 1)
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
                    if (bin > 0)
                        str = bin.ToString("X2");
                    else
                        str = "01";
                    break;
                default:
                    str = "__";
                    break;
            }

            return str;
        }

        // �ϲ� inf �ļ�
        public override IMappingFile Merge(IMappingFile map, string newfile)
        {
            if (!(map is Sinf))
                throw new Exception("Inf �����ļ�ֻ�ܺ� Inf �����ļ��ϲ���");

            return null;
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