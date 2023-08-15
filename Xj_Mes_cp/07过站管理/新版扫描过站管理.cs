using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NPOI.SS.Formula.Functions;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class 新版扫描过站管理 : DockContent
    {
        public 新版扫描过站管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();


        private void pwtSearchBox1_TextChanged(object sender, EventArgs e)
        {
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
        }

        #region 流程卡查询
        private void buttonX4_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1_SearchBtnClick(null, null);
        }
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {

            string LOT_ONLY_CODE = this.pwtSearchBox1.Text.Trim();

            #region 加载基础信息
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

            this.textBoxX5.Text = dt.Rows[0]["客户代码"].ToString();
            this.textBoxX6.Text = dt.Rows[0]["客户名称"].ToString();
            this.textBoxX7.Text = dt.Rows[0]["LOT"].ToString();
            this.textBoxX8.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX9.Text = dt.Rows[0]["版本"].ToString();


            string 数量 = dt.Rows[0]["数量"].ToString();
            this.labelX7.Text = dt.Rows[0]["数量"].ToString();


            string 位号 = dt.Rows[0]["位号"].ToString();
            this.textBoxX1.Text = dt.Rows[0]["位号"].ToString();
            this.pwtSearchBox5.Text= dt.Rows[0]["序号"].ToString();
            this.pwtSearchBox6.Text= dt.Rows[0]["位号简称"].ToString();
            #endregion

            #region 添加工序信息

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

            this.labelX22.Text = temp;
            #endregion

            #region 加载基础当前工序
            //S0NW02.00.NA.01


            DataTable dt_now = ex.Get_Data("[dbo].[hp_1022_cp_up_line_info_get_process_select] '" + LOT_ONLY_CODE + "'");

            if (dt_now.Rows.Count == 0)
            {
                this.labelX21.Text = "待上线";
            }
            else
            {
                this.labelX21.Text = dt_now.Rows[0][0].ToString();

            }
            #endregion



            #region 加载冻结片号
            DataTable dt_dongjie = ex.Get_Data(" [dbo].[hp_cp_hold_process_info_get_hold_post_select]   '" + LOT_ONLY_CODE + "'");

            DtbToUi.DtbToDGV(dt_dongjie, this.pwtDataGridView2);


            string dj_temp = "";
            for (int i = 0; i < dt_dongjie.Rows.Count; i++)
            {
                dj_temp += dt_dongjie.Rows[i]["位号"].ToString() + "、";
            }

            string ok_scan = "";
            int ok_number = 0;
            for (int i = 1; i < 26; i++)
            {
                string post_no = i.ToString().PadLeft(2, '0');
                if (dj_temp.Contains(post_no))
                {
                    ok_scan += post_no + "、";
                    ok_number++;
                }
            }

            if (ok_scan != "")
            {
                ok_scan = ok_scan.Substring(0, ok_scan.Length - 1);
            }
            this.textBoxX4.Text = ok_scan;
            this.labelX20.Text = ok_number.ToString();




            #endregion
        }
        #endregion

        private void 新版扫描过站管理_Load(object sender, EventArgs e)
        {
            this.pwtSearchBox4.Text = base_info.user_name;
        }

        #region 工序名称查询
        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {


            if (this.textBoxX1.Text == "")
            {
                MessageBox.Show("请先查询随件单", "系统提示");
                return;
            }


            string mate_name = this.textBoxX8.Text.ToString();
            string mate_ves = this.textBoxX9.Text.ToString();
            string LOT_ONLY_CODE = this.pwtSearchBox1.Text.Trim();

            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_mate_info_proces_select_user_select] '" + mate_name + "','" + mate_ves + "','" + LOT_ONLY_CODE + "'", new List<int> { 0 });
            //获取过站工序

            mfrom.ShowDialog();
            if (mfrom.select_state != true)
            {
                return;
            }

            this.pwtSearchBox3.Text = mfrom.select_name[0];

            string only_lot = this.pwtSearchBox1.Text;

            //获取选择工序的已经过站数量
            DataTable dt = ex.Get_Data(" [dbo].[hp_1022_cp_up_line_info_get_process_post_info_select]  '" + only_lot + "','" + mfrom.select_name[0] + "'");


            string temp = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp += dt.Rows[i][0].ToString() + "、";
            }

            
            string ok_scan = "";//已过站片号
            int ok_number = 0;
            for (int i = 1; i < 26; i++)
            {
                string post_no = i.ToString().PadLeft(2, '0');
                if (temp.Contains(post_no))
                {
                    ok_scan += post_no + "、";
                    ok_number++;
                }
            }

            if (ok_scan != "")
            {
                ok_scan = ok_scan.Substring(0, ok_scan.Length - 1);
            }
            this.textBoxX2.Text = ok_scan;
            this.labelX8.Text = ok_number.ToString();
            DataTable dtall = ex.Get_Data("[dbo].[new_hp_0915_business_info_select] '" + LOT_ONLY_CODE + "'");
            if (dtall.Rows.Count>0)
            {
                string str = dtall.Rows[0][0].ToString();
                if (str.Contains("全测"))
                {
                    str = str.Split('[')[1].Split(']')[0].ToString();
                    textBoxX10.Text = str;
                }
                else
                {
                    textBoxX10.Text = "";
                }
            }

            //获取过站片号
            string total_post = "";//过站片号
            int total_number = 0;

            foreach (var item in this.textBoxX1.Text.ToString().Split('、'))
            {
                //跳过已经扫描
                if (ok_scan.Contains(item))
                {
                    continue;
                }
                //跳过已经冻结
                if (this.textBoxX4.Text.ToString().Contains(item))
                {
                    continue;
                }
                total_post += item.ToString() + "、";
                total_number++;
            }

            if (total_post != "")
            {
                total_post = total_post.Substring(0, total_post.Length - 1);

            }


            DataTable process_check = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select_user_select] '" + mate_name + "','" + mate_ves + "','" + LOT_ONLY_CODE + "'");


            string user_select_process = this.pwtSearchBox3.Text;

            int temp_process_check = -1;
            for (int i = 0; i < process_check.Rows.Count; i++)
            {
                if (process_check.Rows[i][0].ToString() == user_select_process)
                {
                    temp_process_check = i - 1;
                }
            }

            if (temp_process_check != -1)
            {
                string last_process = process_check.Rows[temp_process_check][0].ToString();
                DataSet check_process_list = ex.Get_Dset("[dbo].[hp_20220614_process_up_number] '" + LOT_ONLY_CODE + "','" + last_process + "','" + user_select_process + "'");

                string A = check_process_list.Tables[0].Rows[0][0].ToString();
                string B = check_process_list.Tables[1].Rows[0][0].ToString();
                if (Convert.ToInt32(A) < (total_number + Convert.ToInt32(B)))
                {
                    MessageBox.Show("过站数量超过上一站过站数量", "系统警告");
                }
            }







            this.textBoxX3.Text = total_post;
            this.textBoxX3.Tag = total_post;
            this.labelX9.Text = total_number.ToString();




        } 
        #endregion

        #region 过站
        private void buttonX1_Click(object sender, EventArgs e)
        {


            if (this.textBoxX1.Text == "")
            {
                MessageBox.Show("请先查询随件单", "系统提示");
                return;
            }

            string scan_post = this.textBoxX3.Text;

            if (scan_post == "")
            {
                MessageBox.Show("没有可过站的片号", "系统提示"); return;
            }
            string lot_code = this.pwtSearchBox1.Text.Trim();//流程卡
            string lot = this.textBoxX7.Text.Trim();//批号
            string eq = this.pwtSearchBox2.Text.Trim();//设备名称
            string process_name = this.pwtSearchBox3.Text.Trim();//工序名称
            string up_user = this.pwtSearchBox4.Text.Trim();//上线人
            if (lot_code == "" || lot == "")
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
            string mate_name = this.textBoxX8.Text.ToString();
            string mate_ves = this.textBoxX9.Text.ToString();
            DataTable dt_check = ex.Get_Data("[dbo].[hp_1022_cp_mate_info_proces_select_user_select] '" + mate_name + "','" + mate_ves + "','" + lot_code + "'");

            int temp_a = -1;
            string temp_b = "";
            //  string temp_c = "";


            for (int i = 0; i < dt_check.Rows.Count; i++)
            {
                if (dt_check.Rows[i][0].ToString() == process_name)
                {
                    temp_a = i;
                    temp_b = dt_check.Rows[i][1].ToString();
                }
            }


            if (temp_a == -1)
            {
                MessageBox.Show("工序选择错误", "系统提示"); return;
            }
            if (temp_a != 0)
            {
                if (dt_check.Rows[temp_a - 1][1].ToString() == "0")
                {
                    MessageBox.Show("不可以跳工序操作", "系统提示"); return;
                }
            }

            string now_process = dt_check.Rows[temp_a][0].ToString();
            string next_process = "";
            if ((temp_a + 1) == dt_check.Rows.Count)
            {
                next_process = "结束";
            }
            else
            {
                next_process = dt_check.Rows[temp_a + 1][0].ToString();
            }

            string post_info = this.textBoxX3.Text;
            //if (post_info.Contains("全测"))
            //{
            //    string id = this.pwtSearchBox5.Text;
            //    string wh = this.pwtSearchBox6.Text;
            //    string str = wh+"(" + post_info + ")";
            //    string sql_update = string.Format("[dbo].[new_hp_0915_business_info_update03] '{0}','{1}'", id, str);
            //    ex.Exe_Data(sql_update);
            //}
            string post_number = this.labelX9.Text;

            //hp_1022_cp_up_line_info_insert01  原始

            string sql_insert = "[dbo].[hp_1022_cp_up_line_info_insert02] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'";


            DataTable dt = ex.Get_Data(string.Format(sql_insert, lot_code, eq, process_name, up_user, "过站", post_info, base_info.user_code, post_number, now_process, next_process));

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("过站成功", "系统提示");




            ////清除上线基础信息
            this.pwtSearchBox1.Text = "";

            this.pwtSearchBox2.Text = "";
            this.pwtSearchBox3.Text = "";
            //this.pwtSearchBox4.Text = "";

            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.textBoxX8.Text = "";
            this.textBoxX9.Text = "";


            this.labelX7.Text = "0";
            this.labelX8.Text = "0";
            this.labelX9.Text = "0";
            this.labelX20.Text = "0";

            this.labelX22.Text = "";
            this.labelX21.Text = "";
            this.pwtSearchBox1.Focus();
            this.pwtDataGridView2.Columns.Clear();





        }
        #endregion

        #region 手动选择过站片号
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string only_lot = this.pwtSearchBox1.Text;

            string post_list_new = this.textBoxX3.Tag.ToString();

            if (post_list_new == "")
            {
                MessageBox.Show("没有可过站的片号", "系统提示"); return;
            };




            Lot数量选择二次选择过站 mfrom = new Lot数量选择二次选择过站(only_lot, post_list_new);

            mfrom.ShowDialog();

            if (mfrom.select_ok == "1")
            {
                return;
            }


            this.textBoxX3.Text = mfrom.total_point;
            this.labelX9.Text = mfrom.total_number;
        } 
        #endregion

        #region 清空
        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.pwtSearchBox1.Text = "";

            this.pwtSearchBox2.Text = "";
            this.pwtSearchBox3.Text = "";
            //  this.pwtSearchBox4.Text = "";

            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.textBoxX8.Text = "";
            this.textBoxX9.Text = "";


            this.labelX7.Text = "0";
            this.labelX8.Text = "0";
            this.labelX9.Text = "0";
            this.labelX20.Text = "0";

            this.labelX22.Text = "";
            this.labelX21.Text = "";

            this.pwtDataGridView2.Columns.Clear();

            this.pwtSearchBox5.Text = "";
            this.pwtSearchBox6.Text = "";
        }
        #endregion

        #region 设备名称查询
        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {


            if (this.textBoxX1.Text == "")
            {
                MessageBox.Show("请先查询随件单", "系统提示"); return;
            }
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_1022_cp_up_line_info_get_eq_info_select]", new List<int> { 0 });

            mfrom.ShowDialog();
            if (mfrom.select_state != true)
            {
                return;
            }

            this.pwtSearchBox2.Text = mfrom.select_name[0];
        }
        #endregion

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string post_number = this.textBoxX3.Text;
            string post_number2 = this.textBoxX10.Text;
            string[] poStrings = post_number.Split('、');
            string[] poStrings2 = post_number2.Split('、');
            StringBuilder sb=new StringBuilder();
            foreach (var s in poStrings)
            {
                sb.Append(s + "(全测)、");
            }

            if (sb.Length>1)
            {
                sb.Length = sb.Length - 1;
            }
            post_number = sb.ToString();
            this.textBoxX3.Text = post_number;

            poStrings = post_number.Split('、');
            HashSet<string> uniqueStrings = new HashSet<string>(poStrings);

            // 从第二个字符串数组中移除已存在于第一个字符串数组的字符串
            foreach (string str in poStrings2)
            {
                uniqueStrings.Remove(str);
            }
            post_number = string.Join("、", uniqueStrings);
            string LOT_ONLY_CODE = this.pwtSearchBox1.Text.Trim();
            DataTable dtall = ex.Get_Data("[dbo].[new_hp_0915_business_info_select] '" + LOT_ONLY_CODE + "'");
            
            if (post_number.Contains("全测"))
            {
                string id = this.pwtSearchBox5.Text;
                string wh = this.pwtSearchBox6.Text;
                string str = "";
                if (dtall.Rows.Count > 0)
                {
                    string str2 = dtall.Rows[0][0].ToString();
                    if (str2.Contains("全测"))
                    {
                        str2 = str2.Split(']')[0].ToString();
                        str = str2 + "、" + post_number + "]";
                    }
                    else
                    {
                        str = wh + "[" + post_number + "]";
                    }
                }
                string sql_update = string.Format("[dbo].[new_hp_0915_business_info_update03] '{0}','{1}'", id, str);
                ex.Exe_Data(sql_update);
            }

            dtall = new DataTable();
            dtall = ex.Get_Data("[dbo].[new_hp_0915_business_info_select] '" + LOT_ONLY_CODE + "'");
            if (dtall.Rows.Count > 0)
            {
                string str = dtall.Rows[0][0].ToString();
                if (str.Contains("全测"))
                {
                    str = str.Split('[')[1].Split(']')[0].ToString();
                    textBoxX10.Text = str;
                }
            }
            this.pwtSearchBox6.Text = dtall.Rows[0][0].ToString();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            //string post_number = this.textBoxX3.Text;
            //if (post_number.Contains("全测"))
            //{
            //    post_number = post_number.Replace("(全测)", "");
            //    this.textBoxX3.Text = post_number;
            //}

            string post_number2 = this.textBoxX10.Text;
            string post_number = this.textBoxX3.Text;

            // 字符串1和字符串2
            string string1 = post_number2;
            string string2 = post_number;

            // 逐个替换字符串2中的数字部分为空字符串
            foreach (var num in string2.Split('、'))
            {
                string1 = string1.Replace(num.Trim('(', ')')+"(全测)", "");
            }

            // 移除多余的逗号和空格
            string1 =string1.Replace("、、", "").Trim();
            string LOT_ONLY_CODE = this.pwtSearchBox1.Text.Trim();
            string id = this.pwtSearchBox5.Text;
            string wh = this.pwtSearchBox6.Text;
            string str = "";
            if (wh.Contains("全测"))
            {
                wh = wh.Split('[')[0];
                if (string1=="")
                {
                    str = wh;
                }
                else
                {
                    str = wh + "[" + string1 + "]";
                }
            }
            string sql_update = string.Format("[dbo].[new_hp_0915_business_info_update03] '{0}','{1}'", id, str);
            ex.Exe_Data(sql_update);
            DataTable dtall = ex.Get_Data("[dbo].[new_hp_0915_business_info_select] '" + LOT_ONLY_CODE + "'");
            if (dtall.Rows.Count > 0)
            {
                string str2 = dtall.Rows[0][0].ToString();
                if (str2.Contains("全测"))
                {
                    str2 = str.Split('[')[1].Split(']')[0].ToString();
                    textBoxX10.Text = str2;
                }
            }
            this.pwtSearchBox6.Text = dtall.Rows[0][0].ToString();
        }
    }
}
