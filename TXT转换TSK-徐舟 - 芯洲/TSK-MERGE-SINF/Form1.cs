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
using System.Linq;

namespace TSK_MERGE_SINF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int txtTotal = 0;
        int txtPass = 0;
        int txtFail = 0;
        int tskPass = 0;
        int tskFail = 0;

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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in dialog.FileNames)
                {
                    this.textBox1.Text = str;

                }
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
        }

        List<string> txtData;
        List<string> txtNewData;
        //-----Sinf 头文件----//////
        string txtDevice;
        string txtLot;
        int txtSlot;
        string txtWaferID;
        string txtFlat;
        int txtRowct = 0;
        int txtColct = 0;

        int txtMark = 0;

        //---------------///////



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
            //////////TXT-READ//////////////////////////////
            FileStream txt_1;

            txt_1 = new FileStream(this.textBox2.Text, FileMode.Open);
            StreamReader read = new StreamReader(txt_1, Encoding.Default);


            if (this.txtData == null)
            {
                this.txtData = new List<string>();
            }
            else
            {
                this.txtData.Clear();
            }
            int lineflag = 0;
            while (true)
            {

                string line = read.ReadLine();
                if (line == null)
                {
                    lineflag++;
                    if (lineflag > 3)
                    {
                        break;
                    }
                }
                if (line != null)
                {
                    this.Parse(line);
                }

            }



            if (txtRowct > 0 && txtColct > 0)
            {

                //for (int i = 0; i < this.txtRowct; i++)
                //{
                //    for (int j = 0; j < this.txtColct; j++)
                //    {

                //        TxtMap[i, j] = txtData[j + i * txtColct];

                //    }
                //}

            }

            else
            {
                // MessageBox.Show("SINF格式不正确!");
                if (MessageBox.Show("TXT格式不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }

            }
            txt_1.Close();
            read.Close();

            /////////////////////////读取TXT 结束////////////////////////////////////

            ///////-------------------------------TSK读取-------------------------//////
            FileStream fs_1;


            fs_1 = new FileStream(this.textBox1.Text, FileMode.Open);
            BinaryReader br_1 = new BinaryReader(fs_1);

            ///TSK1头文件-------------------------------------------------------//

            //Operator Size 20
            string Operator_1 = Encoding.ASCII.GetString(br_1.ReadBytes(20)).Trim();
            //Device Size 16
            string Device_1 = Encoding.ASCII.GetString(br_1.ReadBytes(16)).Trim();
            //WaferSize Size 2
            byte[] WaferSize_1 = br_1.ReadBytes(2);
            //  this.Reverse(ref WaferSize_1);
            //  int TSKWafersize1 = BitConverter.ToInt16(WaferSize_1, 0);
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
            string LotNo_1 = Encoding.ASCII.GetString(br_1.ReadBytes(18)).Trim();
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
                    Environment.Exit(0);
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

            //for(int i=0;i<172;i++)
            //{
            //    bufferhead.Add(br_1.ReadByte());///正常TSK文件继续读取172页内容结束
            //}

            byte[] bufferhead1_20 = br_1.ReadBytes(20);
            byte[] bufferhead2_16 = br_1.ReadBytes(32);
            byte[] bufferhead_total = br_1.ReadBytes(4);
            byte[] bufferhead_pass = br_1.ReadBytes(4);
            byte[] bufferhead_fail = br_1.ReadBytes(4);
            byte[] bufferhead4_11 = br_1.ReadBytes(44);
            byte[] bufferhead1_64 = br_1.ReadBytes(64);


            while (br_1.BaseStream.Position < br_1.BaseStream.Length)
            {
                arry_1.Add(br_1.ReadByte());
            }

            br_1.Close();
            fs_1.Close();

            //------------------------------TSK1模板Read 结束------------------------------//

            //-------------------------------------TXT图谱转角度---------------------------//

            //ArrayList DegtxtData = new ArrayList();
            List<string> DegtxtData = new List<string>();
            int count = txtColct * txtRowct;

            for (int i = 0; i < count; i++)
            {
                DegtxtData.Add(".");
            }

            if (!String.IsNullOrEmpty(this.txtFlat))
            {
                if (this.txtFlat.Equals("DOWN"))
                {
                    this.txtFlat = "180";
                }
                int txtFlat1 = Convert.ToInt32(this.txtFlat);
                int flatDifference = Math.Abs(TSKFlat1 - txtFlat1);

                if (flatDifference == 180)////TXT转180
                {
                    int x = -1, y = -1, xr = -1, yr = -1;

                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            // 计算 x,y 坐标
                            // x = i % this._xmax;
                            x = i % txtColct;
                            // y = i / this._xmax;
                            y = i / txtColct;

                            xr = (txtColct) - 1 - x;
                            yr = (txtRowct) - 1 - y;

                            DegtxtData[yr * txtColct + xr] = txtData[i];
                        }
                        catch (Exception ee)
                        {
                            string msg = ee.Message;
                        }
                    }
                }

                else if (flatDifference == 90)////TXT转270
                {

                    int x = -1, y = -1, xr = -1, yr = -1;

                    for (int i = 0; i < count; i++)
                    {
                        // 计算 x,y 坐标
                        x = i % txtColct;
                        y = i / txtColct;

                        xr = y;
                        yr = (txtColct - 1) - x;

                        DegtxtData[yr * txtRowct + xr] = txtData[i];
                    }

                    // 交换行数与列数
                    x = txtColct;
                    txtColct = txtRowct;
                    txtRowct = x;

                }

                else if (flatDifference == 270)////TXT转90
                {

                    int x = -1, y = -1, xr = -1, yr = -1;
                    for (int i = 0; i < count; i++)
                    {
                        // 计算 x,y 坐标
                        x = i % txtColct;
                        y = i / txtColct;

                        xr = (txtRowct - 1) - y;
                        yr = x;

                        DegtxtData[yr * txtRowct + xr] = txtData[i];
                    }

                    // 交换行数与列数
                    x = txtColct;
                    txtColct = txtRowct;
                    txtRowct = x;

                }

                else if (flatDifference == 0)////TXT不转角度
                {

                    for (int i = 0; i < count; i++)
                    {

                        DegtxtData[i] = txtData[i];
                    }

                }
            }

            

            string[,] TxtMap = new string[this.txtRowct, this.txtColct];

            for (int i = 0; i < this.txtRowct; i++)
            {
                for (int j = 0; j < this.txtColct; j++)
                {

                    TxtMap[i, j] = DegtxtData[j + i * txtColct];

                }

            }

            //int temp = getTotalOfX(DegtxtData, "1", col1_1, row1_1);
           //int  temp = getTotalOfX(TxtMap, "1", col1_1, row1_1);

            ///////------------------------------TXT图谱补边工作---------------------------//
            byte[] firstbyte1_1 = (byte[])arryfirstbyte1_1.ToArray(typeof(byte));
            byte[] firstbyte2_1 = (byte[])arryfirstbyte2_1.ToArray(typeof(byte));

            byte[] secondbyte1_1 = (byte[])arrysecondbyte1_1.ToArray(typeof(byte));
            byte[] secondbyte2_1 = (byte[])arrysecondbyte2_1.ToArray(typeof(byte));

            byte[] thirdbyte1_1 = (byte[])arrythirdbyte1_1.ToArray(typeof(byte));
            byte[] thirdbyte2_1 = (byte[])arrythirdbyte2_1.ToArray(typeof(byte));
            string[,] TSKMap = new string[col1_1, row1_1];

            for (int i = 0; i < col1_1; i++)
            {
                for (int j = 0; j < row1_1; j++)
                {
                    if ((secondbyte1_1[j + i * row1_1] & 192) == 0)//Skip Die
                    {
                        TSKMap[i, j] = ".";
                    }

                    if ((secondbyte1_1[j + i * row1_1] & 192) == 128)//Mark Die
                    {
                        TSKMap[i, j] = ".";
                    }

                    if ((secondbyte1_1[j + i * row1_1] & 192) == 64)//Probe Die
                    {
                        TSKMap[i, j] = "1";
                    }

                }
            }


            int tskrowmin = 0, tskcolmin = 0, tskrowmax = 0, tskcolmax = 0;
            int flag = 0;
            for (int i = 0; i < col1_1; i++)
            {
                for (int j = 0; j < row1_1; j++)
                {
                    if ((TSKMap[i, j].ToString() != "."))
                    {
                        tskcolmin = i;
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
                    if ((TSKMap[i, j].ToString() != "."))
                    {
                        tskcolmax = i;
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
                    if ((TSKMap[j, i].ToString() != "."))
                    {
                        tskrowmin = i;
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
                    if ((TSKMap[j, i].ToString() != "."))
                    {
                        tskrowmax = i;
                        flag = 1;

                    }

                }
                if (flag == 1)
                {
                    break;
                }
            }

            string[,] TxtNewMap = new string[col1_1, row1_1];
            for (int i = 0; i < col1_1; i++)
            {
                for (int j = 0; j < row1_1; j++)
                {

                    TxtNewMap[i, j] = ".";
                }
            }

            for (int i = tskcolmin; i <= tskcolmax; i++)
            {
                for (int j = tskrowmin; j <= tskrowmax; j++)
                {

                    TxtNewMap[i, j] = TxtMap[i - tskcolmin, j - tskrowmin];
                }
            }


            if (this.txtNewData == null)
            {
                this.txtNewData = new List<string>();
            }
            else
            {
                this.txtNewData.Clear();
            }

            for (int i = 0; i < col1_1; i++)
            {
                for (int j = 0; j < row1_1; j++)
                {

                    txtNewData.Add(TxtNewMap[i, j].ToString());

                }
            }
            ///////////////////////////对位点比对工作//////////////////////////////////////////////////

            tskPass = 0;
            tskFail = 0;
            txtMark = 0;
            for (int i = 0; i < col1_1; i++)
            {
                for (int j = 0; j < row1_1; j++)
                {
                    if (TxtNewMap[i, j].ToString() == "1")
                    {
                        tskPass++;
                    }

                    if (TxtNewMap[i, j].ToString() == "X")
                    {
                        tskFail++;
                    }

                    if (TxtNewMap[i, j].ToString() == "M")
                    {
                        txtMark++;
                    }

                    //if (TxtNewMap[i, j].ToString() == "M" && TSKMap[i, j].ToString() != ".")
                    //{
                    //    if (MessageBox.Show("对位点不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //    {
                    //        Environment.Exit(0);
                    //    }

                    //}

                }
            }

            //////////////////////////////PASS数比对///////////////////////////////////////

            if (this.txtPass+this.txtFail != (tskPass + tskFail))
            {

                if (MessageBox.Show("总颗数不匹配!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //Environment.Exit(0);
                }
            }

            //------------------------------根据SINF生成新的TSK-MAP----------------------------//

            FileStream fw;
            int flag2 = 0;


            // fw = new FileStream("D:\\MERGE\\" + this.txtSlot.ToString("000") + "." + this.txtWaferID.TrimEnd('\0'), FileMode.Create);
            fw = new FileStream("D:\\MERGE\\" + Convert.ToInt32(this.comboBox1.Text).ToString("000") + "." + this.txtWaferID.TrimEnd('\0'), FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fw);

            //byte[] firstbyte1_1 = (byte[])arryfirstbyte1_1.ToArray(typeof(byte));
            //byte[] firstbyte2_1 = (byte[])arryfirstbyte2_1.ToArray(typeof(byte));

            //byte[] secondbyte1_1 = (byte[])arrysecondbyte1_1.ToArray(typeof(byte));
            //byte[] secondbyte2_1 = (byte[])arrysecondbyte2_1.ToArray(typeof(byte));

            //byte[] thirdbyte1_1 = (byte[])arrythirdbyte1_1.ToArray(typeof(byte));
            //byte[] thirdbyte2_1 = (byte[])arrythirdbyte2_1.ToArray(typeof(byte));

            /////--------------------Map版本为2，且无扩展信息TSK修改BIN信息代码-------------------////
            if ((arry_1.Count == 0) && ((Convert.ToInt32(MapVersion_1) == 2)))
            {
                for (int k = 0; k < row1_1 * col1_1; k++)
                {
                    if (txtNewData[k].ToString() == ".")//Skip Die
                    {
                        continue;

                    }

                    else
                    {

                        if (txtNewData[k].ToString() == "1")//sinf =pass 不改
                        {
                            //  firstbyte1_1[k] = firstbyte1_1[k];
                            //  firstbyte2_1[k] = firstbyte2_1[k];
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] & 1);
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] | 0);//标记成untested
                            secondbyte1_1[k] = secondbyte1_1[k];
                            secondbyte2_1[k] = secondbyte2_1[k];
                            thirdbyte1_1[k] = thirdbyte1_1[k];
                            thirdbyte2_1[k] = thirdbyte2_1[k];

                        }

                        if (txtNewData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                        {
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] & 1);
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] | 128);//标记成fail
                            firstbyte2_1[k] = firstbyte2_1[k];
                            secondbyte1_1[k] = secondbyte1_1[k];
                            secondbyte2_1[k] = secondbyte2_1[k];
                            thirdbyte1_1[k] = thirdbyte1_1[k];
                            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] & 192);
                            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] | 57);//换成想要的BIN57

                        }



                    }


                }
            }

            /////--------------------Map版本为2，且有扩展信息TSK修改BIN信息代码-------------------////
            if ((arry_1.Count > 0) && ((Convert.ToInt32(MapVersion_1) == 2)))
            {
                for (int k = 0; k < row1_1 * col1_1; k++)
                {
                    if (txtNewData[k].ToString() == ".")//Skip Die
                    {
                        continue;

                    }

                    else
                    {

                        if (txtNewData[k].ToString() == "1")//sinf =pass 不改
                        {
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] & 1);
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] | 0);//标记成untested

                            arry_1[4 * k] = arry_1[4 * k];
                            arry_1[4 * k + 1] = arry_1[4 * k + 1];
                            arry_1[4 * k + 2] = arry_1[4 * k + 2];
                            arry_1[4 * k + 3] = arry_1[4 * k + 3];
                        }

                        if (txtNewData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                        {
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] & 1);
                            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] | 128);//标记成fail

                            thirdbyte1_1[k] = thirdbyte1_1[k];
                            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] & 192);
                            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] | 57);//换成想要的BIN57


                            arry_1[4 * k] = arry_1[4 * k];//sitenum
                                                          // arry_1[4 * k + 1] = arry_1[4 * k + 1];//cate
                            arry_1[4 * k + 1] = Convert.ToByte(Convert.ToByte(arry_1[4 * k + 1]) & 192);
                            arry_1[4 * k + 1] = Convert.ToByte(Convert.ToByte(arry_1[4 * k + 1]) | 57);//换成想要的BIN58


                            arry_1[4 * k + 2] = arry_1[4 * k + 2];
                            arry_1[4 * k + 3] = arry_1[4 * k + 3];

                        }



                    }


                }
            }


            /////--------------------Map版本为4，且有扩展信息TSK修改BIN信息代码-------------------////



            //----------------------------TSK修改BIN信息-----------------------------------------------------

            //Operator Size20
            string str = string.Format("{0,-20:G}", Operator_1);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 20);

            //Device Size16
            str = string.Format("{0,-16:G}", Device_1);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 16);

            byte[] buf;
            //WaferSize
            bw.Write(WaferSize_1);
            //MachineNo
            bw.Write(MachineNo_1);
            //IndexSizeX
            bw.Write(IndexSizeX_1);
            //IndexSizeY
            bw.Write(IndexSizeY_1);
            //FlatDir
            this.Reverse(ref FlatDir_1);
            bw.Write(FlatDir_1);
            //MachineType
            bw.Write(MachineType_1);
            //MapVersion
            bw.Write(MapVersion_1);
            //Row
            bw.Write(row_1[1]);
            bw.Write(row_1[0]);
            //Col
            bw.Write(col_1[1]);
            bw.Write(col_1[0]);
            //MapDataForm
            bw.Write(MapDataForm_1);

            //NewWaferID
            str = string.Format("{0,-21:G}", this.txtWaferID.TrimEnd('\0'));
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 21);


            //ProbingNo
            bw.Write(BitConverter.GetBytes(ProbingNo_1), 0, 1);

            //NewLotNo
            str = string.Format("{0,-18:G}", this.txtLot);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 18);

            //CN
            buf = BitConverter.GetBytes((short)CassetteNo_1);
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            //SN
            SlotNo_1 = Convert.ToInt16(comboBox1.Text);
            buf = BitConverter.GetBytes((short)SlotNo_1);
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            //Idex
            bw.Write(IdeX_1);
            //Idey
            bw.Write(IdeY_1);
            //Rdsp
            bw.Write(Rdsp_1);
            //Reserved1
            bw.Write(Reserved1_1);
            //Tdpx
            bw.Write(Tdpx_1);
            //Tdpy
            bw.Write(Tdpy_1);

            //Rdcx
            bw.Write(Rdcx_1);
            //Rdcy
            bw.Write(Rdcy_1);
            //Psps
            bw.Write(Psps_1);
            //Pds
            bw.Write(Pds_1);
            //Reserved2
            bw.Write(Reserved2_1);
            //DistanceX
            bw.Write(DistanceX_1);
            //DistanceY
            bw.Write(DistanceY_1);

            //CoordinatorX
            bw.Write(CoordinatorX_1);
            //CoordinatorY
            bw.Write(CoordinatorY_1);
            //Fdcx
            bw.Write(FdcX_1);
            //Fdxy
            bw.Write(FdcY_1);
            //WTSTIME
            bw.Write(WTSTime_1);
            //WTETIME
            bw.Write(WTETime_1);
            //WLTIME
            bw.Write(WLTime_1);
            //WULT
            bw.Write(WULT_1);

            //MachineNo1
            bw.Write(MachineNo1_1);
            //MachineNo2
            bw.Write(MachineNo2_1);
            //Specialchar
            bw.Write(SpecialChar_1);
            //TestEndInfo
            bw.Write(TestEndInfo_1);
            //Reserved3
            bw.Write(Reserved3_1);
            //Totaldice
            //buf = BitConverter.GetBytes((short)(tskFail+tskPass));-----20221128
            buf = BitConverter.GetBytes((short)(tskFail));
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            // bw.Write(Totaldice_1);
            //TotalPdice
            // bw.Write(TotalPdice_1);
            buf = BitConverter.GetBytes((short)(0));
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            //TotalFdice
            buf = BitConverter.GetBytes((short)(tskFail));
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            // bw.Write(TotalFdice_1);
            //DIAdress
            bw.Write(TDIAdress_1);
            //Numbercategory
            bw.Write(NumberCategory_1);
            //Linecategory
            bw.Write(LineCategory_1);
            //mapconfig
            bw.Write(MapConfig_1);
            //mmsite
            bw.Write(MMSite_1);
            //mcategory
            bw.Write(MCategory_1);
            //Reserved4
            bw.Write(Reserved4_1);

            for (int k = 0; k < row1_1 * col1_1; k++)
            {
                bw.Write(firstbyte1_1[k]);
                bw.Write(firstbyte2_1[k]);
                bw.Write(secondbyte1_1[k]);
                bw.Write(secondbyte2_1[k]);
                bw.Write(thirdbyte1_1[k]);
                bw.Write(thirdbyte2_1[k]);


            }

            //byte[] bufferhead1_20 = br_1.ReadBytes(20);
            //byte[] bufferhead2_16 = br_1.ReadBytes(32);
            //byte[] bufferhead_total = br_1.ReadBytes(4);
            //byte[] bufferhead_pass = br_1.ReadBytes(4);
            //byte[] bufferhead_fail = br_1.ReadBytes(4);
            //byte[] bufferhead4_11 = br_1.ReadBytes(44);
            //byte[] bufferhead1_64 = br_1.ReadBytes(64);
            bw.Write(bufferhead1_20);
            bw.Write(bufferhead2_16);
            // buf = BitConverter.GetBytes((int)(tskFail + tskPass));////不能写total
            buf = BitConverter.GetBytes((int)(tskFail));
            this.Reverse(ref buf);
            bw.Write(buf, 0, 4);
            buf = BitConverter.GetBytes((int)(0));
            this.Reverse(ref buf);
            bw.Write(buf, 0, 4);
            //TotalFdice
            buf = BitConverter.GetBytes((int)(tskFail));
            this.Reverse(ref buf);
            bw.Write(buf, 0, 4);
            bw.Write(bufferhead4_11);
            bw.Write(bufferhead1_64);


            //foreach (byte obj in bufferhead)
            //{
            //    bw.Write(obj);

            //}

            //////扩展信息 mapversion2.3//////////////////////////////////
            foreach (byte obj in arry_1)
            {
                bw.Write(obj);

            }


            bw.Flush();
            bw.Close();
            fw.Close();






            if (MessageBox.Show("转换成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("D:\\MERGE\\");
            }


        }

        private int getTotalOfX(string[,] txtNewData, string v, int col1_1, int row1_1)
        {
            int total = 0;
            for(int i = 0; i < col1_1; i++)
            {
                for(int j = 0; j < row1_1; j++)
                {
                    if ((string)(txtNewData[i, j]) == v)
                    {
                        total += 1;
                    }
                }
            }
           
            return total;
        }

        private void Parse(string line)
        {
            try
            {
                if (!line.Contains("RowData"))
                {
                    string[] strs = line.Split(new char[] { ':','=' });
                    string head = strs[0].Trim();
                    string body = strs[1].Trim().Trim('"');

                    switch (head)
                    {
                        case "DEVICE":
                            this.txtDevice = body;
                            break;
                        case "Lot NO":
                            this.txtLot = body;
                            break;
                        case "Slot NO":
                            this.txtSlot = Convert.ToInt32(body); ;
                            break;
                        case "Wafer ID":
                            this.txtWaferID = body;
                            break;
                        case "Notch Dir":
                            this.txtFlat = body;
                            break;
                        case "ROWCT":
                            this.txtRowct = Convert.ToInt32(body);
                            break;
                        case "COLCT":
                            this.txtColct = Convert.ToInt32(body);
                            break;
                        case "Pass Die":
                            this.txtPass = Convert.ToInt32(body);
                            break;
                        case "Fail Die":
                            this.txtFail = Convert.ToInt32(body);
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
            if (s.StartsWith("RowData"))
            {
                //this.txtColct = s.Length;
                //for (int i = 0; i < s.Length; i++)
                //{
                //    txtData.Add(s[i].ToString());
                //}
                //this.txtRowct++;
                string newLine = s.Substring(s.IndexOf("RowData") + 7 + 1);
                for (int i = 0; i < newLine.Length;)
                {
                    string binNo = newLine.Substring(i, 2);
                    if (binNo.StartsWith("_"))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Trim().Equals("00"))
                    {
                        txtData.Add("1");
                    }
                    else
                    {
                        txtData.Add("X");
                    }
                    i = i + 3;
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
