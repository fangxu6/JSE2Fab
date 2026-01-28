using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DataToExcel
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 命令行参数：--test 运行单元测试
            if (args.Length > 0 && args[0] == "--test")
            {
                RunTests();
                return;
            }

            // 防止重复运行
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            if (processes.Length > 1)
            {
                MessageBox.Show("程序已在运行中！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// 运行单元测试
        /// </summary>
        private static void RunTests()
        {
            Console.WriteLine("=== TSK INK 功能单元测试 ===\n");

            var tests = new Tests.InkRuleTests();
            tests.RunAllTests();

            Console.WriteLine($"\n总计: {tests.PassedTests} 通过, {tests.FailedTests} 失败");
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
