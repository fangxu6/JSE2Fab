////using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniExcelLibs;
using WindowsFormTool.TskUtil;
using WindowsFormTool.TskUtil.InkRules;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DataToExcel
{

    public partial class Form1 : Form
    {
        public string ExcelFilePath;
        public string TSKFilePath;

        private List<string> firstFileList;
        private List<string> secondFileList;
        private string _inkTskPath; // INK功能使用的TSK文件路径

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0; // 默认选择第一个选项
            // 按钮事件会在comboBox1_SelectedIndexChanged中动态绑定
        }

        /// <summary>
        /// 选择第一组TSK文件（用于合并或INK）
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            firstFileList = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo TheFolder = new DirectoryInfo(dialog.SelectedPath);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    firstFileList.Add(str.FullName);
                }
                button6.Text = dialog.SelectedPath;
                UpdateRichTextBox($"已加载 {firstFileList.Count} 个TSK文件\n");
            }
        }

        /// <summary>
        /// 选择第二组TSK文件（用于合并）
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            secondFileList = new List<string>();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo TheFolder = new DirectoryInfo(dialog.SelectedPath);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    secondFileList.Add(str.FullName);
                }
                button2.Text = dialog.SelectedPath;
                UpdateRichTextBox($"已加载目标TSK文件夹：{dialog.SelectedPath}\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var processor = new TskProcessor();
            TskFileHelper.SavePath = SaveFileTo.Text.Trim();
            string newTskPath = TskFileHelper.SavePath;

            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0: // TSK合并
                        if (firstFileList != null && firstFileList.Count > 0 && secondFileList != null && secondFileList.Count > 0)
                        {
                            // 批量TSK合并
                            processor.ProcessBatch(firstFileList, secondFileList, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else
                        {
                            MessageBox.Show(@"请先选择两组TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;

                    case 1: // INK规则 - 使用第一组TSK
                        if (!string.IsNullOrEmpty(_inkTskPath))
                        {
                            ProcessInkRule(_inkTskPath, UpdateRichTextBox);
                        }
                        else
                        {
                            MessageBox.Show(@"请先选择TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;

                    default:
                        MessageBox.Show(@"未选择处理方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                // 处理完成后询问是否打开文件夹
                if (MessageBox.Show(@"TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(newTskPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"处理过程中出错：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateRichTextBox($"错误：{ex.Message}\n");
            }
        }

        //更新RichTextBox
        private void UpdateRichTextBox(string message)
        {
            richTextBox1.AppendText(message);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();;
            Application.DoEvents();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 根据 ComboBox 的选择更改 Button 的文本和事件
            switch (comboBox1.SelectedIndex)
            {
                case 0: // TSK合并
                    button6.Text = "选择TSK 1（模板）";
                    button2.Text = "选择TSK 2（目标）";
                    button4.Text = "说明：\r\n将TSK 1中的Fail合并到TSK 2\r\n";
                    button3.Text = "选择TSK 1";
                    button5.Text = "选择TSK 2";
                    // 重新关联事件
                    button6.Click -= button6_Click_INK;
                    button6.Click -= button6_Click;
                    button6.Click += button6_Click;
                    button3.Click -= button6_Click_INK;
                    button3.Click -= button6_Click;
                    button3.Click += button6_Click;
                    button5.Click -= button5_Click;
                    button5.Click += button5_Click;
                    button1.Text = "开始合并";
                    break;
                case 1: // INK规则
                    button6.Text = "选择TSK文件";
                    button2.Text = "已选：-";
                    button4.Text = "说明：\r\n选择TSK文件后，点击开始进行INK处理\r\n";
                    button3.Text = "选择TSK";
                    button5.Text = "-";
                    // 关联到INK选择事件
                    button6.Click -= button6_Click;
                    button6.Click -= button6_Click_INK;
                    button6.Click += button6_Click_INK;
                    button3.Click -= button6_Click;
                    button3.Click -= button6_Click_INK;
                    button3.Click += button6_Click_INK;
                    button5.Text = "-";
                    button1.Text = "开始INK";
                    break;
                default:
                    button6.Text = "请选择功能";
                    button2.Text = "请选择功能";
                    button4.Text = "说明：\r\n请从下拉菜单选择功能\r\n";
                    break;
            }
        }

        /// <summary>
        /// 加载TSK文件（用于INK功能）
        /// </summary>
        private void button6_Click_INK(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "TSK文件|*.tsk|所有文件|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _inkTskPath = dialog.FileName;
                button6.Text = Path.GetFileName(_inkTskPath);
                button2.Text = "已选：1个";
                UpdateRichTextBox($"已加载TSK文件：{_inkTskPath}\n");
            }
        }

        /// <summary>
        /// 处理INK规则
        /// </summary>
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
                            if (MessageBox.Show($@"INK处理完成，共INK {result.TotalInkedCount} 颗Die。

是否保存修改后的TSK文件？", "完成", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

        /// <summary>
        /// 保存INK后的TSK文件
        /// </summary>
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
