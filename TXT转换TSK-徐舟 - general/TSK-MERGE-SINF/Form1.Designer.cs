namespace TSK_MERGE_SINF
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button_loadTsk = new System.Windows.Forms.Button();
            this.button_loadTxt = new System.Windows.Forms.Button();
            this.txtAndTskMapMergeButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.inkBinNoBox = new System.Windows.Forms.ComboBox();
            this.markDieCompareBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.generalDeviceBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.waferIDCompareBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 227);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "TSK-MAP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 125);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "TXT-MAP:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(171, 227);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(504, 35);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(171, 125);
            this.textBox2.Margin = new System.Windows.Forms.Padding(5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(504, 35);
            this.textBox2.TabIndex = 3;
            // 
            // button_loadTsk
            // 
            this.button_loadTsk.Location = new System.Drawing.Point(733, 227);
            this.button_loadTsk.Margin = new System.Windows.Forms.Padding(5);
            this.button_loadTsk.Name = "button_loadTsk";
            this.button_loadTsk.Size = new System.Drawing.Size(149, 45);
            this.button_loadTsk.TabIndex = 4;
            this.button_loadTsk.Text = "Load TSK";
            this.button_loadTsk.UseVisualStyleBackColor = true;
            this.button_loadTsk.Click += new System.EventHandler(this.buttonLoadTsk_Click);
            // 
            // button_loadTxt
            // 
            this.button_loadTxt.Location = new System.Drawing.Point(733, 125);
            this.button_loadTxt.Margin = new System.Windows.Forms.Padding(5);
            this.button_loadTxt.Name = "button_loadTxt";
            this.button_loadTxt.Size = new System.Drawing.Size(149, 45);
            this.button_loadTxt.TabIndex = 5;
            this.button_loadTxt.Text = "Load Txt";
            this.button_loadTxt.UseVisualStyleBackColor = true;
            this.button_loadTxt.Click += new System.EventHandler(this.buttonLoadTxt_Click);
            // 
            // txtAndTskMapMergeButton
            // 
            this.txtAndTskMapMergeButton.Location = new System.Drawing.Point(373, 499);
            this.txtAndTskMapMergeButton.Margin = new System.Windows.Forms.Padding(5);
            this.txtAndTskMapMergeButton.Name = "txtAndTskMapMergeButton";
            this.txtAndTskMapMergeButton.Size = new System.Drawing.Size(381, 115);
            this.txtAndTskMapMergeButton.TabIndex = 6;
            this.txtAndTskMapMergeButton.Text = "开始转换";
            this.txtAndTskMapMergeButton.UseVisualStyleBackColor = true;
            this.txtAndTskMapMergeButton.Click += new System.EventHandler(this.txtAndTskMapMergeButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(132, 43);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(598, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "TXT和TSK需要数量一致，否则TSK会选择第一个作为模版";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 331);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "ink bin no:";
            // 
            // inkBinNoBox
            // 
            this.inkBinNoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inkBinNoBox.FormattingEnabled = true;
            this.inkBinNoBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64"});
            this.inkBinNoBox.Location = new System.Drawing.Point(205, 325);
            this.inkBinNoBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inkBinNoBox.Name = "inkBinNoBox";
            this.inkBinNoBox.Size = new System.Drawing.Size(173, 32);
            this.inkBinNoBox.TabIndex = 9;
            // 
            // markDieCompareBox
            // 
            this.markDieCompareBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.markDieCompareBox.FormattingEnabled = true;
            this.markDieCompareBox.Items.AddRange(new object[] {
            "是",
            "否"});
            this.markDieCompareBox.Location = new System.Drawing.Point(205, 417);
            this.markDieCompareBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.markDieCompareBox.Name = "markDieCompareBox";
            this.markDieCompareBox.Size = new System.Drawing.Size(173, 32);
            this.markDieCompareBox.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 423);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "对位点比较:";
            // 
            // generalDeviceBox
            // 
            this.generalDeviceBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.generalDeviceBox.FormattingEnabled = true;
            this.generalDeviceBox.Items.AddRange(new object[] {
            "是",
            "否"});
            this.generalDeviceBox.Location = new System.Drawing.Point(697, 325);
            this.generalDeviceBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.generalDeviceBox.Name = "generalDeviceBox";
            this.generalDeviceBox.Size = new System.Drawing.Size(173, 32);
            this.generalDeviceBox.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(521, 331);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 24);
            this.label6.TabIndex = 12;
            this.label6.Text = "TXT缺少信息:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(949, 125);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(404, 467);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "说明：\nink bin no将TSK中对应的TXT fail bin ink为指定bin；\nTXT缺少头信息，选是以TSK为准，否以TXT为准；\n对位点比较建议开" +
    "启，以防合错图，TXT没有对位点选否；\n比较WaferID，选是TXT和TSK中WaferID一致才合图，否不需要一致也会合图。";
            // 
            // waferIDCompareBox
            // 
            this.waferIDCompareBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.waferIDCompareBox.FormattingEnabled = true;
            this.waferIDCompareBox.Items.AddRange(new object[] {
            "是",
            "否"});
            this.waferIDCompareBox.Location = new System.Drawing.Point(697, 417);
            this.waferIDCompareBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waferIDCompareBox.Name = "waferIDCompareBox";
            this.waferIDCompareBox.Size = new System.Drawing.Size(173, 32);
            this.waferIDCompareBox.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(521, 423);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 24);
            this.label7.TabIndex = 15;
            this.label7.Text = "比较WaferID:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 707);
            this.Controls.Add(this.waferIDCompareBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.generalDeviceBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.markDieCompareBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.inkBinNoBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAndTskMapMergeButton);
            this.Controls.Add(this.button_loadTxt);
            this.Controls.Add(this.button_loadTsk);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.Text = "TXT-TO-TSK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button_loadTsk;
        private System.Windows.Forms.Button button_loadTxt;
        private System.Windows.Forms.Button txtAndTskMapMergeButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox inkBinNoBox;
        private System.Windows.Forms.ComboBox markDieCompareBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox generalDeviceBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox waferIDCompareBox;
        private System.Windows.Forms.Label label7;
    }
}

