using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_Report
{
    public partial class 线上预警查询管理 : DockContent
    {
        public 线上预警查询管理()
        {
            InitializeComponent();
        }
        db_deal db_Helper = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            DataTable dt, dt_warm;
            DataSet ds;
            this.pwtDataGridView1.DataSource = null;
            this.pwtDataGridView2.DataSource = null;

            string lot = this.textBoxX1.Text;

            if (this.pwtRadioButton1.Checked == true)
            {
                dt = db_Helper.Get_Data("[dbo].[hp_20220112_tsk_info_by_lot_select] '" + lot + "'");
                dt_warm = db_Helper.Get_Data("[dbo].[hp_20220620_warm_lot_select] '" + lot + "'");
                ds = db_Helper.Get_Dset("[dbo].[hp_20220620_warm_lot_analy_warm_select] '" + lot + "'");
            }
            else
            {
                dt = db_Helper.Get_Data("[dbo].[hp_20220112_tsk_info_by_lotOnly_select] '" + lot + "'");
                dt_warm = db_Helper.Get_Data("[dbo].[hp_20220620_warm_lot_only_select] '" + lot + "'");
                ds = db_Helper.Get_Dset("[dbo].[hp_20220620_warm_lot_only_analy_warm_select] '" + lot + "'");
            }

            this.pwtDataGridView1.DataSource = dt;
            this.pwtDataGridView2.DataSource = dt_warm;


            this.pwtDataGridView3.DataSource = ds.Tables[0];
            this.pwtDataGridView4.DataSource = ds.Tables[1];

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                string id = this.pwtDataGridView1.Rows[i].Cells["序号"].Value.ToString();


                db_Helper.Exe_Data(" [dbo].[tsk_total_only_one_update]  '" + id + "'");
                
            }
            MessageBox.Show("OK");
        }

        private void 基础资料修改TSK更新_Load(object sender, EventArgs e)
        {

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            try
            {
                this.buttonX3.Enabled = false;
                string lot = this.textBoxX1.Text;
                TskWarnManage(lot);
                MessageBox.Show("OK");
                buttonX1_Click(null,null);
            }
            finally
            {
                this.buttonX3.Enabled = true;
            }
        }
        public string TskWarnManage( string update_lot)
        {
            


            //================================
            //获取采集   TSK数据   未 处理信息
            //================================
             DataTable dt;

             if (this.pwtRadioButton1.Checked == true)
             {
                 dt = db_Helper.Get_Data("[dbo].[hp_20220209_warning_manage_info_update_info_by_lot_select] '" + update_lot + "'");

             }
             else
             {
                 dt = db_Helper.Get_Data("[dbo].[hp_20220209_warning_manage_info_update_info_by_lot_only_select] '" + update_lot + "'");
             }
             // DataTable dt = db_Helper.Get_Data("[dbo].[hp_20220209_warning_manage_info_update_info_by_lot_select] '" + update_lot + "'");


            
           
            int error_no = 1;
            string error_info_check = "0000";
            string error_info = DateTime.Now.ToString("MM-dd HH:mm") + "预警信息：\r\n";
            String warning_info = "设备:{0},批次:{1},型号:{2},版本:{8},工序:{3},类型:{7},位号:#{4},目标:{5},实际:{6},总数量:{9},Pass数量:{10},Fail数量:{11},低于目标良率\r\n-----------------------\r\n";


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Application.DoEvents();
                labelX2.Text = Math.Round((i * 1.0) / ((dt.Rows.Count - 1) * 1.0)*100, 2).ToString()+"%";
                try
                {


                    error_info_check = i.ToString() + "1.1";


                    string Voltage_Version = dt.Rows[i]["Voltage_Version"].ToString();//eq

                    string test_type = dt.Rows[i]["test_type"].ToString();//eq

                    string eq_name = dt.Rows[i]["eq"].ToString();//eq
                    string Mate_type = dt.Rows[i]["Mate_type"].ToString();//eq

                    string id = dt.Rows[i]["ID"].ToString();//id
                    string lot = dt.Rows[i]["lot"].ToString();//Lot
                    string PostNo = dt.Rows[i]["wafer_id"].ToString();//位号
                    string product_process = dt.Rows[i]["product_process"].ToString();//工序


                    string TotalDie = dt.Rows[i]["TotalDie"].ToString();//TotalDie
                    string PassDie = dt.Rows[i]["PassDie"].ToString();//PassDie
                    string FailDie = dt.Rows[i]["FailDie"].ToString();//FailDie



                    //获取排产物料基础信息
                    //获取TSK   批次  和 片号  通过该信息到追溯 查询  物料基础信息
                    DataTable data = db_Helper.Get_Data("[dbo].[hp_20220209_lot_post_no_get_mate_info] '" + lot + "','" + PostNo + "'");

                    if (data.Rows.Count == 0)
                    {
                        //=======================================
                        //修改状态    已处理/未处理
                        // 未查询到相关信息  修改相关信息处理状态
                        //=======================================
                        db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update] '" + id + "','Y','未获取排产信息',''");
                        //无数据    是否获取到流程配置设置预警 比例
                        continue;
                    }

                    error_info_check = i.ToString() + "1.2";
                    string MateID = data.Rows[0]["MateID"].ToString();

                    string DC = data.Rows[0]["DC"].ToString();
                    string test_eq = data.Rows[0]["test_eq"].ToString();
                    string cus_name = data.Rows[0]["CUS_NAME"].ToString();

                    string ves_name = data.Rows[0]["VES_NAME"].ToString();
                    string 排产时间 = data.Rows[0]["排产时间"].ToString();
                    string 排产单号 = data.Rows[0]["排产单号"].ToString();
                    string 排产ID = data.Rows[0]["排产ID"].ToString();


                    string info01 = MateID;
                    string info02 = 排产ID;

                    string info03 = "";
                    error_info_check = i.ToString() + "1.3";
                    string temp_sql_add = ",'" + ves_name + "','" + 排产时间 + "','" + 排产单号 + "','" + info01 + "','" + info02 + "','" + info03 + "'";

                    //=======================================
                    // 根据 晶圆基础信息查询预警信息
                    //=======================================
                    DataTable data_group = db_Helper.Get_Data("[dbo].[hp_20220210_tsk_group_info_select] '" + MateID + "','" + product_process + "'");



                    if (data_group.Rows.Count == 0)
                    {
                        db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update02] '" + id + "','Y','未获取晶圆型号与版本物料信息','','" + cus_name + "','" + test_eq + "','','" + DC + "'" + temp_sql_add);
                        //无数据    是否获取到流程配置设置预警 比例
                        continue;
                    }
                    error_info_check = i.ToString() + "1.4";
                    Voltage_Version = ves_name;


                    string test_program = data_group.Rows[0]["test_program"].ToString();



                    #region 判断目标良率

                    if (data_group.Rows[0]["info10"].ToString().Trim() == "")
                    {
                        db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update02] '" + id + "','Y','晶圆型号与版本未配置预警值','','" + cus_name + "','" + test_eq + "','" + test_program + "','" + DC + "'" + temp_sql_add);
                        //无数据    是否获取到流程配置设置预警 比例
                        continue;
                    }
                    error_info_check = i.ToString() + "1.5";
                    float yeb = 0;

                    try
                    {
                        yeb = float.Parse(data_group.Rows[0]["info10"].ToString().Trim());
                    }
                    catch (Exception error_in)
                    {
                        db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update02] '" + id + "','Y','晶圆型号与版本配置预警值错误','','" + cus_name + "','" + test_eq + "','" + test_program + "','" + DC + "'" + temp_sql_add);

                        //db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update] '" + id + "','Y','晶圆型号与版本配置预警值错误',''");
                        //无数据    是否获取到流程配置设置预警 比例
                        continue;
                    }

                    #endregion

                    error_info_check = i.ToString() + "1.6";

                    float 良率 = float.Parse(dt.Rows[i]["良率"].ToString());





                    //==============================================================

                    string post_no = dt.Rows[i]["PostNo"].ToString();
                    
                    #region Site预警
                    //  MateID
                    DataSet site_bin_warm = db_Helper.Sys_Get_Dset("[dbo].[hp_20220307_analy_bin_site_select] '" + id + "'");


                    DataTable dt_site_warm = db_Helper.Get_Data("[dbo].[hp_cp_site_warming_select] '" + MateID + "'");
                    if (site_bin_warm.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < site_bin_warm.Tables[0].Rows.Count; j++)
                        {
                            String site = site_bin_warm.Tables[0].Rows[i][0].ToString();
                            String site_zb = site_bin_warm.Tables[0].Rows[i][3].ToString();

                            for (int x = 0; x < dt_site_warm.Rows.Count; x++)
                            {
                                String warm_site = dt_site_warm.Rows[i][0].ToString().Split('-')[1];
                                String warm_site_zb = dt_site_warm.Rows[i][1].ToString();

                                if (Convert.ToDouble(site_zb) < Convert.ToDouble(warm_site_zb))
                                {
                                    db_Helper.Sys_Exe_Data(String.Format("[dbo].[hp_cp_tsk_site_warm_insert] '{0}','{1}','{2}','{3}','{4}','{5}'", id, warm_site, site_zb, warm_site_zb, "小于", post_no));
                                }

                            }

                        }
                    } 
                    #endregion




                    //  
                    #region Bin预警
                    //预警信息
                    DataTable dt_bin_warm = db_Helper.Get_Data("[dbo].[hp_cp_bin_warming_select] '" + MateID + "'");
                    if (site_bin_warm.Tables[1].Rows.Count > 0)
                    {

                        for (int bin_i = 0; bin_i < site_bin_warm.Tables[1].Rows.Count; bin_i++)
                        {

                            string bin = site_bin_warm.Tables[1].Rows[bin_i][0].ToString();
                            string bin_zb = site_bin_warm.Tables[1].Rows[bin_i][3].ToString();
                            //设置目标
                            for (int j = 0; j < dt_bin_warm.Rows.Count; j++)
                            {
                                string warm_bin = dt_bin_warm.Rows[j][0].ToString().Split('-')[1];
                                string warm_set = dt_bin_warm.Rows[j][1].ToString();
                                string warm_zb = dt_bin_warm.Rows[j][2].ToString();



                          

                                //      id   bin  实际  指标   目标
                                if (Convert.ToInt32(bin) == Convert.ToInt32(warm_bin))
                                {
                                    switch (warm_set)
                                    {
                                        case "大于":
                                            if (Convert.ToDouble(bin_zb) > Convert.ToDouble(warm_zb))
                                            {
                                                db_Helper.Sys_Exe_Data(String.Format("[dbo].[hp_cp_tsk_bin_warm_insert] '{0}','{1}','{2}','{3}','{4}','{5}'", id, warm_bin, bin_zb, warm_zb, warm_set, post_no));
                                            }
                                            break;
                                        case "大于等于":
                                            if (Convert.ToDouble(bin_zb) >= Convert.ToDouble(warm_zb))
                                            {
                                                db_Helper.Sys_Exe_Data(String.Format("[dbo].[hp_cp_tsk_bin_warm_insert] '{0}','{1}','{2}','{3}','{4}','{5}'", id, warm_bin, bin_zb, warm_zb, warm_set, post_no));
                                            }
                                            break;
                                        case "等于":
                                            if (Convert.ToDouble(bin_zb) == Convert.ToDouble(warm_zb))
                                            {
                                                db_Helper.Sys_Exe_Data(String.Format("[dbo].[hp_cp_tsk_bin_warm_insert] '{0}','{1}','{2}','{3}','{4}','{5}'", id, warm_bin, bin_zb, warm_zb, warm_set, post_no));
                                            }
                                            break;
                                        case "小于":
                                            if (Convert.ToDouble(bin_zb) < Convert.ToDouble(warm_zb))
                                            {
                                                db_Helper.Sys_Exe_Data(String.Format("[dbo].[hp_cp_tsk_bin_warm_insert] '{0}','{1}','{2}','{3}','{4}','{5}'", id, warm_bin, bin_zb, warm_zb, warm_set, post_no));
                                            }
                                            break;
                                        case "小于等于":
                                            if (Convert.ToDouble(bin_zb) <= Convert.ToDouble(warm_zb))
                                            {
                                                db_Helper.Sys_Exe_Data(String.Format("[dbo].[hp_cp_tsk_bin_warm_insert] '{0}','{1}','{2}','{3}','{4}','{5}'", id, warm_bin, bin_zb, warm_zb, warm_set, post_no));
                                            }
                                            break;
                                    }
                                }

                            }
                        }
                    } 
                    #endregion
                    //==============================================================
                    if (良率 > yeb)
                    {

                        db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update02] '" + id + "','Y','合格','" + yeb + "','" + cus_name + "','" + test_eq + "','" + test_program + "','" + DC + "'" + temp_sql_add);

                    }
                    else
                    {
                        error_info_check = i.ToString() + "1.7";
                        error_info += error_no.ToString() + "、" + string.Format(warning_info, eq_name, lot, Mate_type, product_process, PostNo, Math.Round(yeb, 4).ToString() + "%", Math.Round(良率, 4).ToString() + "%", test_type, Voltage_Version, TotalDie, PassDie, FailDie);
                        error_no++;

                        db_Helper.Sys_Exe_Data("[dbo].[hp_20220112_tsk_info_warning_state_update02] '" + id + "','Y','预警','" + yeb + "','" + cus_name + "','" + test_eq + "','" + test_program + "','" + DC + "'" + temp_sql_add);




                        error_info_check = i.ToString() + "1.8";

                        DataSet site_bin = db_Helper.Sys_Get_Dset("[dbo].[hp_20220307_analy_bin_site_select] '" + id + "'");

                        #region Site说明
                        string site_info = "";
                        if (site_bin.Tables[0].Rows.Count > 0)
                        {
                            string llun_yeil = Math.Round(100.0 / float.Parse(site_bin.Tables[0].Rows.Count.ToString()), 2).ToString();
                            int site_number = site_bin.Tables[0].Rows.Count - 1;
                            site_info = string.Format("Site说明({0}工位/理论：{1}%)\r\n不良总数：{2}\r\nSite{3},不良数：{4},不良占比：{5}%,占比最高,Site{6},不良数：{7},不良占比：{8}% 占比最低",
                                site_bin.Tables[0].Rows.Count.ToString(), llun_yeil, site_bin.Tables[0].Rows[0][1].ToString(),
                                 site_bin.Tables[0].Rows[0][0].ToString(), site_bin.Tables[0].Rows[0][2].ToString(), site_bin.Tables[0].Rows[0][3].ToString(),
                                 site_bin.Tables[0].Rows[site_number][0].ToString(), site_bin.Tables[0].Rows[site_number][2].ToString(), site_bin.Tables[0].Rows[site_number][3].ToString()
                                  );
                        }


                        
                        #endregion


                        error_info_check = i.ToString() + "1.9";

                        #region Bin说明
                        string bin_info_temp = "";
                        if (site_bin.Tables[1].Rows.Count > 0)
                        {
                            //不良总数：+ site_bin.Tables[1].Rows[0][1].ToString()
                            bin_info_temp = "\r\nBin说明(Top3):\r\n";
                            for (int bin_i = 0; bin_i < site_bin.Tables[1].Rows.Count; bin_i++)
                            {
                                if (bin_i > 2)
                                {
                                    continue;
                                }

                                string bin_info = "Top" + (bin_i + 1).ToString() + string.Format("、Bin{0},不良数：{1},不良占比：{2}% ;",
                                site_bin.Tables[1].Rows[bin_i][0].ToString(), site_bin.Tables[1].Rows[bin_i][2].ToString(), site_bin.Tables[1].Rows[bin_i][3].ToString());
                                bin_info_temp += bin_info;

                            }

                        }


                      
                        #endregion

                        error_info += site_info + bin_info_temp + "\r\n-----------------------\r\n";
                    }
                }

                catch (Exception exex_info)
                {
                    Console.WriteLine("异常定位点:" + error_info_check);
                    Console.WriteLine("系统错误：" + exex_info.StackTrace + "，详细错误：" + exex_info.Message);
                }
            }

            

            return "OK,处理数据:" + dt.Rows.Count.ToString() + "条,预警信息：" + (error_no - 1).ToString() + "条. 时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void pwtRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.pwtRadioButton1.Checked == true)
            {
                labelX1.Text = "批次号";
            }
            else {
                labelX1.Text = "流程卡号";
            }
        }


    }
}
