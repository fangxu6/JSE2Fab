using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class CP新出货清单管理 : DockContent
    {
        public CP新出货清单管理()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {


            string cus_name = this.pwtSearchBox1.Text;
            string lot = this.pwtSearchBox2.Text;

            string send_user = this.pwtSearchBox2.Text;
            string list_code = this.pwtSearchBox2.Text;
            string state = this.comboBoxEx1.SelectedItem.ToString();

            if (state=="全部")
            {
                state = "";
            }
            string is_date = "0";
            if (this.checkBoxX1.Checked)
            {
                is_date = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string sql = string.Format("[dbo].[cp_20220307_send_info_send_list_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                cus_name,lot,send_user,list_code,state,is_date,dat1,dat2);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1, true);

        }

        private void CP新出货清单管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
          

            输入信息单选择框 mfrom = new 输入信息单选择框("请输入箱数", "请输入数字");
            mfrom.ShowDialog();


            if (mfrom.select_state!=true)
            {
                return;
            }
            string 箱数 = mfrom.select_name;

            Dictionary<string, int> cus_list = new Dictionary<string, int>();
            Dictionary<string, int> cus_address_list = new Dictionary<string, int>();

            string cus_name = "";
            string cus_address = "";
            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToBoolean(pwtDataGridView1.Rows[i].Cells[0].Value) == true))
                {
                  

                    cus_name = this.pwtDataGridView1.Rows[i].Cells["客户名称"].Value.ToString();
                    cus_address = this.pwtDataGridView1.Rows[i].Cells["收货名称"].Value.ToString();

                    string 出货打印状态 = this.pwtDataGridView1.Rows[i].Cells["出货打印状态"].Value.ToString();


                    if (出货打印状态 != "未打印")
                    {
                        MessageBox.Show("存在已经打印信息", "系统提示"); return;
                    }

                    //========================
                    if (cus_list.Keys.Contains(cus_name))
                    {
                        cus_list[cus_name]++;
                    }
                    else
                    {
                        cus_list.Add(cus_name, 1);
                    }
                    //========================
                    if (cus_address_list.Keys.Contains(cus_address))
                    {
                        cus_address_list[cus_address]++;
                    }
                    else
                    {
                        cus_address_list.Add(cus_address, 1);
                    }
                }
            }


            if (cus_list.Count > 1)
            {
                MessageBox.Show("存在多个客户", "系统提示"); return;
            }
            if (cus_address_list.Count > 1)
            {
                MessageBox.Show("存在多个收货客户", "系统提示"); return;
            }


            DataSet dt_cus_use = ex.Get_Dset(" [dbo].[hp_cus_list_info_cus_send_select]   '" + cus_name + "','" + cus_address + "'");
            if (dt_cus_use.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("公司信息不存在", "系统提示"); return;
            }
            if (dt_cus_use.Tables[1].Rows.Count == 0)
            {
                MessageBox.Show("收货客户信息不存在", "系统提示"); return;
            }

            string company_name = dt_cus_use.Tables[0].Rows[0]["u_name"].ToString(); ;
            string company_phone = dt_cus_use.Tables[0].Rows[0]["u_tel"].ToString();
            string company_address = dt_cus_use.Tables[0].Rows[0]["u_address"].ToString();


            string packdate = DateTime.Now.ToString("yyyy-MM-dd");


            string contact_name = dt_cus_use.Tables[1].Rows[0]["u_user"].ToString();
            string contact_phone = dt_cus_use.Tables[1].Rows[0]["u_tel"].ToString();
            string contact_company = dt_cus_use.Tables[1].Rows[0]["u_info2"].ToString();
            string contact_address = dt_cus_use.Tables[1].Rows[0]["u_address"].ToString();




            string remark = "";
            string send_code = GetPurchesCode("CP发货" + DateTime.Now.ToString("yyyyMMdd"), "SD-" + DateTime.Now.ToString("yyyyMMdd"));






            #region 信息

            string table_title = ReadTxt("01");
            string table_temp_demo = ReadTxt("02");
            string table_down = ReadTxt("03");



            string table_temp = "";
            int box_number = 0;
            int pcs_number = 0;

            string cus_name_use = "";
            string cus_address_use = "";

            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToBoolean(pwtDataGridView1.Rows[i].Cells[0].Value) == true))
                {
                    // MessageBox.Show(pwtDataGridView1.Rows[i].Cells["序号"].Value.ToString());

                    cus_name_use = this.pwtDataGridView1.Rows[i].Cells["客户名称"].Value.ToString();
                    cus_address_use = this.pwtDataGridView1.Rows[i].Cells["收货名称"].Value.ToString();
                    string 测试程序 = this.pwtDataGridView1.Rows[i].Cells["测试程序"].Value.ToString();

                    string product_name = this.pwtDataGridView1.Rows[i].Cells["DeviceName"].Value.ToString();
                    string lot = this.pwtDataGridView1.Rows[i].Cells["批次号"].Value.ToString();
                    string wafer_qty = this.pwtDataGridView1.Rows[i].Cells["发货数量"].Value.ToString();
                    string wafer_id = this.pwtDataGridView1.Rows[i].Cells["位号简称"].Value.ToString();
                    string good_dies = this.pwtDataGridView1.Rows[i].Cells["good_die"].Value.ToString();
                    string Mode = this.pwtDataGridView1.Rows[i].Cells["mode"].Value.ToString();

                    string 补充备注说明 = this.pwtDataGridView1.Rows[i].Cells["备注"].Value.ToString();

                    remark += 补充备注说明;


                    box_number = 0;
                   


                    try
                    {
                        pcs_number += int.Parse(wafer_qty);
                    }
                    catch (Exception error)
                    {

                        box_number += int.Parse("0");
                    }

                    string temp01 = string.Format(table_temp_demo, cus_name_use, 测试程序, product_name, lot, wafer_qty, wafer_id, good_dies, Mode);
                    table_temp += temp01;
                }
            }


            #endregion





            string table_total = string.Format(table_down, 箱数, pcs_number);



            选择打印机 print_show = new 选择打印机();
            print_show.ShowDialog();
            if (print_show.select_state == false)
            {
                return;
            }
            string FIlePath = Application.StartupPath + @"\2_btw\CP出货标签.btw";
            Engine engine = new Engine(true);
            string mb = FIlePath;
            LabelFormatDocument format = engine.Documents.Open(mb);
            format.PrintSetup.PrinterName = print_show.DefaultPrintMac.ToString();

            format.SubStrings["send_table_info"].Value = table_title + table_temp + table_total;
            //format.SubStrings["gross_dies"].Value = gross_dies;
            //format.SubStrings["good_dies"].Value = good_dies;











            format.SubStrings["company_name"].Value = company_name;
            format.SubStrings["company_phone"].Value = company_phone;
            format.SubStrings["company_address"].Value = company_address;
            format.SubStrings["packdate"].Value = packdate;
            format.SubStrings["contact_name"].Value = contact_name;
            format.SubStrings["contact_phone"].Value = contact_phone;
            format.SubStrings["contact_company"].Value = contact_company;
            format.SubStrings["contact_address"].Value = contact_address;
            format.SubStrings["remark"].Value = remark;
            format.SubStrings["send_code"].Value = send_code;

            format.Save();

            Messages messages;
            Result result = format.Print("CP_Lot_Process", 1000, out messages);

            MessageBox.Show("打印成功", "系统提示");




            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToBoolean(pwtDataGridView1.Rows[i].Cells[0].Value) == true))
                {

                    string 序号 = this.pwtDataGridView1.Rows[i].Cells["序号"].Value.ToString();


                    ex.Exe_Data("[dbo].[cp_20220307_send_info_list_print_update]   '" + 序号 + "','已打印','" + send_code + "','" + base_info.user_code + "','"+箱数+"'");

                    this.pwtDataGridView1.Rows[i].Cells["出货打印状态"].Value = "已打印";
                    this.pwtDataGridView1.Rows[i].Cells["发货单号"].Value = send_code;
                }
            }
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


        private string GetPurchesCode(string name, string TitleNo)
        {

            DataTable dtb = ex.Get_Data("[dbo].[HP_ONLY_INFO_CREATE_SELECT] '" + name + "','" + name + "'");

            string sturct = TitleNo;// +DateTime.Now.ToString("yyyyMMdd").Substring(2);
            string sturct_info = dtb.Rows[0][0].ToString().PadLeft(3, '0');

            return sturct + sturct_info;

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToBoolean(pwtDataGridView1.Rows[i].Cells[0].Value) == true))
                {

                    string 序号 = this.pwtDataGridView1.Rows[i].Cells["序号"].Value.ToString();
                    string 发货单号 = this.pwtDataGridView1.Rows[i].Cells["发货单号"].Value.ToString();

                    ex.Exe_Data("[dbo].[cp_20220307_send_info_list_print_cancel_update]   '" + 序号 + "','未打印','" + 发货单号 + "','" + base_info.user_code + "','" + "" + "'");

                    this.pwtDataGridView1.Rows[i].Cells["出货打印状态"].Value = "未打印";
                    this.pwtDataGridView1.Rows[i].Cells["发货单号"].Value = "";
                }
            }
            MessageBox.Show("取消成功", "系统提示");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1.Text = "";
            this.pwtSearchBox2.Text = "";
            this.pwtSearchBox3.Text = "";
            this.pwtDataGridView1.Columns.Clear();
            this.comboBoxEx1.SelectedIndex = 0;
        }
    }
}
