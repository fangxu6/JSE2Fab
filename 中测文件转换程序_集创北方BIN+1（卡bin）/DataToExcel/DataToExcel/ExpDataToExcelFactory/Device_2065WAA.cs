using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2065WAA
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

                    cmd.WriteString("Device: " + cmd.Operator + cmd.Enter);
                    cmd.WriteString("Lot NO:" + cmd.LotNo.Replace("CP2", "").Replace("CP1", "").Replace("CP3", "") + cmd.Enter);
                    cmd.WriteString("Slot No:" + cmd.SlotNo.ToString("00") + cmd.Enter);
                    cmd.WriteString("Wafer ID:" + cmd.WaferID.Replace("CP2", "").Replace("CP1", "").Replace("CP3", "") + cmd.Enter);
                    string WaferSize1 = "";

                    if (cmd.WaferSize == 60)
                    {
                        WaferSize1 = "  6 Inch";
                    }
                    else if (cmd.WaferSize == 80)
                    {
                        WaferSize1 = "  8 Inch";

                    }
                    else if (cmd.WaferSize == 120)
                    {
                        WaferSize1 = "  12 Inch";

                    }

                    cmd.WriteString("Wafer Size: " + WaferSize1 + cmd.Enter);

                    string FlatDir1 = "";

                    if (cmd.FlatDir == 90)
                    {
                        FlatDir1 = "  Right";
                    }

                    else if (cmd.FlatDir == 180)
                    {
                        FlatDir1 = "  Down";
                    }
                    else if (cmd.FlatDir == 270)
                    {
                        FlatDir1 = "  Left";
                    }
                    else if (cmd.FlatDir == 0)
                    {
                        FlatDir1 = "  Top";
                    }

                    cmd.WriteString("Flat Dir: " + FlatDir1 + cmd.Enter);
                    cmd.WriteString("Wafer Test Start Time:" + cmd.LoadTime + cmd.Enter);
                    cmd.WriteString("Wafer Test Finish Time:" + cmd.EndTime + cmd.Enter);
                    cmd.WriteString("Total test die:" + (cmd.PassDie + cmd.FailDie) + cmd.Enter);
                    cmd.WriteString("Pass Die:" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("Fail Die:" + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)(cmd.PassDie / ((double)(cmd.PassDie + cmd.FailDie)))), 4).ToString("0.0000%") + cmd.Enter);


                    int skipDieNum = cmd.DieMatrix.DieAttributeAccurateStat(DieCategory.SkipDie2);
                    if (skipDieNum > ConstDefine.WarningSipDieNumber)
                    {
                        MessageBox.Show(string.Format("片号" + cmd.SlotNo.ToString("00") + "的skip die '#' 超过{0:d}个，请注意。", ConstDefine.WarningSipDieNumber));
                    }

                    for (int y = 0; y < cmd.DieMatrix.YMax - 1; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
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
                                        else if (cmd.DieMatrix[x, y].Bin > 55)
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
