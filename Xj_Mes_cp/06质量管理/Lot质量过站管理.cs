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
    public partial class Lot质量过站管理 : DockContent
    {
        public Lot质量过站管理()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {

            if (this.comboBoxEx1.SelectedIndex==-1)
            {
                MessageBox.Show("请选择查询类型","系统提示");
                return;
            }
            string qc_type = this.comboBoxEx1.SelectedItem.ToString();



            string lot = this.textBoxX1.Text.Trim();
            string cus = this.textBoxX2.Text.Trim();
            string mate_name = this.textBoxX3.Text.Trim();
            string mate_ves = this.textBoxX4.Text.Trim();
            string only_code = this.textBoxX5.Text.Trim();


            string is_time = "0";
            if (this.checkBoxX1.Checked)
            {
                is_time = "1";
            }

            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");


            string sql = string.Format("[dbo].[hp_1116_qc_info_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}'", qc_type, lot, cus, mate_name, mate_ves, only_code, is_time, dat1, dat2);

            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();



            if (MessageBox.Show("确定删除IQC检验数据","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            ex.Exe_Data("[dbo].[hp_1116_qc_info_delete] '" + id + "','" + base_info.user_code + "'");
            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
        }

        private void Lot质量过站管理_Load(object sender, EventArgs e)
        {

            DataTable dt = ex.Get_Data("[dbo].[CC_sys_system_basic_info_select] '质量过站检验类型'");
            DtbToUi.DtbToComboBoxEx(dt, this.comboBoxEx1);
        }
    }
}
