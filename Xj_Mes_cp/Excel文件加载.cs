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
    public partial class Excel文件加载 : Form
    {
        public Excel文件加载()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {


            string ExcelPath = @"C:\Users\Administrator\Desktop\4000167115-BZ1903AC-C1S(1).xls";

           // DataTable dt = pwt_system_comm.ExcelHelper.InputFromExcel(ExcelPath, "[Sheet1]");

            DataTable dt = pwt_system_comm_out.NPIOExcelHelper.ImportExeclToDataTable();

            this.pwtDataGridView1.DataSource = dt;

            this.pwtDataGridView2.Rows.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.pwtDataGridView2.Rows.Add();

                this.pwtDataGridView2.Rows[i].Cells[0].Value = dt.Rows[i]["委工單號"].ToString();
                this.pwtDataGridView2.Rows[i].Cells[1].Value = dt.Rows[i]["委工單項次"].ToString();
                this.pwtDataGridView2.Rows[i].Cells[2].Value = dt.Rows[i]["Lot No. in"].ToString();
                this.pwtDataGridView2.Rows[i].Cells[3].Value = dt.Rows[i]["Lot No. Out"].ToString();

                if (dt.Rows[i]["Part No. out."].ToString().Split('-').Length == 2)
                {
                    this.pwtDataGridView2.Rows[i].Cells[4].Value = dt.Rows[i]["Part No. out."].ToString().Split('-')[0];
                    this.pwtDataGridView2.Rows[i].Cells[5].Value = dt.Rows[i]["Part No. out."].ToString().Split('-')[1];
                }
                else {
                    this.pwtDataGridView2.Rows[i].Cells[4].Value = dt.Rows[i]["Part No. out."].ToString();
                    this.pwtDataGridView2.Rows[i].Cells[5].Value = "";
                }

             

                this.pwtDataGridView2.Rows[i].Cells[6].Value = dt.Rows[i]["Part No. in"].ToString();
                this.pwtDataGridView2.Rows[i].Cells[7].Value = dt.Rows[i]["Part No. out."].ToString();
                this.pwtDataGridView2.Rows[i].Cells[8].Value = dt.Rows[i]["Date Code"].ToString();

                this.pwtDataGridView2.Rows[i].Cells[9].Value = dt.Rows[i]["數量1"].ToString();
                this.pwtDataGridView2.Rows[i].Cells[10].Value = dt.Rows[i]["刻號資訊"].ToString();
                this.pwtDataGridView2.Rows[i].Cells[11].Value = dt.Rows[i]["加工方式"].ToString();

                //#01-05,07-25



                string post_info = "";
                foreach (var item_C in dt.Rows[i]["刻號資訊"].ToString().Split(','))
                {

                    string item=item_C.Replace("#","");
                    if (item.Contains('-'))
                    {
                        int s_i =int.Parse( item.Split('-')[0]);
                        int e_i = int.Parse(item.Split('-')[1]);

                        for (int j = s_i; j <= e_i; j++)
                        {
                            post_info += j.ToString().PadLeft(2, '0') + "、";
                        }


                    }
                    else {
                        post_info += item.PadLeft(2, '0') + "、";
                    
                    }
                    
                }
                post_info = post_info.Substring(0, post_info.Length - 1);
                this.pwtDataGridView2.Rows[i].Cells[11].Value = post_info;
            }
        }
    }
}
