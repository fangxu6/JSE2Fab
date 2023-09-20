using Excel;
using System;
using System.IO;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2053WMA
    {
        public static void Save(CmdTxt cmd)
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

                    cmd.WriteString("Device:" + cmd.Device + cmd.Enter);
                    cmd.WriteString("Lot NO:" + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("Wafer ID:" + cmd.SlotNo.ToString("00") + cmd.Enter);
                    string WaferSize1 = "";

                    if (cmd.WaferSize == 60)
                    {
                        WaferSize1 = "6 Inch";
                    }
                    else if (cmd.WaferSize == 80)
                    {
                        WaferSize1 = "8 Inch";

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

                    cmd.WriteString("Flat Dir:" + FlatDir1 + cmd.Enter);
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
                    cmd.WriteString("Test Program:ICND2053WLA_CP2_16D_ST2564_V06_HT;" + cmd.Enter);
                    cmd.WriteString("Tester ID:ST2564-04" + cmd.Enter);
                    cmd.WriteString("Operator ID:" + cmd.Enter);
                    cmd.WriteString("Sort ID:" + cmd.Enter);
                    cmd.WriteString("Test site: 16" + cmd.Enter);
                    cmd.WriteString("Probe Card ID:ICND2053WLA-8-16-P02" + cmd.Enter);
                    cmd.WriteString("Load Board ID:" + cmd.Enter);


                    cmd.WriteString("Gross die:" + (cmd.PassDie + cmd.FailDie) + cmd.Enter);
                    cmd.WriteString("Pass Die:" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("Fail Die:" + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)(cmd.PassDie / ((double)(cmd.PassDie + cmd.FailDie)))), 6).ToString("0.0000%") + cmd.Enter);
                    cmd.WriteString("StrBin:1,1;2,2;3,3;4,4;5,5;6,6;7,7;8,8;9,9;10,A;11,B;12,C;13,D;14,E;15,F;16,G;17,H;18,I;19,J;20,K;21,L;22,M;23,N;24,O;25,P;26,Q;27,R;28,S;29,T;30,U;31,V;32,W;33,Y;34,Z;35,a;60,X;" + cmd.Enter);


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
