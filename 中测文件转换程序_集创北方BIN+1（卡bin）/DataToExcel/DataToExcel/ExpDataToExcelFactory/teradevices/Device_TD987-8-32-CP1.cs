using Excel;
using System.Windows.Forms;
using System;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_TD987_8_32_CP1 : ExpToExcelSoftBin
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
            Device_YiCunXin.Save(cmd);
        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

    }
        
}
