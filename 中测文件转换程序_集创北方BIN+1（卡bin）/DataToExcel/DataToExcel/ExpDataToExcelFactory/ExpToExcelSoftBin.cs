using System;
using System.Collections.Generic;
using System.Text;

namespace DataToExcel.ExpDataToExcelFactory
{
    public abstract class ExpToExcelSoftBin
    {
        public abstract void expToExcel(Excel.Worksheet worksheet);
    }
}
