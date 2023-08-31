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
    public partial class 各种报表管理 : DockContent
    {
        public 各种报表管理()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {


            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker1.Value.AddDays(1).ToString("yyyy-MM-dd");

            string cus = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string mate_type = this.textBoxX3.Text;

            string isdate = "0";
            if (this.checkBox1.Checked==true)
            {
                isdate = "1";
            }

            string select_name = this.comboBoxEx1.SelectedItem.ToString();

            string select_sql = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (select_name==dt.Rows[i]["report_name"].ToString())
                {
                    select_sql = dt.Rows[i]["report_sql"].ToString();
                }
            }
            // 是否时间,时间1,时间2，客户名称,批次号,晶圆型号
            string sql = string.Format(select_sql, isdate, dat1, dat2, cus, lot, mate_type);

            DataTable dt_report = ex.Get_Data(sql);
            this.pwtDataGridView1.DataSource = dt_report;
            MessageBox.Show("查询成功","系统提示");
        }

        db_deal ex = new db_deal();

        DataTable dt = new DataTable();
        private void 各种报表管理_Load(object sender, EventArgs e)
        {

            this.comboBoxEx1.SelectedIndex = 0;
              dt = ex.Get_Data("[dbo].[HP_REPORT_INFO_LIST_SELECT] 'CP'");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.comboBoxEx1.Items.Add(dt.Rows[i]["report_name"].ToString());
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.pwtDataGridView1.DataSource = null;
        }
    }
}
