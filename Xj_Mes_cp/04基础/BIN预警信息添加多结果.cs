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
    public partial class BIN预警信息添加多结果 : Form
    {
        public BIN预警信息添加多结果()
        {
            InitializeComponent();
        }

        private void BIN预警信息添加多结果_Load(object sender, EventArgs e)
        {


            System.Windows.Forms.DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column1.HeaderText = "BIN名称";
            //Column1.Name = "BIN名称";
            //Column1.HeaderCell.Value = "BIN名称";
            Column1.ReadOnly = true;
            this.pwtDataGridView1.Columns.Add(Column1);



            DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn ComboBoxExColumn = new DevComponents.DotNetBar.Controls.DataGridViewComboBoxExColumn();
            ComboBoxExColumn.HeaderText = "类型";
            ComboBoxExColumn.Items.Add("大于");
            ComboBoxExColumn.Items.Add("大于登记");
            ComboBoxExColumn.Items.Add("登记");
            ComboBoxExColumn.Items.Add("小于");
            ComboBoxExColumn.Items.Add("小于登记");
           
            this.pwtDataGridView1.Columns.Add(ComboBoxExColumn);


            System.Windows.Forms.DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column2.HeaderText = "预警值";
            //Column2.Name = "BIN名称";
            //Column2.HeaderCell.Value = "BIN名称";
            this.pwtDataGridView1.Columns.Add(Column2);
            



            for (int i = 0; i < 128; i++)
            {
                this.pwtDataGridView1.Rows.Add();
                this.pwtDataGridView1.Rows[i].Cells[0].Value = "BIN-" + i.ToString(); ;

                this.pwtDataGridView1.Rows[i].Cells[2].Value = "0.0" ;
            }
        }


        public bool select_state = false;
        private void buttonX2_Click(object sender, EventArgs e)
        {
            select_state = false;
            this.Close();
        }

        public Dictionary<string, string> dic = new Dictionary<string, string>();
        private void buttonX1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < this.pwtDataGridView1.Rows.Count; i++)
            {


                if (this.pwtDataGridView1.Rows[i].Cells[1].Value==null)
                {
                    continue;
                }
                if (this.pwtDataGridView1.Rows[i].Cells[2].Value == null)
                {
                    continue;
                }


                string bin_name = this.pwtDataGridView1.Rows[i].Cells[0].Value.ToString();
                string type = this.pwtDataGridView1.Rows[i].Cells[1].Value.ToString();
                string type_value = this.pwtDataGridView1.Rows[i].Cells[2].Value.ToString();


                if (type_value == "0.0" || type_value=="0")
                {
                    continue;
                }


                if (type=="")
                {
                    continue;
                }


                dic.Add(bin_name, type + "|" + type_value);

            }
            select_state = true;
            this.Close();

        }
    }
}
