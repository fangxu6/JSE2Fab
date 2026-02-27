using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormTool.Forms
{
    public class StackedWafersDialog : Form
    {
        private Label lotSizeValueLabel;
        private NumericUpDown thresholdNumeric;
        private NumericUpDown targetBinNumeric;
        private TextBox previewTextBox;
        private Button previewButton;
        private Button applyButton;
        private Button cancelButton;

        public event Action PreviewRequested;

        public StackedWafersDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Stacked Wafers参数";
            this.Size = new Size(460, 360);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lotSizeLabel = new Label();
            lotSizeLabel.Text = "Lot片数：";
            lotSizeLabel.Location = new Point(20, 20);
            lotSizeLabel.Size = new Size(80, 20);
            this.Controls.Add(lotSizeLabel);

            lotSizeValueLabel = new Label();
            lotSizeValueLabel.Text = "0";
            lotSizeValueLabel.Location = new Point(110, 20);
            lotSizeValueLabel.Size = new Size(100, 20);
            this.Controls.Add(lotSizeValueLabel);

            var thresholdLabel = new Label();
            thresholdLabel.Text = "阈值(%)：";
            thresholdLabel.Location = new Point(20, 55);
            thresholdLabel.Size = new Size(80, 20);
            this.Controls.Add(thresholdLabel);

            thresholdNumeric = new NumericUpDown();
            thresholdNumeric.Location = new Point(110, 53);
            thresholdNumeric.Size = new Size(80, 23);
            thresholdNumeric.Minimum = 1;
            thresholdNumeric.Maximum = 100;
            thresholdNumeric.Value = 50;
            thresholdNumeric.DecimalPlaces = 0;
            this.Controls.Add(thresholdNumeric);

            var targetBinLabel = new Label();
            targetBinLabel.Text = "目标Bin号：";
            targetBinLabel.Location = new Point(20, 90);
            targetBinLabel.Size = new Size(80, 20);
            this.Controls.Add(targetBinLabel);

            targetBinNumeric = new NumericUpDown();
            targetBinNumeric.Location = new Point(110, 88);
            targetBinNumeric.Size = new Size(80, 23);
            targetBinNumeric.Minimum = 1;
            targetBinNumeric.Maximum = 255;
            targetBinNumeric.Value = 63;
            this.Controls.Add(targetBinNumeric);

            previewTextBox = new TextBox();
            previewTextBox.Location = new Point(20, 125);
            previewTextBox.Size = new Size(400, 150);
            previewTextBox.ReadOnly = true;
            previewTextBox.Multiline = true;
            previewTextBox.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(previewTextBox);

            previewButton = new Button();
            previewButton.Text = "预览";
            previewButton.Location = new Point(20, 285);
            previewButton.Size = new Size(80, 30);
            previewButton.Click += PreviewButton_Click;
            this.Controls.Add(previewButton);

            applyButton = new Button();
            applyButton.Text = "应用";
            applyButton.Location = new Point(120, 285);
            applyButton.Size = new Size(80, 30);
            applyButton.DialogResult = DialogResult.OK;
            applyButton.Click += ApplyButton_Click;
            this.Controls.Add(applyButton);

            cancelButton = new Button();
            cancelButton.Text = "取消";
            cancelButton.Location = new Point(220, 285);
            cancelButton.Size = new Size(80, 30);
            cancelButton.DialogResult = DialogResult.Cancel;
            this.Controls.Add(cancelButton);

            this.AcceptButton = applyButton;
            this.CancelButton = cancelButton;
        }

        public void SetLotSize(int lotSize)
        {
            if (lotSize < 0)
            {
                lotSize = 0;
            }

            lotSizeValueLabel.Text = lotSize.ToString();
        }

        public double GetThresholdFraction()
        {
            return (double)thresholdNumeric.Value / 100.0;
        }

        public int GetTargetBinNo()
        {
            return (int)targetBinNumeric.Value;
        }

        public void ShowPreviewResult(string result)
        {
            previewTextBox.Text = result;
        }

        private void PreviewButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            ShowPreviewResult("正在预览...");
            OnPreviewRequested();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateInputs()
        {
            var threshold = (int)thresholdNumeric.Value;
            if (threshold <= 0 || threshold > 100)
            {
                MessageBox.Show(@"阈值必须在1-100之间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                thresholdNumeric.Focus();
                return false;
            }

            var targetBinNo = (int)targetBinNumeric.Value;
            if (targetBinNo < 1 || targetBinNo > 255)
            {
                MessageBox.Show(@"目标Bin号必须在1-255之间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                targetBinNumeric.Focus();
                return false;
            }

            return true;
        }

        protected virtual void OnPreviewRequested()
        {
            PreviewRequested?.Invoke();
        }
    }
}
