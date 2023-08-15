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
    public partial class 工站组 : DockContent
    {
        db_deal ex = new db_deal();
        public 工站组()
        {
            InitializeComponent();
            string sql = "czj_sys_system_basic_info_station_select '工序'";
            DataTable dt = ex.Get_Data(sql);
            combox_databind(comboBoxEx1, dt);
        }
        private void combox_databind(ComboBox combo, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                combo.Items.Add(dt.Rows[i][0]);
            }

        }

        private void 工站组_Load(object sender, EventArgs e)
        {

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sname = textBoxX1.Text;
            string sql = "czj_xj_station_team_info_select '"+sname+"'";


            DataTable dt = ex.Get_Data(sql);
            Comm_Class.DtbToDGV(dt, pwtDataGridView1);
            this.pwtDataGridView1.Columns["序号"].Visible = false;
            
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string sname = textBoxX1.Text;
            string iuser = base_info.user_name;
            string itime = DateTime.Now.ToString();
            string bz = textBoxX2.Text;
            if (sname == "")
            {
                MessageBox.Show("工站组名称不能为空");
                return;
            }
            DataTable dd = ex.Get_Data("czj_xj_station_team_info_check_select '" + sname + "'");
            if (dd.Rows.Count>0)
            {
                MessageBox.Show("工站组名称重复","系统提示");
                return;
            }
            string sql = "czj_xj_station_team_info_install '"+sname+"','"+ bz + "','"+iuser+"','"+itime+"'";
            DataTable dt = ex.Get_Data(sql);
            Comm_Class.Gridview_add_row(dt, pwtDataGridView1);
            MessageBox.Show("添加成功");
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            pwtDataGridView1.Rows.Clear();
            textBoxX1.Focus();
        }

      
       

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择你需要操作的行");
                return;
            }

            string ssname = this.pwtDataGridView1.SelectedRows[0].Cells["工站组名称"].Value.ToString();

            string sql = "czj_xj_station_team_info_delect'"+ssname+ "'";
            if (MessageBox.Show("你确定要删除吗？"
               , "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ex.Exe_Data(sql);
                this.pwtDataGridView1.Rows.RemoveAt(this.pwtDataGridView1.SelectedRows[0].Index);
                MessageBox.Show("删除成功");
            }
            pwtDataGridView2.Rows.Clear();

        }
       
        private void buttonX9_Click(object sender, EventArgs e)
        {


            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string gname = comboBoxEx1.Text;
            string iuser = base_info.user_name;
            string itime = DateTime.Now.ToString();
            if (gname == "")
            {
                MessageBox.Show("工序名称不能为空");
                return;
            }

            string ssname = this.pwtDataGridView1.SelectedRows[0].Cells["工站组名称"].Value.ToString();

            DataTable dd = ex.Get_Data("czj_xj_station_team_info_gname_select2 '" + ssname + "','"+gname+"'");
            if (dd.Rows.Count > 0)
            {
                MessageBox.Show("工站组名称重复");
                return;
            }
            DataTable ds = ex.Get_Data("czj_xj_station_team_info_gname_select '"+ssname+"'");
            int a = ds.Rows.Count+1;
           
                string sql = "czj_xj_station_team_info_install2 '" + gname + "','" + ssname + "','" + iuser + "','" + itime + "','" + a + "'";
                DataTable dt = ex.Get_Data(sql);
                Comm_Class.Gridview_add_row(dt, pwtDataGridView2);
                MessageBox.Show("添加成功","系统提示");
            
            pwtDataGridView2.CurrentCell = pwtDataGridView2.Rows[pwtDataGridView2.Rows.Count-1].Cells[0];

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (pwtDataGridView2.Rows.Count == 0)
            {
                return;
            }

            string ssname = this.pwtDataGridView1.SelectedRows[0].Cells["工站组名称"].Value.ToString();

            string xname = this.pwtDataGridView2.SelectedRows[0].Cells["工序名称"].Value.ToString();


            string sql = "czj_xj_station_team_info_delect1 '" + ssname + "','" + xname + "'";

            if (MessageBox.Show("你确定要删除吗？"
              , "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ex.Exe_Data(sql);
                this.pwtDataGridView2.Rows.RemoveAt(this.pwtDataGridView2.SelectedRows[0].Index);
                MessageBox.Show("删除成功");
            }


        }
        

        private void buttonX5_Click(object sender, EventArgs e)
        {
            comboBoxEx1.Items.Clear();
            string sql = "czj_sys_system_basic_info_station_select '工序'";
            DataTable dt = ex.Get_Data(sql);
            combox_databind(comboBoxEx1, dt);
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {

            if (this.pwtDataGridView2.Rows.Count==0)
            {
                return;
            }
            for (int i = 0; i < this.pwtDataGridView2.Rows.Count; i++)
            {

                
            }
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }


            string process_name = this.pwtDataGridView1.SelectedRows[0].Cells["工站组名称"].Value.ToString();



            DataTable dt = ex.Get_Data(" [dbo].[czj_xj_station_team_info_prcess_name_select]   '" + process_name + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);
        }

        private void pwtDataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView2.SelectedRows.Count==0)
            {
                return;
            }

            string id = this.pwtDataGridView2.SelectedRows[0].Cells["序号"].Value.ToString();
            string p_no = this.pwtDataGridView2.SelectedRows[0].Cells["排序"].Value.ToString();
            输入信息单选择框 mfrom = new 输入信息单选择框("请输入排序序号",p_no);
            mfrom.ShowDialog();


            if (mfrom.select_state!=true)
            {
                return;
            }


            string new_pn = mfrom.select_name;


            ex.Exe_Data("[dbo].[czj_xj_station_team_info_prcess_name_update]    '" + id + "','" + new_pn + "'");
            this.pwtDataGridView2.SelectedRows[0].Cells["排序"].Value = new_pn;
            MessageBox.Show("排序序号修改完成");
        }
    }
}
