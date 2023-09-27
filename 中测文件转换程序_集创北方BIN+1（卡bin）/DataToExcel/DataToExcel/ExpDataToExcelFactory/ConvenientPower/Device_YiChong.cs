using Excel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_YiChong
    {
        public static int defatultRotate()
        {
            return 0;
        }
        public static void Save(CmdTxt cmd)
        {
            try
            {
                try
                {
                    String[] split = cmd.WaferID.Split('-');
                    String waferID = split[1].Substring(0, 2);
                    int id = Int32.Parse(waferID);
                    String idString = String.Format("{0:D2}", id);

                    String lotNo = cmd.LotNo;
                    int slotId = 0;
                    if (lotNo.Contains("CP"))
                    {
                        slotId = Int32.Parse(lotNo.Substring(lotNo.IndexOf("CP") + 2)) + 1;
                        lotNo = lotNo.Substring(0, lotNo.IndexOf("CP"));
                    }
                    else
                    {
                        MessageBox.Show("TSK解析错误，TSK中批次号不包含工序CP。");
                        return;
                    }
                    cmd.FullName = ReFullName(cmd.FullName,  lotNo + "-" + slotId + "-" + idString);

                    if (File.Exists(cmd.FullName))
                    {
                        File.Delete(cmd.FullName);
                    }
                    cmd.OpenWriter();

                    String binQuanYield;

                    binQuanYield = String.Format("{0,-12}{1,2}", lotNo, idString);
                    cmd.WriteString(binQuanYield);

                    String startTime = cmd.LoadTime.ToString("yyyy-MM-dd HH:mm:ss");
                    binQuanYield = String.Format("{0,-32}{1,-4}{2,-6}{3,-8}{4,-8}{5,-30}{6,-19}", "", "", "", "N/A", "", "", startTime);
                    cmd.WriteString(binQuanYield);

                    String endTime = cmd.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string orientation;
                    //U(0),R(90),D(180),L(270);
                    //EndTime ProbleCardID LoadBoardID Bd_File Notch SortID Test_Site Fd_File
                    if (cmd.FlatDir==0)
                    {
                        orientation = "U";
                    } else if (cmd.FlatDir == 90)
                    {
                        orientation = "R";
                    } else if (cmd.FlatDir == 180)
                    {
                        orientation = "D";
                    } else
                    {
                        orientation = "L";
                    }
                    binQuanYield = String.Format("{0,-19}{1,-12}{2,-12}{3,-20}{4,-1}{5,1}{6,-8}{7,-20}", endTime, "", "", "Bd_File", orientation, slotId, "JSE","");
                    cmd.WriteString(binQuanYield);
                    cmd.WriteString(cmd.Enter);

                    int xMin = Int32.MaxValue;
                    int yMin = Int32.MaxValue;
                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {
                            if (xMin > cmd.DieMatrix[x, y].X)
                            {
                                xMin = cmd.DieMatrix[x, y].X;
                            }
                            if (yMin > cmd.DieMatrix[x, y].Y)
                            {
                                yMin = cmd.DieMatrix[x, y].Y;
                            }
                        }
                    }

                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {
                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {
                            DieData waferMapData = cmd.DieMatrix[x, y];
                            int visualInspection = 0;
                            if (cmd.DieMatrix[x, y].Attribute.Equals(DieCategory.PassDie))
                            {
                                visualInspection = 1;
                            }
                            else if (cmd.DieMatrix[x, y].Attribute.Equals(DieCategory.FailDie))
                            {
                                visualInspection = 0;
                            } else
                            {
                                continue;
                            }
                            binQuanYield = String.Format("{0,4}{1,4}{2,4}{3,4}", cmd.DieMatrix[x, y].X - xMin, cmd.DieMatrix[x, y].Y - yMin,
                                cmd.DieMatrix[x, y].Bin - 1, visualInspection);
                            cmd.WriteString(binQuanYield);
                            cmd.WriteString(cmd.Enter);
                        }
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

        public static string ReFullName(string fullName,string newFileName)
        {
            string parentPath = fullName.Substring(0,fullName.LastIndexOf(@"\"));
            string newFullName= parentPath +@"\" + newFileName + ".txt";
            return newFullName;
        }
    }
}
