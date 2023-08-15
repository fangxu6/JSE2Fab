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
    public partial class 生产异常处理报告 : DockContent
    {
        public 生产异常处理报告()
        {
            InitializeComponent();
        }

        #region 客户名称查询
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            string CUS_NAME = this.pwtSearchBox1.Text;
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT01] 'CP','" + CUS_NAME + "','' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
        } 
        #endregion

        #region 产品型号选择
        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {

            string CUS_NAME = this.pwtSearchBox1.Text;
            string MATE_NAME = this.pwtSearchBox2.Text;
            //待调整SQL
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_W_Wafer_Materials_information_Info_SELECT01] '" + MATE_NAME + "','" + CUS_NAME + "'", new List<int> { 0, 1, 2 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }


            this.pwtSearchBox2.Text = mfrom.select_name[2];
        }

        #endregion

        #region GetCPCode
        private string GetCPCode(string name, string TitleNo)
        {

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(3, '0');

            return sturct + sturct_info;

        } 
        #endregion

        db_deal ex = new db_deal();
        #region 登记
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string mate_name = this.pwtSearchBox2.Text;
            string lot = this.textBoxX1.Text;

            string eq_code = this.pwtSearchBox3.Text;
            string lot_number = this.textBoxX2.Text;
            string error_number = this.textBoxX3.Text;

            string error_info = this.textBoxX4.Text;

            string find_user = this.textBoxX5.Text;
            string find_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string do_user = base_info.user_code;

            string info1 = this.textBoxX6.Text;
            string info2 = this.textBoxX13.Text;//批次良率
            string info3 = this.pwtSearchBox4.Text;//单片良率
            string info4 = this.comboBoxEx1.SelectedItem.ToString();//工序
            string info5 = this.pwtSearchBox5.Text;//流程卡号



            string error_only_code = GetCPCode("生产批次异常", "E" + DateTime.Now.ToString("yyyyMMdd"));
            string info6 = error_only_code;//this.textBoxX11.Text;

            string sql = string.Format("[dbo].[hp_1220_cp_error_report_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'",
                cus_name, mate_name, lot, eq_code, lot_number, error_number,
                error_info, find_user, find_date, do_user,
                info1, info2, info3, info4, info5, info6);
            DataTable dt = ex.Get_Data(sql);


            DataTable dtb = ex.Get_Data("[dbo].[CC_sys_system_basic_info_select] 'CP异常报告部门'");

            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                //ex.Exe_Data("[dbo].[hp_1220_cp_error_why_info_depe_insert]  '" + dt.Rows[0]["序号"].ToString() + "','" + dtb.Rows[i][0].ToString() + "'");
                string str = string.Format("[dbo].[hp_1220_cp_error_why_info_depe_insert01] '{0}','{1}','{2}','{3}'",
                    dt.Rows[0]["序号"].ToString(), dtb.Rows[i][0].ToString(), "", "");
                ex.Exe_Data(str);
            }

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("登记成功", "系统提示");
        } 
        #endregion

        #region 查询
        private void buttonX1_Click(object sender, EventArgs e)
        {


            string cus_name = this.pwtSearchBox1.Text;
            string mate_name = this.pwtSearchBox2.Text;
            string lot = this.textBoxX1.Text;

            string eq_code = this.pwtSearchBox3.Text;
            string lot_number = this.textBoxX2.Text;
            string error_number = this.textBoxX3.Text;

            string error_info = this.textBoxX4.Text;

            string find_user = this.textBoxX5.Text;
            string find_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string do_user = base_info.user_code;

            string info1 = this.textBoxX6.Text;
            string info2 = this.textBoxX13.Text;//批次良率
            string info3 = this.pwtSearchBox4.Text;//单片良率
            string info4 = this.textBoxX9.Text;
            string info5 = this.textBoxX10.Text;
            string info6 = this.textBoxX11.Text;

            string is_check = "0";

            if (this.checkBoxX1.Checked == true)
            {

                is_check = "1";
            }
            string dat1 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");


            string sql = string.Format("[dbo].[hp_1220_cp_error_report_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'",
                cus_name, mate_name, lot, eq_code, lot_number, error_number,
                error_info, find_user, find_date, do_user,
                info1, info2, info3, info4, info5, info6, is_check, dat1, dat2);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("查询成功", "系统提示");



        }
        #endregion

        #region 双击显示
        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string cus_name = this.pwtSearchBox1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string mate_name = this.pwtSearchBox2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            string lot = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户批次"].Value.ToString();

            string eq_code = this.pwtSearchBox3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["机台编号"].Value.ToString();
            string lot_number = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["批量数"].Value.ToString();
            string error_number = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["不良数"].Value.ToString();

            string error_info = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["异常描述"].Value.ToString();

            string find_user = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["发现人"].Value.ToString();
            //  string find_date =
            this.dateTimePicker1.Value = DateTime.Parse(this.pwtDataGridView1.SelectedRows[0].Cells["发现日期"].Value.ToString());

            string do_user = base_info.user_code;

            string info1 = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();
            string info2 = this.textBoxX13.Text = this.pwtDataGridView1.SelectedRows[0].Cells["批次良率"].Value.ToString();
            string info3 = this.pwtSearchBox4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["单片良率"].Value.ToString();
            string info4 = this.textBoxX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells["工序"].Value.ToString();
            //string info5 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info6 = this.textBoxX11.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
        } 
        #endregion

        #region 修改
        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }


            if (MessageBox.Show("确定修改？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            string cus_name = this.pwtSearchBox1.Text;
            string mate_name = this.pwtSearchBox2.Text;
            string lot = this.textBoxX1.Text;

            string eq_code = this.pwtSearchBox3.Text;
            string lot_number = this.textBoxX2.Text;
            string error_number = this.textBoxX3.Text;

            string error_info = this.textBoxX4.Text;

            string find_user = this.textBoxX5.Text;
            string find_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string do_user = base_info.user_code;

            string info1 = this.textBoxX6.Text;
            string info2 = this.textBoxX13.Text;//批次良率
            string info3 = this.pwtSearchBox4.Text;//单片良率
            string info4 = this.comboBoxEx1.SelectedItem.ToString();
            string info5 = this.textBoxX10.Text;
            string info6 = this.textBoxX11.Text;

            string is_check = "0";

            if (this.checkBoxX1.Checked == true)
            {

                is_check = "1";
            }
            string dat1 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");


            string sql = string.Format("[dbo].[hp_1220_cp_error_report_update] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}'",
              id, cus_name, mate_name, lot, eq_code, lot_number, error_number,
                error_info, find_user, find_date, do_user,
                info1, info2, info3, info4, info5, info6);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbUpdateToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("修改成功", "系统提示");
        }
        #endregion

        #region 删除
        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("确定删除？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            ex.Exe_Data("[dbo].[hp_1220_cp_error_report_delete]  '" + id + "','" + base_info.user_code + "'");

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
            MessageBox.Show("删除成功", "系统提示");
        }
        #endregion

        #region 清空
        private void buttonX5_Click(object sender, EventArgs e)
        {

            string cus_name = this.pwtSearchBox1.Text = "";
            string mate_name = this.pwtSearchBox2.Text = "";
            string lot = this.textBoxX1.Text = "";

            string eq_code = this.pwtSearchBox3.Text = "";
            string lot_number = this.textBoxX2.Text = "";
            string error_number = this.textBoxX3.Text = "";

            string error_info = this.textBoxX4.Text = "";

            string find_user = this.textBoxX5.Text = "";
            //  string find_date =
            this.dateTimePicker1.Value = DateTime.Now;

            string do_user = base_info.user_code;

            string info1 = this.textBoxX6.Text = "";
            string info2 = this.textBoxX7.Text = "";
            string info3 = this.textBoxX8.Text = "";
            string info4 = this.textBoxX9.Text = "";
            string info5 = this.textBoxX10.Text = "";
            string info6 = this.textBoxX11.Text = "";

            this.pwtSearchBox4.Text = "";
            textBoxX13.Text = "";
            this.comboBoxEx1.SelectedIndex = 0;

            this.pwtDataGridView1.Columns.Clear();
        }
        #endregion

        #region 单片良率
        private void pwtSearchBox4_SearchBtnClick(object sender, EventArgs e)
        {
            生产异常处理单片良率信息管理 mfrom = new 生产异常处理单片良率信息管理();
            mfrom.ShowDialog();
            string pass = mfrom.my_info;
            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox4.Text = pass;
        }
        #endregion

        private void 生产异常处理报告_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        private void pwtSearchBox5_SearchBtnClick(object sender, EventArgs e)
        {
            string lot = this.pwtSearchBox5.Text;
            string sql = string.Format("[dbo].[hp_1022_cp_up_line_select01] '{0}'", lot);
            DataTable dt = ex.Get_Data(sql);
            if (dt.Rows.Count<=0)
            {
                return;
            }
            this.pwtSearchBox1.Text = dt.Rows[0]["客户名称"].ToString();
            this.pwtSearchBox2.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX1.Text = dt.Rows[0]["LOT"].ToString();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            string str1 = this.textBoxX12.Text;//片号
            string str2 = this.textBoxX15.Text;//Bin
            if (str1==""&&str2=="")
            {
                return;
            }

            string yc = "片号:#" + str1 + ",Bin:" + str2 + ",不达标;";
            this.textBoxX4.Text = this.textBoxX4.Text + yc;
            this.textBoxX12.Text = "";
            this.textBoxX15.Text = "";
        }
    }
}
