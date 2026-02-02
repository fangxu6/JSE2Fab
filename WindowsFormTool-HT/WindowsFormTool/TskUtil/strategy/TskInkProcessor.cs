using DataToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowsFormTool.TskUtil.InkRules;

namespace WindowsFormTool.TskUtil
{
    public class TskInkProcessor : ITskProcessor
    {
        public void ProcessSingle(string tskPath, string notUsed, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (string.IsNullOrEmpty(tskPath) || !File.Exists(tskPath))
            {
                MessageBox.Show(@"TSK文件路径无效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProcessInkRule(tskPath, updateStatus);
        }

        public void ProcessBatch(List<string> tskFiles, List<string> notUsed, Action<string> updateStatus, ProgressBar progressBar = null)
        {
            if (tskFiles == null || tskFiles.Count == 0)
            {
                MessageBox.Show(@"请先选择TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // INK功能只处理单个文件
            ProcessSingle(tskFiles[0], null, updateStatus, progressBar);
        }

        private void ProcessInkRule(string tskPath, Action<string> updateStatus)
        {
            try
            {
                updateStatus("正在加载TSK文件...\n");
                Tsk tsk = TskFileLoader.LoadTsk(tskPath);

                // 弹出INK规则对话框
                using (var dialog = new WindowsFormTool.Forms.InkRuleDialog())
                {
                    dialog.PreviewRequested += parameters =>
                    {
                        var rule = InkRuleManager.Instance.GetRule(dialog.GetRuleId());
                        if (rule != null)
                        {
                            var previewResult = tsk.DieMatrix.PreviewInkResult(rule, parameters);
                            var count = previewResult.Count;
                            var previewText = $"预览结果：\n将INK {count} 颗Die\n\n坐标列表：\n" +
                                string.Join(", ", previewResult.Select(c => $"({c.Item1},{c.Item2})").Take(50));
                            if (previewResult.Count > 50)
                                previewText += $"\n...还有 {previewResult.Count - 50} 颗";
                            dialog.ShowPreviewResult(previewText);
                        }
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var ruleId = dialog.GetRuleId();
                        var parameters = dialog.GetParameters();

                        updateStatus($"正在应用INK规则：{dialog.Text}\n");
                        updateStatus($"参数：目标Bin={parameters["targetBinNo"]}\n");

                        var rule = InkRuleManager.Instance.GetRule(ruleId);
                        if (rule == null)
                        {
                            MessageBox.Show(@"未找到规则", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // 应用INK规则
                        var result = tsk.DieMatrix.ApplyInkRule(rule, parameters);

                        if (result.Success)
                        {
                            updateStatus($"INK处理完成：{result.GetSummaryText()}\n");
                            updateStatus($"耗时：{result.ElapsedMilliseconds}ms\n");

                            // 询问是否保存
                            if (MessageBox.Show($@"INK处理完成，共INK {result.TotalInkedCount} 颗Die。是否保存修改后的TSK文件？", "完成", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                SaveInkedTsk(tsk, updateStatus);
                            }
                        }
                        else
                        {
                            updateStatus($"INK处理失败：{result.ErrorMessage}\n");
                            MessageBox.Show(result.ErrorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                updateStatus($"错误：{ex.Message}\n");
                MessageBox.Show($@"INK处理出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveInkedTsk(Tsk tsk, Action<string> updateStatus)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "TSK文件|*.tsk|所有文件|*.*";
            saveDialog.FileName = Path.GetFileNameWithoutExtension(tsk.WaferID) + "_ink.tsk";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    tsk.Save(saveDialog.FileName);
                    updateStatus($"已保存INK后的TSK文件：{saveDialog.FileName}\n");
                    MessageBox.Show(@"文件保存成功", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    updateStatus($"保存失败：{ex.Message}\n");
                    MessageBox.Show($@"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}