using Excel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_XinHe
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

                    string orientation = "";
                    if (cmd.FlatDir == 0)
                    {
                        cmd.DeasilRotate(180);
                        orientation = "Down";
                    }
                    else if (cmd.FlatDir == 90)
                    {
                        cmd.DeasilRotate(90);
                        orientation = "Down";
                    }
                    else if (cmd.FlatDir == 180)
                    {
                        orientation = "Down";
                    }
                    else
                    {
                        cmd.DeasilRotate(270);
                        orientation = "Down";
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

                    int[] binCount = new int[64];
                    for (int i = 0; i < 64; i++)
                    {
                        binCount[i] = 0;
                    }

                    for (int y = yMin; y <= yMax; y++)
                    {
                        for (int x = xMin; x <= xMax; x++)
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

                                        else if (cmd.DieMatrix[x, y].Bin == 36)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "a"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 37)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "b"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 38)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "c"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;

                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 39)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "d"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 40)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "e"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 41)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "f"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 42)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "g"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 43)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "h"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 44)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "i"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 45)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "j"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 46)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "k"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 47)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "l"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 48)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "m"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 49)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "n"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 50)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "o"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 51)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "p"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 52)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "q"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 53)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "r"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 54)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "s"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 55)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "t"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 56)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "u"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 57)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "v"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 58)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "w"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 59)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "x"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 60)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "y"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 61)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "z"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin == 62)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "`"));
                                            binCount[cmd.DieMatrix[x, y].Bin]++;
                                        }
                                        else if (cmd.DieMatrix[x, y].Bin >= 63)
                                        {
                                            cmd.WriteString(string.Format("{0,1:G}", "*"));
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
                    int rowCount = yMax - yMin + 1;
                    int colCount = xMax - xMin + 1;
                    cmd.WriteString("[Product Information]" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("Product name = " + cmd.Device.Split('-')[0] + cmd.Enter);
                    cmd.WriteString("Lot     name = " + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("Wafer-ID     = " + cmd.WaferID + cmd.Enter);
                    cmd.WriteString("WF Start time= " + cmd.StartTime + cmd.Enter);
                    cmd.WriteString("WF End   time= " + cmd.EndTime + cmd.Enter);
                    cmd.WriteString("X max coor.  = " + colCount + cmd.Enter);//列数
                    cmd.WriteString("Y max coor.  = " + rowCount + cmd.Enter);//行数


                    cmd.WriteString("Flat         = " + orientation + cmd.Enter);
                    cmd.WriteString(cmd.Enter);

                    cmd.WriteString("[Wafer Bin Summary]" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);


                    string yield = Math.Round((double)(cmd.PassDie) / ((double)(cmd.TotalDie)), 4).ToString("0.00%");
                    cmd.WriteString("bin     1 " + String.Format("{0,8}{1,7}", cmd.PassDie, yield) + cmd.Enter);
                    cmd.WriteString("bin     2 " + String.Format("{0,8}{1,7}", binCount[2], Math.Round((double)(binCount[2]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     3 " + String.Format("{0,8}{1,7}", binCount[3], Math.Round((double)(binCount[3]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     4 " + String.Format("{0,8}{1,7}", binCount[4], Math.Round((double)(binCount[4]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     5 " + String.Format("{0,8}{1,7}", binCount[5], Math.Round((double)(binCount[5]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     6 " + String.Format("{0,8}{1,7}", binCount[6], Math.Round((double)(binCount[6]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     7 " + String.Format("{0,8}{1,7}", binCount[7], Math.Round((double)(binCount[7]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     8 " + String.Format("{0,8}{1,7}", binCount[8], Math.Round((double)(binCount[8]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin     9 " + String.Format("{0,8}{1,7}", binCount[9], Math.Round((double)(binCount[9]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  10(A)" + String.Format("{0,8}{1,7}", binCount[10], Math.Round((double)(binCount[10]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  11(B)" + String.Format("{0,8}{1,7}", binCount[11], Math.Round((double)(binCount[11]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  12(C)" + String.Format("{0,8}{1,7}", binCount[12], Math.Round((double)(binCount[12]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  13(D)" + String.Format("{0,8}{1,7}", binCount[13], Math.Round((double)(binCount[13]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  14(E)" + String.Format("{0,8}{1,7}", binCount[14], Math.Round((double)(binCount[14]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  15(F)" + String.Format("{0,8}{1,7}", binCount[15], Math.Round((double)(binCount[15]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  16(G)" + String.Format("{0,8}{1,7}", binCount[16], Math.Round((double)(binCount[16]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  17(H)" + String.Format("{0,8}{1,7}", binCount[17], Math.Round((double)(binCount[17]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  18(I)" + String.Format("{0,8}{1,7}", binCount[18], Math.Round((double)(binCount[18]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  19(J)" + String.Format("{0,8}{1,7}", binCount[19], Math.Round((double)(binCount[19]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  20(K)" + String.Format("{0,8}{1,7}", binCount[20], Math.Round((double)(binCount[20]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  21(L)" + String.Format("{0,8}{1,7}", binCount[21], Math.Round((double)(binCount[21]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  22(M)" + String.Format("{0,8}{1,7}", binCount[22], Math.Round((double)(binCount[22]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  23(N)" + String.Format("{0,8}{1,7}", binCount[23], Math.Round((double)(binCount[23]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  24(O)" + String.Format("{0,8}{1,7}", binCount[24], Math.Round((double)(binCount[24]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  25(P)" + String.Format("{0,8}{1,7}", binCount[25], Math.Round((double)(binCount[25]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  26(Q)" + String.Format("{0,8}{1,7}", binCount[26], Math.Round((double)(binCount[26]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  27(R)" + String.Format("{0,8}{1,7}", binCount[27], Math.Round((double)(binCount[27]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  28(S)" + String.Format("{0,8}{1,7}", binCount[28], Math.Round((double)(binCount[28]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  29(T)" + String.Format("{0,8}{1,7}", binCount[29], Math.Round((double)(binCount[29]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  30(U)" + String.Format("{0,8}{1,7}", binCount[30], Math.Round((double)(binCount[30]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  31(V)" + String.Format("{0,8}{1,7}", binCount[31], Math.Round((double)(binCount[31]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  32(W)" + String.Format("{0,8}{1,7}", binCount[32], Math.Round((double)(binCount[32]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  33(X)" + String.Format("{0,8}{1,7}", binCount[33], Math.Round((double)(binCount[33]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  34(Y)" + String.Format("{0,8}{1,7}", binCount[34], Math.Round((double)(binCount[34]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  35(Z)" + String.Format("{0,8}{1,7}", binCount[35], Math.Round((double)(binCount[35]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  36(a)" + String.Format("{0,8}{1,7}", binCount[36], Math.Round((double)(binCount[36]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  37(b)" + String.Format("{0,8}{1,7}", binCount[37], Math.Round((double)(binCount[37]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  38(c)" + String.Format("{0,8}{1,7}", binCount[38], Math.Round((double)(binCount[38]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  39(d)" + String.Format("{0,8}{1,7}", binCount[39], Math.Round((double)(binCount[39]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  40(e)" + String.Format("{0,8}{1,7}", binCount[40], Math.Round((double)(binCount[40]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  41(f)" + String.Format("{0,8}{1,7}", binCount[41], Math.Round((double)(binCount[41]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  42(g)" + String.Format("{0,8}{1,7}", binCount[42], Math.Round((double)(binCount[42]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  43(h)" + String.Format("{0,8}{1,7}", binCount[43], Math.Round((double)(binCount[43]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  44(i)" + String.Format("{0,8}{1,7}", binCount[44], Math.Round((double)(binCount[44]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  45(j)" + String.Format("{0,8}{1,7}", binCount[45], Math.Round((double)(binCount[45]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  46(k)" + String.Format("{0,8}{1,7}", binCount[46], Math.Round((double)(binCount[46]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  47(l)" + String.Format("{0,8}{1,7}", binCount[47], Math.Round((double)(binCount[47]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  48(m)" + String.Format("{0,8}{1,7}", binCount[48], Math.Round((double)(binCount[48]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  49(n)" + String.Format("{0,8}{1,7}", binCount[49], Math.Round((double)(binCount[49]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  50(o)" + String.Format("{0,8}{1,7}", binCount[50], Math.Round((double)(binCount[50]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  51(p)" + String.Format("{0,8}{1,7}", binCount[51], Math.Round((double)(binCount[51]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  52(q)" + String.Format("{0,8}{1,7}", binCount[52], Math.Round((double)(binCount[52]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  53(r)" + String.Format("{0,8}{1,7}", binCount[53], Math.Round((double)(binCount[53]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  54(s)" + String.Format("{0,8}{1,7}", binCount[54], Math.Round((double)(binCount[54]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  55(t)" + String.Format("{0,8}{1,7}", binCount[55], Math.Round((double)(binCount[55]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  56(u)" + String.Format("{0,8}{1,7}", binCount[56], Math.Round((double)(binCount[56]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  57(v)" + String.Format("{0,8}{1,7}", binCount[57], Math.Round((double)(binCount[57]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  58(w)" + String.Format("{0,8}{1,7}", binCount[58], Math.Round((double)(binCount[58]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  59(x)" + String.Format("{0,8}{1,7}", binCount[59], Math.Round((double)(binCount[59]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  60(y)" + String.Format("{0,8}{1,7}", binCount[60], Math.Round((double)(binCount[60]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  61(z)" + String.Format("{0,8}{1,7}", binCount[61], Math.Round((double)(binCount[61]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  62(`)" + String.Format("{0,8}{1,7}", binCount[62], Math.Round((double)(binCount[62]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);
                    cmd.WriteString("bin  63(*)" + String.Format("{0,8}{1,7}", binCount[63], Math.Round((double)(binCount[63]) / ((double)(cmd.TotalDie)), 4).ToString("0.00%")) + cmd.Enter);

                    cmd.WriteString("pass die :" + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("fail die :" + cmd.FailDie + cmd.Enter);
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

        public static string ReFullName(string fullName,string newFileName)
        {
            string parentPath = fullName.Substring(0,fullName.LastIndexOf(@"\"));
            string newFullName= parentPath +@"\" + newFileName + ".txt";
            return newFullName;
        }
    }
}
