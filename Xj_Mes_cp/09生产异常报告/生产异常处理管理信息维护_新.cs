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
    public partial class 生产异常处理管理信息维护_新 : Form
    {
        private string my_error_id = "";
        private string my_dept_name = "";
        db_deal ex = new db_deal();
        public bool select_state = false;
        public 生产异常处理管理信息维护_新(string error_id, string dept_name)
        {
            my_error_id = error_id;
            my_dept_name = dept_name;
            InitializeComponent();
        }

        private void 生产异常处理管理信息维护_新_Load(object sender, EventArgs e)
        {
            string aa = labelX4.Text;

            string cc = aa.Substring(0, 5);//取前10个字符

            string dd = aa.Substring(6); 

            labelX4.Text = cc.Trim() + "\n" + dd.Trim();

            //string aaa = labelX5.Text;

            //string ccc = aaa.Substring(0, 5);//取前10个字符

            //string ddd = aaa.Substring(6);

            //labelX5.Text = ccc.Trim() + "\n" + ddd.Trim();

            DataTable dt = ex.Get_Data("[dbo].[hp_1220_cp_error_why_info_dept_select]    '" + my_error_id + "','" + my_dept_name + "'");

            this.textBoxX1.Text = dt.Rows[0]["原因分析"].ToString();
            this.textBoxX2.Text = dt.Rows[0]["处理意见"].ToString();
            this.textBoxX3.Text = dt.Rows[0]["备注"].ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string why_info = this.textBoxX1.Text;
            string deal_info = this.textBoxX2.Text;
            string remark = this.textBoxX3.Text;//工程师姓名
            string do_user = base_info.user_code;
            string yj = this.textBoxX5.Text;//客户工程师的处理意见
            string email = this.textBoxX4.Text;//客户放行的邮件标题
            //string sql = string.Format("[dbo].[hp_1220_cp_error_why_info_dept_update] '{0}','{1}','{2}','{3}','{4}','{5}'", my_error_id, my_dept_name, why_info, deal_info, remark, do_user); 
            string sql = string.Format("[dbo].[hp_1220_cp_error_why_info_dept_update01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", my_error_id, my_dept_name, why_info, deal_info, remark, do_user, yj,email);
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
