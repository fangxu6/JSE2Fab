using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class CP_TSK数据分析 : DockContent
    {
        public CP_TSK数据分析()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void CP_TSK数据分析_Load(object sender, EventArgs e)
        {
            //axFramerControl1.Open(Application.StartupPath + @"\Sample2.xls");
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            string eq = this.pwtSearchBox1.Text;
            string mate_type = this.textBoxX2.Text;
            string lot = this.textBoxX3.Text;

            string cus_name = this.textBoxX1.Text;

            string is_check = "0";
            if (this.checkBoxX1.Checked)
            {
                is_check = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:00");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:00");



            string test_type = this.pwtSearchBox2.Text;
            string process_name = this.pwtSearchBox3.Text;


            string warining = this.pwtSearchBox4.Text;



            DataTable dt = ex.Get_Data("[dbo].[hp_20220112_tsk_info_analy_select01_01] '" + eq + "','" + mate_type + "','" + lot + "','" + is_check + "','" + dat1 + "','" + dat2 + "','" + test_type + "','" + process_name + "','" + cus_name + "'");


            this.pwtDataGridView1.DataSource = dt;


            this.pwtDataGridView2.DataSource = null;
            this.pwtDataGridView3.DataSource = null;
            this.pwtDataGridView4.DataSource = null;



            MessageBox.Show("查询成功", "系统提示");
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void 单一分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }





            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            DataSet dt = ex.Get_Dset("[dbo].[hp_20220112_site_bin_info_analy_select] '" + id + "'");

            this.pwtDataGridView2.DataSource = dt.Tables[0];

            this.pwtDataGridView3.DataSource = dt.Tables[1];

            this.pwtDataGridView4.DataSource = dt.Tables[2];
        }

        private void 批量分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }



            if (this.pwtSearchBox4.Text != "")
            {
                MessageBox.Show("分析数据,无法添加预警结果条件", "系统提示");
                return;
            }

            string eq = this.pwtSearchBox1.Text;
            string mate_type = this.textBoxX2.Text;
            string lot = this.textBoxX3.Text;


            string is_check = "0";
            if (this.checkBoxX1.Checked)
            {
                is_check = "1";
            }
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:00");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:00");


            //   string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string test_type = this.pwtSearchBox2.Text;
            string process_name = this.pwtSearchBox3.Text;


            if (is_check == "1")
            {

                DataSet dt = ex.Get_Dset("[dbo].[hp_20220112_site_bin_info_analy_total_add_data_select01]  '" + eq + "','" + mate_type + "','" + lot + "','" + is_check + "','" + dat1 + "','" + dat2 + "','" + test_type + "','" + process_name + "'");

                this.pwtDataGridView2.DataSource = dt.Tables[0];

                this.pwtDataGridView3.DataSource = dt.Tables[1];

                this.pwtDataGridView4.DataSource = dt.Tables[2];
            }
            else
            {


                DataSet dt = ex.Get_Dset("[dbo].[hp_20220112_site_bin_info_analy_total_select01]  '" + eq + "','" + mate_type + "','" + lot + "','" + is_check + "','" + dat1 + "','" + dat2 + "','" + test_type + "','" + process_name + "'");

                this.pwtDataGridView2.DataSource = dt.Tables[0];

                this.pwtDataGridView3.DataSource = dt.Tables[1];

                this.pwtDataGridView4.DataSource = dt.Tables[2];


            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            单一分析ToolStripMenuItem_Click(null, null);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            批量分析ToolStripMenuItem_Click(null, null);
        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_20220112_tsk_info_get_type_select] '设备'", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state == true)
            {
                this.pwtSearchBox1.Text = mfrom.select_name[0];
            }

        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_20220112_tsk_info_get_type_select] '测试类型'", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state == true)
            {
                this.pwtSearchBox2.Text = mfrom.select_name[0];
            }
        }

        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_20220112_tsk_info_get_type_select] '工序'", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state == true)
            {
                this.pwtSearchBox3.Text = mfrom.select_name[0];
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {

            this.pwtSearchBox1.Text = "";
            this.pwtSearchBox2.Text = "";
            this.pwtSearchBox3.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";


            this.pwtDataGridView1.DataSource = null;
            this.pwtDataGridView2.DataSource = null;
            this.pwtDataGridView3.DataSource = null;
            this.pwtDataGridView4.DataSource = null;

        }

        private void pwtSearchBox4_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_20220112_tsk_info_get_type_select] '预警结果'", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state == true)
            {
                this.pwtSearchBox4.Text = mfrom.select_name[0];
            }
        }

        private void 查看Map图ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private static int check_web = 0;
        private void buttonX5_Click(object sender, EventArgs e)
        {


            if (check_web==0)
            {
                if (MessageBox.Show(@"请再查看MAP之前确认是否可以打开公共盘"+Environment.NewLine+@"地址：\\192.168.5.26\prober\MAP\", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                check_web++;
            }

           
           


            if (this.pwtDataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("请选择需要查看MAP数据", "系统提示");
                return;
            }

            try
            {
                this.buttonX5.Enabled = false;
                string old_FilePath = this.pwtDataGridView1.SelectedRows[0].Cells["测试文件"].Value.ToString();
                string FilePath = old_FilePath.Replace(@"D:\PROBER\MAP\", @"\\192.168.5.26\prober\MAP\");

                if (!System.IO.File.Exists(FilePath))
                {
                    MessageBox.Show("TSK文件不存在", "系统提示"); return;
                }



                string lot = this.pwtDataGridView1.SelectedRows[0].Cells["批次号"].Value.ToString();
                string post = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_id"].Value.ToString();
                Pwt_Tsk.Tsk tsk = new Pwt_Tsk.Tsk(FilePath);
                tsk.Read();


                int Xmax = tsk.DieMatrix.XMax;
                int Ymax = tsk.DieMatrix.YMax;
                Object[,] Die_Info = tsk.DieMatrix.GetDieArray();



                string ExcelPath = Application.StartupPath + @"\2_Temp\" + post.PadLeft(3, '0') + "." + lot + "-" + post.PadLeft(2, '0');
                Pwt_Tsk.ToExcelHelper.Exeort2Excel(Die_Info, Ymax, Xmax, ExcelPath, false);




                //Process pr = new Process();//声明一个进程类对象
                //pr.StartInfo.FileName = ExcelPath + ".xls";
                //pr.Start();



                if (MessageBox.Show("文件已经生成是否查看","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)!= System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

             

                 System.Diagnostics.Process.Start(ExcelPath + ".xlsx");


            }
            catch (Exception exerror)
            {
                MessageBox.Show("错误:" + exerror.Message.ToString(), "系统提示");
            }
            finally
            {
                this.buttonX5.Enabled = true;
            }

        }


        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,

                FileShare.None);

                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用
        }

        private void 单片图表分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            string lot = this.pwtDataGridView1.SelectedRows[0].Cells["批次号"].Value.ToString();
            string no = this.pwtDataGridView1.SelectedRows[0].Cells["wafer_id"].Value.ToString();
            string process = this.pwtDataGridView1.SelectedRows[0].Cells["工序"].Value.ToString();
            string test_type = this.pwtDataGridView1.SelectedRows[0].Cells["测试类型"].Value.ToString();


            工程异常分析图谱 mfrom = new 工程异常分析图谱(lot, no, process, test_type);
            mfrom.ShowDialog();
        }

        private void 批次图表分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string lot = this.pwtDataGridView1.SelectedRows[0].Cells["批次号"].Value.ToString();
            string no = "";
            string process = this.pwtDataGridView1.SelectedRows[0].Cells["工序"].Value.ToString();
            string test_type = this.pwtDataGridView1.SelectedRows[0].Cells["测试类型"].Value.ToString();

            工程异常分析图谱 mfrom = new 工程异常分析图谱(lot, no, process, test_type);
            mfrom.ShowDialog();

        }
    }
}
