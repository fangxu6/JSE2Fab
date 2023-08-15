using Pawote.UI.Controls;
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
    public partial class CP出货登记管理_显示信息 : Form
    {
        public CP出货登记管理_显示信息(string c_id, string c_lot, string c_p_lot, string c_post_info)
        {
            id = c_id;
            lot = c_lot;
            p_lot = c_p_lot;
            post_info = c_post_info;
            InitializeComponent();
        }
        public bool select_state = false;
        string id = "";
        string post_info = "";
        string lot = "";
        string p_lot = "";
        db_deal ex = new db_deal();
        private void CP出货登记管理_显示信息_Load(object sender, EventArgs e)
        {
            DataTable dt = ex.Get_Data("[dbo].[cp_20220307_send_info_info_select]  '" + id + "'");

            DtbToUi.DtbToDGV(dt, this.pwtDataGridView2);
        }
        
        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.labelX10.Visible = true;
            this.buttonX1.Enabled = false;

            try
            {

                List<string> is_post_send = new List<string>();
                for (int i = 0; i < this.pwtDataGridView2.Rows.Count; i++)
                {
                    string post_name = this.pwtDataGridView2.Rows[i].Cells["位号"].Value.ToString();


                    foreach (var item in post_name.Split('、'))
                    {

                        if (!is_post_send.Contains(item))
                        {
                            is_post_send.Add(item);
                        }
                    }
                }

                Application.DoEvents();

                this.pwtDataGridView1.Rows.Clear();
                string[] post_list = post_info.Split('、');



                DataTable new_dtb = new DataTable();


                this.progressBar1.Maximum = post_list.Length;
                this.progressBar1.Minimum = 0;

                int pro_no=0;
                foreach (var item in post_list)
                {
                    this.progressBar1.Value = pro_no;
                    pro_no++;
                    if (item == "")
                    {
                        continue;
                    }
                    //2022-05-08  更新数据
                    //hp_20220112_tsk_info_by_lot_post_select
                    //
                    DataTable post_info_dtb = ex.Get_Data("[dbo].[hp_20220112_tsk_info_by_lot_post_select_20220508]   '" + lot + "','" + item + "'");

                    //2022-03-25 取消直接赋值到 pwtDataGridView1
                    // add(post_info_dtb, this.pwtDataGridView1);

                    add(post_info_dtb, new_dtb);

                }
                this.progressBar1.Value = pro_no;


                ////按照批次查询   拆批次需要人员注意  看看根据实际情况
                //DataTable post_info_dtb = ex.Get_Data("[dbo].[hp_20220112_tsk_info_by_lot_post_select_20220508]   '" + lot + "',''");
                // new_dtb = post_info_dtb.Copy();



                new_dtb.DefaultView.Sort = "工序,位号 ASC";

                new_dtb = new_dtb.DefaultView.ToTable();

                DtbToUi.DtbToDGV(new_dtb, this.pwtDataGridView1, true);


                for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
                {
                    string new_post = this.pwtDataGridView1.Rows[i].Cells["位号"].Value.ToString();

                    if (is_post_send.Contains(new_post))
                    {
                        this.pwtDataGridView1.Rows[i].Cells["发货状态"].Value = "已发货";
                    }

                }


            }
            catch (Exception ex_info)
            {
                MessageBox.Show("数据加载错误:" + ex_info.Message.ToString(), "系统提示"); return;
            }
            finally
            {
                this.labelX10.Visible = false;
                this.buttonX1.Enabled = true;
            }


        }


        private void add(DataTable dt, PwtDataGridView dataGridView)
        {
           

            //当数据没有行是,则进行绑定
            if (dataGridView.Rows.Count == 0)
            {
                dataGridView.Columns.Clear();

                System.Windows.Forms.DataGridViewCheckBoxColumn CheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                CheckBox.HeaderText = "选择";
                CheckBox.Name = "选择";
                CheckBox.ReadOnly = false;


                dataGridView.Columns.Add(CheckBox);
                dataGridView.Columns[0].ReadOnly = false;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dataGridView.Columns.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
                    dataGridView.Columns[i+1].ReadOnly = true;
                }




                if (dt.Rows.Count==0)
                {
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dataGridView.Rows.Add();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dataGridView.Rows[i].Cells[j+1].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                    }

                }

 

            }
            else {
                if (dt.Rows.Count == 0)
                {
                    return;
                }
                //dataGridView.Rows.Add();
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[i+1].Value = dt.Rows[0][i].ToString();
                //}

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dataGridView.Rows.Add();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dataGridView.Rows[dataGridView.Rows.Count-1].Cells[j + 1].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                    }

                }
               
            }


            #region 添加选择控件及事件
            ContextMenuStrip Cms = new ContextMenuStrip();

            ToolStripMenuItem 全选 = new ToolStripMenuItem();
            全选.Text = "全选";
            ToolStripMenuItem 反选 = new ToolStripMenuItem();
            反选.Text = "反选";
            ToolStripMenuItem 清除 = new ToolStripMenuItem();
            清除.Text = "清除";

            全选.Click += (demo, e) =>
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = true;
                }
                
            };

            反选.Click += (demo, e) =>
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = !Convert.ToBoolean(dataGridView.Rows[i].Cells[0].Value);
                }
                
            };

            清除.Click += (demo, e) =>
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = false;
                }
               
            };

            Cms.Items.Add(全选);
            Cms.Items.Add(反选);
            Cms.Items.Add(清除);


            dataGridView.MouseClick += (demo, e) =>
            {
                if (dataGridView.SelectedRows.Count != 0)
                {
                    dataGridView.SelectedRows[0].Cells[0].Value = !Convert.ToBoolean(dataGridView.SelectedRows[0].Cells[0].Value);
                }
            };

            dataGridView.ContextMenuStrip = Cms;
            dataGridView.Columns[0].ReadOnly = false;
            #endregion


            if (dt.Columns.Contains("序号"))
            {
                dataGridView.Columns["序号"].Visible = false;
            }
        }

        private void add(DataTable dt, DataTable dataGridView)
        {


            //当数据没有行是,则进行绑定
            if (dataGridView.Rows.Count == 0)
            {
                dataGridView.Columns.Clear();

             


               

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dataGridView.Columns.Add(dt.Columns[i].Caption );
                    
                }




                if (dt.Rows.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dataGridView.Rows.Add();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dataGridView.Rows[i][j  ] = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                    }

                }



            }
            else
            {
                if (dt.Rows.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dataGridView.Rows.Add();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dataGridView.Rows[dataGridView.Rows.Count - 1][j  ] = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                    }

                }

            }

 
        }
        private void pwtSearchBox1_SearchBtnClick(object sender, EventArgs e)
        {
            选择信息窗口 mfrom = new 选择信息窗口("[dbo].[hp_cus_list_info_get_info_select] '收货信息' ", new List<int> {0, 1, 2, 3, 4 });
            mfrom.ShowDialog();

            if (mfrom.select_state == false)
            {
                return;
            }
            this.pwtSearchBox1.Text = mfrom.select_name[0];
            this.textBoxX1.Text = mfrom.select_name[1];
            this.textBoxX2.Text = mfrom.select_name[2];
            this.textBoxX3.Text = mfrom.select_name[3];
            this.textBoxX4.Text = mfrom.select_name[4];
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            string send_user = this.pwtSearchBox1.Text.ToString(); ;



            if (send_user=="")
            {
                MessageBox.Show("收货人信息不可以为空","系统提示"); return;
            }
            int select_no = 0;
            int gross_die = 0;
            int good_die = 0;

            



            List<string> post_list = new List<string>();
            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {
                if ((Convert.ToBoolean(pwtDataGridView1.Rows[i].Cells[0].Value) == true))
                {
                    select_no++;
                    gross_die += int.Parse(this.pwtDataGridView1.Rows[i].Cells["TotalDie"].Value.ToString());
                    good_die += int.Parse(this.pwtDataGridView1.Rows[i].Cells["PassDie"].Value.ToString());

                    string post_name = this.pwtDataGridView1.Rows[i].Cells["位号"].Value.ToString();
                    if (!post_list.Contains(post_name))
                    {
                        post_list.Add(post_name);
                    }


                    if ( this.pwtDataGridView1.Rows[i].Cells["发货状态"].Value.ToString()=="已发货")
                    {
                         MessageBox.Show("选择中位号存在已经发货", "系统提示");
                return;
                    }
                }
            }

            if (select_no == 0)
            {
                MessageBox.Show("未选择发料清单", "系统提示");
                return;
            }





            #region 正常位号简称

            List<int> post_list_sinple = new List<int>();

            foreach (var item in post_list)
            {
                post_list_sinple.Add(int.Parse(item));
            }
            string new_simple_string = "";

            foreach (var item in LotSelect.PinSimpleString(post_list_sinple))
            {
                new_simple_string += item + "；";
            }


            if (new_simple_string != "")
            {
                new_simple_string = new_simple_string.Substring(0, new_simple_string.Length - 1);
            }
            
            #endregion

           



            





            string post_send = "";
           
            foreach (var item in post_list)
            {
                post_send += item + "、";
            }
            if (post_send!="")
            {
                post_send = post_send.Substring(0, post_send.Length - 1);
            }


            string remark = this.textBoxX5.Text;


            string send_date = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string mode = this.textBoxX6.Text;
            string other_info = this.textBoxX7.Text;



            string sql = string.Format("[dbo].[cp_20220307_send_info_insert] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}'", id, post_send, post_list.Count.ToString(), gross_die, good_die, send_user, remark, lot, p_lot, new_simple_string, base_info.user_code, send_date,mode,other_info);
            ex.Exe_Data(sql);


            select_state = true;
            this.Close();
        }

       

        private void buttonX3_Click(object sender, EventArgs e)
        {
            select_state = true;
            this.Close();
        }
    }
}
