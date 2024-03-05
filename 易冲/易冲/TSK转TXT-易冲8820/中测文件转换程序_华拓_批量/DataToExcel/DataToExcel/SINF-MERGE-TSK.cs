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

namespace DataToExcel
{
    public partial class SINF_MERGE_TSK : Form
    {
        public SINF_MERGE_TSK()
        {
            InitializeComponent();
        }

        ArrayList sinf_Name = new ArrayList();
        ArrayList tsk_Name = new ArrayList();
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
                    tsk_Name.Add(str.Name);

                }
            }

            if (tsk_Name.Count != sinf_Name.Count)
            {
                MessageBox.Show("图谱数量不对应");
            
            }  

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadSINFFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void LoadSINFFile()
        {
            this.LoadSINF();
        }

        private void LoadSINF()
        {
            /*
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in dialog.FileNames)
                {
                    this.textBox2.Text = str;

                }
            }
              */

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox2.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    sinf_Name.Add(str.Name);

                //    Tsk tsk = new Tsk(str.FullName);
                //    tsk.Read();
                //    this.LotNo = tsk.LotNo.Trim();
                //    ListViewItem item = new ListViewItem(tsk.WaferID);
                //    item.Tag = tsk;
                //    this.lsvItems.Items.Add(item);
                //    item.SubItems.Add(str.FullName);

                }
            }

        }

        ArrayList sinfData;
        //-----Sinf 头文件----//////
        string sinfDevice;
        string sinfLot;
        string sinfWafer;
        string sinfFnloc;
        int sinfRowct = 0;
        int sinfColct = 0;
        string sinfBcequ;
        int sinfRefpx;
        int sinfRefpy;
        string sinfDutms;
        decimal sinfXdies;
        decimal sinfYdies;
        //---------------///////



        private void button3_Click(object sender, EventArgs e)
        {

            if (this.textBox2.Text == "")
            {
                MessageBox.Show("请选择sinf图谱");
            }

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请选择TSK图谱");
            }

            string LotNo_1="";
            object[,] LotSum = new object[100, 100];

            for (int ii = 0; ii < tsk_Name.Count; ii++)
            {


                ///////-------------------------------TSK读取-------------------------//////
                FileStream fs_1;


                fs_1 = new FileStream(this.textBox1.Text + @"\" + tsk_Name[ii], FileMode.Open);
                BinaryReader br_1 = new BinaryReader(fs_1);

                ///TSK1头文件-------------------------------------------------------//

                //Operator Size 20
                string Operator_1 = Encoding.ASCII.GetString(br_1.ReadBytes(20)).Trim();
                //Device Size 16
                string Device_1 = Encoding.ASCII.GetString(br_1.ReadBytes(16)).Trim();
                //WaferSize Size 2
                byte[] WaferSize_1 = br_1.ReadBytes(2);
                this.Reverse(ref WaferSize_1);
                int TSKWafersize1 = BitConverter.ToInt16(WaferSize_1, 0);
                //MachineNo Size2
                byte[] MachineNo_1 = br_1.ReadBytes(2);
                //IndexSizeX Size4
                byte[] IndexSizeX_1 = br_1.ReadBytes(4);
                //IndexSizeY Size4
                byte[] IndexSizeY_1 = br_1.ReadBytes(4);
                //FlatDir Size2
                byte[] FlatDir_1 = br_1.ReadBytes(2);
                this.Reverse(ref FlatDir_1);
                int TSKFlat1 = BitConverter.ToInt16(FlatDir_1, 0);
                //MachineType Size1
                byte MachineType_1 = br_1.ReadByte();
                //MapVersion Size1
                byte MapVersion_1 = br_1.ReadByte();
                //row Size2
                byte[] row_1 = br_1.ReadBytes(2);
                //col Size2
                byte[] col_1 = br_1.ReadBytes(2);
                //MapDataForm Size4
                byte[] MapDataForm_1 = br_1.ReadBytes(4);
                //WaferID Size21
                string WaferID_1 = Encoding.ASCII.GetString(br_1.ReadBytes(21)).Trim();
                //ProbingNo Size1
                byte ProbingNo_1 = br_1.ReadByte();
                //LotNo Size18
                LotNo_1 = Encoding.ASCII.GetString(br_1.ReadBytes(18)).Trim();
                //CassetteNo Size2
                byte[] CN_1 = br_1.ReadBytes(2);
                this.Reverse(ref CN_1);
                int CassetteNo_1 = BitConverter.ToInt16(CN_1, 0);

                //SlotNo Size2
                byte[] SN_1 = br_1.ReadBytes(2);
                this.Reverse(ref SN_1);
                int SlotNo_1 = BitConverter.ToInt16(SN_1, 0);
                //X axis coordinates increase direction Size1
                byte IdeX_1 = br_1.ReadByte();
                //Y axis coordinates increase direction Size1
                byte IdeY_1 = br_1.ReadByte();
                //Reference die setting procedures Size1
                byte Rdsp_1 = br_1.ReadByte();
                //Reserved1 Size1
                byte Reserved1_1 = br_1.ReadByte();
                //Target die position X Size4
                byte[] Tdpx_1 = br_1.ReadBytes(4);
                //Target die position Y Size4
                byte[] Tdpy_1 = br_1.ReadBytes(4);
                //Reference die coordinator X Size2
                byte[] Rdcx_1 = br_1.ReadBytes(2);
                //Reference die coordinator Y
                byte[] Rdcy_1 = br_1.ReadBytes(2);
                // Probing start position Size1
                byte Psps_1 = br_1.ReadByte();
                //Probing direction Size1
                byte Pds_1 = br_1.ReadByte();
                //Reserved2 Size2
                byte[] Reserved2_1 = br_1.ReadBytes(2);
                //Distance X to wafer center die origin Szie4
                byte[] DistanceX_1 = br_1.ReadBytes(4);
                //Distance Y to wafer center die origin Size4
                byte[] DistanceY_1 = br_1.ReadBytes(4);
                //Coordinator X of wafer center die Size4
                byte[] CoordinatorX_1 = br_1.ReadBytes(4);
                //Coordinator Y of wafer center die Size4
                byte[] CoordinatorY_1 = br_1.ReadBytes(4);
                //First Die Coordinator X Size4
                byte[] FdcX_1 = br_1.ReadBytes(4);
                //First Die Coordinator Y Size4
                byte[] FdcY_1 = br_1.ReadBytes(4);
                //Wafer Testing Start Time Data Size12
                byte[] WTSTime_1 = br_1.ReadBytes(12);
                //Wafer Testing End Time Data Size12
                byte[] WTETime_1 = br_1.ReadBytes(12);
                //Wafer Loading Time Data Size 12
                byte[] WLTime_1 = br_1.ReadBytes(12);
                //Wafer Unloading Time Data Size12
                byte[] WULT_1 = br_1.ReadBytes(12);
                //Machine No1 Size4
                byte[] MachineNo1_1 = br_1.ReadBytes(4);
                //Machine No2 Size4
                byte[] MachineNo2_1 = br_1.ReadBytes(4);

                // Special Characters Size4
                byte[] SpecialChar_1 = br_1.ReadBytes(4);
                //Testing End Information Size1
                byte TestEndInfo_1 = br_1.ReadByte();
                //Reserved3 Size1
                byte Reserved3_1 = br_1.ReadByte();
                //Total tested dice Size2
                byte[] Totaldice_1 = br_1.ReadBytes(2);
                //Total pass dice Size2
                byte[] TotalPdice_1 = br_1.ReadBytes(2);
                //Total fail dice Size2
                byte[] TotalFdice_1 = br_1.ReadBytes(2);
                //Test Die Information Address Size4
                byte[] TDIAdress_1 = br_1.ReadBytes(4);
                //Number of line category data Size4
                byte[] NumberCategory_1 = br_1.ReadBytes(4);
                //Line category address Size4
                byte[] LineCategory_1 = br_1.ReadBytes(4);
                // Map File Configuration Size2
                byte[] MapConfig_1 = br_1.ReadBytes(2);
                // Max. Multi Site Size2
                byte[] MMSite_1 = br_1.ReadBytes(2);
                //Max. Categories Size2
                byte[] MCategory_1 = br_1.ReadBytes(2);
                //Do not use,Reserved4 Size2
                byte[] Reserved4_1 = br_1.ReadBytes(2);
                ////////Die 信息/////////////////////

                int row1_1 = ByteToInt16(ref row_1);
                int col1_1 = ByteToInt16(ref col_1);
                if (row1_1 == 0 && col1_1 == 0)
                {
                    // MessageBox.Show("TSK图谱不正确!");
                    if (MessageBox.Show("TSK图谱不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }

                ArrayList arryfirstbyte1_1 = new ArrayList();
                ArrayList arryfirstbyte2_1 = new ArrayList();
                ArrayList arrysecondbyte1_1 = new ArrayList();
                ArrayList arrysecondbyte2_1 = new ArrayList();
                ArrayList arrythirdbyte1_1 = new ArrayList();
                ArrayList arrythirdbyte2_1 = new ArrayList();

                for (int k = 0; k < row1_1 * col1_1; k++)
                {
                    arryfirstbyte1_1.Add(br_1.ReadByte());
                    arryfirstbyte2_1.Add(br_1.ReadByte());
                    arrysecondbyte1_1.Add(br_1.ReadByte());
                    arrysecondbyte2_1.Add(br_1.ReadByte());
                    arrythirdbyte1_1.Add(br_1.ReadByte());
                    arrythirdbyte2_1.Add(br_1.ReadByte());

                }


                ArrayList arry_1 = new ArrayList();


                while (br_1.BaseStream.Position < br_1.BaseStream.Length)
                {
                    arry_1.Add(br_1.ReadByte());
                }

                br_1.Close();
                fs_1.Close();

                byte[] firstbyte1_1 = (byte[])arryfirstbyte1_1.ToArray(typeof(byte));
                byte[] firstbyte2_1 = (byte[])arryfirstbyte2_1.ToArray(typeof(byte));

                byte[] secondbyte1_1 = (byte[])arrysecondbyte1_1.ToArray(typeof(byte));
                byte[] secondbyte2_1 = (byte[])arrysecondbyte2_1.ToArray(typeof(byte));

                byte[] thirdbyte1_1 = (byte[])arrythirdbyte1_1.ToArray(typeof(byte));
                byte[] thirdbyte2_1 = (byte[])arrythirdbyte2_1.ToArray(typeof(byte));

                object[,] TSKMap = new object[col1_1, row1_1];

                for (int i = 0; i < col1_1; i++)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        if ((secondbyte1_1[j + i * row1_1] & 192) == 0)//Skip Die
                        {
                            // TSKMap[i, j] = ".";
                            TSKMap[i, j] = " ";

                        }

                        if ((secondbyte1_1[j + i * row1_1] & 192) == 128)//Mark Die
                        {
                            // TSKMap[i, j] = ".";
                            TSKMap[i, j] = " ";

                        }

                        if ((secondbyte1_1[j + i * row1_1] & 192) == 64)//Probe Die
                        {

                            // if ((firstbyte1_1[j + i * row1_1] & 64) == 64)//PASS
                            // {
                            // TSKMap[i, j] = "P";
                            TSKMap[i, j] = (thirdbyte2_1[j + i * row1_1] & 0xff).ToString("00");
                            // }

                            //if ((firstbyte1_1[j + i * row1_1] & 128) == 128)//FAIL
                            //{
                            //TSKMap[i, j] = "F";

                            //}

                        }

                    }
                }
                //------------------------------TSK1模板Read 结束------------------------------//

                //////////SINF-READ//////////////////////////////
                string[] strs = tsk_Name[ii].ToString().Split(new char[] { '-' });

                string sinfName11 = LotNo_1 + "-" + strs[1].Substring(0,strs[1].Length-2);

                FileStream sinf_1;
                sinf_1 = new FileStream(this.textBox2.Text+ @"\" + sinfName11+".txt", FileMode.Open);
                StreamReader read = new StreamReader(sinf_1, Encoding.Default);


                if (this.sinfData == null)
                {
                    this.sinfData = new ArrayList();
                }
                else
                {
                    this.sinfData.Clear();
                }
                while (true)
                {
                    string line = read.ReadLine();
                    if (line == null)
                        break;
                    this.Parse(line);

                }


                object[,] SinfMap = new object[this.sinfRowct, this.sinfColct];
                if (sinfRowct > 0 && sinfColct > 0)
                {

                    for (int i = 0; i < this.sinfRowct; i++)
                    {
                        for (int j = 0; j < this.sinfColct; j++)
                        {

                            SinfMap[i, j] = sinfData[j + i * sinfColct];

                        }
                    }



                }

                else
                {
                    // MessageBox.Show("SINF格式不正确!");
                    if (MessageBox.Show("SINF格式不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }

                }
                sinf_1.Close();
                read.Close();

                /////////////////////////读取SINF 结束////////////////////////////////////

               


                //------------------------------TSK MAP去边----------------------------//

                int tskrowmin = 0, tskcolmin = 0, tskrowmax = 0, tskcolmax = 0;
                int flag = 0;
                for (int i = 0; i < col1_1; i++)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        // if ((TSKMap[i, j].ToString() == "P") || (TSKMap[i, j].ToString() == "F"))
                        if ((TSKMap[i, j].ToString() != " "))
                        {
                            tskrowmin = i;
                            flag = 1;
                            break;

                        }
                    }
                    if (flag == 1)
                    {
                        break;
                    }
                }

                flag = 0;
                for (int i = col1_1 - 1; i >= 0; i--)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        // if ((TSKMap[i, j].ToString() == "P") || (TSKMap[i, j].ToString() == "F"))
                        if ((TSKMap[i, j].ToString() != " "))
                        {
                            tskrowmax = i;
                            flag = 1;
                            break;

                        }

                    }
                    if (flag == 1)
                    {
                        break;

                    }
                }

                flag = 0;
                for (int i = 0; i < row1_1; i++)
                {
                    for (int j = 0; j < col1_1; j++)
                    {
                        // if ((TSKMap[j, i].ToString() == "P") || (TSKMap[j, i].ToString() == "F"))
                        if ((TSKMap[j, i].ToString() != " "))
                        {
                            tskcolmin = i;
                            flag = 1;

                        }

                    }
                    if (flag == 1)
                    {
                        break;
                    }
                }

                flag = 0;
                for (int i = row1_1 - 1; i >= 0; i--)
                {
                    for (int j = 0; j < col1_1; j++)
                    {
                        // if ((TSKMap[j, i].ToString() == "P") || (TSKMap[j, i].ToString() == "F"))
                        if ((TSKMap[j, i].ToString() != " "))
                        {
                            tskcolmax = i;
                            flag = 1;

                        }

                    }
                    if (flag == 1)
                    {
                        break;
                    }
                }

                if ((tskcolmax - tskcolmin + 1) != sinfColct)
                {
                    //MessageBox.Show("SINF与TSK列数不匹配");
                    if (MessageBox.Show("SINF与TSK列数不匹配", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }

                if ((tskrowmax - tskrowmin + 1) != sinfRowct)
                {
                    //MessageBox.Show("SINF与TSK行数不匹配");
                    if (MessageBox.Show("SINF与TSK行数不匹配", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }

                object[,] TSKMapNew = new object[sinfRowct, sinfColct];

                for (int i = 0; i < sinfRowct; i++)
                {
                    for (int j = 0; j < sinfColct; j++)
                    {
                        TSKMapNew[i, j] = TSKMap[i + tskrowmin, j + tskcolmin];

                    }

                }

                //////////////////////------TSK转角度-----------------------/////////////////////////////////




                /////////////////-------------------SINF合并到TSK---------------------////////

                int mergepass = 0, mergefail = 0;
                for (int i = 0; i < sinfRowct; i++)
                {
                    for (int j = 0; j < sinfColct; j++)
                    {

                        // if (SinfMap[i, j].ToString() == "F" && TSKMapNew[i, j].ToString() != ".")
                        if (SinfMap[i, j].ToString() == "99" && TSKMapNew[i, j].ToString() != " ")
                        {
                            //    TSKMapNew[i, j] = "F";
                            TSKMapNew[i, j] = "99";

                        }

                        // if (SinfMap[i, j].ToString() == "." && TSKMapNew[i, j].ToString() != ".")
                        if (SinfMap[i, j].ToString() == " " && TSKMapNew[i, j].ToString() != " ")
                        {
                            if (MessageBox.Show("对位点形状不一致是否关闭程序", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Application.Exit();
                            }
                        }


                        if ((TSKMapNew[i, j].ToString() != "01") && (TSKMapNew[i, j].ToString() != " "))
                        {
                            mergefail++;

                        }

                        if (TSKMapNew[i, j].ToString() == "01")
                        {
                            mergepass++;

                        }


                    }

                }

                ////////////////////////////////输出TMA//////////////////////////////////
                if (!Directory.Exists("D:\\MERGE\\" + LotNo_1 + "\\"))
                {
                    Directory.CreateDirectory("D:\\MERGE\\" + LotNo_1 + "\\");
                }
                FileStream fw;
                fw = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\" + SlotNo_1.ToString("000") + WaferID_1.TrimEnd('\0') + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fw);
                sw.Write("   ");



                for (int i = 1; i <= sinfRowct; i++)
                {
                    if (i <= 99)
                        sw.Write(string.Format("{0,3:G}", i.ToString("00")));
                    else if (i > 99 && i <= 999)
                        sw.Write(string.Format("{0,4:G}", i.ToString("000")));
                    else if (i > 199 && i <= 9999)
                        sw.Write(string.Format("{0,5:G}", i.ToString("0000")));
                }
                string formatStr1 = "", formatStr2 = "", empty = "";
                formatStr1 = "{0,3:G}";
                formatStr2 = "000";
                empty = "   ";
                sw.WriteLine();
                sw.Write(empty + "+");
                for (int i = 0; i < sinfRowct; i++)
                {
                    sw.Write("+-+");
                }

                for (int x = 0; x < sinfRowct; x++)
                {
                    sw.WriteLine();
                    sw.Write(string.Format(formatStr1, (x + 1).ToString(formatStr2)));
                    sw.Write("|");

                    for (int y = 0; y < sinfColct; y++)
                    {
                        sw.Write(string.Format("{0,3:G}", TSKMapNew[x, y]));
                    }



                }
                sw.WriteLine();
                sw.WriteLine("[BOF]");
                sw.WriteLine("PRODUCT ID      :" + Device_1);
                sw.WriteLine("LOT ID          :" + LotNo_1);
                sw.WriteLine("WAFER ID        :" + WaferID_1);
                sw.WriteLine("FLOW ID         :  CP2");
                // sw.WriteLine("START TIME      : 2020/07/06 00:26:00");
                // sw.WriteLine("STOP  TIME      : 2020/07/06 00:30:00");
                // sw.WriteLine("SUBCON          :");
                // sw.WriteLine("TESTER NAME     :");
                // sw.WriteLine("TEST PROGRAM    :");
                // sw.WriteLine("LOAD BOARD ID   :");
                // sw.WriteLine("PROBE CARD ID   :");
                //  sw.WriteLine("SITE NUM        :");
                // sw.WriteLine("DUT DIFF NUM    :");
                //  sw.WriteLine("OPERATOR ID     :");
                sw.WriteLine("TESTED DIE      :" + (mergefail + mergepass).ToString());
                sw.WriteLine("PASS DIE        :" + mergepass.ToString());
                sw.WriteLine("TYIELD          :" + Math.Round((double)(Convert.ToDouble(mergepass) / ((double)Convert.ToInt32(mergepass + mergefail))), 4).ToString("0.00%"));
                sw.WriteLine("SOURCE NOTCH    :" + "DOWN");
                //  sw.WriteLine("MAP COLUMN      :");
                //  sw.WriteLine("MAP ROW         :");
                //  sw.WriteLine("MAP BIN LENGTH  :");
                //  sw.WriteLine("SHIP            :");
                //  sw.WriteLine("XSIZE           :");
                //  sw.WriteLine("YSIZE           :");
                //  sw.WriteLine("CODE1           :");
                //   sw.WriteLine("CODE2           :");
                //   sw.WriteLine("CODE3           :");
                //   sw.WriteLine("CODE4           :");
                sw.WriteLine("[EOF]");
                sw.WriteLine();
                /*
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("============ Wafer Information () ===========");
                sw.WriteLine("  Device: " + Device_1);
                sw.WriteLine("  Lot NO: " + LotNo_1);
                sw.WriteLine("  Slot NO: " + SlotNo_1);
                sw.WriteLine("  Wafer ID: " + WaferID_1);
                sw.WriteLine("  Operater: " + Operator_1);
                sw.WriteLine("  Wafer Size: " + (TSKWafersize1 / 10).ToString());
                sw.WriteLine("  Flat Dir: " + TSKFlat1);

                sw.WriteLine("  Wafer Test Start Time: " + "2020/7/1 10:50:00 ");
                sw.WriteLine("  Wafer Test Finish Time: " + "2020/7/1 10:50:00 ");
                sw.WriteLine("  Wafer Load Time: " + "2020/7/1 10:50:00 ");
                sw.WriteLine("  Wafer Unload Time: " + "2020/7//1 10:50:00 ");

                sw.WriteLine("  Total test die: " + (mergefail + mergepass).ToString());
                sw.WriteLine("  Pass Die: " + mergepass.ToString());
                sw.WriteLine("  Fail Die: " + mergefail.ToString());
                sw.WriteLine("  Yield: " + Math.Round((double)(Convert.ToDouble(mergepass) / ((double)Convert.ToInt32(mergepass + mergefail))), 4).ToString("0.00%"));
                sw.WriteLine("  Rows:" + sinfRowct);
                sw.WriteLine("  Cols:" + sinfColct);
                sw.WriteLine();
                */

                sw.Close();
                fw.Close();

               
                LotSum[ii, 0] = LotNo_1;
                LotSum[ii, 1] = SlotNo_1.ToString("00");
                LotSum[ii, 2] = WaferID_1;
                LotSum[ii, 3] = mergepass + mergefail;
                LotSum[ii, 4] = mergepass ;
                LotSum[ii, 5] = mergefail;
                LotSum[ii, 6] = Math.Round((double)(Convert.ToDouble(mergepass) / ((double)Convert.ToInt32(mergepass + mergefail))), 4).ToString("0.00%");


            }

            FileStream fwt;
            fwt = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\" + LotNo_1 + "_Summary" + ".txt", FileMode.Create);
            StreamWriter swt = new StreamWriter(fwt);
            swt.WriteLine("LotNo     Slot   Wafer ID                    GrossDie  PassDie   FailDie   Yield");
            swt.WriteLine("---------------------------------------------------------------------------------");
            int alldie = 0, allpass = 0, allfail = 0;
            for (int ii = 0; ii < tsk_Name.Count; ii++)
            {
                swt.WriteLine(LotSum[ii, 0].ToString() + "     " + LotSum[ii, 1].ToString() + "   " + LotSum[ii, 2].ToString() + "                    " + LotSum[ii, 3].ToString() + "  " + LotSum[ii, 4].ToString() + "   " + LotSum[ii, 5].ToString() + "   " + LotSum[ii, 6].ToString());
                alldie += Convert.ToInt32(LotSum[ii, 3]);
                allpass += Convert.ToInt32(LotSum[ii, 4]);
                allfail += Convert.ToInt32(LotSum[ii, 5]);
            }
            swt.WriteLine("---------------------------------------------------------------------------------");
            swt.WriteLine("Total                                        " + alldie + "  " + allpass + "   " + allfail + "   " + Math.Round((double)(Convert.ToDouble(allpass) / ((double)Convert.ToInt32(allpass + allfail))), 4).ToString("0.00%"));


            swt.Close();
            fwt.Close();

          

            if (MessageBox.Show("合并成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                Process.Start("D:\\MERGE\\");
            }


        }


        private void Parse(string line)
        {
            try
            {
                string[] strs = line.Split(new char[] { ':' });

                switch (strs[0].ToUpper())
                {
                    case "DEVICE":
                        this.sinfDevice = strs[1].Trim();
                        break;
                    case "LOT":
                        this.sinfLot = strs[1].Trim();
                        break;
                    case "WAFER":
                        this.sinfWafer = strs[1].Trim();
                        break;
                    case "FNLOC":
                        this.sinfFnloc = strs[1].Trim();
                        break;
                    case "ROWCT":
                        this.sinfRowct = Int32.Parse(strs[1].Trim());
                        break;
                    case "COLCT":
                        this.sinfColct = Int32.Parse(strs[1].Trim());
                        break;
                    case "BCEQU":
                        this.sinfBcequ = strs[1].Trim();
                        break;
                    case "REFPX":
                        this.sinfRefpx = Int32.Parse(strs[1].Trim());
                        break;
                    case "REFPY":
                        this.sinfRefpy = Int32.Parse(strs[1].Trim());
                        break;
                    case "DUTMS":
                        this.sinfDutms = strs[1].Trim();
                        break;
                    case "XDIES":
                        this.sinfXdies = Decimal.Parse(strs[1].Trim());
                        break;
                    case "YDIES":
                        this.sinfYdies = Decimal.Parse(strs[1].Trim());
                        break;
                    case "ROWDATA":
                        this.ParseDies(strs[1]);
                        break;
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        private void ParseDies(string s)
        {
            string[] dies = s.Split(new char[] { ' ' });

            foreach (string d in dies)
            {
                if (d == "___")
                {
                    //sinfData.Add(".");
                    sinfData.Add(" ");
                }
                else if (d == "000")
                {
                   // sinfData.Add("P");
                    sinfData.Add("01");
                }
                else
                {
                   // sinfData.Add("F");
                    sinfData.Add("99");
                }

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


    }
} 



