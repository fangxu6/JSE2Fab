using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileChange
{
    public static class txt_helper
    {
        public static string ReadTxtStr(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            };

            string txt = "";
            StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default);

            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                txt += str + Environment.NewLine;
            }

            sr.Close();

            return txt;

        }

        public static void WriteTxtStr(string fileName, string info)
        {
            System.IO.File.WriteAllText(fileName, string.IsNullOrEmpty(info) ? "" : info);
        }
        //
    }
}
