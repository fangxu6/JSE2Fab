using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Test
{
    public class FtpUploader
    {
        public void CopyFolderToFTP(string localFolderPath, string ftpServerUri, string username, string password)
        {
            //// 创建FTP请求对象
            //FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpServerUri);
            //ftpRequest.Credentials = new NetworkCredential(username, password);
            //ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory; // 创建目标文件夹

            //// 发送创建文件夹的请求
            //using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
            //{
            //    Console.WriteLine($"FTP 文件夹创建成功：{ftpServerUri}");
            //}

            // 复制本地文件夹到FTP
            CopyFolder(localFolderPath, ftpServerUri, username, password);

            Console.WriteLine("文件夹复制完成");
        }

        private static void CopyFolder(string sourceFolderPath, string ftpServerUri, string username, string password)
        {
            string str= Path.GetFileName(sourceFolderPath);
            // 创建目标文件夹
            FtpWebRequest makeDirRequest = (FtpWebRequest)WebRequest.Create(ftpServerUri+ str+"/");
            makeDirRequest.Credentials = new NetworkCredential(username, password);
            makeDirRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            makeDirRequest.UseBinary = true;
            makeDirRequest.UsePassive = true;

            using (FtpWebResponse makeDirResponse = (FtpWebResponse)makeDirRequest.GetResponse())
            {
                Console.WriteLine($"创建文件夹：{ftpServerUri}");
            }

            // 获取源文件夹下的所有文件和子文件夹
            string[] files = Directory.GetFiles(sourceFolderPath);
            string[] folders = Directory.GetDirectories(sourceFolderPath);

            // 复制文件到FTP
            foreach (string file in files)
            {
                CopyFile(file, Path.Combine(ftpServerUri, Path.GetFileName(file)), username, password);
            }

            // 递归复制子文件夹到FTP
            foreach (string folder in folders)
            {
                string folderName = Path.GetFileName(folder);
                string newFolder = Path.Combine(ftpServerUri, folderName);
                CopyFolder(folder, newFolder, username, password);
            }
        }

        private static void CopyFile(string sourceFilePath, string destinationFilePath, string username, string password)
        {
            using (FileStream sourceStream = File.OpenRead(sourceFilePath))
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(destinationFilePath);
                ftpRequest.Credentials = new NetworkCredential(username, password);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;

                using (Stream destinationStream = ftpRequest.GetRequestStream())
                {
                    sourceStream.CopyTo(destinationStream);
                }

                using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    Console.WriteLine($"文件上传成功：{destinationFilePath}");
                }
            }
        }
    }
}
