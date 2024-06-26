﻿using Excel;
using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_C8A000WBB_8_30_0 : ExpToExcelSoftBin
    {
        //临时修改+1
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:OPEN_SHORT_FAIL";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD_FAIL";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:R_CLK_FAIL";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:IIH_FAIL";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "Bin9:IDD_LDO_REGU_FAIL";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "Bin10:FUNC_nor_FAIL";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "Bin11:FUNC_mbist_FAIL";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "Bin12:FUNC_mbist_2V_FAIL";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "Bin13:FUNC_mbist_1P4V_FAIL";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
            rngbin14.Value2 = "Bin14:SDO_5V_FAIL";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
            rngbin15.Value2 = "Bin15:IOUT_27K_FAIL";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
            rngbin16.Value2 = "Bin16:SKEW_27K_FAIL";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "Bin17:IOUT_27K_AVE_FAIL";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "Bin18:VR_DN_FAIL";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "Bin19:LEAKAGE_ad1_FAIL";

        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            int errflag = 0;
            //片良率
            if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.985)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 5], worksheet2.Cells[(num2 + 1) + 8, 5]).Interior.ColorIndex = 7;
                errflag++;
            }

            //卡bin
            for (int i = 5; i <= 19; i++)
            {
                errflag += overYield(arrayHeaderInfo, i, 0.005, worksheet2, num2);
            }


            if (errflag > 0)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
                //MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            }
        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        public override bool defatultSave()
        {
            return false;
        }

        public override void Save(CmdTxt cmd)
        {
            try
            {
                try
                {

                    if (File.Exists(cmd.FullName))
                    {
                        File.Delete(cmd.FullName);
                    }
                    cmd.OpenWriter();

                    cmd.WriteString("Device:" + cmd.Operator + cmd.Enter);
                    cmd.WriteString("Lot NO:" + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("Wafer ID:" + cmd.SlotNo.ToString("00") + cmd.Enter);
                    string WaferSize1 = "";

                    if (cmd.WaferSize == 60)
                    {
                        WaferSize1 = "6 Inch";
                    }
                    else if (cmd.WaferSize == 80)
                    {
                        WaferSize1 = "8";

                    }

                    else if (cmd.WaferSize == 120)
                    {
                        WaferSize1 = "12 Inch";

                    }
                    cmd.WriteString("Wafer Size:" + WaferSize1 + cmd.Enter);

                    string FlatDir1 = "";

                    if (cmd.FlatDir == 90)
                    {
                        FlatDir1 = "Right";
                    }

                    else if (cmd.FlatDir == 180)
                    {
                        FlatDir1 = "Down";
                    }
                    else if (cmd.FlatDir == 270)
                    {
                        FlatDir1 = "Left";
                    }
                    else if (cmd.FlatDir == 0)
                    {
                        FlatDir1 = "Up";
                    }

                    cmd.WriteString("Notch:" + FlatDir1 + cmd.Enter);
                    int flagbin = 0;

                    int ymin = 1000, xmin = 1000, ymax = 0, xmax = 0;
                    {
                        for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                        {

                            for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                            {
                                switch (cmd.DieMatrix[x, y].Attribute)
                                {
                                    case DieCategory.PassDie:
                                    case DieCategory.FailDie:
                                    case DieCategory.SkipDie2:
                                        if (xmin > x) { xmin = x; }
                                        if (ymin > y) { ymin = y; }
                                        if (ymax < y) { ymax = y; }
                                        if (xmax < x) { xmax = x; }
                                        int xxx = cmd.DieMatrix[x, y].Bin;
                                        if (xxx == 2)
                                        {
                                            flagbin = 1;
                                        }
                                        break;
                                }

                            }
                        }
                    }

                    cmd.WriteString("ROWCT:" + (ymax - ymin + 1) + cmd.Enter);
                    cmd.WriteString("COLCT:" + (xmax - xmin + 1) + cmd.Enter);
                    if (flagbin == 0)
                    {
                        cmd.WriteString("PASS BIN:1" + cmd.Enter);
                    }
                    else
                    {
                        cmd.WriteString("PASS BIN:1,2" + cmd.Enter);
                    }
                    cmd.WriteString("Test Start Time:" + cmd.LoadTime.ToString("yy/MM/dd HH:mm:ss") + cmd.Enter);
                    cmd.WriteString("Test End Time:" + cmd.EndTime.ToString("yy/MM/dd HH:mm:ss") + cmd.Enter);
                    cmd.WriteString("Gross die:" + (cmd.PassDie + cmd.FailDie) + cmd.Enter);
                    cmd.WriteString("Pass Die:" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("Fail Die:" + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)(cmd.PassDie / ((double)(cmd.PassDie + cmd.FailDie)))), 6).ToString("0.0000%") + cmd.Enter);
                    cmd.WriteString("StrBin:1,1;5,5;6,6;7,7;8,8;9,9;10,A;11,B;12,C;13,D;14,E;15,F;16,G;17,H;18,I;19,J;60,X;" + cmd.Enter);

                    int skipDieNum = cmd.DieMatrix.DieAttributeAccurateStat(DieCategory.SkipDie2);
                    if (skipDieNum > ConstDefine.WarningSipDieNumber)
                    {
                        MessageBox.Show(string.Format("片号" + cmd.SlotNo.ToString("00") + "的skip die '#' 超过{0:d}个，请注意。", ConstDefine.WarningSipDieNumber));
                    }

                    for (int y = ymin; y < ymax + 1; y++)
                    {

                        for (int x = xmin; x < xmax + 1; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        int xxx = cmd.DieMatrix[x, y].Bin;
                                        cmd.WriteString(string.Format("{0,1:G}", cmd.DieMatrix[x, y].Bin));
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                case DieCategory.SkipDie2:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(cmd.DieMatrix[x, y].Attribute)));
                                        break;
                                    }

                                case DieCategory.FailDie:
                                    {
                                        if (cmd.DieMatrix[x, y].Bin < 10)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", cmd.DieMatrix[x, y].Bin));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 10)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "A"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 11)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "B"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 12)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "C"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 13)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "D"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 14)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "E"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 15)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "F"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 16)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "G"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 17)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "H"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 18)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "I"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 19)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "J"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 20)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "K"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 21)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "L"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 22)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "M"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 23)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "N"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 24)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "O"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 25)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "P"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 26)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "Q"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 27)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "R"));
                                        }

                                        else if (cmd.DieMatrix[x, y].Bin == 28)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "S"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 29)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "T"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 30)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "U"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 31)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "V"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 32)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "W"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 33)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "Y"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 34)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "Z"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 35)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "a"));
                                        }

                                        else if (cmd.DieMatrix[x, y].Bin > 35)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "X"));
                                        }

                                        break;

                                    }

                            }
                        }
                        cmd.WriteString(cmd.Enter);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                cmd.CloseWriter();
            }
        }

    }
}
