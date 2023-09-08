using Excel;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_THEMIS_8_8_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            Excel.Range rngbin1 = (Excel.Range)worksheet.Cells[7, 7];
            rngbin1.Value2 = "Bin1:Pass";

            Excel.Range rngbin2 = (Excel.Range)worksheet.Cells[7, 8];
            rngbin2.Value2 = "Bin2:SCAN_test";

            Excel.Range rngbin3 = (Excel.Range)worksheet.Cells[7, 9];
            rngbin3.Value2 = "Bin3:Bist_test";

            Excel.Range rngbin4 = (Excel.Range)worksheet.Cells[7, 10];
            rngbin4.Value2 = "Bin4:ICC";


            Excel.Range rngbin5 = (Excel.Range)worksheet.Cells[7, 11];
            rngbin5.Value2 = "Bin5:os_down_test";

            Excel.Range rngbin6 = (Excel.Range)worksheet.Cells[7, 12];
            rngbin6.Value2 = "Bin6:eFlash_CHIP_Init";

            Excel.Range rngbin7 = (Excel.Range)worksheet.Cells[7, 13];
            rngbin7.Value2 = "Bin7:eFlash_Isb";

            Excel.Range rngbin8 = (Excel.Range)worksheet.Cells[7, 14];
            rngbin8.Value2 = "Bin8:eFlash_Erase_Ref_Cell";

            Excel.Range rngbin9 = (Excel.Range)worksheet.Cells[7, 15];
            rngbin9.Value2 = "Bin9:eFlash_DC_Stress";

            Excel.Range rngbin10 = (Excel.Range)worksheet.Cells[7, 16];
            rngbin10.Value2 = "Bin10:eFlash_Mass_Progam";

            Excel.Range rngbin11 = (Excel.Range)worksheet.Cells[7, 17];
            rngbin11.Value2 = "Bin11:eFlash_Mass_Erase";

            Excel.Range rngbin12 = (Excel.Range)worksheet.Cells[7, 18];
            rngbin12.Value2 = "Bin12:eFlash_Program_First_6Rows";

            Excel.Range rngbin13 = (Excel.Range)worksheet.Cells[7, 19];
            rngbin13.Value2 = "Bin13:eFlash_Mass_Progam_1";

            Excel.Range rngbin14 = (Excel.Range)worksheet.Cells[7, 20];
            rngbin14.Value2 = "Bin14:eFlash_Mass_Erase_1";

            Excel.Range rngbin15 = (Excel.Range)worksheet.Cells[7, 21];
            rngbin15.Value2 = "Bin15:eFlash_Write_Disturb";

            Excel.Range rngbin16 = (Excel.Range)worksheet.Cells[7, 22];
            rngbin16.Value2 = "Bin16:eFlash_Cycling_10x";

            Excel.Range rngbin17 = (Excel.Range)worksheet.Cells[7, 23];
            rngbin17.Value2 = "Bin17:eFlash_Verify";

            Excel.Range rngbin18 = (Excel.Range)worksheet.Cells[7, 24];
            rngbin18.Value2 = "Bin18:eFlash_Weak_Erase";

            Excel.Range rngbin19 = (Excel.Range)worksheet.Cells[7, 25];
            rngbin19.Value2 = "Bin19：eFlash_Tox_Stress;";

            Excel.Range rngbin20 = (Excel.Range)worksheet.Cells[7, 26];
            rngbin20.Value2 = "Bin20:eFlash_Mass_Erase_2;";

            Excel.Range rngbin21 = (Excel.Range)worksheet.Cells[7, 27];
            rngbin21.Value2 = "Bin21:eFlash_Program_Full_Diagonal";

            Excel.Range rngbin22 = (Excel.Range)worksheet.Cells[7, 28];
            rngbin22.Value2 = "Bin22:eFlash_Verify_Diagonal";

            Excel.Range rngbin23 = (Excel.Range)worksheet.Cells[7, 29];
            rngbin23.Value2 = "Bin23：eFlash_Mass_Progam_2";

            Excel.Range rngbin24 = (Excel.Range)worksheet.Cells[7, 30];
            rngbin24.Value2 = "Bin24:eFlash_Page_Erase";

            Excel.Range rngbin25 = (Excel.Range)worksheet.Cells[7, 31];
            rngbin25.Value2 = "Bin25:eFlash_Verify_MRG_1_Read_info";

            Excel.Range rngbin26 = (Excel.Range)worksheet.Cells[7, 32];
            rngbin26.Value2 = "Bin26:eFlash_Page_Erase_1";

            Excel.Range rngbin27 = (Excel.Range)worksheet.Cells[7, 33];
            rngbin27.Value2 = "Bin27:eFlash_Verify_MRG_1_Read_main";


            Excel.Range rngbin28 = (Excel.Range)worksheet.Cells[7, 34];
            rngbin28.Value2 = "Bin28:eFlash_Mass_Erase_3";

            Excel.Range rngbin29 = (Excel.Range)worksheet.Cells[7, 35];
            rngbin29.Value2 = "Bin29:eFlash_Punch_Through";

            Excel.Range rngbin30 = (Excel.Range)worksheet.Cells[7, 36];
            rngbin30.Value2 = "Bin30:eFlash_Mass_Erase_4";

            Excel.Range rngbin31 = (Excel.Range)worksheet.Cells[7, 37];
            rngbin31.Value2 = "Bin31:eFlash_Program_0xFFFF";

            Excel.Range rngbin32 = (Excel.Range)worksheet.Cells[7, 38];
            rngbin32.Value2 = "Bin32:eFlash_Mass_Erase_5";

            Excel.Range rngbin33 = (Excel.Range)worksheet.Cells[7, 39];
            rngbin33.Value2 = "Bin33:eFlash_Program_Check_Board";

            Excel.Range rngbin34 = (Excel.Range)worksheet.Cells[7, 40];
            rngbin34.Value2 = "Bin34:eFlash_Mass_Erase_6";

            Excel.Range rngbin35 = (Excel.Range)worksheet.Cells[7, 41];
            rngbin35.Value2 = "Bin35:eFlash_Program_Check_Board_1;";

            Excel.Range rngbin36 = (Excel.Range)worksheet.Cells[7, 42];
            rngbin36.Value2 = "Bin36:eFlash_Thin_Oxide_Leak;";

            Excel.Range rngbin37 = (Excel.Range)worksheet.Cells[7, 43];
            rngbin37.Value2 = "Bin37:eFlash_Read_Disturb";

            Excel.Range rngbin38 = (Excel.Range)worksheet.Cells[7, 44];
            rngbin38.Value2 = "Bin38:eFlash_Mass_Erase_7";


            Excel.Range rngbin39 = (Excel.Range)worksheet.Cells[7, 45];
            rngbin39.Value2 = "Bin39:eFlash_Mass_Program_Single_FFFF";

            Excel.Range rngbin40 = (Excel.Range)worksheet.Cells[7, 46];
            rngbin40.Value2 = "Bin40:eFlash_Mass_Progam_3";

            Excel.Range rngbin41 = (Excel.Range)worksheet.Cells[7, 47];
            rngbin41.Value2 = "Bin41:eFlash_Mass_Erase_8";

            Excel.Range rngbin42 = (Excel.Range)worksheet.Cells[7, 48];
            rngbin42.Value2 = "Bin42:eFlash_Verify_MRG_1_Read";

            /* Excel.Range rngbin43 = (Excel.Range)worksheet.Cells[7, 49];
             rngbin43.Value2 = "Bin43:eFlash_Good_Die_Record";

             Excel.Range rngbin44 = (Excel.Range)worksheet.Cells[7, 50];
             rngbin44.Value2 = "Bin44:eFlash_Bake_Write_Verify";

             Excel.Range rngbin45 = (Excel.Range)worksheet.Cells[7, 51];
             rngbin45.Value2 = "Bin45:eFlash_CHIP_Init";

             Excel.Range rngbin46 = (Excel.Range)worksheet.Cells[7, 52];
             rngbin46.Value2 = "Bin46:eFlash_Check_Intf_Mode";

             Excel.Range rngbin47 = (Excel.Range)worksheet.Cells[7, 53];
             rngbin47.Value2 = "Bin47:eFlash_Verify_MRG1_Read_before";

             Excel.Range rngbin48 = (Excel.Range)worksheet.Cells[7, 54];
             rngbin48.Value2 = "Bin48:eFlash_Erase_Ref_Cell ";

             Excel.Range rngbin49 = (Excel.Range)worksheet.Cells[7, 55];
             rngbin49.Value2 = "Bin49:eFlash_Verify_MRG1_Read ";

             Excel.Range rngbin50 = (Excel.Range)worksheet.Cells[7, 56];
             rngbin50.Value2 = "Bin50:eFlash_Mass_Erase ";

             Excel.Range rngbin51 = (Excel.Range)worksheet.Cells[7, 57];
             rngbin51.Value2 = "Bin51:eFlash_Verify_MRG1_Read_2nd ";*/

            Excel.Range rngbin52 = (Excel.Range)worksheet.Cells[7, 58];
            rngbin52.Value2 = "Bin52: eFlash_Bake_Write_Verify_ff";

            Excel.Range rngbin55 = (Excel.Range)worksheet.Cells[7, 61];
            rngbin55.Value2 = "Bin55: eFlash_CHIP_Init";

            Excel.Range rngbin56 = (Excel.Range)worksheet.Cells[7, 62];
            rngbin56.Value2 = "Bin56: eFlash_Check_Intf_Mode";

            Excel.Range rngbin57 = (Excel.Range)worksheet.Cells[7, 63];
            rngbin57.Value2 = "Bin57: eFlash_Verify_MRG1_Read_i2c";

            Excel.Range rngbin58 = (Excel.Range)worksheet.Cells[7, 64];
            rngbin58.Value2 = "Bin58: eFlash_Erase_Ref_Cell";

            Excel.Range rngbin59 = (Excel.Range)worksheet.Cells[7, 65];
            rngbin59.Value2 = "Bin59: eFlash_Verify_MRG1_Read_i2c_1";

            Excel.Range rngbin60 = (Excel.Range)worksheet.Cells[7, 66];
            rngbin60.Value2 = "Bin60: eFlash_Mass_Erase";

            Excel.Range rngbin61 = (Excel.Range)worksheet.Cells[7, 67];
            rngbin61.Value2 = "Bin61: eFlash_Verify_MRG1_Read_2nd";

            Excel.Range rngbin62 = (Excel.Range)worksheet.Cells[7, 68];
            rngbin62.Value2 = "Bin62: eFlash_Good_Die_Record";
        }
    }
}
