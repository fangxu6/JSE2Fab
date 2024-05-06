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
using DataToExcel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

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
            }

          

            for (int ii = 0; ii < tsk_Name1.Count; ii++)
            {
                string LotNo_1 = "";
                string LotNo_2 = "";
                string NewWaferID_1 = "";

                ///////-------------------------------TSK1读取-------------------------//////
                //Tsk tsk = new Tsk(this.textBox1.Text + @"\" + tsk_Name1[ii]);
                //tsk.Read();
                //tsk.DeasilRotate(90);
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
                short TSKFlat1 = BitConverter.ToInt16(FlatDir_1, 0);
                //MachineType Size1
                byte MachineType_1 = br_1.ReadByte();
                //MapVersion Size1 TODO
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

                byte[] firstbyte1_1 = (byte[])arryfirstbyte1_1.ToArray(typeof(byte));
                byte[] firstbyte2_1 = (byte[])arryfirstbyte2_1.ToArray(typeof(byte));

                byte[] secondbyte1_1 = (byte[])arrysecondbyte1_1.ToArray(typeof(byte));
                byte[] secondbyte2_1 = (byte[])arrysecondbyte2_1.ToArray(typeof(byte));

                byte[] thirdbyte1_1 = (byte[])arrythirdbyte1_1.ToArray(typeof(byte));
                byte[] thirdbyte2_1 = (byte[])arrythirdbyte2_1.ToArray(typeof(byte));

                //------------------------------TSK1模板Read 结束------------------------------//

               string step = LotNo_1.Substring(LotNo_1.Length - 3, 3);
               if (step == "CP1")
               { 
               LotNo_2 = LotNo_1.Replace("CP1", "CP2");
               NewWaferID_1 = WaferID_1.Replace("CP1", "CP2");
               }

               else if (step == "CP2")
               {
                   LotNo_2 = LotNo_1.Replace("CP2", "CP3");
                   NewWaferID_1 = WaferID_1.Replace("CP2", "CP3");
               }
               else
               {
                   LotNo_2 = LotNo_1;
                   NewWaferID_1 = WaferID_1;
               
               }


           


                //----------------------------重写TSK----------------------------------------//
                FileStream fw;
                if (!Directory.Exists("D:\\NewTsk\\" + LotNo_2 + "\\"))
                {
                    Directory.CreateDirectory("D:\\NewTsk\\" + LotNo_2 + "\\");
                }

                fw = new FileStream("D:\\NewTsk\\"+LotNo_2+"\\" + SlotNo_1.ToString("000") + "." + NewWaferID_1.TrimEnd('\0'), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fw);


                //Operator Size20
                string str = string.Format("{0,-20:G}", Operator_1);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 20);

                //Device Size16
                str = string.Format("{0,-16:G}", Device_1);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 16);

                byte[] buf;
                //WaferSize
                this.Reverse(ref WaferSize_1);
                bw.Write(WaferSize_1);
                //MachineNo
                bw.Write(MachineNo_1);
                //IndexSizeX
                bw.Write(IndexSizeX_1);
                //IndexSizeY
                bw.Write(IndexSizeY_1);
                //FlatDir
                TSKFlat1 = (short)(TSKFlat1 + 90);
                FlatDir_1 = BitConverter.GetBytes(TSKFlat1);
                this.Reverse(ref FlatDir_1);
                bw.Write(FlatDir_1);
                //MachineType
                bw.Write(MachineType_1);
                //MapVersion
                bw.Write(MapVersion_1);
                //Row 互换
                bw.Write(col_1[1]);
                bw.Write(col_1[0]);
                //Col 互换
                bw.Write(row_1[1]); 
                bw.Write(row_1[0]); 
                //MapDataForm
                bw.Write(MapDataForm_1);

                //NewWaferID
                str = string.Format("{0,-21:G}", NewWaferID_1.TrimEnd('\0'));
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 21);


                //ProbingNo
                bw.Write(BitConverter.GetBytes(ProbingNo_1), 0, 1);

                //NewLotNo
                str = string.Format("{0,-18:G}", LotNo_2);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 18);

                //CN
                buf = BitConverter.GetBytes((short)CassetteNo_1);
                this.Reverse(ref buf);
                bw.Write(buf, 0, 2);
                //SN
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
                bw.Write(TotalFdice_1);
                buf = BitConverter.GetBytes((short)(0));
                this.Reverse(ref buf);
                bw.Write(buf, 0, 2);
                //TotalFdice
                bw.Write(TotalFdice_1);
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





                int TotalDieCount = row1_1 * col1_1;
                byte[] newTskFirstbyte1_1 = new byte[TotalDieCount];
                byte[] newTskFirstbyte2_1 = new byte[TotalDieCount];
                byte[] newTskSecondbyte1_1 = new byte[TotalDieCount];
                byte[] newTskSecondbyte2_1 = new byte[TotalDieCount];
                byte[] newTskThirdbyte1_1 = new byte[TotalDieCount];
                byte[] newTskThirdbyte2_1 = new byte[TotalDieCount];

                int x = -1, y = -1, xr = -1, yr = -1;
                for (int i = 0; i < row1_1 * col1_1; i++)
                {
                    // 计算 x,y 坐标
                    x = i % col1_1;
                    y = i / col1_1;

                    xr = (row1_1 - 1) - y;
                    yr = x;

                    //DegtxtData[yr * row1_1 + xr] = txtData[i];
                    newTskFirstbyte1_1[yr * row1_1 + xr] = firstbyte1_1[i];
                    newTskFirstbyte2_1[yr * row1_1 + xr] = firstbyte2_1[i];
                    newTskSecondbyte1_1[yr * row1_1 + xr] = secondbyte1_1[i];
                    newTskSecondbyte2_1[yr * row1_1 + xr] = secondbyte2_1[i];
                    newTskThirdbyte1_1[yr * row1_1 + xr] = thirdbyte1_1[i];
                    newTskThirdbyte2_1[yr * row1_1 + xr] = thirdbyte2_1[i];





                }

                for (int k = 0; k < row1_1 * col1_1; k++)
                {
                    bw.Write(newTskFirstbyte1_1[k]  );
                    bw.Write(newTskFirstbyte2_1[k]  );
                    bw.Write(newTskSecondbyte1_1[k]  );
                    bw.Write(newTskSecondbyte2_1[k]  );
                    bw.Write(newTskThirdbyte1_1[k]  );
                    bw.Write(newTskThirdbyte2_1[k]  );
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
                bw.Write(bufferhead_fail);
                buf = BitConverter.GetBytes((int)(0));
                this.Reverse(ref buf);
                bw.Write(buf, 0, 4);
                //TotalFdice
                bw.Write(bufferhead_fail);
                bw.Write(bufferhead4_11);
                bw.Write(bufferhead1_64);

                if (arry_1.Count > 0)
                {
                    Console.WriteLine("it has extend info.");
                }
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
            }


            if (MessageBox.Show("转换成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("D:\\NewTsk\\");
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
