using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using WindowsFormTool.TskUtil;

namespace DataToExcel
{
    public partial class Form1 : Form
    {
        public string ExcelFilePath;
        public string TSKFilePath;

        private List<string> firstFileList;
        private List<string> secondFileList;
        private string _inkTskPath; // INK功能使用的TSK文件路径

        // 当前“选择TSK”按钮实际执行的动作（避免反复 +=/-= Click）
        private Action _selectPrimaryAction;

        private enum Mode
        {
            Merge = 0,
            Ink = 1,
            StackedWafers = 2
        }

        private sealed class ModeUi
        {
            public string PrimaryLabelText;
            public string SecondaryLabelText;
            public string DescriptionText;
            public string PrimaryBrowseText;
            public string SecondaryBrowseText;
            public bool SecondaryBrowseEnabled;
            public string StartButtonText;
            public Action<Form1> SelectPrimaryAction;
        }

        private readonly Dictionary<int, ModeUi> _modeUiMap;

        public Form1()
        {
            InitializeComponent();

            // 统一绑定一次 Click，后续通过切换 _selectPrimaryAction 来改变行为
            button6.Click += SelectPrimary_Click;
            button3.Click += SelectPrimary_Click;
            button5.Click += SelectSecondary_Click;

            _modeUiMap = new Dictionary<int, ModeUi>
            {
                [(int)Mode.Merge] = new ModeUi
                {
                    PrimaryLabelText = "选择TSK 1（模板）",
                    SecondaryLabelText = "选择TSK 2（目标）",
                    DescriptionText = "说明：\r\n将TSK 1中的Fail合并到TSK 2\r\n",
                    PrimaryBrowseText = "选择TSK 1",
                    SecondaryBrowseText = "选择TSK 2",
                    SecondaryBrowseEnabled = true,
                    StartButtonText = "开始合并",
                    SelectPrimaryAction = f => f.SelectFolderAsFirstGroup()
                },
                [(int)Mode.Ink] = new ModeUi
                {
                    PrimaryLabelText = "选择TSK文件",
                    SecondaryLabelText = "已选：-",
                    DescriptionText = "说明：\r\n选择TSK文件后，点击开始进行INK处理\r\n",
                    PrimaryBrowseText = "选择TSK",
                    SecondaryBrowseText = "-",
                    SecondaryBrowseEnabled = false,
                    StartButtonText = "开始INK",
                    SelectPrimaryAction = f => f.SelectSingleTskForInk()
                },
                [(int)Mode.StackedWafers] = new ModeUi
                {
                    PrimaryLabelText = "选择TSK文件夹（Lot）",
                    SecondaryLabelText = "已选：-",
                    DescriptionText = "说明：\r\n选择同一Lot的TSK文件夹后，点击开始进行叠片分析\r\n",
                    PrimaryBrowseText = "选择TSK文件夹",
                    SecondaryBrowseText = "-",
                    SecondaryBrowseEnabled = false,
                    StartButtonText = "开始叠片分析",
                    SelectPrimaryAction = f => f.SelectFolderAsStackedLot()
                }
            };

            comboBox1.SelectedIndex = 0; // 默认选择第一个选项
        }

        /// <summary>
        /// 选择第一组TSK文件（用于合并或INK）
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            firstFileList = new List<string>();
            using (var dialog = new FolderBrowserDialog())
            {
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
        }

        /// <summary>
        /// 选择第二组TSK文件（用于合并）
        /// </summary>
        private void button5_Click(object sender, EventArgs e)
        {
            secondFileList = new List<string>();
            using (var dialog = new FolderBrowserDialog())
            {
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
                            processor.ProcessBatch(firstFileList, secondFileList, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else
                        {
                            MessageBox.Show(@"请先选择两组TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;

                    case 1: // INK规则 - 现在也使用统一的processor接口
                        if (!string.IsNullOrEmpty(_inkTskPath))
                        {
                            var inkFileList = new List<string> { _inkTskPath };
                            processor.ProcessBatch(inkFileList, null, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else
                        {
                            MessageBox.Show(@"请先选择TSK文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;

                    case 2: // Stacked Wafers
                        if (firstFileList != null && firstFileList.Count > 0)
                        {
                            processor.ProcessBatch(firstFileList, null, comboBox1.SelectedIndex,
                                UpdateRichTextBox, progressBar1);
                        }
                        else
                        {
                            MessageBox.Show(@"请先选择TSK文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;

                    default:
                        MessageBox.Show(@"未选择处理方式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                }

                // // 处理完成后询问是否打开文件夹
                // if (MessageBox.Show(@"TSK新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                // {
                //     Process.Start(newTskPath);
                // }
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
            richTextBox1.ScrollToCaret();
            Application.DoEvents();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyMode(comboBox1.SelectedIndex);
        }

        private void ApplyMode(int selectedIndex)
        {
            ModeUi ui;
            if (!_modeUiMap.TryGetValue(selectedIndex, out ui))
            {
                // fallback
                button6.Text = "请选择功能";
                button2.Text = "请选择功能";
                button4.Text = "说明：\r\n请从下拉菜单选择功能\r\n";
                button3.Text = "浏览";
                button5.Text = "浏览";
                button5.Enabled = false;
                button1.Text = "开始";
                _selectPrimaryAction = null;
                return;
            }

            button6.Text = ui.PrimaryLabelText;
            button2.Text = ui.SecondaryLabelText;
            button4.Text = ui.DescriptionText;

            button3.Text = ui.PrimaryBrowseText;
            button5.Text = ui.SecondaryBrowseText;
            button5.Enabled = ui.SecondaryBrowseEnabled;

            button1.Text = ui.StartButtonText;

            _selectPrimaryAction = ui.SelectPrimaryAction == null ? (Action)null : (() => ui.SelectPrimaryAction(this));
        }

        private void SelectPrimary_Click(object sender, EventArgs e)
        {
            var action = _selectPrimaryAction;
            if (action != null)
            {
                action();
            }
        }

        private void SelectSecondary_Click(object sender, EventArgs e)
        {
            // 只有合并模式需要 second group；Ink 模式下 button5.Enabled=false 不会触发
            button5_Click(sender, e);
        }

        private void SelectFolderAsFirstGroup()
        {
            _inkTskPath = null; // 切换到文件夹选择时清掉单文件选择
            button6_Click(this, EventArgs.Empty);
        }

        private void SelectFolderAsStackedLot()
        {
            _inkTskPath = null;
            firstFileList = new List<string>();
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                DirectoryInfo TheFolder = new DirectoryInfo(dialog.SelectedPath);
                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    firstFileList.Add(str.FullName);
                }

                button6.Text = dialog.SelectedPath;
                button2.Text = $"已选：{firstFileList.Count}个";
                UpdateRichTextBox($"已加载 {firstFileList.Count} 个TSK文件\n");
            }
        }

        /// <summary>
        /// 加载TSK文件（用于INK功能）——重命名为更语义化的方法，避免在模式切换里频繁绑/解绑 Click
        /// </summary>
        private void SelectSingleTskForInk()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "所有文件|*.*";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _inkTskPath = dialog.FileName;
                firstFileList = null;

                // 只在这里更新与“已选文件”相关的 UI，避免模式切换里堆积 Text 赋值
                button6.Text = Path.GetFileName(_inkTskPath);
                button2.Text = "已选：1个";

                UpdateRichTextBox($"已加载TSK文件：{_inkTskPath}\n");
            }
        }

        /// <summary>
        /// 保留原事件方法签名（如果别处有引用），内部转调到新实现。
        /// </summary>
        private void button6_Click_INK(object sender, EventArgs e)
        {
            SelectSingleTskForInk();
        }
    }
}
