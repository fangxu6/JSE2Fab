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
    public partial class 仓库收料管理 : DockContent
    {
        public 仓库收料管理()
        {
            InitializeComponent();
        }
        #region 位号查询
        /// <summary>
        /// 位号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {



            选择信息窗口 mfrom = new 选择信息窗口(" [dbo].[HP_WARHOUSE_BASE_INFO_SELECT_NEWS0629] 'CP收料'", new List<int> { 1, 2 });
            mfrom.ShowDialog();
            if (mfrom.select_state == false)
            {
                return;
            }
            this.pwtSearchBox3.Text = mfrom.select_name[1];
        } 
        #endregion

        #region 客户信息查询
        /// <summary>
        /// 客户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region 片号选择

        /// <summary>
        /// 位号选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            Lot数量选择 lotShow = new Lot数量选择();
            lotShow.ShowDialog();
            if (lotShow.select_ok == "1")
            {
                return;
            }
            List<int> lot = lotShow.str_no;
            string total_number = lotShow.total_number;
            string total_point = lotShow.total_point;
            string total_point_remark = lotShow.total_point_remark;
            this.textBoxX3.Text = total_point;
            this.textBoxX4.Text = total_number;
            this.textBoxX6.Text = total_point_remark;
        } 
        #endregion

        #region GetCPCode
        private string GetCPCode(string name, string TitleNo)
        {
            db_deal ex = new db_deal();

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo + DateTime.Now.ToString("yyyyMMddHHmm").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(2, '0');

            return sturct + sturct_info;

        } 
        #endregion

        #region 收料
        private void buttonX3_Click(object sender, EventArgs e)
        {



            try
            {
                this.buttonX3.Enabled = false;
                string cus_name = this.pwtSearchBox1.Text.Trim();
                string cus_code = this.pwtSearchBox4.Text.Trim();
                string mate_type = this.pwtSearchBox2.Text.Trim();

                string lot = this.textBoxX1.Text.Trim();

                string res_date = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");
                string res_order = this.textBoxX2.Text.Trim();

                string wms_code = this.pwtSearchBox3.Text.Trim();

                string post_info = this.textBoxX3.Text.Trim();
                string post_simple = this.textBoxX6.Text.Trim();
                string cp_number = this.textBoxX4.Text.Trim();

                string remark = this.textBoxX5.Text.Trim();





                if (cus_name == "")
                {
                    MessageBox.Show("请输入客户信息", "系统提示"); return;
                }
                if (lot == "")
                {
                    MessageBox.Show("请输入批次信息", "系统提示"); return;
                }
                if (wms_code == "")
                {
                    MessageBox.Show("请输入库位信息", "系统提示"); return;
                }

                if (post_info == "")
                {
                    MessageBox.Show("请输入CP位号信息", "系统提示"); return;
                }
                if (mate_type == "")
                {
                    MessageBox.Show("请输入晶圆信息", "系统提示"); return;
                }



                // 验证  是否已经存在
                DataTable mate_check = ex.Get_Data("[dbo].[W_Wafer_Materials_information_Info_check_list_select]  '" + cus_name + "','" + cus_code + "','" + mate_type + "'");






                #region 判断相同批次和型号 的位号已经收料

                //
                DataTable dt_check = ex.Get_Data("[dbo].[hp_0915_cp_res_info_list_post_check_select] '" + lot + "','" + mate_type + "'");

                string temp_check_no = "";
                for (int i = 0; i < dt_check.Rows.Count; i++)
                {
                    string cp_no = dt_check.Rows[i]["位号编码"].ToString();

                    foreach (var item in post_info.Split('、'))
                    {
                        if (item == cp_no)
                        {
                            temp_check_no += "位号：" + item + Environment.NewLine;
                        }
                    }

                }

                if (temp_check_no != "")
                {
                    MessageBox.Show(temp_check_no + "当前批次和型号对应的位号已经收料", "系统提示"); return;
                }
                #endregion


                string info1 = "";
                string info2 = "";
                string info3 = GetCPCode("CP收料唯一码" + DateTime.Now.ToString("yyyy-MM-dd"), "CPT-"); ;



                // 写入数据添加库位修改
                string sql_str = string.Format("[dbo].[hp_0915_cp_res_info_insert]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'"
                    , cus_name, cus_code, lot, mate_type, res_date, res_order, wms_code, post_info, cp_number
                    , post_simple, remark, info1, info2, info3, base_info.user_code);


                DataTable dt = ex.Get_Data(sql_str);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("数据写入失败"); return;
                }
                string lot_id = dt.Rows[0]["序号"].ToString();
                pwtProgressBar1.Visible = true;
                int post_no_show = 0;
                Application.DoEvents();
                foreach (var item in post_info.Split('、'))
                {
                    pwtProgressBar1.ValueNumber = (post_no_show / post_info.Split('、').Length);
                    ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_insert] '" + lot_id + "','" + item + "','已入库','','','" + info3 + "'");
                    post_no_show++;
                }
                pwtProgressBar1.Visible = false;
                DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
                pwtDataGridView1_MouseDoubleClick(null, null);
                MessageBox.Show("收料成功", "系统提示");
            }
            finally
            {
                this.buttonX3.Enabled = true;
                LoadDayNumber();
            }

        } 
        #endregion

        db_deal ex = new db_deal();

        #region 收料信息双击

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            #region 位号信息
            DataTable dt = ex.Get_Data("[dbo].[hp_0915_cp_res_info_list_select] '" + id + "'");
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);


            if (this.pwtDataGridView2.Columns.Contains("INFO1"))
            {
                this.pwtDataGridView2.Columns["INFO1"].Visible = false;
            }
            if (this.pwtDataGridView2.Columns.Contains("INFO2"))
            {
                this.pwtDataGridView2.Columns["INFO2"].Visible = false;
            }
            if (this.pwtDataGridView2.Columns.Contains("INFO3"))
            {
                this.pwtDataGridView2.Columns["INFO3"].Visible = false;
            }

            #endregion

            #region 输出信息


            string cus_name = this.pwtSearchBox1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string cus_code = this.pwtSearchBox4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户代码"].Value.ToString();
            string mate_type = this.pwtSearchBox2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();

            string lot = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["LOT"].Value.ToString();

            //  string res_date = 
            this.dateTimePicker3.Value = DateTime.Parse(this.pwtDataGridView1.SelectedRows[0].Cells["收料日期"].Value.ToString());


            string res_order = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["收料单号"].Value.ToString();

            string wms_code = this.pwtSearchBox3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["库位号"].Value.ToString();

            string post_info = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["位号信息"].Value.ToString();
            string post_simple = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["简称"].Value.ToString();
            string cp_number = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["数量"].Value.ToString();

            string remark = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();


            #endregion
        } 
        #endregion

        #region 删除
        private void buttonX5_Click(object sender, EventArgs e)
        {


            try
            {
                this.buttonX5.Enabled = false;
                if (this.pwtDataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择需要删除的收料信息", "系统提示"); return;
                }


                if (MessageBox.Show("确定删除选择的收料信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

                //判断位号是否已经存在
                //删除收料信息，删除位号信息
                ex.Exe_Data(string.Format("[dbo].[hp_0915_cp_res_info_delete] '{0}','{1}'", id, base_info.user_code));
                DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
                this.pwtDataGridView1_MouseDoubleClick(null, null);

                MessageBox.Show("删除成功", "系统提示");
            }
            finally
            {
                this.buttonX5.Enabled = true;
                LoadDayNumber();
            }




        }
        #endregion

        #region 晶圆型号查询
        /// <summary>
        /// 晶圆型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region 加载今天 批次 和 今天收料片数 LoadDayNumber
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
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string cus_code = this.pwtSearchBox4.Text;
            string mate_type = this.pwtSearchBox2.Text;

            string lot = this.textBoxX1.Text;

            string res_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string res_order = this.textBoxX2.Text;

            string wms_code = this.pwtSearchBox3.Text;

            string post_info = this.textBoxX3.Text;
            string post_simple = this.textBoxX6.Text;
            string cp_number = this.textBoxX4.Text;

            string remark = this.textBoxX5.Text;


            string info1 = "";
            string info2 = "";
            string info3 = "";


            string check = "";
            if (this.checkBoxX1.Checked == true)
            {
                check = "1";
            }
            else
            {
                check = "0";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");


            string sql_str = string.Format("[dbo].[hp_0915_cp_res_info_select]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'"
                , cus_name, cus_code, lot, mate_type, res_date, res_order, wms_code, post_info, cp_number
                , post_simple, remark, info1, info2, info3, base_info.user_code, check, dat1, dat2);


            DataTable dt = ex.Get_Data(sql_str);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            this.pwtDataGridView1_MouseDoubleClick(null, null);
            LoadDayNumber();
        }
        #endregion

        #region 修改
        private void buttonX4_Click(object sender, EventArgs e)
        {

            try
            {

                this.buttonX4.Enabled = false;
                if (this.pwtDataGridView1.SelectedRows.Count == 0)
                {
                    return;
                }

                if (MessageBox.Show("确定修改选择的收料信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }


                string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


                string cus_name = this.pwtSearchBox1.Text;
                string cus_code = this.pwtSearchBox4.Text;
                string mate_type = this.pwtSearchBox2.Text;

                string lot = this.textBoxX1.Text;

                string res_date = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");
                string res_order = this.textBoxX2.Text;

                string wms_code = this.pwtSearchBox3.Text;

                string post_info = this.textBoxX3.Text;
                string post_simple = this.textBoxX6.Text;
                string cp_number = this.textBoxX4.Text;

                string remark = this.textBoxX5.Text;


                string info1 = "";
                string info2 = "";
                string info3 = "";


                if (cus_name == "")
                {
                    MessageBox.Show("请输入客户信息", "系统提示"); return;
                }
                if (lot == "")
                {
                    MessageBox.Show("请输入批次信息", "系统提示"); return;
                }
                if (wms_code == "")
                {
                    MessageBox.Show("请输入库位信息", "系统提示"); return;
                }

                if (post_info == "")
                {
                    MessageBox.Show("请输入CP位号信息", "系统提示"); return;


                }
                #region 判断相同批次和型号 的位号已经收料

                ////2022-05-12 去除 同 批次 型号 限制修改条件  
                ////
                //DataTable dt_check = ex.Get_Data("[dbo].[hp_0915_cp_res_info_list_post_check_select] '" + lot + "','" + mate_type + "'");

                //string temp_check_no = "";
                //for (int i = 0; i < dt_check.Rows.Count; i++)
                //{
                //    string cp_no = dt_check.Rows[i]["位号编码"].ToString();
                //    string cp_no_state = dt_check.Rows[i]["状态"].ToString();


                //    if (cp_no_state != "已入库")
                //    {
                //        temp_check_no += "位号：" + cp_no + " 状态：" + cp_no_state + Environment.NewLine;
                //    }
                //}

                //if (temp_check_no != "")
                //{
                //    MessageBox.Show(temp_check_no + "当前批次和型号对应的位号状态错误\r\n无法进行修改", "系统提示"); return;
                //}
                #endregion




                string sql_str = string.Format("[dbo].[hp_0915_cp_res_info_update]   '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}'"
                    , id, cus_name, cus_code, lot, mate_type, res_date, res_order, wms_code, post_info, cp_number
                    , post_simple, remark, info1, info2, info3, base_info.user_code);


                DataTable dt = ex.Get_Data(sql_str);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("数据写入失败", "系统提示"); return;
                }

                ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_delete] '" + id + "'");

                pwtProgressBar1.Visible = true;
                int post_no_show = 0;
                Application.DoEvents();
                foreach (var item in post_info.Split('、'))
                {
                    pwtProgressBar1.ValueNumber = (post_no_show / post_info.Split('、').Length);
                    ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_insert] '" + id + "','" + item + "','已入库','','',''");
                    post_no_show++;
                }
                pwtProgressBar1.Visible = false;
                DtbToUi.DtbUpdateToDGV(dt, this.pwtDataGridView1);
                pwtDataGridView1_MouseDoubleClick(null, null);
                MessageBox.Show("修改成功", "系统提示");
            }
            finally
            {
                this.buttonX4.Enabled = true;
                LoadDayNumber();
            }


        }
        #endregion

        #region 清空

        private void buttonX6_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text = "";
            string cus_code = this.pwtSearchBox4.Text = "";
            string mate_type = this.pwtSearchBox2.Text = "";

            string lot = this.textBoxX1.Text = "";

            this.dateTimePicker1.Value = DateTime.Now;
            string res_order = this.textBoxX2.Text = "";

            string wms_code = this.pwtSearchBox3.Text = "";

            string post_info = this.textBoxX3.Text = "";
            string post_simple = this.textBoxX6.Text = "";
            string cp_number = this.textBoxX4.Text = "";

            string remark = this.textBoxX5.Text = "";


            this.pwtDataGridView1.Columns.Clear();
            this.pwtDataGridView2.Columns.Clear();
        }
        #endregion

        #region 空
        private void 仓库收料管理_Load(object sender, EventArgs e)
        {

        }

        private void superTabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region 右击库位信息-冻结
        private void 冻结ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                return;
            }



            for (int i = 0; i < this.pwtDataGridView2.SelectedRows.Count; i++)
            {
                string state = this.pwtDataGridView2.SelectedRows[0].Cells["状态"].Value.ToString();
                if (state != "已入库")
                {
                    MessageBox.Show("状态错误", "系统提示"); return;
                }
            }

            if (MessageBox.Show("确定冻结选择的片号信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;


            }
            for (int i = 0; i < this.pwtDataGridView2.SelectedRows.Count; i++)
            {
                string id = this.pwtDataGridView2.SelectedRows[i].Cells["序号"].Value.ToString();

                ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_dongjie_state_update] '" + id + "','冻结','" + base_info.user_code + "','','',''");
                this.pwtDataGridView2.SelectedRows[i].Cells["状态"].Value = "冻结";
            }

            MessageBox.Show("冻结成功", "系统提示");

        }
        #endregion

        #region 右击库位信息-解冻
        private void 解冻ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.pwtDataGridView2.SelectedRows.Count; i++)
            {
                string state = this.pwtDataGridView2.SelectedRows[i].Cells["状态"].Value.ToString();
                if (state != "冻结")
                {
                    MessageBox.Show("状态错误", "系统提示"); return;
                }
            }

            if (MessageBox.Show("确定冻结选择的片号信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            for (int i = 0; i < this.pwtDataGridView2.SelectedRows.Count; i++)
            {
                string id = this.pwtDataGridView2.SelectedRows[i].Cells["序号"].Value.ToString();
                ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_dongjie_state_update] '" + id + "','已入库','" + base_info.user_code + "','','',''");
                this.pwtDataGridView2.SelectedRows[i].Cells["状态"].Value = "已入库";
            }
            MessageBox.Show("解锁冻结成功", "系统提示");
        } 
        #endregion
    }
}
