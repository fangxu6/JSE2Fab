using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_ICNC66_12_8_01P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "Bin2:OS_PMU";

            Excel.Range rngbin3 = (Excel.Range)worksheet.Cells[7, 9];
            rngbin3.Value2 = "Bin3:OS_PWR";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "Bin4:LEAK_TEST";


            /*   Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
               rngbin5.Value2 = "Bin5:VIHL_TEST";*/

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:VOHL_TEST ";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:VBG_TEST";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:IDDQ_TEST";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "Bin9:ATPG_TEST ";
        }
    }
}
