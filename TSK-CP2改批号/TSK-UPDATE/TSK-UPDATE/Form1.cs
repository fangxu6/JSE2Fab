using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Security.Cryptography;

namespace TSK_UPDATE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ArrayList tsk_Name1 = new ArrayList();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadTSKFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void LoadTSKFile()
        {
            this.LoadTSK();
        }

        private void LoadTSK()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox1.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    tsk_Name1.Add(str.Name);

                }
            }
        }




        private void button2_Click(object sender, EventArgs e)
        {

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请选择TSK图谱");
                return;
            }
            if (this.textBox2.Text == "")
            {
                MessageBox.Show("请输入批次号");
                return;
            }


            string newLot = this.textBox2.Text;

            for (int ii = 0; ii < tsk_Name1.Count; ii++)
            {

                string newFileName = getNewLotName((string)tsk_Name1[ii], newLot);
                if (newFileName == null)
                {
                    return;
                }
                string src = this.textBox1.Text + @"\" + tsk_Name1[ii];
                string dest = this.textBox1.Text + @"\" + newFileName;
                File.Move(src, dest);

            }


            if (MessageBox.Show("转换成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(this.textBox1.Text);
            }


        }



        private void Reverse(ref byte[] target)
        {
            int n1 = 0, n2 = target.Length - 1;
            byte temp;
            while (n1 < n2)
            {
                temp = target[n1];
                target[n1] = target[n2];
                target[n2] = temp;

                n1++;
                n2--;
            }
        }

        private short ByteToInt16(ref byte[] target)
        {
            this.Reverse(ref target);
            return BitConverter.ToInt16(target, 0);

        }

        private string getNewLotName(string originalFileName, string newLot)
        {
            if (!originalFileName.Contains("-"))
            {
                MessageBox.Show(originalFileName+"文件没有'-'分隔符，无法替换");
                return null;
            }
            string[] words = originalFileName.Split('-');
            if (words[0].Contains("."))
            {
                string oldLot = words[0].Substring(words[0].IndexOf(".")+1);
                words[0] = words[0].Replace(oldLot, newLot);
            }
            else
            {
                words[0] = newLot;
            }



            string str = string.Join("-", words);
            return str;
        }



    }
}
