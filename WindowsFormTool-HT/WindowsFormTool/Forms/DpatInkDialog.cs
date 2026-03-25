using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormTool.Forms
{
    public class DpatInkDialog : Form
    {
        private ComboBox testNameComboBox;
        private NumericUpDown sigmaNumeric;
        private NumericUpDown inkBinNumeric;
        private RadioButton formula1Radio;
        private RadioButton formula2Radio;
        private CheckBox allowMissingTestCheckBox;
        private DataGridView testConfigGrid;
        private Button addOrUpdateButton;
        private Button removeButton;
        private Button okButton;
        private Button cancelButton;
        private readonly BindingList<DpatInkTestConfig> selectedTests = new BindingList<DpatInkTestConfig>();

        public DpatInkDialog(IEnumerable<string> testNames)
        {
            InitializeComponent();
            LoadTestNames(testNames);
        }

        public bool AllowMissingTestName => allowMissingTestCheckBox.Checked;
        public IReadOnlyList<DpatInkTestConfig> SelectedTests => selectedTests.ToList();

        private void InitializeComponent()
        {
            Text = "DPAT INK 参数设置";
            Size = new Size(780, 380);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var testNameLabel = new Label
            {
                Text = "测试项(testName)：",
                Location = new Point(20, 20),
                Size = new Size(120, 20)
            };
            Controls.Add(testNameLabel);

            testNameComboBox = new ComboBox
            {
                Location = new Point(150, 18),
                Size = new Size(170, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(testNameComboBox);

            addOrUpdateButton = new Button
            {
                Text = "添加/更新",
                Location = new Point(150, 48),
                Size = new Size(80, 26)
            };
            addOrUpdateButton.Click += AddOrUpdateButton_Click;
            Controls.Add(addOrUpdateButton);

            removeButton = new Button
            {
                Text = "移除",
                Location = new Point(240, 48),
                Size = new Size(60, 26)
            };
            removeButton.Click += RemoveButton_Click;
            Controls.Add(removeButton);

            var sigmaLabel = new Label
            {
                Text = "Sigma：",
                Location = new Point(20, 85),
                Size = new Size(120, 20)
            };
            Controls.Add(sigmaLabel);

            sigmaNumeric = new NumericUpDown
            {
                Location = new Point(150, 83),
                Size = new Size(120, 23),
                DecimalPlaces = 3,
                Minimum = 0,
                Maximum = 100,
                Value = 3
            };
            Controls.Add(sigmaNumeric);

            var inkBinLabel = new Label
            {
                Text = "Ink Bin：",
                Location = new Point(20, 115),
                Size = new Size(120, 20)
            };
            Controls.Add(inkBinLabel);

            inkBinNumeric = new NumericUpDown
            {
                Location = new Point(150, 113),
                Size = new Size(120, 23),
                Minimum = 1,
                Maximum = 255,
                Value = 63
            };
            Controls.Add(inkBinNumeric);

            var formulaGroup = new GroupBox
            {
                Text = "公式选择",
                Location = new Point(20, 145),
                Size = new Size(330, 60)
            };
            Controls.Add(formulaGroup);

            formula1Radio = new RadioButton
            {
                Text = "公式1：均值±标准差",
                Location = new Point(12, 22),
                Size = new Size(170, 20),
                Checked = true
            };
            formulaGroup.Controls.Add(formula1Radio);

            formula2Radio = new RadioButton
            {
                Text = "公式2：中位数/IQR",
                Location = new Point(178, 22),
                Size = new Size(145, 20)
            };
            formulaGroup.Controls.Add(formula2Radio);

            allowMissingTestCheckBox = new CheckBox
            {
                Text = "允许CSV缺失测试项时跳过执行",
                Location = new Point(20, 210),
                Size = new Size(260, 20)
            };
            Controls.Add(allowMissingTestCheckBox);

            var selectedLabel = new Label
            {
                Text = "已选测试项：",
                Location = new Point(350, 20),
                Size = new Size(120, 20)
            };
            Controls.Add(selectedLabel);

            testConfigGrid = new DataGridView
            {
                Location = new Point(350, 45),
                Size = new Size(400, 220),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            testConfigGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "测试项",
                DataPropertyName = nameof(DpatInkTestConfig.TestName)
            });
            testConfigGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Sigma",
                DataPropertyName = nameof(DpatInkTestConfig.Sigma),
                DefaultCellStyle = { Format = "F3" }
            });
            testConfigGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Ink Bin",
                DataPropertyName = nameof(DpatInkTestConfig.InkBin)
            });
            testConfigGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "公式",
                DataPropertyName = nameof(DpatInkTestConfig.FormulaName)
            });
            testConfigGrid.DataSource = selectedTests;
            testConfigGrid.SelectionChanged += TestConfigGrid_SelectionChanged;
            Controls.Add(testConfigGrid);

            okButton = new Button
            {
                Text = "确定",
                Location = new Point(500, 285),
                Size = new Size(80, 30)
            };
            okButton.Click += OkButton_Click;
            Controls.Add(okButton);

            cancelButton = new Button
            {
                Text = "取消",
                Location = new Point(600, 285),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(cancelButton);

            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        private void LoadTestNames(IEnumerable<string> testNames)
        {
            if (testNames == null)
                return;

            var items = testNames.Where(name => !string.IsNullOrWhiteSpace(name)).Distinct().ToList();
            foreach (var name in items)
            {
                testNameComboBox.Items.Add(name);
            }

            if (testNameComboBox.Items.Count > 0)
                testNameComboBox.SelectedIndex = 0;
        }

        private void AddOrUpdateButton_Click(object sender, EventArgs e)
        {
            if (!TryBuildConfig(out var config))
                return;

            var index = FindConfigIndex(config.TestName);
            if (index >= 0)
            {
                selectedTests[index] = config;
            }
            else
            {
                selectedTests.Add(config);
            }

            SelectGridRow(config.TestName);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (testConfigGrid.CurrentRow?.DataBoundItem is DpatInkTestConfig config)
            {
                selectedTests.Remove(config);
            }
        }

        private void TestConfigGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (testConfigGrid.CurrentRow?.DataBoundItem is DpatInkTestConfig config)
            {
                ApplyConfigToInputs(config);
            }
        }

        private int FindConfigIndex(string testName)
        {
            for (int i = 0; i < selectedTests.Count; i++)
            {
                if (string.Equals(selectedTests[i].TestName, testName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }

        private void SelectGridRow(string testName)
        {
            foreach (DataGridViewRow row in testConfigGrid.Rows)
            {
                if (row.DataBoundItem is DpatInkTestConfig config &&
                    string.Equals(config.TestName, testName, StringComparison.OrdinalIgnoreCase))
                {
                    row.Selected = true;
                    testConfigGrid.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void ApplyConfigToInputs(DpatInkTestConfig config)
        {
            SelectTestName(config.TestName);
            sigmaNumeric.Value = ClampToRange(config.Sigma, sigmaNumeric);
            inkBinNumeric.Value = ClampToRange(config.InkBin, inkBinNumeric);
            formula2Radio.Checked = config.UseFormula2;
            formula1Radio.Checked = !config.UseFormula2;
        }

        private void SelectTestName(string testName)
        {
            for (int i = 0; i < testNameComboBox.Items.Count; i++)
            {
                if (string.Equals(testNameComboBox.Items[i]?.ToString(), testName, StringComparison.OrdinalIgnoreCase))
                {
                    testNameComboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private decimal ClampToRange(double value, NumericUpDown numeric)
        {
            var decimalValue = (decimal)value;
            if (decimalValue < numeric.Minimum)
                return numeric.Minimum;

            if (decimalValue > numeric.Maximum)
                return numeric.Maximum;

            return decimalValue;
        }

        private bool TryBuildConfig(out DpatInkTestConfig config)
        {
            config = null;
            var testName = testNameComboBox.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(testName))
            {
                MessageBox.Show(@"请选择 testName", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var sigma = (double)sigmaNumeric.Value;
            if (sigma <= 0)
            {
                MessageBox.Show(@"Sigma 必须大于 0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            config = new DpatInkTestConfig
            {
                TestName = testName,
                Sigma = sigma,
                InkBin = (int)inkBinNumeric.Value,
                UseFormula2 = formula2Radio.Checked
            };

            return true;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (selectedTests.Count == 0)
            {
                MessageBox.Show(@"请先添加至少一个测试项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public sealed class DpatInkTestConfig
    {
        public string TestName { get; set; }
        public double Sigma { get; set; }
        public int InkBin { get; set; }
        public bool UseFormula2 { get; set; }
        public string FormulaName => UseFormula2 ? "公式2" : "公式1";
    }
}
