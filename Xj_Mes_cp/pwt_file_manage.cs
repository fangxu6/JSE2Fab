using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public class pwt_file_manage
    {

        //http://192.168.5.242:10000/API/
        /// <summary>
        /// 上传文件到文件服务器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filepath"></param>
        /// <param name="res_file_name"></param>
        /// <param name="res_upload_file_name"></param>
        /// <param name="res"></param>
        public static void UpLoadFile(string url, string filepath, out string res_file_name, out string res_upload_file_name, out string res)
        {


            pwt_system_comm.WebHelper wh = new pwt_system_comm.WebHelper();



            FileInfo fi = new FileInfo(filepath);
            string file_name = fi.Name;
            string file_type = "";



            string upfileName = "";
            if (file_name.Contains('.'))
            {
                file_type = file_name.Substring(file_name.LastIndexOf('.') + 1);
                upfileName = (DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + file_type).PadRight(30, ' ');
            }
            else
            {
                file_type = "";
                upfileName = (DateTime.Now.ToString("yyyyMMddHHmmssffff")).PadRight(30, ' ');
            }

            string fileInfo = FileToBase64String(filepath);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("file_name", upfileName);
            dic.Add("file_info", fileInfo);


            res = wh.HttpPostRequest(url+"/API/", dic);
            res_file_name = file_name;
            res_upload_file_name = upfileName;

        }


        public static void ShowFile(string url, string filename)
        {


            try
            {
                //从注册表中读取默认浏览器可执行文件路径
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                string s = key.GetValue("").ToString();
                //s就是你的默认浏览器，不过后面带了参数，把它截去，不过需要注意的是：不同的浏览器后面的参数不一样！
                //"D:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                System.Diagnostics.Process.Start(s.Substring(0, s.Length - 8), url + "/File/" + filename);

            }
            catch (Exception )
            {
                MessageBox.Show("系统未设置基础信息\r\n请收到打开网址:" + url + "/File/" + filename, "系统提示");
                Clipboard.SetDataObject(url + "/File/" + filename);
            }



        }
        public static string FileToBase64String(byte[] FileBty)
        {
            //Convert.ToBase64String(File.ReadAllBytes(picInfo.PicPath));
            return Convert.ToBase64String(FileBty);
        }
        public static string FileToBase64String(string FilePath)
        {
            return Convert.ToBase64String(File.ReadAllBytes(FilePath));
        }

        public static void Base64StringToFile(string Base64Text, string FilePath)
        {
            File.WriteAllBytes(FilePath, Convert.FromBase64String(Base64Text));
        }
    }
}
