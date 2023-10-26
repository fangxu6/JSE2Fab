
/*
 * 作者：sky
 * 时间：2008-01-10
 * 作用：通用函数定义
 */

namespace DataToExcel
{
    using System;
    using System.IO;
    using DataToExcel;

    public class UtilFunction
    {
        /// <summary>
        /// 对照 Die category 名称
        /// </summary>
        public static string DieCategoryCaption(DieCategory cate)
        {
            string cap = "";

            switch (cate)
            {
                case DieCategory.FailDie:
                    cap = "F";
                    break;
                case DieCategory.MarkDie:
                    cap = "#";
                    break;
                //case DieCategory.NoneDie:
                //    cap = "N";
                //    break;
                case DieCategory.PassDie:
                    cap = "1";
                    break;
                case DieCategory.NoneDie:
                case DieCategory.SkipDie:
                    cap = ".";
                    break;
                case DieCategory.SkipDie2:
                    cap = "#";
                    break;
                default:
                    cap = "?";
                    break;
            }

            return cap;
        }

        /// <summary>
        /// CMD Txt 格式中对照 Die category 名称
        /// </summary>
        public static string CMDTxtBinText(DieCategory cate)
        {
            string cap = "";

            switch (cate)
            {
                case DieCategory.SkipDie:
                case DieCategory.FailDie:
                    cap = "3";
                    break;
                case DieCategory.NoneDie:
                case DieCategory.MarkDie:
                    cap = "5";
                    break;
                case DieCategory.PassDie:
                    cap = "1";
                    break;
                case DieCategory.SkipDie2:
                    cap = "#";
                    break;
                default:
                    cap = "?";
                    break;
            }

            return cap;
        }

        /// <summary>
        /// 将信息写入文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="msg">要写入的内容</param>
        public static void WriteToFile(string file, string msg)
        {
            string f=file;

            if (f == "")
                f = System.Windows.Forms.Application.StartupPath + "\\debug.txt";

            StreamWriter w = new StreamWriter(f, true);
            w.WriteLine(msg);
            w.Close();
        }
    }
}
