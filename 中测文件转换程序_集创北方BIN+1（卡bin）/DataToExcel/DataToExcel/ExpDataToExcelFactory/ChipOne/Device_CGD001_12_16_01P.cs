using Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CGD001_12_16_01P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:OPEN_SHORT";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:FUN_pulldown";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:RIN/LEAKAGE_in";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:LEAKAGE_out";
        }

        public override bool defatultSave()
        {
            return false;
        }

        public override int defatultRotate()
        {
            return 270;
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


                    int xMin = Int32.MaxValue;
                    int yMin = Int32.MaxValue;
                    int xMax = Int32.MinValue;
                    int yMax = Int32.MinValue;
                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {
                                case DieCategory.PassDie:
                                case DieCategory.FailDie:
                                case DieCategory.SkipDie2:
                                    if (xMin > x) { xMin = x; }
                                    if (yMin > y) { yMin = y; }
                                    if (yMax < y) { yMax = y; }
                                    if (xMax < x) { xMax = x; }
                                    break;
                            }
                        }
                    }

                    for (int y = yMin; y <= yMax; y++)
                    {
                        for (int x = xMin; x <= xMax; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        cmd.WriteString(string.Format("{0,3:G}", "P"));
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                case DieCategory.SkipDie2:
                                    {
                                        cmd.WriteString(string.Format("{0,3:G}", UtilFunction.DieCategoryCaption(cmd.DieMatrix[x, y].Attribute)));
                                        break;
                                    }

                                case DieCategory.FailDie:
                                    {

                                        cmd.WriteString(string.Format("{0,3:G}", "F"));
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
