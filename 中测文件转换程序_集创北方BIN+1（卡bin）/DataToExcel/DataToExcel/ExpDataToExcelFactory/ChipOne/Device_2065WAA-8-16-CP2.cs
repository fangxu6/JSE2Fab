using Excel;
using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2065WAA_8_16_CP2 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "CP1_Bin2:Pass";

            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "CP1_Bin5:OS";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "CP1_Bin6:OS_VDD";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "CP1_Bin7:R_CLK";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "CP1_Bin8:IIH";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "CP1_Bin9:FUN_SDO";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "CP1_Bin10:Leakage_1";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "CP1_Bin11:FUN_NORM";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "CP1_Bin12:FUN_Mbist";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "CP1_Bin13:FUN_Mbist2";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
            rngbin14.Value2 = "CP1_Bin14:FUN_Mbist1";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
            rngbin15.Value2 = "CP1_Bin15:IDD_REXT_12K";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
            rngbin16.Value2 = "CP1_Bin16:IDD_LDO_REGU";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "CP1_Bin17:IOUT_12K";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "CP1_Bin18:SKEW_12K";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "CP1_Bin19:IOUT_12K_AVE";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 26];
            rngbin20.Value2 = "CP1_Bin20:IOUT_12K_1";

            Excel.Range rngbin21 = (Excel.Range)worksheet.Cells[7, 27];
            rngbin21.Value2 = "CP1_Bin21:SKEW_12K_1";

            Excel.Range rngbin22 = (Excel.Range)worksheet.Cells[7, 28];
            rngbin22.Value2 = "CP1_Bin22:IOUT_12K_AVE_1";

            Excel.Range rngbin23 = (Excel.Range)worksheet.Cells[7, 29];
            rngbin23.Value2 = "CP2_Bin23:OS";

            Excel.Range rngbin24 = (Excel.Range)worksheet.Cells[7, 30];
            rngbin24.Value2 = "CP2_Bin24:OS_VDD";

            Excel.Range rngbin25 = (Excel.Range)worksheet.Cells[7, 31];
            rngbin25.Value2 = "CP2_Bin25:IDD_REXT_12K";

            Excel.Range rngbin26 = (Excel.Range)worksheet.Cells[7, 32];
            rngbin26.Value2 = "CP2_Bin26:R_CLK";

            Excel.Range rngbin27 = (Excel.Range)worksheet.Cells[7, 33];
            rngbin27.Value2 = "CP2_Bin27:IIH";

            Excel.Range rngbin28 = (Excel.Range)worksheet.Cells[7, 34];
            rngbin28.Value2 = "CP2_Bin28:FUN_SDO";

            Excel.Range rngbin29 = (Excel.Range)worksheet.Cells[7, 35];
            rngbin29.Value2 = "CP2_Bin29:LEAKAGE_ad1";

            Excel.Range rngbin30 = (Excel.Range)worksheet.Cells[7, 36];
            rngbin30.Value2 = "CP2_Bin30:FUN_NORM";

            Excel.Range rngbin31 = (Excel.Range)worksheet.Cells[7, 37];
            rngbin31.Value2 = "CP2_Bin31:FUN_Mbist";

            Excel.Range rngbin32 = (Excel.Range)worksheet.Cells[7, 38];
            rngbin32.Value2 = "CP2_Bin32:FUN_Mbist2";

            Excel.Range rngbin33 = (Excel.Range)worksheet.Cells[7, 39];
            rngbin33.Value2 = "CP2_Bin33:FUN_Mbist1";

            Excel.Range rngbin34 = (Excel.Range)worksheet.Cells[7, 40];
            rngbin34.Value2 = "CP2_Bin34:IDD_LDO_REGU";

            Excel.Range rngbin35 = (Excel.Range)worksheet.Cells[7, 41];
            rngbin35.Value2 = "CP2_Bin35:IDD_REXT_12K_0P1S";

            Excel.Range rngbin36 = (Excel.Range)worksheet.Cells[7, 42];
            rngbin36.Value2 = "CP2_Bin36:R_CLK_0P1S";

            Excel.Range rngbin37 = (Excel.Range)worksheet.Cells[7, 43];
            rngbin37.Value2 = "CP2_Bin37:IIH_0P1S";

            Excel.Range rngbin38 = (Excel.Range)worksheet.Cells[7, 44];
            rngbin38.Value2 = "CP2_Bin38:FUN_SDO_0P1S";

            Excel.Range rngbin39 = (Excel.Range)worksheet.Cells[7, 45];
            rngbin39.Value2 = "CP2_Bin39:LEAKAGE_ad1_0P1S";

            Excel.Range rngbin40 = (Excel.Range)worksheet.Cells[7, 46];
            rngbin40.Value2 = "CP2_Bin40:FUN_NORM_0P1S";

            Excel.Range rngbin41 = (Excel.Range)worksheet.Cells[7, 47];
            rngbin41.Value2 = "CP2_Bin41:FUN_Mbist_0P1S";

            Excel.Range rngbin42 = (Excel.Range)worksheet.Cells[7, 48];
            rngbin42.Value2 = "CP2_Bin42:FUN_Mbist2_0P1S";

            Excel.Range rngbin43 = (Excel.Range)worksheet.Cells[7, 49];
            rngbin43.Value2 = "CP2_Bin43:FUN_Mbist1_0P1S";

            Excel.Range rngbin44 = (Excel.Range)worksheet.Cells[7, 50];
            rngbin44.Value2 = "CP2_Bin44:IDD_LDO_REGU_0P1S";

        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            int errflag = 0;
            //卡bin
            if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.985)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 5], worksheet2.Cells[(num2 + 1) + 8, 5]).Interior.ColorIndex = 7;
                errflag++;
            }
            errflag += overQuantity(arrayHeaderInfo, 23, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 24, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 25, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 26, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 27, 20, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 28, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 29, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 30, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 31, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 32, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 33, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 34, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 35, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 36, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 37, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 38, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 39, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 40, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 41, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 42, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 43, 9, worksheet2, num2);
            errflag += overQuantity(arrayHeaderInfo, 44, 9, worksheet2, num2);

            if (errflag > 0)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
                MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            }
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
