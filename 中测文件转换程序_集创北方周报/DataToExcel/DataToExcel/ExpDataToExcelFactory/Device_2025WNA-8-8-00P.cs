using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2025WNA_8_8_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "Bin4:OPEN_SHORT";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:OPEN_SHORT_VDD";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:FUN_all_3V";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:IOUT_1P5K";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "Bin9:IOUT_1P5K_AVE";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "Bin10:FUN_all_6V";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "Bin12:LEAKAGE_1";
        }
    }
}
