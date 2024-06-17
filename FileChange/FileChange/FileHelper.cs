using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileChange
{
    public class FileHelper
    {
        /// <summary>
        /// 获取目录中文件的绝对路径
        /// </summary>
        /// <param name="a_dirName"></param>
        /// <param name="a_fileName"></param>
        /// <param name="a_Extension"></param>
        /// <returns></returns>
        public static string FindFilePath(string a_dirName, string a_fileName, string a_Extension)
        {
            string a_strFileAbsolutelyPath = "";
            DirectoryInfo l_dirInfo = new DirectoryInfo(a_dirName);
            FileInfo[] l_files = l_dirInfo.GetFiles();//返回目录中所有文件和子目录
            //含组件号的.csproj文件
            foreach (FileInfo info in l_files)
            {
                if (info.Name.Contains(a_fileName) && info.Extension == a_Extension)
                {
                    a_strFileAbsolutelyPath = info.FullName;
                    return a_strFileAbsolutelyPath;
                }
            }

            //找其它目录
            DirectoryInfo[] l_childsDir = l_dirInfo.GetDirectories();
            if (l_childsDir.Length >= 0)
            {
                foreach (DirectoryInfo dirInfo in l_childsDir)
                {
                    a_strFileAbsolutelyPath = FindFilePath(dirInfo.FullName, a_fileName, a_Extension);
                    if (a_strFileAbsolutelyPath != "")
                    {
                        return a_strFileAbsolutelyPath;
                    }
                }
            }
            return a_strFileAbsolutelyPath;
        }
        /// <summary>
        /// 获取文本文件的字符编码类型
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static Encoding GetEncoding(string fileName)
        {
            Encoding encoding = Encoding.Default;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream, encoding);
            byte[] buffer = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            if (buffer.Length >= 3 && buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
            {
                encoding = Encoding.UTF8;
            }
            else if (buffer.Length >= 3 && buffer[0] == 254 && buffer[1] == 255 && buffer[2] == 0)
            {
                encoding = Encoding.BigEndianUnicode;
            }
            else if (buffer.Length >= 3 && buffer[0] == 255 && buffer[1] == 254 && buffer[2] == 65)
            {
                encoding = Encoding.Unicode;
            }
            else if (IsUTF8Bytes(buffer))
            {
                encoding = Encoding.UTF8;
            }
            return encoding;
        }
        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// BOM（Byte Order Mark），字节顺序标记，出现在文本文件头部，Unicode编码标准中用于标识文件是采用哪种格式的编码。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
            byte curByte; //当前分析的字节. 
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前 
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1 
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }


        /// <summary>
        /// 删除指定目录及其所有文件
        /// </summary>
        /// <param name="a_strPath"></param>
        public static void DelDirSub(string a_strPath)
        {
            try
            {
                //去除文件夹和子文件的只读\隐藏属性
                //去除文件夹的只读\隐藏属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(a_strPath);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读\隐藏属性
                System.IO.File.SetAttributes(a_strPath, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(a_strPath))
                {
                    foreach (string f in Directory.GetFileSystemEntries(a_strPath))
                    {
                        if (File.Exists(f))
                        {
                            File.Delete(f);
                        }
                        else
                        {
                            DelDirSub(f);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(a_strPath);
                }
            }
            catch (Exception)
            { }
        }
    }
}
