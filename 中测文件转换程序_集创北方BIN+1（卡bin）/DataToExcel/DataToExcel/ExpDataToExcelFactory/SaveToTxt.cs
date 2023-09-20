using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class SaveToTxt : CmdTxt
    {
        public SaveToTxt(string file) : base(file)
        {
        }

        public override void Save() {
            try
            {
                try
                {
                    StreamWriter writer;
                    if (File.Exists(base.FullName))
                    {
                        File.Delete(base.FullName);
                    }
                    base.OpenWriter();
                    this.WriteString("     ");
                    for (int i = 0; i < base.DieMatrix.XMax; i++)
                    {
                        int num5 = i + 1;
                        this.WriteString(num5.ToString("00") + " ");
                    }
                    this.WriteString(base.Enter + "     ");
                    for (int j = 0; j < base.DieMatrix.XMax; j++)
                    {
                        this.WriteString("++-");
                    }
                    ToCountDie die = new ToCountDie();
                    for (int k = 0; k < base.DieMatrix.YMax; k++)
                    {
                        this.WriteString(base.Enter + ((k + 1)).ToString("000") + "| ");

                        for (int m = 0; m < base.DieMatrix.XMax; m++)
                        {
                            /*----old txt//////////////////////////////////
                            if (base.DieMatrix[m, k].Attribute == DieCategory.FailDie)
                            {
                                die.CountDie(base.DieMatrix[m, k].Bin);
                            }
                            this.WriteString(UtilFunction.DieCategoryCaption(base.DieMatrix[m, k].Attribute) + " ");
                        }
                              */

                            switch (base.DieMatrix[m, k].Attribute)
                            {

                                case DieCategory.PassDie:
                                    {
                                        int xxx = this.DieMatrix[m, k].Bin;
                                        this.WriteString(string.Format("{0,1:G}", this.DieMatrix[m, k].Bin.ToString("00 ")));
                                        break;
                                    }
                                case DieCategory.MarkDie:
                                case DieCategory.NoneDie:
                                case DieCategory.SkipDie:
                                case DieCategory.SkipDie2:
                                    {

                                        this.WriteString(string.Format("{0,1:G}", "   "));
                                        break;
                                    }

                                case DieCategory.FailDie:
                                    {
                                        this.WriteString(string.Format("{0,1:G}", this.DieMatrix[m, k].Bin.ToString("00 ")));
                                        break;

                                    }

                            }
                        }

                        //this.WriteString(base.Enter);
                    }
                    _singleTotalDie = base.DieMatrix.DieAttributeStat(DieCategory.TIRefFail | DieCategory.TIRefPass | DieCategory.Unknow | DieCategory.FailDie | DieCategory.PassDie);
                    this.WriteString(base.Enter + base.Enter);
                    this.WriteString("============ Wafer Information () ===========" + base.Enter);
                    this.WriteString("  Device: " + this.Device + base.Enter);
                    this.WriteString("  Lot NO: " + this.LotNo + base.Enter);
                    this.WriteString("  Slot NO: " + this.SlotNo + base.Enter);
                    this.WriteString("  Wafer ID: " + this.WaferID + base.Enter);
                    this.WriteString("  Operater: " + base.Enter);
                    this.WriteString("  Wafer Size: " + ((this.WaferSize / 10)).ToString() + "inch" + base.Enter);
                    string FlatDir1 = "";

                    if (this.FlatDir == 90)
                    {
                        FlatDir1 = "  Right";
                    }

                    else if (this.FlatDir == 180)
                    {
                        FlatDir1 = "  Down";
                    }
                    else if (this.FlatDir == 270)
                    {
                        FlatDir1 = "  Left";
                    }
                    else if (this.FlatDir == 0)
                    {
                        FlatDir1 = "  Up";
                    }
                    this.WriteString("  Flat Dir: " + FlatDir1 + base.Enter);
                    this.WriteString("  Wafer Test Start Time: " + this.StartTime + base.Enter);
                    this.WriteString("  Wafer Test Finish Time: " + this.EndTime + base.Enter);
                    this.WriteString("  Wafer Load Time: " + this.LoadTime + base.Enter);
                    this.WriteString("  Wafer Unload Time: " + this.UnloadTime + base.Enter);
                    // this.WriteString("  Total Test Die: " + _singleTotalDie + base.Enter);
                    this.WriteString("  Total Test Die: " + (this.PassDie + this.FailDie) + base.Enter);
                    this.WriteString("  Pass Die: " + this.PassDie + base.Enter);
                    this.WriteString("  Fail Die: " + this.FailDie + base.Enter);
                    this.WriteString("  Yield: " + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(this.PassDie) / ((double)(this.PassDie + this.FailDie)))), 4).ToString("0.00%") + base.Enter);
                    this.WriteString("  Rows: " + this.RowCount + base.Enter);
                    this.WriteString("  Cols: " + this.ColCount + base.Enter);
                    string path = base.FullName.Substring(0, base.FullName.LastIndexOf(@"\")) + @"\Total.txt";
                    if (File.Exists(path))
                    {
                        writer = File.AppendText(path);
                    }
                    else
                    {
                        writer = File.CreateText(path);
                    }
                    _Device = this.Device;
                    _LotNo = this.LotNo;
                    _TotalDie += _singleTotalDie;
                    _TotalPassDie += this.PassDie;
                    _TotalFailDie += this.FailDie;
                    _TotalYield = Math.Round(Convert.ToDouble((double)(Convert.ToDouble(_TotalPassDie) / ((double)_TotalDie))), 4).ToString("0.00%");
                    writer.WriteLine("============ Wafer Information () ===========");
                    writer.WriteLine("  Device: " + this.Device);
                    writer.WriteLine("  Lot NO: " + this.LotNo);
                    writer.WriteLine("  Slot NO: " + this.SlotNo);
                    writer.WriteLine("  Wafer ID: " + this.WaferID);
                    writer.WriteLine("  Operater: ");
                    writer.WriteLine("  Wafer Size: " + ((this.WaferSize / 10)).ToString() + "inch");
                    writer.WriteLine("  Flat Dir: " + this.FlatDir);
                    writer.WriteLine("  Wafer Test Start Time: " + this.StartTime);
                    writer.WriteLine("  Wafer Test Finish Time: " + this.EndTime);
                    writer.WriteLine("  Wafer Load Time: " + this.LoadTime);
                    writer.WriteLine("  Wafer Unload Time: " + this.UnloadTime);
                    //  writer.WriteLine("  Total Test Die: " + _singleTotalDie);
                    writer.WriteLine("  Total Test Die: " + this.PassDie + this.FailDie);
                    writer.WriteLine("  Pass Die: " + this.PassDie);
                    writer.WriteLine("  Fail Die: " + this.FailDie);
                    writer.WriteLine("  Yield: " + Math.Round(Convert.ToDouble((double)(Convert.ToDouble(this.PassDie) / ((double)_singleTotalDie))), 4).ToString("0.00%"));
                    writer.WriteLine("  Rows: " + this.RowCount);
                    writer.WriteLine("  Cols: " + this.ColCount);
                    writer.WriteLine("=============================================");
                    writer.WriteLine(base.Enter);
                    writer.Close();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            finally
            {
                base.CloseWriter();
            }
        }
    }
}
