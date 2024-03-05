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
            this.tSK合并封装厂TXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tSK合并封装厂TXT2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
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
            this.button1.Location = new System.Drawing.Point(9, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "ExportToExcel";
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
            this.lsvItems.Location = new System.Drawing.Point(6, 12);
            this.lsvItems.Name = "lsvItems";
            this.lsvItems.Size = new System.Drawing.Size(665, 393);
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
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFileMenuItem,
            this.clearFileMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 48);
            // 
            // loadFileMenuItem
            // 
            this.loadFileMenuItem.Name = "loadFileMenuItem";
            this.loadFileMenuItem.Size = new System.Drawing.Size(129, 22);
            this.loadFileMenuItem.Text = "Load File";
            this.loadFileMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // clearFileMenuItem
            // 
            this.clearFileMenuItem.Name = "clearFileMenuItem";
            this.clearFileMenuItem.Size = new System.Drawing.Size(129, 22);
            this.clearFileMenuItem.Text = "Clear File";
            this.clearFileMenuItem.Click += new System.EventHandler(this.clearFileMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 29);
            this.button2.TabIndex = 0;
            this.button2.Text = "Load  File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 55);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 29);
            this.button3.TabIndex = 1;
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
            this.FieldListBox1.Location = new System.Drawing.Point(566, 12);
            this.FieldListBox1.Name = "FieldListBox1";
            this.FieldListBox1.Size = new System.Drawing.Size(103, 306);
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
            this.groupBox1.Location = new System.Drawing.Point(0, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 434);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 411);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 20);
            this.panel1.TabIndex = 6;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.ForeColor = System.Drawing.Color.Green;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(668, 20);
            this.progressBar1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 490);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(844, 47);
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
            this.button5.TabIndex = 6;
            this.button5.Text = "Browse";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Target Path:";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(86, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(444, 21);
            this.textBox1.TabIndex = 4;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(9, 194);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(97, 29);
            this.button6.TabIndex = 4;
            this.button6.Text = "ExportToTma";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(9, 160);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(97, 28);
            this.button4.TabIndex = 3;
            this.button4.Text = "ExportToTxt";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.导出ToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.图谱合并ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(844, 25);
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
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(53, 21);
            this.文件ToolStripMenuItem.Text = "File(&F)";
            // 
            // setPathToolStripMenuItem
            // 
            this.setPathToolStripMenuItem.Name = "setPathToolStripMenuItem";
            this.setPathToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.setPathToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.setPathToolStripMenuItem.Text = "Set Path";
            this.setPathToolStripMenuItem.Click += new System.EventHandler(this.button5_Click);
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.loadFileToolStripMenuItem.Text = "Load File";
            this.loadFileToolStripMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // clearListToolStripMenuItem
            // 
            this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            this.clearListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.clearListToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
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
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(73, 21);
            this.导出ToolStripMenuItem.Text = "Export(&E)";
            // 
            // expToExcelToolStripMenuItem
            // 
            this.expToExcelToolStripMenuItem.Name = "expToExcelToolStripMenuItem";
            this.expToExcelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.expToExcelToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.expToExcelToolStripMenuItem.Text = "ExpToExcel";
            this.expToExcelToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // expToTxtToolStripMenuItem
            // 
            this.expToTxtToolStripMenuItem.Name = "expToTxtToolStripMenuItem";
            this.expToTxtToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.expToTxtToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.expToTxtToolStripMenuItem.Text = "ExpToTxt";
            this.expToTxtToolStripMenuItem.Click += new System.EventHandler(this.button4_Click);
            // 
            // expToTmaToolStripMenuItem
            // 
            this.expToTmaToolStripMenuItem.Name = "expToTmaToolStripMenuItem";
            this.expToTmaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.expToTmaToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.expToTmaToolStripMenuItem.Text = "ExpToTma";
            this.expToTmaToolStripMenuItem.Click += new System.EventHandler(this.button6_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.aboutToolStripMenuItem.Text = "Help(&H)";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(156, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // 图谱合并ToolStripMenuItem
            // 
            this.图谱合并ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sINF合并TSKToolStripMenuItem,
            this.tSK合并封装厂TXTToolStripMenuItem,
            this.tSK合并封装厂TXT2ToolStripMenuItem});
            this.图谱合并ToolStripMenuItem.Name = "图谱合并ToolStripMenuItem";
            this.图谱合并ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.图谱合并ToolStripMenuItem.Text = "图谱合并";
            // 
            // sINF合并TSKToolStripMenuItem
            // 
            this.sINF合并TSKToolStripMenuItem.Name = "sINF合并TSKToolStripMenuItem";
            this.sINF合并TSKToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.sINF合并TSKToolStripMenuItem.Text = "SINF合并TSK";
            this.sINF合并TSKToolStripMenuItem.Click += new System.EventHandler(this.sINF合并TSKToolStripMenuItem_Click);
            // 
            // tSK合并封装厂TXTToolStripMenuItem
            // 
            this.tSK合并封装厂TXTToolStripMenuItem.Name = "tSK合并封装厂TXTToolStripMenuItem";
            this.tSK合并封装厂TXTToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.tSK合并封装厂TXTToolStripMenuItem.Text = "TSK合并封装厂TXT";
            this.tSK合并封装厂TXTToolStripMenuItem.Click += new System.EventHandler(this.tSK合并封装厂TXTToolStripMenuItem_Click);
            // 
            // tSK合并封装厂TXT2ToolStripMenuItem
            // 
            this.tSK合并封装厂TXT2ToolStripMenuItem.Name = "tSK合并封装厂TXT2ToolStripMenuItem";
            this.tSK合并封装厂TXT2ToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.tSK合并封装厂TXT2ToolStripMenuItem.Text = "TSK合并封装厂TXT2-JP19A03";
            this.tSK合并封装厂TXT2ToolStripMenuItem.Click += new System.EventHandler(this.tSK合并封装厂TXT2ToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button12);
            this.groupBox3.Controls.Add(this.button11);
            this.groupBox3.Controls.Add(this.button10);
            this.groupBox3.Controls.Add(this.button9);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(680, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(164, 465);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(11, 383);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(95, 36);
            this.button12.TabIndex = 10;
            this.button12.Text = "Export_TXT2";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(9, 345);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(97, 32);
            this.button11.TabIndex = 9;
            this.button11.Text = "Export_MTXT";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(11, 307);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(97, 32);
            this.button10.TabIndex = 8;
            this.button10.Text = "Export_JCAPTXT";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button9.Location = new System.Drawing.Point(11, 270);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(95, 31);
            this.button9.TabIndex = 7;
            this.button9.Text = "Export_易冲8820";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button8.Location = new System.Drawing.Point(9, 229);
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
            this.button7.Location = new System.Drawing.Point(9, 125);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(97, 29);
            this.button7.TabIndex = 5;
            this.button7.Text = "ExportToAW";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(192, 474);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(120, 16);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "图谱顺时针转角度";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(330, 473);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(53, 16);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "90deg";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(410, 473);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(59, 16);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.Text = "180deg";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Enabled = false;
            this.radioButton3.Location = new System.Drawing.Point(490, 473);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(59, 16);
            this.radioButton3.TabIndex = 12;
            this.radioButton3.Text = "270deg";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // MappingToExcel
            // 
            this.ClientSize = new System.Drawing.Size(844, 537);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MappingToExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "易冲8820TSK转换工具V20220928";
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
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private ToolStripMenuItem tSK合并封装厂TXTToolStripMenuItem;
        private CheckBox checkBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private ToolStripMenuItem tSK合并封装厂TXT2ToolStripMenuItem;

    }
}