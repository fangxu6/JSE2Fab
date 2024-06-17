using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChange
{
    class ty_csv_helper01
    {
        public static void FileAnaly(string filePath, out List<string> dic_product_data, out List<string> dic_time_data, out List<string> dic_test_data, out Dictionary<string, TestData> dic_test_info)
        {

            dic_product_data = new List<string>();
            dic_time_data = new List<string>();
            dic_test_data = new List<string>();
            dic_test_info = new Dictionary<string, TestData>();
            List<string> ListTxt = ReadTxt(filePath);
            int set_poist = 0;
            foreach (var item in ListTxt)
            {

                #region 空行继续
                if (item.ToString().TrimEnd() == "")
                {
                    continue;
                }
                #endregion

                #region 判断位置节点
                if (item.ToString().Contains("Test End Date "))
                {
                    set_poist = 1;
                }
                if (item.ToString().Contains("Total:"))
                {
                    set_poist = 2;
                }
                if (item.ToString().Contains("SITE_NUM,"))
                {
                    set_poist = 3;
                }
                #endregion

                #region 数据提取处理

                switch (set_poist)
                {
                    case 0:
                        dic_product_data.Add(item.ToString());
                        break;
                    case 1:
                        dic_time_data.Add(item.ToString());
                        break;
                    case 2:
                        dic_test_data.Add(item.ToString());
                        break;
                    case 3:
                        #region 去除无效行
                        if (item.ToString().Contains("SITE_NUM,"))
                        {
                            continue;
                        }
                        if (item.ToString().Contains("Unit,,"))
                        {
                            continue;
                        }
                        if (item.ToString().Contains("LimitL,,"))
                        {
                            continue;
                        }
                        if (item.ToString().Contains("LimitU,,,"))
                        {
                            continue;
                        }
                        if (item.ToString().Trim() == "")
                        {
                            continue;
                        }

                        ////小于固定长度
                        //if (item.ToString().Split(',').Length < 9)
                        //{
                        //    continue;
                        //}

                        #endregion

                        string x = item.ToString().Split(',')[5].ToString();
                        string y = item.ToString().Split(',')[6].ToString();
                        string SOFT_BIN = item.ToString().Split(',')[3].ToString();
                        string T_TIME = item.ToString().Split(',')[4].ToString();
                        string SITE_NUM = item.ToString().Split(',')[0].ToString();
                        string PASSFG = item.ToString().Split(',')[2].ToString();
                        string Site_check = item.ToString().Split(',')[8].ToString();
                        if (SITE_NUM == "LimitL" || SITE_NUM == "LimitU" || SITE_NUM == "Unit")
                        {
                            continue;
                        }
                        if ((x.Length > 3 && Convert.ToInt32(x) < 0) && (x.Length > 3 && Convert.ToInt32(x) < 0))
                        {
                            continue;
                        }

                        if (x == "" || y == "" || x == "0" || y == "0" || SOFT_BIN == "" || SITE_NUM == "" || PASSFG == "" || Site_check == "")
                        {
                            continue;
                        }

                        TestData td = new TestData();
                        td.x = x;
                        td.y = y;
                        td.SOFT_BIN = SOFT_BIN;
                        td.T_TIME = T_TIME;
                        td.SITE_NUM = SITE_NUM;
                        td.PASSFG = PASSFG;
                        td.Site_check = Site_check;

                        string post_xy = x + "_" + y;

                        if (dic_test_info.ContainsKey(post_xy))
                        {
                            dic_test_info[post_xy] = td;
                        }
                        else
                        {
                            dic_test_info.Add(post_xy, td);
                        }

                        break;
                    default:
                        break;
                }

                #endregion

            }
            int a = 0;
        }

        #region 读取文件
        private static List<string> ReadTxt(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            List<string> txt = new List<string>();

            using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    txt.Add(line);
                }
            }

            return txt;
        }
        #endregion
    }
}
