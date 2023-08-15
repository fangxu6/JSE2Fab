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
    public partial class Lot过站管理 : DockContent
    {
        public Lot过站管理()
        {
            InitializeComponent();
        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_cus_name_select_info]", new List<int> { 0 });
            mfrom.ShowDialog();

            if (mfrom.select_state==false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_mate_name_select_info]", new List<int> { 0,1 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox2.Text = mfrom.select_name[0];
            this.pwtSearchBox3.Text = mfrom.select_name[1];
        }

        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_mate_name_select_info]", new List<int> { 0,1 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox2.Text = mfrom.select_name[0];
            this.pwtSearchBox3.Text = mfrom.select_name[1];
        }

        private void pwtSearchBox4_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_lot_select_info]", new List<int> { 0 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox4.Text = mfrom.select_name[0];
        }

        private void pwtSearchBox5_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_process_select_info]", new List<int> { 0 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox5.Text = mfrom.select_name[0];
        }
        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {




            string cus_name = this.pwtSearchBox1.Text;

            string mate_name = this.pwtSearchBox2.Text;
            string mate_ves = this.pwtSearchBox3.Text;
            string lot = this.pwtSearchBox4.Text;
            string process = this.pwtSearchBox5.Text;
            string only_code = this.pwtSearchBox6.Text;



            string is_check = "0";

            if (this.checkBoxX1.Checked==true)
            {
                is_check = "1";
            }

            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");


            string sql = string.Format("[dbo].[hp_1022_cp_up_line_info_select]  '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'", cus_name, mate_name, mate_ves, lot, process, only_code, is_check, dat1, dat2);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);


        }

        private void Lot过站管理_Load(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }


            if (MessageBox.Show("确定删除选择的过站信息","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            ex.Exe_Data("[dbo].[hp_1022_cp_up_line_info_delete]  '" + id + "','" + base_info.user_code + "'");
            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            string LOT_ONLY_CODE = this.pwtSearchBox1.Text.Trim();
            DataTable dt = ex.Get_Data("[dbo].[hp_1022_cp_up_line_select] '" + LOT_ONLY_CODE + "'");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("流程卡不存在,请确认！", "系统提示"); return;
            }

            DataTable dt_mate = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select]  '" + dt.Rows[0]["产品型号"].ToString() + "','" + dt.Rows[0]["版本"].ToString() + "'");



            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_lot_select_info]", new List<int> { 2 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }


            string process = mfrom.select_name[0];
            if (MessageBox.Show("确定跳过选择站别：<" + process + "> 信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string sql_insert = "[dbo].[hp_1022_cp_up_line_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'";

            ex.Get_Data(string.Format(sql_insert, LOT_ONLY_CODE, "跳过", process, base_info.user_code, "过站", "跳过", base_info.user_code));

            MessageBox.Show("跳站成功","系统提示");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1.Text = "";
            this.pwtSearchBox2.Text = "";
            this.pwtSearchBox3.Text = "";
            this.pwtSearchBox4.Text = "";
            this.pwtSearchBox5.Text = "";
            this.pwtSearchBox6.Text = "";
            this.pwtDataGridView1.Columns.Clear();
        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {

        }
    }
}
