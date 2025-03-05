namespace MES_TMS_IF
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMesMsg = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtSendToTmsMsg = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnFormatSendToTmsMsg = new System.Windows.Forms.Button();
            this.btnToTmsSend = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.txtTmsServiceId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartMes = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTmsMsg = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtSendToMesMsg = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnFormatSendToMesMsg = new System.Windows.Forms.Button();
            this.btnToMesSned = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.txtMesServiceId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartTms = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 669F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1428, 669);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 663);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMesMsg);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 300);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(708, 363);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Receive Message";
            // 
            // txtMesMsg
            // 
            this.txtMesMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMesMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMesMsg.Location = new System.Drawing.Point(3, 17);
            this.txtMesMsg.Multiline = true;
            this.txtMesMsg.Name = "txtMesMsg";
            this.txtMesMsg.ReadOnly = true;
            this.txtMesMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMesMsg.Size = new System.Drawing.Size(702, 343);
            this.txtMesMsg.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtSendToTmsMsg);
            this.groupBox3.Controls.Add(this.panel5);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 49);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(708, 251);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Send Message To TMS";
            // 
            // txtSendToTmsMsg
            // 
            this.txtSendToTmsMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSendToTmsMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendToTmsMsg.Location = new System.Drawing.Point(3, 61);
            this.txtSendToTmsMsg.MaxLength = 0;
            this.txtSendToTmsMsg.Multiline = true;
            this.txtSendToTmsMsg.Name = "txtSendToTmsMsg";
            this.txtSendToTmsMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSendToTmsMsg.Size = new System.Drawing.Size(702, 187);
            this.txtSendToTmsMsg.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnFormatSendToTmsMsg);
            this.panel5.Controls.Add(this.btnToTmsSend);
            this.panel5.Controls.Add(this.textBox4);
            this.panel5.Controls.Add(this.txtTmsServiceId);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(3, 17);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(702, 44);
            this.panel5.TabIndex = 3;
            // 
            // btnFormatSendToTmsMsg
            // 
            this.btnFormatSendToTmsMsg.Location = new System.Drawing.Point(583, 4);
            this.btnFormatSendToTmsMsg.Name = "btnFormatSendToTmsMsg";
            this.btnFormatSendToTmsMsg.Size = new System.Drawing.Size(75, 34);
            this.btnFormatSendToTmsMsg.TabIndex = 2;
            this.btnFormatSendToTmsMsg.Text = "Format";
            this.btnFormatSendToTmsMsg.UseVisualStyleBackColor = true;
            this.btnFormatSendToTmsMsg.Click += new System.EventHandler(this.btnFormatSendToTmsMsg_Click);
            // 
            // btnToTmsSend
            // 
            this.btnToTmsSend.Location = new System.Drawing.Point(502, 4);
            this.btnToTmsSend.Name = "btnToTmsSend";
            this.btnToTmsSend.Size = new System.Drawing.Size(75, 34);
            this.btnToTmsSend.TabIndex = 2;
            this.btnToTmsSend.Text = "Send";
            this.btnToTmsSend.UseVisualStyleBackColor = true;
            this.btnToTmsSend.Click += new System.EventHandler(this.btnToTmsSend_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(337, 16);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(159, 21);
            this.textBox4.TabIndex = 1;
            this.textBox4.Text = "XJ.TMS.DEV.DataEx.API";
            // 
            // txtTmsServiceId
            // 
            this.txtTmsServiceId.Location = new System.Drawing.Point(77, 16);
            this.txtTmsServiceId.Name = "txtTmsServiceId";
            this.txtTmsServiceId.Size = new System.Drawing.Size(159, 21);
            this.txtTmsServiceId.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(242, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Target Subject";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Service Id";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnStartMes);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(708, 49);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(2, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "MES Server";
            // 
            // btnStartMes
            // 
            this.btnStartMes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartMes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartMes.Location = new System.Drawing.Point(573, 5);
            this.btnStartMes.Name = "btnStartMes";
            this.btnStartMes.Size = new System.Drawing.Size(128, 36);
            this.btnStartMes.TabIndex = 0;
            this.btnStartMes.Text = "StartService";
            this.btnStartMes.UseVisualStyleBackColor = true;
            this.btnStartMes.Click += new System.EventHandler(this.btnStartMes_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(717, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 663);
            this.panel2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTmsMsg);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 300);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(708, 363);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Receive Message";
            // 
            // txtTmsMsg
            // 
            this.txtTmsMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTmsMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTmsMsg.Location = new System.Drawing.Point(3, 17);
            this.txtTmsMsg.Multiline = true;
            this.txtTmsMsg.Name = "txtTmsMsg";
            this.txtTmsMsg.ReadOnly = true;
            this.txtTmsMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTmsMsg.Size = new System.Drawing.Size(702, 343);
            this.txtTmsMsg.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtSendToMesMsg);
            this.groupBox4.Controls.Add(this.panel6);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 49);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(708, 251);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Send Message To MES";
            // 
            // txtSendToMesMsg
            // 
            this.txtSendToMesMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSendToMesMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendToMesMsg.Location = new System.Drawing.Point(3, 61);
            this.txtSendToMesMsg.MaxLength = 0;
            this.txtSendToMesMsg.Multiline = true;
            this.txtSendToMesMsg.Name = "txtSendToMesMsg";
            this.txtSendToMesMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSendToMesMsg.Size = new System.Drawing.Size(702, 187);
            this.txtSendToMesMsg.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnFormatSendToMesMsg);
            this.panel6.Controls.Add(this.btnToMesSned);
            this.panel6.Controls.Add(this.textBox5);
            this.panel6.Controls.Add(this.txtMesServiceId);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.label6);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 17);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(702, 44);
            this.panel6.TabIndex = 5;
            // 
            // btnFormatSendToMesMsg
            // 
            this.btnFormatSendToMesMsg.Location = new System.Drawing.Point(601, 4);
            this.btnFormatSendToMesMsg.Name = "btnFormatSendToMesMsg";
            this.btnFormatSendToMesMsg.Size = new System.Drawing.Size(75, 34);
            this.btnFormatSendToMesMsg.TabIndex = 2;
            this.btnFormatSendToMesMsg.Text = "Format";
            this.btnFormatSendToMesMsg.UseVisualStyleBackColor = true;
            this.btnFormatSendToMesMsg.Click += new System.EventHandler(this.btnFormatSendToMesMsg_Click);
            // 
            // btnToMesSned
            // 
            this.btnToMesSned.Location = new System.Drawing.Point(520, 4);
            this.btnToMesSned.Name = "btnToMesSned";
            this.btnToMesSned.Size = new System.Drawing.Size(75, 34);
            this.btnToMesSned.TabIndex = 2;
            this.btnToMesSned.Text = "Send";
            this.btnToMesSned.UseVisualStyleBackColor = true;
            this.btnToMesSned.Click += new System.EventHandler(this.btnToMesSned_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(337, 16);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(159, 21);
            this.textBox5.TabIndex = 1;
            this.textBox5.Text = "XJ.MES.DEV.DataEx.API";
            // 
            // txtMesServiceId
            // 
            this.txtMesServiceId.Location = new System.Drawing.Point(77, 16);
            this.txtMesServiceId.Name = "txtMesServiceId";
            this.txtMesServiceId.Size = new System.Drawing.Size(159, 21);
            this.txtMesServiceId.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Target Subject";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Service Id";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.btnStartTms);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(708, 49);
            this.panel4.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "TMS Server";
            // 
            // btnStartTms
            // 
            this.btnStartTms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartTms.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartTms.Location = new System.Drawing.Point(574, 5);
            this.btnStartTms.Name = "btnStartTms";
            this.btnStartTms.Size = new System.Drawing.Size(128, 36);
            this.btnStartTms.TabIndex = 1;
            this.btnStartTms.Text = "StartService";
            this.btnStartTms.UseVisualStyleBackColor = true;
            this.btnStartTms.Click += new System.EventHandler(this.btnStartTms_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 669);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnStartMes;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnStartTms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTmsMsg;
        private System.Windows.Forms.TextBox txtMesMsg;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtSendToTmsMsg;
        private System.Windows.Forms.TextBox txtSendToMesMsg;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox txtTmsServiceId;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox txtMesServiceId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnToTmsSend;
        private System.Windows.Forms.Button btnToMesSned;
        private System.Windows.Forms.Button btnFormatSendToTmsMsg;
        private System.Windows.Forms.Button btnFormatSendToMesMsg;
    }
}

