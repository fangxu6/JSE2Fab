using DataToExcel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormTool
{
    public partial class Form1 : Form
    {
        public string filesPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filesPath = dialog.SelectedPath;
            }
            UpdateRichTextBox($"已加载目标TSK文件夹：{dialog.SelectedPath}\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var txtToTma = new TxtToTma();
            txtToTma.BatchConvert(filesPath, UpdateRichTextBox); ;
        }

        
        //更新RichTextBox
        private void UpdateRichTextBox(string message)
        {
            richTextBox1.AppendText(message);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret(); ;
            Application.DoEvents();
        }
    }

}
