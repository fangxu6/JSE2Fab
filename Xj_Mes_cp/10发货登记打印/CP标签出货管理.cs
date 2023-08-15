using pwt_system_comm_out;
using Seagull.BarTender.Print;
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
    public partial class CP标签出货管理 : DockContent
    {
        public CP标签出货管理()
        {
            InitializeComponent();
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            Lot数量选择 lotShow = new Lot数量选择();
            lotShow.ShowDialog();
            if (lotShow.select_ok == "1")
            {
                return;
            }
            List<int> lot = lotShow.str_no;
            string total_number = lotShow.total_number;
          //  string total_point = lotShow.total_point;
            string total_point_remark = lotShow.total_point_remark;
           // this.textBoxX3.Text = total_point;
            this.textBoxX5.Text = total_number;
            this.pwtSearchBox2.Text = total_point_remark;
        }


        db_deal ex = new db_deal();
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            string cus_name= this.pwtSearchBox1.Text ;
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT01] 'CP','" + cus_name + "','' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0]; 
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            //




            string cus_name = this.pwtSearchBox1.Text;
            string fab_lot = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string product_name = this.textBoxX3.Text;

            string cus_mate_name = this.textBoxX4.Text;
            string send_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string WAFER_ID = this.pwtSearchBox2.Text;
            string WAFER_NO = this.textBoxX5.Text;


            string GROSS_DIES = this.textBoxX6.Text;
            string GOOD_DIES = this.textBoxX7.Text;

            string remark = this.textBoxX8.Text;

            string wafer_size = this.textBoxX9.Text;



            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX11.Text;
            string info4 = this.textBoxX12.Text;
            string info5 = this.textBoxX13.Text;
            string info6 = this.textBoxX14.Text;

            string do_user = base_info.user_code;



            string sql = string.Format(@"[dbo].[hp_1222_cp_send_print_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'",
                  cus_name, fab_lot, lot, product_name, cus_mate_name, send_date, WAFER_ID, WAFER_NO, GROSS_DIES, GOOD_DIES
                  , remark, wafer_size, info2, info3, info4, info5, info6, do_user);


            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);


            MessageBox.Show("登记成功","系统提示");


        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text;
            string fab_lot = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string product_name = this.textBoxX3.Text;

            string cus_mate_name = this.textBoxX4.Text;
            string send_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string WAFER_ID = this.textBoxX4.Text;
            string WAFER_NO = this.textBoxX5.Text;


            string GROSS_DIES = this.textBoxX6.Text;
            string GOOD_DIES = this.textBoxX7.Text;

            string remark = this.textBoxX8.Text;

            string info1 = this.textBoxX9.Text;
            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX11.Text;
            string info4 = this.textBoxX12.Text;
            string info5 = this.textBoxX13.Text;
            string info6 = this.textBoxX14.Text;

            string do_user = base_info.user_code;


            string is_date = "0";
            if (this.checkBoxX1.Checked==true)
            {
                is_date = "1";
            }

            string dat1 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");



            string sql = string.Format(@"[dbo].[hp_1222_cp_send_print_info_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}'",
                  cus_name, fab_lot, lot, product_name, cus_mate_name, send_date, WAFER_ID, WAFER_NO, GROSS_DIES, GOOD_DIES
                  , remark, info1, info2, info3, info4, info5, info6, do_user,is_date,dat1,dat2);


            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

            MessageBox.Show("查询成功", "系统提示");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            if (MessageBox.Show("确定对选择的信息进行修改","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            string cus_name = this.pwtSearchBox1.Text;
            string fab_lot = this.textBoxX1.Text;
            string lot = this.textBoxX2.Text;
            string product_name = this.textBoxX3.Text;

            string cus_mate_name = this.textBoxX4.Text;
            string send_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string WAFER_ID = this.pwtSearchBox2.Text;
            string WAFER_NO = this.textBoxX5.Text;


            string GROSS_DIES = this.textBoxX6.Text;
            string GOOD_DIES = this.textBoxX7.Text;

            string remark = this.textBoxX8.Text;

            string info1 = this.textBoxX9.Text;
            string info2 = this.textBoxX10.Text;
            string info3 = this.textBoxX11.Text;
            string info4 = this.textBoxX12.Text;
            string info5 = this.textBoxX13.Text;
            string info6 = this.textBoxX14.Text;

            string do_user = base_info.user_code;


            string is_date = "0";
            if (this.checkBoxX1.Checked == true)
            {
                is_date = "1";
            }

            string dat1 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");



            string sql = string.Format(@"[dbo].[hp_1222_cp_send_print_info_update] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}'",
                  cus_name, fab_lot, lot, product_name, cus_mate_name, send_date, WAFER_ID, WAFER_NO, GROSS_DIES, GOOD_DIES
                  , remark, info1, info2, info3, info4, info5, info6, do_user, is_date, dat1, dat2,id);


            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbUpdateToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("确定对选择的信息进行删除", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            ex.Exe_Data("[dbo].[hp_1222_cp_send_print_info_delete]  '" + id + "','" + base_info.user_code + "'");

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);


            MessageBox.Show("删除成功", "系统提示");

        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {


            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            //string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            string cus_name = this.pwtSearchBox1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            string fab_lot = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["fab_lot"].Value.ToString();
            string lot = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["lot"].Value.ToString();
            string product_name = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["生产型号"].Value.ToString();

            string cus_mate_name = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户型号"].Value.ToString();
            //string send_date = 
            this.dateTimePicker1.Value = DateTime.Parse(this.pwtDataGridView1.SelectedRows[0].Cells["发货日期"].Value.ToString());

            string WAFER_ID = this.pwtSearchBox2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_id"].Value.ToString();
            string WAFER_NO = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_qty"].Value.ToString();


            string GROSS_DIES = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["gross_dies"].Value.ToString();
            string GOOD_DIES = this.textBoxX7.Text = this.pwtDataGridView1.SelectedRows[0].Cells["good_dies"].Value.ToString();

            string remark = this.textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();

            string info1 = this.textBoxX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_size"].Value.ToString();
            //string info2 = this.textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info3 = this.textBoxX11.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info4 = this.textBoxX12.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info5 = this.textBoxX13.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();
            //string info6 = this.textBoxX14.Text = this.pwtDataGridView1.SelectedRows[0].Cells[""].Value.ToString();







        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            string cus_name = this.pwtSearchBox1.Text = "";
            string fab_lot = this.textBoxX1.Text = "";
            string lot = this.textBoxX2.Text = "";
            string product_name = this.textBoxX3.Text = "";

            string cus_mate_name = this.textBoxX4.Text = "";
            //string send_date = 
            this.dateTimePicker1.Value = DateTime.Now;

            string WAFER_ID = this.pwtSearchBox2.Text = "";
            string WAFER_NO = this.textBoxX5.Text = "";


            string GROSS_DIES = this.textBoxX6.Text = "";
            string GOOD_DIES = this.textBoxX7.Text = "";

            string remark = this.textBoxX8.Text = "";

            string info1 = this.textBoxX9.Text = "";
            string info2 = this.textBoxX10.Text = "";
            string info3 = this.textBoxX11.Text = "";
            string info4 = this.textBoxX12.Text = "";
            string info5 = this.textBoxX13.Text = "";
            string info6 = this.textBoxX14.Text = "";
            this.pwtDataGridView1.Columns.Clear();
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string cus_name = "";
            string cus_mate_name = "";
            string lot = "";
            string wafer_id = "";
            string wafer_qty = "";
            string wafer_size = "";
            string s_date = "";             
            string remark = "";
            string fab_lot = "";
            string gross_dies = "";
            string good_dies = "";
            string product_name = "";



            cus_name = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            cus_mate_name = this.pwtDataGridView1.SelectedRows[0].Cells["客户型号"].Value.ToString();
            lot = this.pwtDataGridView1.SelectedRows[0].Cells["lot"].Value.ToString(); ;
            wafer_id = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_id"].Value.ToString();
            wafer_qty = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_qty"].Value.ToString();
            s_date = DateTime.Parse( this.pwtDataGridView1.SelectedRows[0].Cells["发货日期"].Value.ToString()).ToString("yyyy-MM-dd") ;
            remark = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();
            fab_lot = this.pwtDataGridView1.SelectedRows[0].Cells["fab_lot"].Value.ToString();
            gross_dies = this.pwtDataGridView1.SelectedRows[0].Cells["gross_dies"].Value.ToString();
            good_dies = this.pwtDataGridView1.SelectedRows[0].Cells["good_dies"].Value.ToString();
            product_name = this.pwtDataGridView1.SelectedRows[0].Cells["生产型号"].Value.ToString();

            wafer_size = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_size"].Value.ToString();


            string FIlePath = Application.StartupPath + @"\2_btw\CP出货标签\" + cus_name + ".btw";


            if (!System.IO.File.Exists(FIlePath))
            {
                FIlePath= Application.StartupPath + @"\2_btw\CP出货标签\Other.btw";
            }


            选择打印机 print_show = new 选择打印机();
            print_show.ShowDialog();
            if (print_show.select_state == false)
            {
                return;
            }
            Engine engine = new Engine(true);
            string mb = FIlePath;
            LabelFormatDocument format = engine.Documents.Open(mb);
            format.PrintSetup.PrinterName = print_show.DefaultPrintMac.ToString();


            format.SubStrings["cus_name"].Value = cus_name;

            format.SubStrings["cus_mate_name"].Value = cus_mate_name;
            format.SubStrings["lot"].Value = lot;
            format.SubStrings["wafer_id"].Value = wafer_id.Replace('；',',');
            format.SubStrings["wafer_size"].Value = wafer_size;

            format.SubStrings["wafer_qty"].Value = wafer_qty;
            format.SubStrings["s_date"].Value = s_date;
            format.SubStrings["remark"].Value = remark;

            format.SubStrings["fab_lot"].Value = fab_lot;
            format.SubStrings["gross_dies"].Value = gross_dies;
            format.SubStrings["good_dies"].Value = good_dies;



            format.SubStrings["product_name"].Value = product_name;
         

            format.Save();



            Messages messages;
            Result result = format.Print("CP_Lot_Process", 1000, out messages);
            ex.Exe_Data("[dbo].[hp_1222_cp_send_print_info_print_number_update] '"+id+"'");
            this.pwtDataGridView1.SelectedRows[0].Cells["状态"].Value = "已打印";
            MessageBox.Show("打印成功");
        }

        private void CP标签出货管理_Load(object sender, EventArgs e)
        {

        }

        private void buttonX7_Click(object sender, EventArgs e)
        {

            
            
            DataTable dtb = new DataTable();
            dtb = NPIOExcelHelper.ImportExeclToDataTable();

            if (dtb.Rows.Count == 0 || dtb == null)
            {
                return;
            }

            数据浏览显示 mfrom = new 数据浏览显示(dtb);
            mfrom.ShowDialog();

            if (mfrom.select_state==false)
            {
                return;
            }

            try
            {
                for (int i = 0; i < dtb.Rows.Count; i++)
                {

                    string cus_name = dtb.Rows[i]["COMPANY NAME"].ToString().Trim(); ;

                  

                    if (cus_name == "")
                    {
                        continue;
                    }





                    string fab_lot = dtb.Rows[i]["FAB LOT"].ToString();
                    string lot = dtb.Rows[i]["LOT NO."].ToString();
                    string product_name = dtb.Rows[i]["PRODUCT NAME"].ToString();

                    string cus_mate_name = dtb.Rows[i]["DEVICE  NAME"].ToString();
                    string send_date = dtb.Rows[i]["DATE"].ToString();

                    // 2022-05-12 
                    // 导入日期格式错误 跳过导入进系统
                    if (send_date == "")
                    {
                        continue;
                    }
                    try
                    {
                        Convert.ToDateTime(send_date);
                    }
                    catch (Exception error)
                    {
                        continue;
                    }




                    string WAFER_ID = dtb.Rows[i]["WAFER  ID"].ToString();
                    if (WAFER_ID == "")
                    {
                        continue;
                    }
                    string WAFER_NO = dtb.Rows[i]["WAFER QTY"].ToString();
                    if (WAFER_NO == "")
                    {
                        continue;
                    }

                    string GROSS_DIES = dtb.Rows[i]["GROSS DIES"].ToString();
                    string GOOD_DIES = dtb.Rows[i]["GOOD DIES"].ToString();

                    string remark = dtb.Rows[i]["备注"].ToString();

                    string wafer_size = dtb.Rows[i]["Wafer Size"].ToString();



                    string info2 = dtb.Rows[i]["测试程序"].ToString();
                    string info3 = dtb.Rows[i]["箱数"].ToString();
                    string info4 = dtb.Rows[i]["Mode"].ToString();
                    string info5 = dtb.Rows[i]["补充备注说明"].ToString();
                    string info6 = dtb.Rows[i]["出货地址"].ToString();

                    string do_user = base_info.user_code;



                    string sql = string.Format(@"[dbo].[hp_1222_cp_send_print_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'",
                          cus_name, fab_lot, lot, product_name, cus_mate_name, send_date, WAFER_ID, WAFER_NO, GROSS_DIES, GOOD_DIES
                          , remark, wafer_size, info2, info3, info4, info5, info6, do_user);


                    DataTable dt = ex.Get_Data(sql);

                    DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
                }

                MessageBox.Show("批量上传成功", "系统提示");
            }
            catch (Exception e_error)
            {
                MessageBox.Show("请使用新版模板导入\r\n模板包含:《测试程序,箱数,Mode,补充备注说明,出货地址》", "系统提示"); return;
            }
            


        }

        private void buttonX8_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }
            try
            {
                buttonX8.Enabled = false;
                // MessageBox.Show("批量待开放,请先确认单一模板");

                选择打印机 print_show = new 选择打印机();
                print_show.ShowDialog();
                if (print_show.select_state == false)
                {
                    return;
                }
                Engine engine = new Engine(true);

                this.progressBar1.Maximum = this.pwtDataGridView1.SelectedRows.Count-1;
                this.progressBar1.Minimum = 0;

                for (int i = 0; i < this.pwtDataGridView1.SelectedRows.Count; i++)
                {
                    Application.DoEvents();
                    this.progressBar1.Value = i;

                    string id = this.pwtDataGridView1.SelectedRows[i].Cells["序号"].Value.ToString();

                    string cus_name = "";
                    string cus_mate_name = "";
                    string lot = "";
                    string wafer_id = "";
                    string wafer_qty = "";
                    string wafer_size = "";
                    string s_date = "";
                    string remark = "";
                    string fab_lot = "";
                    string gross_dies = "";
                    string good_dies = "";
                    string product_name = "";



                    cus_name = this.pwtDataGridView1.SelectedRows[i].Cells["客户名称"].Value.ToString();
                    cus_mate_name = this.pwtDataGridView1.SelectedRows[i].Cells["客户型号"].Value.ToString();
                    lot = this.pwtDataGridView1.SelectedRows[i].Cells["lot"].Value.ToString(); ;
                    wafer_id = this.pwtDataGridView1.SelectedRows[i].Cells["wafer_id"].Value.ToString();
                    wafer_qty = this.pwtDataGridView1.SelectedRows[i].Cells["wafer_qty"].Value.ToString();
                    s_date = DateTime.Parse(this.pwtDataGridView1.SelectedRows[i].Cells["发货日期"].Value.ToString()).ToString("yyyy-MM-dd");
                    remark = this.pwtDataGridView1.SelectedRows[i].Cells["备注"].Value.ToString();
                    fab_lot = this.pwtDataGridView1.SelectedRows[i].Cells["fab_lot"].Value.ToString();
                    gross_dies = this.pwtDataGridView1.SelectedRows[i].Cells["gross_dies"].Value.ToString();
                    good_dies = this.pwtDataGridView1.SelectedRows[i].Cells["good_dies"].Value.ToString();
                    product_name = this.pwtDataGridView1.SelectedRows[i].Cells["生产型号"].Value.ToString();

                    wafer_size = this.pwtDataGridView1.SelectedRows[i].Cells["wafer_size"].Value.ToString();


                    string FIlePath = Application.StartupPath + @"\2_btw\CP出货标签\" + cus_name + ".btw";


                    if (!System.IO.File.Exists(FIlePath))
                    {
                        FIlePath = Application.StartupPath + @"\2_btw\CP出货标签\Other.btw";
                    }



                    string mb = FIlePath;
                    LabelFormatDocument format = engine.Documents.Open(mb);
                    format.PrintSetup.PrinterName = print_show.DefaultPrintMac.ToString();


                    format.SubStrings["cus_name"].Value = cus_name;

                    format.SubStrings["cus_mate_name"].Value = cus_mate_name;
                    format.SubStrings["lot"].Value = lot;
                    format.SubStrings["wafer_id"].Value = wafer_id.Replace('；', ',');
                    format.SubStrings["wafer_size"].Value = wafer_size;

                    format.SubStrings["wafer_qty"].Value = wafer_qty;
                    format.SubStrings["s_date"].Value = s_date;
                    format.SubStrings["remark"].Value = remark;

                    format.SubStrings["fab_lot"].Value = fab_lot;
                    format.SubStrings["gross_dies"].Value = gross_dies;
                    format.SubStrings["good_dies"].Value = good_dies;



                    format.SubStrings["product_name"].Value = product_name;

                    format.Save();
                    Messages messages;
                    Result result = format.Print("CP_Lot_Process", 500, out messages);
                    ex.Exe_Data("[dbo].[hp_1222_cp_send_print_info_print_number_update] '" + id + "'");

                    this.pwtDataGridView1.SelectedRows[i].Cells["状态"].Value = "已打印";

                }

                MessageBox.Show("打印完成", "系统提示");
            }
            finally
            {
                buttonX8.Enabled = true;
            }
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + @"\模板\");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
