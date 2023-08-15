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
    public partial class CP_WIP管理 : DockContent
    {
        public CP_WIP管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();

        DataTable dtb_xls = new DataTable();
        private void buttonX1_Click(object sender, EventArgs e)
        {

            string cus_name = this.textBoxX1.Text;
            string cus_code = this.textBoxX2.Text;
            string lot = this.textBoxX3.Text;
            string mate_name = this.textBoxX4.Text;

            string is_date = "1";

            if (this.checkBoxX1.Checked!=true)
            {
                is_date = "0";
            }

            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd");


            string lot_only = this.textBoxX5.Text;

            string jp_state = this.comboBoxEx1.SelectedItem.ToString();

            if (jp_state=="全部")
            {
                jp_state = "";
            }






            DataTable dt = ex.Get_Data("[dbo].[hp_1102_wip_info_select03] '" + cus_code + "','" + cus_name + "','" + lot + "','" + mate_name + "','" + is_date + "','" + dat1 + "','" + dat2 + "','" + jp_state + "','"+lot_only+"'");
            dtb_xls = dt.Copy();
            this.pwtDataGridView1.DataSource = dt;
            this.pwtDataGridView1.Columns["序号"].Visible = false;


            DataTable dt_wms = ex.Get_Data("[dbo].[hp_1102_wip_info_select02_OK_WMS] '" + cus_code + "','" + cus_name + "','" + lot + "','" + mate_name + "','" + is_date + "','" + dat1 + "','" + dat2 + "','" + jp_state + "'");

            this.pwtDataGridView2.DataSource = dt_wms;

            this.pwtDataGridView2.Columns["序号"].Visible = false;



            ////
            //for (int j = 0; j < dt.Columns.Count; j++)
            //{
            //    if (dt.Columns[j].ColumnName.ToString().Contains("_hold"))
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            dt.Rows[i][j - 1] = long.Parse(dt.Rows[i][j - 1].ToString()) - long.Parse(dt.Rows[i][j].ToString());
            //        }
            //    }
            //}





           // DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

          


            //#region 工序变色
            //for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            //{

            //    string info_mate_name = this.pwtDataGridView1.Rows[i].Cells["晶圆型号"].Value.ToString();
            //    string info_mate_ves = this.pwtDataGridView1.Rows[i].Cells["版本"].Value.ToString();


            //    string str_sql = string.Format("[dbo].[HP_PRCOESS_REMARK_SELECT]   '{0}','{1}'", info_mate_name, info_mate_ves);
            //    DataTable dt_process = ex.Get_Data(str_sql);



            //    int columns_no = 0;
            //    for (int x = 0; x < this.pwtDataGridView1.Columns.Count; x++)
            //    {
            //        if (this.pwtDataGridView1.Columns[x].Name=="IQC")
            //        {
            //            columns_no = x;
            //        }
                
            //    }

            //    for (int x = columns_no; x < this.pwtDataGridView1.Columns.Count; x++)
            //    {
            //        int TEMP_CHECK = 0;
            //        for (int j = 0; j < dt_process.Rows.Count; j++)
            //        {
            //            string old_process = dt_process.Rows[j][0].ToString();
            //            string old_process_remark = dt_process.Rows[j][1].ToString();

            //            if (this.pwtDataGridView1.Columns[x].Name.ToString() == old_process_remark ||
            //               this.pwtDataGridView1.Columns[x].Name.ToString() == old_process_remark + "_hold")
            //            {
            //                TEMP_CHECK = 1;
            //            }
            //        }
            //        if (TEMP_CHECK == 0)
            //        {
            //            this.pwtDataGridView1.Rows[i].Cells[x].Style.BackColor = Color.DimGray;
            //        }
            //    }
            //} 
            //#endregion


            MessageBox.Show("查询成功","系统提示");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
        }

        private void CP_WIP管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonX3_Click_1(object sender, EventArgs e)
        {


            pwt_system_comm_out.NPIOExcelHelper.ImportDataTableToExecl(dtb_xls,"CP_WIP.xlsx" );
               MessageBox.Show("导出成功","系统提示");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string lot = this.pwtDataGridView1.SelectedRows[0].Cells["批次"].Value.ToString();
            string no = "";
            string process = "";
            string test_type = "";
            string only_lot = this.pwtDataGridView1.SelectedRows[0].Cells["流程卡"].Value.ToString();

            工程异常分析图谱 mfrom = new 工程异常分析图谱(lot, no, process, test_type, only_lot);
            mfrom.ShowDialog();
        }










    }
}
