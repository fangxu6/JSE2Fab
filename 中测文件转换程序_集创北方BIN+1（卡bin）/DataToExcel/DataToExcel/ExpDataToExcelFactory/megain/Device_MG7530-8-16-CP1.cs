using Excel;
using System.Windows.Forms;
using System;
using System.IO;
using System.Drawing.Printing;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_MG7530_8_16_CP1 : ExpToExcelSoftBin
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

                    int BIN1 = 0, BIN2 = 0, BIN3 = 0, BIN4 = 0, BIN5 = 0, BIN6 = 0, BIN7 = 0, BIN8 = 0, BIN9 = 0, BIN10 = 0, BIN11 = 0, BIN12 = 0, BIN13 = 0, BIN14 = 0, BIN15 = 0, BIN16 = 0, BIN17 = 0, BIN18 = 0,
                    BIN19 = 0, BIN20 = 0, BIN21 = 0, BIN22 = 0, BIN23 = 0, BIN24 = 0, BIN25 = 0, BIN26 = 0, BIN27 = 0, BIN28 = 0, BIN29 = 0, BIN30 = 0, BIN31 = 0, BIN32 = 0;

                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {
                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        switch (cmd.DieMatrix[x, y].Bin)
                                        {
                                            case 1: { BIN1++; break; }
                                            case 2: { BIN2++; break; }

                                        }
                                        break;
                                    }

                            }


                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.FailDie:
                                    {
                                        switch (cmd.DieMatrix[x, y].Bin)
                                        {

                                            case 3: { BIN3++; break; }
                                            case 4: { BIN4++; break; }
                                            case 5: { BIN5++; break; }
                                            case 6: { BIN6++; break; }
                                            case 7: { BIN7++; break; }
                                            case 8: { BIN8++; break; }
                                            case 9: { BIN9++; break; }
                                            case 10: { BIN10++; break; }
                                            case 11: { BIN11++; break; }
                                            case 12: { BIN12++; break; }
                                            case 13: { BIN13++; break; }
                                            case 14: { BIN14++; break; }
                                            case 15: { BIN15++; break; }
                                            case 16: { BIN16++; break; }
                                            case 17: { BIN17++; break; }
                                            case 18: { BIN18++; break; }
                                            case 19: { BIN19++; break; }
                                            case 20: { BIN20++; break; }
                                            case 21: { BIN21++; break; }
                                            case 22: { BIN22++; break; }
                                            case 23: { BIN23++; break; }
                                            case 24: { BIN24++; break; }
                                            case 25: { BIN25++; break; }
                                            case 26: { BIN26++; break; }
                                            case 27: { BIN27++; break; }
                                            case 28: { BIN28++; break; }
                                            case 29: { BIN29++; break; }
                                            case 30: { BIN30++; break; }
                                            case 31: { BIN31++; break; }
                                            case 32: { BIN32++; break; }
                                        }
                                        break;
                                    }

                            }
                        }
                    }


                    string[] newwaferid = cmd.WaferID.Split(new char[] { '-' });

                    // 打开或创建文件
                    cmd.OpenWriter();
                    string[] b = cmd.Device.Split(new char[] { '-' }, StringSplitOptions.None);
                    cmd.WriteString("[BOF]" + "\r\n");
                    cmd.WriteString("PRODUCT ID      : " + cmd.Operator + "\r\n");
                    cmd.WriteString("LOT ID          : " + cmd.LotNo + "\r\n");
                    // cmd.WriteString("WAFER ID        :" + cmd.WaferID + "\r\n");
                    cmd.WriteString("WAFER ID        : " + newwaferid[1] + "\r\n");
                    cmd.WriteString("FLOW ID         : " + "CP3" + "\r\n");
                    cmd.WriteString("START TIME      : " + cmd.LoadTime.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n");
                    cmd.WriteString("STOP TIME       : " + cmd.UnloadTime.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n");
                    cmd.WriteString("SUBCON          : " + "JSE" + "\r\n");
                    cmd.WriteString("TESTER NAME     : " + "ACCO" + "\r\n");
                    cmd.WriteString("TEST PROGRAM    : " + cmd.Device + "\r\n");
                    cmd.WriteString("LOAD BOARD ID   : " + cmd.Device + "-1" + "\r\n");
                    cmd.WriteString("PROBE CARD ID   : " + cmd.Device + "-1" + "\r\n");
                    cmd.WriteString("SITE NUM        : " + "8" + "\r\n");
                    cmd.WriteString("DUT ID          : " + "\r\n");
                    cmd.WriteString("DUT DIFF NUM    : " + "\r\n");
                    cmd.WriteString("OPERATOR ID     : " + "\r\n");
                    cmd.WriteString("TESTED DIE      : " + cmd.TotalDie + "\r\n");
                    cmd.WriteString("PASS DIE        : " + cmd.PassDie + "\r\n");
                    cmd.WriteString("YIELD           : " + Math.Round(((double)cmd.PassDie / (double)(cmd.TotalDie)), 4).ToString("0.00%") + "\r\n");
                    if (cmd.FlatDir == 90)
                    {
                        //DeasilRotate(90);
                        cmd.WriteString("SOURCE NOTCH    : " + "RIGHT" + "\r\n");
                    }
                    if (cmd.FlatDir == 0)
                    {
                        //cmd.DeasilRotate(180);
                        cmd.WriteString("SOURCE NOTCH    : " + "UP" + "\r\n");
                    }
                    if (cmd.FlatDir == 180)
                    {
                        cmd.WriteString("SOURCE NOTCH    : " + "DOWN" + "\r\n");
                    }
                    if (cmd.FlatDir == 270)
                    {
                        //DeasilRotate(270);
                        cmd.WriteString("SOURCE NOTCH    : " + "LEFT" + "\r\n");
                    }
                    cmd.WriteString("MAP ROW         : " + cmd.DieMatrix.YMax + "\r\n");
                    cmd.WriteString("MAP COLUMN      : " + cmd.DieMatrix.XMax + "\r\n");
                    cmd.WriteString("MAP BIN LENGTH  : " + "1" + "\r\n");
                    cmd.WriteString("SHIP            : " + "YSE" + "\r\n");
                    cmd.WriteString("\r\n");
                    cmd.WriteString("[SOFT BIN]" + "\r\n");
                    cmd.WriteString("               BINNAME,    DIENUM,  YIELD,   DESCRIPTION" + "\r\n");
                    cmd.WriteString("   BIN,        1," + BIN1.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN1 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + ",  {[GOODBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        2," + BIN2.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN2 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[GOODBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        3," + BIN3.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN3 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        4," + BIN4.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN4 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        5," + BIN5.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN5 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        6," + BIN6.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN6 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        7," + BIN7.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN7 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        8," + BIN8.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN8 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,        9," + BIN9.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN9 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       10," + BIN10.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN10 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       11," + BIN11.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN11 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       12," + BIN12.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN12 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       13," + BIN13.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN13 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       14," + BIN14.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN14 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       15," + BIN15.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN15 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       16," + BIN16.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN16 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       17," + BIN17.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN17 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       18," + BIN18.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN18 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       19," + BIN19.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN19 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       20," + BIN20.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN20 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       21," + BIN21.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN21 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       22," + BIN22.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN22 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       23," + BIN23.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN23 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       24," + BIN24.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN24 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       25," + BIN25.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN25 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       26," + BIN26.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN26 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       27," + BIN27.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN27 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       28," + BIN28.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN28 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       29," + BIN29.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN29 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       30," + BIN30.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN30 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       31," + BIN31.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN31 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");
                    cmd.WriteString("   BIN,       32," + BIN32.ToString().PadLeft(15, ' ') + "," + Math.Round(((double)BIN32 / (double)(cmd.TotalDie)), 4).ToString("0.00%").PadLeft(9, ' ') + "," + "  {[FAILBIN]}" + "\r\n");

                    cmd.WriteString("[SOFT BIN MAP]" + "\r\n");
                    int a1, a2, a3;
                    cmd.WriteString("   ");
                    for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                    {

                        a1 = ((x + 1) / 100) % 10;
                        cmd.WriteString(a1.ToString());
                        //a2 = x / 10 - 10 * x;
                        //a3 = x % 10;
                    }
                    cmd.WriteString("\r\n");

                    cmd.WriteString("   ");
                    for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                    {
                        a2 = ((x + 1) / 10) % 10;
                        cmd.WriteString(a2.ToString());

                    }
                    cmd.WriteString("\r\n");
                    cmd.WriteString("   ");

                    for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                    {
                        a3 = (x + 1) % 10;
                        cmd.WriteString(a3.ToString());
                    }

                    // 写入 Die 数据
                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        cmd.WriteString("\r\n");
                        cmd.WriteString((y + 1).ToString("000"));

                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        switch (cmd.DieMatrix[x, y].Bin)
                                        {
                                            case 23:
                                                cmd.WriteString(string.Format("{0,1:G}", "X"));
                                                break;
                                            case 24:
                                                cmd.WriteString(string.Format("{0,1:G}", "Y"));
                                                break;
                                            default:
                                                {

                                                    cmd.WriteString(string.Format("{0,1:G}", cmd.DieMatrix[x, y].Bin));
                                                    break;

                                                }
                                        }
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                case DieCategory.NoneDie:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(cmd.DieMatrix[x, y].Attribute)));
                                        break;
                                    }
                                case DieCategory.SkipDie:
                                    {
                                        cmd.WriteString(string.Format("{0,1:G}",  " "));
                                        break;
                                    }
                                case DieCategory.FailDie:
                                    {


                                        //    cmd.WriteString(string.Format("{0,1:G}", UtilFunction.DieCategoryCaption(cmd.DieMatrix[x, y].Attribute)));
                                        //    break;
                                        switch (cmd.DieMatrix[x, y].Bin)
                                        {

                                            case 2:
                                            case 3:
                                            case 4:
                                            case 5:
                                            case 6:
                                            case 7:
                                            case 8:
                                            case 9:

                                                cmd.WriteString(string.Format("{0,1:G}", cmd.DieMatrix[x, y].Bin));
                                                break;


                                            case 10:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "A"));
                                                    break;

                                                }
                                            case 11:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "B"));
                                                    break;

                                                }
                                            case 12:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "C"));
                                                    break;

                                                }
                                            case 13:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "D"));
                                                    break;

                                                }
                                            case 14:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "E"));
                                                    break;

                                                }
                                            case 15:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "F"));
                                                    break;

                                                }
                                            case 16:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "G"));
                                                    break;

                                                }
                                            case 17:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "H"));
                                                    break;

                                                }
                                            case 18:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "I"));
                                                    break;

                                                }
                                            case 19:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "J"));
                                                    break;

                                                }
                                            case 20:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "K"));
                                                    break;

                                                }
                                            case 21:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "L"));
                                                    break;

                                                }
                                            case 22:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "M"));
                                                    break;


                                                }
                                            case 23:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "N"));
                                                    break;

                                                }
                                            case 24:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "O"));
                                                    break;

                                                }
                                            case 25:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "P"));
                                                    break;

                                                }
                                            case 26:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "Q"));
                                                    break;

                                                }
                                            case 27:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "R"));
                                                    break;

                                                }
                                            case 28:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "S"));
                                                    break;

                                                }
                                            case 29:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "T"));
                                                    break;

                                                }
                                            case 30:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "U"));
                                                    break;

                                                }
                                            case 31:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "V"));
                                                    break;

                                                }
                                            case 32:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "W"));
                                                    break;

                                                }

                                            case 33:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "X"));
                                                    break;

                                                }
                                            case 34:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "Y"));
                                                    break;

                                                }
                                            case 35:
                                                {
                                                    cmd.WriteString(string.Format("{0,1:G}", "Z"));
                                                    break;

                                                }

                                            default:
                                                {

                                                    cmd.WriteString(string.Format("{0,1:G}", "F"));
                                                    break;

                                                }
                                        }

                                        break;
                                    }

                            }


                        }
                    }
                    cmd.WriteString("\r\n" + "[EXTENSION]" + "\r\n");
                    cmd.WriteString("\r\n" + "[EOF]" + "\r\n");

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