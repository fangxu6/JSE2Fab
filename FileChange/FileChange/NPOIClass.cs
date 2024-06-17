using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FileChange
{
    class NPOIClass
    {
        #region 获取sheet表对应的DataTable 
        /// <summary>  
        /// 获取sheet表对应的DataTable  
        /// </summary>  
        /// <param name="sheet">Excel工作表</param>  
        /// <param name="strMsg"></param>  
        /// <returns></returns>  
        private static DataTable GetSheetDataTable(ISheet sheet, out string strMsg)
        {
            strMsg = "";
            DataTable dt = new DataTable();
            string sheetName = sheet.SheetName;
            int startIndex = 0; // sheet.FirstRowNum;  
            int lastIndex = sheet.LastRowNum;
            //最大列数  
            int cellCount = 0;
            IRow maxRow = sheet.GetRow(0);
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null && cellCount < row.LastCellNum)
                {
                    cellCount = row.LastCellNum;
                    maxRow = row;
                }
            }

            //列名设置  
            try
            {
                for (int i = 0; i < maxRow.LastCellNum; i++) //maxRow.FirstCellNum  
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
                    //DataColumn column = new DataColumn("Column" + (i + 1).ToString());  
                    //dt.Columns.Add(column);  
                }
            }
            catch
            {
                strMsg = "工作表" + sheetName + "中无数据";
                return null;
            }

            //数据填充  
            for (int i = startIndex; i <= lastIndex; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow drNew = dt.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
                    {
                        if (row.GetCell(j) != null)
                        {
                            ICell cell = row.GetCell(j);
                            switch (cell.CellType)
                            {
                                case CellType.Blank:
                                    drNew[j] = "";
                                    break;
                                case CellType.Numeric:
                                    short format = cell.CellStyle.DataFormat;
                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理   
                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                        drNew[j] = cell.DateCellValue;
                                    else
                                        drNew[j] = cell.NumericCellValue;
                                    if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 ||
                                        cell.CellStyle.DataFormat == 188)
                                        drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    break;
                                case CellType.String:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                                case CellType.Formula:
                                    try
                                    {
                                        drNew[j] = cell.NumericCellValue;
                                        if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 ||
                                            cell.CellStyle.DataFormat == 188)
                                            drNew[j] = cell.NumericCellValue.ToString("#0.00");
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            drNew[j] = cell.StringCellValue;
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    break;
                                default:
                                    drNew[j] = cell.StringCellValue;
                                    break;
                            }
                        }
                    }
                }

                dt.Rows.Add(drNew);
            }

            return dt;
        }
        #endregion

        #region GetExcelToDataSet
        public static DataSet GetExcelToDataSet(string filePath, out string strMsg)
        {
            strMsg = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string fileType = Path.GetExtension(filePath).ToLower();
            string fileName = Path.GetFileName(filePath).ToLower();
            try
            {
                ISheet sheet = null;
                int sheetNumber = 0;
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                if (fileType == ".xlsx")
                {
                    // 2007版本  
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    sheetNumber = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetNumber; i++)
                    {
                        string sheetName = workbook.GetSheetName(i);
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            dt = GetSheetDataTable(sheet, out strMsg);
                            if (dt != null)
                            {
                                dt.TableName = sheetName.Trim();
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                MessageBox.Show("Sheet数据获取失败，原因：" + strMsg);
                            }
                        }
                    }
                }
                else if (fileType == ".xls")
                {
                    // 2003版本  
                    HSSFWorkbook workbook = new HSSFWorkbook(fs);
                    sheetNumber = workbook.NumberOfSheets;
                    for (int i = 0; i < sheetNumber; i++)
                    {
                        string sheetName = workbook.GetSheetName(i);
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            dt = GetSheetDataTable(sheet, out strMsg);
                            if (dt != null)
                            {
                                dt.TableName = sheetName.Trim();
                                ds.Tables.Add(dt);
                            }
                            else
                            {
                                MessageBox.Show("Sheet数据获取失败，原因：" + strMsg);
                            }
                        }
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return null;
            }
        }
        #endregion
    }
}
