using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MES_TMS_IF
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void ultraChart1_ChartDataClicked(object sender, Infragistics.UltraChart.Shared.Events.ChartDataEventArgs e)
        {

        }

        private void ultraPanel1_ClickClient(object sender, EventArgs e)
        {
     
        }

        private void ultraPanel1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void ultraPanel1_MouseDownClient(object sender, MouseEventArgs e)
        {
        }

        private void ultraPanel1_Click(object sender, EventArgs e)
        { 
            Console.WriteLine(DateTime.Now);
        }
    }
}
