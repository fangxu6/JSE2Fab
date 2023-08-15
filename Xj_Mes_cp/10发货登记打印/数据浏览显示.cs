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
    public partial class 数据浏览显示 : Form
    {
        public 数据浏览显示(DataTable dt)
        {
            my_dtb = dt;
            InitializeComponent();
        }

        private DataTable my_dtb = new DataTable();
        private void 数据浏览显示_Load(object sender, EventArgs e)
        {


            this.pwtDataGridView1.DataSource = my_dtb;


            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {

                if (this.pwtDataGridView1.Rows[i].Cells["GROSS DIES"].Value.ToString()=="")
                {
                    continue;
                }
                if (this.pwtDataGridView1.Rows[i].Cells["GOOD DIES"].Value.ToString() == "")
                {
                    continue;
                }
                if (this.pwtDataGridView1.Rows[i].Cells["DATE"].Value.ToString() == "")
                {
                    continue;
                }

                try
                {
                    Convert.ToDateTime(this.pwtDataGridView1.Rows[i].Cells["DATE"].Value.ToString());
                }
                catch (Exception error)
                {
                    this.pwtDataGridView1.Rows[i].Cells["DATE"].Style.BackColor = Color.Red;
                }

                try
                {
                    Convert.ToInt32(this.pwtDataGridView1.Rows[i].Cells["GROSS DIES"].Value.ToString());
                }
                catch (Exception error)
                {
                    this.pwtDataGridView1.Rows[i].Cells["GROSS DIES"].Style.BackColor = Color.Red;
                }

                try
                {
                    Convert.ToInt32(this.pwtDataGridView1.Rows[i].Cells["GOOD DIES"].Value.ToString());
                }
                catch (Exception error)
                {
                    this.pwtDataGridView1.Rows[i].Cells["GOOD DIES"].Style.BackColor = Color.Red;
                }
            }
        }

        public bool select_state = false;
        private void buttonX1_Click(object sender, EventArgs e)
        {
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
