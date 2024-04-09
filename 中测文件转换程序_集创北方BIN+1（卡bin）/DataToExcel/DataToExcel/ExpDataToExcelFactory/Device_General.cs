using Excel;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_General : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
            
        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            //int errflag = 0;
            ////卡bin total4809
            ////CP1:单片97% OS 0.15%  CP2:单片98% OS 0.1%  CP3:单片98% OS 0.1%
            //errflag += overQuantity(arrayHeaderInfo, 2, 7, worksheet2, num2);
            //errflag += overQuantity(arrayHeaderInfo, 12, 4, worksheet2, num2);
            //errflag += overQuantity(arrayHeaderInfo, 22, 4, worksheet2, num2);


            //if (errflag > 0)
            //{
            //    worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
            //    MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            //}
        }
       
    }
}
