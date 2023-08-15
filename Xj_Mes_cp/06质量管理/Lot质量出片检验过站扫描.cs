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
    public partial class Lot质量出片检验过站扫描 : DockContent
    {
        public Lot质量出片检验过站扫描()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string LOT_ONLY_CODE = this.textBoxX1.Text.Trim();
            DataTable dt = ex.Get_Data("[dbo].[hp_1022_cp_up_line_select] '" + LOT_ONLY_CODE + "'");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("流程卡不存在,请确认！", "系统提示"); return;
            }

            DataTable dt_mate = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select]  '" + dt.Rows[0]["产品型号"].ToString() + "','" + dt.Rows[0]["版本"].ToString() + "'");


            if (dt_mate.Rows.Count == 0)
            {
                MessageBox.Show("晶圆信息未配置", "系统提示");
                return;
            }

            this.textBoxX3.Text = dt.Rows[0]["客户代码"].ToString();
            this.textBoxX4.Text = dt.Rows[0]["客户名称"].ToString();
            this.textBoxX5.Text = dt.Rows[0]["LOT"].ToString();
            this.textBoxX6.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX7.Text = dt.Rows[0]["版本"].ToString();
            

            this.textBoxX8.Text = dt.Rows[0]["数量"].ToString();
            this.labelX9.Text = dt.Rows[0]["数量"].ToString();


            this.textBoxX9.Text = dt.Rows[0]["位号"].ToString();
            this.textBoxX12.Text = dt.Rows[0]["位号"].ToString();

        }

     

        private void Lot过站扫描_Load(object sender, EventArgs e)
        {

            DataTable dt = ex.Get_Data("[dbo].[CC_sys_system_basic_info_select] 'IQC检验结果'");
            DtbToUi.DtbToComboBoxEx(dt, this.comboBoxEx1);

            dt = ex.Get_Data("[dbo].[CC_sys_system_basic_info_select] 'IQC处理结果'");
            DtbToUi.DtbToComboBoxEx(dt, this.comboBoxEx2);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

            string only_code = this.textBoxX1.Text.Trim();
            string qc_res = this.comboBoxEx1.SelectedItem.ToString();
            string deal_res = this.comboBoxEx2.SelectedItem.ToString();
            string do_user = base_info.user_code;


            string post_info = this.textBoxX12.Text;
            string post_number = this.labelX9.Text;



            if (this.textBoxX3.Text == "" || this.textBoxX4.Text == "" || this.textBoxX5.Text == "" || this.textBoxX6.Text == "")
            {
                MessageBox.Show("请确认流程信息", "系统提示"); return;
            }
            if (this.comboBoxEx1.SelectedIndex==-1)
            {
                MessageBox.Show("请选择检验类型", "系统提示"); return;
            }
            if (this.comboBoxEx2.SelectedIndex == -1)
            {
                MessageBox.Show("请选择处理类型", "系统提示"); return;
            }

            //DataTable dt = ex.Get_Data("[dbo].[hp_1116_qc_info_insert]  '出片检验','" + only_code + "','" + qc_res + "','" + deal_res + "','" + do_user + "'");

            //DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);


            DataTable dt = ex.Get_Data("[dbo].[hp_1116_qc_info_insert01]  '出片检验','" + only_code + "','" + qc_res + "','" + deal_res + "','" + do_user + "','" + post_info + "','" + post_number + "'");


            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);

            MessageBox.Show("添加成功","系统提示");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            string only_lot = this.textBoxX1.Text;

            string post_list_new = this.textBoxX9.Text;

            Lot数量选择二次选择过站 mfrom = new Lot数量选择二次选择过站(only_lot, post_list_new);

            mfrom.ShowDialog();

            if (mfrom.select_ok == "1")
            {
                return;
            }


            this.textBoxX12.Text = mfrom.total_point;
            this.labelX9.Text = mfrom.total_number;
        }
    }
}
