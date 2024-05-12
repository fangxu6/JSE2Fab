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
                //易冲特殊处理，原tsk没有型号
                //clazzNmae="DataToExcel.ExpDataToExcelFactory.Device_UPM6720_8_16_04P";
                //o = Type.GetType(clazzNmae);
                //通用转换
                //MessageBox.Show("该型号在笑脸整合软件中未定义，将使用通用格式txt图谱。");
                if (name.Contains("CPS4061"))
                {
                    clazzNmae = "DataToExcel.ExpDataToExcelFactory.Device_CPS4061_12_32_00";
                } else
                {
                    clazzNmae = "DataToExcel.ExpDataToExcelFactory.Device_General";
                }
                o = Type.GetType(clazzNmae);
                //MessageBox.Show("该型号未定义，请联系IT并告知批次号。");
                //throw new Exception("该型号不支持");
            }
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (ExpToExcelSoftBin)obj;

        }
    }
}
