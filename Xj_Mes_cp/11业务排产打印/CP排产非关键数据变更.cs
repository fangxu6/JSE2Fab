using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public partial class CP排产非关键数据变更 : Form
    {
        public CP排产非关键数据变更(string c_dc, string c_weigong, string c_cihao, string c_epn, string c_lot_in, string c_lot_out, string c_demo_process, string c_remark, string c_打点, string c_订购日期, string c_需求日期, string c_计划完成日期)
        {

            dc = c_dc;
            weigong = c_weigong;
            cihao = c_cihao;
            epn = c_epn;
            lot_in = c_lot_in;
            lot_out = c_lot_out;
            demo_process = c_demo_process;
            remark = c_remark;
            打点 = c_打点;

            订购日期 = c_订购日期;
            需求日期 = c_需求日期;
            计划完成日期 = c_计划完成日期;


            InitializeComponent();
        }
        public string dc, weigong, cihao, epn, lot_in, lot_out, demo_process, remark, 打点,订购日期,需求日期,计划完成日期;
          

       public bool select_state = false;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            dc = this.textBoxX1.Text;
            weigong = this.textBoxX2.Text;
            cihao = this.textBoxX3.Text;
            epn = this.textBoxX4.Text;
            lot_in = this.textBoxX5.Text;
            lot_out = this.textBoxX6.Text;
            demo_process = this.textBoxX7.Text;
            remark = this.textBoxX8.Text;


            if (this.comboBoxEx1.SelectedIndex==-1)
            {
                MessageBox.Show("请选择打点类型"); return;
            }

            打点=this.comboBoxEx1.SelectedItem.ToString();



            订购日期 = this.textBoxX9.Text;
            需求日期 = this.textBoxX10.Text;
            计划完成日期 = this.textBoxX11.Text;

            select_state = true;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }

        private void CP排产非关键数据变更_Load(object sender, EventArgs e)
        {
             this.textBoxX1.Text = dc;
              this.textBoxX2.Text=weigong;
              this.textBoxX3.Text=cihao;
              this.textBoxX4.Text=epn;
             this.textBoxX5.Text=lot_in;
             this.textBoxX6.Text=lot_out;
             this.textBoxX7.Text=demo_process;
            this.textBoxX8.Text=remark;


               this.textBoxX9.Text=订购日期;
                  this.textBoxX10.Text = 需求日期;
                  this.textBoxX11.Text = 计划完成日期;


            if (打点 == "")
            {
                this.comboBoxEx1.SelectedIndex = -1;
            }
            else {

                this.comboBoxEx1.SelectedItem = 打点;
            }
        }
    }
}
