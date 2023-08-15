using Seagull.BarTender.Print;
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
    public partial class 生产异常处理管理_工程 : DockContent
    {
        public 生产异常处理管理_工程()
        {
            InitializeComponent();
        }
        string dept_name = "工程";
        private void groupPanel1_Click(object sender, EventArgs e)
        {

        }
        db_deal ex = new db_deal();
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
            string info2 = this.textBoxX7.Text;
            string info3 = this.textBoxX8.Text;
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



           
            string dept_state = this.comboBoxEx1.SelectedItem.ToString();


            if (dept_state=="全部")
            {
                dept_state = "";
            }



            //string sql = string.Format("[dbo].[hp_1220_cp_error_report_deal_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}'",
            //    cus_name, mate_name, lot, eq_code, lot_number, error_number,
            //    error_info, find_user, find_date, do_user,
            //    info1, info2, info3, info4, info5, info6, is_check, dat1, dat2, dept_name, dept_state);
            string sql = string.Format("[dbo].[hp_1220_cp_error_report_deal_select01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}'",
                cus_name, mate_name, lot, eq_code, lot_number, error_number,
                error_info, find_user, find_date, do_user,
                info1, info2, info3, info4, info5, info6, is_check, dat1, dat2, dept_name, dept_state);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("查询成功", "系统提示");
        }

        private void 生产异常处理管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

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
            this.textBoxX13.Text = this.pwtDataGridView1.SelectedRows[0].Cells["批次良率"].Value.ToString();
            this.textBoxX12.Text = this.pwtDataGridView1.SelectedRows[0].Cells["单片良率"].Value.ToString();

            string do_user = base_info.user_code;

            string info1 = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();
            string info2 = this.textBoxX13.Text = this.pwtDataGridView1.SelectedRows[0].Cells["批次良率"].Value.ToString();
            string info3 = this.textBoxX12.Text = this.pwtDataGridView1.SelectedRows[0].Cells["单片良率"].Value.ToString();
            string info4 = this.textBoxX14.Text = this.pwtDataGridView1.SelectedRows[0].Cells["工序"].Value.ToString();
            //string info5 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info6 = this.textBoxX11.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();


            string id=this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            DataTable dt = ex.Get_Data("[dbo].[hp_1220_cp_error_why_info_select]  '" + id + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            //生产异常处理管理信息维护 mfrom = new 生产异常处理管理信息维护(id, dept_name);
            生产异常处理管理信息维护_新 mfrom = new 生产异常处理管理信息维护_新(id, dept_name);
            mfrom.ShowDialog();

            if (mfrom.select_state==false)
            {
                return;
            }

          
            DataTable dt = ex.Get_Data("[dbo].[hp_1220_cp_error_why_info_select]  '" + id + "'");
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);
        }

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


            //this.pwtDataGridView1.Columns.Clear();
            //this.pwtDataGridView2.Columns.Clear();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }


            string FIlePath = Application.StartupPath + @"\2_btw\生产异常处理报告.btw";
            选择打印机 print_show = new 选择打印机();
            print_show.ShowDialog();
            if (print_show.select_state == false)
            {
                return;
            }



            string error_only_code = this.pwtDataGridView1.SelectedRows[0].Cells["异常编号"].Value.ToString();
            string cus_name = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string mate_name = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            string lot = this.pwtDataGridView1.SelectedRows[0].Cells["客户批次"].Value.ToString();

            string eq_code = this.pwtDataGridView1.SelectedRows[0].Cells["机台编号"].Value.ToString();
            string lot_number = this.pwtDataGridView1.SelectedRows[0].Cells["批量数"].Value.ToString();
            string error_number = this.pwtDataGridView1.SelectedRows[0].Cells["不良数"].Value.ToString();
            string error_info = this.pwtDataGridView1.SelectedRows[0].Cells["异常描述"].Value.ToString();
            string find_user = this.pwtDataGridView1.SelectedRows[0].Cells["发现人"].Value.ToString();
            string find_date = this.pwtDataGridView1.SelectedRows[0].Cells["发现日期"].Value.ToString();

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            DataTable dt = ex.Get_Data("[dbo].[hp_1220_cp_error_why_info_select]  '" + id + "'");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("系统错误,未查询到各部门异常处理信息"); return;
            }
            string p_why = "";
            string p_deal = "";
            string p_user = "";
            string p_date = "";

            string g_why = "";
            string g_deal = "";
            string g_user = "";
            string g_date = "";

            string q_why = "";
            string q_deal = "";
            string q_user = "";
            string q_date = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["部门"].ToString() == "生产")
                {
                    p_why = dt.Rows[i]["原因分析"].ToString();
                    p_deal = dt.Rows[i]["处理意见"].ToString();
                    p_user = dt.Rows[i]["操作人"].ToString();
                    p_date = dt.Rows[i]["操作时间"].ToString();
                }
                if (dt.Rows[i]["部门"].ToString() == "工程")
                {
                    g_why = dt.Rows[i]["原因分析"].ToString();
                    g_deal = dt.Rows[i]["处理意见"].ToString();
                    g_user = dt.Rows[i]["操作人"].ToString();
                    g_date = dt.Rows[i]["操作时间"].ToString();
                }
                if (dt.Rows[i]["部门"].ToString() == "质量")
                {
                    q_why = dt.Rows[i]["原因分析"].ToString();
                    q_deal = dt.Rows[i]["处理意见"].ToString();
                    q_user = dt.Rows[i]["操作人"].ToString();
                    q_date = dt.Rows[i]["操作时间"].ToString();
                }
            }






            Engine engine = new Engine(true);
            string mb = FIlePath;
            LabelFormatDocument format = engine.Documents.Open(mb);
            format.PrintSetup.PrinterName = print_show.DefaultPrintMac.ToString();


            format.SubStrings["error_only_code"].Value = error_only_code;

            format.SubStrings["cus_name"].Value = cus_name;
            format.SubStrings["mate_name"].Value = mate_name;
            format.SubStrings["lot"].Value = lot;


            format.SubStrings["eq_code"].Value = eq_code;
            format.SubStrings["lot_number"].Value = lot_number;
            format.SubStrings["error_number"].Value = error_number;

            format.SubStrings["error_info"].Value = error_info;
            format.SubStrings["find_user"].Value = find_user;
            format.SubStrings["find_date"].Value = find_date;



            format.SubStrings["p_why"].Value = p_why;
            format.SubStrings["p_deal"].Value = p_deal;
            format.SubStrings["p_user"].Value = p_user;
            format.SubStrings["p_date"].Value = p_date;



            format.SubStrings["g_why"].Value = g_why;
            format.SubStrings["g_deal"].Value = g_deal;
            format.SubStrings["g_user"].Value = g_user;
            format.SubStrings["g_date"].Value = g_date;


            format.SubStrings["q_why"].Value = q_why;
            format.SubStrings["q_deal"].Value = q_deal;
            format.SubStrings["q_user"].Value = q_user;
            format.SubStrings["q_date"].Value = q_date;


            format.Save();



            Messages messages;
            Result result = format.Print("CP_Lot_Process", 1000, out messages);

            MessageBox.Show("打印成功");
        }

        private void 批次分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string lot = this.pwtDataGridView1.SelectedRows[0].Cells["客户批次"].Value.ToString();
            工程异常分析图谱 mfrom = new 工程异常分析图谱(lot, "", "","");
            mfrom.ShowDialog();

        }
    }
}
