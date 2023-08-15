using Pawote.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Xj_Mes_cp
{
    public class CP_PrintHelper
    {


        public static string PrintTableClass(PwtDataGridView PwtDGV, string lot, string process_name)
        {


            if (PwtDGV.Rows.Count == 0)
            {
                return "";
            }

            db_deal ex = new db_deal();



            //string top_table = " <table width=\"100%\" border=\"1\" cellspacing=\"0\"style= \"font-family:微软雅黑;font-size:14px\" >" +
  //  "<tr  height=\"20px\"><td  style=\"width:40px\" align=\"center\" >序号</td><td  align=\"center\">晶圆刻号</td><td  align=\"center\"  >测试 Good</td><td    align=\"center\" >测试 Yield</td><td  align=\"center\">复测 Good</td><td     align=\"center\">复测 Yield</td><td  align=\"center\">Map ID</td><td    align=\"center\">Datalog</td><td     align=\"center\">作业员</td><td  style=\"width: 150px;\" align=\"center\">备注</td></tr>";
            string top_table = " <table width=\"100%\" border=\"1\" cellspacing=\"0\"style= \"font-family:微软雅黑;font-size:14px\" >" +
  "<tr  height=\"20px\"><td  style=\"width:40px\" align=\"center\" >序号</td><td  align=\"center\">晶圆刻号</td><td  align=\"center\"  >测试 Good</td><td    align=\"center\" >测试 Yield</td><td  align=\"center\">复测 Good</td><td     align=\"center\">复测 Yield</td><td     align=\"center\">作业员</td><td  style=\"width: 150px;\" align=\"center\">备注</td></tr>";


            string demo_tr = "<tr  height=\"20px\"><td  style=\"width:40px\" align=\"center\" >{0}</td><td  align=\"center\">{1}</td><td  align=\"center\"  ></td><td    align=\"center\" ></td><td  align=\"center\"></td><td     align=\"center\"></td><td     align=\"center\"></td><td  style=\"width: 150px;\" align=\"center\"></td></tr>";

            string demo_list = "";
            for (int i = 1; i < 26; i++)
            {
                int icheck = 0;
                for (int j = 0; j < PwtDGV.Rows.Count; j++)
                {
                    if (i == int.Parse(PwtDGV.Rows[j].Cells["位号"].Value.ToString()))
                    {
                        icheck = 1;
                    }
                }


                if (icheck == 0)
                {
                    //未发现
                    demo_list += string.Format(demo_tr, i.ToString(), "");
                }
                else
                {
                    // 发现
                    demo_list += string.Format(demo_tr, i.ToString(), lot + process_name +"-"+ i.ToString().PadLeft(2, '0'));
                }
            }



            return top_table + demo_list + "</table >";
        }


    }
}
