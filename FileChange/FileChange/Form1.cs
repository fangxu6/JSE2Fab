using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileChange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 原始文件夹
        private void button1_Click(object sender, EventArgs e)
        {
            button4_Click(null, null);
            string file = "";
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                file = dilog.SelectedPath;
            }
            List<string> fileList = new List<string>();
            if (file == "")
            {
                return;
            }

            //if (file.Contains(","))
            //{
            //    file = file.Replace(",", "$");
            //}
            fileList = Director(file);
            for (int i = 0; i < fileList.Count; i++)
            {
                string str = fileList[i].Split(',')[0];
                string strTime= fileList[i].Split(',')[1];
                string searchString = "csv";

                bool containsCsv = str.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0;
                if (containsCsv)
                {
                    string fileName = GetLastPartNameOfPath(fileList[i]);
                    string fileName2 = fileList[i].Substring(fileList[i].LastIndexOf("\\") + 1);
                    string fileType = fileName.Substring(fileName2.LastIndexOf('.') + 1);
                    this.dataGridView1.Rows.Add();
                    this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0].Value = this.dataGridView1.Rows.Count.ToString();
                    this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[1].Value = fileName2;
                    this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[2].Value = file + "\\" + fileList[i].Split(',')[0];
                    this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[3].Value = strTime;
                    //this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[3].Value = fileType;
                }
                else
                {
                    continue;
                }
            }
            textBox2.Text = file;
        }
        #endregion

        #region 保存文件
        private void button2_Click(object sender, EventArgs e)
        {
            string file = "";
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                file = dilog.SelectedPath;
            }
            textBox3.Text = file;
        }
        #endregion

        #region 获取#前的文件名
        public  string ExtractString(string input, string pattern)
        {
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
        #endregion

        #region 转换
        private void button3_Click(object sender, EventArgs e)
        {
            #region 提醒
            string file = textBox2.Text;
            if (textBox2.Text == "")
            {
                MessageBox.Show("请选择原始路径", "系统信息");
                return;
            }
            string fileMuBiao = textBox3.Text;
            if (fileMuBiao == "")
            {
                MessageBox.Show("请选择目标路径", "系统信息");
                return;
            }
            if (this.dataGridView1.Rows.Count<=0)
            {
                MessageBox.Show("该文件夹下没有需要转换的文件，请核对文件路径是否正确", "系统信息");
                return;
            }
            #endregion

            List<string> pathGroup = new List<string>();//文件组集合
            List<string> pathList = new List<string>();//文件名集合
            if (dataGridView1.Rows.Count <= 0)
            {
                return;
            }
            pathGroup = GroupFile();
            if (pathGroup.Count <= 0)
            {
                return;
            }
            string filePath = pathGroup[0].Split(',')[0];
            string pattern = @"\\([^\\#]+)#";
            string s = ExtractString(filePath, pattern);
            #region CSV/XML
            string mubiaoPath = fileMuBiao + @"\csv"+s;
            if (!Directory.Exists(mubiaoPath))
            {
                Directory.CreateDirectory(mubiaoPath);
                fileMuBiao = mubiaoPath;
            }
            else
            {
                fileMuBiao = mubiaoPath;
            }
            //DelectDir(fileMuBiao);
            string PRODUCT_ID = this.textBox5.Text;
            string OP_NAME = this.textBox6.Text;
            string 委工单 = this.textBox11.Text;
            for (int i = 0; i < pathGroup.Count; i++)
            {
                pathList.Clear();
                string pathlist = pathGroup[i].Split('@')[0];
                int a = pathlist.Split('$').Length;
                string nameStrings = GetName(pathlist.Split('$')[0]);
                if (a==1)
                {
                    //List<string> nameList = GetName(pathlist);
                    //string nameStrings = textBox5.Text + "_" + nameList[0] + "_" + textBox11.Text + "_" + nameList[1] + "_" + textBox6.Text + "_" +  pathGroup[i].Split('@')[1];
                    //string nameStrings = PRODUCT_ID + "_" + nameList[0].ToUpper().Replace("CP1", "").Replace("CP2", "").Replace("CP3", "") + "_" + 委工单 + "_" + nameList[1].PadLeft(2, '0') + "_" + OP_NAME + "_" + nameList[2].Replace("-", "").Replace(":", "").Replace(" ", "");
                    //string nameStrings = pathlist.Split('\\')[pathlist.Split('\\').Length-1];
                    //string nameStrings = textBox5.Text + "_" + textBox6.Text + "_" + num + "_" + DateTime.Now.ToString("yyyyMMdd");
                    pathList.Add(pathlist);
                    JieXi(pathList, fileMuBiao, nameStrings);
                    //File.Copy(pathlist, fileMuBiao+'\\'+ nameStrings+".csv", true);
                }
                else
                {
                    string[] pStrings = pathlist.Split('$');
                    //List<string> nameList = GetName(pStrings[0]);
                    //string nameStrings = PRODUCT_ID + "_" + nameList[0].ToUpper().Replace("CP1", "").Replace("CP2", "").Replace("CP3", "") + "_" + 委工单 + "_" + nameList[1].PadLeft(2, '0') + "_" + OP_NAME + "_" + nameList[2].Replace("-", "").Replace(":", "").Replace(" ", "");
                    //string nameStrings = textBox5.Text + "_" + textBox6.Text + "_"+num+"_" + DateTime.Now.ToString("yyyyMMdd");
                    foreach (string pString in pStrings)
                    {
                        pathList.Add(pString);
                    }
                    JieXi(pathList, fileMuBiao, nameStrings);
                }
            }
            List<string> fileList = Director(fileMuBiao);
            string folderName = Path.GetFileName(Path.GetDirectoryName(filePath));
            folderName = folderName.Split('#')[0];
            for (int j = 0; j < fileList.Count; j++)
            {
                if (fileList[j].Contains(".txt"))
                {
                    continue;
                }
                string filename = fileMuBiao + "\\" + fileList[j].Split(',')[0];
                MyFileAnalyDo(filename, s,folderName);
            }
            #endregion

            MessageBox.Show("解析完成","系统信息");
        }
        #endregion

        #region 获取路径中最后一部分的名称
        /// <summary>
        /// 获取路径中最后一部分的名称（文件名或文件夹名）。
        /// </summary>
        /// <param name="path">文件路径或文件夹路径</param>
        /// <returns></returns>
        public static string GetLastPartNameOfPath(string path)
        {
            // 正则说明：
            // [^/\\]   ：表示匹配除了斜杠(/)和反斜杠(\)以外的任意字符，双反斜杠用于转义
            // +        ：表示匹配前面的表达式一次或多次
            // [/\\]    ：表示匹配斜杠(/)或反斜杠(\)
            // *        ：表示匹配零次或多次
            // $        ：表示从后向前匹配

            // 截取最后一部分名称，名称的末尾可能带有多个斜杠(/)或反斜杠(\)
            var pattern = @"[^/\\]+[/\\]*$";
            var match = System.Text.RegularExpressions.Regex.Match(path, pattern);
            var name = match.Value;

            // 截取名称中不带斜杠(/)或反斜杠(\)的部分
            pattern = @"[^/\\]+";
            match = System.Text.RegularExpressions.Regex.Match(name, pattern);
            name = match.Value;

            return name;
        }

        #endregion

        #region 获取文件名
        public List<string> Director(string dir)
        {
            List<string> list = new List<string>();
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();//文件
            files = files.OrderBy(y => y.Name, new FileNameComparerClass()).ToArray();
            DirectoryInfo[] directs = d.GetDirectories();//文件夹
            //foreach (FileInfo f in files)
            //{
            //    list.Add(f.Name);//添加文件名到列表中  
            //}
            for (int i = 0; i < files.Length; i++)
            {
                list.Add(files[i].Name+","+files[i].LastWriteTime.ToString("yyyyMMddHHmmss"));//添加文件名到列表中 
            }
            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo dd in directs)
            {
                Director(dd.FullName);
            }

            return list;
        }

        public class FileNameComparerClass : IComparer<string>
        {
            [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string psz1, string psz2);
            public int Compare(string psz1, string psz2)
            {
                return StrCmpLogicalW(psz1, psz2);
            }
        }
        #endregion

        #region 删除所有文件
        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 转换csv文件
        public static DataTable  CSVGetDataTable(string filePath)
        {
            //实例化一个datatable用来存储数据
            DataTable dt = new DataTable();
            //文件流读取

            using (FileStream fs = new FileStream(filePath, System.IO.FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
                string tempCount = "";
                string tempText = "";
                int row = 0;
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while ((tempText = sr.ReadLine()) != null)
                {
                    string[] arr = tempText.Split(new char[] { ',' });
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dr[i] = i < arr.Length ? arr[i] : "";
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion

        #region 解析

        private DataTable ReadDesFromFile(DataTable dtTitle, DataTable dtContent)
        {
            int titleNum = dtTitle.Rows.Count;
            int contentNum = dtContent.Rows.Count;
            if (titleNum==0|| contentNum==0)
            {
                return null;
            }
            #region 添加bin
            List<string> binList = new List<string>();
            DataTable dt = dtContent;
            dt.Columns.Add("id");
            int o = 0;
            int start = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (o==1)
                {
                    continue;
                }
                if (dt.Rows[i][1].ToString().Contains("Bin#"))
                {
                    start = i + 1;
                    o = 1;
                }

            }
            List<string> list = new List<string>();
            List<string> oldList = new List<string>();
            List<string> newList = new List<string>();
            List<string> indexList = new List<string>();
            List<string> intinList=new List<string>();
            for (int i = start; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["id"] = i + 1;
                //string SOFT_BIN = dt.Rows[i][1].ToString();//SOFT_BIN
                //string PASSFG = dt.Rows[i][2].ToString();//PASSFG
                //string SITE_CHK = dt.Rows[i][8].ToString();//SITE_CHK
                //string TEST_NUM = dt.Rows[i][7].ToString();//TEST_NUM
                string str1 = dt.Rows[i][1].ToString();//bin
                string str2 = dt.Rows[i][3].ToString();//x
                string str3 = dt.Rows[i][4].ToString();//y
                string id = dt.Rows[i]["id"].ToString();
                if (str2 == "XAdr" || str3 == "YAdr")
                {
                    continue;
                }
                if ((str2.Length > 3 && Convert.ToInt32(str2) < 0) && (str3.Length > 3 && Convert.ToInt32(str3) < 0))
                {
                    intinList.Add(id);
                    continue;
                }
                if (str2 == "" || str3 == "" || str2 == "0" || str3 == "0" )
                {
                    continue;
                }
                string str = str2 + "," + str3;
                string stre = str1 + "," + str2 + "," + str3 + "," + id;
                list.Add(str);
                oldList.Add(stre);
            }

            for (int i = dtContent.Rows.Count-1; i >=0 ; i--)
            {
                string index = dtContent.Rows[i]["id"].ToString();
                for (int j = 0; j < intinList.Count; j++)
                {
                    if (index==intinList[j])
                    {
                        dtContent.Rows.RemoveAt(i);
                    }
                }
            }
            list = RemoveT(list);
            List<string> see=new List<string>();
            List<string> idindex=new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                string x = list[i].Split(',')[0];
                string y = list[i].Split(',')[1];
                DataRow[] dr = dt.Select(" info3 = '" + x + "'and info4 = '" + y + "'");
                int rowCol = dr[0].ItemArray.Length;
                int a = 0;
                if (dr.Length > 1)
                {
                    foreach (DataRow dataRow in dr)
                    {
                        a++;
                        string passstr = dataRow[2].ToString();
                        string binstr = dataRow[3].ToString();
                        string xstr = dataRow[5].ToString();
                        string ystr = dataRow[6].ToString();
                        string idstr = dataRow[rowCol-1].ToString();
                        if (a == dr.Length)
                        {
                            newList.Add(passstr + "," + binstr + "," + xstr + "," + ystr + "," + idstr);
                        }
                        else if(a < dr.Length)
                        {
                            indexList.Add(idstr);
                            see.Add(binstr + "," + xstr + "," + ystr + "," + idstr);
                            idindex.Add(idstr);
                        }
                    }
                }
            }

            HashSet<string> idSet = new HashSet<string>(idindex);

            for (int i = dtContent.Rows.Count - 1; i >= 0; i--)
            {
                string index = dtContent.Rows[i]["id"].ToString();
                if (idSet.Contains(index))
                {
                    dtContent.Rows.RemoveAt(i);
                }
            }

            int qqq = dtContent.Rows.Count;
            //for (int i = dtContent.Rows.Count - 1; i >= 0; i--)
            //{
            //    string index = dtContent.Rows[i]["id"].ToString();
            //    for (int j = 0; j < idindex.Count; j++)
            //    {
            //        if (index == idindex[j])
            //        {
            //            dtContent.Rows.RemoveAt(i);
            //        }
            //    }
            //}
            //qqq = dtContent.Rows.Count;
            HashSet<string> indexSet = new HashSet<string>(see);

            oldList.RemoveAll(indexSet.Contains);
            //for (int i = see.Count-1; i >=0 ; i--)
            //{
            //    string index = see[i];
            //    oldList.Remove(index);
            //}
            List<string> binNew = new List<string>();
            for (int i = 0; i < oldList.Count; i++)
            {
                string bin = oldList[i].Split(',')[0];
                binList.Add(bin);
            }
            #endregion
            binNew = RemoveT(binList);
            double binCount = 0;
            Dictionary<string, double> dic = new Dictionary<string, double>();
            for (int j = 0; j < binNew.Count; j++)
            {
                string bin = binNew[j];
                binCount = CountTimes(binList, bin);
                dic.Add(bin,binCount);
            }

            double total = 0;
            double pass = 0;
            if (dic.TryGetValue("1", out pass))
            {
                pass = dic["1"];
            }
            else
            {
                pass = 0;
            }
            string passp = "";
            double fail = 0;
            string failp = "";
            for (int i = 0; i < binNew.Count; i++)
            {
                string bin = binNew[i];
                total += dic[bin];
                if (bin=="1")
                {
                    continue;
                }
                fail+= dic[bin];
            }

            passp = (pass / total).ToString("0.00%");
            failp = (fail / total).ToString("0.00%");
            #region 更改数据

            int over = 0;
            int row = 0;
            int sample = 0;
            for (int i = 0; i < dtTitle.Rows.Count; i++)
            {
                if (over==1)
                {
                    continue;
                }
                string str = dtTitle.Rows[i][0].ToString();
                if (str.Contains("SBin["))
                {
                    row = i;
                    over = 1;
                }if (str.Replace(" ","").Contains("SamplePass"))
                {
                    sample = i+1;
                }
            }

            dtTitle.Rows[sample][0] = $"{total}  {pass}  {passp}  {fail}  {failp}";
            for (int i = row; i < dtTitle.Rows.Count; i++)
            {
                string match = dtTitle.Rows[i][0].ToString();
                if (Regex.Matches(match, @"^[A-Za-z]").Count<0)
                {
                    continue;
                }
                string str = dtTitle.Rows[i][0].ToString();
                List<string> strList=new List<string>();
                strList = ChangeList(str);
                //if (str.Contains("Total:"))
                //{
                //    strList[1] = Convert.ToString(total);
                //}
                //else if (str.Contains("Pass:"))
                //{
                //    strList[1] = Convert.ToString(pass);
                //    strList[4] = Convert.ToString(passp);
                //}
                //else if (str.Contains("Fail:"))
                //{
                //    strList[1] = Convert.ToString(fail);
                //    strList[4] = Convert.ToString(failp);
                //}
                //else
                if (str.Contains("SBin["))
                {
                    string[] binStr = strList[0].Split('[');
                    binStr = binStr[1].Split(']');
                    string bin = binStr[0];
                    List<int> ind = new List<int>();
                    for (int j = 0; j < strList.Count; j++)
                    {
                        string reg = strList[j];
                        if (reg!="")
                        {
                            ind.Add(j);
                        }
                    }

                    int countList = ind.Count+1;
                    if (!dic.Keys.Contains(bin))
                    {
                        strList[ind[countList-3]] = "0";
                        strList[ind[countList-2]] = "0.00%";
                    }
                    else
                    {
                        strList[ind[countList - 3]] = dic[bin].ToString();
                        string binp = (dic[bin] / total).ToString("0.00%");
                        strList[ind[countList - 2]] = binp;
                    }
                    //strList[ind[countList-1]] = bin;
                }
                else
                {
                    continue;
                }

                if (strList[0]=="")
                {
                    continue;
                }

                string s = dtTitle.Rows[i][0].ToString();
                List<string> str111=new List<string>();
                for (int j = 0; j < strList.Count; j++)
                {
                    if (strList[j]=="")
                    {
                        continue;
                    }
                    str111.Add(strList[j]);
                }
                StringBuilder sb = new StringBuilder();
                int num = 0;
                for (int j = 0; j < str111.Count; j++)
                {
                    string str2 = str111[j];
                    if (str2 != "")
                    {
                        num++;
                    }

                    if (num == 1)
                    {
                        sb.Append(str2);
                    }
                    if (num == 2)
                    {
                        while (sb.Length < 40)
                        {
                            sb.Append(" ");
                        }
                        sb.Append(str2);
                    }

                    if (num == 3)
                    {
                        while (sb.Length < 80)
                        {
                            sb.Append(" ");
                        }
                        sb.Append(str2);
                    }
                }

                dtTitle.Rows[i][0] = "";
                dtTitle.Rows[i][0] = sb.ToString();
            }
            #endregion

            DataTable dtSum=getDatatable(dtTitle, dtContent);
            return dtSum;
        }
        #endregion

        #region 线程池
       
    #endregion

        #region 计算list里元素出现的次数
    public static int CountTimes<T>(List<T> inputList, T searchItem)

        {
            return ((from t in inputList where t.Equals(searchItem) select t).Count());
        }
        #endregion

        #region list去重
        public List<T> RemoveT<T>(List<T> items)
        {
            HashSet<T> set = new HashSet<T>();

            var res = new List<T>();//返回

            for (int i = 0; i < items.Count; i++)
            {
                if (!set.Contains(items[i]))
                {
                    set.Add(items[i]);
                    res.Add(items[i]);
                }
            }
            return res;
        }

        #endregion

        #region 字符串转list
        public List<string> ChangeList(string str)
        {
            string[] listes = str.Split(' ');
            List<string> strCount = new List<string>();
            foreach (string listArray in listes)
            {
                strCount.Add(listArray);
            }
            return strCount;
        }
        #endregion

        #region 合并datatable

        public DataTable getDatatable(DataTable dt1, DataTable dt2)
        {
            int row1 = dt1.Columns.Count;//5
            int row2 = dt2.Columns.Count;//2
            int row = dt1.Rows.Count;
            for (int i = row1; i < row2; i++)
            {
                string str = "info" + i+1;
                dt1.Columns.Add(str);
            }

            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt1.Rows.Add(dt2.Rows[i].ItemArray);
            }
            return dt1;
        }
        #endregion

        #region 清空
        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            dataGridView1.Rows.Clear();
        }
        #endregion

        #region 分解文件列表

        public List<string> GroupFile()
        {
            List<string> pathList = new List<string>();//文件名集合
            StringBuilder sb=new StringBuilder();
           








            List<string> pathSum = new List<string>();//文件名集合
            int rowCount = this.dataGridView1.Rows.Count;
            for (int i = 0; i < rowCount; i++)
            {
                string path = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
                pathList.Add(path);
            }

            string time = this.dataGridView1.Rows[rowCount-1].Cells[3].Value.ToString();
            var groupedFiles = pathList.GroupBy(file => GetGroupNumber(file.Split('\\')[file.Split('\\').Length-1]));

            List<string> resultList = new List<string>();
            foreach (var group in groupedFiles)
            {
                string filesString = string.Join("$", group)+"@"+ time;
                resultList.Add(filesString);
            }




            //List<string> much=new List<string>();
            //StringBuilder sbo=new StringBuilder();
            //string aaa ="";
            //int u = 0;
            //for (int i = 0; i < pathList.Count; i++)
            //{
            //    string[] a = pathList[i].Split('!')[0].Split('\\');
            //    string aa = a[a.Length - 1];
            //    string[] b = aa.Split('-');
            //    string bb = b[0];
            //    string[] c = b[1].Split('_');
            //    string cc = c[0];
            //    string ccc = cc.Split('.')[0];
            //    if (u==0)
            //    {
            //        sbo.Append(pathList[i].Split('!')[0] + ",");
            //        u++;
            //    }
            //    if (aaa == ccc)
            //    {
            //        sbo.Append(pathList[i].Split('!')[0] + ",");
            //    }
            //    else
            //    {
            //        if (aaa!="")
            //        {
            //            sbo.Length = sbo.Length - 1;
            //            sbo.Append('@'+ pathList[i].Split('!')[1] + ";" + pathList[i].Split('!')[0] + ",");
            //            u++;
            //        }
            //    }

            //    if (i== pathList.Count-1)
            //    {
            //        sbo.Length = sbo.Length - 1;
            //        sbo.Append('@'+pathList[i].Split('!')[1]);
            //    }
            //    aaa = ccc;
            //    string str = bb + "-" + cc;
            //    //much.Add(sbo.ToString());
            //}
            ////sbo.Length = sbo.Length - 1;
            //string ssss = sbo.ToString();
            //for (int i = 0; i < u; i++)
            //{
            //    string sL = ssss.Split(';')[i];
            //    much.Add(sL);
            //}
            //much = RemoveT(much);
            ////for (int j = 0; j < much.Count; j++)
            ////{
            ////    StringBuilder sb = new StringBuilder();
            ////    for (int i = 0; i < pathList.Count; i++)
            ////    {
            ////        string str = pathList[i].Split('\\')[pathList[i].Split('\\').Length - 1].Split('_')[1];
            ////        string muchstr = much[j].Split('_')[1];
            ////        //if (pathList[i].Contains(much[j]))
            ////        if (str.IndexOf(muchstr )!= -1)
            ////        {
            ////            sb.Append(pathList[i] + ",");
            ////        }
            ////    }
            ////    sb.Length = sb.Length - 1;
            ////    pathSum.Add(sb.ToString());
            ////}
            return resultList;
        }
        #endregion

        #region 寻找匹配的文件路径
        public  int GetGroupNumber(string file)
        {
            //Regex regex = new Regex(@"-(\d+)");
            //Match match = regex.Match(file);
            //if (match.Success)
            //{
            //    string groupNumberString = match.Groups[1].Value;
            //    return int.Parse(groupNumberString);
            //}
            //return -1; // 若找不到匹配的数字，返回-1或其他合适的默认值

            Regex regex = new Regex(@"-(\d+)");
            Match match = regex.Match(file);

            if (match.Success)
            {
                string numberString = match.Groups[1].Value;
                return int.Parse(numberString);
            }

            return -1; // 或其他适当的默认值
        }
        #endregion

        #region 获取名称
        public string GetName(string filePath)
        {
            string PRODUCT_ID = this.textBox5.Text;
            string OP_NAME = this.textBox6.Text;
            string VERSION = this.textBox13.Text;
            string TEMPERATURE = this.textBox7.Text;
            string NOTCH = this.textBox8.Text;
            string XYDIR = this.textBox9.Text;
            string LOT_TYPE = this.textBox10.Text;
            string 委工单 = this.textBox11.Text;
            List<string> dic_product_data = new List<string>();
            List<string> dic_time_data = new List<string>();
            List<string> dic_test_data = new List<string>();
            Dictionary<string, TestData> dic_test_info = new Dictionary<string, TestData>();
            ty_csv_helper01.FileAnaly(filePath, out dic_product_data, out dic_time_data, out dic_test_data, out dic_test_info);
            string Wafer_Id = "";
            string Lot_Id = "";
            string Wafer_no = "";
            string Program = "";
            string Program_name = "";
            string Site = "";
            string lot_id = "";

            foreach (var item in dic_product_data)
            {
                string value = item.Replace("Wafer Id", "").Replace("WAFER_ID", "").Replace("Wafer ID", "").Replace("Lot Id", "").Replace("Lot ID", "").Replace("LOT_ID", "").Replace("Program", "").Replace("Site", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("F", "");

                if (item.Contains("Wafer Id") || item.Contains("WAFER_ID")|| item.Contains("Wafer ID"))
                {
                    Wafer_Id = value.Trim().Split(':')[1].Split('-')[1];
                }
                if (item.Contains("Lot Id") || item.Contains("LOT_ID")|| item.Contains("LotID")|| item.Contains("Lot ID"))
                {
                    Lot_Id = value.Trim().Split(':')[1];
                }
            }
            #region 旧
            //foreach (var item in dic_product_data)
            //{
            //    if (item.Contains("Wafer Id:"))
            //    {
            //        Wafer_Id = item.Replace("Wafer Id:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("F", "");
            //        Wafer_no = Wafer_Id.Trim().Substring(Wafer_Id.LastIndexOf('-') + 1).Replace(",", "");
            //    }
            //    if (item.Contains("WAFER_ID:"))
            //    {
            //        Wafer_Id = item.Replace("WAFER_ID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("F", "");
            //        Wafer_no = Wafer_Id.Trim().Substring(Wafer_Id.LastIndexOf('-') + 1).Replace(",", "");
            //    }
            //    if (item.Contains("Lot Id:"))
            //    {
            //        Lot_Id = item.Replace("Lot Id:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Split('F')[0];
            //        lot_id = item.Replace("Lot Id:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "")
            //            .Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "")
            //            .Replace("CP1", "").Replace("CP2", "").Replace("CP3", "");
            //    }
            //    if (item.Contains("LOT_ID:"))
            //    {
            //        Lot_Id = item.Replace("LOT_ID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Split('F')[0];
            //        lot_id = item.Replace("LOT_ID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "")
            //            .Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "")
            //            .Replace("CP1", "").Replace("CP2", "").Replace("CP3", "");
            //    }
            //    if (item.Contains("Program:"))
            //    {
            //        Program = item.Replace("Program:", "").Replace(",", "");
            //        Program_name = Program.Substring(Program.LastIndexOf('\\') + 1).Replace(",", "");

            //    }
            //    if (item.Contains("Site:"))
            //    {
            //        Site = item.Replace("Site:", "").Replace(",", "");
            //    }
            //} 
            #endregion
            string Ending_Time = "";

            foreach (var item in dic_time_data)
            {
                if (item.Contains("Test End Date"))
                {
                    string[] splitArray = item.ToString().Split(new[] { ','},2);
                    if(splitArray.Length < 1)
                    {
                        continue;
                    }
                    string program = splitArray[0];
                    program = program.Split(new[] { ':' }, 2)[1];

                    //string input = item.Split(':')[1].Trim();
                    DateTime dateTime = DateTime.ParseExact(program.Trim(), "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
                    Ending_Time = dateTime.ToString("yyyyMMddHHmmss");
                    //Ending_Time = DateTime.Parse(item.Replace("Ending Time:", "").Replace(",", "")).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            string nameStrings = PRODUCT_ID + "_" + Lot_Id.ToUpper().Replace("CP1", "").Replace("CP2", "").Replace("CP3", "") + "_" + 委工单 + "_" + Wafer_Id.PadLeft(2, '0') + "_" + OP_NAME + "_" + Ending_Time.Replace("-", "").Replace(":", "").Replace(" ", "");
            return nameStrings;
        } 
        #endregion

        #region 转换xml
        /// <summary>
        /// 处理最终文件
        /// </summary>
        /// <param name="filePath"></param>
        public void MyFileAnalyDo(string filePath,string s,string cp)//,string fileName 
        {
            string PRODUCT_ID = this.textBox5.Text;
            string OP_NAME = this.textBox6.Text;
            string VERSION = this.textBox13.Text;
            string TEMPERATURE = this.textBox7.Text;
            string NOTCH = this.textBox8.Text;
            string XYDIR = this.textBox9.Text;
            string LOT_TYPE = this.textBox10.Text;
            string 委工单 = this.textBox11.Text;
            List<string> dic_product_data = new List<string>();
            List<string> dic_time_data = new List<string>();
            List<string> dic_test_data = new List<string>();
            Dictionary<string, TestData> dic_test_info = new Dictionary<string, TestData>();
            ty_csv_helper03.FileAnaly(filePath, out dic_product_data, out dic_time_data, out dic_test_data, out dic_test_info);
            foreach (var item in dic_test_info)
            {
                string SOFT_BIN = item.Value.SOFT_BIN.Trim();
                string site_n = item.Value.SITE_NUM;
                string x = item.Value.x;
                string y = item.Value.y;
            }
            #region 基础信息
            string Wafer_Id = "";
            string Lot_Id = "";
            string Wafer_no = "";
            string Program = "";
            string Program_name = "";
            string Site = "";

            #region 时间信息
            string Beginning_Time = "";
            string Ending_Time = "";
            string Total_Testing_Time = "";
            string Average_Test_Time = "";

            foreach (var item in dic_product_data)
            {
                if (item.Contains("Wafer Id:"))
                {
                    Wafer_Id = item.Replace("Wafer Id:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("F", "");
                    Wafer_no = Wafer_Id.Trim().Substring(Wafer_Id.LastIndexOf('-') + 1).Replace(",", "");
                }
                if (item.Contains("WAFER_ID:"))
                {
                    Wafer_Id = item.Replace("WAFER_ID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("F", "");
                    Wafer_no = Wafer_Id.Trim().Substring(Wafer_Id.LastIndexOf('-') + 1).Replace(",", "");
                }if (item.Contains("Wafer ID"))
                {
                    Wafer_Id = item.Replace("Wafer ID", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("F", "").Split(':')[1];
                    Wafer_no = Wafer_Id.Trim().Split('-')[1];
                }
                if (item.Contains("Lot Id:"))
                {
                    string str= Lot_Id = item.Replace("Lot Id:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Split('S')[0];
                    Lot_Id = item.Replace("Lot Id:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("CP1F", "").Replace("CP2F", "").Replace("CP3F", "").Split('S')[0];
                }if (item.Contains("LOT_ID:"))
                {
                    Lot_Id = item.Replace("LOT_ID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("CP1F", "").Replace("CP2F", "").Replace("CP3F", "").Split('S')[0];
                }if (item.Contains("Wafer_Lot ID:"))
                {
                    Lot_Id = item.Replace("Wafer_Lot ID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("CP1F", "").Replace("CP2F", "").Replace("CP3F", "").Split('S')[0];
                }if (item.Contains("LotID:"))
                {
                    Lot_Id = item.Replace("LotID:", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("CP1F", "").Replace("CP2F", "").Replace("CP3F", "").Split('S')[0];
                }if (item.Contains("Lot ID"))
                {
                    Lot_Id = item.Replace("Lot ID", "").Replace(",", "").Replace("-CP1F", "").Replace("-CP2F", "").Replace("-CP3F", "").Replace("-CP1", "").Replace("-CP2", "").Replace("-CP3", "").Replace("CP1", "").Replace("CP2", "").Replace("CP3", "").Replace("CP1F", "").Replace("CP2F", "").Replace("CP3F", "").Split('S')[0].Split(':')[1];
                }
                if (item.Contains("Program:"))
                {
                    Program = item.Replace("Program:", "").Replace(",", "");
                    Program_name = Program.Substring(Program.LastIndexOf('\\') + 1).Replace(",", "");

                }
                if (item.Contains("Test Program Name       : "))
                {
                    Program = item.Replace("Test Program Name       : ", "").Replace(",", "");
                    Program_name = Program.Substring(Program.LastIndexOf('\\') + 1).Replace(",", "");

                }
                if (item.Contains("Site:"))
                {
                    Site = item.Replace("Site:", "").Replace(",", "");
                }

                if (item.Contains("Test Start Date         : "))
                {
                    Beginning_Time = item.Replace("Test Start Date         : ", "").Replace(",", "").Trim();
                    string format = "ddd MMM d HH:mm:ss yyyy";
                    CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                    Beginning_Time = DateTime.ParseExact(Beginning_Time, "ddd MMM d HH:mm:ss yyyy", cultureInfo).ToString("yyyy-MM-dd HH:mm:ss");

                }

                if (item.Contains("Test End Date           :"))
                {
                    Ending_Time = item.Replace("Test End Date           :", "").Replace(",", "").Trim();
                    string format = "ddd MMM d HH:mm:ss yyyy";
                    CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                    Ending_Time = DateTime.ParseExact(Ending_Time, "ddd MMM d HH:mm:ss yyyy", cultureInfo).ToString("yyyy-MM-dd HH:mm:ss");

                }



            }
            #endregion
           

            foreach (var item in dic_time_data)
            {
                if (item.Contains("Beginning Time:"))
                {
                    string stime = item.Replace("Beginning Time:", "").Replace(",", "").Trim();
                    Beginning_Time = DateTime.Parse(stime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (item.Contains("Ending Time:"))
                {
                    Ending_Time = DateTime.Parse(item.Replace("Ending Time:", "").Replace(",", "")).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (item.Contains("Total Testing Time:"))
                {
                    Total_Testing_Time = item.Replace("Total Testing Time:", "").Replace(",", "");
                }
                if (item.Contains("Average Test Time(ms):"))
                {
                    Average_Test_Time = item.Replace("Average Test Time(ms):", "").Replace(",", "");
                }
            }
            #endregion
            #region 数据信息
            int Total = 0;
            string Pass_info = "";
            int Pass = 0;
            string Pass_bfb = "";
            string Fail = "";
            Dictionary<string, TestDataTotal> data_total_list = new Dictionary<string, TestDataTotal>();
            foreach (var item in dic_test_data)
            {
                if (item.Contains("SBin[") && item.Contains("%"))
                {
                    int len_item = item.ToString().Length;
                    TestDataTotal tdtl = new TestDataTotal();
                    tdtl.bin_name = item.Substring(0, 10).Trim().Replace(",", "");
                    tdtl.bin_id = item.Substring(0, 10).Replace("SBin[", "").Replace("]", "").Trim().Replace(",", "");
                    tdtl.bin_remark = item.Substring(10, 25).Trim().Replace(",", "");
                    tdtl.bin_number = item.Substring(36, 10).Trim().Replace(",", "");
                    tdtl.bin_bfb = item.Substring(44, 10).Trim().Replace(",", "");
                    tdtl.bin_no = item.Substring(54).Trim().Replace(",", "");
                    data_total_list.Add(tdtl.bin_name, tdtl);
                }
            } 
            #endregion
            #region Bin信息
            string TTTTTTT = "";

            TTTTTTT = string.Join(Environment.NewLine, dic_test_info.Keys);



            //foreach (var item in dic_test_info)
            //{
            //    TTTTTTT += item.Key + Environment.NewLine;
            //}

            HashSet<string> uniqueBinIds = new HashSet<string>(data_total_list.Select(x => x.Value.bin_id.Trim()));

            foreach (var kvp in dic_test_info)
            {
                var item = kvp.Value;
                Total++;

                if (uniqueBinIds.Contains(item.SOFT_BIN.Trim()))
                {
                    foreach (var item_test in data_total_list)
                    {
                        if (item_test.Value.bin_id.Trim() == item.SOFT_BIN.Trim())
                        {
                            data_total_list[item_test.Key].other_bin_number++;
                        }
                    }
                }

                ////这个错误的，需要改掉
                //if (item.SOFT_BIN.Trim() == "0" || item.SOFT_BIN.Trim() == "1" || item.SOFT_BIN.Trim() == "2")
                //{
                //    Pass++;
                //}
            }


            //foreach (var item in dic_test_info)
            //{
            //    Total++;
            //    foreach (var item_test in data_total_list)
            //    {
            //        if (item_test.Value.bin_id.Trim() == item.Value.SOFT_BIN.Trim())
            //        {
            //            data_total_list[item_test.Key].other_bin_number = data_total_list[item_test.Key].other_bin_number + 1;
            //        }
            //    }
            //    if (item.Value.SOFT_BIN.Trim() == "0" || item.Value.SOFT_BIN.Trim() == "1" || item.Value.SOFT_BIN.Trim() == "2")
            //    {
            //        Pass++;
            //    }
            //}
            //<BIN>1|1|P|Pass|95</BIN>
            string bin_temp = "<BIN>{0}|{1}|{2}|{3}|{4}</BIN>";
            string bin_temp_str = "";
            string passBIN = "";
            if (cp.Contains("CP1"))
            {
                passBIN = "58";
            }
            else if(cp.Contains("CP2")|| cp.Contains("CP3"))
            {
                passBIN = "1";
            }
            //foreach (var item in data_total_list)
            //{
            //    if (item.Value.bin_remark.Contains("AllFail"))
            //    {
            //        bin_temp_str += string.Format(bin_temp, item.Value.bin_id, item.Value.bin_id, "F", item.Value.bin_remark, item.Value.other_bin_number) + Environment.NewLine;
            //    }
            //    else
            //    {
            //        bin_temp_str += string.Format(bin_temp, item.Value.bin_id, item.Value.bin_id, "P", item.Value.bin_remark, item.Value.other_bin_number) + Environment.NewLine;
            //    }
            //}
            foreach (var item in data_total_list)
            {
                if (item.Value.bin_id.Equals(passBIN))
                {
                    bin_temp_str += string.Format(bin_temp, item.Value.bin_id, item.Value.bin_id, "P", item.Value.bin_remark, item.Value.other_bin_number) + Environment.NewLine;
                    Pass = item.Value.other_bin_number;
                }
                else
                {
                    bin_temp_str += string.Format(bin_temp, item.Value.bin_id, item.Value.bin_id, "F", item.Value.bin_remark, item.Value.other_bin_number) + Environment.NewLine;
                }
            }
            #endregion
            #region Site信息
            int max_site = -1;

            foreach (var item in dic_test_info)
            {
                string site_n = item.Value.SITE_NUM;
                int parsedSiteNum = int.Parse(site_n);
                if (parsedSiteNum > max_site)
                {
                    max_site = parsedSiteNum;
                }
            }

            //foreach (var item in dic_test_info)
            //{
            //    string site_n = item.Value.SITE_NUM;
            //    if (int.Parse(site_n) > max_site)
            //    {
            //        max_site = int.Parse(site_n);
            //    }
            //}
            Dictionary<string, int[]> site_info = new Dictionary<string, int[]>();
            foreach (var item in dic_test_info)
            {
                string SOFT_BIN = item.Value.SOFT_BIN.Trim();
                int site_n = int.Parse(item.Value.SITE_NUM);
                //修改
                if (site_info.ContainsKey(SOFT_BIN))
                {
                    site_info[SOFT_BIN][site_n]++;
                    site_info[SOFT_BIN][0]++;
                }
                else
                {
                    int[] SiteDataNumber = new int[max_site + 1];
                    for (int i = 0; i < max_site + 1; i++)
                    {
                        SiteDataNumber[i] = 0;
                    }
                    SiteDataNumber[site_n]++;
                    SiteDataNumber[0]++;
                    site_info.Add(SOFT_BIN, SiteDataNumber);
                }
            }
            #endregion
            #region site 测试结果
            string site_list_str = "";
            foreach (var item in site_info)
            {
                site_list_str += "<BIN>" + item.Key + "|";

                for (int i = 0; i < max_site + 1; i++)
                {
                    site_list_str += item.Value[i] + "|";
                }

                site_list_str = site_list_str.Substring(0, site_list_str.Length - 1);
                site_list_str += "</BIN>" + Environment.NewLine;
                // site_list_str += "<BIN>" + item.Key + "|" + item.Value.total_number.ToString() + "|" + item.Value.site1.ToString() + "|" + item.Value.site2.ToString() + "|" + item.Value.site3.ToString() + "|" + item.Value.site4.ToString() + "</BIN>" + Environment.NewLine;
            }
            #endregion
            #region 测试结果
            //string post_list_str = "";
            //foreach (var item in dic_test_info)
            //{
            //    //需要优化，数据量太大
            //    post_list_str += item.Value.x + " " + item.Value.y + " " + item.Value.SOFT_BIN + " " + item.Value.SITE_NUM + Environment.NewLine;
            //    foreach (var item_test in data_total_list)
            //    {
            //        if (item_test.Value.bin_no == item.Value.SOFT_BIN)
            //        {
            //            data_total_list[item_test.Key].other_bin_number = data_total_list[item_test.Key].other_bin_number + 1;
            //        }
            //    }
            //}

            List<string> post_list = new List<string>();

            foreach (var item in dic_test_info)
            {
                post_list.Add($"{item.Value.x} {item.Value.y} {item.Value.SOFT_BIN} {item.Value.SITE_NUM}");

                foreach (var item_test in data_total_list.Where(x => x.Value.bin_no == item.Value.SOFT_BIN))
                {
                    data_total_list[item_test.Key].other_bin_number++;
                }
            }

            string post_list_str = string.Join(Environment.NewLine, post_list);



            #endregion
            //1   bin 描述
            //2   site 信息
            //3   点位详细信息
            //4   开始时间
            //5   结束时间
            //6    LOT_ID
            //7   WAFER_ID
            //8   TEST_DIE
            //9   Pass
            //10  TEST_PG
            //11
            //12  VERSION  TEMPERATURE    NOTCH   XYDIR   LOT_TYPE
            #region XML
            string str5 = "";
            Lot_Id.Trim();
            Wafer_Id.Trim();
            if (Lot_Id.ToUpper().Substring(Lot_Id.Length - 3) == "CP1" || Lot_Id.ToUpper().Substring(Lot_Id.Length - 3) == "CP2" || Lot_Id.ToUpper().Substring(Lot_Id.Length - 3) == "CP3")
            {
                str5 = Lot_Id.ToUpper().Substring(0, Lot_Id.Length - 3);
            }
            else
            {
                str5 = Lot_Id.ToUpper();
            }
            
            string txt = txt_helper.ReadTxtStr(Application.StartupPath + @"\xml_module.txt");
            txt = string.Format(txt, bin_temp_str, site_list_str, post_list_str, Beginning_Time, Ending_Time, str5.ToUpper().Trim(), Wafer_Id.ToUpper().Trim(),
                Total, Pass, Program_name, PRODUCT_ID, OP_NAME, Wafer_no,
                VERSION, TEMPERATURE, NOTCH, XYDIR, LOT_TYPE);
            string save_xml = textBox3.Text + @"\xml"+s+@"\";
            string xy = "";
            if (!Directory.Exists(save_xml))
            {
                Directory.CreateDirectory(save_xml);
                xy = save_xml;
            }
            else
            {
                xy = save_xml;
            }
            string new_file_name = Lot_Id.ToUpper().Replace("CP1", "").Replace("CP2", "").Replace("CP3", "") + "_" + OP_NAME + "_" + Wafer_no.PadLeft(2, '0') + ".xml";//+ "_" + DateTime.Now.ToString("ffff")
            txt_helper.WriteTxtStr(xy + new_file_name, txt);
            #endregion
            // Program.calculate_time = stw.Elapsed;
        }
        #endregion

        #region 解析

        public void JieXi(List<string> pathList,string fileMuBiao,string name)
        {
            DataTable dt = new DataTable();
            DataTable csvTitledt = new DataTable();
            DataTable csvContentdt = new DataTable();
            DataTable csvTitleSumdt = new DataTable();
            DataTable csvContentSumdt = new DataTable();
            DataSet ds = new DataSet();
            int fileCount = pathList.Count;
            if (fileCount <= 0)
            {
                return;
            }

            int over = 0;
            #region 变量
            string Lot_Id = "";
            int Average_time = 0;//平均时间
            int Sum_Day = 0;//总天数
            int Sum_Hour = 0;//总时间
            int Sum_Min = 0;//总时间
            int Sum_Sec = 0;//总时间
            int index = 1;
            
            List<string> binList=new List<string>();
            #endregion
            for (int i = 0; i < fileCount; i++)
            {
                int binIndexST = 0;
                int binIndexED = 0;
                int st = 0;
                dt.Rows.Clear();
                string filePath = pathList[i];
                ds = CSVHelp.CSVToDataTableByStreamReader(filePath);
                csvTitledt = ds.Tables[0];
                csvContentdt = ds.Tables[1];
                string program = "";
                for (int j = 0; j < csvTitledt.Rows.Count; j++)
                {
                    string programName = csvTitledt.Rows[j][0].ToString().Split(':')[0].Replace(" ", "").ToUpper();
                    if (programName.Contains("PROGRAM"))
                    {
                        string[] splitArray = csvTitledt.Rows[j][0].ToString().Split(new[] { ':' }, 2);
                        program = splitArray.Length > 1 ? splitArray[1] : string.Empty;
                    }
                }
                string folderName = Path.GetFileName(Path.GetDirectoryName(filePath));
                folderName = folderName.Split('#')[0];
                int c = 0;
                if (folderName.Contains("CP1"))
                {
                    c = 0;
                }
                else
                {
                    c = 1;
                }
                program = program.Split('\\')[program.Split('\\').Length - 2];
                string mubiao= fileMuBiao+@"\error.txt";
                //if (c==0)
                //{
                //    if (!program.Contains("C1"))
                //    {
                //        CreateOrAppendTextToFile(mubiao, filePath);
                //        continue;
                //    }
                //}
                //else
                //{
                //    if (!program.Contains("C2"))
                //    {
                //        CreateOrAppendTextToFile(mubiao, filePath);
                //        continue;
                //    }
                //}

                if (csvContentdt.Columns.Count==55)
                {
                    csvContentdt.Columns.Remove("info54");
                }
                //for (int j = 0; j < csvTitledt.Rows.Count; j++)
                //{
                //    if (st==1)
                //    {
                //        if ((csvTitledt.Rows[j][0].ToString().Contains("SBin") || csvTitledt.Rows[j][0].ToString().Contains("HBin"))
                //            && csvTitledt.Rows[j + 1][0].ToString().Replace(" ","") == "")
                //        {
                //            binIndexED = j;
                //            continue;
                //        }
                //        else
                //        {
                //            continue;
                //        }
                //    }
                //    if (csvTitledt.Rows[j][0].ToString().Contains("SBin")||csvTitledt.Rows[j][0].ToString().Contains("HBin"))
                //    {
                //        binIndexST = j;
                //        st = 1;
                //    }
                //}

                //if (binIndexST==0&& binIndexED==0)
                //{
                //    binList = binList;
                //}
                //else
                //{
                //    List<string> bins = new List<string>();
                //    for (int j = binIndexST; j <= binIndexED; j++)
                //    {
                //        string bin = csvTitledt.Rows[j][0].ToString();
                //        string[] binStrings = bin.Split(' ');
                //        StringBuilder sb = new StringBuilder();
                //        int x = 0;
                //        if (binStrings[0].Contains("SBin"))
                //        {
                //            for (int q = 0; q < binStrings.Length; q++)
                //            {
                //                string str1 = binStrings[q];
                //                if (str1 != "")
                //                {
                //                    if (x == 2)
                //                    {
                //                        str1 = "0";
                //                    }

                //                    if (x == 3)
                //                    {
                //                        str1 = "0.00%";
                //                    }

                //                    if (x == 4)
                //                    {
                //                        str1 = "0";
                //                    }
                //                    sb.Append(str1 + "    ");
                //                    x++;
                //                }
                //            }
                //        }else if (binStrings[0].Contains("HBin"))
                //        {
                //            for (int q = 0; q < binStrings.Length; q++)
                //            {
                //                string str1 = binStrings[q];
                //                if (str1 != "")
                //                {
                //                    if (x == 1)
                //                    {
                //                        str1 = "0";
                //                    }

                //                    if (x == 2)
                //                    {
                //                        str1 = "0.00%";
                //                    }
                //                    sb.Append(str1 + "    ");
                //                    x++;
                //                }
                //            }
                //        }

                //        sb.Length = sb.Length - 4;
                //        binList.Add(sb.ToString());
                //    }
                //}

                #region 合并抬头
                //binList = RemoveT(binList);
                int indexLot_Id = 0;
                //int indexAverage_Test_Time = 0;
                //int indexTotal_Testing_Time = 0;
                for (int j = 0; j < csvTitledt.Rows.Count; j++)
                {
                    string strLot_Id = csvTitledt.Rows[j][0].ToString();
                    //string strAverage_Test_Time = csvTitledt.Rows[j][0].ToString();
                    //string strTotal_Testing_Time = csvTitledt.Rows[j][0].ToString();
                    string lotid = csvTitledt.Rows[j][0].ToString().Split(':')[0].ToUpper().Replace(" ", "");
                    if (lotid.Contains("LOTID") || csvTitledt.Rows[j][0].ToString().Contains("WaferLotID"))
                    {
                        indexLot_Id = j;
                    }
                    //if (csvTitledt.Rows[j][0].ToString().Contains("Average Test Time(ms)"))
                    //{
                    //    indexAverage_Test_Time = j;
                    //}
                    //if (csvTitledt.Rows[j][0].ToString().Contains("Total Testing Time"))
                    //{
                    //    indexTotal_Testing_Time = j;
                    //}
                }

                Lot_Id = csvTitledt.Rows[indexLot_Id][0].ToString().Split(':')[1].ToUpper();
                ////平均时间
                //string avg = csvTitledt.Rows[indexAverage_Test_Time][0].ToString();
                //string[] avgStrings = avg.Split(':');
                //avg = avgStrings[1].Trim();
                //Average_time += Convert.ToInt32(avg);
                ////总时长
                //string sumTime = csvTitledt.Rows[indexTotal_Testing_Time][0].ToString();
                //sumTime = sumTime.Replace(' ', '\\');
                ////Total\Testing\Time:\0\day\1:50:7
                //string[] sumDayStrings = sumTime.Split('\\');
                //Sum_Day += Convert.ToInt32(sumDayStrings[3]);
                //string[] sumTimeStrings = sumDayStrings[5].Split(':');
                //Sum_Hour += Convert.ToInt32(sumTimeStrings[0]);
                //Sum_Min += Convert.ToInt32(sumTimeStrings[1]);
                //Sum_Sec += Convert.ToInt32(sumTimeStrings[2]);//70
                #endregion

                #region 合并内容
                if (index == 1)
                {
                    csvContentSumdt = ds.Tables[1];
                }
                else if (index != 1)
                {
                    for (int j = 5; j >= 0; j--)//TODO
                    {
                        ds.Tables[1].Rows.RemoveAt(j);
                    }
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        DataRow dr = ds.Tables[1].Rows[j];
                        csvContentSumdt.Rows.Add(dr.ItemArray);
                    }
                }
                index++;

                int sss = csvContentSumdt.Rows.Count;
                #endregion
            }

            if (over==1)
            {
                return;
            }

            int o = 0;
            int binStart = 0;
            for (int i = 0; i < csvContentSumdt.Rows.Count; i++)
            {
                if (o == 1)
                {
                    continue;
                }
                string binStr = csvContentSumdt.Rows[i][1].ToString();
                if (binStr.Contains("Bin#"))
                {
                    binStart = i + 1;
                }
            }

            for (int i = binStart; i < csvContentSumdt.Rows.Count; i++)
            {
                binList.Add(csvContentSumdt.Rows[i][1].ToString());
            }
            binList = RemoveT(binList);
            Dictionary<int, int> binDictionary = new Dictionary<int, int>();
            List<int> iList=new List<int>();
            List<int> bList=new List<int>();
            List<string> binListNew = new List<string>();
            for (int i = 0; i < binList.Count; i++)
            {
                string aaa = binList[i]/*.Split(' ')[0].Split('[')[1].Split(']')[0]*/;
                //binDictionary.Add(Convert.ToInt32(aaa),i);
                iList.Add(Convert.ToInt32(aaa));
            }
            iList.Sort((x, y) => x.CompareTo(y));
            //for (int i = 0; i < iList.Count; i++)
            //{
            //    int a = binDictionary[iList[i]];
            //    bList.Add(a);
            //}

            for (int i = 0; i < iList.Count; i++)
            {
                string space = "";
                if (iList[i].ToString().Length==1)
                {
                    space = "  ";
                }
                string bin = string.Format("SBin[{0}]{1}        0           0.00%", iList[i], space);
                binListNew.Add(bin);
            }
            //if (Sum_Sec % 60 > 0)
            //{
            //    int yu = Sum_Sec % 60;
            //    int cu = Sum_Sec / 60;
            //    Sum_Sec = yu;
            //    Sum_Min += cu;
            //}
            //if (Sum_Min % 60 > 0)
            //{
            //    int yu = Sum_Min % 60;
            //    int cu = Sum_Min / 60;
            //    Sum_Min = yu;
            //    Sum_Hour += cu;
            //}
            //if (Sum_Hour % 24 > 0)
            //{
            //    int yu = Sum_Hour % 60;
            //    int cu = Sum_Hour / 60;
            //    Sum_Hour = yu;
            //    Sum_Day += cu;
            //}
            //ds = CSVHelp.CSVToDataTableByStreamReader(pathList[0]);
            //string sum_time = "Total Testing Time: " + Sum_Day + " day " + Sum_Hour + ":" + Sum_Min + ":" + Sum_Sec;
            //string avg_time = "Average Test Time(ms): " + Convert.ToString(Convert.ToInt32(Average_time / fileCount));
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    if (ds.Tables[0].Rows[i][0].ToString().Contains("Average Test Time(ms)"))
            //    {
            //        ds.Tables[0].Rows[i][0] = avg_time;
            //    }
            //    if (ds.Tables[0].Rows[i][0].ToString().Contains("Total Testing Time"))
            //    {
            //        ds.Tables[0].Rows[i][0] = sum_time;
            //    }
            //}


            //ds.Tables[0].Rows[10][0] = avg_time;
            //ds.Tables[0].Rows[14][0] = sum_time;
            csvTitleSumdt = ds.Tables[0];

            //int s = 0;
            //int start = 0;
            //int end = 0;

            //for (int j = 0; j < csvTitleSumdt.Rows.Count; j++)
            //{
            //    if (s == 1)
            //    {
            //        if ((csvTitleSumdt.Rows[j][0].ToString().Contains("SBin") && csvTitleSumdt.Rows[j + 1][0].ToString() == "")||(csvTitleSumdt.Rows[j][0].ToString().Contains("HBin") && csvTitleSumdt.Rows[j + 1][0].ToString() == ""))
            //        {
            //            end = j;
            //            continue;
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //    }
            //    if (csvTitleSumdt.Rows[j][0].ToString().Contains("SBin")|| csvTitleSumdt.Rows[j][0].ToString().Contains("HBin"))
            //    {
            //        start = j;
            //        s = 1;
            //    }
            //}

            //for (int i = csvTitleSumdt.Rows.Count-1; i >= start; i--)
            //{
            //    csvTitleSumdt.Rows.RemoveAt(i);
            //}
            csvTitleSumdt.Rows.Add("Sample   Pass   Pass%   Fail   Fail%");
            csvTitleSumdt.Rows.Add("0   0   0   0   0");
            for (int i = 0; i < binListNew.Count; i++)
            {
                csvTitleSumdt.Rows.Add(binListNew[i]);
            }

            csvTitleSumdt.Rows.Add("");
            csvTitleSumdt.Rows.Add("");
            csvTitleSumdt.Rows.Add("");
            #region 解析

            //int r = csvContentSumdt.Rows.Count;
            DataTable dtNew = ReadDesFromFile(csvTitleSumdt, csvContentSumdt);
            #endregion

            #region 生成csv

            if (Lot_Id=="")
            {
                return;
            }
            //string[] str = Lot_Id.Split(':');
            //Lot_Id = str[1];
            //name = name.Split(':')[1];
            string mubiaoPath = fileMuBiao + @"\" + name + ".csv";
            CSVHelp.DataTableToCSV(dtNew, mubiaoPath);

            #endregion
        }
        #endregion

        #region 保证数据长度相同
        /// <summary>
        /// 保证数据长度相同
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="len"></param>
        /// <param name="afterFill">后填充/前填充</param>
        /// <returns></returns>
        public string GetSameLenString(object obj, int len, bool afterFill = true)
        {
            string name = obj.ToString();
            //int count = len - name.Length;//不能用这个 汉字和英文占用的长度不同
            int count = len - System.Text.Encoding.Default.GetBytes(name).Length;

            if (afterFill)
            {
                for (int i = 0; i < count; i++)
                {
                    name += " ";
                }
                return name;

            }
            else
            {
                string value = "";
                for (int i = 0; i < count; i++)
                {
                    value += " ";
                }
                value += name;
                return value;
            }
        }
        #endregion

        #region txt文件写入
        public void CreateOrAppendTextToFile(string filePath, string content)
        {
            if (!File.Exists(filePath))
            {
                // 文件不存在，创建新文件并写入内容
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(content);
                }
            }
            else
            {
                // 文件已存在，在文件末尾追加内容
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(content);
                }
            }
        }
        #endregion
    }
}
