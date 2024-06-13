using Excel;
using System.Windows.Forms;
using System;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CPS4038_8_32_01P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "CP3_Bin1:Pass";

            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "CP1_Bin2:OS";

            Excel.Range rngbin3 = (Excel.Range)worksheet.Cells[7, 9];
            rngbin3.Value2 = "CP1_Bin3: ";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "CP1_Bin4: ";


            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "CP1_Bin5: ";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "CP2_Bin6:PASS";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "CP3_Bin10:OS ";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "CP3_Bin11: ";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "CP3_Bin12: ";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "CP3_Bin13: ";

            /* Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
             rngbin14.Value2 = "Bin14:eFlash_Mass_Erase_1";

             Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
             rngbin15.Value2 = "Bin15:eFlash_Write_Disturb";

             Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
             rngbin16.Value2 = "Bin16:eFlash_Cycling_10x";
             */
            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "CP1_Bin17:PASS";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "CP2_Bin18: OS";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "CP2_Bin19： ;";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 26];
            rngbin20.Value2 = "CP2_Bin20: ;";

            Excel.Range rngbin21 = (Excel.Range)worksheet.Cells[7, 27];
            rngbin21.Value2 = "CP2_Bin21: ";

            Excel.Range rngbin22 = (Excel.Range)worksheet.Cells[7, 28];
            rngbin22.Value2 = "CP2_Bin22: ";

            Excel.Range rngbin24 = (Excel.Range)worksheet.Cells[7, 30];
            rngbin24.Value2 = "CP2_Bin24: ";

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
            cmd.Device = "TMNS01";
            //Device_YiChong_General.Save(cmd);
            Device_YiChong.Save(cmd);
        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            int errflag = 0;
            //卡bin
            // CPS4038A1  CP1良率变更为 单片97.8%  整批97.8%  OS<0.6%
            //CP2良率变更为 单片97.4%  整批97.4% OS<0.1% 
            //CP3:良率变更为 单片97%   整批97%   OS<0.1%
            //片良率

            if (arrayHeaderInfo[0].ToString().Contains("CP1") && Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.978)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 3], worksheet2.Cells[(num2 + 1) + 8, 3]).Interior.ColorIndex = 7;
                errflag++;
            }
            errflag += overYield(arrayHeaderInfo, 2, 0.006, worksheet2, num2);


            if (arrayHeaderInfo[0].ToString().Contains("CP2") && Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.974)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 11], worksheet2.Cells[(num2 + 1) + 8, 11]).Interior.ColorIndex = 7;
                errflag++;
            }
            errflag += overYield(arrayHeaderInfo, 10, 0.001, worksheet2, num2);


            if (arrayHeaderInfo[0].ToString().Contains("CP3") && Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.970)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 19], worksheet2.Cells[(num2 + 1) + 8, 19]).Interior.ColorIndex = 7;
                errflag++;
            }
            errflag += overYield(arrayHeaderInfo, 18, 0.001, worksheet2, num2);


            if (errflag > 0)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
                MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            }
        }

    }
}
