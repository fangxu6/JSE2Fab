using Excel;
using System.Windows.Forms;
using System;
using System.IO;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CMPS2001_8_16_00 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

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

                    cmd.WriteString("Test House:JSE" + cmd.Enter);
                    cmd.WriteString("Customer Name:WXLW" + cmd.Enter);
                    cmd.WriteString("Device Name:" + cmd.Operator + cmd.Enter);

                    string WaferSize1 = "";
                    if (cmd.WaferSize == 60)
                    {
                        WaferSize1 = "6.0Inch";
                    }
                    else if (cmd.WaferSize == 80)
                    {
                        WaferSize1 = "8.0Inch";
                    }
                    else if (cmd.WaferSize == 120)
                    {
                        WaferSize1 = "12Inch";
                    }

                    cmd.WriteString("Wafer Size:" + WaferSize1 + cmd.Enter);
                    cmd.WriteString("LOT:" + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("SLOT:" + cmd.SlotNo.ToString("00") + cmd.Enter);
                    cmd.WriteString("Good die:" + cmd.PassDie + cmd.Enter);

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
                    cmd.WriteString("Orientation:" + FlatDir1 + cmd.Enter);

                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
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
                                        cmd.DieMatrix[x, y].Bin = cmd.DieMatrix[x, y].Bin;
                                        //cmd.WriteString(string.Format("{0,1:G}", "X"));
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
                                        else if (cmd.DieMatrix[x, y].Bin == 36)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "b"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 37)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "c"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 38)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "d"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 39)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "e"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 40)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "f"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 41)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "g"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 42)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "h"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 43)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "i"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 44)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "j"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 45)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "k"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 46)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "l"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 47)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "m"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 48)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "n"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 49)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "o"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 50)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "p"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 51)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "q"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 52)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "r"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 53)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "s"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 54)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "t"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 55)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "u"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 56)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "v"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 57)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "w"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 58)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "x"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 59)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "y"));
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 60)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "X"));
                                        }
                                        else
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "`"));
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