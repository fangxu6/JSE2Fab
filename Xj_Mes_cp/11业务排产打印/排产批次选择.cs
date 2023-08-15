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
    public partial class 排产批次选择 : Form
    {
        string my_mate_type = "";
        string my_cus_name = "";
        string my_lot = "";
        public 排产批次选择(string cus_name,string mate_type,string lot)
        {
            my_cus_name = cus_name;
            my_mate_type = mate_type;
            my_lot = lot;
            InitializeComponent();
        }

        int h = 5;
        int w = 5;
        int total_no = 25;



        private void LoadLotNo()
        {
            this.dataGridView1.Columns.Clear();
            for (int i = 0; i < w; i++)
            {
                this.dataGridView1.Columns.Add(string.Format("第 {0} 列", (i + 1).ToString()), string.Format("第 {0} 列", (i + 1).ToString()));
                
            }

            int temp = 1;
            for (int i = 0; i < h; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Height = 50;
                for (int j = 0; j < w; j++)
                {
                    if (temp > total_no)
                    {

                        this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.DarkGray;
                        this.dataGridView1.Rows[i].Cells[j].Value = "不可以选";

                    }
                    else
                    {
                        if (temp.ToString().Length < 2)
                        {
                            this.dataGridView1.Rows[i].Cells[j].Value = "0" + temp.ToString();
                        }
                        else
                        {
                            this.dataGridView1.Rows[i].Cells[j].Value = "" + temp.ToString();
                        }
                        this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                        temp++;
                    }

                }
            }



        }


        db_deal ex = new db_deal();
        private void Lot数量选择_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            LoadLotNo();
            check_number();

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;



            //查询库存产品信息

            DataTable dt = ex.Get_Data("[dbo].[hp_0915_cp_res_info_select_lot_select] '" + my_cus_name + "','" + my_mate_type + "','" + my_lot + "'");
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

            this.pwtDataGridView1_MouseDoubleClick(null, null);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.SelectedCells.Count; i++)
            {
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.DarkGray)
                {
                    continue;
                }
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.Red)
                {
                    continue;
                }
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.Yellow)
                {
                    continue;
                }
                this.dataGridView1.SelectedCells[i].Style.BackColor = Color.Green;
            }
            check_number();
        }

        private void 选中清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.SelectedCells.Count; i++)
            {
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.DarkGray)
                {
                    continue;
                }
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.Red)
                {
                    continue;
                }
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.Yellow)
                {
                    continue;
                }
                this.dataGridView1.SelectedCells[i].Style.BackColor = Color.White;
            }
            check_number();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (this.dataGridView1.SelectedCells[0].Style.BackColor == Color.DarkGray)
            {
                return;
            }
            if (this.dataGridView1.SelectedCells[0].Style.BackColor == Color.Red)
            {
                return;
            } 
            if (this.dataGridView1.SelectedCells[0].Style.BackColor == Color.Yellow)
            {
                return;
            }
            if (this.dataGridView1.SelectedCells[0].Style.BackColor == Color.Green)
            {
                this.dataGridView1.SelectedCells[0].Style.BackColor = Color.White;
            }
            else
            {
                this.dataGridView1.SelectedCells[0].Style.BackColor = Color.Green;
            }

            check_number();
        }

        private void check_number()
        {

            int temp = 0;
            string str_post = "";
            str_no = new List<int>();


            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                {
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.DarkGray)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Red)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Yellow)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Value.ToString() == "不可以选")
                    {
                        continue;
                    }

                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Green)
                    {
                        temp++;
                        str_post += this.dataGridView1.Rows[i].Cells[j].Value.ToString() + "、";
                        int temp_np = Convert.ToInt32(this.dataGridView1.Rows[i].Cells[j].Value.ToString().Replace('#', ' ').Trim());
                        str_no.Add(temp_np);
                    }

                }
            }


            List<string> SimpleNameString = LotSelect.PinSimpleString(str_no);

            string temp_jx = "";
            if (SimpleNameString != null)
            {
                foreach (var item in SimpleNameString)
                {
                    temp_jx += item + "；";
                }
            }
           
           
            if (temp_jx != "")
            {
                temp_jx = temp_jx.Substring(0, temp_jx.Length - 1);
            }

            this.textBox2.Text = temp_jx;



            if (str_post != "")
            {
                str_post = str_post.Substring(0, str_post.Length - 1);
            }

            this.labelX2.Text = temp.ToString();
            this.textBox1.Text = str_post;
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {

                for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                {
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.DarkGray)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Red)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Yellow)
                    {
                        continue;
                    }
                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Green;
                }
            }
            check_number();
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                {

                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.DarkGray)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Red)
                    {
                        continue;
                    }
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Yellow)
                    {
                        continue;
                    }
                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
            check_number();
        }



        public string select_ok = "1";

        public List<int> str_no = new List<int>();

        public string total_number = "";
        public string total_point = "";
        public string total_point_remark = "";


        public string res_id = "";
        public string cus_name = "";
        public string cus_code = "";
        public string select_mate_type = "";
        public string select_lot = "";

        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_ok = "1";
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            select_ok = "0";
            total_number = this.labelX2.Text;
            total_point = this.textBox1.Text;
            total_point_remark = this.textBox2.Text;


            cus_name = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            cus_code = this.pwtDataGridView1.SelectedRows[0].Cells["客户代码"].Value.ToString();
            select_mate_type = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            select_lot = this.pwtDataGridView1.SelectedRows[0].Cells["LOT"].Value.ToString();
            res_id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            this.Close();
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            DataTable dt = ex.Get_Data("[dbo].[hp_0915_cp_res_info_list_post_get_select] '" + id + "'");


            int NO1 = 0;
            int NO2 = 0;
            int NO3 = 0;
            int NO4 = 0;

            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {

                for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                {

                    string p_name = this.dataGridView1.Rows[i].Cells[j].Value.ToString();

                   this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.DarkGray;
                   for (int x = 0; x < dt.Rows.Count; x++)
                   {
                       string db_p_name = dt.Rows[x][0].ToString();
                       string db_p_state = dt.Rows[x][1].ToString();
                       if (p_name == db_p_name)
                       {

                           switch (db_p_state)
                           {
                               case "已入库":
                                   NO3++;
                                   this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                                   break;
                               case "已排产":
                                   NO2++;
                                   this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                   break;
                               case "已领料":
                                   NO2++;
                                   this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                   break;
                               case "冻结":
                                   NO4++;
                                   this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                                   break;
                               default:
                                   NO1++;
                                   this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.DarkGray;
                                   break;
                           }
                       }
                   }
                }
            }


            this.labelX12.Text = (25 - NO2 - NO3 - NO4).ToString();
            this.labelX13.Text = NO2.ToString();
            this.labelX14.Text = NO3.ToString();
            this.labelX18.Text = NO4.ToString();

        }


    }
}
