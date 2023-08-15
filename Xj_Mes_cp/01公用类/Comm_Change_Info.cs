using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pawote.UI.Controls;

namespace Xj_Mes_cp
{
    public class Comm_Change_Info
    {
       
        public static void ChangeInfoName(PwtDataGridView pwtDataGridView1)
        {
            db_deal ex = new db_deal();

            DataTable dt = ex.Get_Data("[dbo].[new_hp_basic_table_info_select] 'FT基础表'");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                 pwtDataGridView1.Columns[dt.Rows[i][0].ToString()].HeaderText = dt.Rows[i][1].ToString();


                if (dt.Rows[i][2].ToString() == "1")
                {
                    pwtDataGridView1.Columns[dt.Rows[i][0].ToString()].Visible = true;
                }
                else
                {
                    pwtDataGridView1.Columns[dt.Rows[i][0].ToString()].Visible = false;
                }
            }

        }
    }
}
