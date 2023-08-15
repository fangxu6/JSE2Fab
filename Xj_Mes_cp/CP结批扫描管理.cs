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
    public partial class CP结批扫描管理 : DockContent
    {
        public CP结批扫描管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void buttonX5_Click(object sender, EventArgs e)
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

           
            this.textBoxX2.Text = dt.Rows[0]["客户名称"].ToString();
            this.textBoxX3.Text = dt.Rows[0]["LOT"].ToString();
            this.textBoxX4.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX5.Text = dt.Rows[0]["版本"].ToString();

            this.textBoxX6.Text = dt.Rows[0]["数量"].ToString();

            this.textBoxX8.Text = dt.Rows[0]["位号"].ToString();
          

         
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string only_code = this.textBoxX1.Text;

            string cus_name = this.textBoxX2.Text;
            string lot = this.textBoxX3.Text;
            string mate_name = this.textBoxX4.Text;
            string mate_ves = this.textBoxX5.Text;

            string p_number = this.textBoxX6.Text;
            string p_info = this.textBoxX8.Text;

            string is_date = "0";
            if (this.checkBoxX1.Checked==true)
            {
                is_date = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");


            string sql = string.Format("[dbo].[hp_1227_cp_close_info_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'", only_code, cus_name,
               lot, mate_name, mate_ves, p_number, p_info, base_info.user_code, is_date,dat1,dat2);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("查询成功");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string only_code = this.textBoxX1.Text;

            string cus_name = this.textBoxX2.Text;
            string lot = this.textBoxX3.Text;
            string mate_name = this.textBoxX4.Text;
            string mate_ves = this.textBoxX5.Text;

            string p_number = this.textBoxX6.Text;
            string p_info = this.textBoxX8.Text;


            string sql = string.Format("[dbo].[hp_1227_cp_close_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'", only_code, cus_name,
               lot, mate_name, mate_ves, p_number, p_info, base_info.user_code);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("结批完成");

        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string only_code = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["唯一码"].Value.ToString();

            string cus_name = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string lot = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["lot"].Value.ToString();
            string mate_name = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆型号"].Value.ToString();
            string mate_ves = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["版本"].Value.ToString();

            string p_number = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["数量"].Value.ToString();
            string p_info = this.textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["位号"].Value.ToString();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }


            if (MessageBox.Show("确定对选择的结批数据进行删除","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            ex.Exe_Data("[dbo].[hp_1227_cp_close_info_delete] '" + id + "','" + base_info.user_code + "'");

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
            MessageBox.Show("删除完成");

        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string only_code = this.textBoxX1.Text = "";

            string cus_name = this.textBoxX2.Text = "";
            string lot = this.textBoxX3.Text = "";
            string mate_name = this.textBoxX4.Text = "";
            string mate_ves = this.textBoxX5.Text = "";

            string p_number = this.textBoxX6.Text = "";
            string p_info = this.textBoxX8.Text = "";
        }

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                this.buttonX5_Click(null, null);
            }
        }
    }
}
