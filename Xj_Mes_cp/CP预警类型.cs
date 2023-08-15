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
    public partial class CP预警类型 : Form
    {
        public CP预警类型()
        {
            InitializeComponent();
        }



        public bool select_state = false;
        public string select_res = "";
        private void button1_Click(object sender, EventArgs e)
        {


            select_res = ((Button)sender).Text.ToString();
            select_state = true;


        }
    }
}
