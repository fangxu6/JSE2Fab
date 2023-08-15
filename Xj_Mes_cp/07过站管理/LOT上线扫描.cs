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
    public partial class LOT上线扫描 : DockContent
    {
        public LOT上线扫描()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void LOT上线扫描_Load(object sender, EventArgs e)
        {


          
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string LOT_ONLY_CODE = this.textBoxX1.Text.Trim();
            DataTable dt = ex.Get_Data("[dbo].[hp_1022_cp_up_line_select] '" + LOT_ONLY_CODE + "'");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("流程卡不存在,请确认！", "系统提示"); return;
            }

            DataTable dt_mate = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select]  '" + dt.Rows[0]["产品型号"].ToString() + "','" + dt.Rows[0]["版本"].ToString() + "'");


            if (dt_mate.Rows.Count == 0)
            {
                MessageBox.Show("晶圆信息未配置", "系统提示");
                return;
            }

            this.textBoxX3.Text = dt.Rows[0]["客户代码"].ToString();
            this.textBoxX4.Text = dt.Rows[0]["客户名称"].ToString();
            this.textBoxX5.Text = dt.Rows[0]["LOT"].ToString();
            this.textBoxX6.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX7.Text = dt.Rows[0]["版本"].ToString();
            this.textBoxX8.Text = dt.Rows[0]["数量"].ToString();
            this.textBoxX9.Text = dt.Rows[0]["位号"].ToString();
            this.textBoxX12.Text = dt.Rows[0]["位号"].ToString();

            //string temp = "开始--》";
            //for (int i = 0; i < dt_mate.Rows.Count; i++)
            //{
            //    temp += dt_mate.Rows[i][2].ToString() + "--》";
            //}
            //temp += "结束";

            //this.labelX14.Text = temp;



            //DataTable dt_now = ex.Get_Data("[dbo].[hp_1022_cp_up_line_info_get_process_select] '" + LOT_ONLY_CODE + "'");

            //if (dt_now.Rows.Count == 0)
            //{
            //    this.labelX17.Text = "待上线";
            //}
            //else {
            //    this.labelX17.Text = dt_now.Rows[0][0].ToString();
            
            //}

        }

        private void textBoxX4_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

           

            string lot_code = this.textBoxX1.Text.Trim();
            string eq = this.pwtSearchBox1.Text.Trim();
            string process_name = this.pwtSearchBox2.Text.Trim();
            string up_user = this.textBoxX11.Text.Trim();


            if (lot_code==""|| this.textBoxX3.Text.ToString().Trim()=="")
            {
                MessageBox.Show("请先扫描流程卡", "系统提示"); return;
            }
            if (eq == "")
            {
                MessageBox.Show("请先扫描设备编码", "系统提示"); return;
            }
            if (process_name == "")
            {
                MessageBox.Show("请先扫描流程工序", "系统提示"); return;
            }

          
            //if (!this.labelX14.Text.ToString().Contains(process_name))
            //{
            //    MessageBox.Show("当前流程未在配置流程中", "系统提示"); return;
            //}


            string mate_name = this.textBoxX6.Text;
            string mate_ves = this.textBoxX7.Text;

            DataTable dt_mate_info = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select]  '" + mate_name + "','" + mate_ves + "'");


            if (dt_mate_info.Rows.Count == 0)
            {
                MessageBox.Show("晶圆信息未配置流程", "系统提示");
                return;
            }


            //int check_process = 0;
            //int next_process = -1;
            //for (int i = 0; i < dt_mate_info.Rows.Count; i++)
            //{
            //    if (dt_mate_info.Rows[i][2].ToString() == process_name)
            //    {
            //        check_process++;
            //    }
            //    if (labelX17.Text.ToString() == dt_mate_info.Rows[i][2].ToString())
            //    {
            //        next_process = i;
            //    }
            //}

            //if ("待上线"==labelX17.Text.ToString())
            //{
            //    next_process = 0;
            //}

            //if (process_name!=dt_mate_info.Rows[next_process+1][2].ToString())
            //{
            //     MessageBox.Show("扫描工序错误,请确认", "系统提示");
            //    return;
            //}



            //if (check_process == 0)
            //{
            //    MessageBox.Show("扫描的流程错误", "系统提示");
            //    return;
            //}



            string post_info = this.textBoxX9.Text;

            string sql_insert = "[dbo].[hp_1022_cp_up_line_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}'";

            DataTable dt = ex.Get_Data(string.Format(sql_insert, lot_code, eq, process_name, up_user, "上线", post_info, base_info.user_code));

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("上线成功","系统提示");

            this.textBoxX1.Text = "";
            this.pwtSearchBox1.Text = "";


            //清除上线基础信息
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.textBoxX8.Text = "";
            this.textBoxX9.Text = "";
            this.pwtSearchBox2.Text = "";
            this.textBoxX11.Text = "";


        }

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                this.buttonX1_Click(null, null);
            }
        }

        private void textBoxX2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.buttonX2_Click(null, null);
            }
        }

        private void textBoxX10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxX10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                string mate_name = this.textBoxX6.Text;
                string mate_ves = this.textBoxX7.Text;

                DataTable dt_mate_info = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select]  '" + mate_name + "','" + mate_ves + "'");


                if (dt_mate_info.Rows.Count == 0)
                {
                    MessageBox.Show("晶圆信息未配置流程", "系统提示");
                    return;
                }

                string process_name = pwtSearchBox2.Text;
                int check_process = 0;
                int next_process = -1;
                for (int i = 0; i < dt_mate_info.Rows.Count; i++)
                {
                    if (dt_mate_info.Rows[i][2].ToString() == process_name)
                    {
                        check_process++;
                    }
                    if (labelX17.Text.ToString() == dt_mate_info.Rows[i][2].ToString())
                    {
                        next_process = i;
                    }
                }


                if (next_process == -1)
                {
                    MessageBox.Show("扫描工序错误,请确认", "系统提示");
                    return;
                }

                if ("待上线" == labelX17.Text.ToString())
                {
                    next_process = 0;
                }

                if (process_name != dt_mate_info.Rows[next_process][2].ToString())
                {
                    MessageBox.Show("扫描工序错误,请确认", "系统提示");
                    return;
                }

            }
        }

       

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_tsk_collect_info_get_cp_eq_select]", new List<int> { 0 });
            mfrom.ShowDialog();

            if (mfrom.select_state!=true)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            //选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_tsk_collect_info_get_cp_process_select]", new List<int> { 0 });
            //mfrom.ShowDialog();

            //if (mfrom.select_state != true)
            //{
            //    return;
            //}

            //this.pwtSearchBox2.Text = mfrom.select_name[0];
        }

        
    }
}
