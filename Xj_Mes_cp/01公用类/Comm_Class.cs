using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pawote.UI.Controls;
using System.Data;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Spire.Xls;
using System.IO;

namespace Xj_Mes_cp
{
    public class Comm_Class
    {
        public static Boolean Gridview_new_add_row(DataTable dt, PwtDataGridView dataGridView)
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
            return true;
        }
        public static void combox_colum_databind(DataTable dt, ComboBox combo, String str)
        {
            combo.Items.Clear();
            String[] list =
                dt.AsEnumerable().ToList().Select(p => p.Field<String>(str))
               .ToList().Distinct().ToArray();
            combo.Items.AddRange(list);
        }
        public static DataTable file_format_change(String format, int index)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "文件浏览|*." + format;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataTable dt = new DataTable();
                Workbook workbook = new Workbook();
                String filename = dialog.FileName;
                dt.TableName = System.IO.Path.GetFileName(filename); 
                String[] file = filename.Split('.');
                if (file[file.Length - 1].ToUpper() != "CSV")
                {
                    workbook.LoadFromFile(dialog.FileName);
                    Worksheet worksheet = workbook.Worksheets[0];
                    worksheet.SaveToFile("config_file.csv", ",", Encoding.UTF8);
                    filename = Application.StartupPath + @"\config_file.csv";
                }
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(fs, Encoding.Default);
                List<String> data = reader.ReadToEnd().Split('\n').Where
                    (p => p.Split(',')[0] != "").ToList();
                
                
                if (data.Count <= index + 1)
                {
                    return null;
                }
                for (int i = 0; i < index; i++)
                {
                    data.RemoveAt(0);
                }
                List<String> colum_data = data[0].Replace('"', ' ').Split(',').ToList();
                for (int i = 0; i < colum_data.Count; i++)
                {
                    dt.Columns.Add(colum_data[i]);
                }
                for (int i = 1; i < data.Count; i++)
                {
                    List<String> row_data = data[i].Replace('"', ' ').Split(',').ToList();
                    dt.Rows.Add();
                    for (int j = 0; j < row_data.Count; j++)
                    {
                        dt.Rows[dt.Rows.Count - 1][j] = row_data[j].Replace(" ","");
                    }
                }
                int count = colum_data.Where(p => p == "").ToList().Count;
                for (int m = 0; m < count; m++)
                {
                    int index1 = colum_data.IndexOf("");
                    colum_data.RemoveAt(index1);
                    dt.Columns.RemoveAt(index1);
                }
                if (colum_data[colum_data.Count - 1] == "\r")
                {
                    dt.Columns.RemoveAt(dt.Columns.Count - 1);
                }
               
                return dt;
                
            }
            else
            {
                return null;
            }
        }
        public static String get_combox_value(ComboBox combo)
        {
            if (combo.SelectedIndex == -1)
            {
                return "";
            }
            return combo.SelectedItem.ToString();
        }
        public static void combox_databind(ComboBox combo, DataTable dt)
        {
            combo.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                combo.Items.Add(dt.Rows[i][0]);
            }
            
        }
        private static db_deal ex = new db_deal();
        public static void GetComBox(string type_name, ComboBoxEx CbBEx)
        {

            DataTable dtb = ex.Get_Data("[dbo].[SYSTEM_INFO_LIST_GET_SELECT] '" + type_name + "'");
            CbBEx.Items.Clear();
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                CbBEx.Items.Add(dtb.Rows[i][0].ToString());
            }
        }

        //在pwtdatagridview中添加一行数据
        public static Boolean Gridview_add_row(DataTable dt,PwtDataGridView dataGridView)
        {
            //当数据没有行是,则进行绑定
            if (dataGridView.Rows.Count == 0)
            {
                DtbToDGV(dt, dataGridView);
               
                return true;
            }
            dataGridView.Rows.Add();
            if (dt.Columns.Count != dataGridView.ColumnCount)
            {
                MessageBox.Show("输入的数据的条数不相等");
                return false;
            }
            for(int i = 0; i < dt.Columns.Count; i++)
            {
                dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[i].Value = dt.Rows[0][i].ToString();
            }
            return true;
        }
        public static Boolean gridview_update_row(String [] data,PwtDataGridView pwtDataGridView)
        {
            if (data.Length != pwtDataGridView.ColumnCount)
            {
                MessageBox.Show("输入的数据的条数不相等");
                return false;
            }
            for(int i = 0; i<data.Length; i++)
            {
                pwtDataGridView.SelectedRows[0].Cells[i].Value = data[i];
            }
            return true;
        }
        public static void DtbToDGV(DataTable dt, PwtDataGridView Dgv)
        {

            Dgv.Columns.Clear();


            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Dgv.Columns.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
            }


            if (dt == null || dt.Rows.Count == 0)
            {
                
                return;
            }




            //if (dt.Rows.Count <= 100)
            //{
            //    for (int i = 0; i < dt.Columns.Count; i++)
            //    {
            //        Dgv.Columns[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            //    }
            //}




            for (int i = 0; i < dt.Rows.Count; i++)
            {

                Dgv.Rows.Add();

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Dgv.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString().Replace(',', ' ').Replace('\'', ' ').TrimEnd().TrimStart();

                }

            }

        }
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

        public static Boolean Gridview_add_row_new(DataTable dt, PwtDataGridView dataGridView)
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
            return true;
        }


    }
}
