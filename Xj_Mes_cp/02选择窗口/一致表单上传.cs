using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public partial class 一致表单上传 : Form
    {
        private string my_mate_id = "";
        public 一致表单上传(string mate_id)
        {
            my_mate_id = mate_id;
            InitializeComponent();
        }


        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            if (of.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            } 

            string fileName = of.FileName;

            this.pictureBox1.Image = Image.FromFile(fileName);

             baseImage = ImageHelper.ConvertImageToBase64(Image.FromFile(fileName));


         

        }
        string baseImage = "";
        private void buttonX2_Click(object sender, EventArgs e)
        {

            if (baseImage=="")
            {
                MessageBox.Show("请选择上传一致表单图片","系统提示"); return;
            }

            DataTable dt = ex.Get_Data(" [dbo].[cp_hp_0714_mate_image_info_insert]    '" + my_mate_id + "','图片','" + baseImage + "','"+base_info.user_code+"'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);

            baseImage = "";
            MessageBox.Show("上传成功");
        }

        private void 一致表单上传_Load(object sender, EventArgs e)
        {
            DataTable dt = ex.Get_Data(" [dbo].[cp_hp_0714_mate_image_info_select] '" + my_mate_id + "'");
            DtbToUi.DtbToDGV(dt, this.pwtDataGridView1);
            this.pwtDataGridView1.Columns["序号"].Visible = false;
            this.pwtDataGridView1.Columns["晶圆ID"].Visible = false;
            this.pwtDataGridView1.Columns["图片信息"].Visible = false;
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string image_txt = this.pwtDataGridView1.SelectedRows[0].Cells["图片信息"].Value.ToString();

            this.pictureBox1.Image = ImageHelper.ConvertBase64ToImage(image_txt);

        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


         ex.Exe_Data("[dbo].[cp_hp_0714_mate_image_info_delete]  '" + id + "','" + base_info.user_code + "'");

         this.pwtDataGridView1.Rows.Remove(this.pwtDataGridView1.SelectedRows[0]);
         MessageBox.Show("删除成功");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {

          


            this.Close();
        }
    }
}
