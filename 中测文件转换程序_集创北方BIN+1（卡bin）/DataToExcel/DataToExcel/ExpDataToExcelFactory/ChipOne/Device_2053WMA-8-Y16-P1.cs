using Excel;
using System.IO;
using System;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2053WMA_8_Y16_P1 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "Bin2:Pass";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:OPEN_SHORT";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:OPEN_SHORT_VDD";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:R_CLK";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:IIH";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "Bin9:FUN_MBIST_2V";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "Bin10:FUN_MBIST_1P8V";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "Bin11:FUN_MBIST_1P5V";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "Bin12:FUN_ATPG";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "Bin13:FUN_NORM";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
            rngbin14.Value2 = "Bin14:IDD_REXT_12K";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
            rngbin15.Value2 = "Bin15:LEAKAGE_AD1";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
            rngbin16.Value2 = "Bin16:IOUT_12K";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "Bin17:SKEW_12K";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "Bin18:IOUT_12K_AVE";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "Bin19:IOUT_12K_1";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 26];
            rngbin20.Value2 = "Bin20:SKEW_12K_1";

            Excel.Range rngbin21 = (Excel.Range)worksheet.Cells[7, 27];
            rngbin21.Value2 = "Bin21:IOUT_12K_AVE_1";

            Excel.Range rngbin22 = (Excel.Range)worksheet.Cells[7, 28];
            rngbin22.Value2 = "Bin22:IOUT_2P2K";

            Excel.Range rngbin23 = (Excel.Range)worksheet.Cells[7, 29];
            rngbin23.Value2 = "Bin23:SKEW_2P2K";

            Excel.Range rngbin24 = (Excel.Range)worksheet.Cells[7, 30];
            rngbin24.Value2 = "Bin24:IOUT_2P2K_AVE";

        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            int errflag = 0;
            //卡bin21
            errflag += overQuantity(arrayHeaderInfo, 21, 10, worksheet2, num2);
           

            if (errflag > 0)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
                MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            }
        }




        public override int defatultRotate()
        {
            return Device_2053WMA.defatultRotate();
        }

        public override bool defatultSave()
        {
            return false;
        }

        public override void Save(CmdTxt cmd)
        {
            Device_2053WMA.Save(cmd);
        }
    }
}
