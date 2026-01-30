namespace txt2tma
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblFilePath;
        private TextBox txtFilePath;
        private Button btnSelectFile;
        private Button btnConvert;
        private Button btnBatchConvert;
        private ProgressBar progressBar;
        private Label lblStatus;
        private TextBox txtLog;
        private Button btnSelectFolder;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblFilePath = new Label();
            this.txtFilePath = new TextBox();
            this.btnSelectFile = new Button();
            this.btnConvert = new Button();
            this.btnBatchConvert = new Button();
            this.progressBar = new ProgressBar();
            this.lblStatus = new Label();
            this.txtLog = new TextBox();
            this.btnSelectFolder = new Button();
            this.SuspendLayout();

            //
            // lblFilePath
            //
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(12, 15);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(53, 12);
            this.lblFilePath.TabIndex = 0;
            this.lblFilePath.Text = "文件路径:";

            //
            // txtFilePath
            //
            this.txtFilePath.Location = new System.Drawing.Point(75, 12);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(400, 21);
            this.txtFilePath.TabIndex = 1;

            //
            // btnSelectFile
            //
            this.btnSelectFile.Location = new System.Drawing.Point(485, 10);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);

            //
            // btnConvert
            //
            this.btnConvert.Location = new System.Drawing.Point(12, 50);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(100, 30);
            this.btnConvert.TabIndex = 3;
            this.btnConvert.Text = "转换";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);

            //
            // btnBatchConvert
            //
            this.btnBatchConvert.Location = new System.Drawing.Point(120, 50);
            this.btnBatchConvert.Name = "btnBatchConvert";
            this.btnBatchConvert.Size = new System.Drawing.Size(100, 30);
            this.btnBatchConvert.TabIndex = 4;
            this.btnBatchConvert.Text = "批量转换";
            this.btnBatchConvert.UseVisualStyleBackColor = true;
            this.btnBatchConvert.Click += new System.EventHandler(this.btnBatchConvert_Click);

            //
            // btnSelectFolder
            //
            this.btnSelectFolder.Location = new System.Drawing.Point(230, 50);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(100, 30);
            this.btnSelectFolder.TabIndex = 5;
            this.btnSelectFolder.Text = "选择文件夹";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);

            //
            // progressBar
            //
            this.progressBar.Location = new System.Drawing.Point(12, 95);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(550, 20);
            this.progressBar.TabIndex = 6;

            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 125);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(29, 12);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "就绪";

            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(12, 150);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(550, 200);
            this.txtLog.TabIndex = 8;

            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 362);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnBatchConvert);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFilePath);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "txt2tma 转换器";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
