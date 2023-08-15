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
    public partial class Lot过站扫描 : DockContent
    {
        public Lot过站扫描()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        #region 查询流程卡
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string LOT_ONLY_CODE = this.textBoxX1.Text.Trim();


            if (LOT_ONLY_CODE == "")
            {
                MessageBox.Show("请扫描流程卡号", "系统提示"); return;
            }






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


            string 数量 = dt.Rows[0]["数量"].ToString();
            this.textBoxX8.Text = dt.Rows[0]["数量"].ToString();
            this.labelX20.Text = dt.Rows[0]["数量"].ToString();


            string 位号 = dt.Rows[0]["位号"].ToString();
            this.textBoxX9.Text = dt.Rows[0]["位号"].ToString();
            this.textBoxX12.Text = dt.Rows[0]["位号"].ToString();


            DataTable tz_dtb = ex.Get_Data("[dbo].[HP_SYS_SYSTEM_BASIC_INFO_SELECT] 'CP跳站配置','','',''");

            string temp = "开始--》";
            for (int i = 0; i < dt_mate.Rows.Count; i++)
            {
                int zt_no = 0;
                for (int x = 0; x < tz_dtb.Rows.Count; x++)
                {
                    if (tz_dtb.Rows[x][2].ToString().ToUpper() == dt_mate.Rows[i][2].ToString().ToUpper())
                    {
                        zt_no++;
                    }
                }

                if (zt_no > 0)
                {
                    temp += dt_mate.Rows[i][2].ToString() + "(跳站)--》";
                }
                else
                {
                    temp += dt_mate.Rows[i][2].ToString() + "--》";
                }


            }
            temp += "结束";

            this.labelX14.Text = temp;



            DataTable dt_now = ex.Get_Data("[dbo].[hp_1022_cp_up_line_info_get_process_select] '" + LOT_ONLY_CODE + "'");

            if (dt_now.Rows.Count == 0)
            {
                this.labelX17.Text = "待上线";
            }
            else
            {
                this.labelX17.Text = dt_now.Rows[0][0].ToString();

            }



            DataTable dt_dongjie = ex.Get_Data(" [dbo].[hp_cp_hold_process_info_get_hold_post_select]   '" + LOT_ONLY_CODE + "'");

            DtbToUi.DtbToDGV(dt_dongjie, this.pwtDataGridView2);

            string new_post = "";
            int new_post_number = 0;
            foreach (var item in 位号.Split('、'))
            {

                int temp_check_post = 0;

                for (int i = 0; i < dt_dongjie.Rows.Count; i++)
                {

                    if (dt_dongjie.Rows[i]["位号"].ToString().Contains(item))
                    {
                        temp_check_post++;
                    }

                }



                if (temp_check_post == 0)
                {
                    new_post_number++;
                    new_post += item + "、";
                }
            }



            int dongjie_number = 0;
            for (int i = 0; i < dt_dongjie.Rows.Count; i++)
            {
                dongjie_number += int.Parse(dt_dongjie.Rows[i]["冻结数量"].ToString());
            }
            this.labelX10.Text = dongjie_number.ToString();


            if (new_post != "")
            {
                new_post = new_post.Substring(0, new_post.Length - 1);
            }


            this.textBoxX12.Text = new_post;
            this.labelX20.Text = new_post_number.ToString();


        }
        #endregion

        #region 流程卡过站
        private void buttonX2_Click(object sender, EventArgs e)
        {



            if (this.textBoxX12.Text == "")
            {
                MessageBox.Show("无可过站片号", "系统提示"); return;
            }

            string sql_insert = "[dbo].[hp_1022_cp_up_line_info_insert01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'";


            string lot_code = this.textBoxX1.Text.Trim();
            string eq = this.pwtSearchBox1.Text.Trim();
            string process_name = this.pwtSearchBox2.Text.Trim();
            string up_user = this.textBoxX11.Text.Trim();


            if (lot_code == "" || this.textBoxX3.Text.ToString().Trim() == "")
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


            #region 过站验证 
            int check_process = 0;
            int next_process = -1;
            int next_eqc = 1;


            DataTable tz_dtb = ex.Get_Data("[dbo].[HP_SYS_SYSTEM_BASIC_INFO_SELECT] 'CP跳站配置','','',''");

            for (int i = 0; i < dt_mate_info.Rows.Count; i++)
            {
                next_eqc = 1;

                //验证流程是否存在
                if (dt_mate_info.Rows[i][2].ToString() == process_name)
                {
                    check_process++;
                }
                //获取当前流程序号
                if (labelX17.Text.ToString() == dt_mate_info.Rows[i][2].ToString())
                {
                    next_process = i;
                }


                int zt_no = 0;
                for (int x = 0; x < tz_dtb.Rows.Count; x++)
                {
                    if (tz_dtb.Rows[x][2].ToString().ToUpper() == dt_mate_info.Rows[next_process + 1][2].ToString().ToUpper())
                    {
                        zt_no++;
                    }
                }

                if (zt_no > 0)
                {
                    next_eqc++;
                }

                ////去除EQC
                //if (dt_mate_info.Rows[next_process + 1][2].ToString().ToUpper()=="EQC1")
                //{
                //    next_eqc++;
                //}
                //if (dt_mate_info.Rows[next_process + 1][2].ToString().ToUpper() == "EQC2")
                //{
                //    next_eqc++;
                //}
                //if (dt_mate_info.Rows[next_process + 1][2].ToString().ToUpper() == "EQC3")
                //{
                //    next_eqc++;
                //}
            }

            if ("待上线" == labelX17.Text.ToString())
            {
                next_process = -1;
            }

            if (process_name != dt_mate_info.Rows[next_process + next_eqc][2].ToString())
            {
                MessageBox.Show("扫描工序错误或当前站是跳过站,请确认", "系统提示");
                return;
            }



            if (check_process == 0)
            {
                MessageBox.Show("扫描的流程错误", "系统提示");
                return;
            }

            #endregion


            string post_info = this.textBoxX12.Text;
            string post_number = this.labelX20.Text;





            // ===============冻结===============


            DataSet hold_dt = ex.Get_Dset(" [dbo].[hp20220107_hp_cp_hold_process_info_hold_info_select]    '" + lot_code + "','" + process_name + "'");
            if (hold_dt.Tables[0].Rows.Count > 0)
            {
                string show_message_temp = "";
                string show_message = "工序:{0},数量：{1} ,片号:{2} 状态:冻结";// string.Format();
                for (int i = 0; i < hold_dt.Tables[0].Rows.Count; i++)
                {
                    show_message_temp += string.Format(show_message, hold_dt.Tables[0].Rows[i]["hold_process"].ToString(), hold_dt.Tables[0].Rows[i]["hold_bumber"].ToString(), hold_dt.Tables[0].Rows[i]["hold_post"].ToString()) + "\r\n";
                }

                MessageBox.Show("当前工序随件单 有冻结信息：\r\n" + show_message_temp + " 请注意扣留.谢谢！", "系统提示");
            }


            // ===============冻结===============





            DataTable dt = ex.Get_Data(string.Format(sql_insert, lot_code, eq, process_name, up_user, "过站", post_info, base_info.user_code, post_number));

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("过站成功", "系统提示");

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
        #endregion

        private void Lot过站扫描_Load(object sender, EventArgs e)
        {

        }

        #region 流程卡号查询
        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_tsk_collect_info_get_cp_process_select]", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state != true)
            {
                return;
            }

            this.pwtSearchBox2.Text = mfrom.select_name[0];
        }
        #endregion

        #region 设备编码查询
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_eq_info_select]", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state != true)
            {
                return;
            }

            this.pwtSearchBox1.Text = mfrom.select_name[0];
        } 
        #endregion

        #region 拆分过站
        private void buttonX3_Click(object sender, EventArgs e)
        {

            string only_lot = this.textBoxX1.Text;
            //   string post_list=this.textBoxX9.Text;

            string post_list_new = this.textBoxX12.Text;

            Lot数量选择二次选择过站 mfrom = new Lot数量选择二次选择过站(only_lot, post_list_new);

            mfrom.ShowDialog();

            if (mfrom.select_ok == "1")
            {
                return;
            }


            this.textBoxX12.Text = mfrom.total_point;
            this.labelX20.Text = mfrom.total_number;
        } 
        #endregion
    }
}
