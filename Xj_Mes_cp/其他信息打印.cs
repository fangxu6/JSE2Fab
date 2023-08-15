using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using Pawote.UI.Controls;
using Seagull.BarTender.Print;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class 其他信息打印 : DockContent
    {
        public 其他信息打印()
        {
            InitializeComponent();
        }

        #region 打印方法
        private void buttonX1_Click(object sender, EventArgs e)
        {
            #region 赋值

            string lot = this.textBoxX14.Text;
            string xh = this.textBoxX15.Text;
            string remark = this.textBoxX1.Text;

            string bin1 = this.textBoxX13.Text;
            string bin2 = this.textBoxX16.Text;
            string bin3 = this.textBoxX39.Text;
            string bin4 = this.textBoxX40.Text;
            string bin5 = this.textBoxX12.Text;
            string bin6 = this.textBoxX17.Text;
            string bin7 = this.textBoxX38.Text;
            string bin8 = this.textBoxX41.Text;
            string bin9 = this.textBoxX10.Text;
            string bin10 = this.textBoxX18.Text;
            string bin11 = this.textBoxX37.Text;
            string bin12 = this.textBoxX42.Text;
            string bin13 = this.textBoxX11.Text;
            string bin14 = this.textBoxX19.Text;
            string bin15 = this.textBoxX36.Text;
            string bin16 = this.textBoxX43.Text;
            string bin17 = this.textBoxX9.Text;
            string bin18 = this.textBoxX20.Text;
            string bin19 = this.textBoxX35.Text;
            string bin20 = this.textBoxX44.Text;
            string bin21 = this.textBoxX8.Text;
            string bin22 = this.textBoxX21.Text;
            string bin23 = this.textBoxX34.Text;
            string bin24 = this.textBoxX45.Text;
            string bin25 = this.textBoxX7.Text;
            string bin26 = this.textBoxX22.Text;
            string bin27 = this.textBoxX33.Text;
            string bin28 = this.textBoxX46.Text;
            string bin29 = this.textBoxX6.Text;
            string bin30 = this.textBoxX23.Text;
            string bin31 = this.textBoxX32.Text;
            string bin32 = this.textBoxX47.Text;
            string bin33 = this.textBoxX5.Text;
            string bin34 = this.textBoxX24.Text;
            string bin35 = this.textBoxX31.Text;
            string bin36 = this.textBoxX48.Text;
            string bin37 = this.textBoxX4.Text;
            string bin38 = this.textBoxX25.Text;
            string bin39 = this.textBoxX30.Text;
            string bin40 = this.textBoxX49.Text;
            string bin41 = this.textBoxX3.Text;
            string bin42 = this.textBoxX26.Text;
            string bin43 = this.textBoxX29.Text;
            string bin44 = this.textBoxX50.Text;
            string bin45 = this.textBoxX2.Text;
            string bin46 = this.textBoxX27.Text;
            string bin47 = this.textBoxX28.Text;
            string bin48 = this.textBoxX51.Text;

            string path = Application.StartupPath + @"\2_btw\其他信息查询.btw";
            #endregion

            #region 打印信息
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("lot", lot);
            Dic.Add("ipn", xh);
            Dic.Add("remark", remark);

            Dic.Add("bin1", bin1);
            Dic.Add("bin2", bin2);
            Dic.Add("bin3", bin3);
            Dic.Add("bin4", bin4);
            Dic.Add("bin5", bin5);
            Dic.Add("bin6", bin6);
            Dic.Add("bin7", bin7);
            Dic.Add("bin8", bin8);
            Dic.Add("bin9", bin9);
            Dic.Add("bin10", bin10);
            Dic.Add("bin11", bin11);
            Dic.Add("bin12", bin12);
            Dic.Add("bin13", bin13);
            Dic.Add("bin14", bin14);
            Dic.Add("bin15", bin15);
            Dic.Add("bin16", bin16);
            Dic.Add("bin17", bin17);
            Dic.Add("bin18", bin18);
            Dic.Add("bin19", bin19);
            Dic.Add("bin20", bin20);
            Dic.Add("bin21", bin21);
            Dic.Add("bin22", bin22);
            Dic.Add("bin23", bin23);
            Dic.Add("bin24", bin24);
            Dic.Add("bin25", bin25);
            Dic.Add("bin26", bin26);
            Dic.Add("bin27", bin27);
            Dic.Add("bin28", bin28);
            Dic.Add("bin29", bin29);
            Dic.Add("bin30", bin30);
            Dic.Add("bin31", bin31);
            Dic.Add("bin32", bin32);
            Dic.Add("bin33", bin33);
            Dic.Add("bin34", bin34);
            Dic.Add("bin35", bin35);
            Dic.Add("bin36", bin36);
            Dic.Add("bin37", bin37);
            Dic.Add("bin38", bin38);
            Dic.Add("bin39", bin39);
            Dic.Add("bin40", bin40);
            Dic.Add("bin41", bin41);
            Dic.Add("bin42", bin42);
            Dic.Add("bin43", bin43);
            Dic.Add("bin44", bin44);
            Dic.Add("bin45", bin45);
            Dic.Add("bin46", bin46);
            Dic.Add("bin47", bin47);
            Dic.Add("bin48", bin48); 
            #endregion

            DY(path,Dic,"1");
            MessageBox.Show("打印成功", "系统信息");
        }
        public void DY(string filename, Dictionary<string, string> Dic, string print_num)
        {
            Engine engine = new Engine();

            try
            {
                for (int i = 0; i < Convert.ToInt32(print_num); i++)
                {
                    string iPath = filename;
                    // 启动BarTender引擎
                    engine.Start();

                    // 打开标签格式文档
                    LabelFormatDocument format = engine.Documents.Open(iPath);

                    // 将字典中的值设置到标签模板对应的子字符串中
                    foreach (KeyValuePair<string, string> entry in Dic)
                    {
                        SubString subString = format.SubStrings[entry.Key];
                        subString.Value = entry.Value;
                    }

                    // 打印设置
                    //PrintSetup printSetup = format.PrintSetup;
                    ////printSetup.PrinterName = "Your_Printer_Name"; // 设置打印机名称
                    //printSetup.IdenticalCopiesOfLabel = Convert.ToInt32(print_num); // 控制打印份数

                    // 执行打印操作
                    format.Print("Barcode", waitForCompletionTimeout: 1500, messages: out Seagull.BarTender.Print.Messages messages);

                    // 关闭并保存格式化文档
                    format.Close(SaveOptions.SaveChanges);
                }


            }
            finally
            {
                // 停止BarTender引擎
                engine.Stop();
            }
        }
        #endregion

        #region 清空
        private void buttonX2_Click(object sender, EventArgs e)
        {
            Clear(tableLayoutPanel1);
        }
        protected void Clear(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is PwtSearchBox)
                {
                    ((PwtSearchBox)(c)).Text = "";
                }
                else if (c is TextBoxX)
                {
                    ((TextBoxX)(c)).Text = "";
                }
                else if (c is ComboBoxEx)
                {
                    ((ComboBoxEx)(c)).SelectedIndex = 0;
                }
            }
        }
        #endregion
    }
}
