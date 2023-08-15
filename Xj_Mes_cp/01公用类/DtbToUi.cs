using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Pawote.UI.Controls;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public class DtbToUi
    {

        public static void DtbDeleteToDGV(PwtDataGridView Dgv)
        {
            Dgv.Rows.Remove(Dgv.SelectedRows[0]);
        }

        public static void DtbUpdateToDGV(DataTable dt, PwtDataGridView Dgv)
        {
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("返回无数据", "系统提示");
            }
            else
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    Dgv.SelectedRows[0].Cells[i].Value = dt.Rows[0][i].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();
                }
            }
        }
        public static Boolean DtbAddToDGV(DataTable dt, PwtDataGridView dataGridView)
        {
            //当数据没有行是,则进行绑定
            if (dataGridView.Rows.Count == 0)
            {
                DtbToDGV(dt, dataGridView);
                return true;
            }
            dataGridView.Rows.Add();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[i].Value = dt.Rows[0][i].ToString();
            }
            if (dt.Columns.Contains("序号"))
            {
                dataGridView.Columns["序号"].Visible = false;
            }
            return true;
        }
        /// <summary>
        /// dt输出ComboBoxEx
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Cbbox"></param>
        /// <param name="index"></param>
        public static void DtbToComboBoxEx(DataTable dt, ComboBoxEx Cbbox, int index = 0)
        {
            Cbbox.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Cbbox.Items.Add(dt.Rows[i][index].ToString());
            }
        }

      

        /// <summary>
        /// dt输出PwtDataGridView
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Dgv"></param>
        public static void DtbToDGV(DataTable dt, PwtDataGridView Dgv)
        {

            Dgv.Columns.Clear();
 

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Dgv.Columns.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
            }

            if (dt.Rows.Count <= 100)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Dgv.Columns[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
                }
            }




            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Dgv.Rows.Add();

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                }

            }


            if (dt.Columns.Contains("序号"))
            {
                Dgv.Columns["序号"].Visible = false;
            }

        }

        /// <summary>
        /// dt输出PwtDataGridView
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Dgv"></param>
        public static void DtbToDGV02(DataTable dt, PwtDataGridView Dgv)
        {

            Dgv.Columns.Clear();

            //if (dt== null || dt.Rows.Count == 0)
            //{
            //    return;
            //}

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Dgv.Columns.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
            }





            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Dgv.Rows.Add();

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                }

            }

        }




        /// <summary>
        /// DataTable 赋值 PwtDataGridView 首列添加CheckBox选择
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Dgv"></param>
        /// <param name="FirstCheckBox"></param>
        public static void DtbToDGV(DataTable dt, PwtDataGridView Dgv, bool FirstCheckBox)
        {

            Dgv.Columns.Clear();

            if (FirstCheckBox == true)
            {
                #region 添加CheckBox控件
                System.Windows.Forms.DataGridViewCheckBoxColumn CheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                CheckBox.HeaderText = "选择";
                CheckBox.Name = "选择";
                //CheckBox.ReadOnly = false;


                Dgv.Columns.Add(CheckBox);
                Dgv.Columns[0].ReadOnly = false;
                #endregion

                #region 添加数据至UI
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Dgv.Columns.Add(dt.Columns[i].Caption.ToString(), dt.Columns[i].Caption.ToString());
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dgv.Rows.Add();
                    for (int j = 1; j < dt.Columns.Count + 1; j++)
                    {
                        Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j - 1].ToString();
                    }
                }
                Dgv.Columns["序号"].Visible = false;
                #endregion

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
                    for (int i = 0; i < Dgv.Rows.Count; i++)
                    {
                        Dgv.Rows[i].Cells[0].Value = true;
                    }
                };

                反选.Click += (demo, e) =>
                {
                    for (int i = 0; i < Dgv.Rows.Count; i++)
                    {
                        Dgv.Rows[i].Cells[0].Value = !Convert.ToBoolean(Dgv.Rows[i].Cells[0].Value);
                    }
                };

                清除.Click += (demo, e) =>
                {
                    for (int i = 0; i < Dgv.Rows.Count; i++)
                    {
                        Dgv.Rows[i].Cells[0].Value = false;
                    }
                };

                Cms.Items.Add(全选);
                Cms.Items.Add(反选);
                Cms.Items.Add(清除);


                Dgv.MouseClick += (demo, e) =>
                {
                    if (Dgv.SelectedRows.Count != 0)
                    {
                        Dgv.SelectedRows[0].Cells[0].Value = !Convert.ToBoolean(Dgv.SelectedRows[0].Cells[0].Value);
                    }
                };

                Dgv.ContextMenuStrip = Cms;
                Dgv.Columns[0].ReadOnly = false;
                #endregion
            }
            else
            {
                #region 添加数据至UI
                Dgv.Columns.Clear();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Dgv.Columns.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
                    if (dt.Columns[i].Caption.ToString() == "序号")
                    {
                        Dgv.Columns[i].Visible = false;
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dgv.Rows.Add();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();
                    }
                }
                #endregion
            }
        }



        /// <summary>
        /// DataTable 赋值 PwtDataGridView 首列添加CheckBox选择
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Dgv"></param>
        /// <param name="FirstCheckBox"></param>
        public static void DtbToDGV_pici(DataTable dt, PwtDataGridView Dgv, bool FirstCheckBox)
        {

            Dgv.Columns.Clear();

            if (FirstCheckBox == true)
            {
                #region 添加CheckBox控件
                System.Windows.Forms.DataGridViewCheckBoxColumn CheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
                CheckBox.HeaderText = "选择";
                CheckBox.Name = "选择";
                //CheckBox.ReadOnly = false;


                Dgv.Columns.Add(CheckBox);
                Dgv.Columns[0].ReadOnly = false;
                #endregion

                #region 添加数据至UI
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Dgv.Columns.Add(dt.Columns[i].Caption.ToString(), dt.Columns[i].Caption.ToString());
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dgv.Rows.Add();
                    for (int j = 1; j < dt.Columns.Count + 1; j++)
                    {
                        Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j - 1].ToString();
                        // Dgv.Columns[j].ReadOnly = true;
                    }
                }
              //  Dgv.Columns["序号"].Visible = false;
                #endregion

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
                    for (int i = 0; i < Dgv.Rows.Count; i++)
                    {
                        Dgv.Rows[i].Cells[0].Value = true;
                    }
                };

                反选.Click += (demo, e) =>
                {
                    for (int i = 0; i < Dgv.Rows.Count; i++)
                    {
                        Dgv.Rows[i].Cells[0].Value = !Convert.ToBoolean(Dgv.Rows[i].Cells[0].Value);
                    }
                };

                清除.Click += (demo, e) =>
                {
                    for (int i = 0; i < Dgv.Rows.Count; i++)
                    {
                        Dgv.Rows[i].Cells[0].Value = false;
                    }
                };

                Cms.Items.Add(全选);
                Cms.Items.Add(反选);
                Cms.Items.Add(清除);


                Dgv.MouseClick += (demo, e) =>
                {
                    if (Dgv.SelectedRows.Count != 0)
                    {
                        Dgv.SelectedRows[0].Cells[0].Value = !Convert.ToBoolean(Dgv.SelectedRows[0].Cells[0].Value);
                    }
                };

                Dgv.ContextMenuStrip = Cms;
                Dgv.Columns[0].ReadOnly = false;
                #endregion
            }
            else
            {
                #region 添加数据至UI
                Dgv.Columns.Clear();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    Dgv.Columns.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
                    //if (dt.Columns[i].Caption.ToString() == "序号")
                    //{
                    //    Dgv.Columns[i].Visible = false;
                    //}
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dgv.Rows.Add();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();
                    }
                }
                #endregion
            }
        }

    }
}
