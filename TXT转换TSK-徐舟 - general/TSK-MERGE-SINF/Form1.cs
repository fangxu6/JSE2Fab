using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DataToExcel;
using TSK_MERGE_SINF.Template;
using System;

namespace TSK_MERGE_SINF
{
    public partial class Form1 : Form
    {
        List<string> txt_Name = new List<string>();
        List<string> tsk_Name = new List<string>();

        int txtTotal = 0;
        int txtPass = 0;
        int txtFail = 0;
        int tskPass = 0;
        int tskFail = 0;


        List<string> txtData; //原始txt数据
        List<string> DegtxtData; //旋转角度后的txt数据
        List<string> txtNewData; //生成的txt数据
        //-----Sinf 头文件----//////
        string txtDevice;
        string txtLot;
        int txtSlot;
        string txtWaferID;
        string txtFlat;
        int txtRowct = 0;   //行数
        int txtColct = 0;   //列数

        int txtMark = 0;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedItem = "61";
            comboBox2.SelectedItem = "是";
        }

        private void buttonLoadTxt_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadTxt();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 加载txt文件
        /// </summary>
        private void LoadTxt()
        {
            txt_Name.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox2.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    txt_Name.Add(str.FullName);
                }
            }
        }

        private void buttonLoadTsk_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadTSK();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 加载tsk文件
        /// </summary>
        private void LoadTSK()
        {
            tsk_Name.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox1.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    tsk_Name.Add(str.FullName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text == "")
            {
                MessageBox.Show("请选择txt图谱");
            }

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请选择TSK图谱");
            }

            for (int i = 0; i < txt_Name.Count; i++)
            {
                string txtFile = txt_Name[i];
                string tskFile = tsk_Name[0];
                if (txt_Name.Count==tsk_Name.Count)
                    tskFile = tsk_Name[i];
                Txt2Tsk(txtFile, tskFile);
            }
            if (MessageBox.Show("转换成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("D:\\MERGE\\");
            }
        }

        private void Txt2Tsk(string txtFile, string tskFile)
        {
            Tsk tsk = ParseTsk(tskFile);
            IncomingFileToTskTemplate incomingFilePattern = DeviceFactory.GetDeviceFromTsk(tsk.Device);
            incomingFilePattern.Txt_Name = txt_Name;
            incomingFilePattern.Tsk_Name = tsk_Name;
            incomingFilePattern.Run(tsk, txtFile, comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
        }

        private Tsk ParseTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read();
            return tsk;
        }

    }
}
