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
    public partial class 报告单_片号检测信息 : Form
    {

        string my_report_only = "";
        public 报告单_片号检测信息(string report_only)
        {
            my_report_only = report_only;
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void 报告单_片号检测信息_Load(object sender, EventArgs e)
        {

            this.pwtDataGridView1.Rows.Clear();
            for (int i = 1; i < 26; i++)
            {
                this.pwtDataGridView1.Rows.Add();
                this.pwtDataGridView1.Rows[i-1].Cells[0].Value = "No."+i.ToString().PadLeft(2,'0');
            }

            string sql = string.Format("System_base_info_select_Sys_Info_name '{0}'", "来料检验报告异常");
            DataTable dt = ex.Get_Data(sql);

            this.pwtDataGridView2.Rows.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.pwtDataGridView2.Rows.Add();
                this.pwtDataGridView2.Rows[i].Cells[0].Value = dt.Rows[i][0].ToString();
            }



           // DataTable dt_cp = ex.Get_Data(" [dbo].[hp_cp_1206_qms_report_error_total_select] '" + my_report_only + "'");


        }

        private void pwtDataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                return;
            }




            输入框双参数选择框 mfrom = new 输入框双参数选择框();
           // 输入信息单选择框 mfrom = new 输入信息单选择框("请输入检验结果");
            mfrom.ShowDialog();

            if (mfrom.select_state==false)
            {
                return;
            }


            string number = mfrom.info1;
            string info_post = mfrom.info2;
            string info_remark = mfrom.info3;


            string NoPost = this.pwtDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string error_info = this.pwtDataGridView2.SelectedRows[0].Cells[0].Value.ToString();


            ex.Exe_Data(string.Format("[dbo].[hp_cp_1206_qms_report_error_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", my_report_only, NoPost, error_info, number, base_info.user_code, info_post,info_remark));
            this.pwtDataGridView2.SelectedRows[0].Cells[1].Value = number;
            this.pwtDataGridView2.SelectedRows[0].Cells[2].Value = info_post;
            this.pwtDataGridView2.SelectedRows[0].Cells[3].Value = info_remark;



        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                return;
            }

            string NoPost = this.pwtDataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            DataTable dt = ex.Get_Data(string.Format("[dbo].[hp_cp_1206_qms_report_error_select] '{0}','{1}'", my_report_only, NoPost));


            for (int j = 0; j < this.pwtDataGridView2.Rows.Count; j++)
            {
                this.pwtDataGridView2.Rows[j].Cells[1].Value = "";
            }

            for (int j = 0; j < this.pwtDataGridView2.Rows.Count; j++)
            {
                string error_info = this.pwtDataGridView2.Rows[j].Cells[0].Value.ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string tb_error = dt.Rows[i]["error_info"].ToString();

                    if (error_info == tb_error)
                    {
                        this.pwtDataGridView2.Rows[j].Cells[1].Value = dt.Rows[i]["error_number"].ToString();
                        this.pwtDataGridView2.Rows[j].Cells[2].Value = dt.Rows[i]["error_post"].ToString();
                        this.pwtDataGridView2.Rows[j].Cells[3].Value = dt.Rows[i]["error_remark"].ToString();
                    }
                }
            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                return;
            }

            string NoPost = this.pwtDataGridView1.SelectedRows[0].Cells[0].Value.ToString();

          


            string error_info = this.pwtDataGridView2.SelectedRows[0].Cells[0].Value.ToString();


            ex.Exe_Data(string.Format("[dbo].[hp_cp_1206_qms_report_error_delete] '{0}','{1}','{2}'", my_report_only, NoPost, error_info));

            this.pwtDataGridView2.SelectedRows[0].Cells[1].Value = "";
        }
    }
}
