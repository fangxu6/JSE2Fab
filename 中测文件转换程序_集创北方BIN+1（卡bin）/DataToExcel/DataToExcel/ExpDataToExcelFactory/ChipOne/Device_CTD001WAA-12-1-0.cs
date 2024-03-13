using Excel;
using System.Windows.Forms;
using System;
using System.IO;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CTD001WAA_12_1_0 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "Bin2:OPEN_SHORT_FAIL";

            Excel.Range rngbin3 = (Excel.Range)worksheet.Cells[7, 9];
            rngbin3.Value2 = "Bin3:POWER_OS_FAIL";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "Bin4:LEAKAGE_FAIL";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:VIHL_FAIL";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:VOHL_FAIL";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:ATPG_FAIL";
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
                    cmd.WriteString("Wafer Type:" + "CP1" + cmd.Enter);
                    string WaferSize1 = "";

                    if (cmd.WaferSize == 60)
                    {
                        WaferSize1 = "6";
                    }
                    else if (cmd.WaferSize == 80)
                    {
                        WaferSize1 = "8 ";

                    }

                    else if (cmd.WaferSize == 120)
                    {
                        WaferSize1 = "12";

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
                        cmd.WriteString("PASS BIN:1" + cmd.Enter);
                    }
                    cmd.WriteString("Test Start Time:" + cmd.LoadTime.ToString("yyyy/MM/dd HH:mm:ss") + cmd.Enter);
                    cmd.WriteString("Test End Time:" + cmd.EndTime.ToString("yyyy/MM/dd HH:mm:ss") + cmd.Enter);


                    cmd.WriteString("Gross die:" + (cmd.PassDie + cmd.FailDie) + cmd.Enter);
                    cmd.WriteString("Pass Die:" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("Fail Die:" + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)(cmd.PassDie / ((double)(cmd.PassDie + cmd.FailDie)))), 6).ToString("0.00%") + cmd.Enter);
                    cmd.WriteString("StrBin:1,1;2,2;3,3;4,4;5,5;6,6;7,7;60,X;" + cmd.Enter);


                    for (int y = ymin; y < ymax + 1; y++)
                    //  for (int y = 0; y < cmd.DieMatrix.YMax-1; y++)
                    {

                        for (int x = xmin; x < xmax + 1; x++)
                        //  for (int x = 0; x < cmd.DieMatrix.XMax; x++)
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
                                        else if (cmd.DieMatrix[x, y].Bin > 20)
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