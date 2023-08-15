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
    public partial class 单一参数输入选择框 : Form
    {
        public 单一参数输入选择框()
        {
            InitializeComponent();
        }


        public bool select_state = false;
        private void 单一参数输入选择框_Load(object sender, EventArgs e)
        {

        }


        public string select_info = "";
        private void button1_Click(object sender, EventArgs e)
        {
            select_info = this.textBox1.Text;
            if (select_info=="")
            {
                MessageBox.Show("请输入程序名称","系统提示");
                return;
            }
            select_state = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}
