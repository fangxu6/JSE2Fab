using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class TSK片数统计 : DockContent
    {
        public TSK片数统计()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string dat1 = this.dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string dat2 = this.dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss");

            string lot = this.textBoxX1.Text;
            string eq = this.textBoxX2.Text;
            string process_name = this.textBoxX3.Text;

           

            db_deal EX=new db_deal ();
            string sql = string.Format("[dbo].[hp_1026_tsk_number_collect_select] '{0}','{1}','{2}','{3}','{4}'",lot,eq,process_name,dat1,dat2);
            DataTable dt = EX.Get_Data(sql);

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
        }
    }
}
