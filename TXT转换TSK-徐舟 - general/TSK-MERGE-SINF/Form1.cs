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
        private readonly List<string> _txtNameList = new List<string>();
        private readonly List<string> _tskNameList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            inkBinNoBox.SelectedItem = "61";
            markDieCompareBox.SelectedItem = "否";
            generalDeviceBox.SelectedItem = "是";
            waferIDCompareBox.SelectedItem = "否";
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
            _txtNameList.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox2.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    _txtNameList.Add(str.FullName);
                }
            }
        }

        private void buttonLoadTsk_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadTsk();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 加载tsk文件
        /// </summary>
        private void LoadTsk()
        {
            _tskNameList.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.SelectedPath;
                DirectoryInfo theFolder = new DirectoryInfo(this.textBox1.Text);

                foreach (FileInfo str in theFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    _tskNameList.Add(str.FullName);
                }
            }
        }

        private void txtAndTskMapMergeButton_Click(object sender, EventArgs e)
        {
            if (this.textBox2.Text == "")
            {
                MessageBox.Show("请选择txt图谱");
            }

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请选择TSK图谱");
            }

            for (int i = 0; i < _txtNameList.Count; i++)
            {
                string txtFile = _txtNameList[i];
                string tskFile = _tskNameList[0];
                if (_txtNameList.Count==_tskNameList.Count)
                    tskFile = _tskNameList[i];
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
            IncomingFileToTskTemplate incomingFilePattern = DeviceFactory.GetDeviceFromTsk(tsk.Device, generalDeviceBox.SelectedItem.ToString());
            incomingFilePattern.TxtName = _txtNameList; 
            incomingFilePattern.TskName = _tskNameList;
            incomingFilePattern.Run(tsk, txtFile, tskFile,inkBinNoBox.SelectedItem.ToString(), markDieCompareBox.SelectedItem.ToString(),waferIDCompareBox.SelectedItem.ToString());
        }

        private Tsk ParseTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read();
            return tsk;
        }
    }
}
