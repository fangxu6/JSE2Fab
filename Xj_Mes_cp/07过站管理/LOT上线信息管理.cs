using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class LOT上线信息管理 : DockContent
    {
        public LOT上线信息管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {

            string cus_name = this.pwtSearchBox1.Text;
            string lot = this.pwtSearchBox2.Text;
            string mate_name = this.pwtSearchBox3.Text;
            string mate_ves = this.pwtSearchBox4.Text;


            string check_date = "0";
            if (this.pwtCheckBox1.Checked == true)
            {
                check_date = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string sql_str = string.Format("[dbo].[hp_1022_cp_up_line_info_deal_select_upline] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", cus_name, lot, mate_name, mate_ves, check_date, dat1, dat2);
            DataTable dt = ex.Get_Data(sql_str);
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);


        }
    }
}
