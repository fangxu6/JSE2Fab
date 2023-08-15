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
    public partial class 输入信息单选择框 : Form
    {

        string title = "";
        string info = "";
        public 输入信息单选择框(string a, string b = "请输入参数")
        {
            title = a;
            info = b;
            InitializeComponent();
        }



        public bool select_state = false;

        public string select_name = "";

        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            select_name = this.textBoxX1.Text.Trim();

            if (select_name=="")
            {
                MessageBox.Show("不可以为空","系统提示");
                return;
            }
            select_state = true;
            this.Close();
        }

        private void 输入信息选择框_Load(object sender, EventArgs e)
        {
            this.labelX1.Text = title;
            this.textBoxX1.WatermarkText = info;
            this.textBoxX1.Text = info;
        }

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                buttonX1_Click(null, null);
            }
        }
    }
}
