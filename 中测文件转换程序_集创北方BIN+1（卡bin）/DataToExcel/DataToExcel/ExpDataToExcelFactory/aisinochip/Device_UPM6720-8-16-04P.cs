using Excel;
using System.Windows.Forms;
using System;
using System.IO;
namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_UPM6720_8_16_04P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

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

                    int xMin = Int32.MaxValue;
                    int yMin = Int32.MaxValue;
                    int xMax = Int32.MinValue;
                    int yMax = Int32.MinValue;
                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)//83
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)//57
                        {

                            if (cmd.DieMatrix[x, y].Attribute.Equals(DieCategory.FailDie))
                            {
                                if (xMin > x)
                                {
                                    xMin = x;
                                }
                                if (yMin > y)
                                {
                                    yMin = y;
                                }
                                if (xMax < x)
                                {
                                    xMax = x;
                                }
                                if (yMax < y)
                                {
                                    yMax = y;
                                }
                            }
                        }
                    }

                    int[] binCount = new int[64];
                    for (int i = 0; i < 64; i++)
                    {
                        binCount[i] = 0;
                    }

                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {
                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        int xxx = cmd.DieMatrix[x, y].Bin;
                                        cmd.WriteString(string.Format("{0,1:G}", "1"));
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", "#"));
                                        break;
                                    }
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", "."));
                                        break;
                                    }
                                case DieCategory.SkipDie2:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", "#"));
                                        break;
                                    }
                                case DieCategory.FailDie:
                                    {
                                        if (cmd.DieMatrix[x, y].Bin < 10)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", cmd.DieMatrix[x, y].Bin));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 10)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "A"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 11)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "B"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 12)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "C"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;

                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 13)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "D"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 14)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "E"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 15)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "F"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 16)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "G"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 17)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "H"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 18)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "I"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 19)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "J"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 20)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "K"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 21)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "L"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 22)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "M"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 23)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "N"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 24)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "O"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 25)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "P"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 26)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "Q"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 27)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "R"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 28)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "S"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 29)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "T"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 30)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "U"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 31)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "V"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 32)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "W"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 33)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "X"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 34)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "Y"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 35)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "Z"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 41)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 4f)
                                        {
                                            cmd.WriteString(string.Format("{0,1:f}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 50)
                                        {
                                            cmd.WriteString(string.Format("{0,1:i}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 51)
                                        {
                                            cmd.WriteString(string.Format("{0,1:j}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 55)
                                        {
                                            cmd.WriteString(string.Format("{0,1:n}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 56)
                                        {
                                            cmd.WriteString(string.Format("{0,1:o}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 59)
                                        {
                                            cmd.WriteString(string.Format("{0,1:r}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 60)
                                        {
                                            cmd.WriteString(string.Format("{0,1:s}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 61)
                                        {
                                            cmd.WriteString(string.Format("{0,1:t}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 62)
                                        {
                                            cmd.WriteString(string.Format("{0,1:u}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 63)
                                        {
                                            cmd.WriteString(string.Format("{0,1:v}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        break;

                                    }

                            }
                        }
                        cmd.WriteString(cmd.Enter);
                    }
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("[Product Information]" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("Product name = " + cmd.Device + cmd.Enter);
                    cmd.WriteString("Lot     name = " + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("Wafer-ID     = " + cmd.WaferID + cmd.Enter);
                    cmd.WriteString("WF Start time= " + cmd.StartTime + cmd.Enter);
                    cmd.WriteString("WF End   time= " + cmd.EndTime + cmd.Enter);
                    cmd.WriteString("X max coor.  = " + cmd.RowCount + cmd.Enter);
                    cmd.WriteString("Y max coor.  = " + cmd.ColCount + cmd.Enter);

                    string orientation;
                    if (cmd.FlatDir == 0)
                    {
                        orientation = "Up";
                    }
                    else if (cmd.FlatDir == 90)
                    {
                        orientation = "Right";
                    }
                    else if (cmd.FlatDir == 180)
                    {
                        orientation = "Down";
                    }
                    else
                    {
                        orientation = "Left";
                    }
                    cmd.WriteString("Flat         = " + orientation + cmd.Enter);
                    cmd.WriteString(cmd.Enter);

                    cmd.WriteString("[Wafer Bin Summary]" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);


                    string yield = Math.Round((double)(cmd.PassDie) / ((double)(cmd.TotalDie)), 4).ToString("0.00%");
                    cmd.WriteString("bin     1 " + String.Format("{0,8}{1,7}", cmd.PassDie , yield) + cmd.Enter);
                    cmd.WriteString("bin     5 " + String.Format("{0,8}{1,7}", binCount[5] ,Math.Round((double)(binCount[5]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     6 " + String.Format("{0,8}{1,7}", binCount[6], Math.Round((double)(binCount[6]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     7 " + String.Format("{0,8}{1,7}", binCount[7], Math.Round((double)(binCount[7]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     8 " + String.Format("{0,8}{1,7}", binCount[8], Math.Round((double)(binCount[8]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);

                    cmd.WriteString("bin  14(E)" + String.Format("{0,8}{1,7}", binCount[14],Math.Round((double)(binCount[14]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  17(H)" + String.Format("{0,8}{1,7}", binCount[17],Math.Round((double)(binCount[17]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  22(M)" + String.Format("{0,8}{1,7}", binCount[22],Math.Round((double)(binCount[22]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  24(O)" + String.Format("{0,8}{1,7}", binCount[24],Math.Round((double)(binCount[24]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  32(W)" + String.Format("{0,8}{1,7}", binCount[32],Math.Round((double)(binCount[32]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  50(o)" + String.Format("{0,8}{1,7}", binCount[50],Math.Round((double)(binCount[50]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  55(t)" + String.Format("{0,8}{1,7}", binCount[55],Math.Round((double)(binCount[55]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  63(*)" + String.Format("{0,8}{1,7}", binCount[63],Math.Round((double)(binCount[63]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);

                    cmd.WriteString("pass die :" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("fial die :" + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("total die:" + cmd.TotalDie + cmd.Enter);


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