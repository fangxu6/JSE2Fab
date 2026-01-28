using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormTool.TskUtil.InkRules;

namespace WindowsFormTool.Forms
{
    public partial class InkRuleDialog : Form
    {
        private ComboBox ruleComboBox;
        private NumericUpDown targetBinNumeric;
        private NumericUpDown ringsNumeric;
        private Label ringsLabel;
        private Label targetBinLabel;
        private GroupBox modeGroupBox;
        private RadioButton mode1RadioButton;
        private RadioButton mode2RadioButton;
        private Button previewButton;
        private Button applyButton;
        private Button cancelButton;
        private RichTextBox previewRichTextBox;

        private IInkRule _selectedRule;
        private Dictionary<string, object> _currentParameters;

        public Dictionary<string, object> SelectedParameters { get; private set; }
        public string SelectedRuleId { get; private set; }
        public int SelectedMode { get; private set; }

        public InkRuleDialog()
        {
            InitializeComponent();
            LoadRules();
        }

        private void InitializeComponent()
        {
            this.Text = "INK规则设置";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 规则选择
            var ruleLabel = new Label();
            ruleLabel.Text = "选择规则：";
            ruleLabel.Location = new Point(20, 20);
            ruleLabel.Size = new Size(80, 20);
            this.Controls.Add(ruleLabel);

            ruleComboBox = new ComboBox();
            ruleComboBox.Location = new Point(110, 18);
            ruleComboBox.Size = new Size(200, 23);
            ruleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ruleComboBox.SelectedIndexChanged += RuleComboBox_SelectedIndexChanged;
            this.Controls.Add(ruleComboBox);

            // 目标Bin号
            targetBinLabel = new Label();
            targetBinLabel.Text = "目标Bin号：";
            targetBinLabel.Location = new Point(20, 55);
            targetBinLabel.Size = new Size(80, 20);
            this.Controls.Add(targetBinLabel);

            targetBinNumeric = new NumericUpDown();
            targetBinNumeric.Location = new Point(110, 53);
            targetBinNumeric.Size = new Size(80, 23);
            targetBinNumeric.Minimum = 1;
            targetBinNumeric.Maximum = 255;
            targetBinNumeric.Value = 63;
            this.Controls.Add(targetBinNumeric);

            // 十字围点模式选择
            modeGroupBox = new GroupBox();
            modeGroupBox.Text = "十字围点模式";
            modeGroupBox.Location = new Point(20, 90);
            modeGroupBox.Size = new Size(380, 70);
            modeGroupBox.Visible = false;
            this.Controls.Add(modeGroupBox);

            mode1RadioButton = new RadioButton();
            mode1RadioButton.Text = "模式1：纯Fail包围（上下左右四颗均为Fail Die）";
            mode1RadioButton.Location = new Point(15, 20);
            mode1RadioButton.Size = new Size(350, 20);
            mode1RadioButton.Checked = true;
            modeGroupBox.Controls.Add(mode1RadioButton);

            mode2RadioButton = new RadioButton();
            mode2RadioButton.Text = "模式2：含Mark Fail包围（1-3颗Mark Die + 其余Fail Die）";
            mode2RadioButton.Location = new Point(15, 42);
            mode2RadioButton.Size = new Size(350, 20);
            modeGroupBox.Controls.Add(mode2RadioButton);

            // 圈数（九宫格用）
            ringsLabel = new Label();
            ringsLabel.Text = "INK圈数：";
            ringsLabel.Location = new Point(210, 55);
            ringsLabel.Size = new Size(80, 20);
            ringsLabel.Visible = false;
            this.Controls.Add(ringsLabel);

            ringsNumeric = new NumericUpDown();
            ringsNumeric.Location = new Point(300, 53);
            ringsNumeric.Size = new Size(60, 23);
            ringsNumeric.Minimum = 1;
            ringsNumeric.Maximum = 3;
            ringsNumeric.Value = 1;
            ringsNumeric.Visible = false;
            this.Controls.Add(ringsNumeric);

            // 预览区域
            previewRichTextBox = new RichTextBox();
            previewRichTextBox.Location = new Point(20, 175);
            previewRichTextBox.Size = new Size(390, 120);
            previewRichTextBox.ReadOnly = true;
            this.Controls.Add(previewRichTextBox);

            // 预览按钮
            previewButton = new Button();
            previewButton.Text = "预览";
            previewButton.Location = new Point(20, 310);
            previewButton.Size = new Size(80, 30);
            previewButton.Click += PreviewButton_Click;
            this.Controls.Add(previewButton);

            // 应用按钮
            applyButton = new Button();
            applyButton.Text = "应用";
            applyButton.Location = new Point(120, 310);
            applyButton.Size = new Size(80, 30);
            applyButton.DialogResult = DialogResult.OK;
            this.Controls.Add(applyButton);

            // 取消按钮
            cancelButton = new Button();
            cancelButton.Text = "取消";
            cancelButton.Location = new Point(220, 310);
            cancelButton.Size = new Size(80, 30);
            cancelButton.DialogResult = DialogResult.Cancel;
            this.Controls.Add(cancelButton);

            this.AcceptButton = applyButton;
            this.CancelButton = cancelButton;
        }

        private void LoadRules()
        {
            var manager = InkRuleManager.Instance;
            var rules = manager.GetAllRules();

            foreach (var rule in rules)
            {
                ruleComboBox.Items.Add(new RuleItem
                {
                    RuleId = rule.RuleId,
                    RuleName = rule.RuleName,
                    Rule = rule
                });
            }

            if (ruleComboBox.Items.Count > 0)
                ruleComboBox.SelectedIndex = 0;
        }

        private void RuleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ruleComboBox.SelectedItem is RuleItem selectedItem)
            {
                _selectedRule = selectedItem.Rule;
                _currentParameters = _selectedRule.GetDefaultParameters();

                // 根据规则类型显示不同UI
                if (_selectedRule.RuleId == CrossPatternInkRule.RULE_ID)
                {
                    modeGroupBox.Visible = true;
                    ringsLabel.Visible = false;
                    ringsNumeric.Visible = false;

                    // 设置默认模式
                    mode1RadioButton.Checked = true;
                }
                else if (_selectedRule.RuleId == NineGridInkRule.RULE_ID)
                {
                    modeGroupBox.Visible = false;
                    ringsLabel.Visible = true;
                    ringsNumeric.Visible = true;

                    if (_currentParameters.ContainsKey("rings"))
                        ringsNumeric.Value = (int)_currentParameters["rings"];
                }

                if (_currentParameters.ContainsKey("targetBinNo"))
                    targetBinNumeric.Value = (int)_currentParameters["targetBinNo"];
            }
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            if (_selectedRule == null)
                return;

            try
            {
                var parameters = GetCurrentParameters();
                previewRichTextBox.Text = $"预览参数：\n{FormatParameters(parameters)}\n\n正在预览...";

                // 这里需要传入当前的DieMatrix
                // 预览逻辑在Form1中处理
                OnPreviewRequested(parameters);
            }
            catch (Exception ex)
            {
                previewRichTextBox.Text = $"预览失败：{ex.Message}";
            }
        }

        private Dictionary<string, object> GetCurrentParameters()
        {
            var parameters = new Dictionary<string, object>();

            parameters["targetBinNo"] = (int)targetBinNumeric.Value;

            if (_selectedRule.RuleId == CrossPatternInkRule.RULE_ID)
            {
                parameters["mode"] = mode1RadioButton.Checked ? 1 : 2;
            }
            else if (_selectedRule.RuleId == NineGridInkRule.RULE_ID)
            {
                parameters["rings"] = (int)ringsNumeric.Value;
            }

            return parameters;
        }

        private string FormatParameters(Dictionary<string, object> parameters)
        {
            var lines = new List<string>();
            foreach (var kvp in parameters)
            {
                lines.Add($"  {kvp.Key}: {kvp.Value}");
            }
            return string.Join("\n", lines);
        }

        public void ShowPreviewResult(string result)
        {
            previewRichTextBox.Text = result;
        }

        public Dictionary<string, object> GetParameters()
        {
            return GetCurrentParameters();
        }

        public string GetRuleId()
        {
            if (ruleComboBox.SelectedItem is RuleItem selectedItem)
                return selectedItem.RuleId;
            return null;
        }

        public int GetMode()
        {
            if (mode1RadioButton.Checked)
                return 1;
            return 2;
        }

        public event Action<Dictionary<string, object>> PreviewRequested;

        protected virtual void OnPreviewRequested(Dictionary<string, object> parameters)
        {
            PreviewRequested?.Invoke(parameters);
        }

        private class RuleItem
        {
            public string RuleId { get; set; }
            public string RuleName { get; set; }
            public IInkRule Rule { get; set; }

            public override string ToString()
            {
                return RuleName;
            }
        }
    }
}
