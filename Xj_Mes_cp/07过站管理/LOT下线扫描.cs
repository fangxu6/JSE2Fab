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
    public partial class LOT下线扫描 : DockContent
    {
        public LOT下线扫描()
        {
            InitializeComponent();
        }
        db_deal ex = new db_deal();
        private void LOT上线扫描_Load(object sender, EventArgs e)
        {


          
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string LOT_ONLY_CODE = this.textBoxX1.Text.Trim();
            DataTable dt = ex.Get_Data("[dbo].[hp_1022_cp_up_line_select] '" + LOT_ONLY_CODE + "'");

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("流程卡不存在,请确认！", "系统提示"); return;
            }

            this.textBoxX3.Text = dt.Rows[0]["客户代码"].ToString();
            this.textBoxX4.Text = dt.Rows[0]["客户名称"].ToString();
            this.textBoxX5.Text = dt.Rows[0]["LOT"].ToString();
            this.textBoxX6.Text = dt.Rows[0]["产品型号"].ToString();
            this.textBoxX7.Text = dt.Rows[0]["版本"].ToString();
            this.textBoxX8.Text = dt.Rows[0]["数量"].ToString();
            this.textBoxX9.Text = dt.Rows[0]["位号"].ToString();




            DataTable dteq = ex.Get_Data("[dbo].[hp_1022_cp_up_line_info_get_eq_select] '" + LOT_ONLY_CODE + "'");

            if (dteq.Rows.Count == 0)
            {
                MessageBox.Show("流程卡未上线,无法进行下线扫描", "系统提示"); return;
            }
            List<string> list_eq = new List<string>();
            for (int i = 0; i < dteq.Rows.Count; i++)
            {
                list_eq.Add(dteq.Rows[i][0].ToString());
            }


            LOT下线扫描_TSK采集 mfrom = new LOT下线扫描_TSK采集(list_eq, dt.Rows[0]["LOT"].ToString(), LOT_ONLY_CODE);
            mfrom.ShowDialog();

        }

        private void textBoxX4_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string lot_only_code = this.textBoxX1.Text;
            string do_people = this.textBoxX2.Text;

            string cus_code = this.textBoxX3.Text;
            string cus_name = this.textBoxX4.Text;

            string lot = this.textBoxX5.Text;
            string mate_name = this.textBoxX6.Text;

            string mate_ves = this.textBoxX7.Text;
            string post_info = this.textBoxX9.Text;
            string post_number = this.textBoxX8.Text;



            if (lot == "" || lot_only_code == "")
            {
                MessageBox.Show("请先上传数据,再进行点击下线操作"); return;
            }


            string sql = string.Format("[dbo].[hp_1022_cp_down_line_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'"
                , lot_only_code, cus_code, cus_name, lot, mate_name, mate_ves, post_number, post_info, do_people, "", "", "", base_info.user_code);

            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);




            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";

            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";

            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";

            this.textBoxX7.Text = "";
            this.textBoxX9.Text = "";
            this.textBoxX8.Text = "";
            this.textBoxX1.Focus();
         
        }

        private void textBoxX5_TextChanged(object sender, EventArgs e)
        {
            
        }

       

        
    }
}
