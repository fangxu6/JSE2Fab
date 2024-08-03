
/*
 * ���ߣ�sky
 * ʱ�䣺2008-01-09
 * ���ã�������������
 */

namespace DataToExcel
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Drawing;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    public class ConstDefine
    {
        public const string FileType_TMA = "tma";
        public const string FileType_TIWW = "tiww";
        public const string FileType_TSK = "tsk";
        public const string FileType_SINF = "sinf";
        public const string FileType_CMDTXT = "cmdtxt";
        public const string FileType_DAT = "dat";
    }

    /*
     * ö�پ�������
     */
    public enum DieCategory : short
    {
        Unknow = 1,
        PassDie = 2,
        FailDie = 4,
        SkipDie = 8,
        SkipDie2 = 9,
        NoneDie = 16,
        MarkDie = 32,

        TIRefPass = 64,
        TIRefFail = 128
    }

    /*
     * ���Խ��ö��
     */
    public enum TestResult
    {
        Pass = 0,
        Fail
    }

    /*
     * Die ����
     */
    public class DieData
    {
        // Fields
        private DieCategory _attribute = DieCategory.Unknow;
        private int _bin = -1;
        private int _x = 0;
        private int _y = 0;

        // Methods
        public DieData Clone()
        {
            DieData data = new DieData();
            data._attribute = this._attribute;
            data._bin = this._bin;
            data._x = this._x;
            data._y = this._y;
            return data;
        }

        public override bool Equals(object o)
        {
            if (!((o != null) && (o is DieData)))
            {
                return false;
            }
            DieData data = (DieData)o;
            if ((((this._attribute != data._attribute) || (this._bin != data._bin)) || (this._x != data._x)) || (this._y != data._y))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this._attribute.GetHashCode();
        }

        public static DieData operator +(DieData item1, DieData item2)
        {
            DieData data = new DieData();
            if ((item1.Attribute == DieCategory.PassDie) && (item2.Attribute == DieCategory.PassDie))
            {
                data.Attribute = DieCategory.PassDie;
                return data;
            }
            if ((item1.Attribute == DieCategory.MarkDie) || (item2.Attribute == DieCategory.MarkDie))
            {
                data.Attribute = DieCategory.MarkDie;
                return data;
            }
            if ((item1.Attribute == DieCategory.NoneDie) || (item2.Attribute == DieCategory.NoneDie))
            {
                data.Attribute = DieCategory.NoneDie;
                return data;
            }
            if ((item1.Attribute == DieCategory.FailDie) || (item2.Attribute == DieCategory.FailDie))
            {
                data.Attribute = DieCategory.FailDie;
                return data;
            }
            if ((item1.Attribute == DieCategory.Unknow) || (item2.Attribute == DieCategory.Unknow))
            {
                data.Attribute = DieCategory.Unknow;
                return data;
            }
            if ((item1.Attribute == DieCategory.SkipDie) || (item2.Attribute == DieCategory.SkipDie))
            {
                data.Attribute = DieCategory.SkipDie;
                return data;
            }
            data.Attribute = DieCategory.Unknow;
            return data;
        }

        public static bool operator ==(DieData item1, DieData item2)
        {
            object obj2 = item1;
            object obj3 = item2;
            if (obj2 == null)
            {
                return (obj3 == null);
            }
            return item1.Equals(item2);
        }

        public static bool operator !=(DieData item1, DieData item2)
        {
            object obj2 = item1;
            object obj3 = item2;
            if (obj2 == null)
            {
                return (obj3 != null);
            }
            return !item1.Equals(item2);
        }

        // Properties
        public DieCategory Attribute
        {
            get
            {
                return this._attribute;
            }
            set
            {
                this._attribute = value;
            }
        }

        public int Bin
        {
            get
            {
                return this._bin;
            }
            set
            {
                this._bin = value;
            }
        }

        public int X
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        public int Y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }
    }



    /*
     * Die ����ʽ�洢
     */
    public class DieMatrix
    {
        private int _xmax; // X ���������ֵ������
        private int _ymax; // Y ���������ֵ������

        private ArrayList _items;

        public int XMax
        {
            get { return this._xmax; }
        }

        public int YMax
        {
            get { return this._ymax; }
        }

        public int Count
        {
            get { return this._items.Count; }
        }

        public ICollection Items
        {
            get { return this._items; }
        }

        public DieData this[int index]
        {
            get
            {
                return (DieData)this._items[index];
            }
        }

        public DieData this[int x, int y]
        {
            get
            {
                if (x >= this._xmax)
                    throw new Exception("�������������Χ��");

                if (y >= this._ymax)
                    throw new Exception("�������������Χ��");

                return (DieData)this._items[y * this._xmax + x];
            }
            set
            {
                if (x >= this._xmax)
                    throw new Exception("�������������Χ��");

                if (y >= this._ymax)
                    throw new Exception("�������������Χ��");

                this._items[y * this._xmax + x] = value;
            }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        private DieMatrix()
        {
            this._xmax = -1;
            this._ymax = -1;

            this._items = new ArrayList();
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public DieMatrix(int xmax, int ymax)
        {
            this._xmax = xmax;
            this._ymax = ymax;

            this._items = new ArrayList();
            int count = xmax * ymax;

            for (int i = 0; i < count; i++)
            {
                DieData d = new DieData();
                d.Attribute = DieCategory.NoneDie;

                this._items.Add(d);
            }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public DieMatrix(IList dies, int xmax, int ymax)
        {
            this._xmax = xmax;
            this._ymax = ymax;

            this._items = new ArrayList();
            foreach (DieData d in dies)
            {
                this._items.Add(d);
            }
        }

        public void SetValue(int x, int y, DieData die)
        {
            this[x, y] = die;
        }

        /// <summary>
        /// ˳ʱ����ת����
        /// </summary>
        /// <param name="degree">��ת�ĽǶ�</param>
        public void DeasilRotate(int degree)
        {
            switch (degree)
            {
                case 0:
                    break;
                case 90:
                    this.R90();
                    break;
                case 270:
                case -90:
                    this.R270();
                    break;
                case 180:
                    this.R180();
                    break;
                default:
                    throw new Exception("����֧�� " + degree + " �ȵ���ת��");
            }
        }

        // ˳ʱ����ת 90 ��
        private void R90()
        {
            int x = -1, y = -1, xr = -1, yr = -1, count = this._items.Count;
            DieData[] dies = new DieData[count];

            for (int i = 0; i < count; i++)
            {
                // ���� x,y ����
                x = i % this._xmax;
                y = i / this._xmax;

                xr = (this._ymax - 1) - y;
                yr = x;

                dies[yr * this._ymax + xr] = (DieData)this._items[i];
            }

            // ��������������
            x = this._xmax;
            this._xmax = this._ymax;
            this._ymax = x;

            this._items = ArrayList.Adapter(dies);
        }

        // ˳ʱ����ת 270 �ȣ�����ʱ����ת 90 ��
        private void R270()
        {
            int x = -1, y = -1, xr = -1, yr = -1, count = this._items.Count;
            DieData[] dies = new DieData[count];

            for (int i = 0; i < count; i++)
            {
                // ���� x,y ����
                x = i % this._xmax;
                y = i / this._xmax;

                xr = y;
                yr = (this._xmax - 1) - x;

                dies[yr * this._ymax + xr] = (DieData)this._items[i];
            }

            // ��������������
            x = this._xmax;
            this._xmax = this._ymax;
            this._ymax = x;

            this._items = ArrayList.Adapter(dies);
        }

        // ��ת 180 ��
        private void R180()
        {
            int x = -1, y = -1, xr = -1, yr = -1, count = this._items.Count;
            DieData[] dies = new DieData[count];

            for (int i = 0; i < this._items.Count; i++)
            {
                try
                {
                    // ���� x,y ����
                    x = i % this._xmax;
                    y = i / this._xmax;

                    xr = (this._xmax) - 1 - x;
                    yr = (this._ymax) - 1 - y;

                    dies[yr * this._xmax + xr] = (DieData)this._items[i];
                }
                catch (Exception ee)
                {
                    string msg = ee.Message;
                }
            }

            this._items = ArrayList.Adapter(dies);
        }

        /// <summary>
        /// ����ƽ�Ʋ���
        /// </summary>
        public void Offset(OffsetDir dir, int qty)
        {
            if (dir == OffsetDir.X)
                this.OffsetX(qty);
            else if (dir == OffsetDir.Y)
                this.OffsetY(qty);
        }

        /// <summary>
        /// X �����ϵľ���ƫ�ƣ��ճ���λ���Կ� Die �������
        /// </summary>
        private void OffsetX(int qty)
        {
            if (qty == 0)
                return;

            if (Math.Abs(qty) >= this._xmax)
                throw new Exception("X ����λ�Ƶĳ��ȱ���С�ھ��󳤶ȡ�");

            if (qty > 0)
            {
                // ����ֵ
                for (int i = this._xmax - 1; i >= qty; i--)
                {
                    for (int j = 0; j < this._ymax; j++)
                    {
                        this[i, j].Attribute = this[i - qty, j].Attribute;
                    }
                }

                // �մ����� die ����
                for (int i = 0; i < qty; i++)
                {
                    for (int j = 0; j < this._ymax; j++)
                    {
                        this[i, j].Attribute = DieCategory.NoneDie;
                    }
                }
            }
            else if (qty < 0)
            {
                // ����ֵ
                for (int i = 0; i < this._xmax - qty; i++)
                {
                    for (int j = 0; j < this._ymax; j++)
                    {
                        this[i, j].Attribute = this[i + qty, j].Attribute;
                    }
                }

                // �մ����� die ����
                for (int i = qty; i < this._xmax; i++)
                {
                    for (int j = 0; j < this._ymax; j++)
                    {
                        this[i, j].Attribute = DieCategory.NoneDie;
                    }
                }
            }
        }

        /// <summary>
        /// Y �����ϵľ���ƫ�ƣ��ճ���λ���Կ� Die �������
        /// </summary>
        private void OffsetY(int qty)
        {
            if (qty == 0)
                return;

            if (Math.Abs(qty) >= this._ymax)
                throw new Exception("y ����λ�Ƶĳ��ȱ���С�ھ����ȡ�");

            if (qty > 0)
            {
                // ����ֵ
                for (int i = this._ymax - 1; i >= qty; i--)
                {
                    for (int j = 0; j < this._xmax; j++)
                    {
                        this[j, i].Attribute = this[j, i - qty].Attribute;
                    }
                }

                // �մ����� die ����
                for (int i = 0; i < qty; i++)
                {
                    for (int j = 0; j < this._xmax; j++)
                    {
                        this[j, i].Attribute = DieCategory.NoneDie;
                    }
                }
            }
            else if (qty < 0)
            {
                // ����ֵ
                for (int i = 0; i < this._ymax - qty; i++)
                {
                    for (int j = 0; j < this._xmax; j++)
                    {
                        this[j, i].Attribute = this[j, i + qty].Attribute;
                    }
                }

                // �մ����� die ����
                for (int i = qty; i < this._ymax; i++)
                {
                    for (int j = 0; j < this._xmax; j++)
                    {
                        this[j, i].Attribute = DieCategory.NoneDie;
                    }
                }
            }
        }

        /// <summary>
        /// ��չ��������
        /// </summary>
        /// <param name="dir">��չ�����ϡ��¡�����</param>
        /// <param name="qty">��չ����</param>
        public void Expand(ExpandDir dir, int qty)
        {
            if (qty <= 0)
                throw new Exception("������չ������������0��");

            if (Math.Abs(qty) >= this._xmax || Math.Abs(qty) >= this._ymax)
                throw new Exception("������չ���������С�ھ�����������");

            // ����ԭʼ����
            int x = this._xmax, xi = 0;
            int y = this._ymax, yi = 0;

            // ����չ������������������ڸ������ݵ�λ�Ʋ�
            switch (dir)
            {
                case ExpandDir.Left:
                    x += qty;
                    break;
                case ExpandDir.Right:
                    xi = qty;
                    x += qty;
                    break;
                case ExpandDir.Up:
                    yi = qty;
                    y += qty;
                    break;
                case ExpandDir.Down:
                    y += qty;
                    break;
            }

            int count = x * y;
            ArrayList arr = new ArrayList();

            // ����� die ����
            for (int i = 0; i < count; i++)
            {
                DieData d = new DieData();
                d.Attribute = DieCategory.NoneDie;
                arr.Add(d);
            }

            // ��ԭ�������ݸ��Ƶ��¾�����
            for (int i = 0; i < this._ymax; i++)
            {
                for (int j = 0; j < this._xmax; j++)
                {
                    arr[(i + yi) * x + (j + xi)] = this[j, i].Clone();
                }
            }

            this._items = arr;
            this._xmax = x;
            this._ymax = y;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="dir">���������ϡ��¡�����</param>
        /// <param name="qty">��������</param>
        public void Collapse(ExpandDir dir, int qty)
        {
            if (qty <= 0)
                throw new Exception("������չ������������0��");

            if (Math.Abs(qty) >= this._xmax || Math.Abs(qty) >= this._ymax)
                throw new Exception("������չ���������С�ھ�����������");

            // ����ԭʼ����
            int x = this._xmax, xi = 0;
            int y = this._ymax, yi = 0;

            // ����չ������������������ڸ������ݵ�λ�Ʋ�
            switch (dir)
            {
                case ExpandDir.Left:
                    xi = qty;
                    x -= qty;
                    break;
                case ExpandDir.Right:
                    x -= qty;
                    break;
                case ExpandDir.Up:
                    yi = qty;
                    y -= qty;
                    break;
                case ExpandDir.Down:
                    y -= qty;
                    break;
            }

            int count = x * y;
            ArrayList arr = new ArrayList();

            // ����� die ����
            for (int i = 0; i < count; i++)
            {
                DieData d = new DieData();
                d.Attribute = DieCategory.NoneDie;
                arr.Add(d);
            }

            // ��ԭ�������ݸ��Ƶ��¾�����
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    arr[i * x + j] = this[j + xi, i + yi].Clone();
                }
            }

            this._items = arr;
            this._xmax = x;
            this._ymax = y;
        }

        /// <summary>
        /// �ж�����ʵ���Ƿ����
        /// </summary>
        public override bool Equals(object o)
        {
            if ((o == null) || !(o is DieMatrix))
                return false;

            DieMatrix dies = (DieMatrix)o;

            int count = dies._items.Count;

            if (count != this._items.Count)
                return false;

            for (int i = 0; i < count; i++)
            {
                if (dies._items[i] != this._items[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(DieMatrix item1, DieMatrix item2)
        {
            object o1 = item1;
            object o2 = item2;

            if (o1 == null)
                return (o2 == null);

            return item1.Equals(item2);
        }

        public static bool operator !=(DieMatrix item1, DieMatrix item2)
        {
            object o1 = item1;
            object o2 = item2;

            if (o1 == null)
                return (o2 != null);

            return !item1.Equals(item2);
        }

        // ��������أ�����Ϊ����������ص��������
        public static DieMatrix operator +(DieMatrix items1, DieMatrix items2)
        {
            int count = items1.Count;

            if (count != items2.Count)
                throw new Exception("������Ԫ�ظ�������ͬ���޷�ִ�мӷ����㡣");

            if ((items1._xmax != items2._xmax) || (items1._ymax != items2._ymax))
                throw new Exception("�������������ƥ�䣬�޷�ִ�мӷ����㡣");

            DieData[] dies = new DieData[count];

            for (int i = 0; i < count; i++)
            {
                dies[i] = (DieData)items1._items[i] + (DieData)items2._items[i];
            }

            return new DieMatrix(dies, items1._xmax, items1._ymax);
        }

        /// <summary>
        /// ��¡����
        /// </summary>
        public DieMatrix Clone()
        {
            DieMatrix items = new DieMatrix();

            items._xmax = this._xmax;
            items._ymax = this._ymax;

            foreach (DieData die in this._items)
            {
                items._items.Add(die.Clone());
            }

            return items;
        }

        public override string ToString()
        {
            string text = "";

            for (int i = 0; i < this._ymax; i++)
            {
                for (int j = 0; j < this._xmax; j++)
                {
                    text += ((int)this[j, i].Attribute).ToString() + "  ";
                }

                text += "\n";
            }

            return text;
        }

        // ���� die ����
        public void Paint(Graphics g, float xsize, float ysize, bool isprint)
        {
            Hashtable colors = new Hashtable();

            colors.Add(DieCategory.PassDie, new SolidBrush(Color.FromArgb(172, 221, 0)));
            colors.Add(DieCategory.FailDie, new SolidBrush(Color.FromArgb(214, 46, 47)));
            colors.Add(DieCategory.SkipDie, new SolidBrush(Color.FromArgb(98, 91, 161)));
            colors.Add(DieCategory.MarkDie, new SolidBrush(Color.FromArgb(255, 222, 0)));
            colors.Add(DieCategory.NoneDie, new SolidBrush(Color.FromArgb(218, 218, 218)));
            colors.Add(DieCategory.Unknow, new SolidBrush(Color.Black));

            colors.Add(DieCategory.TIRefPass, new SolidBrush(Color.FromArgb(0, 166, 174)));
            colors.Add(DieCategory.TIRefFail, new SolidBrush(Color.FromArgb(92, 12, 123)));

            this.Paint(g, xsize, ysize, colors, isprint);
        }



        // ���� die ����
        public void Paint(Excel.Worksheet g, bool isprint)
        {
            Hashtable colors = new Hashtable();

            colors.Add(DieCategory.PassDie, new SolidBrush(Color.FromArgb(172, 221, 0)));
            colors.Add(DieCategory.FailDie, new SolidBrush(Color.FromArgb(214, 46, 47)));
            colors.Add(DieCategory.SkipDie, new SolidBrush(Color.FromArgb(98, 91, 161)));
            colors.Add(DieCategory.MarkDie, new SolidBrush(Color.FromArgb(255, 222, 0)));
            colors.Add(DieCategory.NoneDie, new SolidBrush(Color.FromArgb(218, 218, 218)));
            colors.Add(DieCategory.Unknow, new SolidBrush(Color.Black));

            colors.Add(DieCategory.TIRefPass, new SolidBrush(Color.FromArgb(0, 166, 174)));
            colors.Add(DieCategory.TIRefFail, new SolidBrush(Color.FromArgb(92, 12, 123)));

            this.Paint(g, colors, isprint);
        }

        // ���� die ����
        public void Paint(Excel.Worksheet sheet, Hashtable colors, bool isprint)
        {
            SolidBrush brush = new SolidBrush(Color.FromArgb(0x59, 0x57, 0x57));
            int xMax = this.XMax;
            int yMax = this.YMax;
            if ((xMax > 0) && (yMax > 0))
            {
                int num4;
                ToCountDie die = new ToCountDie();
                ToCountDie._ToCountDie = new Hashtable();
                object[,] objArray = new object[yMax, xMax];
                int num3 = 0;
                while (num3 < xMax)
                {
                    num4 = 0;
                    while (num4 < yMax)
                    {
                        DieCategory attribute = this[num3, num4].Attribute;
                        if (attribute <= DieCategory.SkipDie)
                        {
                            switch (attribute)
                            {
                                case DieCategory.Unknow:
                                    objArray[num4, num3] = "?";
                                    goto Label_0156;

                                case DieCategory.PassDie:
                                    objArray[num4, num3] = 0;
                                    goto Label_0156;

                                case (DieCategory.Unknow | DieCategory.PassDie):
                                    goto Label_0144;

                                case DieCategory.FailDie:
                                    objArray[num4, num3] = this[num3, num4].Bin;
                                    die.CountDie(this[num3, num4].Bin);
                                    goto Label_0156;

                                case DieCategory.SkipDie:
                                    goto Label_010E;
                            }
                            goto Label_0144;
                        }
                        if (attribute != DieCategory.NoneDie)
                        {
                            if (attribute == DieCategory.MarkDie)
                            {
                                goto Label_0120;
                            }
                            goto Label_0144;
                        }
                        objArray[num4, num3] = "N";
                        goto Label_0156;
                    Label_010E:
                        objArray[num4, num3] = "";
                        goto Label_0156;
                    Label_0120:
                        objArray[num4, num3] = "M";
                        goto Label_0156;
                    Label_0144:
                        objArray[num4, num3] = "?";
                    Label_0156:
                        num4++;
                    }
                    num3++;
                }
                if (xMax <= 0x100)
                {
                    sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[yMax, xMax]).Value2 = objArray;
                    for (num4 = 0; num4 < yMax; num4++)
                    {
                        num3 = 0;
                        while (num3 < xMax)
                        {
                            if (objArray[num4, num3].ToString() == "S")
                            {
                                sheet.get_Range(sheet.Cells[num4 + 1, num3 + 1], sheet.Cells[num4 + 1, num3 + 1]).Interior.ColorIndex = 7;
                            }
                            num3++;
                        }
                    }
                }
                else
                {
                    int num5 = (xMax / 0x100) + 1;
                    int num6 = 5;
                    for (num3 = 0; num3 < num5; num3++)
                    {
                        object[,] objArray2;
                        if (num3 != (num5 - 1))
                        {
                            objArray2 = new object[yMax, 0x100];
                            for (int i = 0; i < yMax; i++)
                            {
                                for (int k = 0; k < 0x100; k++)
                                {
                                    objArray2[i, k] = objArray[i, k + (num3 * 0x100)];
                                }
                            }
                            sheet.get_Range(sheet.Cells[1 + ((num3 * yMax) + (num3 * num6)), 1], sheet.Cells[yMax + ((num3 * yMax) + (num3 * num6)), 0x100]).Value2 = objArray2;
                            for (int j = 0; j < yMax; j++)
                            {
                                for (int m = 0; m < 0x100; m++)
                                {
                                    if (objArray2[j, m].ToString() == "S")
                                    {
                                        sheet.get_Range(sheet.Cells[(j + 1) + ((num3 * yMax) + (num3 * num6)), m + 1], sheet.Cells[(j + 1) + ((num3 * yMax) + (num3 * num6)), m + 1]).Interior.ColorIndex = 7;
                                    }
                                }
                            }
                        }
                        else
                        {
                            objArray2 = new object[yMax, xMax - (num3 * 0x100)];
                            for (int n = 0; n < yMax; n++)
                            {
                                for (int num12 = 0; num12 < (xMax - (num3 * 0x100)); num12++)
                                {
                                    objArray2[n, num12] = objArray[n, num12 + (num3 * 0x100)];
                                }
                            }
                            sheet.get_Range(sheet.Cells[1 + ((num3 * yMax) + (num3 * num6)), 1], sheet.Cells[yMax + ((num3 * yMax) + (num3 * num6)), xMax - (num3 * 0x100)]).Value2 = objArray2;
                            for (int num13 = 0; num13 < yMax; num13++)
                            {
                                for (int num14 = 0; num14 < (xMax - (num3 * 0x100)); num14++)
                                {
                                    if (objArray2[num13, num14].ToString() == "S")
                                    {
                                        sheet.get_Range(sheet.Cells[(num13 + 1) + ((num3 * yMax) + (num3 * num6)), num14 + 1], sheet.Cells[(num13 + 1) + ((num3 * yMax) + (num3 * num6)), num14 + 1]).Interior.ColorIndex = 7;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        // ���� die ����
        public void Paint(Graphics g, float width, float height, Hashtable colors, bool isprint)
        {
            SolidBrush lineBrush = new SolidBrush(Color.FromArgb(89, 87, 87));

            int rows = this.YMax;
            int cols = this.XMax;

            if (rows <= 0 || cols <= 0)
                return;

            float xspace, yspace;

            if (isprint)
            {
                xspace = width / 10;
                yspace = height / 10;
            }
            else
            {
                xspace = width / 10.515f;
                yspace = height / 10.32f;
            }

            float margin = 0;

            // ����
            g.FillRectangle(lineBrush, margin, margin, cols * xspace + 5, rows * yspace + 10);

            RectangleF rect = new RectangleF(0, 0, xspace - 1, yspace - 1);

            // ��ɫ
            for (int i = 0; i < this.YMax; i++)
            {
                rect.Y += yspace;
                rect.X = 0;

                for (int j = 0; j < this.XMax; j++)
                {
                    rect.X += xspace;
                    g.FillRectangle((SolidBrush)colors[this[j, i].Attribute], rect);
                }
            }
        }

        // ���� die ����
        public void Paint(Graphics g, RectangleF bounds, bool isprint)
        {
            Hashtable colors = new Hashtable();

            colors.Add(DieCategory.PassDie, new SolidBrush(Color.FromArgb(172, 221, 0)));
            colors.Add(DieCategory.FailDie, new SolidBrush(Color.FromArgb(214, 46, 47)));
            colors.Add(DieCategory.SkipDie, new SolidBrush(Color.FromArgb(98, 91, 161)));
            colors.Add(DieCategory.MarkDie, new SolidBrush(Color.FromArgb(255, 222, 0)));
            colors.Add(DieCategory.NoneDie, new SolidBrush(Color.FromArgb(218, 218, 218)));
            colors.Add(DieCategory.Unknow, new SolidBrush(Color.Black));

            colors.Add(DieCategory.TIRefPass, new SolidBrush(Color.FromArgb(0, 166, 174)));
            colors.Add(DieCategory.TIRefFail, new SolidBrush(Color.FromArgb(92, 12, 123)));

            this.Paint(g, bounds, colors, isprint);
        }

        // ���� die ����
        public void Paint(Graphics g, RectangleF bounds, Hashtable colors, bool isprint)
        {
            SolidBrush lineBrush = new SolidBrush(Color.FromArgb(89, 87, 87));

            int rows = this.YMax;
            int cols = this.XMax;

            if (rows <= 0 || cols <= 0)
                return;

            float xspace, yspace;

            xspace = bounds.Width / cols;
            yspace = bounds.Height / rows;

            float margin = 0;

            // ����
            g.FillRectangle(lineBrush, margin, margin, cols * xspace + 5, rows * yspace + 10);

            RectangleF rect = new RectangleF(0, 0, xspace - 1, yspace - 1);

            // ��ɫ
            for (int i = 0; i < this.YMax; i++)
            {
                rect.Y += yspace;
                rect.X = 0;

                for (int j = 0; j < this.XMax; j++)
                {
                    rect.X += xspace;
                    g.FillRectangle((SolidBrush)colors[this[j, i].Attribute], rect);
                }
            }
        }

        // ͳ�ƾ����з���ָ�����Ե� die �ĸ���
        public int DieAttributeStat(DieCategory attr)
        {
            int count = 0;

            foreach (DieData die in this._items)
            {
                if (((int)die.Attribute & (int)attr) > 0)
                    count += 1;
            }

            return count;
        }

        public enum OffsetDir
        {
            X = 0,  // X ����λ��
            Y       // Y ����λ��
        }

        public enum ExpandDir
        {
            Left = 0, // ������չ
            Right,  // ������չ
            Up,     // ������չ
            Down    // ������չ
        }
    }

    /*
     * ��ʽת������
     */
    public class ConvertConfig
    {
        // Fields
        private ConvertFieldList _fields;
        private string _from;
        private int _notchAppoint;
        private int _rotate;
        private string _to;
        private string _trimDir;

        // Methods
        public ConvertConfig(string from, string to)
        {
            this._from = from;
            this._to = to;
            this._rotate = 0;
            this._notchAppoint = -1;
            this._trimDir = "";
            this._fields = new ConvertFieldList();
            this.GetConfig(from, to);
        }

        private void GetConfig(string from, string to)
        {
            try
            {
                string path = Application.StartupPath + @"\FieldMapping_TI.xml";
                if (!File.Exists(path))
                {
                    throw new Exception("δ�ҵ���ʽת���ֶ�ӳ�������ļ� FieldMapping.xml��");
                }
                XmlDocument document = new XmlDocument();
                document.Load(path);
                XmlNode documentElement = document.DocumentElement;
                XmlNode node2 = null;
                foreach (XmlNode node3 in documentElement.ChildNodes)
                {
                    if ((node3.Attributes["from"].InnerText.ToLower() == from.ToLower()) && (node3.Attributes["to"].InnerText.ToLower() == to.ToLower()))
                    {
                        node2 = node3;
                        break;
                    }
                }
                if (node2 == null)
                {
                    throw new Exception("�����ļ���δ�ҵ� " + from + " ��ʽ�� " + to + " ��ʽ��ת���ֶ�ӳ��������Ϣ��");
                }
                try
                {
                    this._rotate = int.Parse(node2.Attributes["rotate"].InnerText);
                }
                catch
                {
                    this._rotate = 0;
                }
                try
                {
                    this._notchAppoint = int.Parse(node2.Attributes["notchappoint"].InnerText);
                }
                catch
                {
                    this._notchAppoint = -1;
                }
                try
                {
                    this._trimDir = node2.Attributes["trimdir"].InnerText;
                }
                catch
                {
                    this._trimDir = "";
                }
                foreach (XmlNode node3 in node2.ChildNodes)
                {
                    this._fields.Add(new ConvertField(node3.Attributes["from"].InnerText, node3.Attributes["to"].InnerText));
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        // Properties
        public ConvertFieldList Fields
        {
            get
            {
                return this._fields;
            }
        }

        public string From
        {
            get
            {
                return this._from;
            }
        }

        public int NotchAppoint
        {
            get
            {
                return this._notchAppoint;
            }
        }

        public int Rotate
        {
            get
            {
                return this._rotate;
            }
        }

        public string To
        {
            get
            {
                return this._to;
            }
        }

        public string TrimDir
        {
            get
            {
                return this._trimDir;
            }
        }

        // Nested Types
        public class ConvertField
        {
            // Fields
            private string _from;
            private string _to;

            // Methods
            public ConvertField(string f, string t)
            {
                this._from = f;
                this._to = t;
            }

            // Properties
            public string From
            {
                get
                {
                    return this._from;
                }
            }

            public string To
            {
                get
                {
                    return this._to;
                }
            }
        }

        public class ConvertFieldList : ArrayList
        {
            // Properties
            public new ConvertConfig.ConvertField this[int index]
            {
                get
                {
                    return (ConvertConfig.ConvertField)base[index];
                }
                set
                {
                    base[index] = value;
                }
            }
        }
    }


}
