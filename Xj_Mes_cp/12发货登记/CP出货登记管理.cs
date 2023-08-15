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
    public partial class CP出货登记管理 : DockContent
    {
        public CP出货登记管理()
        {
            InitializeComponent();
        }

        private void CP出货登记管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string cus_name = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string mate_name = this.textBoxX3.Text;

            string is_data = "0";

            if (this.checkBoxX1.Checked==true)
            {
                is_data = "1";
            }

            string send_state = this.comboBoxEx1.SelectedItem.ToString();

            if (send_state=="全部")
            {
                send_state = "";
            }

            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");


            DataTable dt = ex.Get_Data("[dbo].[cp_hp_20220307_send_out]  '" + cus_name + "','" + lot + "','" + mate_name + "','" + is_data + "','" + dat1 + "','" + dat2 + "','" + send_state + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string post_info = this.pwtDataGridView1.SelectedRows[0].Cells["位号"].Value.ToString();

            string lot = this.pwtDataGridView1.SelectedRows[0].Cells["批次号"].Value.ToString();
            string p_lot = this.pwtDataGridView1.SelectedRows[0].Cells["流程卡号"].Value.ToString();

            //string post_info = this.pwtDataGridView1.SelectedRows[0].Cells["批次号"].Value.ToString();

            CP出货登记管理_显示信息 mfrom = new CP出货登记管理_显示信息(id, lot, p_lot, post_info);
            mfrom.ShowDialog();


            if (mfrom.select_state==false)
            {
                return;
            }
            //更新发货数据
            pwtDataGridView1_MouseDoubleClick(null, null);


            //更新数据
            DataTable dt_new = ex.Get_Data("[dbo].[cp_hp_20220307_send_out_BY_ID_SELECT] '"+id+"'");
            DtbToUi.DtbUpdateToDGV(dt_new, this.pwtDataGridView1);


        }

        private void groupPanel1_Click(object sender, EventArgs e)
        {

        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();



            DataTable dt = ex.Get_Data("[dbo].[cp_20220307_send_info_info_select]  '" + id + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);
        }

        private void 删除发货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView2.SelectedRows.Count==0)
            {
                return;
            }

            if (MessageBox.Show("确定删除发货信息?","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string id = this.pwtDataGridView2.SelectedRows[0].Cells["序号"].Value.ToString();


            ex.Exe_Data("[dbo].[cp_20220307_send_info_delete]  '" + id + "','" + base_info.user_code + "'");

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView2);


            //更新数据
            string Fid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            
            DataTable dt_new = ex.Get_Data("[dbo].[cp_hp_20220307_send_out_BY_ID_SELECT] '" + Fid + "'");
            DtbToUi.DtbUpdateToDGV(dt_new, this.pwtDataGridView1);



        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
         
        }
    }
}
