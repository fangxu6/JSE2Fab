using Excel;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2047WAA_8_8_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin3 = (Excel.Range)worksheet.Cells[7, 9];
            rngbin3.Value2 = "Bin3:OS_PMU";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "Bin4:OS_PMU_VDD";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:FUN_sdo";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:IOUT_12K";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:SKEW_12K";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "Bin10:IOUT_12K_AVE";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "Bin11:ft_open_debug";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "Bin12:LEAKAGE_1";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "Bin13:OS_PMU_1";
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
            Device_2065WAA.Save(cmd);
        }
    }
    
}
