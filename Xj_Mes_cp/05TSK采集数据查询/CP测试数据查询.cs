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
    public partial class CP测试数据查询 : DockContent
    {
        public CP测试数据查询()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void CP测试数据查询_Load(object sender, EventArgs e)
        {


            DataSet dst = ex.Get_Dset("[dbo].[hp_1009_cp_tsk_info_lot_and_eq_select]");


            AutoCompleteStringCollection dt = new AutoCompleteStringCollection();
            DataTable dtb = dst.Tables[0];
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                dt.Add(dtb.Rows[i][0].ToString());

            }

            this.textBoxX1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBoxX1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.textBoxX1.AutoCompleteCustomSource = dt;
            this.textBoxX1.Enabled = true;
            this.textBoxX1.Text = "";



            AutoCompleteStringCollection dt_EQ = new AutoCompleteStringCollection();
            DataTable dtb_EQ = dst.Tables[1];
            for (int i = 0; i < dtb_EQ.Rows.Count; i++)
            {
                dt_EQ.Add(dtb_EQ.Rows[i][0].ToString());

            }

            this.textBoxX3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBoxX3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.textBoxX3.AutoCompleteCustomSource = dt_EQ;
            this.textBoxX3.Enabled = true;
            this.textBoxX3.Text = "";
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string lot = this.textBoxX1.Text;
            string eq = this.textBoxX3.Text;

            string info1 = this.textBoxX2.Text;
            string info2 = "";
            string info3 = "";


            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:00");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:00");

            string is_check = "1";
            if (this.checkBox1.Checked == true)
            {
                is_check = "1";
            }
            else {
                is_check = "0";
            }


            string sql_str = string.Format("[dbo].[hp_1009_tsk_info_select]  '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", lot, eq, is_check, dat1, dat2, info1, info2, info3);
            DataTable dt = ex.Get_Data(sql_str);



            
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

            MessageBox.Show("查询成功","系统提示");

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";

        }
    }
}
