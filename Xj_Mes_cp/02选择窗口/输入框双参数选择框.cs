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
    public partial class 输入框双参数选择框 : Form
    {
        public 输入框双参数选择框()
        {
            InitializeComponent();
        }


        public Boolean select_state = false;
        private void buttonX2_Click(object sender, EventArgs e)
        {

            select_state = false;
            this.Close();
        }


        public string info1 = "";
        public string info2 = "";
        public string info3 = "";
        private void buttonX1_Click(object sender, EventArgs e)
        {

            info1 = this.textBoxX1.Text;
            info2 = this.textBoxX2.Text;
            info3 = this.textBoxX3.Text;

            select_state = true;
            this.Close();


        }

        private void 输入框双参数选择框_Load(object sender, EventArgs e)
        {

        }
    }
}
