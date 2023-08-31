using System;

using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Configuration;
using system_comm;
using pwt_system_comm;

namespace Xj_Mes_Report
{
    public class db_deal
    {
        private static string db = "pawoteWJ";
        private static string StrUid = "sa";
        private static string db_type = "SQlServer";
        private static string db_path = "";
        private static string StrServerip = "www.wxsc-tech.com";
        private static string StrPwd = "QAZWSXqazwsx@123";


        public db_deal()
        {
            #region 加载配置文件系统数据库链接信息 db_system.pdk

          string[] strlist = IniHelper.ReadIniAllKeys("XJDZ_db", Application.StartupPath + "\\db_system.pdk");

             // string[] strlist = IniHelper.ReadIniAllKeys("XJDZ_XIJI_db", Application.StartupPath + "\\db_system.pdk");
            foreach (string str in strlist)
            {
                //数据库名称
                if (str.Split('=')[0] == "db")
                    db = str.Split('=')[1];
                //数据库账号
                if (str.Split('=')[0] == "StrUid")
                    StrUid = (str.Split('=')[1]);
                //数据密码
                if (str.Split('=')[0] == "StrPwd")
                    StrPwd = (str.Split('=')[1]);
                //链接数据库类型
                if (str.Split('=')[0] == "db_type")
                    db_type = str.Split('=')[1];
                //Access地址
                if (str.Split('=')[0] == "db_path")
                    db_path = str.Split('=')[1];
                //服务IP
                if (str.Split('=')[0] == "StrServerip")
                    StrServerip = (str.Split('=')[1]);
            }
            #endregion
        }



        public void Change_DbType(string DbType)
        {
            db_type = DbType;
        }
        public void Change_db_path(string Db_Path)
        {
            db_path = Db_Path;
        }


        /// <summary>
        /// 保存动作日志
        /// </summary>
        /// <param name="FunctionName"></param>
        /// <param name="FunctionID"></param>
        public void SaveOperatingLogs(string FunctionName, String FunctionID)
        {

            string op_log = string.Format("[dbo].[System_log_operation_insert] '{0}','{1}','{2}'", FunctionName, FunctionID, base_info.user_code);
            Sys_Exe_Data(op_log);
        }

        public void Exe_Data(string strSQL)
        {

            Sys_Exe_Data(string.Format("[dbo].[System_log_insert] '" + strSQL.Replace('\'','~') + "','" + base_info.user_code + "'"));
            Sys_Exe_Data(strSQL);
        }
         
        public DataTable Get_Data(string strSQL)
        {
            Sys_Exe_Data(string.Format("[dbo].[System_log_insert] '" + strSQL.Replace('\'', '~') + "','" + base_info.user_code + "'"));
            DataTable dt = Sys_Get_Data(strSQL);
            return dt;
        }


        public DataSet Get_Dset(string strSQL)
        {
            Sys_Exe_Data(string.Format("[dbo].[System_log_insert] '" + strSQL.Replace('\'', '~') + "','" + base_info.user_code + "'"));
            DataSet dst = Sys_Get_Dset(strSQL);
            return dst;
        }

        #region SqlServer
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataSet Sys_Get_Dset(string strSQL)
        {

            string cString = "Data Source=" + StrServerip + ";Initial Catalog=" + db + ";User ID=" + StrUid + ";Password=" + StrPwd + ";Connection Timeout=600";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = cString;
            conn.Open();


            SqlDataAdapter Sql_Adp = new SqlDataAdapter();
            Sql_Adp.SelectCommand = new SqlCommand(strSQL, conn);
            System.Data.DataSet dst = new System.Data.DataSet();
            Sql_Adp.Fill(dst);
            conn.Close();
            return dst;
        }




        private DataTable Sys_Get_Data(string strSQL)
        {

            string cString = "Data Source=" + StrServerip + ";Initial Catalog=" + db + ";User ID=" + StrUid + ";Password=" + StrPwd + ";Connection Timeout=600";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = cString;
            conn.Open();
            SqlDataAdapter Sql_Adp = new SqlDataAdapter();
            Sql_Adp.SelectCommand = new SqlCommand(strSQL, conn);
            System.Data.DataTable Dtb = new System.Data.DataTable();
            Sql_Adp.Fill(Dtb);
            conn.Close();
            return Dtb;

        }

        public void Sys_Exe_Data(string strSQL)
        {

            string cString = "Data Source=" + StrServerip + ";Initial Catalog=" + db + ";User ID=" + StrUid + ";Password=" + StrPwd + ";Connection Timeout=600";
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = cString;
            conn.Open();
            SqlCommand command = new SqlCommand(strSQL, conn);
            command.ExecuteNonQuery();
            conn.Close();

        } 
        #endregion

        #region Access
        public void Exe_AccessData(string strSQL)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection();
            conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=root;Data Source =" + db_path);//Application.StartupPath +

            conn.Open();
            cmd = new OleDbCommand(strSQL, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public DataSet Get_AccessDset(String strSQL)
        {

            OleDbConnection conn = new OleDbConnection();
            OleDbDataAdapter oda = new OleDbDataAdapter();

            conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Database Password=root;Data Source =" + Application.StartupPath + db_path);
            DataSet myds = new DataSet();
            myds = new DataSet();
            oda = new OleDbDataAdapter(strSQL, conn);
            oda.Fill(myds);
            return myds;
        }

        public DataTable Get_AccessData(string strSQL)
        {
            OleDbConnection conn = new OleDbConnection();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + db_path);
            //conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source =" + db_path);

            DataSet myds = new DataSet();
            myds = new DataSet();
            oda = new OleDbDataAdapter(strSQL, conn);
            oda.Fill(myds);
            return myds.Tables[0];
        }
        
        #endregion

        #region 事务
        public DataSet ExeTransSql(string sql)
        {
            string cString = "Data Source=" + StrServerip + ";Initial Catalog=" + db + ";User ID=" + StrUid + ";Password=" + StrPwd + ";";
            SqlConnection con = new SqlConnection(cString);//获取数据库连接
            con.Open();//打开连接
            SqlTransaction sqltra = con.BeginTransaction();//开始事务
            SqlCommand cmd = new SqlCommand();//实例化
            cmd.Connection = con;//获取数据连接
            cmd.Transaction = sqltra;//，在执行SQL时，
            try
            {
                cmd.CommandText = sql;
                object js = cmd.ExecuteScalar();
                sqltra.Commit();
                return null;
            }
            catch (Exception exp)
            {
                sqltra.Rollback();
                return null;
            }
            finally
            {
                con.Close();
                sqltra.Dispose();
                con.Dispose();
            }
        } 
        #endregion

        #region Excel
        public DataTable GetXlsDtb(string path, string strSQL, int version)
        {
            string strCon = "";
            switch (version)
            {
                case 7:
                    strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES\"";
                    break;
                case 3:
                    strCon = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source =" + path + ";Extended Properties=Excel 8.0";
                    break;
            }
            DataTable Dtb = new DataTable();
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = strSQL;
            myConn.Open();
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.CommandText = strSQL;
            myCommand.Connection = myConn;
            OleDbDataAdapter Adp = new OleDbDataAdapter();
            Adp.SelectCommand = myCommand;
            Adp.Fill(Dtb);
            myConn.Close();
            return Dtb;


        } 
        #endregion
    }
}
