using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToExcel.ExpDataToExcelFactory
{
    public abstract class ExpToExcelSoftBin
    {
        public abstract void expToExcel(Excel.Worksheet worksheet);
    }
}
