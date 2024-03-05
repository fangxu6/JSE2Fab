
/*
 * 作者：sky
 * 日间：2008-01-09
 * 作用：Mapping File 基类
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

        protected string _fileType;     // mapping 文件类型
        protected string _path;         // mapping 文件路径
        protected string _fileName;     // mapping 文件名
        protected string _fullName;     // mapping 文件完整名称

        protected Stream _inputStream;  // 由流来创建 mapping 文件对象时的输入流对象
        protected BinaryReader _reader; // 用于读 mapping 文件的流对象
        protected FileStream _writer;   // 用于写 mapping 文件的流对象

        //protected int _rows = -1;       // 行数
        //protected int _cols = -1;       // 列数
        protected DieMatrix _dieMatrix; // die 数据矩阵

        protected ArrayList _keys;      // mapping 属性名称
        protected Hashtable _properties;// mapping 属性

        private object _tag;

        protected readonly DateTime EmpDate = new DateTime(1900, 1, 1);// 空日期时间

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

        // 获取和设置 die 对象列表
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

        // 从 mapping 文件完整文件名中解析出文件名
        protected virtual string GetFileName(string str)
        {
            return str.Substring(str.LastIndexOf('\\') + 1);
        }

        // 初始化 mapping 属性列表
        protected abstract void InitialProperties();

        // 读取 mapping 文件
        public abstract void Read();

        // 保存 mapping 文件
        public abstract void Save();

        // 合并 mapping 文件
        public abstract IMappingFile Merge(IMappingFile map, string newfile);

        // 判断一个 die 是否为空 die
        public abstract bool IsEmptyDie(DieData die);

        // 旋转指定角度
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

        // 打开文件流，准备读取
        protected void OpenReader()
        {
            try
            {
                // 如果流对象不为空，则从流对象创建读取对象，即从流对象来创建 mapping 文件
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

        // 关闭文件流
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

        // 打开文件流，准备写文件
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

        // 关闭文件流，停止写入
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

        // 从文件中读取一行
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

        // 从文件流中读取字符串
        protected virtual string ReadToString(int length)
        {
            byte[] buffer = this._reader.ReadBytes(length);
            return Encoding.ASCII.GetString(buffer).Trim();
        }

        // 从文件流中读取字符串
        protected virtual string ReadToString(int sp, int length)
        {
            this._reader.BaseStream.Position = sp;
            byte[] buffer = this._reader.ReadBytes(length);

            return Encoding.ASCII.GetString(buffer).Trim();
        }

        // 从文件中读取字符
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


        // 读取日期
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

        // 从文件中读取整数
        protected virtual short ReadToInt16()
        {

            byte[] buffer = this._reader.ReadBytes(2);

            // 反转字节顺序
            this.Reverse(ref buffer);

            return BitConverter.ToInt16(buffer, 0);
        }

        // 从文件中读取整数
        protected virtual short ReadToInt16(int sp)
        {
            this._reader.BaseStream.Position = sp;
            byte[] buffer = this._reader.ReadBytes(2);

            // 反转字节顺序
            this.Reverse(ref buffer);

            return BitConverter.ToInt16(buffer, 0);
        }

        // 从文件中读取整数
        protected virtual int ReadToInt32()
        {
            byte[] buffer = this._reader.ReadBytes(4);

            // 反转字节顺序
            this.Reverse(ref buffer);

            return BitConverter.ToInt32(buffer, 0);
        }

        // 从文件中读取整数
        protected virtual int ReadToInt32(int sp)
        {
            this._reader.BaseStream.Position = sp;
            byte[] buffer = this._reader.ReadBytes(4);

            // 反转字节顺序
            this.Reverse(ref buffer);

            return BitConverter.ToInt32(buffer, 0);
        }

        // 反转字节顺序
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

        // 向文件中写入一个字符串
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

        // 向属性列表中添加新属性
        public void AddProperity(string key, object val)
        {
            if (this._keys == null || this._properties == null)
                throw new Exception("属性列表未被创建。");

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
