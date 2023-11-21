using System;
using System.Windows.Forms;

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
                MessageBox.Show("该型号未定义，请联系IT并告知批次号。");
                throw new Exception("该型号不支持");
            }
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (ExpToExcelSoftBin)obj;

        }
    }
}
