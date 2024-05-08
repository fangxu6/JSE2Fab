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
    public partial class GTCP1_TXT : Form
    {
        public GTCP1_TXT()
        {
            InitializeComponent();
        }

        ArrayList tsk_Name1 = new ArrayList();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadTSK1File();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void LoadTSK1File()
        {
            this.LoadTSK1();
        }

        private void LoadTSK1()
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
                MessageBox.Show("请选择CP1图谱");
            }

            string LotNo_1 = "";
            object[,] LotSum = new object[100, 100];

            for (int ii = 0; ii < tsk_Name1.Count; ii++)
            {

                ///////-------------------------------TSK1读取-------------------------//////
                FileStream fs_1;
                fs_1 = new FileStream(this.textBox1.Text + @"\" + tsk_Name1[ii], FileMode.Open);
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

                object[,] TSKMap1 = new object[col1_1, row1_1];

                for (int i = 0; i < col1_1; i++)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        if ((secondbyte1_1[j + i * row1_1] & 192) == 0)//Skip Die
                        {
                            // TSKMap[i, j] = ".";
                            TSKMap1[i, j] = ".";

                        }

                        if ((secondbyte1_1[j + i * row1_1] & 192) == 128)//Mark Die
                        {
                            TSKMap1[i, j] = ".";
                        }

                        if ((secondbyte1_1[j + i * row1_1] & 192) == 64)//Probe Die
                        {

                            if ((firstbyte1_1[j + i * row1_1] & 64) == 64)//PASS
                            {
                                TSKMap1[i, j] = "A";
                            }

                            if ((firstbyte1_1[j + i * row1_1] & 128) == 128)//FAIL
                            {
                                TSKMap1[i, j] = "X";

                            }

                        }

                    }
                }



                 object[,] TSKMap3 = new object[col1_1, row1_1];
                int mergepass = 0, mergefail = 0;

                for (int i = 0; i < col1_1; i++)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        TSKMap3[i, j] = "X";

                        if (TSKMap1[i, j] ==null)
                        {
                            TSKMap1[i, j] = ".";
                        }

                        if (TSKMap1[i, j].ToString() == ".")
                        {
                            TSKMap3[i, j] = ".";

                        }
                        if (TSKMap1[i, j].ToString() == "A")
                        {
                            TSKMap3[i, j] = "A";
                            mergepass++;

                        }

                        if (TSKMap3[i, j].ToString() == "X")
                        {
                            mergefail++;
                        }
                       

                    }
                }

                //////////////////////TSKMAP3 去边////////////////////////////////////////////////////
                //------------------------------TSK MAP去边----------------------------//

                int tskrowmin = 0, tskcolmin = 0, tskrowmax = 0, tskcolmax = 0;
                int flag = 0;
                for (int i = 0; i < col1_1; i++)
                {
                    for (int j = 0; j < row1_1; j++)
                    {
                        if ((TSKMap3[i, j].ToString() != "."))
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
                        if ((TSKMap3[i, j].ToString() != "."))
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
                        if ((TSKMap3[j, i].ToString() != "."))
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
                        if ((TSKMap3[j, i].ToString() != "."))
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


                ////////////////////////////////输出TXT//////////////////////////////////
              

                if (!Directory.Exists("D:\\MERGE\\" + LotNo_1 + "\\"))
                {
                    Directory.CreateDirectory("D:\\MERGE\\" + LotNo_1 + "\\");
                }
                FileStream fw;
                //fw = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\" + SlotNo_1.ToString("000") + WaferID_1.TrimEnd('\0') + ".txt", FileMode.Create);
                fw = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\" + LotNo_1 + "." + SlotNo_1.ToString(), FileMode.Create);
                StreamWriter sw = new StreamWriter(fw);
                sw.WriteLine("Lot ID : " + LotNo_1);
                sw.WriteLine("CTM Lot ID: " + LotNo_1);
                sw.WriteLine("Wafer ID : " + SlotNo_1);
                sw.WriteLine("ProductID : " + Device_1);
                sw.WriteLine("Customer code : ");
                sw.WriteLine("Mapping file : ");
                sw.WriteLine("Notch Side : Down");
                sw.WriteLine();

                for (int i = tskrowmin; i < tskrowmax+1; i++)
                {
                    sw.WriteLine();
                    for (int j = tskcolmin; j < tskcolmax+1; j++)
                    {
                        sw.Write(string.Format("{0,1:G}", TSKMap3[i, j]));
                    }

                }

                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine();
                sw.WriteLine("A - Good  die(bin 1)     "+mergepass+"EA");
                sw.WriteLine("X - Bad die");

                sw.Close();
                fw.Close();

                LotSum[ii, 0] = LotNo_1;
                LotSum[ii, 1] = SlotNo_1.ToString("00");
                LotSum[ii, 2] = WaferID_1;
                LotSum[ii, 3] = mergepass + mergefail;
                LotSum[ii, 4] = mergepass;
                LotSum[ii, 5] = mergefail;
                LotSum[ii, 6] = Math.Round((double)(Convert.ToDouble(mergepass) / ((double)Convert.ToInt32(mergepass + mergefail))), 4).ToString("0.00%");

            }

            FileStream fwt;
            fwt = new FileStream("D:\\MERGE\\" + LotNo_1 + "\\" + LotNo_1 + "_Summary" + ".txt", FileMode.Create);
            StreamWriter swt = new StreamWriter(fwt);
            swt.WriteLine("JSE Wafer Sort Summary Report");
            swt.WriteLine("Lot ID : "+LotNo_1);
            swt.WriteLine("CTM Lot ID: " + LotNo_1);
            swt.WriteLine("-----------------");
            swt.WriteLine("|WAF| Good|  YLD|");
            swt.WriteLine("|NO.| Dies|    %|");
            swt.WriteLine("|---+-----+-----+");
            int alldie = 0, allpass = 0, allfail = 0;
            for (int ii = 0; ii < tsk_Name1.Count; ii++)
            {
                swt.WriteLine("| " + LotSum[ii, 1] + "|" + LotSum[ii, 4] + "|" + LotSum[ii, 6]+"|");
                alldie += Convert.ToInt32(LotSum[ii, 3]);
                allpass += Convert.ToInt32(LotSum[ii, 4]);
                allfail += Convert.ToInt32(LotSum[ii, 5]);
            }

            swt.WriteLine("|TTl| " + allpass + "|" + Math.Round((double)(Convert.ToDouble(allpass) / ((double)Convert.ToInt32(allpass + allfail))), 4).ToString("0.00%") + "|");
            swt.WriteLine("----------------");
            swt.WriteLine();
            swt.WriteLine();
            swt.WriteLine("Wafer Count : " + tsk_Name1.Count);
            swt.WriteLine("Total Good Dies :  " + allpass);
            swt.Close();
            fwt.Close();
           



            if (MessageBox.Show("合并成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                Process.Start("D:\\MERGE\\");
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
