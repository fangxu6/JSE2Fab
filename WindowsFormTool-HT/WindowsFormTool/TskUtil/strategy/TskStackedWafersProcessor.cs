using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataToExcel;
using WindowsFormTool.Forms;
using WindowsFormTool.TskUtil.StackedWafers;

namespace WindowsFormTool.TskUtil
{
    public class TskStackedWafersProcessor : ITskProcessor
    {
        public void ProcessSingle(string tskPath, string notUsed, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (string.IsNullOrWhiteSpace(tskPath))
            {
                MessageBox.Show(@"请先选择TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ProcessBatch(new List<string> { tskPath }, null, updateStatus, progressBar);
        }

        public void ProcessBatch(List<string> tskFiles, List<string> notUsed, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (!ValidateTskFiles(tskFiles))
            {
                return;
            }

            List<Tsk> tskList = new List<Tsk>();
            try
            {
                foreach (var tskFile in tskFiles)
                {
                    tskList.Add(TskFileLoader.LoadTsk(tskFile));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"加载TSK文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var matrices = tskList.Select(tsk => tsk.DieMatrix).ToList();
            if (!StackedWafersCalculator.TryValidateSameShape(matrices, out var error))
            {
                MessageBox.Show($@"TSK图谱不一致：{error}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dialog = new StackedWafersDialog())
            {
                dialog.SetLotSize(tskFiles.Count);
                dialog.PreviewRequested += () =>
                {
                    try
                    {
                        var threshold = dialog.GetThresholdFraction();
                        var required = StackedWafersCalculator.RequiredFailCount(threshold, matrices.Count);
                        var coordinates = StackedWafersCalculator.ComputeStackedBadCoordinates(matrices, threshold);

                        // coordinates 是 (indexX,indexY)，这里转成实际坐标 (die.X,die.Y) 用于预览
                        const int maxPreviewCoordinates = 200;
                        var reference = matrices[0];
                        var takeCount = Math.Min(coordinates.Count, maxPreviewCoordinates);
                        var coordLines = new List<string>(takeCount);

                        for (int i = 0; i < takeCount; i++)
                        {
                            var idx = coordinates[i];
                            // var die = reference[idx.Item1, idx.Item2];
                            coordLines.Add($"({idx.Item1},{idx.Item2})");
                        }

                        var previewText =
                            $"预览结果：\n" +
                            $"Lot片数：{matrices.Count}\n" +
                            $"Fail阈值：{required}/{matrices.Count}\n" +
                            $"堆叠坏点数量：{coordinates.Count}\n" +
                            $"坐标列表(显示前{takeCount}个)：\n" +
                            string.Join("\n", coordLines) +
                            (coordinates.Count > maxPreviewCoordinates
                                ? $"\n...其余 {coordinates.Count - maxPreviewCoordinates} 个未显示"
                                : string.Empty);

                        dialog.ShowPreviewResult(previewText);
                    }
                    catch (Exception ex)
                    {
                        dialog.ShowPreviewResult($"预览失败：{ex.Message}");
                    }
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var threshold = dialog.GetThresholdFraction();
                    var targetBinNo = dialog.GetTargetBinNo();
                    var required = StackedWafersCalculator.RequiredFailCount(threshold, matrices.Count);
                    var coordinates = StackedWafersCalculator.ComputeStackedBadCoordinates(matrices, threshold);

                    updateStatus?.Invoke($"Stacked Wafers参数：阈值={threshold:P0} 目标Bin={targetBinNo}\n");
                    updateStatus?.Invoke($"Fail阈值：{required}/{matrices.Count}\n");
                    updateStatus?.Invoke($"堆叠坏点数量：{coordinates.Count}\n");

                    int totalApplied = 0;
                    for (int i = 0; i < tskList.Count; i++)
                    {
                        int applied = StackedWafersCalculator.ApplyStackedBadCoordinates(tskList[i].DieMatrix, coordinates, targetBinNo);
                        totalApplied += applied;
                        updateStatus?.Invoke($"第 {i + 1}/{tskList.Count} 片应用堆叠坏点：{applied} 颗\n");
                    }

                    updateStatus?.Invoke($"总共应用堆叠坏点：{totalApplied} 颗\n");

                    if (MessageBox.Show($@"堆叠坏点已应用，共 {totalApplied} 颗。是否保存修改后的TSK文件？", "完成", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SaveStackedTskFiles(tskList, tskFiles, updateStatus);
                    }
                }
            }
        }

        private void SaveStackedTskFiles(List<Tsk> tskList, List<string> tskFiles, Action<string> updateStatus)
        {
            string savePath = TskFileHelper.GetSaveFilePath();

            for (int i = 0; i < tskList.Count; i++)
            {
                var tsk = tskList[i];
                var originalPath = tskFiles[i];
                string fileName = Path.GetFileNameWithoutExtension(originalPath) + "_stacked.tsk";
                string outputPath = Path.Combine(savePath, fileName);

                try
                {
                    tsk.Save(outputPath);
                    updateStatus?.Invoke($"已保存：{outputPath}\n");
                }
                catch (Exception ex)
                {
                    updateStatus?.Invoke($"保存失败：{outputPath}，原因：{ex.Message}\n");
                    MessageBox.Show($@"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show(@"堆叠坏点TSK文件已保存", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ValidateTskFiles(List<string> tskFiles)
        {
            if (tskFiles == null || tskFiles.Count == 0)
            {
                MessageBox.Show(@"请先选择TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (var tskFile in tskFiles)
            {
                if (string.IsNullOrWhiteSpace(tskFile))
                {
                    MessageBox.Show(@"TSK文件路径无效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!File.Exists(tskFile))
                {
                    MessageBox.Show($@"TSK文件不存在: {tskFile}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
