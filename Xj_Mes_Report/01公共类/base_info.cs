using System;using system_comm;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using system_dll_com;
using System.Data;
using System.Windows.Forms;
using pwt_system_comm;


namespace Xj_Mes_Report
{
    public static class base_info
    {

        public static string system_his_user_filepath
        {
            get
            {
                return System.Environment.CurrentDirectory + "\\his_user.ini";
            }
        }

        public static string system_temp_data
        {
            get
            {
                return System.Environment.CurrentDirectory + "\\temp_data.data";
            }
        }

        public static string system
        {
            get
            {
                return System.Environment.CurrentDirectory + "\\system.ini";
            }
        }
        public static string system_Final_filepath
        {
            get
            {
                return Application.StartupPath + @"\5_config" + "\\Final_Inspection.ini";
            }
        }
        public static string system_Pack_filepath
        {
            get
            {
                return Application.StartupPath + @"\5_config" + "\\Pack_Info.ini";
            }
        }

 
        /// <summary>
        /// 部门
        /// </summary>
        public static string dept
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "dept", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 角色
        /// </summary>
        public static string user_role
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "user_role", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 账号
        /// </summary>
        public static string user_code
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "user_code", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public static string user_name
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "user_name", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 电脑名称
        /// </summary>
        public static string computer_name
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "computer_name", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 系统版本
        /// </summary>
        public static string Vison
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "Vison", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 电脑IP
        /// </summary>
        public static string IP
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "ip", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 电脑Mac地址
        /// </summary>
        public static string MAC
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "mac", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 系统名称
        /// </summary>
        public static string systemName
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "systemName", base_info.system_temp_data);
            }
        }
        /// <summary>
        /// 系统简称
        /// </summary>
        public static string systemMinName
        {
            get
            {
                return IniHelper.ReadIniKeys("system_info", "systemMinName", base_info.system_temp_data);
            }
        }
       
    }
}
