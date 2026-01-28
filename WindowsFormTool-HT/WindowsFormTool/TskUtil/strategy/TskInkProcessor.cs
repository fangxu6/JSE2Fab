using DataToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormTool.TskUtil
{
    public class TskInkProcessor : ITskProcessor
    {

        public void ProcessSingle(string excelPath, string tskPath, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (!TskFileHelper.ValidateFiles(excelPath, tskPath))
                return;

            ProcessTskMerge(excelPath, tskPath, updateStatus);
        }

        public void ProcessBatch(List<string> firstFiles, List<string> secondFiles, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (firstFiles == null || secondFiles == null || firstFiles.Count != secondFiles.Count)
            {
                MessageBox.Show(@"文件列表无效或数量不匹配", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < firstFiles.Count; i++)
            {
                updateStatus($"处理第 {i + 1}/{firstFiles.Count} 个文件...\n");
                ProcessSingle(firstFiles[i], secondFiles[i], updateStatus, progressBar);
            }
        }

        private void ProcessTskMerge(string originalPath, string mergePath, Action<string> updateStatus)
        {
            updateStatus("开始合并TSK图谱\n");
            Tsk originalTsk = TskFileLoader.LoadTsk(originalPath);
            Tsk mergeTsk = TskFileLoader.LoadTsk(mergePath);

            if (!ValidateTskMatch(originalTsk, mergeTsk))
                return;

            string newTskPath = TskFileHelper.SavePath;
            string newTskFilePath = Path.Combine(newTskPath, Path.GetFileName(mergePath));
            updateStatus($"生成图谱路径{newTskFilePath}\n");

            mergeTsk.Merge(originalTsk, newTskFilePath);
        }

        private bool ValidateTskMatch(Tsk originalTsk, Tsk mergeTsk)
        {
            if (originalTsk.Rows != mergeTsk.Rows || originalTsk.Cols != mergeTsk.Cols)
            {
                MessageBox.Show(@"TSK图谱行列数不一致，无法合并", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string originalLotNo = Regex.Replace(originalTsk.LotNo, "CP[1-3]", "").Trim();
            string mergeLotNo = Regex.Replace(mergeTsk.LotNo, "CP[1-3]", "").Trim();

            if (originalLotNo != mergeLotNo || originalTsk.SlotNo != mergeTsk.SlotNo)
            {
                MessageBox.Show(@"TSK图谱WaferID不一致，无法合并", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
