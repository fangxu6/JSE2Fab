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
    public partial class 选择信息窗口_新 : Form
    {
        private string my_str_sql = "";
        private List<int> my_select_index = null; 
        private List<string> my_select_condition = null;
        public List<string> select_name = new List<string>();
        public bool select_state = false;
        db_deal ex = new db_deal();
        DataTable dtb = new DataTable();
        public 选择信息窗口_新(string str_sql, List<int> select_index,List<string> select_condition)
        {
            InitializeComponent(); 
            my_str_sql = str_sql;
            my_select_index = select_index;
            my_select_condition = select_condition;
        }

        private void 选择信息窗口_新_Load(object sender, EventArgs e)
        {
            if (my_select_condition.Count<=0)
            {
                dtb = ex.Get_Data(my_str_sql);
            }
            else
            {
                StringBuilder sb=new StringBuilder();
                for (int i = 0; i < my_select_condition.Count; i++)
                {
                    string str = my_select_condition[i];
                    sb.Append("\'" + str + "\',");
                }

                if (sb.Length>1)
                {
                    sb.Length = sb.Length - 1;
                }
                dtb = ex.Get_Data(my_str_sql+" "+sb.ToString());
            }









            

            Application.DoEvents();
            // Comm_Class.DtbToDGV(dtb, this.pwtDataGridView1);
            this.pwtDataGridView1.DataSource = dtb;

            for (int i = 0; i < this.pwtDataGridView1.Columns.Count; i++)
            {
                if (this.pwtDataGridView1.Columns[i].HeaderText == "序号")
                {
                    this.pwtDataGridView1.Columns[i].Visible = false;
                }
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            string info = this.textBoxX1.Text.ToString();
            if (info == "")
            {
                this.pwtDataGridView1.DataSource = dtb;
                return;
            }

            this.pwtDataGridView1.DataSource = search(dtb, info);
        }
        private DataTable search(DataTable new_dtb, string text)
        {

            DataTable dtb_temp = new DataTable();


            for (int i = 0; i < new_dtb.Columns.Count; i++)
            {
                dtb_temp.Columns.Add(new_dtb.Columns[i].ToString(), typeof(string));
            }


            for (int i = 0; i < new_dtb.Rows.Count; i++)
            {
                dtb_temp.Rows.Add();
                for (int j = 0; j < new_dtb.Columns.Count; j++)
                {
                    dtb_temp.Rows[i][j] = new_dtb.Rows[i][j].ToString();
                }

            }


            string temp = " ";
            for (int i = 0; i < new_dtb.Columns.Count; i++)
            {
                temp += " `" + new_dtb.Columns[i].ToString() + "` like '%" + text + "%' or";
            }

            if (temp != "")
            {
                temp = temp.Substring(0, temp.Length - 2);
            }


            DataRow[] drArr1 = dtb_temp.Select(temp);
            DataTable dtNew = new_dtb.Clone();
            for (int i = 0; i < drArr1.Length; i++)
            {
                dtNew.ImportRow(drArr1[i]); //ImportRow 是复制

            }
            return dtNew;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            pwtDataGridView1_MouseDoubleClick(null, null);
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择需要使用的信息", "系统提示");
                return;
            }

            foreach (var item in my_select_index)
            {
                select_name.Add(this.pwtDataGridView1.SelectedRows[0].Cells[item].Value.ToString());
            }
            select_state = true;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }

        private void textBoxX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                buttonX3_Click(null, null);
            }
        }
    }
}
