using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using Ionic.Zip;

namespace Test
{
    public partial class Form1 : Form
    {
        //List<string> filePathList = new List<string>();
        //List<string> fileNameList = new List<string>();
        private static System.Timers.Timer timer;
        public Form1()
        {
            InitializeComponent();
        }

        #region 原始文件夹
        private void button1_Click(object sender, EventArgs e)
        {
            //Clear();
            string file = "";
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                file = dilog.SelectedPath;
            }
           
            textBox2.Text = file;
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
        public List<string> Director2(string dir)
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
                list.Add(files[i].Name);//添加文件名到列表中 
            }
            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo dd in directs)
            {
                Director2(dd.FullName);
            }

            return list;
        }
        public List<string> Director(string dir)
        {
            List<string> list = new List<string>();
            DirectoryInfo d = new DirectoryInfo(dir);
            FileSystemInfo[] files = d.GetFileSystemInfos(); // 获取目录下所有文件和文件夹
            files = files.OrderBy(y => y.Name, new FileNameComparerClass()).ToArray();

            foreach (FileSystemInfo item in files)
            {
                if (item is FileInfo) // 如果是文件
                {
                    list.Add(item.Name);
                }
                else if (item is DirectoryInfo) // 如果是文件夹
                {
                    list.Add($"{item.Name}");
                    //list.AddRange(Director(item.FullName)); // 递归获取子文件夹中的文件
                }
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


        private void btnStart_Click(object sender, EventArgs e)
        {
            MethodTimer2(null, null);
            ////int tim = Convert.ToInt32(txtSysExecuteTime.Text) * 1000 * 60 * 60;
            ////System.Timers.Timer time = new System.Timers.Timer(tim); //实例化Timer类，规定每隔30秒执行一次
            ////time.Elapsed += new System.Timers.ElapsedEventHandler(MethodTimer2); //当达到规定的时间内执行aa 这个方法
            ////time.AutoReset = true;//false 执行一次，true 一直执行
            ////time.Enabled = true;//设置是否执行time.Elapsed 时间

            //// 获取当前时间
            //DateTime now = DateTime.Now;

            //// 计算下一次执行时间，默认为当天的早上 8 点
            //DateTime nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, 9, 0, 0);

            //// 如果当前时间已经过了今天的早上 8 点，则下一次执行时间推迟到明天早上 8 点
            //if (now > nextExecutionTime)
            //{
            //    nextExecutionTime = nextExecutionTime.AddDays(1);
            //}

            //// 计算当前时间与下一次执行时间的差值
            //TimeSpan timeUntilNextExecution = nextExecutionTime - now;

            //// 创建定时器并设置相关属性
            //timer = new System.Timers.Timer();
            //timer.Elapsed += new ElapsedEventHandler(MethodTimer2);
            //timer.Interval = timeUntilNextExecution.TotalMilliseconds;
            //timer.AutoReset = false;
            //timer.Start();
        }

        private void MethodTimer2(object sender, ElapsedEventArgs e)
        {
            string filename = this.textBox2.Text;
            List<string> filePathList = new List<string>();
            List<string> fileNameList = new List<string>();
            GetPathInfo(filename, ref filePathList, ref fileNameList);
            foreach (string s in filePathList)
            {
                if (s.Contains(".zip"))
                {
                    GetZip(s, filename);
                }
            }


            filePathList = new List<string>();
            fileNameList = new List<string>();
            GetPathInfo(filename, ref filePathList, ref fileNameList);
            //filename = filename + @"\" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string pattern = @"FT1-[A-Za-z0-9]{3}_[A-Za-z0-9]{2}";
            List<string> newfileList = new List<string>();
            newfileList = filePathList; List<string> processlotPathList = new List<string>();
            processlotPathList = OrganizeFiles(fileNameList, filename);
            foreach (string s in newfileList)
            {
                //filePathList = new List<string>();
                //fileNameList = new List<string>();
                //GetPathInfo(s1, ref filePathList, ref fileNameList);
                FileAttributes attributes = File.GetAttributes(s);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    int index = s.IndexOf("_");

                    if (index >= 0)
                    {
                        string str = s.Substring(0, index);
                        string name = str.Split('\\')[str.Split('\\').Length - 1];
                        foreach (string path in processlotPathList)
                        {
                            if (path.Contains(name))
                            {
                                DirectoryInfo sourceFolder = new DirectoryInfo(s);
                                DirectoryInfo destinationFolder = new DirectoryInfo(Path.Combine(path, Path.GetFileName(s)));

                                // 创建目标文件夹
                                destinationFolder.Create();

                                // 复制源文件夹到目标文件夹
                                CopyFolderRecursive(sourceFolder, destinationFolder);
                            }
                        }
                    }
                }
                else
                {
                    Match match = Regex.Match(s, pattern);
                    int index = s.IndexOf("-FT1");

                    if (index >= 0)
                    {
                        string str = s.Substring(0, index);
                        if (match.Success)
                        {
                            string name = str.Split('\\')[str.Split('\\').Length - 1];
                            foreach (string path in processlotPathList)
                            {
                                if (path.Contains(name))
                                {
                                    // 确保目标文件夹存在
                                    Directory.CreateDirectory(path);

                                    // 获取源文件名
                                    string sourceFileName = Path.GetFileName(s);

                                    // 构建目标文件的路径
                                    string destinationFilePath = Path.Combine(path, sourceFileName);

                                    // 复制文件
                                    File.Copy(s, destinationFilePath, true);
                                }
                            }
                        }
                    }
                }
                //foreach (string s in filePathList)
                //{
                   
                    //if (s.Contains('.'))
                    //{
                    //    string str = s.Split('\\')[s.Split('\\').Length - 1].Split('.')[0];
                    //    int count = str.Split('-').Length;
                    //    if (count == 5)
                    //    {
                    //        string name = str.Split('-')[0] + "-" + str.Split('-')[1] + "-" + str.Split('-')[2];
                    //        foreach (string path in processlotPathList)
                    //        {
                    //            if (path.Contains(name))
                    //            {
                    //                // 确保目标文件夹存在
                    //                Directory.CreateDirectory(path);

                    //                // 获取源文件名
                    //                string sourceFileName = Path.GetFileName(s);

                    //                // 构建目标文件的路径
                    //                string destinationFilePath = Path.Combine(path, sourceFileName);

                    //                // 复制文件
                    //                File.Copy(s, destinationFilePath, true);
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    string str = s.Split('\\')[s.Split('\\').Length - 1].Split('.')[0];
                    //    int count = str.Split('-').Length;
                    //    if (count == 5)
                    //    {
                    //        string name = str.Split('-')[0] + "-" + str.Split('-')[1] + "-" + str.Split('-')[2].Split('_')[0];
                    //        foreach (string path in processlotPathList)
                    //        {
                    //            if (path.Contains(name))
                    //            {
                    //                DirectoryInfo sourceFolder = new DirectoryInfo(s);
                    //                DirectoryInfo destinationFolder = new DirectoryInfo(Path.Combine(path, Path.GetFileName(s)));

                    //                // 创建目标文件夹
                    //                destinationFolder.Create();

                    //                // 复制源文件夹到目标文件夹
                    //                CopyFolderRecursive(sourceFolder, destinationFolder);
                    //            }
                    //        }
                    //    }
                    //}
                //}
                
                
            }
            Change(filename);
            BackupAndDelete(filename);




            //CompressFolderToZip(filename, filename + ".zip");
            //UploadFileTwo(filename + ".zip", ftpname, "FT_Datalog", "seagatek");
            //DeleteFolder(filename);
            //DeleteZipFile(filename + ".zip");
            //BackupZipFiles(this.textBox2.Text, beifenname);

        }

        #region 解压文件

        public void GetZip(string filename,string filepath)
        {
            using (ZipFile zip = new ZipFile(filename))
            {
                zip.ExtractAll(filepath, ExtractExistingFileAction.OverwriteSilently);
            }

            DeleteZipFile( filename);
        }
        #endregion

        #region 复制文件夹
        private void CopyFolderRecursive(DirectoryInfo sourceFolder, DirectoryInfo destinationFolder)
        {
            // 复制源文件夹中的所有文件
            foreach (FileInfo file in sourceFolder.GetFiles())
            {
                string destinationFilePath = Path.Combine(destinationFolder.FullName, file.Name);
                file.CopyTo(destinationFilePath, false);
            }

            // 递归复制源文件夹中的所有子文件夹
            foreach (DirectoryInfo subFolder in sourceFolder.GetDirectories())
            {
                string destinationSubFolderPath = Path.Combine(destinationFolder.FullName, subFolder.Name);
                CopyFolderRecursive(subFolder, new DirectoryInfo(destinationSubFolderPath));
            }
        }
        #endregion

        #region 上传文件

        public void UploadFile(string filePath, string targetFtpPath, string username, string password)
        {
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                return;
            }

            // 创建 FTP 请求
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Path.Combine(targetFtpPath, Path.GetFileName(filePath)));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);

            // 读取文件内容
            byte[] fileBytes = File.ReadAllBytes(filePath);

            try
            {
                // 获取请求的 FTP 数据流
                using (Stream requestStream = request.GetRequestStream())
                {
                    // 将文件内容写入 FTP 数据流
                    requestStream.Write(fileBytes, 0, fileBytes.Length);
                }

                // 获取 FTP 响应
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                // 关闭响应
                response.Close();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public void UploadFile(string filePath, string targetFolderPath)
        {
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                return;
            }

            // 获取目标文件夹路径
            string targetPath = Path.Combine(targetFolderPath, Path.GetFileName(filePath));

            try
            {
                // 复制文件到目标文件夹
                File.Copy(filePath, targetPath, true);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void UploadFileTwo(string filePath, string ftpUrl, string userName, string password)
        {
            // 创建FTP请求对象
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + Path.GetFileName(filePath));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(userName, password);

            // 读取压缩文件内容
            byte[] fileContents = File.ReadAllBytes(filePath);

            // 将文件内容写入请求流
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            // 发送FTP请求并获取响应
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine("上传完毕，状态为: " + response.StatusDescription);
            }
        }
        #endregion

        #region 处理文件

        public List<string> OrganizeFiles(List<string> fileNames,string fileMuBiao)
        {
            //GIT-2318g2C-01-FT1-FT02

            List<string> processlotPathList = new List<string>();
            //foreach (string filenames in fileNames)
            //{
            //    int count = filenames.Split('-').Length;
            //    if (count<3)
            //    {
            //        string mubiaoPath = fileMuBiao + @"\其他";
            //        if (!Directory.Exists(mubiaoPath))
            //        {
            //            CreateFolderWithPermission(mubiaoPath, FileSystemRights.FullControl, AccessControlType.Allow);
            //            Change(fileMuBiao);
            //        }
            //        processlotPathList.Add(mubiaoPath);
            //    }
            //}
            List<string> cusList = new List<string>();
            List<string> lotList = new List<string>();
            List<string> processlotList = new List<string>();
            foreach (string filenames in fileNames)
            {
                string path = fileMuBiao + "\\" + filenames;
                FileAttributes attributes = File.GetAttributes(path);
                if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    int index = filenames.IndexOf("_");

                    if (index >= 0)
                    {
                        string result = filenames.Substring(0, index);
                        processlotList.Add(result);
                    }
                }
                else
                {
                    string pattern = @"FT1-[A-Za-z0-9]{3}_[A-Za-z0-9]{2}";
                    Match match = Regex.Match(filenames, pattern);
                    if (match.Success)
                    {
                        //string processlot = filenames.Split('-')[0] + "-" + filenames.Split('-')[1] + "-" +
                        //                    filenames.Split('-')[2].Split('_')[0];
                        int index = filenames.IndexOf("-FT1");

                        if (index >= 0)
                        {
                            string result = filenames.Substring(0, index);
                            processlotList.Add(result);
                        }
                    }
                }
              


                //    int count = filenames.Split('-').Length;
                ////if (count < 3)
                ////{
                ////    string mubiaoPath = fileMuBiao + @"\其他";
                ////    if (!Directory.Exists(mubiaoPath))
                ////    {
                ////        CreateFolderWithPermission(mubiaoPath, FileSystemRights.FullControl, AccessControlType.Allow);
                ////        Change(fileMuBiao);
                ////    }
                ////    processlotPathList.Add(mubiaoPath);
                ////}
                //if (count == 5)
                //{
                //    string processlot = filenames.Split('-')[0] + "-" + filenames.Split('-')[1] + "-" +
                //                        filenames.Split('-')[2].Split('_')[0];
                //    processlotList.Add(processlot);
                //}
            }
            processlotList = RemoveT(processlotList);
            foreach (string fileName in processlotList)
            {
                string processlot = fileName.Split('-')[0];
                cusList.Add(processlot);
            }
            cusList = RemoveT(cusList);
            foreach (string fileName in processlotList)
            {
                string processlot = fileName.Split('-')[0] + "-" + fileName.Split('-')[1];
                lotList.Add(processlot);
            }
            lotList = RemoveT(lotList);

            #region 创建文件夹

            if (!Directory.Exists(fileMuBiao))
            {
                CreateFolderWithPermission(fileMuBiao, FileSystemRights.FullControl, AccessControlType.Allow);
                Change(fileMuBiao);
            }

            List<string> cusPathList = new List<string>();
            foreach (string s in cusList)
            {
                string mubiaoPath = fileMuBiao + @"\" + s;
                if (!Directory.Exists(mubiaoPath))
                {
                    CreateFolderWithPermission(mubiaoPath, FileSystemRights.FullControl, AccessControlType.Allow);
                    Change(fileMuBiao);
                }
                cusPathList.Add(mubiaoPath);
            }
            List<string> lotPathList = new List<string>();
            foreach (string s in lotList)
            {
                string str = s.Split('-')[0];
                string lot = s.Split('-')[1];
                foreach (string cus in cusPathList)
                {
                    if (cus.Contains(str))
                    {
                        string mubiaoPath = cus + @"\" + lot;
                        if (!Directory.Exists(mubiaoPath))
                        {
                            CreateFolderWithPermission(mubiaoPath, FileSystemRights.FullControl, AccessControlType.Allow);
                            Change(fileMuBiao);
                            lotPathList.Add(mubiaoPath);
                        }
                    }
                }
            }
            foreach (string s in processlotList)
            {
                string str = s.Split('-')[0] + "\\" + s.Split('-')[1];
                foreach (string lot in lotPathList)
                {
                    if (lot.Contains(str))
                    {
                        string mubiaoPath = lot + @"\" + s;
                        if (!Directory.Exists(mubiaoPath))
                        {
                            CreateFolderWithPermission(mubiaoPath, FileSystemRights.FullControl, AccessControlType.Allow);
                            Change(fileMuBiao);
                            processlotPathList.Add(mubiaoPath);
                        }
                    }
                }
            }
            #endregion
            processlotPathList = RemoveT(processlotPathList);
            return processlotPathList;
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

        #region 创建文件
        public void CreateFolderWithPermission(string folderPath, FileSystemRights rights, AccessControlType controlType)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    Console.WriteLine($"文件夹已存在：{folderPath}");
                    return;
                }
                DirectoryInfo directoryInfo = Directory.CreateDirectory(folderPath);

                // 获取文件夹的访问控制列表
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                // 添加文件夹的访问规则
                directorySecurity.AddAccessRule(new FileSystemAccessRule(Environment.UserName, rights, controlType));

                // 设置文件夹的访问控制列表
                directoryInfo.SetAccessControl(directorySecurity);

                Console.WriteLine($"文件夹已创建：{folderPath}，权限设置为：{rights}，类型：{controlType}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建文件夹失败: {ex.Message}");
            }
        }

        public void Change(string folderPath)
        {
            try
            {
                // 获取文件夹信息
                DirectoryInfo folderInfo = new DirectoryInfo(folderPath);

                // 获取当前权限
                DirectorySecurity folderSecurity = folderInfo.GetAccessControl();

                // 移除只读属性
                folderSecurity.RemoveAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.Read, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                // 设置新权限
                folderInfo.SetAccessControl(folderSecurity);

                Console.WriteLine("文件夹权限已更改为可写。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误: " + ex.Message);
            }
        }
        #endregion

        #region 压缩文件
        public static void CompressFolderToZip(string filePath, string zipPath)
        {
            //// 使用临时目录存储 ZIP 文件
            //string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            //Directory.CreateDirectory(tempDirectory);

            //// 生成 ZIP 文件的完整路径
            //string tempZipFilePath = Path.Combine(tempDirectory, "compressed.zip");

            //// 创建一个新的 ZIP 文件
            //using (var zipArchive = ZipFile.Open(tempZipFilePath, ZipArchiveMode.Create))
            //{
            //    // 枚举文件夹中的文件，并逐个添加到 ZIP 文件中
            //    var files = new DirectoryInfo(folderPath).EnumerateFiles("*", SearchOption.AllDirectories);
            //    foreach (var file in files)
            //    {
            //        // 生成相对路径来保持文件的目录结构
            //        string relativePath = file.FullName.Substring(folderPath.Length).TrimStart('\\');

            //        // 创建新的 ZipEntry 并为文件流式传输数据
            //        var entry = zipArchive.CreateEntry(relativePath, CompressionLevel.Optimal);
            //        using (var stream = entry.Open())
            //        using (var fileStream = file.OpenRead())
            //        {
            //            fileStream.CopyTo(stream);
            //        }
            //    }
            //}

            //// 移动 ZIP 文件到目标路径
            //File.Move(tempZipFilePath, zipFilePath);

            //// 删除临时目录及其内容
            //Directory.Delete(tempDirectory, true);

            //Console.WriteLine("文件夹已成功压缩为ZIP文件：" + zipFilePath);

            // 创建一个 ZipFile 对象用于压缩
            using (ZipFile zip = new ZipFile(zipPath, System.Text.Encoding.Default))
            {
                zip.AddDirectory(filePath);//添加文件夹
                zip.Save();
            }

            // 输出成功信息
            Console.WriteLine("文件已成功压缩为：" + zipPath);
        }
        #endregion

        #region 删除文件
        public static void DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                // 删除文件夹及其内容
                Directory.Delete(folderPath, true);

                Console.WriteLine("文件夹已成功删除：" + folderPath);
            }
            else
            {
                Console.WriteLine("文件夹不存在：" + folderPath);
            }
        }

        public static void DeleteZipFile(string zipFilePath)
        {
            if (File.Exists(zipFilePath))
            {
                // 删除 ZIP 文件
                File.Delete(zipFilePath);

                Console.WriteLine("ZIP 文件已成功删除：" + zipFilePath);
            }
            else
            {
                Console.WriteLine("ZIP 文件不存在：" + zipFilePath);
            }
        }

        public static void BackupAndDeleteFiles(string targetDirectory, string backupPath)
        {
            // 获取目标文件夹中的所有文件
            string[] files = Directory.GetFiles(targetDirectory);

            // 遍历并备份所有文件
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string backupFilePath = Path.Combine(backupPath, fileName);

                // 备份文件
                File.Copy(file, backupFilePath, true);

                // 检查备份是否成功
                if (File.Exists(backupFilePath))
                {
                    // 备份成功，删除原文件
                    File.Delete(file);
                }
                else
                {
                    // 备份失败，不删除原文件
                    Console.WriteLine("备份文件失败: " + file);
                }
            }
        }

        public void BackupZipFiles(string sourceFolderPath, string destinationFolder)
        {
            try
            {
                // 检查源文件夹是否存在
                if (!Directory.Exists(sourceFolderPath))
                {
                    Console.WriteLine("源文件夹不存在！");
                    return;
                }

                // 检查目标文件夹是否存在，如果不存在则创建
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                // 备份所有的 zip 文件
                string[] zipFiles = Directory.GetFiles(sourceFolderPath, "*.zip");
                foreach (string zipFile in zipFiles)
                {
                    // 获取目标文件的路径
                    string destinationPath = Path.Combine(destinationFolder, Path.GetFileName(zipFile));

                    // 复制 zip 文件到目标路径
                    File.Copy(zipFile, destinationPath);

                    Console.WriteLine("备份成功：" + destinationPath);
                }

                // 删除源文件夹下所有文件
                string[] files = Directory.GetFiles(sourceFolderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine("删除成功：" + file);
                }

                // 检查备份是否成功（如果源文件夹仍有文件，则备份失败）
                if (Directory.GetFiles(sourceFolderPath).Length != 0)
                {
                    Console.WriteLine("备份失败！");
                    return;
                }

                Console.WriteLine("备份成功！");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("备份过程中出现异常：" + ex.Message);
                return;
            }
        }

        public void BackupAndDelete(string sourcePath)
        {
            // 删除源目录下的所有文件和文件夹
            string[] files = Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (!file.Contains("-"))
                {
                    if (!file.Contains("."))
                    {
                        continue;
                    }
                }
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            string[] directories = Directory.GetDirectories(sourcePath);

            var filteredDirectories = directories.Where(directory => directory.Contains("-"));

            foreach (string directory in filteredDirectories)
            {
                Directory.Delete(directory, true);
            }
        }
        #endregion

        #region 获取文件

        public void GetPathInfo(string file,ref List<string> filePathList, ref List<string> fileNameList)
        {
            List<string> fileList = new List<string>();
            if (file == "")
            {
                return;
            }
            fileList = Director(file);
            for (int i = 0; i < fileList.Count; i++)
            {
                string str = file + "\\" + fileList[i].Split(',')[0];
                string name = fileList[i]/*.Split('.')[0]*/;
                filePathList.Add(str);
                fileNameList.Add(name);
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //btnStart_Click(null, null);
        }

        #region 备份文件
        private void button3_Click(object sender, EventArgs e)
        {
            string file = "";
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                file = dilog.SelectedPath;
            }
            //textBox1.Text = file;
        }
        #endregion

    }
}
