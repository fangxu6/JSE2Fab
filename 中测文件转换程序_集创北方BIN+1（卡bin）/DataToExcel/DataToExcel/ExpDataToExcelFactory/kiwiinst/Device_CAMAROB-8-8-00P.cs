using Excel;
using System;
using System.IO;
using System.Windows.Forms;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_CAMAROB_8_8_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {
           
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
            Device_BiYi.Save(cmd);
        }
    }
}
