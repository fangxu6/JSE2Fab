using Excel;
using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CPS4019_8_32_01P : ExpToExcelSoftBin
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
            rngbin6.Value2 = "CP1_Bin6:";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "CP1_Bin7:";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "CP1_Bin8:";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "CP1_Bin9:";



            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "CP1_Bin11:Pass ";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "CP2_Bin12:OS";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "CP2_Bin13: ";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
            rngbin14.Value2 = "CP2_Bin14:";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
            rngbin15.Value2 = "CP2_Bin15:";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
            rngbin16.Value2 = "CP2_Bin16:";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "CP2_Bin17:Pass";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "CP2_Bin18: ";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "CP2_Bin19： ;";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 26];
            rngbin20.Value2 = "CP2_Bin20: ;";

            Excel.Range rngbin21 = (Excel.Range)worksheet.Cells[7, 27];
            rngbin21.Value2 = "CP2_Bin21: ";

            Excel.Range rngbin22 = (Excel.Range)worksheet.Cells[7, 28];
            rngbin22.Value2 = "CP3_Bin22: os";

            Excel.Range rngbin23 = (Excel.Range)worksheet.Cells[7, 29];
            rngbin23.Value2 = "CP2_Bin23: ";

            Excel.Range rngbin24 = (Excel.Range)worksheet.Cells[7, 30];
            rngbin24.Value2 = "CP2_Bin24: ";

            Excel.Range rngbin25 = (Excel.Range)worksheet.Cells[7, 31];
            rngbin25.Value2 = "CP2_Bin25: ";

            Excel.Range rngbin26 = (Excel.Range)worksheet.Cells[7, 32];
            rngbin26.Value2 = "CP2_Bin26: ";

            Excel.Range rngbin27 = (Excel.Range)worksheet.Cells[7, 33];
            rngbin27.Value2 = "CP2_Bin27: ";


            Excel.Range rngbin28 = (Excel.Range)worksheet.Cells[7, 34];
            rngbin28.Value2 = "CP2_Bin28: ";

        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            //int errflag = 0;
            ////卡bin total4809
            ////CP1:单片97% OS 0.15%  CP2:单片98% OS 0.1%  CP3:单片98% OS 0.1%
            //errflag += overQuantity(arrayHeaderInfo, 2, 7, worksheet2, num2);
            //errflag += overQuantity(arrayHeaderInfo, 12, 4, worksheet2, num2);
            //errflag += overQuantity(arrayHeaderInfo, 22, 4, worksheet2, num2);


            //if (errflag > 0)
            //{
            //    worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
            //    MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            //}
        }
        public override bool defatultSave()
        {
            return false;
        }

        public override void Save(CmdTxt cmd)
        {
            //cmd.Device = "TMPP47";
            //Device_YiChong.Save(cmd);
            try
            {
                if (File.Exists(cmd.FullName))
                {
                    File.Delete(cmd.FullName);
                }
                cmd.OpenWriter();
                int ymin = 1000;
                int xmin = 1000;
                int ymax = 0;
                int xmax = 0;
                for (int y = 0; y < cmd.DieMatrix.YMax; y++)
                {
                    for (int x = 0; x < cmd.DieMatrix.XMax; x++)
                    {
                        switch (cmd.DieMatrix[x, y].Attribute)
                        {
                            case DieCategory.PassDie:
                            case DieCategory.FailDie:
                                if (xmin > x)
                                {
                                    xmin = x;
                                }
                                if (ymin > y)
                                {
                                    ymin = y;
                                }
                                if (ymax < y)
                                {
                                    ymax = y;
                                }
                                if (xmax < x)
                                {
                                    xmax = x;
                                }
                                break;
                        }
                    }
                }
                cmd.WriteString("DEVICE:" + cmd.Device + cmd.Enter);
                cmd.WriteString("LOT:" + cmd.LotNo.Substring(0, cmd.LotNo.Length - 3) + cmd.Enter);
                cmd.WriteString("WAFER:" + cmd.WaferID + cmd.Enter);
                cmd.WriteString("FNLOC:" + cmd.FlatDir + cmd.Enter);
                cmd.WriteString("ROWCT:" + (ymax - ymin + 1) + cmd.Enter);
                cmd.WriteString("COLCT:" + (xmax - xmin + 1) + cmd.Enter);
                cmd.WriteString("BCEQU:00" + cmd.Enter);//cmd.Bcequ +
                cmd.WriteString("REFPX:7" + cmd.Enter);
                cmd.WriteString("REFPY:10" + cmd.Enter);
                cmd.WriteString("DUTMS:MM" + cmd.Enter);// cmd.Dutms +
                cmd.WriteString("XDIES:" + ((double)cmd.IndexSizeX / 100000.0).ToString("0.000000") + cmd.Enter);
                cmd.WriteString("YDIES:" + ((double)cmd.IndexSizeY / 100000.0).ToString("0.000000") + cmd.Enter);
                for (int y = ymin; y < ymax + 1; y++)
                {
                    cmd.WriteString("RowData:");
                    for (int x = xmin; x < xmax + 1; x++)
                    {
                        cmd.WriteString(DieCategoryCaption(cmd.DieMatrix[x, y].Attribute, cmd.DieMatrix[x, y].Bin));
                        cmd.WriteString(" ");
                    }
                    cmd.WriteString(cmd.Enter);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
            finally
            {
                cmd.CloseWriter();
            }
            // CPS4038A1  CP1良率变更为 单片97.8%  整批97.8%  OS<0.6%
            //CP2良率变更为 单片97.4%  整批97.4% OS<0.1% bin几还要确认下

            //CP3:良率变更为 单片97%   整批97%   OS<0.1%

        }
        private string DieCategoryCaption(DieCategory attr, int bin)
        {
            string str = "?? ";
            //return attr switch
            //{
            //    DieCategory.PassDie => "00",
            //    //CDYC新分bin要求sinf图中 bin14 15 为02
            //    DieCategory.FailDie when bin == 14 || bin == 15 => "02",
            //    DieCategory.FailDie => "01",
            //    DieCategory.SkipDie2 => "@@",
            //    _ => "__",
            //};
            switch (attr)
            {
                case DieCategory.PassDie:
                    str = "00";
                    break;
                case DieCategory.FailDie:
                    str = "01";
                    break;
                case DieCategory.SkipDie2:
                    str = "@@";
                    break;
                default:
                    str = "__";
                    break;
            }
            return str;
        }
    }
}
