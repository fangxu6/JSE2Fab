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
    public partial class 收货信息维护管理 : DockContent
    {
        public 收货信息维护管理()
        {
            InitializeComponent();
        }

        private void 收货信息维护管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        db_deal ex = new db_deal();
        private void buttonX2_Click(object sender, EventArgs e)
        {

            string c_type = this.comboBoxEx1.SelectedItem.ToString();
            string c_name = this.textBoxX1.Text;
            string c_tel = this.textBoxX2.Text;
            string c_user = this.textBoxX3.Text;
            string c_address = this.textBoxX6.Text;
            string remark = this.textBoxX4.Text;
            string info2 = this.textBoxX5.Text;
            string info3 = this.textBoxX7.Text;

            string info4 = this.textBoxX8.Text;
            string info5 = this.textBoxX9.Text;
            string info6 = this.textBoxX10.Text;



            string sql_check = string.Format("[dbo].[hp_cus_list_info_check_select]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'",
             c_type, c_name, c_tel, c_user, c_address, remark, info2, info3, info4, info5, info6, base_info.user_code);

            DataTable dt_check = ex.Get_Data(sql_check);


            if (dt_check.Rows.Count != 0)
            {
                MessageBox.Show("该类型对应名称已经存在", "系统提示"); return;
            }




            string sql = string.Format("[dbo].[hp_cus_list_info_insert]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'",
                c_type, c_name, c_tel, c_user, c_address, remark, info2, info3, info4, info5, info6, base_info.user_code);

            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string c_type = this.comboBoxEx1.SelectedItem.ToString();
            string c_name = this.textBoxX1.Text;
            string c_tel = this.textBoxX2.Text;
            string c_user = this.textBoxX3.Text;
            string c_address = this.textBoxX6.Text;
            string remark = this.textBoxX4.Text;
            string info2 = this.textBoxX5.Text;
            string info3 = this.textBoxX7.Text;

            string info4 = this.textBoxX8.Text;
            string info5 = this.textBoxX9.Text;
            string info6 = this.textBoxX10.Text;



            string sql_check = string.Format("[dbo].[hp_cus_list_info_check_id_select]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'",
                id, c_type, c_name, c_tel, c_user, c_address, remark, info2, info3, info4, info5, info6, base_info.user_code);
            DataTable dt_check = ex.Get_Data(sql_check);


            if (dt_check.Rows.Count != 0)
            {
                MessageBox.Show("该类型对应名称已经存在", "系统提示"); return;
            }



            if (MessageBox.Show("确定对选择的信息进行修改", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }




            string sql = string.Format("[dbo].[hp_cus_list_info_update]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'",
                id, c_type, c_name, c_tel, c_user, c_address, remark, info2, info3, info4, info5, info6, base_info.user_code);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbUpdateToDGV(dt, this.pwtDataGridView1);

            MessageBox.Show("修改成功", "系统提示");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string c_type = this.comboBoxEx1.SelectedItem.ToString();
            string c_name = this.textBoxX1.Text;
            string c_tel = this.textBoxX2.Text;
            string c_user = this.textBoxX3.Text;
            string c_address = this.textBoxX6.Text;
            string remark = this.textBoxX4.Text;
            string info2 = this.textBoxX5.Text;
            string info3 = this.textBoxX7.Text;

            string info4 = this.textBoxX8.Text;
            string info5 = this.textBoxX9.Text;
            string info6 = this.textBoxX10.Text;

            if (MessageBox.Show("确定对选择的信息进行删除", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string sql = string.Format("[dbo].[hp_cus_list_info_delete]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'",
                id, c_type, c_name, c_tel, c_user, c_address, remark, info2, info3, info4, info5, info6, base_info.user_code);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);

            MessageBox.Show("删除成功", "系统提示");
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }



            this.comboBoxEx1.SelectedItem = this.pwtDataGridView1.SelectedRows[0].Cells["类型"].Value.ToString();
            string c_name = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["名称"].Value.ToString();
            string c_tel = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["电话"].Value.ToString();
            string c_user = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["联系人"].Value.ToString();
            string c_address = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["地址"].Value.ToString();
            string remark = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();
            string info2 = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["其他信息1"].Value.ToString();
            string info3 = this.textBoxX7.Text = this.pwtDataGridView1.SelectedRows[0].Cells["其他信息2"].Value.ToString();

            //string info4 = this.textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["其他信息3"].Value.ToString();
            //string info5 = this.textBoxX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells["其他信息4"].Value.ToString();
            //string info6 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells["其他信息5"].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            string c_type = this.comboBoxEx1.SelectedItem.ToString();
            string c_name = this.textBoxX1.Text;
            string c_tel = this.textBoxX2.Text;
            string c_user = this.textBoxX3.Text;
            string c_address = this.textBoxX6.Text;
            string remark = this.textBoxX4.Text;
            string info2 = this.textBoxX5.Text;
            string info3 = this.textBoxX7.Text;

            string info4 = this.textBoxX8.Text;
            string info5 = this.textBoxX9.Text;
            string info6 = this.textBoxX10.Text;


            string sql = string.Format("[dbo].[hp_cus_list_info_select]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'",
                c_type, c_name, c_tel, c_user, c_address, remark, info2, info3, info4, info5, info6, base_info.user_code);

            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
            string c_name = this.textBoxX1.Text = "";
            string c_tel = this.textBoxX2.Text = "";
            string c_user = this.textBoxX3.Text = "";
            string c_address = this.textBoxX6.Text = "";
            string remark = this.textBoxX4.Text = "";
            string info2 = this.textBoxX5.Text = "";
            string info3 = this.textBoxX7.Text = "";

            string info4 = this.textBoxX8.Text = "";
            string info5 = this.textBoxX9.Text = "";
            string info6 = this.textBoxX10.Text = "";


            this.pwtDataGridView1.Columns.Clear();
        }
    }
}
