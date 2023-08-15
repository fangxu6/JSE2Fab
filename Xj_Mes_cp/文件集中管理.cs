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
    public partial class 文件集中管理 : Form
    {
        public 文件集中管理(string _type, string _id)
        {
            type = _type;
            id = _id;
            InitializeComponent();
        }

        private string type = "";
        private string id = "";
        db_deal ex = new db_deal();
        private void 文件集中管理_Load(object sender, EventArgs e)
        {

            DataTable dt = ex.Get_Data(" [dbo].[hp_file_save_list_select]   '" + type + "','" + id + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView2.SelectedRows.Count==0)
            {
                return;
            }
            this.pwtDataGridView2.Rows.Remove(this.pwtDataGridView2.SelectedRows[0]);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                this.buttonX2.Enabled = false;
                labelX1.Visible = true;
                for (int i = 0; i < this.pwtDataGridView2.Rows.Count; i++)
                {
                    string filename = "";
                    string resfilename = "";
                    string res = "";
                    pwt_file_manage.UpLoadFile("http://192.168.5.242:10000", this.pwtDataGridView2.Rows[i].Cells[1].Value.ToString(), out filename, out resfilename, out res);


                    string sql = "[dbo].[hp_file_save_list_insert] '" + type + "','" + id + "','" + filename + "','" + resfilename + "','" + base_info.user_code + "'";
                    ex.Exe_Data(sql);
                }

                DataTable dt = ex.Get_Data(" [dbo].[hp_file_save_list_select]   '" + type + "','" + id + "'");

                DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

                MessageBox.Show("上传成功", "系统提示");
            }
            finally
            {
                this.buttonX2.Enabled = true;
                labelX1.Visible = false;
            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()!= System.Windows.Forms.DialogResult.OK)
            {
                return;   
            }


            string[] file_list = openFileDialog1.FileNames;

            foreach (var item in file_list)
            {
              System.IO.FileInfo fi = new System.IO.FileInfo(item);
              this.pwtDataGridView2.Rows.Add();
              this.pwtDataGridView2.Rows[this.pwtDataGridView2.Rows.Count - 1].Cells[0].Value = fi.Name;
              this.pwtDataGridView2.Rows[this.pwtDataGridView2.Rows.Count - 1].Cells[1].Value = fi.FullName;

            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            ex.Exe_Data(" [dbo].[hp_file_save_list_by_id_delete] '" + id + "','" + base_info.user_code + "'");
            this.pwtDataGridView1.Rows.Remove(this.pwtDataGridView1.SelectedRows[0]);
            
        }

        private void 查看下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }
            pwt_file_manage.ShowFile("http://192.168.5.242:10000", this.pwtDataGridView1.SelectedRows[0].Cells["保存文件名称"].Value.ToString());
        }
    }
}
