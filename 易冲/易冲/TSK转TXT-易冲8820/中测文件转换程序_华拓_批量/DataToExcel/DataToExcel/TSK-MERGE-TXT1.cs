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
    public partial class TSK_MERGE_TXT1 : Form
    {
        public TSK_MERGE_TXT1()
        {
            InitializeComponent();
        }

        ArrayList txt_Name = new ArrayList();
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

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadTXTFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void LoadTXTFile()
        {
            this.LoadTXT();
        }

        private void LoadTXT()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox2.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    txt_Name.Add(str.Name);

                }
            }

            if (tsk_Name.Count != txt_Name.Count)
            {
                MessageBox.Show("图谱数量不对应");

            }

        }

        ArrayList txtData;
        //-----Sinf 头文件----//////
        string txtDevice;
        string txtLot;
        string txtWafer;
        string txtFnloc;
        int txtTotal = 0;
        int txtPass = 0;
        int txtFail = 0;
        string txtYield;
        int txtRowct = 0;
        int txtColct = 0;
        string txtBcequ;
        int txtRefpx=0;
        int txtRefpy=0;
        string txtDutms;
        decimal txtXdies;
        decimal txtYdies;
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

            string LotNo_1 = "";
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

                string txtName11 = LotNo_1 + "-" + SlotNo_1.ToString("00");

                FileStream txt_1;
                txt_1 = new FileStream(this.textBox2.Text + @"\" + txtName11 + ".txt", FileMode.Open);
                StreamReader read = new StreamReader(txt_1, Encoding.Default);


                if (this.txtData == null)
                {
                    this.txtData = new ArrayList();
                }
                else
                {
                    this.txtData.Clear();
                }
                while (true)
                {
                    string line = read.ReadLine();
                    if (line == null)
                        break;
                    this.Parse(line);

                }


                object[,] txtMap = new object[this.txtRowct, this.txtColct];
                if (txtRowct > 0 && txtColct > 0)
                {

                    for (int i = 0; i < this.txtRowct; i++)
                    {
                        for (int j = 0; j < this.txtColct; j++)
                        {

                            txtMap[i, j] = txtData[j + i * txtColct];

                        }
                    }



                }

                else
                {
                    // MessageBox.Show("SINF格式不正确!");
                    if (MessageBox.Show("TXT格式不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }

                }
                txt_1.Close();
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

                if ((tskcolmax - tskcolmin + 1) != txtColct)
                {
                    //MessageBox.Show("SINF与TSK列数不匹配");
                    if (MessageBox.Show(this.txtWafer+"TXT与TSK列数不匹配", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }

                if ((tskrowmax - tskrowmin + 1) != txtRowct)
                {
                    //MessageBox.Show("SINF与TSK行数不匹配");
                    if (MessageBox.Show(this.txtWafer+"TXT与TSK行数不匹配", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }

                object[,] TSKMapNew = new object[txtRowct, txtColct];

                for (int i = 0; i < txtRowct; i++)
                {
                    for (int j = 0; j < txtColct; j++)
                    {
                        TSKMapNew[i, j] = TSKMap[i + tskrowmin, j + tskcolmin];

                    }

                }

                //////////////////////------TSK转角度-----------------------/////////////////////////////////

                /////////////////-------------------SINF合并到TSK---------------------////////

                int mergepass = 0, mergefail = 0;
                for (int i = 0; i < txtRowct; i++)
                {
                    for (int j = 0; j < txtColct; j++)
                    {

                       
                        if ((txtMap[i, j].ToString() != ".") && ((txtMap[i, j].ToString() != "0")) && TSKMapNew[i, j].ToString() != " ")
                        {
                          TSKMapNew[i, j] = txtMap[i, j];

                        }

                      
                        if (txtMap[i, j].ToString() == "." && TSKMapNew[i, j].ToString() != " ")
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
                fw = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\"+ this.txtWafer + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fw);
                sw.WriteLine("DEVICE:" + this.txtDevice);
                sw.WriteLine("Lot NO:" + this.txtLot);
                sw.WriteLine("Wafer ID:" + this.txtWafer);
                sw.WriteLine("Notch Dir:" + this.txtFnloc);
                sw.WriteLine("Total inspection die:" + (mergefail + mergepass).ToString());
                sw.WriteLine("Pass Die:" + mergepass.ToString());
                sw.WriteLine("Fail Die:" + mergefail.ToString());
                sw.WriteLine("Yield:" + Math.Round((double)(Convert.ToDouble(mergepass) / ((double)Convert.ToInt32(mergepass + mergefail))), 4).ToString("0.00%"));
                sw.WriteLine("ROWCT:" + this.txtRowct);
                sw.WriteLine("COLCT:" + this.txtColct);
                sw.WriteLine("BCEQU:" + this.txtBcequ);
                sw.WriteLine("REFPX:" + this.txtRefpx);
                sw.WriteLine("REFPY:" + this.txtRefpy);
                sw.WriteLine("DUTMS:" + this.txtDutms);
                sw.WriteLine("XDIES:" + this.txtXdies);
                sw.WriteLine("YDIES:" + this.txtYdies);

                for (int x = 0; x < txtRowct; x++)
                {
                    for (int y = 0; y < txtColct; y++)
                    {
                        switch (TSKMapNew[x,y].ToString())
                        {
                            case " ":
                            sw.Write(string.Format("{0,1:G}", "."));
                            break;
                            case "01":
                            sw.Write(string.Format("{0,1:G}", "0"));
                            break;
                            case "02":
                            sw.Write(string.Format("{0,1:G}", "1"));
                            break;
                            case "03":
                            sw.Write(string.Format("{0,1:G}", "2"));
                            break;
                            case "04":
                            sw.Write(string.Format("{0,1:G}", "3"));
                            break;
                            case "05":
                            sw.Write(string.Format("{0,1:G}", "4"));
                            break;
                            case "06":
                            sw.Write(string.Format("{0,1:G}", "5"));
                            break;
                            case "07":
                            sw.Write(string.Format("{0,1:G}", "6"));
                            break;
                            case "08":
                            sw.Write(string.Format("{0,1:G}", "7"));
                            break;
                            case "09":
                            sw.Write(string.Format("{0,1:G}", "8"));
                            break;
                            case "10":
                            sw.Write(string.Format("{0,1:G}", "9"));
                            break;
                            case "11":
                            sw.Write(string.Format("{0,1:G}", "A"));
                            break;
                            case "12":
                            sw.Write(string.Format("{0,1:G}", "B"));
                            break;
                            case "13":
                            sw.Write(string.Format("{0,1:G}", "C"));
                            break;
                            case "14":
                            sw.Write(string.Format("{0,1:G}", "D"));
                            break;
                            case "15":
                            sw.Write(string.Format("{0,1:G}", "E"));
                            break;
                            case "16":
                            sw.Write(string.Format("{0,1:G}", "F"));
                            break;
                            case "17":
                            sw.Write(string.Format("{0,1:G}", "G"));
                            break;
                            case "18":
                            sw.Write(string.Format("{0,1:G}", "H"));
                            break;
                            case "19":
                            sw.Write(string.Format("{0,1:G}", "I"));
                            break;
                            case "20":
                            sw.Write(string.Format("{0,1:G}", "J"));
                            break;
                            case "21":
                            sw.Write(string.Format("{0,1:G}", "K"));
                            break;
                            case "22":
                            sw.Write(string.Format("{0,1:G}", "L"));
                            break;
                            case "23":
                            sw.Write(string.Format("{0,1:G}", "M"));
                            break;
                            case "24":
                            sw.Write(string.Format("{0,1:G}", "N"));
                            break;
                            case "25":
                            sw.Write(string.Format("{0,1:G}", "O"));
                            break;
                            case "26":
                            sw.Write(string.Format("{0,1:G}", "P"));
                            break;
                            case "27":
                            sw.Write(string.Format("{0,1:G}", "Q"));
                            break;
                            case "28":
                            sw.Write(string.Format("{0,1:G}", "R"));
                            break;
                            case "29":
                            sw.Write(string.Format("{0,1:G}", "S"));
                            break;
                            case "30":
                            sw.Write(string.Format("{0,1:G}", "T"));
                            break;
                            case "31":
                            sw.Write(string.Format("{0,1:G}", "U"));
                            break;
                            case "32":
                            sw.Write(string.Format("{0,1:G}", "V"));
                            break;
                            case "33":
                            sw.Write(string.Format("{0,1:G}", "W"));
                            break;
                            case "34":
                            sw.Write(string.Format("{0,1:G}", "X"));
                            break;
                            case "35":
                            sw.Write(string.Format("{0,1:G}", "Y"));
                            break;
                            case "36":
                            sw.Write(string.Format("{0,1:G}", "Z"));
                            break;
                            default:
                            sw.Write(string.Format("{0,1:G}", TSKMapNew[x, y].ToString()));
                            break;

                        }

                    }
                    sw.WriteLine();


                }

                sw.Close();
                fw.Close();







            }



            if (MessageBox.Show("合并成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                Process.Start("D:\\MERGE\\");
            }


        }



        private void Parse(string line)
        {
            try
            {

                if (line.IndexOf(':') >= 0)
                {
                    string[] strs = line.Split(new char[] { ':' });
                    switch (strs[0])
                    {
                        case "DEVICE":
                            this.txtDevice = strs[1].Trim();
                            break;
                        case "Lot NO":
                            this.txtLot = strs[1].Trim();
                            break;
                        case "Wafer ID":
                            this.txtWafer = strs[1].Trim();
                            break;
                        case "Notch Dir":
                            this.txtFnloc = strs[1].Trim();
                            break;
                        case "Total inspection die":
                            this.txtTotal = Int32.Parse(strs[1].Trim());
                            break;
                        case "Pass Die":
                            this.txtPass = Int32.Parse(strs[1].Trim());
                            break;
                        case "Fail Die":
                            this.txtPass = Int32.Parse(strs[1].Trim());
                            break;
                        case "Yield":
                            this.txtYield = strs[1].Trim();
                            break;
                        case "ROWCT":
                            this.txtRowct = Int32.Parse(strs[1].Trim());
                            break;
                        case "COLCT":
                            this.txtColct = Int32.Parse(strs[1].Trim());
                            break;
                        case "BCEQU":
                            this.txtBcequ = strs[1].Trim();
                            break;
                        case "REFPX":
                            this.txtRefpx = Int32.Parse(strs[1].Trim());
                            break;
                        case "REFPY":
                            this.txtRefpy = Int32.Parse(strs[1].Trim());
                            break;
                        case "DUTMS":
                            this.txtDutms = strs[1].Trim();
                            break;
                        case "XDIES":
                            this.txtXdies = Decimal.Parse(strs[1].Trim());
                            break;
                        case "YDIES":
                            this.txtYdies = Decimal.Parse(strs[1].Trim());
                            break;
                    }
                }
                else 
                
                {
                    this.ParseDies(line);
                
                }




            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        private void ParseDies(string s)
        {
          //  string[] dies = s.Split();

          //  foreach (string d in dies)
            for(int i=0;i<s.Length;i++)
            {

                txtData.Add(s[i].ToString());

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
