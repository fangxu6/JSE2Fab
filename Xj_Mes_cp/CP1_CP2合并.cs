using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Xj_Mes_cp
{
    public partial class CP1_CP2合并 : DockContent
    {
        public CP1_CP2合并()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            try
            {
                this.buttonX1.Enabled = false;
                string lot = this.textBoxX1.Text;
                if (lot == "")
                {
                    return;
                }


                Dictionary<string, string> dic = new Dictionary<string, string>();

                dic.Add("lot", lot);
                //夜班产量推送
                string web = "http://192.168.5.242:9950/CPTskDeal/TskDealAdd";

                pwt_system_comm.WebHelper webhelper = new pwt_system_comm.WebHelper();
                string res = webhelper.HttpPostRequest(web, dic);

                string Path = @"\\192.168.5.26\共享文件夹\CP1_CP2合并\" + lot;

                System.Diagnostics.Process.Start(Path);
                MessageBox.Show("执行结果：" + res, "系统提示");

            }
            catch (Exception exerror)
            {
                MessageBox.Show("系统错误：" + exerror.Message.ToString(), "系统提示");
            }
            finally
            {
                this.buttonX1.Enabled = true;
            }
        }
    }
}
