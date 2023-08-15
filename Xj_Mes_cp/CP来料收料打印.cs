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
    public partial class CP来料收料打印 : Form
    {
        public CP来料收料打印()
        {
            InitializeComponent();
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
           
        }

        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            Lot数量选择 lotShow = new Lot数量选择();
            lotShow.ShowDialog();
            if (lotShow.select_ok == "1")
            {
                return;
            }
            List<int> lot = lotShow.str_no;
            string total_number = lotShow.total_number;
            string total_point = lotShow.total_point;
            string total_point_remark = lotShow.total_point_remark;
            this.pwtSearchBox3.Text = total_point;
            this.textBoxX5.Text = total_number;
            this.textBoxX6.Text = total_point_remark;

        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {

        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string lot = this.textBoxX1.Text;

            string cus_name = this.pwtSearchBox1.Text;
            string type_name = this.pwtSearchBox2.Text;
            string post_simple = this.pwtSearchBox3.Text;

            string post_number = this.textBoxX5.Text;
            string post_post_info = this.textBoxX6.Text;

            string remark = this.textBoxX7.Text;

            string only_total_info = GetCPCode("", "CPT-");


        }



        private string GetCPCode(string name, string TitleNo)
        {
            db_deal ex = new db_deal();

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(2, '0');

            return sturct + sturct_info;

        }
    }
}
