using Excel;
using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_2053WMA_8_Y16_P2 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

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


            Excel.Range rngbin25 = (Excel.Range)worksheet.Cells[7, 31];
            rngbin25.Value2 = "Bin25:OPEN_SHORT";

            Excel.Range rngbin26 = (Excel.Range)worksheet.Cells[7, 32];
            rngbin26.Value2 = "Bin26:OPEN_SHORT_VDD";

            Excel.Range rngbin27 = (Excel.Range)worksheet.Cells[7, 33];
            rngbin27.Value2 = "Bin27:R_CLK";

            Excel.Range rngbin28 = (Excel.Range)worksheet.Cells[7, 34];
            rngbin28.Value2 = "Bin28:IIH";

            Excel.Range rngbin29 = (Excel.Range)worksheet.Cells[7, 35];
            rngbin29.Value2 = "Bin29:FUN_MBIST_2V";

            Excel.Range rngbin30 = (Excel.Range)worksheet.Cells[7, 36];
            rngbin30.Value2 = "Bin30:FUN_MBIST_1P8V";

            Excel.Range rngbin31 = (Excel.Range)worksheet.Cells[7, 37];
            rngbin31.Value2 = "Bin31:FUN_MBIST_1P5V";

            Excel.Range rngbin32 = (Excel.Range)worksheet.Cells[7, 38];
            rngbin32.Value2 = "Bin32:FUN_ATPG";

            Excel.Range rngbin33 = (Excel.Range)worksheet.Cells[7, 39];
            rngbin33.Value2 = "Bin33:FUN_NORM";

            Excel.Range rngbin34 = (Excel.Range)worksheet.Cells[7, 40];
            rngbin34.Value2 = "Bin34:IDD_REXT_12K";

            Excel.Range rngbin35 = (Excel.Range)worksheet.Cells[7, 41];
            rngbin35.Value2 = "Bin35:LEAKAGE_AD1";


        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            int errflag = 0;
            //卡bin
          
                errflag += overNumber(arrayHeaderInfo, 30, 114, worksheet2, num2);

            //if (Convert.ToInt32(arrayHeaderInfo[30]) > 114) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 31], worksheet2.Cells[(num2 + 1) + 8, 31]).Interior.ColorIndex = 7; flagbin++; }//bin25
            //if (Convert.ToInt32(arrayHeaderInfo[31]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 32], worksheet2.Cells[(num2 + 1) + 8, 32]).Interior.ColorIndex = 7; flagbin++; }//bin26
            //if (Convert.ToInt32(arrayHeaderInfo[32]) > 85) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 33], worksheet2.Cells[(num2 + 1) + 8, 33]).Interior.ColorIndex = 7; flagbin++; }//bin27
            //if (Convert.ToInt32(arrayHeaderInfo[33]) > 156) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 34], worksheet2.Cells[(num2 + 1) + 8, 34]).Interior.ColorIndex = 7; flagbin++; }//bin28
            //if (Convert.ToInt32(arrayHeaderInfo[34]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 35], worksheet2.Cells[(num2 + 1) + 8, 35]).Interior.ColorIndex = 7; flagbin++; }//bin29
            //if (Convert.ToInt32(arrayHeaderInfo[35]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 36], worksheet2.Cells[(num2 + 1) + 8, 36]).Interior.ColorIndex = 7; flagbin++; }//bin30
            //if (Convert.ToInt32(arrayHeaderInfo[36]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 37], worksheet2.Cells[(num2 + 1) + 8, 37]).Interior.ColorIndex = 7; flagbin++; }//bin31
            //if (Convert.ToInt32(arrayHeaderInfo[37]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 38], worksheet2.Cells[(num2 + 1) + 8, 38]).Interior.ColorIndex = 7; flagbin++; }//bin32
            //if (Convert.ToInt32(arrayHeaderInfo[38]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 39], worksheet2.Cells[(num2 + 1) + 8, 39]).Interior.ColorIndex = 7; flagbin++; }//bin33
            //if (Convert.ToInt32(arrayHeaderInfo[39]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 40], worksheet2.Cells[(num2 + 1) + 8, 40]).Interior.ColorIndex = 7; flagbin++; }//bin34
            //if (Convert.ToInt32(arrayHeaderInfo[40]) > 17) { worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 41], worksheet2.Cells[(num2 + 1) + 8, 41]).Interior.ColorIndex = 7; flagbin++; }//bin35



            //片良率
            if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.985)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 5], worksheet2.Cells[(num2 + 1) + 8, 5]).Interior.ColorIndex = 7;
                errflag++;
            }

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
