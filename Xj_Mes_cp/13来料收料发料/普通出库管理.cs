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
    public partial class 普通出库管理 : DockContent
    {
        public 普通出库管理()
        {
            InitializeComponent();
        }

        #region 空
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void labelX7_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        private void labelX9_Click(object sender, EventArgs e)
        {

        }

        private void textBoxX3_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelX16_Click(object sender, EventArgs e)
        {

        }

        private void textBoxX6_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelX18_Click(object sender, EventArgs e)
        {

        } 
        #endregion

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
        db_deal ex = new db_deal();
        #region 版本号查询--隐藏
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




            #region 加载晶圆基础信息
            string str_sql = string.Format("[dbo].[cp_hp_0707_W_Wafer_Materials_information_Info_select]   '{0}','{1}'", mate_type_new, mate_ves);
            DataTable dt = ex.Get_Data(str_sql);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("未查询到晶圆基础信息资料"); return;
            }
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView3);
            #endregion


            #region 加载程序信息


            string mate_id = dt.Rows[0]["序号"].ToString();
            //获取程序组信息
            DataTable process_dtb = ex.Get_Data("y_getmaterials_voltage_list '" + mate_id + "'");
            DtbToUi.DtbToDGV(process_dtb, this.pwtDataGridView4);

            #endregion



        } 
        #endregion

        private void 业务排产管理_Load(object sender, EventArgs e)
        {

        }

        #region GetCPCode
        private string GetCPCode(string name, string TitleNo)
        {

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(2, '0');

            return sturct + sturct_info;

        } 
        #endregion

        #region 普通出库
        private void buttonX3_Click(object sender, EventArgs e)
        {

            try
            {
                this.buttonX3.Enabled = false;

                string cus_name = this.pwtSearchBox1.Text;
                string cus_code = this.pwtSearchBox4.Text;
                if (cus_name == "")
                {
                    MessageBox.Show("缺少客户信息", "系统提示"); return;
                }
                string lot = this.pwtSearchBox3.Text;
                string mate_type = this.pwtSearchBox2.Text;
                string mate_ves = this.pwtSearchBox5.Text;


                //取消控制版本
                //if (mate_ves == "")
                //{
                //    MessageBox.Show("缺少版本信息", "系统提示"); return;
                //}



                string post = this.textBoxX3.Text;
                if (post == "")
                {
                    MessageBox.Show("缺少位号信息", "系统提示"); return;
                }
                string post_simple = this.textBoxX6.Text;
                string post_number = this.textBoxX4.Text;

                string dc = this.textBoxX1.Text;

                string weigong = this.textBoxX2.Text;
                string cihao = this.textBoxX7.Text;
                string epn = this.textBoxX8.Text;

                string demo_process = this.textBoxX11.Text;

                string lot_in = this.textBoxX9.Text;
                string lot_out = this.textBoxX10.Text;
                string remark = this.textBoxX5.Text;

                string info1 = this.textBoxX12.Text;
                string info2 = this.textBoxX13.Text;
                string info3 = this.textBoxX14.Text;

                string res_id = this.labelX26.Text;


                string tital_name = lot + "." + mate_ves + ".";
                string CP_ONLY_CODE = GetCPCode(tital_name, tital_name);

                string sql_str = string.Format("[dbo].[hp_0915_business_info_insert01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','普通出库'",
                    cus_name, cus_code, lot, mate_type, mate_ves, post, post_simple, post_number, dc, weigong, cihao, epn, lot_in, lot_out, demo_process, remark + "-普通出库操作", info1, info2, info3, base_info.user_code, res_id, CP_ONLY_CODE); ;
                DataTable dt = ex.Get_Data(sql_str);


                ex.Exe_Data("[dbo].[hp_cp_wms_out_number_info_insert] '" + CP_ONLY_CODE + "','" + post_number + "','" + post + "'");

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("系统通信错误"); return;
                }
                DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);

                string id = dt.Rows[0][0].ToString();

                foreach (var item in post.Split('、'))
                {
                    ex.Exe_Data("[dbo].[hp_0915_business_info_list_insert] '" + id + "','" + item + "','已排产','特殊出库','',''");
                    ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_state_update] '" + res_id + "','" + item + "','已排产'");
                }

                this.pwtDataGridView1_MouseDoubleClick(null, null);
                LoadDayNumber();
                MessageBox.Show("特殊出库登记成功", "系统提示");
            }
            finally
            {
                this.buttonX3.Enabled = true;

            }

        }
        #endregion

        #region 修改-隐藏
        private void buttonX4_Click(object sender, EventArgs e)
        {

        }

        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            string lot = pwtSearchBox3.Text;

            string cus_name = this.pwtSearchBox1.Text;
            string mate_type = this.pwtSearchBox2.Text;
            排产批次选择 mfrom = new 排产批次选择(cus_name, mate_type, lot);

            mfrom.ShowDialog();


            if (mfrom.select_ok != "0")
            {
                return;
            }

            this.textBoxX4.Text = mfrom.total_number;
            this.textBoxX3.Text = mfrom.total_point;
            this.textBoxX6.Text = mfrom.total_point_remark;

            this.pwtSearchBox1.Text = mfrom.cus_name;
            this.pwtSearchBox4.Text = mfrom.cus_code;
            this.pwtSearchBox2.Text = mfrom.select_mate_type;
            this.pwtSearchBox3.Text = mfrom.select_lot;

            this.labelX26.Text = mfrom.res_id;

        } 
        #endregion

        #region 双击排产订单信息显示
        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            #region 加载位号信息

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            DataTable dt_post = ex.Get_Data("[dbo].[hp_0915_business_info_list_select] '" + id + "'");
            DtbToUi.DtbToDGV(dt_post, this.pwtDataGridView2);

            #endregion


            string mate_type_new = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            string mate_ves = this.pwtDataGridView1.SelectedRows[0].Cells["版本"].Value.ToString();


            #region 加载晶圆基础信息
            //string str_sql = string.Format("[dbo].[cp_hp_0707_W_Wafer_Materials_information_Info_select]   '{0}','{1}'", mate_type_new, mate_ves);
            //DataTable dt = ex.Get_Data(str_sql);

            //if (dt.Rows.Count == 0)
            //{
            //    MessageBox.Show("未查询到晶圆基础信息资料"); return;
            //}
            //DtbToUi.DtbToDGV(dt, this.pwtDataGridView3);
            #endregion


            #region 加载程序信息


            //string mate_id = dt.Rows[0]["序号"].ToString();
            ////获取程序组信息
            //DataTable process_dtb = ex.Get_Data("y_getmaterials_voltage_list '" + mate_id + "'");
            //DtbToUi.DtbToDGV(process_dtb, this.pwtDataGridView4);

            #endregion




        } 
        #endregion

        #region 清空
        private void buttonX7_Click(object sender, EventArgs e)
        {
            this.labelX26.Text = "";

            string cus_name = this.pwtSearchBox1.Text = "";
            string cus_code = this.pwtSearchBox4.Text = "";

            string lot = this.pwtSearchBox3.Text = "";
            string mate_type = this.pwtSearchBox2.Text = "";
            string mate_ves = this.pwtSearchBox5.Text = "";


            string post = this.textBoxX3.Text = "";

            string post_simple = this.textBoxX6.Text = "";
            string post_number = this.textBoxX4.Text = "";

            string dc = this.textBoxX1.Text = "";

            string weigong = this.textBoxX2.Text = "";
            string cihao = this.textBoxX7.Text = "";
            string epn = this.textBoxX8.Text = "";

            string demo_process = this.textBoxX11.Text = "";

            string lot_in = this.textBoxX9.Text = "";
            string lot_out = this.textBoxX10.Text = "";
            string remark = this.textBoxX5.Text = "";

            string info1 = this.textBoxX12.Text = "";
            string info2 = this.textBoxX13.Text = "";
            string info3 = this.textBoxX14.Text = "";

            string res_id = this.labelX26.Text = "";


            this.pwtDataGridView1.Columns.Clear();
            this.pwtDataGridView2.Columns.Clear();
            this.pwtDataGridView3.Columns.Clear();
            this.pwtDataGridView4.Columns.Clear();
        }
        #endregion


        #region 加载今天 批次 和 今天收料片数
        public void LoadDayNumber()
        {

            DataSet dst = ex.Get_Dset("[dbo].[hp_0915_business_info_total_select]");

            this.labelX14.Text = dst.Tables[0].Rows[0][0].ToString();
            this.labelX15.Text = dst.Tables[1].Rows[0][0].ToString();


        } 
        #endregion

        #region 查询
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string cus_code = this.pwtSearchBox4.Text;
            string lot = this.pwtSearchBox3.Text;
            string mate_type = this.pwtSearchBox2.Text;
            string mate_ves = this.pwtSearchBox5.Text;
            string post = this.textBoxX3.Text;
            string post_simple = this.textBoxX6.Text;
            string post_number = this.textBoxX4.Text;

            string dc = this.textBoxX1.Text;

            string weigong = this.textBoxX2.Text;
            string cihao = this.textBoxX7.Text;
            string epn = this.textBoxX8.Text;

            string demo_process = this.textBoxX11.Text;

            string lot_in = this.textBoxX9.Text;
            string lot_out = this.textBoxX10.Text;
            string remark = this.textBoxX5.Text;

            string info1 = this.textBoxX12.Text;
            string info2 = this.textBoxX13.Text;
            string info3 = this.textBoxX14.Text;

            string res_id = this.labelX26.Text;

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

            string sql_str = string.Format("[dbo].[hp_0915_business_info_select01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','普通出库'",
                cus_name, cus_code, lot, mate_type, mate_ves, post, post_simple, post_number, dc, weigong, cihao, epn, lot_in, lot_out, demo_process, remark, info1, info2, info3, base_info.user_code, res_id, check, dat1, dat2); ;
            DataTable dt = ex.Get_Data(sql_str);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            LoadDayNumber();
            MessageBox.Show("查询成功", "系统提示");

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
                    return;
                }
                this.pwtDataGridView1_MouseDoubleClick(null, null);


                if (MessageBox.Show("确定删除选择的排产信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }


                string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

                DataTable dt_check = ex.Get_Data(" [dbo].[hp_0915_business_info_list_select]  '" + id + "'");

                for (int i = 0; i < dt_check.Rows.Count; i++)
                {

                    if (dt_check.Rows[i]["状态"].ToString() != "已排产")
                    {
                        MessageBox.Show("状态错误", "系统提示"); return;
                    }
                }


                string res_id = this.pwtDataGridView1.SelectedRows[0].Cells["收料序号"].Value.ToString();


                ex.Exe_Data("[dbo].[hp_0915_business_info_delete] '" + id + "'");


                for (int i = 0; i < this.pwtDataGridView2.Rows.Count; i++)
                {
                    string item = this.pwtDataGridView2.Rows[i].Cells["位号"].Value.ToString();
                    ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_state_update] '" + res_id + "','" + item + "','已入库'");
                }

                DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
                this.pwtDataGridView1_MouseDoubleClick(null, null);
                LoadDayNumber();

                MessageBox.Show("删除成功", "系统提示");

            }
            finally
            {
                this.buttonX5.Enabled = true;

            }

        }
        #endregion

        private void buttonX6_Click(object sender, EventArgs e)
        {

        }
    }
}
