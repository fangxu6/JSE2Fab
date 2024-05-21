using Excel;
using System.Windows.Forms;
using System;
using System.IO;
namespace DataToExcel.ExpDataToExcelFactory
{
    public class Device_UPM6722_12_8_00P : ExpToExcelSoftBin
    {
        public override void expToExcel(Worksheet worksheet)
        {

        }

        public override int defatultRotate()
        {
            return 90;
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