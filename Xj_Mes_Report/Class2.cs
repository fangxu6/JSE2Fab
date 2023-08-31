//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Xj_Mes_Report
//{
//    class Class2
//    {
//        /// <summary>
//        /// DataGridViewX导出数据
//        /// </summary>
//        /// <param name="strCaption">导出数据表名</param>
//        /// <param name="dgv">DataGridViewX</param>
//        public static void Exeort2Excel(Object[,] my_obj, int x, int y, string filePath, bool bo = false)
//        {

//            int total_no = 0;
//            Dictionary<string, int> tj = new Dictionary<string, int>();
//            for (int i = 0; i < x; i++)
//            {
//                for (int j = 0; j < y; j++)
//                {
//                    string module_name = my_obj[i, j].ToString();
//                    if (tj.ContainsKey(module_name))
//                    {
//                        tj[module_name]++;
//                    }
//                    else
//                    {
//                        tj.Add(module_name, 1);
//                    }
//                    if (module_name != "" && module_name != "M")
//                    {
//                        total_no++;
//                    }
//                }
//            }


//            Object[,] my_tj_obj = new Object[tj.Count + 1, 3];

//            my_tj_obj[0, 0] = "Bin";
//            my_tj_obj[0, 1] = "数量";
//            my_tj_obj[0, 2] = "占比";
//            int temp_no = 1;


//            foreach (var item in tj)
//            {
//                my_tj_obj[temp_no, 0] = "BIN-" + item.Key;
//                my_tj_obj[temp_no, 1] = item.Value;


//                if (item.Key != "" && item.Key != "M")
//                {
//                    my_tj_obj[temp_no, 2] = double.Parse(item.Value.ToString()) / double.Parse(total_no.ToString());
//                }
//                else
//                {
//                    my_tj_obj[temp_no, 2] = "无效";
//                }
//                temp_no++;
//            }






//            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
//            if (app == null)
//            {
//                // MessageBox.Show("无法启动，可能你的机器上没有安装Excel！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }



//            string this_filepath = filePath + ".xlsx";



//            if (!File.Exists(this_filepath))
//            {
//                File.Copy(Application.StartupPath + @"\Sample2.xlsx", this_filepath);

//            }
//            else
//            {
//                this_filepath = filePath + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xlsx";
//                File.Copy(Application.StartupPath + @"\Sample2.xlsx", this_filepath);
//            }

//            Microsoft.Office.Interop.Excel.Workbook workbook;

//            if (bo)
//            {
//                workbook = app.Workbooks.Add(true);
//            }
//            else
//            {
//                workbook = app.Workbooks.Open(this_filepath);
//            }

//            #region Map 图谱

//            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets["Sheet1"];

//            Microsoft.Office.Interop.Excel.Range range;
//            range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[x, y]];

//            range.Value = my_obj;
//            range.RowHeight = 20;
//            range.ColumnWidth = 1.5;

//            #endregion




//            #region 统计
//            Microsoft.Office.Interop.Excel.Worksheet worksheet_tj = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets["Sheet2"];

//            Microsoft.Office.Interop.Excel.Range range_tj;
//            range_tj = worksheet_tj.Range[worksheet_tj.Cells[1, 1], worksheet_tj.Cells[tj.Count + 1, 3]];

//            range_tj.Value = my_tj_obj;
//            #endregion


//            try
//            {
//                app.Visible = false;
//                workbook.Save();
//            }
//            catch (Exception exx)
//            {
//                TxtHelper.AddTxt(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "、文件:" + this_filepath + " 保存出现错误：" + exx.Message, "[ExcelDealFile]");
//            }
//            finally
//            {
//                worksheet = null;
//                workbook.Close();
//                app.Quit();
//                GC.Collect();
//            }




//        }
//    }
//}
