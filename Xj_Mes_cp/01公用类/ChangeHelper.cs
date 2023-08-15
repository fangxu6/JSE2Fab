using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xj_Mes_cp
{
    public static class ChangeHelper
    {
        /// <summary>
        ///10进制 转16进制
        /// </summary>
        /// <returns></returns>
        public static string ToSixteen(string number_info)
        {
            int temp =Convert.ToInt32(  number_info);

            return temp.ToString("x").ToUpper();

        }

        /// <summary>
        ///16进制 转10进制
        /// </summary>
        /// <returns></returns>
        public static string ToTen(string number_info)
        {
           

            //28de1212
            string temp = number_info;


            return Convert.ToInt32(temp, 16).ToString().ToUpper();

        }


    }
}
