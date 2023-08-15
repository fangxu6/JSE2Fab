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
    public partial class 质量来料检验报告 : DockContent
    {
        public 质量来料检验报告()
        {
            InitializeComponent();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string cus_name = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string mate_name = this.textBoxX3.Text;
            string mate_spce = this.textBoxX4.Text;
            string mate_number = this.textBoxX5.Text;
            string mate_post = this.textBoxX6.Text;
            string check_number = this.textBoxX7.Text;
            string remark = this.textBoxX8.Text;

            string info1 = this.textBoxX9.Text;
            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX11.Text;


            string IQC_only = GetPurchesCode("质量来料检验报告", "IQ"+DateTime.Now.ToString("yyyyMMdd"));

            string sql = string.Format("[dbo].[hp_cp_1206_qms_report_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'",
                IQC_only, cus_name, lot, mate_name, mate_spce, mate_number, mate_post, check_number, remark, info1, info2, info3,base_info.user_code);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("添加成功", "系统提示");
        }
        db_deal ex = new db_deal();
        private string GetPurchesCode(string name, string TitleNo)
        {

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(2, '0');

            return sturct + sturct_info;

        }
        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string cus_name = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string lot = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户批号"].Value.ToString();
            string mate_name = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["产品名称"].Value.ToString();
            string mate_spce = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["规格型号"].Value.ToString();
            string mate_number = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["来料数量"].Value.ToString();
            string mate_post = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["片号"].Value.ToString();
            string check_number = this.textBoxX7.Text = this.pwtDataGridView1.SelectedRows[0].Cells["检验数量"].Value.ToString();
            string remark = this.textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();

            //string info1 = this.textBoxX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info2 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info3 = this.textBoxX11.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();


            string report_only = this.textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["报告编号"].Value.ToString();

            DataTable dt = ex.Get_Data(" [dbo].[hp_cp_1206_qms_report_error_total_select] '" + report_only + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);
        }

        private void 质量来料检验报告_Load(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string cus_name = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string mate_name = this.textBoxX3.Text;
            string mate_spce = this.textBoxX4.Text;
            string mate_number = this.textBoxX5.Text;
            string mate_post = this.textBoxX6.Text;
            string check_number = this.textBoxX7.Text;
            string remark = this.textBoxX8.Text;

            string info1 = this.textBoxX9.Text;
            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX11.Text;


            string IQC_only = "";// GetPurchesCode("质量来料检验报告", "IQ" + DateTime.Now.ToString("yyyyMMdd"));


            string icheck_date = "0";
            if (this.checkBoxX1.Checked==true)
            {
                icheck_date = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string sql = string.Format("[dbo].[hp_cp_1206_qms_report_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'",
                IQC_only, cus_name, lot, mate_name, mate_spce, mate_number, mate_post, check_number, remark, info1, info2, info3, base_info.user_code, icheck_date,dat1,dat2);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            if (MessageBox.Show("确定修改选择的信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string cus_name = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string mate_name = this.textBoxX3.Text;
            string mate_spce = this.textBoxX4.Text;
            string mate_number = this.textBoxX5.Text;
            string mate_post = this.textBoxX6.Text;
            string check_number = this.textBoxX7.Text;
            string remark = this.textBoxX8.Text;

            string info1 = this.textBoxX9.Text;
            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX11.Text;


            string IQC_only = "";// GetPurchesCode("质量来料检验报告", "IQ" + DateTime.Now.ToString("yyyyMMdd"));

            string sql = string.Format("[dbo].[hp_cp_1206_qms_report_update] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}'",
              id,  IQC_only, cus_name, lot, mate_name, mate_spce, mate_number, mate_post, check_number, remark, info1, info2, info3, base_info.user_code);

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
            if (MessageBox.Show("确定删除选择的信息","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return; 
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string sql = string.Format("[dbo].[hp_cp_1206_qms_report_delete] '{0}','{1}'", id, base_info.user_code);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbDeleteToDGV( this.pwtDataGridView1);

            MessageBox.Show("删除成功","系统提示");

        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string cus_name = this.textBoxX1.Text = "";
            string lot = this.textBoxX2.Text = "";
            string mate_name = this.textBoxX3.Text = "";
            string mate_spce = this.textBoxX4.Text = "";
            string mate_number = this.textBoxX5.Text = "";
            string mate_post = this.textBoxX6.Text = "";
            string check_number = this.textBoxX7.Text = "";
            string remark = this.textBoxX8.Text = "";

            string info1 = this.textBoxX9.Text = "";
            string info2 = this.textBoxX10.Text = "";
            string info3 = this.textBoxX11.Text = "";


            this.pwtDataGridView1.Columns.Clear();
            this.pwtDataGridView2.Columns.Clear();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string report_only = this.pwtDataGridView1.SelectedRows[0].Cells["报告编号"].Value.ToString();

            报告单_片号检测信息 mfrom = new 报告单_片号检测信息(report_only);
            mfrom.ShowDialog();



            pwtDataGridView1_MouseDoubleClick(null, null);

        }

        private void 上传图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            文件集中管理 mform = new 文件集中管理("质量来料检验报告", id);
            mform.ShowDialog();
        }
    }
}
