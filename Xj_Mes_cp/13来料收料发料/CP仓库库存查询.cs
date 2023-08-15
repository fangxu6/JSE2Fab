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
    public partial class CP仓库库存查询 : DockContent
    {
        public CP仓库库存查询()
        {
            InitializeComponent();
        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT] 'CP' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
            this.pwtSearchBox4.Text = mfrom.select_name[1];
        }

        private void pwtSearchBox4_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT] 'CP' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
            this.pwtSearchBox4.Text = mfrom.select_name[1];
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            //待调整SQL
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_W_Wafer_Materials_information_Info_SELECT] ", new List<int> { 0, 1, 2 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[1];
            this.pwtSearchBox4.Text = mfrom.select_name[0];
            this.pwtSearchBox2.Text = mfrom.select_name[2];
        }

        db_deal ex = new db_deal();

        private void buttonX1_Click(object sender, EventArgs e)
        {

            string cus_name = this.pwtSearchBox1.Text;
            string mate_name = this.pwtSearchBox2.Text;

            string lot = this.textBoxX1.Text;


            DataTable dt = ex.Get_Data(" [dbo].[mail_hp_0915_cp_res_info_select_wms_lot_PC_select]   '" + cus_name + "','" + mate_name + "','" + lot + "'");


            this.pwtDataGridView1.DataSource = dt;
            this.pwtDataGridView1.Columns["序号"].Visible = false;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1.Text = "";
            this.pwtSearchBox2.Text = "";
            this.pwtSearchBox4.Text = "";
            this.textBoxX1.Text = "";

        }
    }
}
