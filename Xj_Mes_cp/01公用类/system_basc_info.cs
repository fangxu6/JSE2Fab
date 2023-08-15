//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using system_dll_com;
//using pwt_system_comm;
//using system_basic;

//namespace Xj_Mes_by
//{
//    public static class system_basc_info
//    {
//        public static string system_his_user_filepath
//        {
//            get
//            {
//                return System.Environment.CurrentDirectory + "\\his_user.ini";
//            }
//        }

//        public static string system_temp_data
//        {
//            get
//            {
//                return System.Environment.CurrentDirectory + "\\temp_data.data";
//            }
//        }

//        public static string system
//        {
//            get
//            {
//                return System.Environment.CurrentDirectory + "\\system.ini";
//            }
//        }

       
//        /// <summary>
//        /// 部门
//        /// </summary>
//        public static string dept
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "dept", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 角色
//        /// </summary>
//        public static string user_role
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "user_role", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 账号
//        /// </summary>
//        public static string user_code
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "user_code", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 用户名称
//        /// </summary>
//        public static string user_name
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "user_name", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 电脑名称
//        /// </summary>
//        public static string computer_name
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "computer_name", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 系统版本
//        /// </summary>
//        public static string Vison
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "Vison", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 电脑IP
//        /// </summary>
//        public static string IP
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "ip", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 电脑Mac地址
//        /// </summary>
//        public static string MAC
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "mac", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 系统名称
//        /// </summary>
//        public static string systemName
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "systemName", base_info.system_temp_data);
//            }
//        }
//        /// <summary>
//        /// 系统简称
//        /// </summary>
//        public static string systemMinName
//        {
//            get
//            {
//                return IniHelper.ReadIniKeys("system_info", "systemMinName", base_info.system_temp_data);
//            }
//        }


//        //当前地址
//        private static string _mCurrentPath;
//        private static string Platform
//        {
//            get
//            {
//                return Environment.OSVersion.Platform.ToString();
//            }
//        }
//        public static string CurrentPath
//        {
//            get
//            {
//                if (Platform.Equals("WinCE"))
//                {
//                    _mCurrentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
//                }
//                else if (Platform.Equals("Win32NT"))
//                {
//                    _mCurrentPath = System.IO.Directory.GetCurrentDirectory();
//                }
//                return _mCurrentPath;
//            }
//        }
//    }
//}
