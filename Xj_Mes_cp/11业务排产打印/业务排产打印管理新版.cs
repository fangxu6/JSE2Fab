using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class 业务排产打印管理新版 : DockContent
    {
        public 业务排产打印管理新版()
        {
            InitializeComponent();
        }

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

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {

            string cus_name = this.pwtSearchBox1.Text;
            string cus_code = this.pwtSearchBox4.Text;

            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT01] 'CP','" + cus_name + "','" + cus_code + "' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
            this.pwtSearchBox4.Text = mfrom.select_name[1];
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string mate_name = this.pwtSearchBox2.Text;
            //待调整SQL
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_W_Wafer_Materials_information_Info_SELECT01] '" + mate_name + "','" + cus_name + "' ", new List<int> { 0, 1, 2 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[1];
            this.pwtSearchBox4.Text = mfrom.select_name[0];
            this.pwtSearchBox2.Text = mfrom.select_name[2];
        }
        db_deal ex = new db_deal();
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

        private void 业务排产管理_Load(object sender, EventArgs e)
        {
            
        }

        private string GetCPCode(string name, string TitleNo)
        {

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(2, '0');

            return sturct + sturct_info;

        }

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

                if (mate_ves == "")
                {
                    MessageBox.Show("缺少版本信息", "系统提示"); return;
                }
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

                string sql_str = string.Format("[dbo].[hp_0915_business_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}'",
                    cus_name, cus_code, lot, mate_type, mate_ves, post, post_simple, post_number, dc, weigong, cihao, epn, lot_in, lot_out, demo_process, remark, info1, info2, info3, base_info.user_code, res_id, CP_ONLY_CODE); ;
                DataTable dt = ex.Get_Data(sql_str);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("系统通信错误"); return;
                }
                DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);

                string id = dt.Rows[0][0].ToString();

                foreach (var item in post.Split('-'))
                {
                    ex.Exe_Data("[dbo].[hp_0915_business_info_list_insert] '" + id + "','" + item + "','已排产','','',''");
                    ex.Exe_Data("[dbo].[hp_0915_cp_res_info_list_state_update] '" + res_id + "','" + item + "','已排产'");
                }

                this.pwtDataGridView1_MouseDoubleClick(null, null);
                LoadDayNumber();
                MessageBox.Show("排产成功", "系统提示");
            }
            finally
            {
                this.buttonX3.Enabled = true;

            }

        }

        private void buttonX4_Click(object sender, EventArgs e)
        {

        }

        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            //string lot = pwtSearchBox3.Text;

            //string cus_name = this.pwtSearchBox1.Text;
            //string mate_type = this.pwtSearchBox2.Text;
            //排产批次选择 mfrom = new 排产批次选择(cus_name, mate_type, lot);

            //mfrom.ShowDialog();


            //if (mfrom.select_ok != "0")
            //{
            //    return;
            //}

            //this.textBoxX4.Text = mfrom.total_number;
            //this.textBoxX3.Text = mfrom.total_point;
            //this.textBoxX6.Text = mfrom.total_point_remark;

            //this.pwtSearchBox1.Text = mfrom.cus_name;
            //this.pwtSearchBox4.Text = mfrom.cus_code;
            //this.pwtSearchBox2.Text = mfrom.select_mate_type;
            //this.pwtSearchBox3.Text = mfrom.select_lot;

            //this.labelX26.Text = mfrom.res_id;

        }

        #region 单击显示排产订单信息
        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            this.pwtDataGridView2.Rows.Clear();
            this.pwtDataGridView3.Rows.Clear();
            this.pwtDataGridView4.Rows.Clear();

            #region 加载位号信息

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            DataTable dt_post = ex.Get_Data("[dbo].[hp_0915_business_info_list_select] '" + id + "'");
            DtbToUi.DtbToDGV(dt_post, this.pwtDataGridView2);

            #endregion


            string mate_type_new = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            string mate_ves = this.pwtDataGridView1.SelectedRows[0].Cells["版本"].Value.ToString();


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


        public void LoadDayNumber()
        {

            DataSet dst = ex.Get_Dset("[dbo].[hp_0915_business_info_total_select]");

            this.labelX14.Text = dst.Tables[0].Rows[0][0].ToString();
            this.labelX15.Text = dst.Tables[1].Rows[0][0].ToString();


        }
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

            string sql_str = string.Format("[dbo].[hp_0915_business_info_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}'",
                cus_name, cus_code, lot, mate_type, mate_ves, post, post_simple, post_number, dc, weigong, cihao, epn, lot_in, lot_out, demo_process, remark, info1, info2, info3, base_info.user_code, res_id, check, dat1, dat2); ;





            DataTable dt = ex.Get_Data(sql_str);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            LoadDayNumber();
            MessageBox.Show("查询成功", "系统提示");

        } 
        #endregion

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


        #region 打印
        private void buttonX1_Click_1(object sender, EventArgs e)
        {
            try
            {
                buttonX1.Enabled = false;


                //   this.pwtDataGridView1_MouseDoubleClick(null, null);


                if (this.pwtDataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("未选中排产订单!", "系统提示");

                    return;
                }

                if (this.pwtDataGridView2.Rows.Count == 0)
                {
                    MessageBox.Show("请先双击选择需要打印的流程卡", "系统提示"); return;
                }

                if (this.pwtDataGridView3.Rows.Count == 0)
                {
                    MessageBox.Show("获取物料信息组失败", "系统提示"); return;
                }

                if (this.pwtDataGridView4.Rows.Count == 0)
                {
                    MessageBox.Show("获取物料流程组失败", "系统提示"); return;
                }

                #region 基础信息
                string Produc_Status = this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value.ToString();


                string only_lot_code = this.pwtDataGridView1.SelectedRows[0].Cells["流程卡号"].Value.ToString();

                string IID_LOT = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();



                string Material_Name = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
                string Material_Version = this.pwtDataGridView1.SelectedRows[0].Cells["版本"].Value.ToString();
                string Post_jc = this.pwtDataGridView1.SelectedRows[0].Cells["位号简称"].Value.ToString();

                string lot_no_out = this.pwtDataGridView1.SelectedRows[0].Cells["LOT_OUT"].Value.ToString();


                string cus_name = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
                string cus_code = this.pwtDataGridView1.SelectedRows[0].Cells["客户代码"].Value.ToString();
                string 是否打点 = this.pwtDataGridView1.SelectedRows[0].Cells["是否打点"].Value.ToString();

                string mateID = this.pwtDataGridView3.Rows[0].Cells["序号"].Value.ToString();



                DataTable dt = ex.Get_Data("y_materials_info_select '" + mateID + "'");



                //晶圆尺寸
                string Material_Size = dt.Rows[0]["Material_Size"].ToString();
                //针卡名称
                string Needlecard_Name = dt.Rows[0]["Needlecard_Name"].ToString();
                //单片良率
                string Yield_indicators_Chip = dt.Rows[0]["Yield_indicators_Chip"].ToString();
                //整批良率
                string Yield_indicators_Batch = dt.Rows[0]["Yield_indicators_Batch"].ToString();
                //中测台程式
                string Measured = dt.Rows[0]["Measured"].ToString();
                //测试版
                string Test_Version = dt.Rows[0]["Test_Version"].ToString();
                //厂内批号    名称+电压版本
                string Produc_Name = Material_Name + " " + Material_Version;
                //客户批次
                string Lot_No = this.pwtDataGridView1.SelectedRows[0].Cells["LOT"].Value.ToString();
                //晶片数量
                string Material_Qty = this.pwtDataGridView1.SelectedRows[0].Cells["数量"].Value.ToString();




                //排产编号  二维码
                string Produc_OnlyCode = this.pwtDataGridView1.SelectedRows[0].Cells["流程卡号"].Value.ToString();

                //厂内批号
                string Produc_Order = this.pwtDataGridView1.SelectedRows[0].Cells["LOT"].Value.ToString();
                //注意事项
                string Produc_Remark = 是否打点 + "," + this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();

                //测试机型
                string test_eq = dt.Rows[0]["Info1"].ToString();

                //工程注意
                string gc_remark = dt.Rows[0]["Info2"].ToString();
                #endregion


                if (MessageBox.Show(string.Format("确定打印批次：{0}\r\n型号：{1}\r\n版本:{2}\r\n数量:{3}\r\n位号:{4}", Produc_Order, Material_Name, Material_Version, Material_Qty, Post_jc), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                {

                    MessageBox.Show("打印取消", "系统提示");
                    return;
                }
                #region 流程卡表单打印




                List<string> print_file = new List<string>();


                string FIlePath = Application.StartupPath + @"\2_btw\单道测试流程单.btw";




                List<string> wif_list = new List<string>();




                string TableID = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


                //获取位号
                DataTable dtb = ex.Get_Data("[dbo].[hp_0915_business_info_list_print_post_select] '" + TableID + "'");
                if (dtb.Rows.Count == 0)
                {
                    MessageBox.Show("未查询到排产位号");
                    return;
                }
                //记录位号信息
                for (int i = 0; i < dtb.Rows.Count; i++)
                {
                    wif_list.Add(dtb.Rows[i]["晶圆刻号"].ToString());
                }

                string Audit_Operator = base_info.user_code;


                //  format.SubStrings["a01"].Value = total_table_info;

                Engine engine = new Engine(true);
                string mb = FIlePath;
                LabelFormatDocument format = engine.Documents.Open(mb);


                //==================  打印机选择 ==================
                选择打印机 print_show = new 选择打印机();
                print_show.ShowDialog();
                if (print_show.select_state == false)
                {
                    return;
                }

                format.PrintSetup.PrinterName = print_show.DefaultPrintMac.ToString();
                //==================  打印机选择 ==================


                format.SubStrings["Material_Size"].Value = Material_Size;
                //format.SubStrings["Needlecard_Name"].Value = Needlecard_Name;
                //format.SubStrings["Measured"].Value = Measured;
                format.SubStrings["Test_Version"].Value = Test_Version;
                format.SubStrings["Produc_Name"].Value = Produc_Name;
                //  format.SubStrings["Lot_No"].Value = Lot_No;
                format.SubStrings["Material_Qty"].Value = Material_Qty;
                format.SubStrings["Produc_OnlyCode"].Value = Produc_OnlyCode;
                format.SubStrings["Material_Name"].Value = Material_Name;
                format.SubStrings["Produc_Name"].Value = Produc_Name;
                format.SubStrings["Produc_Order"].Value = Produc_Order;
                format.SubStrings["Produc_Remark"].Value = Produc_Remark;
                format.SubStrings["Audit_Operator"].Value = Audit_Operator;
                format.SubStrings["post_jc"].Value = Post_jc;
                format.SubStrings["mate_ves"].Value = Material_Version;

                format.SubStrings["lot_no_out"].Value = lot_no_out;
                format.SubStrings["cus_code"].Value = cus_code;
                format.SubStrings["cus_name"].Value = cus_name;
                //=====================================
                //测试机型
                format.SubStrings["csjx"].Value = test_eq;

                //清针频率
                format.SubStrings["qzpv"].Value = "待定";
                //
                format.SubStrings["gcremark"].Value = gc_remark;
                #endregion



                #region 打印BTW 流程卡

                format.SubStrings["cp1how"].Value = "0";
                format.SubStrings["cp1show"].Value = "0";
                format.SubStrings["cp2how"].Value = "0";
                format.SubStrings["cp2show"].Value = "0";
                format.SubStrings["cp3how"].Value = "0";
                format.SubStrings["cp3show"].Value = "0";

                format.SubStrings["lasercheck"].Value = "0";

                //初始化 

                format.SubStrings["lasercheck_LASER0"].Value = "0";
                format.SubStrings["lasercheck_LASER1"].Value = "0";
                format.SubStrings["lasercheck_LASER2"].Value = "0";





                string total_prcess = "开始" + " --> ";


                int check_cp_number = 0;
                string cptotal_info = "";
                for (int i = 0; i < this.pwtDataGridView4.Rows.Count; i++)
                {


                    string process_name_s = this.pwtDataGridView4.Rows[i].Cells[1].Value.ToString();

                    //if (process_name_s=="TargetDie")
                    //{
                    //    continue;
                    //}
                    if (process_name_s.ToUpper() == "CP1" || process_name_s.ToUpper() == "CP1S" || process_name_s.ToUpper() == "CP2S" || process_name_s.ToUpper() == "CP3S" || process_name_s.ToUpper() == "CP2" || process_name_s.ToUpper() == "CP3")
                    {
                        check_cp_number++;
                        cptotal_info += process_name_s + ",";
                    }
                }





                for (int i = 0; i < this.pwtDataGridView4.Rows.Count; i++)
                {
                    string process_name_s = this.pwtDataGridView4.Rows[i].Cells[1].Value.ToString();
                    total_prcess += process_name_s + "->";


                    DataTable dt_cp = ex.Get_Data("[dbo].[hp_1014_cp_mate_process_basic_info_select] '" + mateID + "','" + process_name_s + "'");
                    if (dt_cp.Rows.Count == 0)
                    {
                        continue;
                    }

                    switch (process_name_s)
                    {
                        case "CP1":
                        case "CP1S":
                        case "CP2":
                        case "CP2S":
                        case "CP3":
                        case "CP3S":

                            //LotNoCP1
                            //LotNoCP1S
                            //LotNoCP2
                            //LotNoCP2S
                            //LotNoCP3
                            //LotNoCP3S

                            //2021-11-1 挂批次 +  流程
                            //Anson要求


                            //2021-11-21 添加逻辑条件 只有CP1 的情况下,不显示CP1 其他清空显示CP1
                            if (check_cp_number == 1)
                            {
                                format.SubStrings["LotNo" + process_name_s].Value = Lot_No;
                            }
                            else
                            {
                                format.SubStrings["LotNo" + process_name_s].Value = Lot_No + process_name_s.Substring(0, 3);
                            }



                            format.SubStrings[process_name_s.ToLower() + "how"].Value = "1";//是否有流程 1有 0无

                            if (dt_cp.Rows[0]["Name"].ToString() == "")
                            {
                                format.SubStrings["test_program" + process_name_s.ToLower()].Value = "NA";//程序
                            }
                            else
                            {
                                format.SubStrings["test_program" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["Name"].ToString();//程序
                            }

                            //程序信息

                            if (dt_cp.Rows[0]["Name"].ToString().Contains(':'))
                            {
                                format.SubStrings["procode" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["Name"].ToString().Split(':')[1];//程序
                            }
                            else if (dt_cp.Rows[0]["Name"].ToString().Contains('：'))
                            {
                                format.SubStrings["procode" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["Name"].ToString().Split('：')[1];//程序
                            }
                            else
                            {
                                if (dt_cp.Rows[0]["Name"].ToString() == "")
                                {
                                    format.SubStrings["procode" + process_name_s.ToLower()].Value = "NA";//程序
                                }
                                else
                                {
                                    format.SubStrings["procode" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["Name"].ToString();//程序
                                }

                            }

                            //中测台程序1
                            if (dt_cp.Rows[0]["info14"].ToString().Contains(':'))
                            {
                                format.SubStrings["Measured" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["info14"].ToString().Split(':')[1];//程序
                            }
                            else if (dt_cp.Rows[0]["info14"].ToString().Contains('：'))
                            {
                                format.SubStrings["Measured" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["info14"].ToString().Split('：')[1];//程序
                            }
                            else
                            {
                                if (dt_cp.Rows[0]["info14"].ToString() == "")
                                {
                                    if (dt.Rows[0]["Measured"].ToString() == "")
                                    {
                                        format.SubStrings["Measured" + process_name_s.ToLower()].Value = "NA";//程序
                                    }
                                    else
                                    {
                                        format.SubStrings["Measured" + process_name_s.ToLower()].Value = dt.Rows[0]["Measured"].ToString();
                                    }
                                }
                                else
                                {
                                    format.SubStrings["Measured" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["info14"].ToString();//程序
                                }

                            }

                            //中测台程序2
                            if (dt_cp.Rows[0]["info15"].ToString().Contains(':'))
                            {
                                format.SubStrings["Measured2" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["info15"].ToString().Split(':')[1];//程序
                            }
                            else if (dt_cp.Rows[0]["info15"].ToString().Contains('：'))
                            {
                                format.SubStrings["Measured2" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["info15"].ToString().Split('：')[1];//程序
                            }
                            else
                            {
                                if (dt_cp.Rows[0]["info15"].ToString() == "")
                                {
                                    if (dt.Rows[0]["Measured"].ToString()=="")
                                    {
                                        format.SubStrings["Measured2" + process_name_s.ToLower()].Value = "";//程序
                                    }
                                    else
                                    {
                                        format.SubStrings["Measured2" + process_name_s.ToLower()].Value = "";
                                    }
                                }
                                else
                                {
                                    format.SubStrings["Measured2" + process_name_s.ToLower()].Value = "/"+dt_cp.Rows[0]["info15"].ToString();//程序
                                }

                            }

                            //针卡信息
                            if (dt_cp.Rows[0]["info16"].ToString() == "")
                            {
                                if (dt.Rows[0]["Needlecard_Name"].ToString() == "")
                                {
                                    format.SubStrings["Needlecard_Name" + process_name_s.ToLower()].Value = "NA";//程序
                                }
                                else
                                {
                                    format.SubStrings["Needlecard_Name" + process_name_s.ToLower()].Value = dt.Rows[0]["Needlecard_Name"].ToString();
                                }
                            }
                            else
                            {
                                format.SubStrings["Needlecard_Name" + process_name_s.ToLower()].Value = dt_cp.Rows[0]["info16"].ToString();//程序
                            }


                            format.SubStrings[process_name_s.ToLower() + "yield"].Value = string.Format("单片:{0} {2} 整批:{1} {3} ", dt_cp.Rows[0]["info10"].ToString(), dt_cp.Rows[0]["info11"].ToString(), dt_cp.Rows[0]["info12"].ToString(), dt_cp.Rows[0]["info13"].ToString());//良率指标
                            if (check_cp_number == 1)
                            {
                                format.SubStrings["test_program_table" + process_name_s.ToLower()].Value = CP_PrintHelper.PrintTableClass(this.pwtDataGridView2, Lot_No, "");//表格输出
                            }
                            else
                            {
                                //客户要求  TSK  数据转换
                                format.SubStrings["test_program_table" + process_name_s.ToLower()].Value = CP_PrintHelper.PrintTableClass(this.pwtDataGridView2, Lot_No, process_name_s.Substring(0, 3));//表格输出

                            }
                            break;

                        case "LASER":



                            List<string> laser_pro = new List<string>();
                            laser_pro.Add(dt_cp.Rows[0]["Name"].ToString());
                            laser_pro.Add(dt_cp.Rows[0]["info10"].ToString());
                            laser_pro.Add(dt_cp.Rows[0]["info11"].ToString());

                            laser_pro.Add(dt_cp.Rows[0]["info12"].ToString());//取消
                            laser_pro.Add(dt_cp.Rows[0]["info13"].ToString());//取消
                            laser_pro.Add(dt_cp.Rows[0]["info14"].ToString());//取消
                            string table_laser = "";

                            //  string table_laser = CreateTable(laser_pro, wif_list, "");
                            //  区分 LASER  CP0 ,CP1,CP2,CP3
                            //2022-09-23
                            List<string> wif_list_LASER = new List<string>();
                            for (int wi = 0; wi < wif_list.Count; wi++)
                            {
                                wif_list_LASER.Add(wif_list[wi].ToString() + "-CP0");
                            }
                            table_laser = CreateTable(laser_pro, wif_list_LASER, "");
                            //获取 镭射 程序列表




                            format.SubStrings["lasercheck"].Value = "1";
                            string basic_table = ReadTxt("basic_table");
                            format.SubStrings["lasertable"].Value = string.Format(basic_table, table_laser);
                            break;

                        case "LASER0":
                        case "LASER1":
                        case "LASER2":




                            List<string> laser_pro_temp = new List<string>();
                            laser_pro_temp.Add(dt_cp.Rows[0]["Name"].ToString());
                            laser_pro_temp.Add(dt_cp.Rows[0]["info10"].ToString());
                            laser_pro_temp.Add(dt_cp.Rows[0]["info11"].ToString());

                            laser_pro_temp.Add(dt_cp.Rows[0]["info12"].ToString());//取消
                            laser_pro_temp.Add(dt_cp.Rows[0]["info13"].ToString());//取消
                            laser_pro_temp.Add(dt_cp.Rows[0]["info14"].ToString());//取消


                            // string table_laser_temp = CreateTable(laser_pro_temp, wif_list, "");
                            //  区分 LASER  CP0 ,CP1,CP2,CP3
                            //2022-09-23  修改
                            List<string> wif_list_LASER_temp = new List<string>();
                            for (int wi = 0; wi < wif_list.Count; wi++)
                            {
                                wif_list_LASER_temp.Add(wif_list[wi].ToString() + "-CP" + process_name_s.ToUpper().Replace("LASER", ""));
                            }
                            string table_laser_temp = CreateTable(laser_pro_temp, wif_list_LASER_temp, "");



                            //获取 镭射 程序列表
                            format.SubStrings["lasercheck_" + process_name_s].Value = "1";
                            string basic_table_temp = ReadTxt("basic_table");
                            format.SubStrings["lasertable_" + process_name_s].Value = string.Format(basic_table_temp, table_laser_temp);
                            break;

                        default:

                            break;
                    }
                }
                total_prcess += " 结束";

                format.SubStrings["totalprocess"].Value = "";
                format.SubStrings["totalprocess"].Value = total_prcess;


                if (cptotal_info != "")
                {
                    cptotal_info = cptotal_info.Substring(0, cptotal_info.Length - 1);
                }
                format.SubStrings["cptotal"].Value = cptotal_info;




                #region RUN CARD 控制流程单 每页打印数量

                string process_html_info = ReadTxt("module_total");
                string process_html_table = ReadTxt("module_table");
                string process_html_ini = ReadTxt("ini");
                if (process_html_info == "")
                {
                    MessageBox.Show("未配置打印主信息", "系统提示");
                    return;
                }
                if (process_html_table == "")
                {
                    MessageBox.Show("未配置打印表格信息", "系统提示");
                    return;
                }
                if (process_html_ini == "")
                {
                    MessageBox.Show("未配置打印表格配置信息", "系统提示");
                    return;
                }



                string[] process_html_ini_list = process_html_ini.Split(';');
                Dictionary<string, string> process_html_ini_dic = new Dictionary<string, string>();
                int Page_NO = 0;
                int html_number = 0;
                foreach (var item in process_html_ini_list)
                {
                    if (item == "" || item == "\n")
                    {
                        continue;
                    }
                    process_html_ini_dic.Add(item.Split('=')[1], item.Split('=')[0]);

                    if (item.Split('=')[1] == "pageno")
                    {
                        Page_NO = Convert.ToInt32(item.Split('=')[0]);
                    }
                    if (item.Split('=')[1] == "startno")
                    {
                        html_number = Convert.ToInt32(item.Split('=')[0]);
                    }
                }


                if (Page_NO == 0)
                {
                    MessageBox.Show("未配置流程页行数量", "系统提示");
                    return;
                }
                if (html_number == 0)
                {
                    MessageBox.Show("未配置流程初始行数量", "系统提示");
                    return;
                }
                //================================================================
                bool check_page = false;
                string temp_html = "";
                for (int i = 0; i < this.pwtDataGridView4.Rows.Count; i++)
                {
                    string process_name = this.pwtDataGridView4.Rows[i].Cells[1].Value.ToString();
                    foreach (var item in process_html_ini_dic)
                    {
                        if (item.Key == process_name)
                        {
                            html_number += Convert.ToInt32(item.Value);
                        }
                    }
                    //当大于35 行 切换第二页
                    // check_page  = true 第二次
                    // check_page  = false 第一次次
                    if (check_page)
                    {
                        temp_html += Process_ReadTxt(process_name, only_lot_code);
                    }
                    else
                    {
                        if (html_number >= Page_NO)
                        {
                            check_page = true;

                            process_html_info = string.Format(process_html_info, temp_html);
                            format.SubStrings["processhtml"].Value = process_html_info;

                            temp_html = Process_ReadTxt(process_name, only_lot_code);
                        }
                        else
                        {
                            temp_html += Process_ReadTxt(process_name, only_lot_code);
                        }
                    }
                }

                //当大于35 行 切换第二页
                // check_page  = true 第二次
                // check_page  = false 第一次次
                if (check_page)
                {

                    format.SubStrings["processnumber"].Value = "1";
                    process_html_info = string.Format(process_html_table, temp_html);
                    format.SubStrings["processhtable"].Value = process_html_info;
                }
                else
                {
                    format.SubStrings["processnumber"].Value = "0";
                    process_html_info = string.Format(process_html_info, temp_html);
                    format.SubStrings["processhtml"].Value = process_html_info;
                }

                #endregion



                #endregion

                // ======================================================
                format.Save();


                //   return;

                Messages messages;
                Result result = format.Print("CP_Lot_Process", 1000, out messages);
                // ======================================================




                #region 一致性表单打印
                DataTable dt_image = ex.Get_Data(" [dbo].[cp_hp_0714_mate_image_info_select] '" + Material_Name + "'");


                if (dt_image.Rows.Count == 0)
                {
                    MessageBox.Show("无一致性表单", "系统提示");
                }
                else
                {

                    for (int x = 0; x < dt_image.Rows.Count; x++)
                    {
                        string image_txt = dt_image.Rows[x]["图片信息"].ToString();

                        PrintDocument pd = new PrintDocument();

                        pd.DefaultPageSettings.Landscape = true;//横向打印  true 横 /  false 纵

                        pd.PrintPage += (sender1, args1) =>
                        {
                            Image i = ImageHelper.ConvertBase64ToImage(image_txt);

                            float newWidth = i.Width * 100 / i.HorizontalResolution;
                            float newHeight = i.Height * 100 / i.VerticalResolution;

                            float widthFactor = newWidth / args1.MarginBounds.Width;
                            float heightFactor = newHeight / args1.MarginBounds.Height;

                            if (widthFactor > 1 | heightFactor > 1)
                            {
                                if (widthFactor > heightFactor)
                                {
                                    newWidth = newWidth / widthFactor;
                                    newHeight = newHeight / widthFactor;
                                }
                                else
                                {
                                    newWidth = newWidth / heightFactor;
                                    newHeight = newHeight / heightFactor;
                                }
                            }
                            args1.Graphics.DrawImage(i, 0, 0, (int)newWidth, (int)newHeight);
                        };
                        pd.Print();

                    }
                }
                #endregion



                ex.Exe_Data("[dbo].[hp_0915_business_info_update_print_state_update] '" + IID_LOT + "','打印完成'");

                this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value = "打印完成";
                MessageBox.Show("打印成功", "系统提示");

            }
            finally
            {
                buttonX1.Enabled = true;
            }

        } 
        #endregion


        #region 补打印一致性
        private void buttonX8_Click(object sender, EventArgs e)
        {
            #region 一致性表单打印

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要打印的流程卡", "系统提示");
                return;
            }

            string Material_Name = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();

            DataTable dt_image = ex.Get_Data(" [dbo].[cp_hp_0714_mate_image_info_select] '" + Material_Name + "'");


            if (dt_image.Rows.Count == 0)
            {
                MessageBox.Show("无一致性表单", "系统提示");
            }
            else
            {

                for (int x = 0; x < dt_image.Rows.Count; x++)
                {
                    string image_txt = dt_image.Rows[x]["图片信息"].ToString();

                    PrintDocument pd = new PrintDocument();

                    pd.DefaultPageSettings.Landscape = true;//横向打印  true 横 /  false 纵

                    pd.PrintPage += (sender1, args1) =>
                    {
                        Image i = ImageHelper.ConvertBase64ToImage(image_txt);

                        float newWidth = i.Width * 100 / i.HorizontalResolution;
                        float newHeight = i.Height * 100 / i.VerticalResolution;

                        float widthFactor = newWidth / args1.MarginBounds.Width;
                        float heightFactor = newHeight / args1.MarginBounds.Height;

                        if (widthFactor > 1 | heightFactor > 1)
                        {
                            if (widthFactor > heightFactor)
                            {
                                newWidth = newWidth / widthFactor;
                                newHeight = newHeight / widthFactor;
                            }
                            else
                            {
                                newWidth = newWidth / heightFactor;
                                newHeight = newHeight / heightFactor;
                            }
                        }
                        args1.Graphics.DrawImage(i, 0, 0, (int)newWidth, (int)newHeight);
                    };
                    pd.Print();

                }
            }
            #endregion


            MessageBox.Show("打印一致性表单成功", "系统提示");

        }
        #endregion

        #region 查看一致性表单
        private void 查看一致性表单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            //  string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            //  string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆名称"].Value.ToString();
            string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["产品型号"].Value.ToString();
            一致表单上传 mfrom = new 一致表单上传(mate_id);

            mfrom.ShowDialog();
        } 
        #endregion


        #region 待删除

        ////=============================================

        // /// <summary>
        ///// 激光程序Table生产
        ///// </summary>
        ///// <param name="hang"></param>
        ///// <param name="lie"></param>
        ///// <returns></returns>
        //public static string CreateTable(List<string> hang, List<string> lie,string remark_info)
        //{

        //    string yi = "<tr><td height = '15%' colspan = '10' > 注意事项： " + remark_info + "</td > </tr > ";

        //    if (hang.Count == 2)
        //    {
        //        hang.Add("&nbsp;");
        //    }
        //    if (hang.Count == 1)
        //    {
        //        hang.Add("&nbsp;");
        //        hang.Add("&nbsp;");
        //    }
        //    string er = "<tr><td height='15%' colspan='2'>激光程序:</td>" +
        //        "<td colspan='2' ><div align='center'>" + hang[0] +
        //        "</div></td> <td colspan='2'><div align='center'>" + hang[1] +
        //        "</div></td> <td colspan='2'><div align='center'>" + hang[2] +
        //        "</div></td><td width='=170'>&nbsp;</td><td width='70'>&nbsp;</td></tr>";


        //    string san = " <tr>" +
        //        "<td width = '180' height= '20px' ><div align = 'center'> Wafer ID </div></td>" +
        //        "<td width = '180' ><p align= 'center'>晶圆刻号/Wafer ID</p></td>" +
        //        "<td width = '90' ><p align= 'center'> CP1 - lasser1 </p>    </td >" +
        //        "<td width = '90' ><div align= 'center'> CP2 - laser1 </div></td >" +
        //        "<td width = '90' ><div align= 'center'> CP1 - laser2 </div></td >" +
        //        "<td width = '90' ><div align= 'center'> CP2 - laser2 </div></td >" +
        //        "<td width = '90' ><div align= 'center'> CP1 - laser3 </div></td >" +
        //        "<td width = '90' ><div align= 'center'> CP2 - laser3 </div></td >" +
        //        "<td><div align = 'center'>操作员/OP</div></td >" +
        //        "<td><div align = 'center'>异常记录</div></td >" +
        //                 "</tr>";
        //    List<string> pop = new List<string>();
        //    for (int i = 0; i < lie.Count; i++)
        //    {
        //        int xuhao = i + 1;
        //        pop.Add("<tr>" +
        //       "<td><div align = 'center'>" + xuhao + "</div></td>" +
        //       "<td><div align = 'center'>" + lie[i] + "</div></td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "<td>&nbsp;</td>" +
        //       "</tr>"
        //       );
        //    }
        //    string si = string.Join("", pop.ToArray());
        //    string htmlcode = yi + er + san + si;
        //    return htmlcode;

        //}

        ////读取TXT模板文件
        //private string ReadTxt(string fileName)
        //{

        //    #region 读取文本文件

        //    if (!File.Exists(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt"))
        //    {
        //        return "";
        //    };

        //    string txt = "";
        //    StreamReader sr = new StreamReader(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt", System.Text.Encoding.Default);

        //    while (!sr.EndOfStream)
        //    {
        //        string str = sr.ReadLine();
        //        txt += str + "\n";
        //    }

        //    sr.Close();

        //    return txt;
        //    #endregion
        //}
        ////读取模板文件，补充工序参数
        //private string Process_ReadTxt(string fileName,string process_lot)
        //{

        //    #region 读取文本文件

        //    if (!File.Exists(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt"))
        //    {
        //        return "";
        //    };      
        //    string txt = "";
        //    StreamReader sr = new StreamReader(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt", System.Text.Encoding.Default);

        //    while (!sr.EndOfStream)
        //    {
        //        string str = sr.ReadLine();
        //        txt += str + "\n";
        //    }
        //    sr.Close();
        //    switch (fileName)
        //    {
        //        case "待定工序A":
        //            txt=string.Format(txt,"");
        //            break;
        //        case "待定工序B":
        //            txt=string.Format(txt,"","");
        //            break;
        //        case "待定工序C":
        //            txt=string.Format(txt,"","");
        //            break;
        //        default:

        //            txt=string.Format(txt);
        //            break;
        //    }
        //    return txt;
        //    #endregion
        //}




        ////============================================= 
        #endregion



        /// <summary>
        /// 激光程序Table生产
        /// </summary>
        /// <param name="hang"></param>
        /// <param name="lie"></param>
        /// <returns></returns>
        public static string CreateTable(List<string> hang, List<string> lie, string remark_info)
        {
            for (int i = 0; i < hang.Count; i++)
            {
                if (hang[i].ToString().Trim()=="")
                {
                    hang[i] = "不执行";
                }
            }


            int laser_count = 0;

            Dictionary<string, string> dic_hang = new Dictionary<string, string>();  

            for (int i = 0; i < hang.Count; i++)
            {
                if (hang[i] != "不执行")
                {
                    dic_hang.Add("laser"+i.ToString(), hang[i]);
                }
            }
            foreach (var item in dic_hang)
            {
                laser_count += item.Value.ToString().Split(';').Length;
            }


            string yi = "<tr><td height= '20px'  colspan = '" + (laser_count + 4).ToString() + "' > 注意事项： " + remark_info + "</td > </tr > ";
            string er = "<tr><td height= '20px'  colspan='2'><div align='center'>激光程序:</div></td>";


            //程序输出 行
            foreach (var item in dic_hang)
            {
                foreach (var item_sc in item.Value.ToString().Split(';'))
                {
                    er += "<td colspan='1' ><div align='center'>" + item_sc.ToString() + "</div></td> ";
                }
            }
            er += "<td width='100'>&nbsp;</td><td width='200'>&nbsp;</td></tr>";
            //输出 标题行


            string san = " <tr><td width = '150' height= '20px' ><div align = 'center'>LOT</div></td>" +
                "<td width = '150' ><p align= 'center'>晶圆刻号/Wafer ID</p></td>";

            foreach (var item in dic_hang)
            {
                foreach (var item_sc in item.Value.ToString().Split(';'))
                {
                    san += "<td width = '15%' ><p align= 'center'>" + item.Key.ToString() + "</p></td > ";
                }
            }
            san += "<td><div align = 'center'  width='100'>操作员/OP</div></td >" +
                 "<td><div align = 'center'  width='200'>异常记录</div></td ></tr>";

                
            List<string> pop = new List<string>();
            for (int i = 0; i < lie.Count; i++)
            {
                string temp_laser = "<tr>" +
               "<td><div align = 'center'>" + lie[i].Split('-')[0] + "</div></td>" +
               "<td><div align = 'center'>" + lie[i] + "</div></td>";


                foreach (var item in dic_hang)
                {
                    foreach (var item_sc in item.Value.ToString().Split(';'))
                    {
                        temp_laser += "<td width = '10%' ><p align= 'center'></p></td > ";
                    }
                }
                temp_laser += "<td><div align = 'center'>  </div></td >" +
                     "<td><div align = 'center'> </div></td ></tr>";
 


                pop.Add(temp_laser);
            }
            string si = string.Join("", pop.ToArray());
            string htmlcode = yi + san + er + si;
            return htmlcode;

        }

        private string ReadTxt(string fileName)
        {

            #region 读取文本文件

            if (!File.Exists(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt"))
            {
                return "";
            };

            string txt = "";
            StreamReader sr = new StreamReader(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt", System.Text.Encoding.Default);

            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                txt += str + "\n";
            }

            sr.Close();

            return txt;
            #endregion
        }
        //读取模板文件，补充工序参数
        private string Process_ReadTxt(string fileName, string process_lot, string info1 = "", string info2 = "", string info3 = "")
        {

            #region 读取文本文件

            if (!File.Exists(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt"))
            {
                return "";
            };
            string txt = "";
            StreamReader sr = new StreamReader(Application.StartupPath + @"\2_btw\module_txt\" + fileName + ".txt", System.Text.Encoding.Default);

            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                txt += str + "\n";
            }
            sr.Close();


            DataTable dt = ex.Get_Data("[dbo].[cp_hp_business_station_info_add_prcess_select] '" + process_lot + "','" + fileName + "'");


            


            //
          

            switch (fileName)
            {
                case "待定工序A":
                    txt = string.Format(txt, "");
                    break;
                case "待定工序B":
                    txt = string.Format(txt, "", "");
                    break;
                case "打点_烘烤":

                    if (dt.Rows.Count > 0)
                    {
                        info1 = dt.Rows[0]["info10"].ToString(); //墨水
                        info2 = dt.Rows[0]["info11"].ToString();//温度
                        info3 = dt.Rows[0]["Name"].ToString();//时间
                        txt = string.Format(txt, info1, info2, info3);
                    }
                    else {
                        txt = string.Format(txt, info1, info2, info3);
                    }
                    

                  
                    break;
                case "烘烤":
                case "烘烤1":
                case "烘烤2":
                case "烘烤3":
                case "烘烤4":

                    if (dt.Rows.Count > 0)
                    {
                        info1 = dt.Rows[0]["Name"].ToString();
                        info2 = dt.Rows[0]["info11"].ToString();
                        info3 = dt.Rows[0]["info10"].ToString();
                        txt = string.Format(txt, info1, info2, info3);
                    }
                    else
                    {
                        txt = string.Format(txt, info1, info2, info3);
                    }

                    txt = string.Format(txt, info1, info2, info3);
                    break;
                default:
                    txt = string.Format(txt);
                    break;
            }
            return txt;
            #endregion
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {

        }
    }
}
