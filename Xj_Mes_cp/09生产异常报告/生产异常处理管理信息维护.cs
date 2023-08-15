using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public partial class 生产异常处理管理信息维护 : Form
    {


      private  string my_error_id="";
      private string my_dept_name = "";
        public 生产异常处理管理信息维护(string error_id,string dept_name)
        {
            my_error_id = error_id;
            my_dept_name = dept_name;

            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void 生产异常处理管理信息维护_Load(object sender, EventArgs e)
        {
            DataTable dt = ex.Get_Data("[dbo].[hp_1220_cp_error_why_info_dept_select]    '" + my_error_id + "','" + my_dept_name + "'");

            this.textBoxX1.Text = dt.Rows[0]["原因分析"].ToString();
            this.textBoxX2.Text = dt.Rows[0]["处理意见"].ToString();
            this.textBoxX3.Text = dt.Rows[0]["备注"].ToString();
        }
        public bool select_state = false;
        private void buttonX1_Click(object sender, EventArgs e)
        {

            string why_info=this.textBoxX1.Text;
             string deal_info=this.textBoxX2.Text;
             string remark=this.textBoxX3.Text;
            string do_user=base_info.user_code;
            string sql = string.Format("[dbo].[hp_1220_cp_error_why_info_dept_update] '{0}','{1}','{2}','{3}','{4}','{5}'", my_error_id, my_dept_name, why_info, deal_info, remark, do_user);
            ex.Exe_Data(sql);
            select_state = true;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }
    }
}
