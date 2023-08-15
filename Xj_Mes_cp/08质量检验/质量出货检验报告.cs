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
    public partial class 质量出货检验报告 : DockContent
    {
        public 质量出货检验报告()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void 质量出货检验报告_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
            this.comboBoxEx2.SelectedIndex = 0;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

            string lot = this.textBoxX1.Text;
            string cus = this.textBoxX2.Text;
            string mate = this.textBoxX3.Text;
            string get_number = this.textBoxX4.Text;
            string post_name = this.textBoxX5.Text;
            string report_date = this.textBoxX6.Text;

            string check_type = this.comboBoxEx1.SelectedItem.ToString();
            string check_name = this.textBoxX7.Text;

            string qx = this.textBoxX8.Text;
            string res = this.comboBoxEx2.SelectedItem.ToString();

            string remark = this.textBoxX9.Text;


            string info1 = this.textBoxX10.Text;
            string info2 = this.textBoxX11.Text;
            string info3 = this.textBoxX12.Text;




            string sql = string.Format("[dbo].[hp_cp_1206_qms_out_report_insert]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'"
                ,lot,cus,mate,get_number,post_name,report_date,check_type,check_name,qx,res,remark,base_info.user_code,  info1,info2,info3);

            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("登记成功", "系统提示");
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string lot = this.textBoxX1.Text;
            string cus = this.textBoxX2.Text;
            string mate = this.textBoxX3.Text;
            string get_number = this.textBoxX4.Text;
            string post_name = this.textBoxX5.Text;
            string report_date = this.textBoxX6.Text;

            string check_type = this.comboBoxEx1.SelectedItem.ToString();
            string check_name = this.textBoxX7.Text;

            string qx = this.textBoxX8.Text;
            string res = this.comboBoxEx2.SelectedItem.ToString();

            string remark = this.textBoxX9.Text;


            string info1 = this.textBoxX10.Text;
            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX10.Text;


            string is_check = "0";
            if (this.checkBoxX1.Checked==true)
            {
                is_check = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");


            string sql = string.Format("[dbo].[hp_cp_1206_qms_out_report_select]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'"
                , lot, cus, mate, get_number, post_name, report_date, check_type, check_name, qx, res, remark, base_info.user_code, info1, info2, info3, is_check, dat1, dat2);

            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("查询成功", "系统提示");
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;   
            }

            string lot = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户批号"].Value.ToString();
            string cus = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string mate = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            string get_number = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["来料数量"].Value.ToString();
            string post_name = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["片号"].Value.ToString();
            string report_date = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["报告日期"].Value.ToString();

           // string check_type = 
            this.comboBoxEx1.SelectedItem = this.pwtDataGridView1.SelectedRows[0].Cells["抽检模式"].Value.ToString();
            string check_name = this.textBoxX7.Text = this.pwtDataGridView1.SelectedRows[0].Cells["抽检片号"].Value.ToString();

            string qx = this.textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["缺陷总数"].Value.ToString();
           // string res = 
            this.comboBoxEx2.SelectedItem = this.pwtDataGridView1.SelectedRows[0].Cells["检验结论"].Value.ToString();

            string remark = this.textBoxX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();


            //string info1 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info2 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info3 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string lot = this.textBoxX1.Text;
            string cus = this.textBoxX2.Text;
            string mate = this.textBoxX3.Text;
            string get_number = this.textBoxX4.Text;
            string post_name = this.textBoxX5.Text;
            string report_date = this.textBoxX6.Text;

            string check_type = this.comboBoxEx1.SelectedItem.ToString();
            string check_name = this.textBoxX7.Text;

            string qx = this.textBoxX8.Text;
            string res = this.comboBoxEx2.SelectedItem.ToString();

            string remark = this.textBoxX9.Text;


            string info1 = this.textBoxX10.Text;
            string info2 = this.textBoxX11.Text;
            string info3 = this.textBoxX12.Text;




            string sql = string.Format("[dbo].[hp_cp_1206_qms_out_report_update]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'"
               ,id , lot, cus, mate, get_number, post_name, report_date, check_type, check_name, qx, res, remark, base_info.user_code, info1, info2, info3);

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


            string sql = string.Format("[dbo].[hp_cp_1206_qms_out_report_delete]   '{0}','{1}' ", id, base_info.user_code);

             ex.Exe_Data(sql);
            DtbToUi.DtbDeleteToDGV( this.pwtDataGridView1);

            MessageBox.Show("删除成功", "系统提示");

        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string lot = this.textBoxX1.Text = "";
            string cus = this.textBoxX2.Text = "";
            string mate = this.textBoxX3.Text = "";
            string get_number = this.textBoxX4.Text = "";
            string post_name = this.textBoxX5.Text = "";
            string report_date = this.textBoxX6.Text = "";

            //string check_type = 
                this.comboBoxEx1.SelectedIndex = 0;
            string check_name = this.textBoxX7.Text = "";

            string qx = this.textBoxX8.Text = "";
           // string res =
                this.comboBoxEx2.SelectedIndex = 0;

            string remark = this.textBoxX9.Text = "";


            string info1 = this.textBoxX10.Text = "";
            string info2 = this.textBoxX11.Text = "";
            string info3 = this.textBoxX12.Text = "";

            this.pwtDataGridView1.Columns.Clear();
        }

        private void 查看上传文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            文件集中管理 mform = new 文件集中管理("质量出料检验报告", id);
            mform.ShowDialog();
        }
    }
}
