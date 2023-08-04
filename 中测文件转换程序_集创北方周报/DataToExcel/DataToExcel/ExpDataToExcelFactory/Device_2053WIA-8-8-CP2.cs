using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2053WIA_8_8_CP2 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "CP2_Bin1:Pass";

            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "CP1_Bin2:Pass";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "CP1_Bin4:OS_PMU";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "CP1_Bin5:OS_PMU_VDD";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "CP1_Bin6:FUN_sdo";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "CP1_Bin7:FUN_mbist";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "CP1_Bin8:FUN_mbist2";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "CP1_Bin9:FUN_mbist1";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "CP1_Bin10:IOUT_12K";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "CP1_Bin11:SKEW_12K";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "CP1_Bin12:LEAKAGE_1";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "CP1_Bin13:IOUT_12K_AVE";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
            rngbin14.Value2 = "CP2_Bin14:OS_PMU";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
            rngbin15.Value2 = "CP2_Bin15:OS_PMU_VDD";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
            rngbin16.Value2 = "CP2_Bin16:FUN_mbist";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "CP2_Bin17:FUN_mbist2";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "CP2_Bin18:FUN_mbist1";

        }
    }
}
