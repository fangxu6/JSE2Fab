using Excel;
using System.Windows.Forms;
using System;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_THEMIS_8_8_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

        }

        public override bool defatultSave()
        {
            return false;
        }

        public override void Save(CmdTxt cmd)
        {
            cmd.Device = "TMNS01";//TODO 
            Device_JieLian.Save(cmd);
        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        public override void showErrorMessage(object[] arrayHeaderInfo, Excel.Worksheet worksheet2, int num2)
        {
            int errflag = 0;
            //卡bin
            errflag += overYield(arrayHeaderInfo, 2, 0.0225, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 5, 0.0133, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 62, 0.01, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 10, 0.0082, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 3, 0.0070, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 33, 0.0065, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 4, 0.0075, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 55, 0.0065, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 11, 0.0026, worksheet2, num2);
            errflag += overYield(arrayHeaderInfo, 57, 0.0033, worksheet2, num2);

            //片良率 下限
            if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) < 0.95)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 5], worksheet2.Cells[(num2 + 1) + 8, 5]).Interior.ColorIndex = 7;
                errflag++;
            }
            // 上限
            if (Convert.ToDouble(arrayHeaderInfo[2]) / Convert.ToDouble(arrayHeaderInfo[1]) >0.999)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 5], worksheet2.Cells[(num2 + 1) + 8, 5]).Interior.ColorIndex = 7;
                errflag++;
            }


            if (errflag > 0)
            {
                worksheet2.get_Range(worksheet2.Cells[(num2 + 1) + 8, 1], worksheet2.Cells[(num2 + 1) + 8, 1]).Interior.ColorIndex = 7;
                MessageBox.Show(arrayHeaderInfo[0].ToString() + "--SBL超标,请检查图谱是否有问题");
            }

            return;
        }

    }
        
}
