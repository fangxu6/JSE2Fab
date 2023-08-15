using Pwt_Tsk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xj_Mes_cp
{
    public partial class LOT下线扫描_TSK采集 : Form
    {
        public LOT下线扫描_TSK采集( List<string> list_eq, string lot,string lot_only_code)
        {
            InitializeComponent();
            my_lot = lot;
            my_list_eq = list_eq;
            my_lot_only_code = lot_only_code;
        }

        List<string> my_list_eq = new List<string>();
        string my_lot = "";
        string my_lot_only_code = "";



        db_deal ex = new db_deal();
        private void LOT下线扫描_TSK采集_Load(object sender, EventArgs e)
        {
            this.pwtDataGridView1.Rows.Clear();
            foreach (var item in my_list_eq)
            {
                this.pwtDataGridView1.Rows.Add();
                this.pwtDataGridView1.Rows[this.pwtDataGridView1.Rows.Count - 1].Cells[0].Value = this.pwtDataGridView1.Rows.Count;
                this.pwtDataGridView1.Rows[this.pwtDataGridView1.Rows.Count - 1].Cells[1].Value = item;
            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        private void pwtDataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.pwtDataGridView1.SelectedRows.Count==0)
            {
                return;
            }
            string eq_name = this.pwtDataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            DataTable dt = ex.Get_Data("[dbo].[hp_1022_cp_up_line_info_get_post_info_select]  '" + my_lot_only_code + "','" + eq_name + "'");


            if (dt.Rows.Count==0)
            {
                MessageBox.Show("没有片号信息"); return;
            }

            string[] post_info = dt.Rows[0][0].ToString().Split('、');

            for (int i = 0; i < post_info.Length; i++)
            {
                this.pwtDataGridView2.Rows.Add();
                this.pwtDataGridView2.Rows[this.pwtDataGridView2.Rows.Count - 1].Cells[0].Value = this.pwtDataGridView2.Rows.Count;
                this.pwtDataGridView2.Rows[this.pwtDataGridView2.Rows.Count - 1].Cells[1].Value = post_info[i];
            }

        }



        db_deal exdb = new db_deal();
        private void buttonX2_Click(object sender, EventArgs e)
        {
            
            FolderBrowserDialog  of = new FolderBrowserDialog();

            of.SelectedPath = @"\\192.168.5.26\prober\MAP\";
           
            of.ShowDialog();

            string path = of.SelectedPath;
            this.textBoxX1.Text = path;

            //\\192.168.5.26\prober\MAP
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (var item in folder.GetFiles())
            {
                if (item.FullName.ToUpper().Contains(".INI"))
                {
                    continue;
                }
                if (item.FullName.ToUpper().Contains(".XLS"))
                {
                    continue;
                }

                this.pwtDataGridView4.Rows.Add();
                this.pwtDataGridView4.Rows[this.pwtDataGridView4.Rows.Count - 1].Cells[0].Value = this.pwtDataGridView4.Rows.Count;
                this.pwtDataGridView4.Rows[this.pwtDataGridView4.Rows.Count - 1].Cells[1].Value = item.Name;
                this.pwtDataGridView4.Rows[this.pwtDataGridView4.Rows.Count - 1].Cells[2].Value = "待上传";
                this.pwtDataGridView4.Rows[this.pwtDataGridView4.Rows.Count - 1].Cells[3].Value = item.FullName;
            }



            string lot = path.Substring(path.LastIndexOf('\\') + 1);


            string temp_DirPath = path.Substring(0,path.LastIndexOf('\\') );
            string EqName = temp_DirPath.Substring(temp_DirPath.LastIndexOf('\\') + 1);

            this.textBoxX3.Text = lot;
            this.textBoxX2.Text = EqName;


            if (!lot.Contains(my_lot))
            {
                MessageBox.Show("文件选择错误,批次号不一致\r\n请重新选择","系统提示");
            }

            this.pwtDataGridView4.Rows.Clear();
            


        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = this.pwtDataGridView4.Rows.Count-1;

            for (int i = 0; i < this.pwtDataGridView4.Rows.Count; i++)
            {
                CollectTSK(this.pwtDataGridView4.Rows[i].Cells[1].Value.ToString());
                this.progressBar1.Value = i;
            }

            
        }


        private void CollectTSK(string load_FilePath)
        {


            string FilePath = load_FilePath;// @"C:\Users\Administrator\Desktop\test_tsk\001.5K554500-1";
            string FileName = "";
            string DirPath = "";
            string EqName = "";

            FileInfo fi = new FileInfo(FilePath);

            FileName = fi.Name;

            DirPath = fi.Directory.ToString();
            string temp_DirPath = DirPath.Substring(0, fi.Directory.ToString().LastIndexOf('\\')); ;
            EqName = temp_DirPath.Substring(temp_DirPath.LastIndexOf('\\') + 1);

            Pwt_Tsk.Tsk tsk = new Pwt_Tsk.Tsk(FilePath);

            tsk.Read();

            #region 基础资料获取

            Dictionary<string, string> tsk_info = new Dictionary<string, string>();
            Hashtable hshTB = tsk.Properties;
            foreach (DictionaryEntry fl in hshTB) //获取文件
            {
                string Key = fl.Key.ToString(); //Key
                string Value = fl.Value.ToString(); //Value
                tsk_info.Add(Key, Value);
            }




            string iLotNo = tsk.Properties["LotNo"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iWaferID = tsk.Properties["WaferID"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iDevice = tsk.Properties["Device"].ToString().Replace("\0", "").Replace("\r\n", "");



            string iIndexX = tsk.Properties["IndexSizeX"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iIndexY = tsk.Properties["IndexSizeY"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iWaferSize = tsk.Properties["WaferSize"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iOF_Direction = tsk.Properties["FlatDir"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iLoadTime = tsk.Properties["LoadTime"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iUnloadTime = tsk.Properties["UnloadTime"].ToString().Replace("\0", "").Replace("\r\n", "");

            string istartTime = tsk.Properties["StartTime"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iendTime = tsk.Properties["EndTime"].ToString().Replace("\0", "").Replace("\r\n", "");
            string iUsedTime = (DateTime.Parse(iendTime) - DateTime.Parse(istartTime)).ToString();



            long iTotal = 0;
            long iPass = 0;

            #endregion

            DieMatrix tsk_Die = tsk.DieMatrix;

            ICollection tsk_Die_List = new List<DieData>();

            if (tsk_Die != null)
            {
                tsk_Die_List = tsk_Die.Items;
            }

            #region 准备数组保存
            Dictionary<string, long> site_number_pass = new Dictionary<string, long>();
            Dictionary<string, long> site_number_total = new Dictionary<string, long>();
            Dictionary<string, long> bin_number = new Dictionary<string, long>();

            for (int i = 1; i <= 32; i++)
            {
                site_number_pass.Add("site" + i.ToString().PadLeft(2, '0'), 0);
                site_number_total.Add("site" + i.ToString().PadLeft(2, '0'), 0);
            }
            #endregion

            if (tsk_Die_List != null)
            {
                #region site 参数 生成


                foreach (DieData item_ic in tsk_Die_List)
                {
                    if (item_ic.Attribute == DieCategory.SkipDie || item_ic.Attribute == DieCategory.NoneDie)
                    {
                        continue;
                    }
                    if (item_ic.Bin == -1)
                    {
                        continue;
                    }
                    if (item_ic.Attribute == DieCategory.PassDie)
                    {
                        iPass++;
                    }


                    //X Y 坐标
                    int t_x = item_ic.X;
                    int t_y = item_ic.Y;

                    string cc = "";
                    if (iDevice.Split('-').Length == 4)
                    {

                        cc = iDevice.Split('-')[1];
                    }
                    else
                    {
                        cc = "1";
                    }

                    switch (cc)
                    {
                        case "1":
                            SiteHelper.C1(t_x, t_y, item_ic, ref site_number_pass, ref site_number_total);
                            break;
                        case "2":
                            SiteHelper.C2(t_x, t_y, item_ic, ref site_number_pass, ref site_number_total);
                            break;
                        case "4":
                            SiteHelper.C4(t_x, t_y, item_ic, ref site_number_pass, ref site_number_total);
                            break;
                        case "8":
                            SiteHelper.C8(t_x, t_y, item_ic, ref site_number_pass, ref site_number_total);
                            break;
                        case "16":
                            SiteHelper.C16(t_x, t_y, item_ic, ref site_number_pass, ref site_number_total);
                            break;
                        default:
                            SiteHelper.C8(t_x, t_y, item_ic, ref site_number_pass, ref site_number_total);
                            break;

                    }
                    iTotal++;
                }

                #endregion

                #region Bin汇总信息 生成

                foreach (DieData item_ic in tsk_Die_List)
                {

                    int _bin = ((DieData)item_ic).Bin;
                    if (_bin == -1)
                    {
                        continue;
                    }
                    //Bin明细
                    if (bin_number.ContainsKey("BIN " + _bin.ToString()))
                    {
                        bin_number["BIN " + _bin.ToString()]++;
                    }
                    else
                    {
                        bin_number.Add("BIN " + _bin.ToString(), 1);
                    }
                }
                #endregion
            }

            #region 主数据写入




            long iFail = iTotal - iPass;

            string iYield;

            if (iTotal == 0)
            {
                iYield = "0.0";
            }
            else
            {
                iYield = Math.Round(double.Parse(iPass.ToString()) / double.Parse(iTotal.ToString()), 4).ToString();
            }


            //  去除 -1  的数据
            if (iWaferID == "-1")
            {
                return;
            }
            //去除数据短一些的信息或错误的信息
            if (iLotNo.Length <= 3)
            {
                return;
            }

            string lot_name = "";
            string lot_process = "";
            string lot_no = "";


            if (iLotNo.Substring(iLotNo.Length - 3).ToUpper() != "CP1" && iLotNo.Substring(iLotNo.Length - 3).ToUpper() != "CP2" && iLotNo.Substring(iLotNo.Length - 3).ToUpper() != "CP3")
            {
                lot_process = "CP1";
                lot_name = iLotNo;
            }
            else
            {
                lot_name = iLotNo.Substring(0, iLotNo.Length - 3);
                lot_process = iLotNo.Substring(iLotNo.Length - 3).ToUpper();
            }

            if (iWaferID.Contains("-"))
            {
                lot_no = iWaferID.Substring(iWaferID.LastIndexOf('-') + 1).PadLeft(2, '0');
            }
            else
            {
                lot_no = "00";
            }



            string sql_info = string.Format(@"[dbo].[HP_TSK_COLLECT_INFO_INSERT1017_01] '{0}','{1}','{2}','{3}','{4}','{5}'
                    ,'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}'",

               iLotNo, iWaferID, iDevice, iTotal, iPass, iFail,
               iYield, iIndexX, iIndexY, iWaferSize, iOF_Direction,
               iLoadTime, iUnloadTime, iUsedTime, istartTime, iendTime,
               FileName, FilePath, DirPath, EqName,
              lot_name, lot_process, lot_no);



            DataTable dt = exdb.Get_Data(sql_info);

            DtbToUi.DtbAddToDGV(dt, this.pwtDataGridView3);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("未上传成功"); return;
            }



            if (dt.Rows.Count != 0)
            {

                string lot_id = dt.Rows[0][0].ToString();

                #region Bin信息

                string sql_bin = @"
                      INSERT INTO [dbo].[hp_tsk_bin_collect_info]
                      (
                      dbo.hp_tsk_bin_collect_info.wf_id,
                      dbo.hp_tsk_bin_collect_info.binName,
                      dbo.hp_tsk_bin_collect_info.binNumber
                      )
                      VALUES   ('{0}','{1}','{2}')                
                      ";
                foreach (var item in bin_number)
                {
                    exdb.Exe_Data(string.Format(sql_bin, lot_id, item.Key, item.Value.ToString()));
                }

                #endregion



                #region SITE信息

                string site_bin = @"
                    INSERT INTO [dbo].[hp_tsk_site_collect_info]
                        (
                        dbo.hp_tsk_site_collect_info.wf_id,
                        dbo.hp_tsk_site_collect_info.site_name,
                        dbo.hp_tsk_site_collect_info.pass_number,
                        dbo.hp_tsk_site_collect_info.total_number
                        )
                      VALUES   ('{0}','{1}','{2}','{3}')                
                      ";


                for (int i = 1; i <= 32; i++)
                {
                    if (site_number_total["site" + i.ToString().PadLeft(2, '0')] == 0)
                    {
                        continue;
                    }

                    string str_sql = string.Format(site_bin, lot_id, "site" + i.ToString().PadLeft(2, '0'), site_number_pass["site" + i.ToString().PadLeft(2, '0')], site_number_total["site" + i.ToString().PadLeft(2, '0')]);
                    exdb.Exe_Data(str_sql);
                }

                #endregion



                for (int i = 0; i < this.pwtDataGridView4.Rows.Count; i++)
                {

                    if (this.pwtDataGridView4.Rows[i].Cells[3].Value.ToString() == load_FilePath)
                    {
                        this.pwtDataGridView4.Rows[i].Cells[2].Value = "上传成功";
                    }

                }
            }
            else
            {
                 for (int i = 0; i < this.pwtDataGridView4.Rows.Count; i++)
                {
                    if (this.pwtDataGridView4.Rows[i].Cells[3].Value.ToString() == load_FilePath)
                    {
                        this.pwtDataGridView4.Rows[i].Cells[2].Value = "上传失败";
                    }
                
                }
            }
           
            #endregion





        }

        private void 上传数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pwtDataGridView4.SelectedRows.Count==0)
            {
                MessageBox.Show("请选择上传文件");
                return;
            }
            string file_state = this.pwtDataGridView4.SelectedRows[0].Cells[2].Value.ToString();


            if (file_state=="上传成功")
            {
                if (MessageBox.Show("确定上传已经成功的文件","系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)!=  System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }

            string filename = this.pwtDataGridView4.SelectedRows[0].Cells[3].Value.ToString();
            CollectTSK(this.pwtDataGridView4.SelectedRows[0].Cells[1].Value.ToString());
            MessageBox.Show("上传成功");
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        public bool select_state = false;

        public int up_number = 0;
        private void buttonX4_Click(object sender, EventArgs e)
        {
            select_state = true;
            up_number = this.pwtDataGridView3.Rows.Count;
        }
    }
}
