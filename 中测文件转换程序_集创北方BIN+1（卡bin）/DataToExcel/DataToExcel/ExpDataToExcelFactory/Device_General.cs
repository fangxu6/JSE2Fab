using Excel;
using System.Windows.Forms;
using System;
using System.IO;
namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_General : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

        }

        public override bool defatultSave()
        {
            return false;
        }

        public override bool defatultBinPlusOne()
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

                    int xMin = Int32.MaxValue;
                    int yMin = Int32.MaxValue;
                    int xMax = Int32.MinValue;
                    int yMax = Int32.MinValue;
                    int siteMin = Int32.MaxValue;
                    int siteMax = Int32.MinValue;
                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {
                                case DieCategory.PassDie:
                                case DieCategory.FailDie:
                                    if (siteMax < cmd.DieMatrix[x, y].Site) { siteMax = cmd.DieMatrix[x, y].Site; }
                                    if (siteMin > cmd.DieMatrix[x, y].Site) { siteMin = cmd.DieMatrix[x, y].Site; }
                                    if (xMin > x) { xMin = x; }
                                    if (yMin > y) { yMin = y; }
                                    if (yMax < y) { yMax = y; }
                                    if (xMax < x) { xMax = x; }
                                    break;
                            }
                        }
                    }

                    int[] binCount = new int[128];
                    for (int i = 0; i < 128; i++)
                    {
                        binCount[i] = 0;
                    }

                    string formatStr1 = "", formatStr2 = "", empty = "";
                    

                    //if (yMax -yMin < 100)
                    //{
                        formatStr1 = "{0,3:G}";
                        formatStr2 = "000";
                        empty = "   ";
                    //}
                    //else if (yMax - yMin < 1000)
                    //{
                    //    formatStr1 = "{0,4:G}";
                    //    formatStr2 = "0000";
                    //    empty = "    ";
                    //}
                    //else if (yMax - yMin < 10000)
                    //{
                    //    formatStr1 = "{0,5:G}";
                    //    formatStr2 = "00000";
                    //    empty = "     ";
                    //}

                    cmd.WriteString(empty);
                    for (int i = 0; i <= xMax - xMin; i++)
                    {
                        if (i <= 99)
                            cmd.WriteString(string.Format("{0,3:G}", i.ToString("00")));
                        else if (i > 99 && i <= 999)
                            cmd.WriteString(string.Format("{0,4:G}", i.ToString("000")));
                        else if (i > 999 && i <= 9999)
                            cmd.WriteString(string.Format("{0,5:G}", i.ToString("0000")));
                    }

                    cmd.WriteString(cmd.Enter);

                    cmd.WriteString("--+");
                    for (int i = 0; i <= xMax-xMin; i++)
                    {
                        cmd.WriteString("--+");
                    }
                    cmd.WriteString("-");


                    // 写入 Die 数据
                    for (int y = yMin; y <= yMax; y++)
                    {
                        cmd.WriteString(cmd.Enter);
                        cmd.WriteString(string.Format("{0,2:G}", (y-yMin).ToString("00")));
                        cmd.WriteString("|");
                        for (int x = xMin; x <= xMax; x++)
                        {
                            switch (cmd.DieMatrix[x, y].Attribute)
                            {
                                case DieCategory.PassDie:
                                    {
                                        binCount[cmd.DieMatrix[x, y].Bin]++;
                                        cmd.WriteString(string.Format(formatStr1, "1"));
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                    {
                                        cmd.WriteString(string.Format(formatStr1, ""));
                                        break;
                                    }
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                    {
                                        cmd.WriteString(string.Format(formatStr1, ""));
                                        break;
                                    }
                                case DieCategory.SkipDie2:
                                    {
                                        cmd.WriteString(string.Format(formatStr1, ""));
                                        break;
                                    }
                                case DieCategory.FailDie:
                                    {
                                        cmd.WriteString(string.Format(formatStr1, cmd.DieMatrix[x, y].Bin));
                                        binCount[cmd.DieMatrix[x, y].Bin]++;
                                        break;
                                    }

                            }
                        }
                    }

                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("============ Wafer Information (USC) ===========" + cmd.Enter);
                    cmd.WriteString("Device Name: " + cmd.Device + cmd.Enter);
                    cmd.WriteString("Lot No.: " + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("Slot No: " + cmd.SlotNo + cmd.Enter);
                    cmd.WriteString("Wafer Id: " + cmd.WaferID + cmd.Enter);
                    cmd.WriteString("Test program: " + "" + cmd.Enter);
                    cmd.WriteString("Test NO: " + "" + cmd.Enter);
                    cmd.WriteString("Probe card_id: " + "" + cmd.Enter);
                    cmd.WriteString("Operater Name: 1" + cmd.Enter);
                    cmd.WriteString("Wafer Size: " + cmd.WaferSize +" Inch" + cmd.Enter);
                    cmd.WriteString("Flat: " + cmd.FlatDir + " degree" + cmd.Enter);
                    cmd.WriteString("Test Start Time: " + cmd.StartTime.ToString("yyMMddHHmm") + cmd.Enter);
                    cmd.WriteString("Test Finish Time: " + cmd.EndTime.ToString("yyMMddHHmm")  +cmd.Enter);
                    cmd.WriteString("Wafer Load Time: " + cmd.LoadTime.ToString("yyMMddHHmm") + cmd.Enter);
                    cmd.WriteString("Wafer Unload Time: " + cmd.UnloadTime.ToString("yyMMddHHmm") +cmd.Enter);

                    cmd.WriteString("Gross Dice: " + cmd.TotalDie + cmd.Enter);
                    cmd.WriteString("Pass Die: " + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("Fail Die: " + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("Yield: " + Math.Round((double)(cmd.PassDie) / ((double)(cmd.TotalDie)), 4).ToString("0.00%") + cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString(cmd.Enter);

                    for(int i=0;i<binCount.Length;i++)
                    {
                        cmd.WriteString("Cat " + i + ":" + " " + binCount[i]+cmd.Enter);
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