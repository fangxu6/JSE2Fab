using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using system_basic;

using System.Windows.Forms;

namespace Xj_Mes_cp
{
    internal class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new 业务排产打印管理());
        }
    }
}
