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
    public partial class 生产异常处理单片良率信息管理 : Form
    {
        public  string my_info = "";
        public bool select_state = false;
        public 生产异常处理单片良率信息管理()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string card1 = this.textBoxX1.Text;
            string card2 = this.textBoxX6.Text;
            string card3 = this.textBoxX11.Text;
            string card4 = this.textBoxX16.Text;
            string card5 = this.textBoxX25.Text;
            string card6 = this.textBoxX2.Text;
            string card7 = this.textBoxX7.Text;
            string card8 = this.textBoxX12.Text;
            string card9 = this.textBoxX17.Text;
            string card10 = this.textBoxX24.Text;
            string card11=this.textBoxX3.Text;
            string card12=this.textBoxX8.Text;
            string card13=this.textBoxX13.Text;
            string card14=this.textBoxX18.Text;
            string card15=this.textBoxX23.Text;
            string card16=this.textBoxX4.Text;
            string card17=this.textBoxX9.Text;
            string card18=this.textBoxX14.Text;
            string card19=this.textBoxX19.Text;
            string card20=this.textBoxX22.Text;
            string card21=this.textBoxX5.Text;
            string card22=this.textBoxX10.Text;
            string card23=this.textBoxX15.Text;
            string card24=this.textBoxX20.Text;
            string card25=this.textBoxX21.Text;
            List<string> cardPass=new List<string>();
            cardPass.Add(card1);
            cardPass.Add(card2);
            cardPass.Add(card3);
            cardPass.Add(card4);
            cardPass.Add(card5);
            cardPass.Add(card6);
            cardPass.Add(card7);
            cardPass.Add(card8);
            cardPass.Add(card9);
            cardPass.Add(card10);
            cardPass.Add(card11);
            cardPass.Add(card12);
            cardPass.Add(card13);
            cardPass.Add(card14);
            cardPass.Add(card15);
            cardPass.Add(card16);
            cardPass.Add(card17);
            cardPass.Add(card18);
            cardPass.Add(card19);
            cardPass.Add(card20);
            cardPass.Add(card21);
            cardPass.Add(card22);
            cardPass.Add(card23);
            cardPass.Add(card24);
            cardPass.Add(card25);
            StringBuilder sb=new StringBuilder();
            for (int i = 0; i < cardPass.Count; i++)
            {
                if (cardPass[i]=="")
                {
                    continue;
                }

                string str = "#" + (i + 1) + ":" + cardPass[i] + ",";
                sb.Append(str);
            }

            if (sb.Length>1)
            {
                sb.Length = sb.Length - 1;
            }

            my_info = sb.ToString();
            select_state = true;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }
    }
}
