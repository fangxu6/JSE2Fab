namespace DataToExcel
{
    partial class frmTIView
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTIView));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lsvItems = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.menuMappingFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnColor = new System.Windows.Forms.Button();
            this.cmbPrintScale = new System.Windows.Forms.ComboBox();
            this.cmbShowScale = new System.Windows.Forms.ComboBox();
            this.cmbFileType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtInfo = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pnlMapping = new System.Windows.Forms.Panel();
            this.menuPrint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuPrint_print = new System.Windows.Forms.ToolStripMenuItem();
            this.printer = new System.Drawing.Printing.PrintDocument();
            this.printView = new System.Windows.Forms.PrintPreviewDialog();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.menuMappingFile.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.menuPrint.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel7);
            this.splitContainer1.Size = new System.Drawing.Size(1028, 749);
            this.splitContainer1.SplitterDistance = 325;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lsvItems);
            this.splitContainer2.Panel1.Controls.Add(this.panel4);
            this.splitContainer2.Panel1.Controls.Add(this.panel3);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtInfo);
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(323, 747);
            this.splitContainer2.SplitterDistance = 332;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // lsvItems
            // 
            this.lsvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lsvItems.ContextMenuStrip = this.menuMappingFile;
            this.lsvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvItems.FullRowSelect = true;
            this.lsvItems.GridLines = true;
            this.lsvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvItems.Location = new System.Drawing.Point(0, 129);
            this.lsvItems.Name = "lsvItems";
            this.lsvItems.Size = new System.Drawing.Size(323, 202);
            this.lsvItems.TabIndex = 0;
            this.lsvItems.UseCompatibleStateImageBehavior = false;
            this.lsvItems.View = System.Windows.Forms.View.Details;
            this.lsvItems.DoubleClick += new System.EventHandler(this.lsvItems_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "mapping file";
            this.columnHeader1.Width = 320;
            // 
            // menuMappingFile
            // 
            this.menuMappingFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoad,
            this.menuClose});
            this.menuMappingFile.Name = "menuMappingFile";
            this.menuMappingFile.Size = new System.Drawing.Size(99, 48);
            // 
            // menuLoad
            // 
            this.menuLoad.Name = "menuLoad";
            this.menuLoad.Size = new System.Drawing.Size(98, 22);
            this.menuLoad.Text = "加载";
            this.menuLoad.Click += new System.EventHandler(this.menuLoad_Click);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(98, 22);
            this.menuClose.Text = "关闭";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 128);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(323, 1);
            this.panel4.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Controls.Add(this.btnColor);
            this.panel3.Controls.Add(this.cmbPrintScale);
            this.panel3.Controls.Add(this.cmbShowScale);
            this.panel3.Controls.Add(this.cmbFileType);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(323, 128);
            this.panel3.TabIndex = 1;
            // 
            // btnColor
            // 
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor.Location = new System.Drawing.Point(72, 93);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(75, 23);
            this.btnColor.TabIndex = 2;
            this.btnColor.Text = "调色板";
            this.btnColor.UseVisualStyleBackColor = true;
            // 
            // cmbPrintScale
            // 
            this.cmbPrintScale.Enabled = false;
            this.cmbPrintScale.FormattingEnabled = true;
            this.cmbPrintScale.Location = new System.Drawing.Point(72, 67);
            this.cmbPrintScale.Name = "cmbPrintScale";
            this.cmbPrintScale.Size = new System.Drawing.Size(179, 20);
            this.cmbPrintScale.TabIndex = 1;
            // 
            // cmbShowScale
            // 
            this.cmbShowScale.Enabled = false;
            this.cmbShowScale.FormattingEnabled = true;
            this.cmbShowScale.Location = new System.Drawing.Point(72, 40);
            this.cmbShowScale.Name = "cmbShowScale";
            this.cmbShowScale.Size = new System.Drawing.Size(179, 20);
            this.cmbShowScale.TabIndex = 0;
            // 
            // cmbFileType
            // 
            this.cmbFileType.FormattingEnabled = true;
            this.cmbFileType.Location = new System.Drawing.Point(72, 14);
            this.cmbFileType.Name = "cmbFileType";
            this.cmbFileType.Size = new System.Drawing.Size(179, 20);
            this.cmbFileType.TabIndex = 0;
            this.cmbFileType.SelectedIndexChanged += new System.EventHandler(this.cmbFileType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "文件类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "打印比例：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "显示比例：";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 331);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 1);
            this.panel1.TabIndex = 0;
            // 
            // txtInfo
            // 
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.Location = new System.Drawing.Point(0, 1);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(323, 411);
            this.txtInfo.TabIndex = 0;
            this.txtInfo.Text = "";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(323, 1);
            this.panel2.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.AutoScroll = true;
            this.panel7.Controls.Add(this.pnlMapping);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(698, 747);
            this.panel7.TabIndex = 3;
            // 
            // pnlMapping
            // 
            this.pnlMapping.BackColor = System.Drawing.SystemColors.Window;
            this.pnlMapping.ContextMenuStrip = this.menuPrint;
            this.pnlMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMapping.Location = new System.Drawing.Point(0, 0);
            this.pnlMapping.Name = "pnlMapping";
            this.pnlMapping.Size = new System.Drawing.Size(698, 747);
            this.pnlMapping.TabIndex = 0;
            this.pnlMapping.Resize += new System.EventHandler(this.pnlMapping_Resize);
            this.pnlMapping.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMapping_Paint);
            // 
            // menuPrint
            // 
            this.menuPrint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuPrint_print});
            this.menuPrint.Name = "menuPrint";
            this.menuPrint.Size = new System.Drawing.Size(137, 26);
            // 
            // menuPrint_print
            // 
            this.menuPrint_print.Name = "menuPrint_print";
            this.menuPrint_print.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.menuPrint_print.Size = new System.Drawing.Size(136, 22);
            this.menuPrint_print.Text = "打印";

            // 
            // printer
            // 
            this.printer.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printer_PrintPage);
            // 
            // printView
            // 
            this.printView.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printView.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printView.ClientSize = new System.Drawing.Size(400, 300);
            this.printView.Document = this.printer;
            this.printView.Enabled = true;
            this.printView.Icon = ((System.Drawing.Icon)(resources.GetObject("printView.Icon")));
            this.printView.Name = "printView";
            this.printView.Visible = false;
            // 
            // frmTIView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 749);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmTIView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TI Mapping View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmTIView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.menuMappingFile.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.menuPrint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlMapping;
        private System.Windows.Forms.ListView lsvItems;
        private System.Windows.Forms.ContextMenuStrip menuMappingFile;
        private System.Windows.Forms.ToolStripMenuItem menuLoad;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox txtInfo;
        private System.Windows.Forms.ContextMenuStrip menuPrint;
        private System.Windows.Forms.ToolStripMenuItem menuPrint_print;
        private System.Drawing.Printing.PrintDocument printer;
        private System.Windows.Forms.PrintPreviewDialog printView;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbFileType;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.ComboBox cmbPrintScale;
        private System.Windows.Forms.ComboBox cmbShowScale;
    }
}