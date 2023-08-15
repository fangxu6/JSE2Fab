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
    public partial class CP晶圆基础信息管理审核 : DockContent
    {
        public CP晶圆基础信息管理审核()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region 查询
        private void buttonX1_Click(object sender, EventArgs e)
        {

            string is_weihu = "全部";



            #region 查询
            string material_Name = this.textBoxX1.Text.Trim();
            string material_chicun = this.textBoxX2.Text.Trim();
            string material_code = this.textBoxX3.Text.Trim();
            string shuliang = this.textBoxX4.Text.Trim();
            // string liuchenzu = this.pwtSearchBox1.Text.Trim();
            string dianya = this.pwtSearchBox2.Text.Trim();
            string zhenka = this.textBoxX6.Text.Trim();
            string zhongcetai = this.textBoxX7.Text.Trim();
            string ceshiban = this.textBoxX8.Text.Trim();
            string Info1 = this.textBoxX9.Text.Trim();
            string Info2 = this.textBoxX10.Text.Trim();
            string lianglv_pian = this.textBoxX11.Text.Trim();
            string lianglv_pi = this.textBoxX12.Text.Trim();
            string teshuguankong = this.textBoxX13.Text.Trim();
            string Bin = this.textBoxX14.Text.Trim();
            string Site = this.textBoxX15.Text.Trim();
            string Ramk = this.textBoxX16.Text.Trim();


            string cus_name = this.pwtSearchBox3.Text;
            string cus_code = this.pwtSearchBox4.Text;

            #endregion

            string shenhe = "";
            if (this.pwtRadioButton2.Checked == true)
            {
                shenhe = "审核通过";
            }
            if (this.pwtRadioButton3.Checked == true)
            {
                shenhe = "送审中";
            }


            //string sql = "[dbo].[hp0915_Wafer_Materials_information_Info_select02_add_sh_select] '" + material_Name + "','" + material_chicun + "','" + material_code + "','" + dianya + "','" + zhenka + "','" + zhongcetai + "','" + is_weihu + "','" + cus_name + "','" + cus_code + "','" + shenhe + "'";
            string sql = "[dbo].[hp0915_Wafer_Materials_information_Info_select02_add_sh_select01] '" + material_Name + "','" + material_chicun + "','" + material_code + "','" + dianya + "','" + zhenka + "','" + zhongcetai + "','" + is_weihu + "','" + cus_name + "','" + cus_code + "','" + shenhe + "'";
            DataTable dtb = ex.Get_Data(sql);
            DtbToUi.DtbToDGV(dtb, pwtDataGridView1);

            this.pwtDataGridView1.Columns["Bin指标"].Visible = false;
            this.pwtDataGridView1.Columns["Site指标"].Visible = false;

            this.pwtDataGridView1.Columns["流程组"].Visible = false;
            this.pwtDataGridView1.Columns["特殊管控良率指标"].Visible = false;

            this.pwtDataGridView1.Columns["序号"].Visible = false;
        } 
        #endregion

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {

            if (this.comboBoxEx3.Text == "")
            {
                MessageBox.Show("请选择是否进行打点"); return;

            }



            #region 晶圆基础信息

            string cus_name = this.pwtSearchBox3.Text.Replace("\n","");
            string cus_code = this.pwtSearchBox4.Text.Replace("\n", "");
            string material_Name = this.textBoxX1.Text.Trim().Replace("\n", "");
            string material_chicun = this.textBoxX2.Text.Trim().Replace("\n", "");
            string material_code = this.textBoxX3.Text.Trim().Replace("\n", "");
            string shuliang = this.textBoxX4.Text.Trim().Replace("\n", "");
            //string liuchenzu = this.pwtSearchBox1.Text.Trim();
            string dianya = this.pwtSearchBox2.Text.Trim().Replace("\n", "");
            string zhenka = this.textBoxX6.Text.Trim().Replace("\n", "");
            string zhongcetai = this.textBoxX7.Text.Trim().Replace("\n", "");
            string ceshiban = this.textBoxX8.Text.Trim().Replace("\n", "");
            string test_type = this.textBoxX9.Text.Trim().Replace("\n", "");
            string Info2 = this.textBoxX10.Text.Trim().Replace("\n", "");
            string lianglv_pian = this.textBoxX11.Text.Trim().Replace("\n", "");
            string lianglv_pi = this.textBoxX12.Text.Trim().Replace("\n", "");
            string teshuguankong = this.textBoxX13.Text.Trim().Replace("\n", "");
            string Bin = this.textBoxX14.Text.Trim().Replace("\n", "");
            string Site = this.textBoxX15.Text.Trim().Replace("\n", "");
            string Ramk = this.textBoxX16.Text.Trim().Replace("\n", "");
            string op = base_info.user_code;

            string dadian = this.comboBoxEx3.Text;

            string hk_ms = this.textBoxX22.Text;


            //烘烤时长
            string hkshic = textBoxX19.Text.Trim();
            //烘烤温度
            string hkwd = textBoxX18.Text.Trim();


            if (material_Name == "")
            {
                MessageBox.Show("请输入晶圆名称", "系统提示");
                return;
            }



            if (dianya == "")
            {
                MessageBox.Show("请输入电压版本", "系统提示");
                return;
            }


            DataTable dt_check = ex.Get_Data("[dbo].[W_Wafer_Materials_information_Info_check_insert] '" + material_Name + "','" + dianya + "'");

            if (dt_check.Rows[0][0].ToString() != "0")
            {
                MessageBox.Show("晶圆类型已经存在", "系统提示"); return;
            }


            #endregion

            #region 添加晶圆信息

            // string hk_ms = this.textBoxX22.Text;

            string sql = string.Format("[dbo].[hp_0915_W_Wafer_Materials_information_Info_insert01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}'",
                material_Name, material_chicun, material_code, shuliang, dianya, zhenka,
                zhongcetai, ceshiban, test_type, Info2, lianglv_pian, lianglv_pi,
                teshuguankong, Bin, Site, Ramk, op, hkshic, hkwd, cus_name, cus_code, dadian, hk_ms);
            DataTable dtb = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dtb, pwtDataGridView1);

            this.pwtDataGridView1.Columns["Bin指标"].Visible = false;
            this.pwtDataGridView1.Columns["Site指标"].Visible = false;
            this.pwtDataGridView1.Columns["流程组"].Visible = false;
            this.pwtDataGridView1.Columns["特殊管控良率指标"].Visible = false;
            this.pwtDataGridView1.Columns["序号"].Visible = false;

            MessageBox.Show("添加成功","系统提示");
            #endregion
        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            //工序组选择 mfrom = new 工序组选择(this.pwtSearchBox1);
            //mfrom.ShowDialog();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX3_Click(object sender, EventArgs e)
        {
            //选择删除行
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择你所需要删除的行", "系统提示");
                return;
            }

            if (MessageBox.Show("你确定删除晶圆名称为: 《 " + this.pwtDataGridView1.SelectedRows[0].Cells["晶圆名称"].Value.ToString() + "》 电压版本《" +
               this.pwtDataGridView1.SelectedRows[0].Cells["电压版本"].Value.ToString() + "》？"
               , "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ex.Exe_Data("W_Wafer_Materials_information_Info_delete '" + this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString() + "'");
                this.pwtDataGridView1.Rows.RemoveAt(this.pwtDataGridView1.SelectedRows[0].Index);
                MessageBox.Show("删除成功", "系统提示");
            }
        }
        //  iid;

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            #region 赋值
            textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆名称"].Value.ToString();
            textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆尺寸"].Value.ToString();
            textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆规格"].Value.ToString();
            //pwtSearchBox1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["流程组"].Value.ToString();
            pwtSearchBox2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["电压版本"].Value.ToString();
            textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["针卡名称"].Value.ToString();
            textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["单片数量"].Value.ToString();
            textBoxX7.Text = this.pwtDataGridView1.SelectedRows[0].Cells["中测台程序"].Value.ToString();
            textBoxX8.Text = this.pwtDataGridView1.SelectedRows[0].Cells["测试版"].Value.ToString();
            textBoxX11.Text = this.pwtDataGridView1.SelectedRows[0].Cells["单片良率指标"].Value.ToString();
            textBoxX12.Text = this.pwtDataGridView1.SelectedRows[0].Cells["批次良率指标"].Value.ToString();
            textBoxX13.Text = this.pwtDataGridView1.SelectedRows[0].Cells["特殊管控良率指标"].Value.ToString();
            textBoxX14.Text = this.pwtDataGridView1.SelectedRows[0].Cells["Bin指标"].Value.ToString();
            textBoxX15.Text = this.pwtDataGridView1.SelectedRows[0].Cells["Site指标"].Value.ToString();
            textBoxX9.Text = this.pwtDataGridView1.SelectedRows[0].Cells["测试机型"].Value.ToString();
            textBoxX10.Text = this.pwtDataGridView1.SelectedRows[0].Cells["烘烤时长"].Value.ToString();
            textBoxX16.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();
            textBoxX19.Text = this.pwtDataGridView1.SelectedRows[0].Cells["注意事项"].Value.ToString();
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            textBoxX18.Text = this.pwtDataGridView1.SelectedRows[0].Cells["烘烤温度"].Value.ToString();

            pwtSearchBox3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value.ToString();
            pwtSearchBox4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["客户代码"].Value.ToString();

            this.textBoxX22.Text = this.pwtDataGridView1.SelectedRows[0].Cells["烘烤墨水"].Value.ToString();

            #endregion




            string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string sql3 = "W_Wafer_station_info_select  '" + mate_id + "'";
            DataTable dtb3 = ex.Get_Data(sql3);
            DtbToUi.DtbToDGV(dtb3, this.pwtDataGridView2);
            this.pwtDataGridView2.Columns["晶圆基础ID"].Visible = false;
            this.pwtDataGridView2.Columns["序号"].Visible = false;

            #region Bin信息


            string sql = "HP1915_bin_base_info_select'" + iid + "'";
            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbToDGV(dt, pwtDataGridView3);
            pwtDataGridView3.Columns["序号"].Visible = false; 
            #endregion

            #region Site

            string sql1 = "czj_Site_base_info_select'" + iid + "'";
            DataTable dt1 = ex.Get_Data(sql1);
            DtbToUi.DtbToDGV(dt1, pwtDataGridView4);
            this.pwtDataGridView4.Columns["序号"].Visible = false;
            
            #endregion

            #region 流程
            string str = "czj_liuchengjilu_info_select'" + iid + "'";
            DataTable dq = ex.Get_Data(str);
            DtbToUi.DtbToDGV(dq, pwtDataGridView5);
            this.pwtDataGridView5.Columns["序号"].Visible = false;
            
            #endregion


            #region Bin描述
            RsetBin();

            string bin_sql = "[dbo].[hp_0928_mate_dsc_info_select] '"+iid+"'";
            DataTable bin_dtb = ex.Get_Data(bin_sql);
            for (int i = 0; i < bin_dtb.Rows.Count; i++)
            {
                string binName = bin_dtb.Rows[i][0].ToString();
                string binDsc = bin_dtb.Rows[i][1].ToString();
                for (int j = 0; j < this.pwtDataGridView6.Rows.Count; j++)
                {
                    if (this.pwtDataGridView6.Rows[j].Cells[0].Value.ToString()==binName)
                    {
                        this.pwtDataGridView6.Rows[j].Cells[1].Value = binDsc;
                    }
                }
            }


            #endregion




        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请在信息表中选中一行进行操作", "系统提示");
                return;
            }

            string dadian = this.comboBoxEx3.Text;
            if (dadian == "")
            {
                MessageBox.Show("请选择是否进行打点"); return;
            }
            #region 基础资料
            string cus_name = this.pwtSearchBox3.Text.Replace("\n", "");
            string cus_code = this.pwtSearchBox4.Text.Replace("\n", "");

            string material_Name = this.textBoxX1.Text.Trim().Replace("\n", "");
            string material_chicun = this.textBoxX2.Text.Trim().Replace("\n", "");
            string material_code = this.textBoxX3.Text.Trim().Replace("\n", "");
            string shuliang = this.textBoxX4.Text.Trim().Replace("\n", "");
            ///string liuchenzu = this.pwtSearchBox1.Text.Trim();
            string dianya = this.pwtSearchBox2.Text.Trim().Replace("\n", "");
            string zhenka = this.textBoxX6.Text.Trim().Replace("\n", "");
            string zhongcetai = this.textBoxX7.Text.Trim().Replace("\n", "");
            string ceshiban = this.textBoxX8.Text.Trim().Replace("\n", "");
            string Info1 = this.textBoxX9.Text.Trim().Replace("\n", "");
            string Info2 = this.textBoxX10.Text.Trim().Replace("\n", "");
            string lianglv_pian = this.textBoxX11.Text.Trim().Replace("\n", "");
            string lianglv_pi = this.textBoxX12.Text.Trim().Replace("\n", "");
            string teshuguankong = this.textBoxX13.Text.Trim().Replace("\n", "");
            string Bin = this.textBoxX14.Text.Trim().Replace("\n", "");
            string Site = this.textBoxX15.Text.Trim().Replace("\n", "");
            string hkshic = textBoxX19.Text.Trim().Replace("\n", "");
            string Ramk = this.textBoxX16.Text.Trim().Replace("\n", "");
            string op = base_info.user_code;



            string hkwd = textBoxX18.Text.Trim();
            string hk_ms = this.textBoxX22.Text;

            if (material_Name == "")
            {
                MessageBox.Show("请输入晶圆名称", "系统提示");
                return;
            }


            if (dianya == "")
            {
                MessageBox.Show("请输入电压版本", "系统提示");
                return;
            }



            #endregion
            if (MessageBox.Show("你确定修改晶圆名称为: 《 " + this.pwtDataGridView1.SelectedRows[0].Cells["晶圆名称"].Value.ToString() + "》， 电压版本：《" +

                this.pwtDataGridView1.SelectedRows[0].Cells["电压版本"].Value.ToString()
                + "》？"
            , "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string sql = string.Format("[dbo].[W_Wafer_Materials_information_Info_update01] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}'",
                    material_chicun, material_code, shuliang, dianya, zhenka, zhongcetai, ceshiban, Info1, Info2, lianglv_pian,
                    lianglv_pi, teshuguankong, Bin, Site, Ramk, this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString(),
                    hkshic, hkwd, cus_name, cus_code, dadian, hk_ms);

                DataTable dtb = ex.Get_Data(sql);
                //"W_Wafer_Materials_information_Info_update'" + material_chicun + "','" + material_code + "','" + shuliang + "','" + dianya + "','" + zhenka + "','" + zhongcetai + "','" + ceshiban + "','" + Info1 + "','" + Info2 + "','" + lianglv_pian + "','" + lianglv_pi + "','" + teshuguankong + "','" + Bin + "','" + Site + "','" + Ramk + "','" + this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString() + "'";
                #region 赋值
                //this.pwtDataGridView1.SelectedRows[0].Cells["物料名称"].Value = textBoxX1.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["晶圆尺寸"].Value = textBoxX2.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["晶圆规格"].Value = textBoxX3.Text;
                //this.pwtDataGridView1.SelectedRows[0].Cells["流程组"].Value = pwtSearchBox1.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["电压版本"].Value = pwtSearchBox2.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["针卡名称"].Value = textBoxX6.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["单片数量"].Value = textBoxX4.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["中测台程序"].Value = textBoxX7.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["测试版"].Value = textBoxX8.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["单片良率指标"].Value = textBoxX11.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["批次良率指标"].Value = textBoxX12.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["特殊管控良率指标"].Value = textBoxX13.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["Bin指标"].Value = textBoxX14.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["Site指标"].Value = textBoxX15.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["测试机型"].Value = textBoxX9.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["烘烤时长"].Value = textBoxX10.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value = textBoxX16.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["注意事项"].Value = textBoxX19.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["烘烤温度"].Value = textBoxX18.Text;
                this.pwtDataGridView1.SelectedRows[0].Cells["打点"].Value = dadian;
                this.pwtDataGridView1.SelectedRows[0].Cells["客户名称"].Value = cus_name;
                this.pwtDataGridView1.SelectedRows[0].Cells["客户代码"].Value = cus_code;
                this.pwtDataGridView1.SelectedRows[0].Cells["烘烤墨水"].Value = hk_ms;
                #endregion
            }

        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX5_Click(object sender, EventArgs e)
        {
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            pwtSearchBox2.Text = "";
            textBoxX6.Text = "";
            textBoxX7.Text = "";
            textBoxX8.Text = "";
            textBoxX9.Text = "";
            textBoxX10.Text = "";
            textBoxX11.Text = "";
            textBoxX12.Text = "";
            textBoxX13.Text = "";
            textBoxX14.Text = "";
            textBoxX15.Text = "";
            textBoxX16.Text = "";
            textBoxX19.Text = "";
            pwtSearchBox1.Text = "";
            pwtDataGridView1.Rows.Clear();
            pwtDataGridView2.Columns.Clear();
            pwtDataGridView5.Rows.Clear();
            pwtDataGridView3.Rows.Clear();
            pwtDataGridView4.Rows.Clear();
            comboBoxEx1.SelectedIndex = -1;
            comboBoxEx2.SelectedIndex = -1;
            textBoxX17.Text = "";
            textBoxX5.Text = "";
            this.textBoxX18.Text = "";

            this.pwtSearchBox3.Text = "";
            this.pwtSearchBox4.Text = "";

            this.textBoxX22.Text = "";
        }

        private void textBoxX11_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX4.Text != "" && textBoxX11.Text != "")
            {
                double sum = double.Parse(textBoxX4.Text.Trim());
                if (textBoxX11.Text == "")
                {
                    return;
                }
                double s = double.Parse(textBoxX11.Text.Trim());

                if (s > 100.0)
                {
                    MessageBox.Show("请重新输入");
                    textBoxX11.Text = "";
                    return;
                }
                labelX4.Text = (sum * s * 0.01).ToString();
            }
        }

        private void textBoxX12_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX4.Text != "" && textBoxX12.Text != "")
            {
                double sum = double.Parse(textBoxX4.Text.Trim());
                if (textBoxX12.Text == "")
                {
                    return;
                }
                double s = double.Parse(textBoxX12.Text.Trim());

                if (s > 100)
                {
                    MessageBox.Show("请重新输入");
                    textBoxX12.Text = "";
                    return;
                }
                labelX5.Text = (sum * s * 0.01).ToString();
            }
        }

        private void textBoxX13_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX4.Text != "" && textBoxX13.Text != "")
            {
                double sum = double.Parse(textBoxX4.Text.Trim());
                if (textBoxX13.Text == "")
                {
                    return;
                }
                double s = double.Parse(textBoxX13.Text.Trim());

                if (s > 100)
                {
                    MessageBox.Show("请重新输入");
                    textBoxX13.Text = "";
                    return;
                }
                labelX6.Text = (sum * s * 0.01).ToString();
            }

        }

        private void textBoxX14_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX4.Text != "" && textBoxX14.Text != "")
            {
                double sum = double.Parse(textBoxX4.Text.Trim());
                if (textBoxX14.Text == "")
                {
                    return;
                }
                double s = double.Parse(textBoxX14.Text.Trim());

                if (s > 100)
                {
                    MessageBox.Show("请重新输入");
                    textBoxX14.Text = "";
                    return;
                }
                labelX7.Text = (sum * s * 0.01).ToString();
            }
        }

        private void textBoxX15_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX4.Text != "" && textBoxX15.Text != "")
            {
                double sum = double.Parse(textBoxX4.Text.Trim());
                if (textBoxX15.Text == "")
                {
                    return;
                }
                double s = double.Parse(textBoxX15.Text.Trim());

                if (s > 100)
                {
                    MessageBox.Show("请重新输入");
                    textBoxX15.Text = "";
                    return;
                }
                labelX8.Text = (sum * s * 0.01).ToString();
            }
        }
        public static string ID;
        public static string value1;
        /// <summary>
        /// 双击框框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// str
       
        private void pwtDataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (pwtDataGridView2.SelectedRows.Count == 0)
            {
                return;
            }
            string iuser = base_info.user_name;
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string mate_name = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆名称"].Value.ToString();
            string mate_ves = this.pwtDataGridView1.SelectedRows[0].Cells["电压版本"].Value.ToString();

            string id = this.pwtDataGridView2.SelectedRows[0].Cells["序号"].Value.ToString();
            string gxname = this.pwtDataGridView2.SelectedRows[0].Cells["工序名称"].Value.ToString();
            string old_program = this.pwtDataGridView2.SelectedRows[0].Cells["程序名称"].Value.ToString();



            string temp_program = "";
            switch(gxname){
            
                case "CP1":
                case "CP1S":
                case "CP2":
                case "CP2S":
                case "CP3":
                case "CP3S":

                    Form_CP formcp = new Form_CP(id, mate_name, mate_ves, gxname);
                    formcp.ShowDialog();

                    if (formcp.select_state==false)
                    {
                        return;
                    }
                    temp_program = formcp.program_send;


                    this.pwtDataGridView2.SelectedRows[0].Cells["程序名称"].Value = temp_program;
                     string sql_cp = "czj_liuchengjilu_info_install'" + iid + "','" + iuser + "','" + temp_program + "','" + gxname + "','" + old_program + "'";
                     DataTable da_cp = ex.Get_Data(sql_cp);
                     DtbToUi.DtbAddToDGV(da_cp, pwtDataGridView5);

                    break;

                case "EQC1":
                case "EQC2":
                case "EQC3":
                    break;
                case "打点_烘烤":
                    Form_打点烘烤 formhk = new Form_打点烘烤(id, mate_name, mate_ves, gxname);
                    formhk.ShowDialog();
                    if (formhk.select_state == false)
                    {
                        return;
                    }                    
                    break;
                case "烘烤":
                case "烘烤1":
                case "烘烤2":
                case "烘烤3":
                    Form_烘烤 formhk01 = new Form_烘烤(id, mate_name, mate_ves, gxname);
                    formhk01.ShowDialog();
                    if (formhk01.select_state == false)
                    {
                        return;
                    }
                    break;
                    
                case "包装":
                    break;
                case "出片检验":
                    break;
                case "LASER":
                case "LASER0":
                case "LASER1":
                case "LASER2":

                    Form_LASER formlaser = new Form_LASER(id, mate_name, mate_ves, gxname);
                    formlaser.ShowDialog();

                    if (formlaser.select_state == false)
                    {
                        return;
                    }
                    temp_program = formlaser.program_send;

                    this.pwtDataGridView2.SelectedRows[0].Cells["程序名称"].Value = temp_program;
                    string sql_laser = "czj_liuchengjilu_info_install'" + iid + "','" + iuser + "','" + temp_program + "','" + gxname + "','" + old_program + "'";
                    DataTable da_laser = ex.Get_Data(sql_laser);
                    DtbToUi.DtbAddToDGV(da_laser, pwtDataGridView5);
                    break;
                default:
                    break;
            
            }

        
           

          


           // [dbo].[hp_1012_W_Wafer_station_info_update]

            //单一参数输入选择框 mfrom = new 单一参数输入选择框();
            //mfrom.ShowDialog();
            //if (mfrom.select_state != true)
            //{
            //    return;
            //}


            //string program = mfrom.select_info;           
            //this.pwtDataGridView2.SelectedRows[0].Cells["程序名称"].Value = program;


            ////修改参数
            //DataTable dtb = ex.Get_Data("W_Wafer_station_info1_insert'" + program + "','" + id + "'");
           




           


        }

        private void textBoxX13_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //0~9数字对应的keychar为：48~57，小数点为46，Backspace为8 
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)
            {
                if (textBoxX13.Text.Trim().Length <= 0)
                {
                    e.Handled = true;
                }
                else
                {
                    float f;
                    if (float.TryParse(textBoxX13.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void textBoxX14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //0~9数字对应的keychar为：48~57，小数点为46，Backspace为8 
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)
            {
                if (textBoxX14.Text.Trim().Length <= 0)
                {
                    e.Handled = true;
                }
                else
                {
                    float f;
                    if (float.TryParse(textBoxX14.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void textBoxX15_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //0~9数字对应的keychar为：48~57，小数点为46，Backspace为8 
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)
            {
                if (textBoxX15.Text.Trim().Length <= 0)
                {
                    e.Handled = true;
                }
                else
                {
                    float f;
                    if (float.TryParse(textBoxX15.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {

            //成品选择信息 mfrom = new 成品选择信息(this.pwtSearchBox2, "电压版本");
            //mfrom.ShowDialog();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要修改的晶圆信息");
                return;
            }


            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();



            工序组选择 mfrom = new 工序组选择(this.pwtSearchBox1);
            mfrom.ShowDialog();

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (this.pwtSearchBox1.Text == "")
            {
                return;
            }
            string liuchenzu = this.pwtSearchBox1.Text;
            string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            this.pwtDataGridView1.SelectedRows[0].Cells["流程组"].Value = liuchenzu;

            #region 添加流程信息

            string sql2 = "W_Wafer_station_info_insert'" + liuchenzu + "','" + mate_id + "','" + base_info.user_code + "'";
            DataTable dtb2 = ex.Get_Data(sql2);
            #endregion

            #region 查询流程信息

            string sql3 = "W_Wafer_station_info_select  '" + mate_id + "'";
            DataTable dtb3 = ex.Get_Data(sql3);

            DtbToUi.DtbToDGV(dtb3, this.pwtDataGridView2);
            this.pwtDataGridView2.Columns["晶圆基础ID"].Visible = false;
            this.pwtDataGridView2.Columns["序号"].Visible = false;
            string ms = "添加了流程组";
            string iuser = base_info.user_name;

            string sqlo = "czj_liuchengjilu_info_install1'" + iid + "','" + ms + "','" + iuser + "'";
            DataTable da = ex.Get_Data(sqlo);
            DtbToUi.DtbAddToDGV(da, pwtDataGridView5);


            #endregion
 
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            //流程组删除
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            if (MessageBox.Show("确定删除选择晶圆工序组信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            DataTable dtb = ex.Get_Data("W_Wafer_station_info1_delete'" + id + "'");

            this.pwtDataGridView2.Rows.Clear();
            this.pwtDataGridView1.SelectedRows[0].Cells["流程组"].Value = "";


            #region 添加日志

            string ms = "删除全部工站组";
            string iuser = base_info.user_name;
            string sqlo = "czj_liuchengjilu_info_install1'" + iid + "','" + ms + "','" + iuser + "'";
            DataTable da = ex.Get_Data(sqlo);
            DtbToUi.DtbAddToDGV(da, pwtDataGridView5);
            #endregion


            MessageBox.Show("流程组删除完成", "系统提示");

        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

              string gxname = this.pwtDataGridView2.SelectedRows[0].Cells["工序名称"].Value.ToString();
              string lid = this.pwtDataGridView2.SelectedRows[0].Cells["序号"].Value.ToString();


            DataTable dtb = ex.Get_Data("W_Wafer_station_info1_update'" + lid + "'");
            this.pwtDataGridView2.SelectedRows[0].Cells["程序名称"].Value = "";

            string ms = "删除了工序名称为:" + gxname + "的程序名称";
            string iuser = base_info.user_name;

            string sqlo = "czj_liuchengjilu_info_install1'" + iid + "','" + ms + "','" + iuser + "'";
            DataTable da = ex.Get_Data(sqlo);
            DtbToUi.DtbAddToDGV(da, pwtDataGridView5);

            MessageBox.Show("流程工序删除完成", "系统提示");
        }

        #region 单片次数
        private void textBoxX4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //0~9数字对应的keychar为：48~57，小数点为46，Backspace为8 
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)
            {
                if (textBoxX11.Text.Trim().Length <= 0)
                {
                    e.Handled = true;
                }
                else
                {
                    float f;
                    if (float.TryParse(textBoxX11.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        } 
        #endregion

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void superTabItem4_Click(object sender, EventArgs e)
        {


        }


        private void 晶圆基础信息管理_Load(object sender, EventArgs e)
        {
            //string sql = "czj_select_bin'" + "B" + "'";
            //DataTable dt = ex.Get_Data(sql);
            //combox_databind(comboBoxEx1, dt);

            comboBoxEx1.Items.Clear();
            for (int i = 0; i < 64; i++)
            {
                comboBoxEx1.Items.Add("BIN-" + i.ToString());
            }



            string sqlq = "czj_select_bin'" + "S" + "'";
            DataTable dtq = ex.Get_Data(sqlq);
            combox_databind(comboBoxEx2, dtq);


            this.comboBoxEx3.SelectedIndex = 0;


            RsetBin();

        }

        public void RsetBin() {

            this.pwtDataGridView6.Rows.Clear();
            for (int i = 0; i < 128; i++)
            {
                this.pwtDataGridView6.Rows.Add();
                this.pwtDataGridView6.Rows[i].Cells[0].Value = "BIN-" + i.ToString();
                this.pwtDataGridView6.Rows[i].Cells[1].Value = "";
            }
        }

        private void combox_databind(ComboBox combo, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                combo.Items.Add(dt.Rows[i][0]);
            }

        }

        private void buttonX10_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            string name = comboBoxEx2.Text;
            string zhibiao = textBoxX17.Text.Trim();
            string iuser = base_info.user_name;
            if (iid == "")
            {
                MessageBox.Show("请选择你要操作的数据", "系统提示");
                return;
            }
            if (name == "")
            {
                MessageBox.Show("请选择Site名称", "系统提示");
                return;
            }
            if (zhibiao == "")
            {
                MessageBox.Show("指标不能为空", "系统提示");
                return;
            }
            DataTable ds = ex.Get_Data("czj_bin_only_select'" + "2" + "','" + name + "','" + iid + "'");
            string la = ds.Rows[0][0].ToString();
            int sd = int.Parse(la);
            if (sd != 0)
            {
                MessageBox.Show("信息重复", "系统提示");
                return;
            }
            string sql = "czj_Site_base_info_install'" + name + "','" + zhibiao + "','" + iid + "','" + iuser + "'";
            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbAddToDGV(dt, pwtDataGridView4);
            MessageBox.Show("添加成功", "系统提示");
            this.pwtDataGridView4.Columns["序号"].Visible = false;
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }


            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string name = comboBoxEx1.Text;
            string zhibiao = textBoxX5.Text.Trim();
            string bin_info = this.comboBoxEx4.SelectedItem.ToString().Trim();
            string iuser = base_info.user_name;
            if (iid == "")
            {
                MessageBox.Show("请选择你要操作的数据", "系统提示");
                return;
            }
            if (name == "")
            {
                MessageBox.Show("请选择Bin名称", "系统提示");
                return;
            }
            if (zhibiao == "")
            {
                MessageBox.Show("指标不能为空", "系统提示");
                return;
            }
            if (bin_info == "")
            {
                MessageBox.Show("Bin含义不能为空", "系统提示");
                return;
            }
            DataTable ds = ex.Get_Data("czj_bin_only_select '" + "1" + "','" + name + "','" + iid + "'");
            string la = ds.Rows[0][0].ToString();
            int sd = int.Parse(la);
            if (sd != 0)
            {
                MessageBox.Show("信息重复", "系统提示");
                return;
            }
            string sql = "hp0915_bin_base_info_insatll' " + name + "','" + zhibiao + "','" + iid + "','" + iuser + "','" + bin_info + "'";
            DataTable dt = ex.Get_Data(sql);
            DtbToUi.DtbAddToDGV(dt, pwtDataGridView3);
            this.pwtDataGridView3.Columns["序号"].Visible = false;
            MessageBox.Show("添加成功", "系统提示");
        }

        private void buttonX11_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            string sql = "HP1915_bin_base_info_select'" + iid + "'";
            DataTable dt = ex.Get_Data(sql);
            if (pwtDataGridView3.Columns[0].Visible)
            {
                pwtDataGridView3.Columns[0].Visible = false;
            }
            DtbToUi.DtbToDGV(dt, pwtDataGridView3);
            this.pwtDataGridView3.Columns["序号"].Visible = false;
            MessageBox.Show("查询成功", "系统提示");

        }

        #region 隐藏
        private void buttonX12_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            string sql = "czj_Site_base_info_select'" + iid + "'";
            DataTable dt = ex.Get_Data(sql);
            if (pwtDataGridView4.Columns[0].Visible)
            {
                pwtDataGridView4.Columns[0].Visible = false;
            }
            DtbToUi.DtbToDGV(dt, pwtDataGridView4);
            MessageBox.Show("查询成功", "系统提示");
            this.pwtDataGridView4.Columns["序号"].Visible = false;
        }

        private void superTabControlPanel5_Click(object sender, EventArgs e)
        {

        }

        private void buttonX13_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择你所需要删除的行", "系统提示");
                return;
            }
            string LLD = this.pwtDataGridView3.SelectedRows[0].Cells["序号"].Value.ToString();
            string sql = "czj_bin_base_info_delect'" + LLD + "'";
            ex.Exe_Data(sql);
            this.pwtDataGridView3.Rows.RemoveAt(this.pwtDataGridView3.SelectedRows[0].Index);
            MessageBox.Show("删除成功", "系统提示");
        }

        private void pwtDataGridView3_MouseClick(object sender, MouseEventArgs e)
        {
            //if (this.pwtDataGridView3.SelectedRows.Count == 0)
            //{
            //    MessageBox.Show("请选择你所需要选中的行");
            //    return;
            //}


        }

        private void buttonX14_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView4.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择你所需要删除的行", "系统提示");
                return;
            }
            string ood = this.pwtDataGridView4.SelectedRows[0].Cells["序号"].Value.ToString();
            string sql = "czj_site_base_info_delect'" + ood + "'";
            ex.Exe_Data(sql);
            this.pwtDataGridView4.Rows.RemoveAt(this.pwtDataGridView4.SelectedRows[0].Index);
            MessageBox.Show("删除成功", "系统提示");
        } 
        #endregion
        // string ood;
        private void pwtDataGridView4_MouseClick(object sender, MouseEventArgs e)
        {
            //if (this.pwtDataGridView4.SelectedRows.Count == 0)
            //{
            //    MessageBox.Show("请选择你所需要选中的行");
            //    return;
            //}


        }

        #region 一致表单管理
        private void 上传一致表单ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            //  string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string mate_id = this.pwtDataGridView1.SelectedRows[0].Cells["晶圆名称"].Value.ToString();

            一致表单上传 mfrom = new 一致表单上传(mate_id);

            mfrom.ShowDialog();
        }
        #endregion

        #region 客户名称查询
        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[HP0915_HP_CZJ_XJ_CUSTOMER_INFO_SELECT] 'CP' ", new List<int> { 4, 3 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }
            this.pwtSearchBox3.Text = mfrom.select_name[0];
            this.pwtSearchBox4.Text = mfrom.select_name[1];

        } 
        #endregion

        private void pwtDataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        #region bin定义双击
        private void pwtDataGridView6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            if (this.pwtDataGridView6.SelectedRows.Count == 0)
            {
                return;
            }

            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();



            单一参数输入选择框 mfrom = new 单一参数输入选择框();
            mfrom.ShowDialog();

            if (mfrom.select_state != true)
            {
                return;
            }

            string bin_name = this.pwtDataGridView6.SelectedRows[0].Cells[0].Value.ToString();
            string bin_disc = mfrom.select_info;
            this.pwtDataGridView6.SelectedRows[0].Cells[1].Value = bin_disc;


            ex.Exe_Data("  [dbo].[hp_0928_mate_dsc_info_insert]  '" + iid + "','" + bin_name + "','" + bin_disc + "','" + base_info.user_code + "'");

        } 
        #endregion

        #region 审核
        private void buttonX15_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }




            if (MessageBox.Show("确定通过审核？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            for (int i = 0; i < this.pwtDataGridView1.SelectedRows.Count; i++)
            {


                string id = this.pwtDataGridView1.SelectedRows[i].Cells["序号"].Value.ToString();
                //ex.Exe_Data(" [dbo].[W_Wafer_Materials_information_Info_sh_state_update]  '" + ID + "','Y','" + base_info.user_code + "'");
                ex.Exe_Data(" [dbo].[W_Wafer_Materials_information_Info_sh_state_update01]  '" + id + "','审核通过','" + base_info.user_code + "'");
                //this.pwtDataGridView1.SelectedRows[i].Cells["审核状态"].Value = "Y";
                this.pwtDataGridView1.SelectedRows[i].Cells["审核状态"].Value = "审核通过";
            }

            MessageBox.Show("审核成功", "系统提示");

        }
        #endregion

        #region 反审核
        private void buttonX16_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            for (int i = 0; i < this.pwtDataGridView1.SelectedRows.Count; i++)
            {


                string id = this.pwtDataGridView1.SelectedRows[i].Cells["序号"].Value.ToString();
                ex.Exe_Data(" [dbo].[W_Wafer_Materials_information_Info_sh_state_update01]  '" + id + "','未送审','" + base_info.user_code + "'");
                this.pwtDataGridView1.SelectedRows[i].Cells["审核状态"].Value = "未送审";
            }

            MessageBox.Show("退审成功", "系统提示");
        } 
        #endregion


    }
}

