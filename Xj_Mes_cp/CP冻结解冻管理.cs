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
    public partial class CP冻结解冻管理 : DockContent
    {
        public CP冻结解冻管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        #region 流程卡查询
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {

            string p_lot = this.pwtSearchBox1.Text.Trim();
            string sql = string.Format("[dbo].[hp_1022_cp_up_line_select] '{0}'", p_lot);

            DataTable dt = ex.Get_Data(sql);


            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("流程卡不存在", "系统提示"); return;
            }


            DataTable dt_mate = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select]  '" + dt.Rows[0]["产品型号"].ToString() + "','" + dt.Rows[0]["版本"].ToString() + "'");


            if (dt_mate.Rows.Count == 0)
            {
                MessageBox.Show("晶圆信息未配置", "系统提示");
                return;
            }

            this.textBoxX1.Text = dt.Rows[0]["客户名称"].ToString();

            this.textBoxX4.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX5.Text = dt.Rows[0]["版本"].ToString();

            this.textBoxX2.Text = dt.Rows[0]["LOT"].ToString();
            this.textBoxX3.Text = dt.Rows[0]["数量"].ToString();


            this.textBoxX7.Text = dt.Rows[0]["位号"].ToString();


            this.comboBoxEx1.Items.Clear();
            for (int i = 0; i < dt_mate.Rows.Count; i++)
            {
                this.comboBoxEx1.Items.Add(dt_mate.Rows[i][2].ToString());
            }


            DataTable dt_now = ex.Get_Data("[dbo].[hp_1022_cp_up_line_info_get_process_select] '" + p_lot + "'");

            if (dt_now.Rows.Count == 0)
            {
                this.textBoxX6.Text = "待上线";
            }
            else
            {
                this.textBoxX6.Text = dt_now.Rows[0][0].ToString();

            }
        } 
        #endregion

        #region 冻结位号选择
        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            Lot数量选择 mfrom = new Lot数量选择();
            mfrom.ShowDialog();


            if (mfrom.select_ok == "1")
            {
                return;
            }

            this.pwtSearchBox2.Text = mfrom.total_point;
            this.textBoxX8.Text = mfrom.total_number;
            this.textBoxX9.Text = mfrom.total_point_remark;


        } 
        #endregion

        #region 冻结
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string p_lot = this.pwtSearchBox1.Text;

            string now_process = this.textBoxX6.Text;
            string hold_process = this.comboBoxEx1.SelectedItem.ToString();
            string post_name = this.pwtSearchBox2.Text;

            string post_number = this.textBoxX8.Text;

            string post_simple = this.textBoxX9.Text;


            if (MessageBox.Show("确定进行冻结？\r\n冻结以后根据系统提示是否进行拆新流程单", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string sql = string.Format("[dbo].[hp_cp_hold_process_info_insert] '{0}','{1}','{2}','{3}','{4}','冻结','{5}'", p_lot, now_process, hold_process, post_name, post_number, base_info.user_code);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);



            ////if (MessageBox.Show("是否拆份子流程卡","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            ////{
            ////    return;
            ////}



            ////string lot = this.textBoxX2.Text;
            ////string mate_ves = this.textBoxX5.Text;

            ////string tital_name = lot + "." + mate_ves + ".";
            ////string CP_ONLY_CODE = GetCPCode(tital_name, tital_name);



            ////DataTable dt_info = ex.Get_Data("[dbo].[hp_0915_business_info_chai_only_code_insert] '"+p_lot+"','"
            ////    +CP_ONLY_CODE+"','"+post_name+"','"+post_simple+"','"+post_number+"','"+base_info.user_code+"'");

            ////foreach (var item in post_name.Split('、'))
            ////{
            ////    //更新每一个post信息
            ////}


        } 
        #endregion

        private string GetCPCode(string name, string TitleNo)
        {

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(2, '0');

            return sturct + sturct_info;

        }

        #region 查询 
        private void buttonX1_Click(object sender, EventArgs e)
        {

            string p_lot = this.pwtSearchBox1.Text;


            string cus_name = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string mate_name = this.textBoxX4.Text;


            string is_check = "0";
            if (this.checkBoxX1.Checked == true)
            {
                is_check = "1";
            }

            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");

            DataTable dt = ex.Get_Data("[dbo].[hp_cp_hold_process_info_select] '" + p_lot + "','" + cus_name + "','" + lot + "','" + mate_name + "','" + is_check + "','" + dat1 + "','" + dat2 + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }
        #endregion

        #region 解冻
        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("确定对选择的信息进行解冻", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            ex.Exe_Data("[dbo].[hp_cp_hold_process_info_update]  '" + id + "','" + base_info.user_code + "'");

            this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value = "正常";
            this.pwtDataGridView1.SelectedRows[0].Cells["解冻人"].Value = base_info.user_code;
            this.pwtDataGridView1.SelectedRows[0].Cells["解冻时间"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MessageBox.Show("解冻成功", "系统提示");
        }
        #endregion

        #region 删除
        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("确定对选择的信息进行删除", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            ex.Exe_Data("[dbo].[hp_cp_hold_process_info_detete]  '" + id + "','" + base_info.user_code + "'");
            this.pwtDataGridView1.Rows.Remove(this.pwtDataGridView1.SelectedRows[0]);
            MessageBox.Show("删除成功", "系统提示");

        }
        #endregion

        #region 清空
        private void buttonX5_Click(object sender, EventArgs e)
        {
            this.textBoxX1.Text = "";

            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";

            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";

            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.pwtSearchBox1.Text = "";
            this.pwtSearchBox2.Text = "";

            this.pwtDataGridView1.Columns.Clear();

            this.comboBoxEx1.Items.Clear();
        } 
        #endregion

        private void CP冻结解冻管理_Load(object sender, EventArgs e)
        {

        }
    }
}
