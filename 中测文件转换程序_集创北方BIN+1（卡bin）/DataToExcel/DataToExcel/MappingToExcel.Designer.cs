namespace DataToExcel
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    partial class MappingToExcel
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
            this.button1 = new System.Windows.Forms.Button();
            this.lsvItems = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.FieldListBox1 = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expToTmaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.图谱合并ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sINF合并TSKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 109);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "TSK转90度";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lsvItems
            // 
            this.lsvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvItems.BackColor = System.Drawing.SystemColors.Window;
            this.lsvItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvItems.ContextMenuStrip = this.contextMenuStrip1;
            this.lsvItems.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lsvItems.FullRowSelect = true;
            this.lsvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvItems.HideSelection = false;
            this.lsvItems.Location = new System.Drawing.Point(6, 12);
            this.lsvItems.Name = "lsvItems";
            this.lsvItems.Size = new System.Drawing.Size(590, 346);
            this.lsvItems.TabIndex = 3;
            this.lsvItems.UseCompatibleStateImageBehavior = false;
            this.lsvItems.View = System.Windows.Forms.View.Details;
            this.lsvItems.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lsvItems_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Mapping File";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Path";
            this.columnHeader2.Width = 500;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFileMenuItem,
            this.clearFileMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 64);
            // 
            // loadFileMenuItem
            // 
            this.loadFileMenuItem.Name = "loadFileMenuItem";
            this.loadFileMenuItem.Size = new System.Drawing.Size(159, 30);
            this.loadFileMenuItem.Text = "Load File";
            this.loadFileMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // clearFileMenuItem
            // 
            this.clearFileMenuItem.Name = "clearFileMenuItem";
            this.clearFileMenuItem.Size = new System.Drawing.Size(159, 30);
            this.clearFileMenuItem.Text = "Clear File";
            this.clearFileMenuItem.Click += new System.EventHandler(this.clearFileMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 29);
            this.button2.TabIndex = 1;
            this.button2.Text = "Load  File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 29);
            this.button3.TabIndex = 2;
            this.button3.Text = "Clear  File";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.clearFileMenuItem_Click);
            // 
            // FieldListBox1
            // 
            this.FieldListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FieldListBox1.BackColor = System.Drawing.SystemColors.Window;
            this.FieldListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FieldListBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FieldListBox1.FormattingEnabled = true;
            this.FieldListBox1.Location = new System.Drawing.Point(491, 12);
            this.FieldListBox1.Name = "FieldListBox1";
            this.FieldListBox1.Size = new System.Drawing.Size(103, 252);
            this.FieldListBox1.TabIndex = 4;
            this.FieldListBox1.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lsvItems);
            this.groupBox1.Controls.Add(this.FieldListBox1);
            this.groupBox1.Location = new System.Drawing.Point(0, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(599, 387);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 364);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(593, 20);
            this.panel1.TabIndex = 6;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.ForeColor = System.Drawing.Color.Green;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(593, 20);
            this.progressBar1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 405);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(723, 47);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(5, 43);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(528, 21);
            this.panel3.TabIndex = 7;
            this.panel3.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Location = new System.Drawing.Point(5, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(645, 28);
            this.panel2.TabIndex = 6;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(532, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(60, 24);
            this.button5.TabIndex = 11;
            this.button5.Text = "Browse";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Target Path:";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(86, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(444, 28);
            this.textBox1.TabIndex = 10;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(9, 255);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(97, 29);
            this.button6.TabIndex = 5;
            this.button6.Text = "ExportToTma";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(9, 206);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(97, 29);
            this.button4.TabIndex = 4;
            this.button4.Text = "ExportToTxt";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.导出ToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.图谱合并ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(723, 32);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setPathToolStripMenuItem,
            this.loadFileToolStripMenuItem,
            this.clearListToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(78, 28);
            this.文件ToolStripMenuItem.Text = "File(&F)";
            // 
            // setPathToolStripMenuItem
            // 
            this.setPathToolStripMenuItem.Name = "setPathToolStripMenuItem";
            this.setPathToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.setPathToolStripMenuItem.Size = new System.Drawing.Size(254, 34);
            this.setPathToolStripMenuItem.Text = "Set Path";
            this.setPathToolStripMenuItem.Click += new System.EventHandler(this.button5_Click);
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(254, 34);
            this.loadFileToolStripMenuItem.Text = "Load File";
            this.loadFileToolStripMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(254, 34);
            this.clearListToolStripMenuItem.Text = "Clear List";
            this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearFileMenuItem_Click);
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expToExcelToolStripMenuItem,
            this.expToTxtToolStripMenuItem,
            this.expToTmaToolStripMenuItem});
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(104, 28);
            this.导出ToolStripMenuItem.Text = "Export(&E)";
            // 
            // expToExcelToolStripMenuItem
            // 
            this.expToExcelToolStripMenuItem.Name = "expToExcelToolStripMenuItem";
            this.expToExcelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.expToExcelToolStripMenuItem.Size = new System.Drawing.Size(271, 34);
            this.expToExcelToolStripMenuItem.Text = "ExpToExcel";
            this.expToExcelToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // expToTxtToolStripMenuItem
            // 
            this.expToTxtToolStripMenuItem.Name = "expToTxtToolStripMenuItem";
            this.expToTxtToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.expToTxtToolStripMenuItem.Size = new System.Drawing.Size(271, 34);
            this.expToTxtToolStripMenuItem.Text = "ExpToTxt";
            this.expToTxtToolStripMenuItem.Click += new System.EventHandler(this.button4_Click);
            // 
            // expToTmaToolStripMenuItem
            // 
            this.expToTmaToolStripMenuItem.Name = "expToTmaToolStripMenuItem";
            this.expToTmaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.expToTmaToolStripMenuItem.Size = new System.Drawing.Size(271, 34);
            this.expToTmaToolStripMenuItem.Text = "ExpToTma";
            this.expToTmaToolStripMenuItem.Click += new System.EventHandler(this.button6_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(93, 28);
            this.aboutToolStripMenuItem.Text = "Help(&H)";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(231, 34);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // 图谱合并ToolStripMenuItem
            // 
            this.图谱合并ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sINF合并TSKToolStripMenuItem});
            this.图谱合并ToolStripMenuItem.Name = "图谱合并ToolStripMenuItem";
            this.图谱合并ToolStripMenuItem.Size = new System.Drawing.Size(98, 28);
            this.图谱合并ToolStripMenuItem.Text = "图谱合并";
            // 
            // sINF合并TSKToolStripMenuItem
            // 
            this.sINF合并TSKToolStripMenuItem.Name = "sINF合并TSKToolStripMenuItem";
            this.sINF合并TSKToolStripMenuItem.Size = new System.Drawing.Size(217, 34);
            this.sINF合并TSKToolStripMenuItem.Text = "SINF合并TSK";
            this.sINF合并TSKToolStripMenuItem.Click += new System.EventHandler(this.sINF合并TSKToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(607, 32);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(116, 373);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button8.Location = new System.Drawing.Point(9, 305);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(97, 35);
            this.button8.TabIndex = 6;
            this.button8.Text = "图谱堆叠";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button7.Location = new System.Drawing.Point(9, 155);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(97, 39);
            this.button7.TabIndex = 3;
            this.button7.Text = "ExportToXLSX";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // MappingToExcel
            // 
            this.ClientSize = new System.Drawing.Size(723, 452);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MappingToExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "矽捷TSK转换集创工具V1.1-Build by Aegon_20230612-管控SBL";
            this.Load += new System.EventHandler(this.MappingToExcel_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ToolStripMenuItem clearFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem expToExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expToTmaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expToTxtToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox FieldListBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem loadFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ListView lsvItems;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem setPathToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private Button button7;
        private ToolStripMenuItem 图谱合并ToolStripMenuItem;
        private ToolStripMenuItem sINF合并TSKToolStripMenuItem;
        private Button button8;

    }
}