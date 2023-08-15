using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public partial class 工程异常分析图谱 : Form
    {
        public 工程异常分析图谱(string i_lot, string i_No, string i_process, string i_test_type,string i_only_lot="")
        {
            lot = i_lot;
            No = i_No;
            Process = i_process;
            test_type = i_test_type;

            only_lot = i_only_lot;
            InitializeComponent();
        }

        private string lot = "";
        private string No = "";
        private string Process = "";
        private string test_type = "";
        private string only_lot = "";
        pwt_system_comm.ReportCharHelper rc = new pwt_system_comm.ReportCharHelper();
        db_deal ex = new db_deal();

        private void 工程异常分析图谱_Load(object sender, EventArgs e)
        {





            DataSet dt = ex.Get_Dset("[dbo].[analy_tsk_product_select] '" + lot + "','" + No + "','" + Process + "','" + test_type + "','" + only_lot + "'");

            this.pwtDataGridView1.DataSource = dt.Tables[0];
            this.pwtDataGridView2.DataSource = dt.Tables[1];
            this.pwtDataGridView3.DataSource = dt.Tables[2];


            DataSet dt_report = ex.Get_Dset("[dbo].[analy_tsk_product_report_select] '" + lot + "','" + No + "','" + Process + "','" + test_type + "','" + only_lot + "'");

            string CharType_Site = pwt_system_comm.CharType.柱线型;
            rc.ToChar(rc.changeDta(dt_report.Tables[0]), new string[] { "TotalDie", "PassDie", "良率" }, CharType_Site, this.chart1, true);

            string CharType_Bin = pwt_system_comm.CharType.饼型;
            rc.ToChar(rc.changeDta(dt_report.Tables[1]), new string[] {   "不良占比" }, CharType_Bin, this.chart2,true);




            //==============================================
            DataTable dt01 = ex.Get_Data("[dbo].[CC_sys_system_basic_info_select] 'TSK图表工序'");
            DtbToUi.DtbToComboBoxEx(dt01, this.comboBoxEx1);


            DataTable dt02 = ex.Get_Data("[dbo].[CC_sys_system_basic_info_select] 'TSK图表类型'");
            DtbToUi.DtbToComboBoxEx(dt02, this.comboBoxEx2);

            this.comboBoxEx1.SelectedIndex = 0;
            this.comboBoxEx2.SelectedIndex = 0;


        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {


            try
            {
                this.buttonX2.Enabled = false;
                Process = this.comboBoxEx1.SelectedItem.ToString();
                test_type = this.comboBoxEx2.SelectedItem.ToString();

                if (Process == "全部")
                {
                    Process = "";
                }

                if (test_type == "全部")
                {
                    test_type = "";
                }

                DataSet dt = ex.Get_Dset("[dbo].[analy_tsk_product_select] '" + lot + "','" + No + "','" + Process + "','" + test_type + "','" + only_lot + "'");

                this.pwtDataGridView1.DataSource = dt.Tables[0];
                this.pwtDataGridView2.DataSource = dt.Tables[1];
                this.pwtDataGridView3.DataSource = dt.Tables[2];


                DataSet dt_report = ex.Get_Dset("[dbo].[analy_tsk_product_report_select] '" + lot + "','" + No + "','" + Process + "','" + test_type + "','" + only_lot + "'");

                string CharType_Site = pwt_system_comm.CharType.柱线型;
                rc.ToChar(rc.changeDta(dt_report.Tables[0]), new string[] { "TotalDie", "PassDie", "良率" }, CharType_Site, this.chart1, true);

                string CharType_Bin = pwt_system_comm.CharType.饼型;
                rc.ToChar(rc.changeDta(dt_report.Tables[1]), new string[] { "不良占比" }, CharType_Bin, this.chart2, true);


                MessageBox.Show("二次查询成功","系统提示");
            }
            finally
            {
                this.buttonX2.Enabled = true;
            }
        }
    }
}
