using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_Report
{
    public partial class CP产量统计 : DockContent
    {
        public CP产量统计()
        {
            InitializeComponent();
        }

        private void CP产量统计_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }
        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {



            string cus_name = this.textBoxX1.Text;

            string test_type = this.comboBoxEx1.SelectedItem.ToString();




            if (test_type=="全部")
            {
                test_type = "";
            }


            string is_date = "";
            if (this.checkBoxX1.Checked == true)
            {
                is_date = "1";
            }
            else {
                is_date = "0";
            }


            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd");

            //班次
            string sql = string.Format("[dbo].[report_cp_test_every_day_select] '{0}','{1}','{2}','{3}','{4}'", cus_name, test_type, is_date, dat1, dat2);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

            //天
            string sql1 = string.Format("[dbo].[report_cp_test_every_day_select_day] '{0}','{1}','{2}','{3}','{4}'", cus_name, test_type, is_date, dat1, dat2);

            DataTable dt1 = ex.Get_Data(sql1);

            DtbToUi.DtbToDGV(dt1, this.pwtDataGridView2);


            pwt_system_comm.ReportCharHelper rc = new pwt_system_comm.ReportCharHelper();


            DataTable dt1_report = dt1.Copy();
            dt1_report.Columns.Remove(dt1_report.Columns["Pass总数"]);
            dt1_report.Columns.Remove(dt1_report.Columns["Total总数"]);

            for (int i = 0; i < dt1_report.Rows.Count; i++)
            {
                dt1_report.Rows[i]["日期"] =  dt1_report.Rows[i]["日期"].ToString().Substring(5);
                dt1_report.Rows[i]["合格率"]= float.Parse( dt1_report.Rows[i]["合格率"].ToString().Replace("%","")) ;
            }
            rc.ToChar(rc.changeDta(dt1_report), new string[] { "批次数量", "合格率" }, pwt_system_comm.CharType.柱线型, this.chart1);

            this.chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 60;
            this.chart1.ChartAreas[0].AxisX.Interval = 1;
            //月
            string sql2 = string.Format("[dbo].[report_cp_test_every_day_select_month] '{0}','{1}','{2}','{3}','{4}'", cus_name, test_type, is_date, dat1, dat2);

            DataTable dt2 = ex.Get_Data(sql2);

            DtbToUi.DtbToDGV(dt2, this.pwtDataGridView3);
            DataTable dt2_report = dt2.Copy();
            dt2_report.Columns.Remove(dt2_report.Columns["Pass总数"]);
            dt2_report.Columns.Remove(dt2_report.Columns["Total总数"]);
            for (int i = 0; i < dt2_report.Rows.Count; i++)
            {
                dt2_report.Rows[i]["日期"] = dt2_report.Rows[i]["日期"].ToString().Substring(5);
                dt2_report.Rows[i]["合格率"] = float.Parse(dt2_report.Rows[i]["合格率"].ToString().Replace("%", ""))  ;
            }
            rc.ToChar(rc.changeDta(dt2_report), new string[] { "批次数量", "合格率" }, pwt_system_comm.CharType.柱线型, this.chart2);
            this.chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 60;
            this.chart2.ChartAreas[0].AxisX.Interval = 1;
           
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
         
        }
    }
}
