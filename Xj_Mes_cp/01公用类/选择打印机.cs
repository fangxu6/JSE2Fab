using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Xj_Mes_cp
{
    public partial class 选择打印机 : Form
    {
        public 选择打印机()
        {
            System.Drawing.Printing.PrintDocument print = new System.Drawing.Printing.PrintDocument();
            string sDefault = print.PrinterSettings.PrinterName;//默认打印机名
            //设置默认打印机
            DefaultPrintMac = sDefault;


            InitializeComponent();
        }

        private void 选择打印机_Load(object sender, EventArgs e)
        {
            GetLocalPrinters();
        }

        /// <summary>
        /// 默认打印机
        /// </summary>
        public string DefaultPrintMac = "";

        public bool select_state = false;

        public void GetLocalPrinters()
        {

            this.dataGridView1.Rows.Clear();



            label3.Text = DefaultPrintMac;
           


            foreach (string sPrint in System.Drawing.Printing.PrinterSettings.InstalledPrinters)//获取所有打印机名称
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0].Value = sPrint;
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            //设置选择打印机
            DefaultPrintMac = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            select_state = true;
            this.Close();
        }
    }
}
