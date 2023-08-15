using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pawote.UI.Controls;

namespace Xj_Mes_cp
{
    public partial class 工序组选择 : Form
    {
        public 工序组选择(PwtSearchBox pwtSearchBox11)
        {
            pwtSearchBox1 = pwtSearchBox11;
            InitializeComponent();
        }
        db_deal ex = new db_deal();

        private PwtSearchBox pwtSearchBox1;


        public string select_ok = "1";
        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.pwtDataGridView1.Columns.Clear();
            DataTable dtb = ex.Get_Data("W_czj_station_team_info_select'" + textBoxX1.Text.Trim()+"','"+textBoxX2.Text.Trim()+"','"+textBoxX4.Text.Trim()+"'");
            this.pwtDataGridView1.DataSource = dtb;
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }



            pwtSearchBox1.Text = this.pwtDataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            

            this.Close();
        }

        private void pwtDataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            this.pwtDataGridView2.Columns.Clear();
            string group_name = this.pwtDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            DataTable dtb = ex.Get_Data("[dbo].[W_czj_station_team_info_TIME_select] '" + group_name + "'");
            this.pwtDataGridView2.DataSource = dtb;

            this.pwtDataGridView2.Columns["ID"].Visible = false;
            
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void 工序组选择_Load(object sender, EventArgs e)
        {
            buttonX1_Click(sender, e);
        }
    }
}
