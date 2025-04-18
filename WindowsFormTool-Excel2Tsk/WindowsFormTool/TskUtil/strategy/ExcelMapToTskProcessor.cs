using DataToExcel;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormTool.TskUtil
{
    public class ExcelMapToTskProcessor : ITskProcessor
    {

        public void ProcessSingle(string excelPath, string tskPath, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (!TskFileHelper.ValidateFiles(excelPath, tskPath))
                return;

            ProcessExcelMapToTsk(excelPath, tskPath, updateStatus, progressBar);
        }


        public void ProcessBatch(List<string> excelFiles, List<string> tskFiles, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (excelFiles == null || tskFiles == null || excelFiles.Count != tskFiles.Count)
            {
                MessageBox.Show(@"文件列表无效或数量不匹配", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < excelFiles.Count; i++)
            {
                updateStatus($"处理第 {i + 1}/{excelFiles.Count} 个文件...\n");
                ProcessSingle(excelFiles[i], tskFiles[i], updateStatus, progressBar);
            }
        }

        private void ProcessExcelMapToTsk(string excelPath, string tskPath, Action<string> updateStatus, ProgressBar progressBar)
        {
            if (!TskFileHelper.ValidateFiles(excelPath, tskPath))
                return;

            updateStatus("开始恢复TSK图谱\n");
            var table = MiniExcel.QueryAsDataTable(excelPath, useHeaderRow: false);
            updateStatus("解析Excel信息成功\n");

            updateStatus("开始解析初始TSK图谱\n");
            var tsk = TskFileLoader.LoadTsk(tskPath);
            updateStatus("解析初始TSK图谱结束\n");

            string newTskPath = TskFileHelper.SavePath;
            string newTskFilePath = Path.Combine(newTskPath, Path.GetFileName(tskPath));
            updateStatus($"生成图谱路径{newTskFilePath}\n");

            ProcessTskData(tsk, table, progressBar);
            SaveTskFile(tsk, newTskFilePath);
        }

        private void ProcessTskData(Tsk tsk, DataTable table, ProgressBar progressBar)
        {
            var processor = new TskDataProcessor(progressBar);
            processor.ProcessFromExcelMap(tsk, table);
        }

        private void SaveTskFile(Tsk tsk, string newTskFilePath)
        {
            tsk.FullName = newTskFilePath;
            tsk.Save();
        }
    }
}
