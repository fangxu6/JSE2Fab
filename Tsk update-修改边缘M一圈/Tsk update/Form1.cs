﻿using System;
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
using System.Timers;
using Tsk_update.File;
using Tsk_update.Util;


namespace Tsk_update
{





    public partial class Form1 : Form
    {


        private string NewWaferID;
        private string NewLotNo;
        private int NewSlotNo;
        private string tskpath;

        ArrayList arryWaferID = new ArrayList();
        ArrayList arryLotNo = new ArrayList();
        ArrayList arrySlotNo = new ArrayList();
        ArrayList arrayFilepath = new ArrayList();

        public Form1()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {


            for (int i = 0; i < arryLotNo.Count; i++)
            {


                FileStream fs;

                fs = new FileStream(arrayFilepath[i].ToString(), FileMode.Open);

                BinaryReader br = new BinaryReader(fs);

                ///头文件-------------------------------------------------------//

                //Operator Size 20
                string Operator = Encoding.ASCII.GetString(br.ReadBytes(20)).Trim();
                //Device Size 16
                string Device = Encoding.ASCII.GetString(br.ReadBytes(16)).Trim();
                //WaferSize Size 2
                byte[] WaferSize = br.ReadBytes(2);
                //MachineNo Size2
                byte[] MachineNo = br.ReadBytes(2);
                //IndexSizeX Size4
                byte[] IndexSizeX = br.ReadBytes(4);
                //IndexSizeY Size4
                byte[] IndexSizeY = br.ReadBytes(4);
                //FlatDir Size2
                byte[] FlatDir = br.ReadBytes(2);
                //MachineType Size1
                byte MachineType = br.ReadByte();
                //MapVersion Size1
                byte MapVersion = br.ReadByte();
                //row Size2
                byte[] row = br.ReadBytes(2);
                //col Size2
                byte[] col = br.ReadBytes(2);
                //MapDataForm Size4
                byte[] MapDataForm = br.ReadBytes(4);
                //WaferID Size21
                string WaferID = Encoding.ASCII.GetString(br.ReadBytes(21)).Trim();
                //ProbingNo Size1
                byte ProbingNo = br.ReadByte();
                //LotNo Size18
                string LotNo = Encoding.ASCII.GetString(br.ReadBytes(18)).Trim();
                //CassetteNo Size2
                byte[] CN = br.ReadBytes(2);
                this.Reverse(ref CN);
                int CassetteNo = BitConverter.ToInt16(CN, 0);

                //SlotNo Size2
                byte[] SN = br.ReadBytes(2);
                this.Reverse(ref SN);
                int SlotNo = BitConverter.ToInt16(SN, 0);
                //X axis coordinates increase direction Size1
                byte IdeX = br.ReadByte();
                //Y axis coordinates increase direction Size1
                byte IdeY = br.ReadByte();
                //Reference die setting procedures Size1
                byte Rdsp = br.ReadByte();
                //Reserved1 Size1
                byte Reserved1 = br.ReadByte();
                //Target die position X Size4
                byte[] Tdpx = br.ReadBytes(4);
                //Target die position Y Size4
                byte[] Tdpy = br.ReadBytes(4);
                //Reference die coordinator X Size2
                byte[] Rdcx = br.ReadBytes(2);
                //Reference die coordinator Y
                byte[] Rdcy = br.ReadBytes(2);
                // Probing start position Size1
                byte Psps = br.ReadByte();
                //Probing direction Size1
                byte Pds = br.ReadByte();
                //Reserved2 Size2
                byte[] Reserved2 = br.ReadBytes(2);
                //Distance X to wafer center die origin Szie4
                byte[] DistanceX = br.ReadBytes(4);
                //Distance Y to wafer center die origin Size4
                byte[] DistanceY = br.ReadBytes(4);
                //Coordinator X of wafer center die Size4
                byte[] CoordinatorX = br.ReadBytes(4);
                //Coordinator Y of wafer center die Size4
                byte[] CoordinatorY = br.ReadBytes(4);
                //First Die Coordinator X Size4
                byte[] FdcX = br.ReadBytes(4);
                //First Die Coordinator Y Size4
                byte[] FdcY = br.ReadBytes(4);
                //Wafer Testing Start Time Data Size12
                byte[] WTSTime = br.ReadBytes(12);
                //Wafer Testing End Time Data Size12
                byte[] WTETime = br.ReadBytes(12);
                //Wafer Loading Time Data Size 12
                byte[] WLTime = br.ReadBytes(12);
                //Wafer Unloading Time Data Size12
                byte[] WULT = br.ReadBytes(12);
                //Machine No1 Size4
                byte[] MachineNo1 = br.ReadBytes(4);
                //Machine No2 Size4
                byte[] MachineNo2 = br.ReadBytes(4);

                // Special Characters Size4
                byte[] SpecialChar = br.ReadBytes(4);
                //Testing End Information Size1
                byte TestEndInfo = br.ReadByte();
                //Reserved3 Size1
                byte Reserved3 = br.ReadByte();
                //Total tested dice Size2
                byte[] Totaldice = br.ReadBytes(2);
                //Total pass dice Size2
                byte[] TotalPdice = br.ReadBytes(2);
                //Total fail dice Size2
                byte[] TotalFdice = br.ReadBytes(2);
                //Test Die Information Address Size4
                byte[] TDIAdress = br.ReadBytes(4);
                //Number of line category data Size4
                byte[] NumberCategory = br.ReadBytes(4);
                //Line category address Size4
                byte[] LineCategory = br.ReadBytes(4);
                // Map File Configuration Size2
                byte[] MapConfig = br.ReadBytes(2);
                // Max. Multi Site Size2
                byte[] MMSite = br.ReadBytes(2);
                //Max. Categories Size2
                byte[] MCategory = br.ReadBytes(2);
                //Do not use,Reserved4 Size2
                byte[] Reserved4 = br.ReadBytes(2);
                ////////Die 信息/////////////////////

                int row1 = ByteToInt16(ref row);
                int col1 = ByteToInt16(ref col);


                ArrayList arryfirstbyte1 = new ArrayList();
                ArrayList arryfirstbyte2 = new ArrayList();
                ArrayList arrysecondbyte1 = new ArrayList();
                ArrayList arrysecondbyte2 = new ArrayList();
                ArrayList arrythirdbyte1 = new ArrayList();
                ArrayList arrythirdbyte2 = new ArrayList();

                for (int k = 0; k < row1 * col1; k++)
                {
                    arryfirstbyte1.Add(br.ReadByte());
                    arryfirstbyte2.Add(br.ReadByte());
                    arrysecondbyte1.Add(br.ReadByte());
                    arrysecondbyte2.Add(br.ReadByte());
                    arrythirdbyte1.Add(br.ReadByte());
                    arrythirdbyte2.Add(br.ReadByte());
                }


                ArrayList arry = new ArrayList();


                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    arry.Add(br.ReadByte());
                }

                br.Close();
                fs.Close();
                //------------------------------TSK模板Read 结束------------------------------//

                ////////////////////////////////////////////////////////write new tsk/////////////////////////////////////////////////////////

                FileStream fw;


                NewWaferID = arryWaferID[i].ToString();
                NewLotNo = arryLotNo[i].ToString();
                NewSlotNo = Convert.ToInt16(arrySlotNo[i]);


                fw = new FileStream("D:\\New-Tsk\\" + NewSlotNo.ToString("000") + "." + WaferID.Trim('\0'), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fw);

                byte[] firstbyte1 = (byte[])arryfirstbyte1.ToArray(typeof(byte));
                byte[] firstbyte2 = (byte[])arryfirstbyte2.ToArray(typeof(byte));

                byte[] secondbyte1 = (byte[])arrysecondbyte1.ToArray(typeof(byte));
                byte[] secondbyte2 = (byte[])arrysecondbyte2.ToArray(typeof(byte));

                byte[] thirdbyte1 = (byte[])arrythirdbyte1.ToArray(typeof(byte));
                byte[] thirdbyte2 = (byte[])arrythirdbyte2.ToArray(typeof(byte));

                /////--------------------TSK修改BIN信息代码----------------------------------------------------

                int circleNum = 0;
                //一圈
                if (radioButton1.Checked)
                {
                    circleNum = 1;
                }

                //两圈
                if (radioButton2.Checked)
                {
                    circleNum = 2;
                }

                //三圈
                if (radioButton3.Checked)
                {
                    circleNum = 3;
                }





                bool[] isNext = new bool[row1*col1];
                for (int j = 0; j < row1; j++)
                {
                    for (int k = 0; k < col1; k++)
                    {
                        isNext[k * row1 + j] = false;
                    }
                }
                //上 row1是x，表示列；col1是y，表示行
                for (int j = 0; j < row1; j++)
                {
                    for (int k = 0; k < col1; k++)
                    {

                        if (IsMarkDie(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        {
                            isNext[k * row1 + j] = true;
                        }
                    }
                }

                for (int ii = 0; ii < 3; ii++)
                {
                    for (int j = 0; j < row1; j++)
                    {
                        for (int k = 0; k < col1; k++)
                        {

                            if (IsFailDie(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                            {
                                int curDie = k * row1 + j;
                                if (isNext[k * row1 + j - 1])
                                {
                                    isNext[k * row1 + j] = true;
                                }
                                if (isNext[k * row1 + j + 1])
                                {
                                    isNext[k * row1 + j] = true;
                                }
                                if (isNext[k * row1 + j - row1])
                                {
                                    isNext[k * row1 + j] = true;
                                }
                                if (isNext[k * row1 + j + row1])
                                {
                                    isNext[k * row1 + j] = true;
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < row1; j++)
                {
                    for (int k = 0; k < col1; k++)
                    {
                        if ((k - 1) >= 0)
                        {
                            if (isNext[(k - 1) * row1 + j])
                            {
                                if (IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                                {
                                    for (int m = 0; m < circleNum; m++)
                                    {
                                        int currentDie = (k + m) * row1 + j;
                                        firstbyte1[currentDie] = Convert.ToByte((firstbyte1[currentDie] & 1));
                                        firstbyte1[currentDie] = Convert.ToByte(firstbyte1[currentDie] | 128);//标记为Fail

                                        thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] & 192));
                                        thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] | 60));  //换category,全部换成60
                                    }
                                }
                                //break;
                            }
                        }

                        //if (!IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        //{
                        //    isNext[j] = true;
                        //}
                        //else
                        //{
                        //    isNext[j] = false;
                        //}



                    }
                }

                //下 row1是x，表示列；col1是y，表示行
                for (int j = 0; j < row1; j++)
                {
                    for (int k = col1 - 1; k >= 0; k--)
                    {
                        if ((k + 1) <col1 && isNext[(k + 1) * row1 + j] && IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        {
                            for (int m = 0; m < circleNum; m++)
                            {
                                int currentDie = (k - m) * row1 + j;
                                firstbyte1[currentDie] = Convert.ToByte((firstbyte1[currentDie] & 1));
                                firstbyte1[currentDie] = Convert.ToByte(firstbyte1[currentDie] | 128);//标记为Fail

                                thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] & 192));
                                thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] | 60));  //换category,全部换成60
                            }
                            //break;
                        }
                        //if (!IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        //{
                        //    isNext[j] = true;
                        //}
                        //else
                        //{
                        //    isNext[j] = false;
                        //}
                        
                    }
                }

                //左 row1是x，表示列；col1是y，表示行
                for (int k = 0; k < col1; k++)
                {
                    for (int j = 0; j < row1; j++)
                    {
                        if (j>0 && isNext[k * row1 + j-1] && IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        {
                            for (int m = 0; m < circleNum; m++)
                            {
                                int currentDie = k * row1 + j + m;
                                firstbyte1[currentDie] = Convert.ToByte((firstbyte1[currentDie] & 1));
                                firstbyte1[currentDie] = Convert.ToByte(firstbyte1[currentDie] | 128);//标记为Fail

                                thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] & 192));
                                thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] | 60));  //换category,全部换成60
                            }
                            //break;
                        }
                        //if (!IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        //{
                        //    isNext[j] = true;
                        //}
                        //else
                        //{
                        //    isNext[j] = false;
                        //}

                    }
                }

                //右 row1是x，表示列；col1是y，表示行
                for (int k = 0; k < col1; k++)
                {
                    for (int j = row1 - 1; j >= 0; j--)
                    {
                        if ((j+1) < row1 && isNext[k * row1 + j + 1] && IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        {
                            for (int m = 0; m < circleNum; m++)
                            {
                                int currentDie = k * row1 + j - m;
                                firstbyte1[currentDie] = Convert.ToByte((firstbyte1[currentDie] & 1));
                                firstbyte1[currentDie] = Convert.ToByte(firstbyte1[currentDie] | 128);//标记为Fail

                                thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] & 192));
                                thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] | 60));  //换category,全部换成60
                            }
                            //break;
                        }

                        //if (!IsPassDieOrFail60(k * row1 + j, secondbyte1, firstbyte1, thirdbyte2))
                        //{
                        //    isNext[j] = true;
                        //}
                        //else
                        //{
                        //    isNext[j] = false;
                        //}
                        
                    }
                }

                //for (int k = 0; k < row1 * col1 - row1; k++)
                //{
                //    if ((secondbyte1[k] & 192) == 0 && (secondbyte1[k] & 2) == 2)//Dummy Die
                //    {
                //        continue;
                //    }
                //    if (((secondbyte1[k] & 192) != 64))//Mark Die|Skip Die
                //    {
                //        //是否是边缘Mark Die|Skip Die 临时方案 周边一圈至少有4颗Mark Die|Skip Die才是边缘
                //        //其实需要先检测到是边缘
                //        int tempCircle = 1;
                //        int sumTempMarkDie = 0;
                //        for (int ii = tempCircle * (-1); ii <= tempCircle; ii++)
                //        {
                //            for (int jj = tempCircle * (-1); jj <= tempCircle; jj++)
                //            {
                //                int currentDie = k + ii * row1 + jj;
                //                if (currentDie < 0 | currentDie >= row1 * col1)
                //                {
                //                    continue;
                //                }
                //                if (((secondbyte1[currentDie] & 192) != 64))
                //                {
                //                    sumTempMarkDie++;
                //                }
                //            }
                //        }
                //        if ((sumTempMarkDie < 4))
                //        {
                //            continue;
                //        }

                //        //上下
                //        for (int m = circleNum * (-1); m <= circleNum; m++)
                //        {
                //            int currentDie = k + m * row1;


                //            if (currentDie < 0 | currentDie >= row1 * col1)
                //            {
                //                continue;
                //            }
                //            if ((secondbyte1[currentDie] & 192) == 64)//为测试DIE
                //            {
                //                if ((firstbyte1[currentDie] & 128) != 128)//不是fail Die
                //                {
                //                    firstbyte1[currentDie] = Convert.ToByte((firstbyte1[currentDie] & 1));
                //                    firstbyte1[currentDie] = Convert.ToByte(firstbyte1[currentDie] | 128);//标记为Fail

                //                    thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] & 192));
                //                    thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] | 60));  //换category,全部换成60
                //                }

                //            }
                //        }

                //        //左右
                //        for (int m = circleNum * (-1); m <= circleNum; m++)
                //        {
                //            int currentDie = k + m;
                //            if (currentDie < 0 | currentDie >= row1 * col1)
                //            {
                //                continue;
                //            }
                //            if ((secondbyte1[currentDie] & 192) == 64)//为测试DIE
                //            {
                //                if ((firstbyte1[currentDie] & 128) != 128)//不是fail Die
                //                {
                //                    firstbyte1[currentDie] = Convert.ToByte((firstbyte1[currentDie] & 1));
                //                    firstbyte1[currentDie] = Convert.ToByte(firstbyte1[currentDie] | 128);//标记为Fail

                //                    thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] & 192));
                //                    thirdbyte2[currentDie] = Convert.ToByte((thirdbyte2[currentDie] | 60));  //换category,全部换成60
                //                }

                //            }
                //        }
                //    }

                //}

                /*
                if (((secondbyte1[k] & 192) == 128) && ((secondbyte1[k + 1] & 192) == 64))//Mark Die,且右边为测试DIE
                {


                    if ((secondbyte1[k + 1] & 192) == 64)//右边为测试die
                    {
                        firstbyte1[k + 1] = Convert.ToByte((firstbyte1[k + 1] & 1));
                        firstbyte1[k + 1] = Convert.ToByte(firstbyte1[k + 1] | 128);//标记为Fail

                        thirdbyte2[k + 1] = Convert.ToByte((thirdbyte2[k + 1] & 192));
                        thirdbyte2[k + 1] = Convert.ToByte((thirdbyte2[k + 1] | 60));  //换category,全部换成60

                    }

                    if ((secondbyte1[k-1] & 192) == 64)//左边为测试die
                    {
                        firstbyte1[k - 1] = Convert.ToByte((firstbyte1[k - 1] & 1));
                        firstbyte1[k - 1] = Convert.ToByte(firstbyte1[k - 1] | 128);//标记为Fail

                        thirdbyte2[k - 1] = Convert.ToByte((thirdbyte2[k - 1] & 192));
                        thirdbyte2[k - 1] = Convert.ToByte((thirdbyte2[k - 1] | 60));  //换category,全部换成60

                    }

                    if ((k + row1) < (row1 * col1))
                    {
                        if ((secondbyte1[k + row1] & 192) == 64)//下方为测试die
                        {
                            firstbyte1[k + row1] = Convert.ToByte((firstbyte1[k + row1] & 1));
                            firstbyte1[k + row1] = Convert.ToByte(firstbyte1[k + row1] | 128);//标记为Fail

                            thirdbyte2[k + row1] = Convert.ToByte((thirdbyte2[k + row1] & 192));
                            thirdbyte2[k + row1] = Convert.ToByte((thirdbyte2[k + row1] | 60));  //换category,全部换成60

                        }
                    }

                    if ((k + row1-1) < (row1 * col1))
                    {
                        if ((secondbyte1[k + row1-1] & 192) == 64)//左下方为测试die
                        {
                            firstbyte1[k + row1 - 1] = Convert.ToByte((firstbyte1[k + row1 - 1] & 1));
                            firstbyte1[k + row1 - 1] = Convert.ToByte(firstbyte1[k + row1 - 1] | 128);//标记为Fail

                            thirdbyte2[k + row1 - 1] = Convert.ToByte((thirdbyte2[k + row1 - 1] & 192));
                            thirdbyte2[k + row1 - 1] = Convert.ToByte((thirdbyte2[k + row1 - 1] | 60));  //换category,全部换成60

                        }
                    }

                    if ((k + row1 + 1) < (row1 * col1))
                    {
                        if ((secondbyte1[k + row1 + 1] & 192) == 64)//右下方为测试die
                        {
                            firstbyte1[k + row1 + 1] = Convert.ToByte((firstbyte1[k + row1 + 1] & 1));
                            firstbyte1[k + row1 + 1] = Convert.ToByte(firstbyte1[k + row1 + 1] | 128);//标记为Fail

                            thirdbyte2[k + row1 + 1] = Convert.ToByte((thirdbyte2[k + row1 + 1] & 192));
                            thirdbyte2[k + row1 + 1] = Convert.ToByte((thirdbyte2[k + row1 + 1] | 60));  //换category,全部换成60

                        }
                    }






                    if ((k-row1) >0)
                    {
                        if ((secondbyte1[k - row1] & 192) == 64)//上方为测试die
                        {
                            firstbyte1[k - row1] = Convert.ToByte((firstbyte1[k - row1] & 1));
                            firstbyte1[k - row1] = Convert.ToByte(firstbyte1[k - row1] | 128);//标记为Fail

                            thirdbyte2[k - row1] = Convert.ToByte((thirdbyte2[k - row1] & 192));
                            thirdbyte2[k - row1] = Convert.ToByte((thirdbyte2[k - row1] | 60));  //换category,全部换成60

                        }
                    }

                    if ((k - row1-1) > 0)
                    {
                        if ((secondbyte1[k - row1-1] & 192) == 64)//左上方为测试die
                        {
                            firstbyte1[k - row1 - 1] = Convert.ToByte((firstbyte1[k - row1 - 1] & 1));
                            firstbyte1[k - row1 - 1] = Convert.ToByte(firstbyte1[k - row1 - 1] | 128);//标记为Fail

                            thirdbyte2[k - row1 - 1] = Convert.ToByte((thirdbyte2[k - row1 - 1] & 192));
                            thirdbyte2[k - row1 - 1] = Convert.ToByte((thirdbyte2[k - row1 - 1] | 60));  //换category,全部换成60

                        }
                    }

                    if ((k - row1 + 1) > 0)
                    {
                        if ((secondbyte1[k - row1 + 1] & 192) == 64)//右上方为测试die
                        {
                            firstbyte1[k - row1 + 1] = Convert.ToByte((firstbyte1[k - row1 + 1] & 1));
                            firstbyte1[k - row1 + 1] = Convert.ToByte(firstbyte1[k - row1 + 1] | 128);//标记为Fail

                            thirdbyte2[k - row1 + 1] = Convert.ToByte((thirdbyte2[k - row1 + 1] & 192));
                            thirdbyte2[k - row1 + 1] = Convert.ToByte((thirdbyte2[k - row1 + 1] | 60));  //换category,全部换成60

                        }
                    }



                }
                */

                //if ((secondbyte1[k] & 192) == 64)//Probe Die
                //{
                //    if (((firstbyte1[k] & 128) == 128) && ((secondbyte1[k + 1] & 192) == 64))
                //    {
                //        firstbyte1[k + 1] = Convert.ToByte((firstbyte1[k + 1] & 1));
                //        firstbyte1[k + 1] = Convert.ToByte(firstbyte1[k + 1] | 128);//标记为Fail

                //        thirdbyte2[k + 1] = Convert.ToByte((thirdbyte2[k + 1] & 192));
                //        thirdbyte2[k + 1] = Convert.ToByte((thirdbyte2[k + 1] | 60));  //换category,全部换成20

                //    }

                //}


                // }//----------------------------TSK修改BIN信息-----------------------------------------------------

                //Operator Size20
                string str = string.Format("{0,-20:G}", Operator);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 20);

                //Device Size16
                str = string.Format("{0,-16:G}", Device);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 16);

                byte[] buf;
                //WaferSize
                bw.Write(WaferSize);
                //MachineNo
                bw.Write(MachineNo);
                //IndexSizeX
                bw.Write(IndexSizeX);
                //IndexSizeY
                bw.Write(IndexSizeY);
                //FlatDir
                bw.Write(FlatDir);
                //MachineType
                bw.Write(MachineType);
                //MapVersion
                bw.Write(MapVersion);
                //Row
                bw.Write(row[1]);
                bw.Write(row[0]);
                //Col
                bw.Write(col[1]);
                bw.Write(col[0]);
                //MapDataForm
                bw.Write(MapDataForm);

                //NewWaferID
                str = string.Format("{0,-21:G}", NewWaferID);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 21);

                //ProbingNo
                bw.Write(BitConverter.GetBytes(ProbingNo), 0, 1);

                //NewLotNo
                str = string.Format("{0,-18:G}", NewLotNo);
                bw.Write(Encoding.ASCII.GetBytes(str), 0, 18);

                //CN
                buf = BitConverter.GetBytes((short)CassetteNo);
                this.Reverse(ref buf);
                bw.Write(buf, 0, 2);
                //SN
                buf = BitConverter.GetBytes((short)NewSlotNo);
                this.Reverse(ref buf);
                bw.Write(buf, 0, 2);
                //Idex
                bw.Write(IdeX);
                //Idey
                bw.Write(IdeY);
                //Rdsp
                bw.Write(Rdsp);
                //Reserved1
                bw.Write(Reserved1);
                //Tdpx
                bw.Write(Tdpx);
                //Tdpy
                bw.Write(Tdpy);

                //Rdcx
                bw.Write(Rdcx);
                //Rdcy
                bw.Write(Rdcy);
                //Psps
                bw.Write(Psps);
                //Pds
                bw.Write(Pds);
                //Reserved2
                bw.Write(Reserved2);
                //DistanceX
                bw.Write(DistanceX);
                //DistanceY
                bw.Write(DistanceY);

                //CoordinatorX
                bw.Write(CoordinatorX);
                //CoordinatorY
                bw.Write(CoordinatorY);
                //Fdcx
                bw.Write(FdcX);
                //Fdxy
                bw.Write(FdcY);
                //WTSTIME
                bw.Write(WTSTime);
                //WTETIME
                bw.Write(WTETime);
                //WLTIME
                bw.Write(WLTime);
                //WULT
                bw.Write(WULT);

                //MachineNo1
                bw.Write(MachineNo1);
                //MachineNo2
                bw.Write(MachineNo2);
                //Specialchar
                bw.Write(SpecialChar);
                //TestEndInfo
                bw.Write(TestEndInfo);
                //Reserved3
                bw.Write(Reserved3);
                //Totaldice
                bw.Write(Totaldice);
                //TotalPdice
                bw.Write(TotalPdice);
                //TotalFdice
                bw.Write(TotalFdice);
                //DIAdress
                bw.Write(TDIAdress);
                //Numbercategory
                bw.Write(NumberCategory);
                //Linecategory
                bw.Write(LineCategory);
                //mapconfig
                bw.Write(MapConfig);
                //mmsite
                bw.Write(MMSite);
                //mcategory
                bw.Write(MCategory);
                //Reserved4
                bw.Write(Reserved4);

                for (int k = 0; k < row1 * col1; k++)
                {
                    bw.Write(firstbyte1[k]);
                    bw.Write(firstbyte2[k]);
                    bw.Write(secondbyte1[k]);
                    bw.Write(secondbyte2[k]);
                    bw.Write(thirdbyte1[k]);
                    bw.Write(thirdbyte2[k]);


                }

                foreach (byte obj in arry)
                {
                    bw.Write(obj);

                }

                bw.Flush();
                bw.Close();
                fw.Close();






            }
            //   }

            MessageBox.Show("修改完成");

        }

        void dfs(int[][] grid, int r, int c)
        {
            // 若坐标不合法，直接返回
            if (!(0 <= r && r < grid.Length && 0 <= c && c < grid[0].Length))
            {
                return;
            }
            // 已遍历过（值为2）的岛屿在这里会直接返回，不会重复遍历
            if (grid[r][c] != 1)
            {
                return;
            }
            grid[r][c] = 2; // 将方格标记为"已遍历"



            dfs(grid, r - 1, c); // 上边相邻
            dfs(grid, r + 1, c); // 下边相邻
            dfs(grid, r, c - 1); // 左边相邻
            dfs(grid, r, c + 1); // 右边相邻
        }

        private bool IsPassDieOrFail60(int k, byte[] secondbyte1, byte[] firstbyte1, byte[] thirdbyte2)
        {
            //if ((secondbyte1[k] & 192) == 0 && (secondbyte1[k] & 2) == 2)//Dummy Die
            //{
            //    return false;
            //}
            //if (((secondbyte1[k] & 192) != 64))//Mark Die|Skip Die
            //{
            //}
            if ((secondbyte1[k] & 192) == 64)//Test Die
            {
                if ((firstbyte1[k] & 128) != 128)//Pass Die
                {
                    return true;
                }
                if ((firstbyte1[k] & 128) == 128)//Fail Die且是Fail60
                {
                    if ((thirdbyte2[k] & 60) == 60)
                    {
                        return true;
                    }
                    
                }
            }
            return false;
        }

        private bool IsMarkDie(int k, byte[] secondbyte1, byte[] firstbyte1, byte[] thirdbyte2)
        {
            if ((secondbyte1[k] & 192) == 0 && (secondbyte1[k] & 2) == 2)//Dummy Die
            {
                return true;
            }
            if (((secondbyte1[k] & 192) != 64))//Mark Die|Skip Die
            {
                return true;
            }
            return false;
        }

        private bool IsFailDie(int k, byte[] secondbyte1, byte[] firstbyte1, byte[] thirdbyte2)
        {
            if ((secondbyte1[k] & 192) == 64)//Test Die
            {
                if ((firstbyte1[k] & 128) != 128)//Pass Die
                {
                    return false;
                }
                if ((firstbyte1[k] & 128) == 128)//Fail Die且是Fail60
                {
                    if ((thirdbyte2[k] & 60) != 60)
                    {
                        return true;
                    }
                }
            }
            return false;
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


        /*   private void ReadTSk(string str)
           { 

                FileStream fs;


               fs = new FileStream(str, FileMode.Open);


               BinaryReader br = new BinaryReader(fs);


               string Operator = Encoding.ASCII.GetString(br.ReadBytes(20)).Trim(); 

               string Device = Encoding.ASCII.GetString(br.ReadBytes(16)).Trim();
               int WaferSize = br.ReadInt16();
               int MachineNo = br.ReadInt16();
               int IndexSizeX = br.ReadInt32();
               int IndexSizeY = br.ReadInt32();
               int FlatDir = br.ReadInt16();
               byte MachineType = br.ReadByte();
               byte MapVersion = br.ReadByte();


              // int rows = br.ReadInt16();
              // int cols = br.ReadInt16();
               byte row1 = br.ReadByte();
               byte row2 = br.ReadByte();
               byte col1 = br.ReadByte();
               byte col2 = br.ReadByte();
               int MapDataForm = br.ReadInt32();
               string WaferID = Encoding.ASCII.GetString(br.ReadBytes(21)).Trim();
               byte ProbingNo = br.ReadByte();
               string LotNo = Encoding.ASCII.GetString(br.ReadBytes(18)).Trim();

               byte[] CN = br.ReadBytes(2);
               this.Reverse(ref CN);
               int CassetteNo  = BitConverter.ToInt16(CN, 0);



               byte[] SN = br.ReadBytes(2);
               this.Reverse(ref SN);
               int SlotNo=BitConverter.ToInt16(SN, 0);
               //int SlotNo = br.ReadInt16();


               ArrayList arry = new ArrayList();


               while (br.BaseStream.Position < br.BaseStream.Length)
               {
                   arry.Add(br.ReadByte());
               }






               br.Close();
               fs.Close();


           }
          */

        /*   private void button2_Click(object sender, EventArgs e)
           {


               OpenFileDialog dialog = new OpenFileDialog();
               dialog.RestoreDirectory = false;
               dialog.Multiselect = false;
               dialog.Filter = "";
               if (dialog.ShowDialog() == DialogResult.OK)
               {
                   this.textBox1.Text = Path.GetFullPath(dialog.FileName);
                   tskpath = Path.GetDirectoryName(dialog.FileName);

               }



           }
         */

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in dialog.FileNames)
                {

                    Tsk tsk = new Tsk(str);
                    tsk.Read();

                    arryLotNo.Add(tsk.LotNo);
                    arryWaferID.Add(tsk.WaferID);
                    arrySlotNo.Add(tsk.SlotNo);
                    arrayFilepath.Add(str);


                }

            }

            if (arryLotNo.Count > 0)
            {

                this.dataGridView1.Columns.Clear();

                this.dataGridView1.Columns.Add("c1", "LotNo");
                this.dataGridView1.Columns.Add("c2", "SlotNo");
                this.dataGridView1.Columns.Add("c3", "WaferID");
                this.dataGridView1.Columns.Add("c3", "PATH");
                this.dataGridView1.Rows.Add(arryLotNo.Count);
                for (int i = 0; i < arryLotNo.Count; i++)
                {
                    this.dataGridView1[0, i].Value = arryLotNo[i];
                    this.dataGridView1[1, i].Value = arrySlotNo[i];
                    this.dataGridView1[2, i].Value = arryWaferID[i];
                    this.dataGridView1[3, i].Value = arrayFilepath[i];
                    //if (arrySlotNo[i].ToString() != "0")
                    //{
                    //    this.dataGridView1[1, i].ReadOnly = true;

                    //}

                }


                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

            }



        }


    }
}