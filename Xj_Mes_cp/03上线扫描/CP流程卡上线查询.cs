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
    public partial class CP流程卡上线查询 : DockContent
    {
        public CP流程卡上线查询()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {

            //this.textBoxX3.Text = "";
            //this.textBoxX4.Text = "";
            //this.textBoxX5.Text = "";
            //this.textBoxX6.Text = "";
            //this.textBoxX7.Text = "";
            //this.textBoxX8.Text = "";
            //this.textBoxX9.Text = "";


            //string txt = this.pwtSearchBox1.Text.Trim();

            //if (txt == "")
            //{
            //    return;
            //}

            //DataTable dt = ex.Get_Data("[dbo].[hp_0915_business_info_scan_info_select] '" + txt + "'");

            //if (dt.Rows.Count == 0)
            //{
            //    MessageBox.Show("流程卡不存在");
            //    return;
            //}


            //this.textBoxX3.Text = dt.Rows[0]["客户名称"].ToString();
            //this.textBoxX4.Text = dt.Rows[0]["客户代码"].ToString();
            //this.textBoxX5.Text = dt.Rows[0]["LOT"].ToString();
            //this.textBoxX6.Text = dt.Rows[0]["产品型号"].ToString();
            //this.textBoxX7.Text = dt.Rows[0]["版本"].ToString();
            //this.textBoxX8.Text = dt.Rows[0]["数量"].ToString();
            //this.textBoxX9.Text = dt.Rows[0]["位号"].ToString();
            //this.textBoxX2.Focus();
            //this.textBoxX2.SelectAll();
        }

        private void textBoxX2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    string code = this.textBoxX2.Text.Trim();

            //    DataTable dt = ex.Get_Data("[dbo].[CC_8_17_Employee_Info_select] '','" + code + "','','','','','','','','','','',''");
            //    if (dt.Rows.Count == 0)
            //    {
            //        MessageBox.Show("工号不存在");
            //        this.textBoxX2.Focus();
            //        this.textBoxX2.SelectAll();
            //        return;
            //    }
            // //   this.textBoxX2.Text = code + "_" + dt.Rows[0]["姓名"].ToString();

            //    this.textBoxX1.Focus();
            //    this.textBoxX1.SelectAll();
            //}
        }

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar==13)
            //{
            //    string lot = this.pwtSearchBox1.Text.Trim();
            //    string user = this.textBoxX2.Text.Trim();
            //    string eq = this.textBoxX1.Text.Trim();

            //    string post_info = this.textBoxX9.Text.Trim();

            //    if (this.textBoxX3.Text=="" || this.textBoxX6.Text=="")
            //    {
            //        MessageBox.Show("请先扫描流程卡"); return;
            //    }
            //    DataTable dt = ex.Get_Data("[dbo].[hp_1014_cp_online_scan_info_insert]  '" + lot + "','" + user + "','" + eq + "','" + post_info + "','"+base_info.user_code+"'");

            //    DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            //    MessageBox.Show("上线成功");



            //    this.pwtSearchBox1.Focus();
            //    this.pwtSearchBox1.Text = "";
            //    this.textBoxX1.Text = "";
            //    this.textBoxX2.Text = "";
            //}
        }

        private void CP流程卡上线扫描_Load(object sender, EventArgs e)
        {
            DataSet dst = ex.Get_Dset("[dbo].[hp_1009_cp_tsk_info_lot_and_eq_select]");


            AutoCompleteStringCollection dt_EQ = new AutoCompleteStringCollection();
            DataTable dtb_EQ = dst.Tables[1];
            for (int i = 0; i < dtb_EQ.Rows.Count; i++)
            {
                dt_EQ.Add(dtb_EQ.Rows[i][0].ToString());

            }

            this.textBoxX1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBoxX1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.textBoxX1.AutoCompleteCustomSource = dt_EQ;
            // this.textBoxX1.Enabled = true;
            this.textBoxX1.Text = "";
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string lotonlycode = this.pwtSearchBox1.Text;
            string user = this.textBoxX2.Text;
            string eq = this.textBoxX1.Text;

            string is_date = "0";
            if (this.checkBoxX1.Checked == true)
            {
                is_date = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString();
            string dat2 = this.dateTimePicker2.Value.ToString();


            string cus_name = this.textBoxX3.Text;
            string cus_code = this.textBoxX4.Text;
            string lot = this.textBoxX5.Text;
            string mate_name = this.textBoxX6.Text;
            string mate_ves = this.textBoxX7.Text;



            string sql = string.Format("[dbo].[hp_1014_cp_online_scan_info_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'"
                , lotonlycode, user, eq, cus_name, cus_code, lot, mate_name, mate_ves, is_date, dat1, dat2);

            DataTable dt = ex.Get_Data(sql);


            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1.Text = "";
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";

            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.pwtDataGridView1.Rows.Clear();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0 || this.pwtDataGridView1.SelectedRows.Count>1)
            {
                MessageBox.Show("请选择需要删除的信息");
                return;
            }

            if (MessageBox.Show("确定对选择的信息进行删除", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            ex.Exe_Data("[dbo].[hp_1014_cp_online_scan_info_delete]  '" + id + "','" + base_info.user_code + "'");

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);

        }
    }
}
