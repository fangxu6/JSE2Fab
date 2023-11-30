using Excel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_YiCunXin
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

                    if (File.Exists(cmd.FullName))
                    {
                        File.Delete(cmd.FullName);
                    }
                    cmd.OpenWriter();

                    cmd.WriteString("\tDevice:" + cmd.Device + cmd.Enter);
                    cmd.WriteString("\tLot NO:" + cmd.LotNo + cmd.Enter);
                    cmd.WriteString("\tSlot No: " + cmd.SlotNo + cmd.Enter);
                    cmd.WriteString("\tWafer ID:" + cmd.WaferID.Trim() + cmd.Enter);
                    string WaferSize1 = "";

                    if (cmd.WaferSize == 60)
                    {
                        WaferSize1 = "6.0 inch";
                    }
                    else if (cmd.WaferSize == 80)
                    {
                        WaferSize1 = "8.0 inch";

                    }

                    cmd.WriteString("\tWafer Size: " + WaferSize1 + cmd.Enter);


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
                        FlatDir1 = "Top";
                    }
                    cmd.WriteString("\tFlat Dir: " + FlatDir1 + cmd.Enter);
                    cmd.WriteString(cmd.Enter);

                    cmd.WriteString("[SOFT BIN MAP]" + cmd.Enter);


                    for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                    {

                        for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                        {

                            switch (cmd.DieMatrix[x, y].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        cmd.WriteString(string.Format("{0,1:G}", 1));
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", "M"));
                                        break;
                                    }
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                case DieCategory.SkipDie2:
                                    {

                                        cmd.WriteString(string.Format("{0,1:G}", "M"));
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
                    cmd.WriteString("[EXTENSION]" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("[ Wafer Bin Summary ]" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("BIN   1   =  " + cmd.PassDie + cmd.Enter);
                    cmd.WriteString("BIN   X   =  " + cmd.FailDie + cmd.Enter);
                    cmd.WriteString("***********************" + cmd.Enter);
                    cmd.WriteString("Total Die = " + cmd.TotalDie + cmd.Enter);
                    cmd.WriteString("***********************" + cmd.Enter);
                    cmd.WriteString(cmd.Enter);
                    cmd.WriteString("[EOF]" + cmd.Enter);
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
