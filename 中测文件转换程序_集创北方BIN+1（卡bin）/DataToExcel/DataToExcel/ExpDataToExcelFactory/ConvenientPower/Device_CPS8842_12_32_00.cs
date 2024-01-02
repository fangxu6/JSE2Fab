using Excel;
using System.Windows.Forms;
using System;
using System.IO;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CPS8842_12_32_00 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            
        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            //批≥99%  片≥99%  BIN 3<0.1%  BIN4<0.5%  BIN 5<0.15%  BIN 6<0.5%  BIN 7<0.5%  BIN8<0.15%
            int errflag = 0;
            errflag += equalOrOverYield(arrayHeaderInfo, 3, 0.001, worksheet2, num2);
            errflag += equalOrOverYield(arrayHeaderInfo, 4, 0.005, worksheet2, num2);
            errflag += equalOrOverYield(arrayHeaderInfo, 5, 0.0015, worksheet2, num2);
            errflag += equalOrOverYield(arrayHeaderInfo, 6, 0.005, worksheet2, num2);
            errflag += equalOrOverYield(arrayHeaderInfo, 7, 0.005, worksheet2, num2);
            errflag += equalOrOverYield(arrayHeaderInfo, 8, 0.0015, worksheet2, num2);

            //片良率
            if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) <0.99)
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
    }
}
