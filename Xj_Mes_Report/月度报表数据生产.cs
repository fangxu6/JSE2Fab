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
    public partial class 月度报表数据生产 : DockContent
    {
        public 月度报表数据生产()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string dat1 = this.dateTimePicker1.Value.ToString("yyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.AddDays(1).ToString("yyy-MM-dd");
            DataTable dt = ex.Get_Data("[dbo].[hp_20220329_total_tsk_info_total_create_select]  '" + dat1 + "','" + dat2 + "'");

            this.pwtDataGridView1.DataSource = dt;

            this.labelX3.Text = "0/" + dt.Rows.Count.ToString();
            MessageBox.Show("数据查询成功","系统提示");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

            this.progressBar1.Maximum = this.pwtDataGridView1.Rows.Count - 1;
            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                this.labelX3.Text = i.ToString() + "/" + (this.pwtDataGridView1.Rows.Count - 1).ToString();
                Application.DoEvents();
                this.progressBar1.Value = i;
                string id = this.pwtDataGridView1.Rows[i].Cells[0].Value.ToString();
                ex.Exe_Data("[dbo].[hp_20220329_total_tsk_info_total_insert]   '" + id + "'");
            }
            MessageBox.Show("产生成功", "系统提示");
        }
    }
}
