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
    public partial class 客户信息 : DockContent
    {
        public 客户信息()
        {
            InitializeComponent();
          
        }
        private void combox_databind(ComboBox combo, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                combo.Items.Add(dt.Rows[i][0]);
            }

        }
        private void 客户1_Load(object sender, EventArgs e)
        {
            string sql = "czj_sys_system_basic_info_select'客户类型'";
            DataTable dt = ex.Get_Data(sql);
            combox_databind(comboBoxEx1, dt);

            this.comboBoxEx1.SelectedIndex = 0;
            this.comboBoxEx2.SelectedIndex = 0;
        }
        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string CUS_TYPE = this.comboBoxEx2.SelectedItem.ToString();
            string CUS_NAME = this.textBoxX1.Text.ToString();
            string CUS_CODE = this.textBoxX2.Text.ToString();
            
            string sql = "czj_xj_customer_info_select01 '" + CUS_NAME + "','" + CUS_CODE + "','" + CUS_TYPE + "'";
            DataTable dt = ex.Get_Data(sql);
            Comm_Class.DtbToDGV(dt, this.pwtDataGridView1);
            this.pwtDataGridView1.Columns["序号"].Visible = false;

        }
        string custcode;
        private void pwtDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            custcode = pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string sql = "czj_xj_customer_contacts_info_select'" + custcode + "'";
            DataTable td = ex.Get_Data(sql);
            Comm_Class.DtbToDGV(td, this.pwtDataGridView2);
            this.pwtDataGridView2.Columns["序号"].Visible = false;

        }
      
        private void clear()
        {
            textBoxX1.Text = "";
            textBoxX2.Text = "";
            textBoxX5.Text = "";
            comboBoxEx2.SelectedIndex = 0;
            textBoxX9.Text = "";
            comboBoxEx1.SelectedIndex = 0;
            textBoxX1.Focus();
          
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string cname = textBoxX1.Text;
            string ccode = textBoxX2.Text;
            string jc = textBoxX5.Text;
            string jb = this.comboBoxEx2.SelectedItem.ToString();
            string iuser = base_info.user_name;
            string bz = textBoxX9.Text;


            if (cname =="")
            {
                MessageBox.Show("客户名称不能为空", "系统提示");
                return;
            }
            if (ccode == "")
            {
                MessageBox.Show("客户编码不能为空", "系统提示");
                return;
            } 
            if (comboBoxEx1.SelectedIndex == -1)
            {
                MessageBox.Show("客户类型不能为空", "系统提示");
                return;
            }


            DataTable dd = ex.Get_Data("czj_xj_customer_info_select_custcode01 '" + ccode + "','" + jb + "'");
            if (dd.Rows[0][0].ToString()=="NG")
            {
                MessageBox.Show("客户编码已存在","系统提示");
                return;
            }


            string ctype = comboBoxEx1.Text;
            string sql = "czj_xj_customer_info_install '"+cname+"','"+ccode+"','"+ctype+"','"+jc+"','"+jb+"','"+iuser+"','"+bz+"'";
            DataTable dt = ex.Get_Data(sql);  


            Comm_Class.Gridview_add_row(dt, pwtDataGridView1);


            this.pwtDataGridView1.Columns["序号"].Visible = false;


            MessageBox.Show("添加成功", "系统提示");
           // clear();
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            clear();
            pwtDataGridView1.Rows.Clear();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择你需要操作的行", "系统提示");
                return;
            }

            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string cname = textBoxX1.Text;
            string ccode = textBoxX2.Text;
            string jc = textBoxX5.Text;
            string jb = this.comboBoxEx2.SelectedItem.ToString();
            string bz = textBoxX9.Text;
            string ctype = comboBoxEx1.SelectedItem.ToString();
            string iuser = base_info.user_name;
            string itmer = DateTime.Now.ToString();
            if (cname=="")
            {
                MessageBox.Show("客户名称不能为空", "系统提示");
                return;
            }
            if (ccode == "")
            {
                MessageBox.Show("客户编码不能为空", "系统提示");
                return;
            }
            if (jc == "")
            {
                MessageBox.Show("客户简称不能为空", "系统提示");
                return;
            }
            if (MessageBox.Show("确定对客户信息进行修改", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            ex.Exe_Data("czj_xj_customer_info_updata '"+iid+"','"+cname+"','"+ccode+"','"+ctype+"','"+jc+"','"+jb+"','"+iuser+"','"+itmer+"','"+bz+"'");
            String[] data =
             {
                cname,ccode,jc,ctype,bz,jb,iuser,itmer,iid
            };
            Comm_Class.gridview_update_row(data, this.pwtDataGridView1);
            this.pwtDataGridView1.Columns["序号"].Visible = false;
            MessageBox.Show("修改成功", "系统提示");
        }
       
        private void pwtDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            textBoxX1.Text = pwtDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBoxX2.Text = pwtDataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBoxX5.Text = pwtDataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            this.comboBoxEx2.SelectedItem = pwtDataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBoxX9.Text = pwtDataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBoxEx1.Text = pwtDataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void r(object sender, EventArgs e)
        {

        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选中你需要操作的行", "系统提示");
                return;
            }

            string iid = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            string sql = "czj_xj_customer_info_delect'" + iid + "'";
            if (MessageBox.Show("你确定要删除吗？:"
               , "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ex.Exe_Data(sql);
                this.pwtDataGridView1.Rows.RemoveAt(this.pwtDataGridView1.SelectedRows[0].Index);
                MessageBox.Show("删除成功", "系统提示");
                pwtDataGridView2.Rows.Clear();
            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            string lxr = textBoxX3.Text;
            string lxhm = textBoxX4.Text;
            string address = textBoxX7.Text;
            string bz = textBoxX8.Text;
            string iuser = base_info.user_name;
            if (lxr == "")
            {
                MessageBox.Show("联系人不能为空");
                return;
            }
            if (lxhm=="")
            {
                MessageBox.Show("联系号码不能为空");
                return;
            }
            if (address=="")
            {
                MessageBox.Show("地址不能为空");
                return;
            }

            string custcode_id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();
            //DataTable dd = ex.Get_Data("czj_xj_customer_contacts_info_address_select'" + address + "','"+lxr+"'");
            //if (dd.Rows[0][0].ToString() == "NG")
            //{
            //    MessageBox.Show("同一联系人不能有相同地址");
            //    return;
            //}

            string  sql = "czj_xj_customer_contacts_info_install '"+lxr+"','"+lxhm+"','"+custcode_id+"','"+address+"','"+bz+"','"+iuser+"'";
            DataTable dt = ex.Get_Data(sql);
            Comm_Class.Gridview_add_row(dt, pwtDataGridView2);
            this.pwtDataGridView2.Columns["序号"].Visible = false;
            MessageBox.Show("添加联系人成功");
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            textBoxX7.Text = "";
            textBoxX8.Text = "";
          
        }
       
        private void pwtDataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            textBoxX3.Text = pwtDataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            textBoxX4.Text = pwtDataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            textBoxX7.Text = pwtDataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            textBoxX8.Text = pwtDataGridView2.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择你需要操作的行");
                return;
            }

            string lxr = textBoxX3.Text;
            string lxhm = textBoxX4.Text;
            string address = textBoxX7.Text;
            string bz = textBoxX8.Text;
            string iuser = base_info.user_name;
            string itimer = DateTime.Now.ToString();
            if (lxr == "")
            {
                MessageBox.Show("联系人不能为空");
                return;
               
            }
            if (lxhm == "")
            {
                MessageBox.Show("联系号码不能为空");
                return;
            }
            if (address == "")
            {
                MessageBox.Show("地址不能为空");
                return;
            }

            string idd = this.pwtDataGridView2.SelectedRows[0].Cells["序号"].Value.ToString();
            DataTable dd = ex.Get_Data("czj_xj_customer_contacts_info_address_select'" + address + "','" + lxr + "'");
           
            ex.Exe_Data("czj_xj_customer_contacts_info_updata'"+idd+"','"+lxr+"','"+lxhm+"','"+address+"','"+bz+"','"+iuser+"','"+itimer+"'");
            String[] data1 =
            {
                lxr,lxhm,address,bz,custcode,iuser,itimer,idd
            };
            Comm_Class.gridview_update_row(data1, this.pwtDataGridView2);
            this.pwtDataGridView2.Columns["序号"].Visible = false;
            MessageBox.Show("修改成功");
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            
            if (this.pwtDataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选中你需要操作的行");
                return;
            }

            if (this.pwtDataGridView2.SelectedRows.Count==0)
            {
                return;
            }
            string idd = pwtDataGridView2.SelectedRows[0].Cells["序号"].Value.ToString();
           
            string sql = "czj_xj_customer_contacts_info_delect'" + idd + "'";
            if (MessageBox.Show("你确定要删除吗？"
               , "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ex.Exe_Data(sql);
                this.pwtDataGridView2.Rows.RemoveAt(this.pwtDataGridView2.SelectedRows[0].Index);
                MessageBox.Show("删除成功");
            }
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            textBoxX3.Text = "";
            textBoxX4.Text = "";
            textBoxX7.Text = "";
            textBoxX8.Text = "";
            pwtDataGridView2.Rows.Clear();
        }
    }
}
