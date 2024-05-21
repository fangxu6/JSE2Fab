using Excel;
using System.Windows.Forms;
using System;
using System.IO;
namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_UPA2610_8_8_CP3 : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

        }

        public override bool defatultSave()
        {
            return false;
        }

        public override bool defatultBinPlusOne()
        {
            return false;
        }

        public override void Save(CmdTxt cmd)
        {
            Device_XinHe.Save(cmd);
        }
    }
}