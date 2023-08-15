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
    public partial class Lot数量选择 : Form
    {
        public Lot数量选择()
        {
            InitializeComponent();
        }

        int h = 5;
        int w = 5;
        int total_no = 25;



        #region LoadLotNo
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
        #endregion

        private void Lot数量选择_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            LoadLotNo();
            check_number();

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            
        }


        #region 右击选择选中
        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.SelectedCells.Count; i++)
            {
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.DarkGray)
                {
                    continue;
                }
                this.dataGridView1.SelectedCells[i].Style.BackColor = Color.Green;
            }
            check_number();
        }
        #endregion

        #region 右击选中清除
        private void 选中清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.SelectedCells.Count; i++)
            {
                if (this.dataGridView1.SelectedCells[i].Style.BackColor == Color.DarkGray)
                {
                    continue;
                }
                this.dataGridView1.SelectedCells[i].Style.BackColor = Color.White;
            }
            check_number();
        }
        #endregion

        #region 双击
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (this.dataGridView1.SelectedCells[0].Style.BackColor == Color.DarkGray)
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
        #endregion

        #region check_number
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
        #endregion

        #region 全选
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
                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Green;
                }
            }
            check_number();
        }
        #endregion

        #region 清空
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

                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
            check_number();
        } 
        #endregion



        public string select_ok = "1";

        public List<int> str_no = new List<int>();

        public string total_number = "";
        public string total_point = "";
        public string total_point_remark = "";

        #region 关闭
        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_ok = "1";
            this.Close();
        } 
        #endregion

        #region 提交
        private void buttonX1_Click(object sender, EventArgs e)
        {

            select_ok = "0";
            total_number = this.labelX2.Text;
            total_point = this.textBox1.Text;
            total_point_remark = this.textBox2.Text;
            this.Close();
        } 
        #endregion


    }
}
