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
    public partial class CP产量报告 : DockContent
    {
        public CP产量报告()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void CP产量报告_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                buttonX1.Enabled = false;
                string cus_name = this.pwtSearchBox1.Text;

                string test_type = this.comboBoxEx1.SelectedItem.ToString();


                string lot = this.textBoxX2.Text;

                if (test_type == "全部")
                {
                    test_type = "";
                }


                string is_date = "";
                if (this.checkBoxX1.Checked == true)
                {
                    is_date = "1";
                }
                else
                {
                    is_date = "0";
                }


                string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string dat2 = this.dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd");
                Application.DoEvents();

                string sql = string.Format("[dbo].[hp_20220210_tsk_mail_send_select] '{0}','{1}','{2}','{3}','{4}','{5}'", cus_name, test_type, is_date, dat1, dat2, lot);

                DataTable dt = ex.Get_Data(sql);

                dtb_xls = dt.Copy();

                //DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
                this.pwtDataGridView1.DataSource = dt;
                MessageBox.Show("查询成功","系统提示");
            }
            finally
            {
                buttonX1.Enabled = true;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            //      \\192.168.5.26\prober\MAP





        }
        DataTable dtb_xls = new DataTable();
        private void buttonX3_Click(object sender, EventArgs e)
        {


            if (this.pwtDataGridView1.Rows.Count==0)
            {
                MessageBox.Show("请查询数据","系统提示"); return;
            }
            pwt_system_comm_out.NPIOExcelHelper.ImportDataTableToExecl(dtb_xls, "CP产量报告"+DateTime.Now.ToString("yyyyMMdd")+".xlsx");
            MessageBox.Show("导出成功", "系统提示");
        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {

        }
    }
}
