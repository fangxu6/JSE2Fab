using Excel;
using System.Windows.Forms;
using System;
using System.IO;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CPS8602_8_32_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "CP3_Bin1:Pass";

            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "CP1_Bin2:OS";

            Excel.Range rngbin3 = (Excel.Range)worksheet.Cells[7, 9];
            rngbin3.Value2 = "CP1_Bin3: ";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "CP1_Bin4: ";


            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "CP1_Bin5: ";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "CP2_Bin6:PASS";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "CP3_Bin10:OS ";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "CP3_Bin11: ";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "CP3_Bin12: ";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "CP3_Bin13: ";

            /* Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
             rngbin14.Value2 = "Bin14:eFlash_Mass_Erase_1";

             Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
             rngbin15.Value2 = "Bin15:eFlash_Write_Disturb";

             Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
             rngbin16.Value2 = "Bin16:eFlash_Cycling_10x";
             */
            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "CP1_Bin17:PASS";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "CP2_Bin18: OS";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "CP2_Bin19： ;";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 26];
            rngbin20.Value2 = "CP2_Bin20: ;";

            Excel.Range rngbin21 = (Excel.Range)worksheet.Cells[7, 27];
            rngbin21.Value2 = "CP2_Bin21: ";

            Excel.Range rngbin22 = (Excel.Range)worksheet.Cells[7, 28];
            rngbin22.Value2 = "CP2_Bin22: ";

            Excel.Range rngbin24 = (Excel.Range)worksheet.Cells[7, 30];
            rngbin24.Value2 = "CP2_Bin24: ";

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

                String[] split = cmd.WaferID.Split('-');
                String waferID = split[1].Substring(0, 2);
                int id = Int32.Parse(waferID);
                String idString = String.Format("{0:D2}", id);

                String lotNo = cmd.LotNo;
                if (lotNo.Contains("CP"))
                {
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

                cmd.WriteString(cmd.Operator + cmd.Enter);
                cmd.WriteString(lotNo + cmd.Enter);
                cmd.WriteString(idString + cmd.Enter);

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
                cmd.WriteString("Notch:" + orientation + cmd.Enter);

                cmd.WriteString("Yield:" + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(cmd.PassDie) / ((double)(cmd.PassDie + cmd.FailDie)))), 6).ToString("0.0000%"));

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

                for (int y = yMin; y <= yMax; y++)
                {
                    cmd.WriteString(cmd.Enter);
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
                                    cmd.WriteString(string.Format("{0,1:G}", "X"));
                                    break;

                                }

                        }
                    }
                }
                cmd.WriteString(cmd.Enter);

                cmd.WriteString("Total die count: " + (cmd.PassDie + cmd.FailDie) + cmd.Enter);
                cmd.WriteString("Good die count: " + cmd.PassDie);

            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                cmd.CloseWriter();
            }
        }
        // CPS4038A1  CP1良率变更为 单片97.8%  整批97.8%  OS<0.6%
        //CP2良率变更为 单片97.4%  整批97.4% OS<0.1% bin几还要确认下

        //CP3:良率变更为 单片97%   整批97%   OS<0.1%

    }
}
