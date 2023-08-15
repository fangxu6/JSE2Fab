using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public partial class Form_CP_new : Form
    {
        private string id = "";
        string mate_name = "";
        string mate_ves = "";
        string process_name = "";
        public bool select_state = false;
        public string program_send = "";
        public string pass_pian = "";
        public string pass_pici = "";
        public string pass_pian_other = "";
        public string pass_pici_other = "";

        public string zc_program1 = "";
        public string zc_program2 = "";
        public string zk_name = "";
        db_deal ex = new db_deal();
        public Form_CP_new(string iid, string imate_name, string imate_ves, string iprocess_name)
        {
            id = iid;
            mate_name = imate_name;
            mate_ves = imate_ves;
            process_name = iprocess_name;
            InitializeComponent();
        }

        private void Form_CP_new_Load(object sender, EventArgs e)
        {
            this.labelX8.Text = mate_name;
            this.labelX9.Text = mate_ves;
            this.labelX10.Text = process_name;



            DataTable dt = ex.Get_Data("[dbo].[hp_1012_W_Wafer_station_info_select] '" + id + "'");
            if (dt.Rows.Count != 0)
            {
                this.textBoxX1.Text = dt.Rows[0]["info10"].ToString();//良率指标(片)%
                this.textBoxX2.Text = dt.Rows[0]["info11"].ToString();//良率指标(批)%
                this.textBoxX3.Text = dt.Rows[0]["Name"].ToString();//测试程序
                this.textBoxX4.Text = dt.Rows[0]["info12"].ToString();//片其他备注
                this.textBoxX5.Text = dt.Rows[0]["info13"].ToString();//批次其他备注
                this.textBoxX6.Text = dt.Rows[0]["info14"].ToString();//中测台程序1
                this.textBoxX7.Text = dt.Rows[0]["info15"].ToString();//中测台程序2
                this.textBoxX8.Text = dt.Rows[0]["info16"].ToString();//针卡名称
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string pian = this.textBoxX1.Text;
            string pici = this.textBoxX2.Text;
            string program = this.textBoxX3.Text;

            string pian_other = this.textBoxX4.Text;
            string pici_other = this.textBoxX5.Text;

            string program1 = this.textBoxX6.Text;
            string program2 = this.textBoxX7.Text;
            string name = this.textBoxX8.Text;

            if (Regex.IsMatch(program, @"[\u4e00-\u9fa5]"))
            {
                MessageBox.Show("程序名称不可以包含中文,请去除中文信息", "系统提示"); return;
            }if (Regex.IsMatch(program1, @"[\u4e00-\u9fa5]"))
            {
                MessageBox.Show("程序名称不可以包含中文,请去除中文信息", "系统提示"); return;
            }if (Regex.IsMatch(program2, @"[\u4e00-\u9fa5]"))
            {
                MessageBox.Show("程序名称不可以包含中文,请去除中文信息", "系统提示"); return;
            }










            string sql = string.Format(" [dbo].[hp_1012_W_Wafer_station_info_update]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','','','',''", id, program, pian, pici, pian_other, pici_other, program1, program2,name);

            select_state = true;
            ex.Exe_Data(sql);
            program_send = program;
            pass_pian = pian;
            pass_pici = pici;
            pass_pian_other = pian_other;
            pass_pici_other = pici_other;
            zc_program1 = program1;
            zc_program2 = program2;
            zk_name = name;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
