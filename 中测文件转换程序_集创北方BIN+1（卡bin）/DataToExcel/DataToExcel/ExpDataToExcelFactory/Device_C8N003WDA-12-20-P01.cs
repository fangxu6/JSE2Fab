using Excel;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_C8N003WDA_12_20_P01 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:OPEN_SHORT_FAIL";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD_FAIL";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:R_CLK_FAIL";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:IIH_FAIL";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin9.Value2 = "Bin9:IDD_LDO_REGU_FAIL";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin10.Value2 = "Bin10:FUNC_nor_FAIL";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin11.Value2 = "Bin11:FUNC_PLL_FAIL";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin12.Value2 = "Bin12:FUNC_mbist_FAIL";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin13.Value2 = "Bin13:FUNC_mbist_2V_FAIL";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin14.Value2 = "Bin14:FUNC_mbist_1P4V_FAIL";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin15.Value2 = "Bin15:SDO_5V_FAIL";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin16.Value2 = "Bin16:IOUT_27K_FAIL";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin17.Value2 = "Bin17:SKEW_27K_FAIL";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin18.Value2 = "Bin18:IOUT_27K_AVE_FAIL";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin19.Value2 = "Bin19:VR_DN_FAIL";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin20.Value2 = "Bin20:LEAKAGE_ad1_FAIL";
        }
    }
}
