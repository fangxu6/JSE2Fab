using Excel;
using System.Windows.Forms;
using System;
using System.Text;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_GT5224A_8_72_CP2 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        //public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        //{
        //    int errflag = 0;
        //    //卡bin
        //    //bin 2 12 22
        //    //BIN5<0.5%,BIN8<1.89%,BIN13<2.29%
        //    errflag += overYield(arrayHeaderInfo, 5, 0.005, worksheet2, num2);
        //    errflag += overYield(arrayHeaderInfo, 8, 0.0189, worksheet2, num2);
        //    errflag += overYield(arrayHeaderInfo, 13, 0.0229, worksheet2, num2);

        //    //片良率 下限
        //    if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <= 0.9629)
        //    {
        //        worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 5], worksheet2.Cells[(num2 + 1) + 8, 5]).Interior.ColorIndex = 7;
        //        errflag++;
        //    }

        //    if (errflag > 0)
        //    {
        //        worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
        //        MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
        //    }

        //    return;
        //}

    }
}
