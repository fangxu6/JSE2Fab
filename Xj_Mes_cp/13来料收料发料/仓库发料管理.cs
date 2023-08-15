using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using Pawote.UI.Controls;
using WeifenLuo.WinFormsUI.Docking;
namespace Xj_Mes_cp
{
    public partial class 仓库发料管理 : DockContent
    {
        public 仓库发料管理()
        {
            InitializeComponent();
        }
        private void 仓库发料管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
            LoadLotNo();
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

        #region 客户信息查询
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

        #region 晶圆型号查询
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

        #region 版本号查询
        private void pwtSearchBox5_SearchBtnClick(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string mate_type = this.pwtSearchBox2.Text;
            //待调整SQL
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_0915_W_Wafer_Materials_information_Info_get_mate_list_select] '" + cus_name + "','" + mate_type + "' ", new List<int> { 1, 3, 4, 5 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }
            string mate_type_new = this.pwtSearchBox2.Text = mfrom.select_name[0];
            string mate_ves = this.pwtSearchBox5.Text = mfrom.select_name[1];
            this.pwtSearchBox1.Text = mfrom.select_name[2];
            this.pwtSearchBox4.Text = mfrom.select_name[3];
        } 
        #endregion
        #region 取消发料
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("确定对选择的排产进行发料", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string state = this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value.ToString();
            if (state != "已发料")
            {
                MessageBox.Show("未发料,无需进行取消发料", "系统提示"); return;
            }
            ex.Exe_Data("[dbo].[hp_0915_business_info_send_state_update] '" + id + "','打印完成','" + base_info.user_code + "'");
            this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value = "打印完成";
            MessageBox.Show("取消发料成功", "系统提示");
        } 
        #endregion
        db_deal ex = new db_deal();
        #region 查询
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string cus_code = this.pwtSearchBox4.Text;
            string lot = this.pwtSearchBox3.Text;
            string mate_type = this.pwtSearchBox2.Text;
            string mate_ves = this.pwtSearchBox5.Text;
            string lot_only_code = this.pwtSearchBox6.Text;
            string check = "";
            if (this.checkBoxX1.Checked == true)
            {
                check = "1";
            }
            else
            {
                check = "0";
            }
            string state = this.comboBoxEx1.SelectedItem.ToString();
            if (state == "全部")
            {
                state = "";
            }
            else if (state == "待发料")
            {
                state = "创建";
            }
            else
            {
                state = "已发料";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string sql_str = string.Format(" [dbo].[hp_0915_business_info_wms_send_select]  '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'"
                , cus_name, cus_code, lot, mate_type, mate_ves, check, dat1, dat2, state, lot_only_code);
            DataTable dt = ex.Get_Data(sql_str);
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            this.pwtDataGridView1_MouseDoubleClick(null, null);
            MessageBox.Show("查询成功", "系统提示");
        }
        #endregion

        #region 双击待发料列表
        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LoadLotNo();
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            this.labelX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells["数量"].Value.ToString();
            string post_info = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["位号"].Value.ToString();
            this.labelX13.Text = this.pwtDataGridView1.SelectedRows[0].Cells["流程卡号"].Value.ToString();
            this.labelX15.Text = this.pwtDataGridView1.SelectedRows[0].Cells["库位号"].Value.ToString();

            foreach (var item in post_info.Split('、'))
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                    {
                        string item_name = this.dataGridView1.Rows[i].Cells[j].Value.ToString();

                        if (item == item_name)
                        {
                            this.dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Green;
                        }
                    }
                }
            }
        } 
        #endregion
        private void pwtSearchBox6_SearchBtnClick(object sender, EventArgs e)
        {
            buttonX2_Click(null, null);
        }

        #region 发料
        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("确定对选择的排产进行发料", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string state = this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value.ToString();

            if (state != "打印完成")
            {
                MessageBox.Show("流程卡未打印或已经发料,无需进行发料", "系统提示"); return;
            }
            ex.Exe_Data("[dbo].[hp_0915_business_info_send_state_update] '" + id + "','已发料','" + base_info.user_code + "'");
            this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value = "已发料";
            MessageBox.Show("发料成功", "系统提示");
        }
        #endregion

        #region 清空
        private void buttonX4_Click(object sender, EventArgs e)
        {
            Clear(tableLayoutPanel1);
            Clear(tableLayoutPanel2);
            this.pwtDataGridView1.Rows.Clear();
            this.dataGridView1.Rows.Clear();
        }
        protected void Clear(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is PwtSearchBox)
                {
                    ((PwtSearchBox)(c)).Text = "";
                }
                else if (c is TextBoxX)
                {
                    ((TextBoxX)(c)).Text = "";
                }
                else if (c is ComboBoxEx)
                {
                    ((ComboBoxEx)(c)).SelectedIndex = 0;
                }
            }
        }
        #endregion
    }
}
