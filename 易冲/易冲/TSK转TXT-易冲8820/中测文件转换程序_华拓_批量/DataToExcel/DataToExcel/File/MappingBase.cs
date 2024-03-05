
/*
 * ���ߣ�sky
 * �ռ䣺2008-01-09
 * ���ã�Mapping File ����
 */

namespace DataToExcel
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections;

    public abstract class MappingBase : IMappingFile
    {
        protected readonly string Enter = new string((char)13, 1) + new string((char)10, 1);

        protected string _fileType;     // mapping �ļ�����
        protected string _path;         // mapping �ļ�·��
        protected string _fileName;     // mapping �ļ���
        protected string _fullName;     // mapping �ļ���������

        protected Stream _inputStream;  // ���������� mapping �ļ�����ʱ������������
        protected BinaryReader _reader; // ���ڶ� mapping �ļ���������
        protected FileStream _writer;   // ����д mapping �ļ���������

        //protected int _rows = -1;       // ����
        //protected int _cols = -1;       // ����
        protected DieMatrix _dieMatrix; // die ���ݾ���

        protected ArrayList _keys;      // mapping ��������
        protected Hashtable _properties;// mapping ����

        private object _tag;

        protected readonly DateTime EmpDate = new DateTime(1900, 1, 1);// ������ʱ��

        public virtual string WaferID
        {
            get { return ""; }
            set { }
        }

        public virtual string LotNo
        {
            get { return ""; }
            set { }
        }

        public virtual string DeviceName
        {
            get { return ""; }
            set { }
        }

        public string Path
        {
            get { return this._path; }
            set
            {
                this._path = value;
                this._fullName = this._path + this._fileName;
            }
        }

        public string FileName
        {
            get { return this._fileName; }
            set
            {
                this._fileName = value;

                if (this._path.Length > 0)
                {
                    string lastC = this._path.Substring(this._path.Length - 1);
                    this._fullName = this._path + ((lastC == @"\") ? "" : @"\") + this._fileName;
                }
                else
                {
                    this._fullName = this._fileName;
                }
            }
        }

        public string FullName
        {
            get { return this._fullName; }
            set { this._fullName = value; }
        }

        public string FileType
        {
            get { return this._fileType; }
        }

        // ��ȡ������ die �����б�
        public DieMatrix DieMatrix
        {
            get { return this._dieMatrix; }
            set
            {
                this._dieMatrix = value;
            }
        }

        public Hashtable Properties
        {
            get { return this._properties; }
        }

        public object Tag
        {
            get { return this._tag; }
            set { this._tag = value; }
        }

        public MappingBase(string type)
        {
            this._fileType = type;
            this._fullName = "";

            this._path = "";
            this._fileName = "";

            this._reader = null;
            this._dieMatrix = null;

            this._keys = new ArrayList();
            this._properties = new Hashtable();

            this.InitialProperties();
        }

        public MappingBase(string type, string file)
        {
            this._fileType = type;
            this._fullName = file;

            if (file == "")
            {
                this._path = "";
                this._fileName = "";
            }
            else
            {
                this._path = (file.LastIndexOf('\\') >= 0) ? file.Substring(0, file.LastIndexOf('\\')) : "";
                this._fileName = this.GetFileName(file);
            }

            this._reader = null;
            this._dieMatrix = null;

            this._keys = new ArrayList();
            this._properties = new Hashtable();

            this.InitialProperties();
        }

        // �� mapping �ļ������ļ����н������ļ���
        protected virtual string GetFileName(string str)
        {
            return str.Substring(str.LastIndexOf('\\') + 1);
        }

        // ��ʼ�� mapping �����б�
        protected abstract void InitialProperties();

        // ��ȡ mapping �ļ�
        public abstract void Read();

        // ���� mapping �ļ�
        public abstract void Save();

        // �ϲ� mapping �ļ�
        public abstract IMappingFile Merge(IMappingFile map, string newfile);

        // �ж�һ�� die �Ƿ�Ϊ�� die
        public abstract bool IsEmptyDie(DieData die);

        // ��תָ���Ƕ�
        public virtual void DeasilRotate(int degree)
        {
            try
            {
                if (degree == 0)
                    return;

                if (this._dieMatrix != null)
                    this._dieMatrix.DeasilRotate(degree);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        // ���ļ�����׼����ȡ
        protected void OpenReader()
        {
            try
            {
                // ���������Ϊ�գ���������󴴽���ȡ���󣬼��������������� mapping �ļ�
                if (this._inputStream != null)
                {
                    this._reader = new BinaryReader(this._inputStream);
                }
                else
                {
                    if (this._fullName == "")
                        throw new Exception("Map file is not initialized.");

                    if (!File.Exists(this._fullName))
                        throw new Exception("File " + this._fullName + " is not Exists.");

                    this._reader = new BinaryReader(File.Open(this._fullName, FileMode.Open));
                }
            }
            catch (Exception ee)
            {
                throw new Exception("Open file stream failed:" + ee.Message);
            }
        }

        // �ر��ļ���
        protected void CloseReader()
        {
            try
            {
                this._reader.Close();
                this._reader = null;
            }
            catch (Exception ee)
            {
                throw new Exception("Close file stream failed:" + ee.Message);
            }
        }

        // ���ļ�����׼��д�ļ�
        protected void OpenWriter()
        {
            try
            {
                string file = this._fullName;
                if (file == "")
                    throw new Exception("Map file is not initialized.");

                this._writer = new FileStream(file, FileMode.OpenOrCreate);
            }
            catch (Exception ee)
            {
                throw new Exception("Open writer failed:" + ee.Message);
            }
        }

        // �ر��ļ�����ֹͣд��
        protected void CloseWriter()
        {
            try
            {
                this._writer.Close();
                this._writer = null;
            }
            catch (Exception ee)
            {
                throw new Exception("Close file stream failed:" + ee.Message);
            }
        }

        // ���ļ��ж�ȡһ��
        protected virtual string ReadLine()
        {
            long sp = this._reader.BaseStream.Position;
            long length = this._reader.BaseStream.Length;
            long ep = sp;

            if (sp >= length)
                return null;

            while (true)
            {
                char ch = this._reader.ReadChar();

                switch (ch)
                {
                    case '\n':
                    case '\r':
                        ep = this._reader.BaseStream.Position;
                        this._reader.BaseStream.Position = sp;

                        byte[] buffer = this._reader.ReadBytes((int)(ep - sp));
                        try { this._reader.ReadChar(); }
                        catch { }

                        return Encoding.ASCII.GetString(buffer).Trim();
                }

                if (this._reader.BaseStream.Position >= length)
                {
                    byte[] buffer = this._reader.ReadBytes((int)(length - sp));
                    return Encoding.ASCII.GetString(buffer).Trim();
                }
            }
        }

        // ���ļ����ж�ȡ�ַ���
        protected virtual string ReadToString(int length)
        {
            byte[] buffer = this._reader.ReadBytes(length);
            return Encoding.ASCII.GetString(buffer).Trim();
        }

        // ���ļ����ж�ȡ�ַ���
        protected virtual string ReadToString(int sp, int length)
        {
            this._reader.BaseStream.Position = sp;
            byte[] buffer = this._reader.ReadBytes(length);

            return Encoding.ASCII.GetString(buffer).Trim();
        }

        // ���ļ��ж�ȡ�ַ�
        protected virtual byte ReadToByte()
        {
            byte buffer = this._reader.ReadByte();
            return buffer;
        }

        protected virtual byte[] ReadToBytes(int length)
        {
            byte[] buffer = this._reader.ReadBytes(length);
            return buffer;
        }


        // ��ȡ����
        protected virtual DateTime ReadToDate()
        {
            int year=2000, month=1, day=1, hour=1, min=1;
            string syear = this.ReadToString(2);
            if (syear != "")
            {
                year = 2000 + Int32.Parse(syear);
            }

            string smonth = this.ReadToString(2);
            if (smonth != "")
            {
                month = Int32.Parse(smonth);
            }

            string sday = this.ReadToString(2);
            if (sday != "")
            {
                day =  Int32.Parse(sday);
            }

            string shour = this.ReadToString(2);
            if (shour != "")
            {
                hour = Int32.Parse(shour);
            }

            string smin = this.ReadToString(2);
            if (smin != "")
            {
                min = Int32.Parse(smin);
            }
            //year = 2000 + Int32.Parse(this.ReadToString(2));
            //month = Int32.Parse(this.ReadToString(2));
            //day = Int32.Parse(this.ReadToString(2));
            //hour = Int32.Parse(this.ReadToString(2));
            //min = Int32.Parse(this.ReadToString(2));

            // reserved
            this._reader.ReadBytes(2);

            return new DateTime(year, month, day, hour, min, 0);
        }

        // ���ļ��ж�ȡ����
        protected virtual short ReadToInt16()
        {

            byte[] buffer = this._reader.ReadBytes(2);

            // ��ת�ֽ�˳��
            this.Reverse(ref buffer);

            return BitConverter.ToInt16(buffer, 0);
        }

        // ���ļ��ж�ȡ����
        protected virtual short ReadToInt16(int sp)
        {
            this._reader.BaseStream.Position = sp;
            byte[] buffer = this._reader.ReadBytes(2);

            // ��ת�ֽ�˳��
            this.Reverse(ref buffer);

            return BitConverter.ToInt16(buffer, 0);
        }

        // ���ļ��ж�ȡ����
        protected virtual int ReadToInt32()
        {
            byte[] buffer = this._reader.ReadBytes(4);

            // ��ת�ֽ�˳��
            this.Reverse(ref buffer);

            return BitConverter.ToInt32(buffer, 0);
        }

        // ���ļ��ж�ȡ����
        protected virtual int ReadToInt32(int sp)
        {
            this._reader.BaseStream.Position = sp;
            byte[] buffer = this._reader.ReadBytes(4);

            // ��ת�ֽ�˳��
            this.Reverse(ref buffer);

            return BitConverter.ToInt32(buffer, 0);
        }

        // ��ת�ֽ�˳��
        protected virtual void Reverse(ref byte[] target)
        {
            int n1 = 0, n2 = target.Length - 1;
            byte temp;
            while (n1 < n2)
            {
                temp = target[n1];
                target[n1] = target[n2];
                target[n2] = temp;

                n1++;
                n2--;
            }
        }

        // ���ļ���д��һ���ַ���
        protected virtual void WriteString(string str)
        {
            if (!this._writer.CanWrite)
                throw new Exception(this._path + this._fileName + " can't be writen.");

            this._writer.Write(System.Text.Encoding.ASCII.GetBytes(str), 0, str.Length);
        }

        public virtual IMappingFile Parse(Stream stream)
        {
            return null;
        }

        // �������б������������
        public void AddProperity(string key, object val)
        {
            if (this._keys == null || this._properties == null)
                throw new Exception("�����б�δ��������");

            if (this._keys.Contains(key))
            {
                this._properties[key] = Convert.ChangeType(val, this._properties[key].GetType());
            }
            else
            {
                this._keys.Add(key);
                this._properties.Add(key, val);
            }
        }
    }
}
