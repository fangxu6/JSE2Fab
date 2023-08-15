using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Seagull.BarTender.Print;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class CP出货标签打印管理 : DockContent
    {
        public CP出货标签打印管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {



            string p_lot = this.pwtSearchBox1.Text;
            string cus_name = this.pwtSearchBox2.Text;
            string lot = this.textBoxX1.Text;


            string is_date = "0";
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            if (this.checkBoxX1.Checked == true)
            {
                is_date = "1";
            }

            string print_state = this.comboBoxEx1.SelectedItem.ToString();

            if (print_state == "全部")
            {
                print_state = "";

            }

            string sql = string.Format("[dbo].[cp_20220307_send_info_print_bq_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'",
               p_lot, cus_name, lot, is_date, dat1, dat2, print_state);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1, true);

        }

        private void CP出货标签打印管理_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.SelectedIndex = 0;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

            this.buttonX2.Enabled = false;

            try
            {


                选择打印机 print_show = new 选择打印机();
                print_show.ShowDialog();
                if (print_show.select_state == false)
                {
                    return;
                }


                for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
                {
                    if ((Convert.ToBoolean(pwtDataGridView1.Rows[i].Cells[0].Value) == true))
                    {
                        #region MyRegion

                        string id = this.pwtDataGridView1.Rows[i].Cells["序号"].Value.ToString();


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



                        cus_name = this.pwtDataGridView1.Rows[i].Cells["客户名称"].Value.ToString();
                        cus_mate_name = this.pwtDataGridView1.Rows[i].Cells["晶圆型号"].Value.ToString();
                        lot = this.pwtDataGridView1.Rows[i].Cells["LOT"].Value.ToString(); ;
                        wafer_id = this.pwtDataGridView1.Rows[i].Cells["位号简称"].Value.ToString();
                        wafer_qty = this.pwtDataGridView1.Rows[i].Cells["发货数量"].Value.ToString();
                        s_date = DateTime.Parse(this.pwtDataGridView1.Rows[i].Cells["发货日期"].Value.ToString()).ToString("yyyy-MM-dd");
                        remark = this.pwtDataGridView1.Rows[i].Cells["备注"].Value.ToString();
                        fab_lot = this.pwtDataGridView1.Rows[i].Cells["LOT_OUT"].Value.ToString();
                        gross_dies = this.pwtDataGridView1.Rows[i].Cells["gross_die"].Value.ToString();
                        good_dies = this.pwtDataGridView1.Rows[i].Cells["good_die"].Value.ToString();
                        product_name = this.pwtDataGridView1.Rows[i].Cells["DeviceName"].Value.ToString();

                        wafer_size = this.pwtDataGridView1.Rows[i].Cells["尺寸"].Value.ToString();


                        string FIlePath = Application.StartupPath + @"\2_btw\CP出货标签\" + cus_name + ".btw";
                        if (!System.IO.File.Exists(FIlePath))
                        {
                            FIlePath = Application.StartupPath + @"\2_btw\CP出货标签\Other.btw";
                        }



                        Engine engine = new Engine(true);
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
                        Result result = format.Print("CP_Lot_Process", 1000, out messages);

                        ex.Exe_Data("[dbo].[cp_20220307_send_info_state_update]  '" + id + "','" + base_info.user_code + "'");

                        this.pwtDataGridView1.Rows[i].Cells["状态"].Value = "已打印";
                        #endregion

                    }
                }

            }
            catch (Exception ex_info)
            {
                MessageBox.Show("打印错误:" + ex_info.Message.ToString(), "系统提示"); return;
            }
            finally
            {
                this.buttonX2.Enabled = true;
            }

            MessageBox.Show("打印成功","系统提示");

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1.Text = "";
            this.pwtSearchBox2.Text = "";
            this.textBoxX1.Text = "";
            this.pwtDataGridView1.Columns.Clear();
        }
    }
}
