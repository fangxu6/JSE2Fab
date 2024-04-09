using Excel;
using System.Windows.Forms;
using System;
using System.IO;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CPS4061_12_32_00 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {


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
                cmd.WriteString("BCEQU:00" +  cmd.Enter);//cmd.Bcequ +
                cmd.WriteString("REFPX:7" + cmd.Enter);
                cmd.WriteString("REFPY:10" + cmd.Enter);
                cmd.WriteString("DUTMS:MM" +  cmd.Enter);// cmd.Dutms +
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
                    if (bin == 28 || bin == 15)
                    {
                        str = "02";
                    }
                    else
                    {
                        str = "01";
                    }
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
