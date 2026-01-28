using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormTool.TskUtil
{
    class TskFileHelper
    {
        private const string DEFAULT_PATH = @"D:\New-Tsk\";
        private static string _savePath = DEFAULT_PATH;

        public static string SavePath
        {
            get => _savePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _savePath = DEFAULT_PATH;
                    return;
                }

                // 规范化路径
                string normalizedPath = value.Trim();
                if (!normalizedPath.EndsWith("\\"))
                {
                    normalizedPath += "\\";
                }

                try
                {
                    // 确保路径有效
                    if (!Directory.Exists(normalizedPath))
                    {
                        Directory.CreateDirectory(normalizedPath);
                    }
                    _savePath = normalizedPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($@"设置保存路径失败: {ex.Message}
使用默认路径: {DEFAULT_PATH}",
                        "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _savePath = DEFAULT_PATH;
                }
            }
        }


        public static bool ValidateFiles(string excelPath, string tskPath)
        {
            // 检查文件路径是否为空
            if (string.IsNullOrWhiteSpace(excelPath))
            {
                MessageBox.Show(@"请选择Excel文件", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tskPath))
            {
                MessageBox.Show(@"请选择TSK文件", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 检查文件是否存在
            if (!File.Exists(excelPath))
            {
                MessageBox.Show($@"Excel文件不存在: {excelPath}", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!File.Exists(tskPath))
            {
                MessageBox.Show($@"TSK文件不存在: {tskPath}", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 验证Excel文件格式
            string excelExtension = Path.GetExtension(excelPath).ToLower();
            //if (excelExtension != ".xlsx" && excelExtension != ".xls" && excelExtension != ".csv")
            //{
            //    MessageBox.Show("请选择有效的Excel文件 (.xlsx, .xls, .csv)", "错误提醒", MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);
            //    return false;
            //}

            // 确保输出目录存在
            try
            {
                //Directory.CreateDirectory(newTskPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"创建输出目录失败: {ex.Message}", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 检查文件是否可以访问（未被其他程序锁定）
            try
            {
                using (File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                }

                using (File.Open(tskPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                }
            }
            catch (IOException)
            {
                MessageBox.Show(@"无法访问文件，文件可能被其他程序占用", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"文件访问错误: {ex.Message}", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public static string GetSaveFilePath()
        {
            // 获取SaveFileTo文本框的值
            string path = SavePath;

            // 如果路径为空，使用默认路径
            if (string.IsNullOrWhiteSpace(path))
            {
                path = @"D:\New-Tsk\";
            }

            // 确保路径以反斜杠结尾
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            // 确保目录存在
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"创建目录失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return @"D:\New-Tsk\"; // 返回默认路径
            }

            return path;
        }
    }
}