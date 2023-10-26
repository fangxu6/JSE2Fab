
/*
 * ���ߣ�sky
 * ʱ�䣺2008-01-10
 * ���ã�ͨ�ú�������
 */

namespace DataToExcel
{
    using System;
    using System.IO;
    using DataToExcel;

    public class UtilFunction
    {
        /// <summary>
        /// ���� Die category ����
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
        /// CMD Txt ��ʽ�ж��� Die category ����
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
        /// ����Ϣд���ļ�
        /// </summary>
        /// <param name="file">�ļ���</param>
        /// <param name="msg">Ҫд�������</param>
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
