using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataToExcel.ExpDataToExcelFactory
{
    public class ExpToExcelSoftBinFactory
    {
        public static ExpToExcelSoftBin GetExpToExcelSoft(String name)
        {
            //后期if else改为反射 //命名空间.类型名,程序集
            String clazzNmae = "DataToExcel.ExpDataToExcelFactory." + "Device_" + name.Replace("-", "_");
            Type o = Type.GetType(clazzNmae);
            if (o == null)
            {
                return null;
            }
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (ExpToExcelSoftBin)obj;

        }
    }
}
