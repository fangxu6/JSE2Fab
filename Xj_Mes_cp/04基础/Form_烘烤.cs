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
    public partial class Form_烘烤 : Form
    {
        private string id = "";
        string mate_name = "";
        string mate_ves = "";
        string process_name = "";

        public Form_烘烤(string iid, string imate_name, string imate_ves, string iprocess_name)
        {
            id = iid;
            mate_name = imate_name;
            mate_ves = imate_ves;
            process_name = iprocess_name;
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void Form_CP_Load(object sender, EventArgs e)
        {
            this.labelX8.Text = mate_name;
            this.labelX9.Text = mate_ves;
            this.labelX10.Text = process_name;



            DataTable dt = ex.Get_Data("[dbo].[hp_1012_W_Wafer_station_info_select] '" + id + "'");
            if (dt.Rows.Count!=0)
            {
                this.textBoxX1.Text = dt.Rows[0]["info10"].ToString();
                this.textBoxX2.Text = dt.Rows[0]["info11"].ToString();
                this.textBoxX3.Text = dt.Rows[0]["Name"].ToString();
            }
           

        }
        public bool select_state = false;
        public string program_send = "";
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string pian = this.textBoxX1.Text;
            string pici = this.textBoxX2.Text;
            string program = this.textBoxX3.Text;


            string sql = string.Format(" [dbo].[hp_1012_W_Wafer_station_info_update]   '{0}','{1}','{2}','{3}','','','','','','','','',''", id, program, pian, pici);

            select_state = true;
            ex.Exe_Data(sql);
            program_send = program;
            this.Close();
        }

        private void labelX2_Click(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
