using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_BiYi
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

                    cmd.WriteString("Device: " + cmd.Device + cmd.Enter);
                    cmd.WriteString("Lot NO:" + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("Slot No:" + cmd.SlotNo.ToString("00") + cmd.Enter);
                    cmd.WriteString("Wafer ID:" + cmd.WaferID + cmd.Enter);
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
                    cmd.WriteString("Test Start Time:" + cmd.LoadTime + cmd.Enter);
                    cmd.WriteString("Test Finish Time:" + cmd.EndTime + cmd.Enter);
                    cmd.WriteString("Gross die:" + (cmd.PassDie + cmd.FailDie) + cmd.Enter);
                    cmd.WriteString("Pass Die:" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("Fail Die:" + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)((double)cmd.PassDie / ((double)(cmd.PassDie + cmd.FailDie)))), 4).ToString("0.00%") + cmd.Enter);


                    int skipDieNum = cmd.DieMatrix.DieAttributeAccurateStat(DieCategory.SkipDie2);
                    if (skipDieNum > ConstDefine.WarningSipDieNumber)
                    {
                        MessageBox.Show(string.Format("片号" + cmd.SlotNo.ToString("00") + "的skip die '#' 超过{0:d}个，请注意。", ConstDefine.WarningSipDieNumber));
                    }

                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
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
                                        cmd.WriteString(string.Format("{0,1:G}", "X"));
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
