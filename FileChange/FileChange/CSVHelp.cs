using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace FileChange
{
    public class CSVHelp
    { /// <summary>
      /// CSV转换成DataTable（OleDb数据库访问方式）
      /// </summary>
      /// <param name="csvPath">csv文件路径</param>
      /// <returns></returns>
        public static DataTable CSVToDataTableByOledb(string csvPath)
        {
            DataTable csvdt = new DataTable("csv");
            if (!File.Exists(csvPath))
            {
                throw new FileNotFoundException("csv文件路径不存在!");
            }

            FileInfo fileInfo = new FileInfo(csvPath);
            using (OleDbConnection conn = new OleDbConnection(
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo.DirectoryName +
                ";Extended Properties='Text;'"))
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + fileInfo.Name + "]", conn);
                adapter.Fill(csvdt);
            }

            return csvdt;
        }

        /// <summary>
        /// CSV转换成DataTable（文件流方式）
        /// </summary>
        /// <param name="csvPath">csv文件路径</param>
        /// <returns></returns>
        public static DataSet CSVToDataTableByStreamReader(string csvPath)
        {
            string strline = null;
            DataSet csvDest=new DataSet();
            DataTable csvTitledt = new DataTable();
            DataTable csvContentdt = new DataTable();
            DataColumn columnTitle;
            DataColumn columnContent;
            DataRow dr;
            List<string> fileStream=new List<string>();
            using (StreamReader reader = new StreamReader(csvPath, FileHelper.GetEncoding(csvPath)))
            {
                while (!reader.EndOfStream)
                {
                    strline = reader.ReadLine();
                    fileStream.Add(strline);
                }
            }
            if (fileStream.Count<=0)
            {
                return null;
            }
            int max = 0;
            int row = 0;
            for (int i = 0; i < fileStream.Count; i++)
            {
                int len = fileStream[i].Length;
                if (len>max)
                {
                    max = len;
                    row = i;
                }
            }
            string[] str = fileStream[row].Split(',');
            #region 抬头
            columnTitle = new DataColumn("info1");
            csvTitledt.Columns.Add(columnTitle);
            List<string> titleList = new List<string>();
            for (int i = 0; i < fileStream.Count; i++)
            {
                //titleList.Clear();

                string[] rowStr = fileStream[i].Split(',');
                if (rowStr.Length > 1 && fileStream[i].Split(',')[1] != "" && fileStream[i].Split(',')[0] != "")
                {
                    break;
                }
                titleList.Add(fileStream[i].Split(',')[0]);
            }

            for (int i = 0; i < titleList.Count; i++)
            {
                csvTitledt.Rows.Add(titleList[i]);
            }
            #endregion
            #region 内容
            for (int i = 0; i < str.Length; i++)
            {
                columnContent = new DataColumn("info" + i);
                csvContentdt.Columns.Add(columnContent);
            }
            List<string> rowList = new List<string>();
            for (int i = titleList.Count; i < fileStream.Count; i++)
            {
                rowList.Clear();
                string[] rowStr = fileStream[i].Split(',');
                for (int j = 0; j < rowStr.Length; j++)
                {
                    if (rowStr[j]=="")
                    {
                        continue;
                    }
                    rowList.Add(rowStr[j]);
                }
                if (rowList.Count <= 1)
                {
                    continue;
                }
                csvContentdt.Rows.Add(rowList.ToArray());
            }
            #endregion
            csvDest.Tables.Add(csvTitledt);
            csvDest.Tables.Add(csvContentdt);
            return csvDest;
        }

        /// <summary>
        /// DataTable 生成 CSV
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="csvPath">csv文件路径</param>
        public static void DataTableToCSV(DataTable dt, string csvPath)
        {
            if (null == dt)
                return;

            StringBuilder csvText = new StringBuilder();
            StringBuilder csvrowText = new StringBuilder();
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    csvrowText.Append(",");
            //    csvrowText.Append(dc.ColumnName);
            //}

            //csvText.AppendLine(csvrowText.ToString().Substring(1));

            foreach (DataRow dr in dt.Rows)
            {
                csvrowText = new StringBuilder();
                foreach (DataColumn dc in dt.Columns)
                {
                    csvrowText.Append(",");
                    csvrowText.Append(dr[dc.ColumnName].ToString().Replace(',', ' '));
                }

                csvText.AppendLine(csvrowText.ToString().Substring(1));
            }

            File.WriteAllText(csvPath, string.IsNullOrEmpty(csvText.ToString()) ? "" : csvText.ToString(), Encoding.Default);
        }
    }
}
