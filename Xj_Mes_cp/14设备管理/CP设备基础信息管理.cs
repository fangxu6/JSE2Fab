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
    public partial class CP设备基础信息管理 : DockContent
    {
        public CP设备基础信息管理()
        {
            InitializeComponent();
        }

        db_deal ex = new db_deal();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string iEQ_TYPE = this.pwtSearchBox1.Text;
            string iEQ_CODE = this.textBoxX1.Text;
            string iEQ_CUS = this.pwtSearchBox2.Text;
            string iEQ_ID = this.textBoxX2.Text;
            string iEQ_NAME = this.pwtSearchBox3.Text;
            string iEQ_MS = this.pwtSearchBox4.Text;


            string iIN_TIME = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string iTEST_TIME = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string iPRODUCT_TIME = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");

            string iPRODUCT_USER = this.textBoxX3.Text;
            string iPRODUCT_TEL = this.textBoxX4.Text;
            string iREMARK = this.textBoxX5.Text;

            string iOTHER = this.textBoxX6.Text;
            string iINFO1 = "";
            string iINFO2 = "";
            string iINFO3 = "";
            string ido_user = base_info.user_code;

            string icp_ft = "CP";


            string sql = string.Format("[dbo].[cp_eq_basic_info_select] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'",
                iEQ_TYPE, iEQ_CODE, iEQ_CUS, iEQ_ID, iEQ_NAME, iEQ_MS, iIN_TIME, iTEST_TIME, iPRODUCT_TIME, iPRODUCT_USER, iPRODUCT_TEL,
                iREMARK, iOTHER, iINFO1, iINFO2, iINFO3, ido_user, icp_ft);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("查询成功", "系统提示");
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {


         string iEQ_TYPE=this.pwtSearchBox1.Text;
         string iEQ_CODE = this.textBoxX1.Text;
         string iEQ_CUS=this.pwtSearchBox2.Text;
         string iEQ_ID = this.textBoxX2.Text;
         string iEQ_NAME=this.pwtSearchBox3.Text;
         string iEQ_MS=this.pwtSearchBox4.Text;


         string iIN_TIME=this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
         string iTEST_TIME=this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
         string iPRODUCT_TIME=this.dateTimePicker3.Value.ToString("yyyy-MM-dd");

         string iPRODUCT_USER=this.textBoxX3.Text;
         string iPRODUCT_TEL=this.textBoxX4.Text;
         string iREMARK=this.textBoxX5.Text;

         string iOTHER=this.textBoxX6.Text;
         string iINFO1 = this.pwtSearchBox5.Text;
         string iINFO2 = "";
         string iINFO3 = "";
         string ido_user=base_info.user_code;

         string icp_ft = "CP";



         DataTable eq_check = ex.Get_Data("[dbo].[cp_eq_basic_info_check_select] '" + icp_ft + "','" + iEQ_CODE + "',''");
         if (eq_check.Rows[0][0].ToString() == "1")
         {
             MessageBox.Show("设备编号已经存在", "系统提示"); return;
         }


         string sql = string.Format("[dbo].[cp_eq_basic_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}'",
             iEQ_TYPE ,iEQ_CODE,iEQ_CUS,iEQ_ID,iEQ_NAME,iEQ_MS,iIN_TIME,iTEST_TIME,iPRODUCT_TIME,iPRODUCT_USER,iPRODUCT_TEL,
             iREMARK, iOTHER, iINFO1, iINFO2, iINFO3, ido_user, icp_ft);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("添加成功", "系统提示");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            if (MessageBox.Show("确定修改选择的设备信息", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string iEQ_TYPE = this.pwtSearchBox1.Text;
            string iEQ_CODE = this.textBoxX1.Text;
            string iEQ_CUS = this.pwtSearchBox2.Text;
            string iEQ_ID = this.textBoxX2.Text;
            string iEQ_NAME = this.pwtSearchBox3.Text;
            string iEQ_MS = this.pwtSearchBox4.Text;


            string iIN_TIME = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string iTEST_TIME = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string iPRODUCT_TIME = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");

            string iPRODUCT_USER = this.textBoxX3.Text;
            string iPRODUCT_TEL = this.textBoxX4.Text;
            string iREMARK = this.textBoxX5.Text;

            string iOTHER = this.textBoxX6.Text;
            string iINFO1 = this.pwtSearchBox5.Text;
            string iINFO2 = "";
            string iINFO3 = "";
            string ido_user = base_info.user_code;

            string icp_ft = "CP";



            DataTable eq_check = ex.Get_Data("[dbo].[cp_eq_basic_info_check_select] '" + icp_ft + "','" + iEQ_CODE + "','"+id+"'");
            if (eq_check.Rows[0][0].ToString() == "1")
            {
                MessageBox.Show("设备编号已经存在", "系统提示"); return;
            }


            string sql = string.Format("[dbo].[cp_eq_basic_info_update] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}'",
              id,  iEQ_TYPE, iEQ_CODE, iEQ_CUS, iEQ_ID, iEQ_NAME, iEQ_MS, iIN_TIME, iTEST_TIME, iPRODUCT_TIME, iPRODUCT_USER, iPRODUCT_TEL,
                iREMARK, iOTHER, iINFO1, iINFO2, iINFO3, ido_user, icp_ft);
            DataTable dt = ex.Get_Data(sql);

            DtbToUi.DtbUpdateToDGV(dt, this.pwtDataGridView1);
            MessageBox.Show("修改成功", "系统提示");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("确定删除选择的设备信息","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!= System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();


            string sql = string.Format("[dbo].[cp_eq_basic_info_delete] '{0}','{1}'", id, base_info.user_code);
             ex.Get_Data(sql);

            DtbToUi.DtbDeleteToDGV(this.pwtDataGridView1);
            MessageBox.Show("删除成功", "系统提示");
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string iEQ_TYPE = this.pwtSearchBox1.Text = "";
            string iEQ_CODE = this.textBoxX1.Text = "";
            string iEQ_CUS = this.pwtSearchBox2.Text = "";
            string iEQ_ID = this.textBoxX2.Text = "";
            string iEQ_NAME = this.pwtSearchBox3.Text = "";
            string iEQ_MS = this.pwtSearchBox4.Text = "";


            //string iIN_TIME = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            //string iTEST_TIME = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");
            //string iPRODUCT_TIME = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");

            string iPRODUCT_USER = this.textBoxX3.Text = "";
            string iPRODUCT_TEL = this.textBoxX4.Text = "";
            string iREMARK = this.textBoxX5.Text = "";

            string iOTHER = this.textBoxX6.Text = "";
            string iINFO1 = "";
            string iINFO2 = "";
            string iINFO3 = "";
            string ido_user = base_info.user_code;
        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }
            string iEQ_TYPE = this.pwtSearchBox1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["设备类型"].Value.ToString();
            string iEQ_CODE = this.textBoxX1.Text = this.pwtDataGridView1.SelectedRows[0].Cells["设备编号"].Value.ToString();
            string iEQ_CUS = this.pwtSearchBox2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["设备供应商"].Value.ToString();
            string iEQ_ID = this.textBoxX2.Text = this.pwtDataGridView1.SelectedRows[0].Cells["设备序列号"].Value.ToString();
            string iEQ_NAME = this.pwtSearchBox3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["设备型号"].Value.ToString();
            string iEQ_MS = this.pwtSearchBox4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["设备性质"].Value.ToString();


            this.dateTimePicker1.Value = Convert.ToDateTime(this.pwtDataGridView1.SelectedRows[0].Cells["入场时间"].Value.ToString());
            this.dateTimePicker2.Value = Convert.ToDateTime(this.pwtDataGridView1.SelectedRows[0].Cells["调试时间"].Value.ToString());
            this.dateTimePicker3.Value = Convert.ToDateTime(this.pwtDataGridView1.SelectedRows[0].Cells["生产时间"].Value.ToString());

            string iPRODUCT_USER = this.textBoxX3.Text = this.pwtDataGridView1.SelectedRows[0].Cells["安装人员"].Value.ToString();
            string iPRODUCT_TEL = this.textBoxX4.Text = this.pwtDataGridView1.SelectedRows[0].Cells["人员联系电话"].Value.ToString();
            string iREMARK = this.textBoxX5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["备注"].Value.ToString();

            string iOTHER = this.textBoxX6.Text = this.pwtDataGridView1.SelectedRows[0].Cells["功能配置"].Value.ToString();
            string iINFO1 = this.pwtSearchBox5.Text = this.pwtDataGridView1.SelectedRows[0].Cells["INFO1"].Value.ToString();
            string iINFO2 = "";
            string iINFO3 = "";
            string ido_user = base_info.user_code;
        }

        private void CP设备基础信息管理_Load(object sender, EventArgs e)
        {

        }

        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mf = new 选择信息窗口("[dbo].[cp_eq_basic_info_basic_info_select] '设备类型'", new List<int> { 0 });
            mf.ShowDialog();
            if (mf.select_state==false)
            {
                return;
            }

            this.pwtSearchBox1.Text = mf.select_name[0];
        }

        private void pwtSearchBox2_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mf = new 选择信息窗口("[dbo].[cp_eq_basic_info_basic_info_select] '设备供应商'", new List<int> { 0 });
            mf.ShowDialog();
            if (mf.select_state == false)
            {
                return;
            }

            this.pwtSearchBox2.Text = mf.select_name[0];
        }

        private void pwtSearchBox3_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mf = new 选择信息窗口("[dbo].[cp_eq_basic_info_basic_info_select] '设备型号'", new List<int> { 0 });
            mf.ShowDialog();
            if (mf.select_state == false)
            {
                return;
            }

            this.pwtSearchBox3.Text = mf.select_name[0];
        }

        private void pwtSearchBox4_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mf = new 选择信息窗口("[dbo].[cp_eq_basic_info_basic_info_select] '设备性质'", new List<int> { 0 });
            mf.ShowDialog();
            if (mf.select_state == false)
            {
                return;
            }

            this.pwtSearchBox4.Text = mf.select_name[0];
        }

        private void 查看上传附件文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            string id = this.pwtDataGridView1.SelectedRows[0].Cells["序号"].Value.ToString();

            文件集中管理 mform = new 文件集中管理("CP设备基础附加文件", id);
            mform.ShowDialog();
        }

    
    }
}
