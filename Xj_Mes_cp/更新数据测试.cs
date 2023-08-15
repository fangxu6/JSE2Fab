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
    public partial class 更新数据测试 : Form
    {
        public 更新数据测试()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db_deal ex=new db_deal ();

            string sql = @"
                    SELECT [dbo].[hp_20220329_total_tsk_info].ID FROM [dbo].[hp_20220329_total_tsk_info]
                    ORDER BY [dbo].[hp_20220329_total_tsk_info].[采集时间]
                    ";
            DataTable dt = ex.Get_Data(sql);

            Application.DoEvents();
            this.progressBar1.Maximum = dt.Rows.Count - 1;
            

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.progressBar1.Value = i;
                this.label1.Text = i.ToString() + "/" +( dt.Rows.Count - 1).ToString();
                ex.Exe_Data("[dbo].[hp_20220329_total_tsk_info_only_one_insert] '"+dt.Rows[i][0].ToString()+"'");
            }
            MessageBox.Show("OK");



            //
        }

        private void button2_Click(object sender, EventArgs e)
        {

              //SELECT [dbo].[hp_20220329_total_tsk_info].ID FROM [dbo].[hp_20220329_total_tsk_info]
              //      ORDER BY [dbo].[hp_20220329_total_tsk_info].[采集时间]

            db_deal ex=new db_deal ();

            string sql = @"
                 SELECT   [dbo].[hp_20220329_total_tsk_info].ID
                 FROM [dbo].[hp_20220329_total_tsk_info]
                WHERE
                CONVERT(datetime,[dbo].[hp_20220329_total_tsk_info].[采集时间])>CONVERT(datetime,'2022-1-1 00:00:00')
                ORDER BY [dbo].[hp_20220329_total_tsk_info].[采集时间]
                    ";
            DataTable dt = ex.Get_Data(sql);

            Application.DoEvents();
            this.progressBar1.Maximum = dt.Rows.Count - 1;
            

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.progressBar1.Value = i;
                this.label1.Text = i.ToString() + "/" +( dt.Rows.Count - 1).ToString();
                ex.Exe_Data("[dbo].[hp_20220811_update_cp] '"+dt.Rows[i][0].ToString()+"'");
            }
            MessageBox.Show("OK");


 
        }
    }
}
