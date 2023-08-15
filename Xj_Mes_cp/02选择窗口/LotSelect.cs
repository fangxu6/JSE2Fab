using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xj_Mes_cp
{
    public class LotSelect
    {


        /// <summary>
        /// 数据拼接处理
        /// </summary>
        /// <returns></returns>
        public static List<string> PinSimpleString(List<int> SelectNumberList)
        {
            if (SelectNumberList.Count == 0)
            {
                return null; 
            }

            if (SelectNumberList.Count == 1)
            {
                return new List<string> {SelectNumberList[0].ToString().PadLeft(2,'0') };
            }

            int[] ListNumber = new int[SelectNumberList.Count];
            for (int i = 0; i < SelectNumberList.Count; i++)
            {
                ListNumber[i] = SelectNumberList[i];
            }

            //int[] ListNumber = { 1, 2, 3, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15, 18, 20, 21, 23, 24, 25 };
            List<string> res_info = new List<string>();
            int FirstNo = ListNumber[0];
            int LastNo = 0;
            int StartCheck = ListNumber[0];

            for (int i = 1; i < ListNumber.Length; i++)
            {
                if (ListNumber[i] - StartCheck != 1)
                {

                    if (FirstNo.ToString() == ListNumber[i - 1].ToString())
                    {
                        res_info.Add(""+FirstNo.ToString().PadLeft(2,'0'));
                    }
                    else {
                        res_info.Add("" + FirstNo.ToString().PadLeft(2, '0') + "~" + "" + ListNumber[i - 1].ToString().PadLeft(2, '0'));
                    }
                   
                    StartCheck = ListNumber[i];
                    FirstNo = ListNumber[i];
                }
                else
                {
                    StartCheck = ListNumber[i];
                    LastNo = ListNumber[i];
                }
                if (i == ListNumber.Length - 1)
                {

                    if (FirstNo.ToString() == ListNumber[i].ToString())
                    {
                        res_info.Add("" + FirstNo.ToString().PadLeft(2, '0'));// LastNo.ToString());
                    }
                    else {
                        res_info.Add("" + FirstNo.ToString().PadLeft(2, '0') + "~" + "" + ListNumber[i].ToString().PadLeft(2, '0'));// LastNo.ToString());
                    }
                   
                }
            }


            return res_info;
        }
    }
}
