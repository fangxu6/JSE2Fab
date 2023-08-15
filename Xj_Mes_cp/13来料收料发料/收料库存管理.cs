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
    public partial class 收料库存管理 : DockContent
    {

        public 收料库存管理()
        {

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


            this.comboBoxEx1.SelectedIndex = 0;

        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 清除ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public string select_ok = "1";

        public List<int> str_no = new List<int>();

        public string total_number = "";
        public string total_point = "";
        public string total_point_remark = "";



        public string cus_name = "";
        public string cus_code = "";
        public string select_mate_type = "";
        public string select_lot = "";

        #region 选择库位
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
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Red)
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
                        str_post += this.dataGridView1.Rows[i].Cells[j].Value.ToString() + "-";
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
                    temp_jx += item + "_";
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
        #endregion

        #region 隐藏
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
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Red)
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
                    if (this.dataGridView1.Rows[i].Cells[j].Style.BackColor == Color.Red)
                    {
                        continue;
                    }
                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
            check_number();
        }
        #endregion

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
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
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


            this.Close();
        }
        #endregion 
        #endregion

        #region 双击批次库存信息
        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            DataTable dt = ex.Get_Data("[dbo].[hp_0915_cp_res_info_list_post_get_select] '" + id + "'");

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
                                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                                    break;
                                case "已排产":
                                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    break;
                                case "已领料":
                                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                                    break;
                                default:
                                    this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.DarkGray;
                                    break;

                            }

                        }
                    }

                }

            }

        }
        #endregion

        #region 加载今天 批次 和 今天收料片数
        /// <summary>
        /// 加载今天 批次 和 今天收料片数
        /// </summary>
        private void LoadDayNumber()
        {

            DataSet dst = ex.Get_Dset("[dbo].[hp_0915_cp_res_info_total_select]");
            this.labelX14.Text = dst.Tables[0].Rows[0][0].ToString();
            this.labelX15.Text = dst.Tables[1].Rows[0][0].ToString();

        } 
        #endregion

        #region 查询
        private void buttonX5_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string cus_code = this.pwtSearchBox4.Text;
            string lot = this.textBoxX1.Text;
            string mate_type = this.pwtSearchBox2.Text;
            string wms_code = this.pwtSearchBox3.Text;
            string res_order = this.textBoxX2.Text;


            string check = "";
            if (this.checkBoxX1.Checked == true)
            {
                check = "1";
            }
            else
            {
                check = "0";
            }

            string p_state = this.comboBoxEx1.SelectedItem.ToString();



            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");


            string sql_str = string.Format("[dbo].[hp_0915_cp_res_info_select_wms_lot_select]  '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'",
               cus_name, cus_code, lot, mate_type, wms_code, res_order, check, dat1, dat2, p_state);

            DataTable dt = ex.Get_Data(sql_str);
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

            this.pwtDataGridView1_MouseDoubleClick(null, null);

            //加载今天信息
            LoadDayNumber();
        }
        #endregion
        //===============================================
        #region 客户名称查询
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT] 'CP' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
            this.pwtSearchBox4.Text = mfrom.select_name[1];

        }
        #endregion

        #region 产品型号查询
        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            //待调整SQL
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_W_Wafer_Materials_information_Info_SELECT] ", new List<int> { 0, 1, 2 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[1];
            this.pwtSearchBox4.Text = mfrom.select_name[0];
            this.pwtSearchBox2.Text = mfrom.select_name[2];
        }
        #endregion

        #region 库位号查询
        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口(" [dbo].[HP_WARHOUSE_BASE_INFO_SELECT_NEWS0629] 'CP收料'", new List<int> { 1, 2 });
            mfrom.ShowDialog();
            if (mfrom.select_state == false)
            {
                return;
            }
            this.pwtSearchBox3.Text = mfrom.select_name[0];
        } 
        #endregion

        #region 清空
        private void buttonX6_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text = "";
            string cus_code = this.pwtSearchBox4.Text = "";
            string lot = this.textBoxX1.Text = "";
            string mate_type = this.pwtSearchBox2.Text = "";
            string wms_code = this.pwtSearchBox3.Text = "";
            string res_order = this.textBoxX2.Text = "";
            this.pwtDataGridView1.Columns.Clear();

            //初始化 位号信息
            LoadLotNo();
        } 
        #endregion


    }
}
