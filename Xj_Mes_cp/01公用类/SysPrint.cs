using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seagull.BarTender.Print;
using System.Windows.Forms;
using System.IO;

namespace Xj_Mes_cp
{
    public static class SysPrint
    {

        //CP收料唯一编码.btw

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btwName"></param>
        /// <param name="Ls_Name"></param>
        /// <param name="OpenMoreCode">是否启用条码输出</param>
        /// <param name="MoreCodeList"></param>
        public static void print(string btwName, Dictionary<string, string> Ls_Name,Boolean OpenMoreCode,List<string> MoreCodeList)
        { 

            #region 打印条码模块

            // CP收料唯一编码.btw

          


            string appName = "Barcode";
            Engine engine = new Engine(true);
            string iPath = @"\2_btw\" + btwName;
            string mb = Application.StartupPath + iPath;
            LabelFormatDocument format = engine.Documents.Open(mb);


            foreach (var item in Ls_Name)
            {
                format.SubStrings[item.Key].Value = item.Value;
            }


            if (OpenMoreCode)
            {
                AddMoreCode(MoreCodeList);
            }



            format.Save();
            Messages messages;
            Result result = format.Print(appName, 5000, out messages);
            engine.Dispose();

             

            #endregion
        
        
        }


        private static void AddMoreCode(List<string> CodeList)
        {
            FileStream fs = new FileStream("D:\\barcode.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            foreach (var item in CodeList)
            {
                sw.WriteLine(item);
            }
            sw.Close();
        }

    }
}
