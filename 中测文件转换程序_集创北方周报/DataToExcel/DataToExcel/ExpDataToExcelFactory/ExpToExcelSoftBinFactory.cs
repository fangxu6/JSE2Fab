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
        //    if (name.Equals("ICND2056-8-4-CP2"))
        //    {
        //        return new Device_ICND2056_8_4_CP2();
        //    }
        //    else if (name.Equals("ICND2056-8-4-00P"))
        //    {
        //        return new Device_ICND2056_8_4_00P();
        //    }
        //    else if (name.Equals("2019WCA-8-8-00P"))
        //    {
        //        return new Device_2019WCA_8_8_00P();
        //    }
        //    else if (name.Equals("2019WCA-8-16-00P"))
        //    {
        //        return new Device_2019WCA_8_16_00P();
        //    }
        //    else if (name.Equals("2018WCA-8-32-00P"))
        //    {
        //        return new Device_2018WCA_8_32_00P();
        //    }
        //    else if (name.Equals("2025WMA-8-8-00P"))
        //    {
        //        return new Device_2025WMA_8_8_00P();
        //    }
        //    else if (name.Equals("2025WNA-8-8-00P"))
        //    {
        //        return new Device_2025WNA_8_8_00P();
        //    }
        //    else if (name.Equals("2025WNA-8-16-00P"))
        //    {
        //        return new Device_2025WNA_8_16_00P();
        //    }
        //    else if (name.Equals("2047WAA-8-8-00P"))
        //    {
        //        return new Device_2047WAA_8_8_00P();
        //    }
        //    else if (name.Equals("2047WBA-8-8-04P"))
        //    {
        //        return new Device_2047WBA_8_8_04P();
        //    }
        //    else if (name.Equals("2053WIA-8-8-CP1"))
        //    {
        //        return new Device_2053WIA_8_8_CP1();
        //    }
        //    else if (name.Equals("2053WIA-8-8-CP2"))
        //    {
        //        return new Device_2053WIA_8_8_CP2();
        //    }
        //    else if (name.Equals("2065WCB-8-8-CP1"))
        //    {
        //        return new Device_2065WCB_8_8_CP1();
        //    }
        //    else if (name.Equals("2065WCB-8-8-CP2"))
        //    {
        //        return new Device_2065WCB_8_8_CP2();
        //    }
        //    else if (name.Equals("2065WCB-8-16-CP1"))
        //    {
        //        return new Device_2065WCB_8_16_CP1();
        //    }
        //    else if (name.Equals("2065WCB-8-16-CP2"))
        //    {
        //        return new Device_2065WCB_8_16_CP2();
        //    }
        //    else if (name.Equals("2065WAA-8-8-CP1"))
        //    {
        //        return new Device_2065WAA_8_8_CP1();
        //    }
        //    else if (name.Equals("2065WAA-8-8-CP2"))
        //    {
        //        return new Device_2065WAA_8_8_CP2();
        //    }
        //    else if (name.Equals("2065WAA-8-16-CP1"))
        //    {
        //        return new Device_2065WAA_8_16_CP1();
        //    }
        //    else if (name.Equals("2065WAA-8-Y16-P1"))
        //    {
        //        return new Device_2065WAA_8_Y16_P1();
        //    }
        //    else if (name.Equals("2065WAA-8-16-CP2"))
        //    {
        //        return new Device_2065WAA_8_16_CP2();
        //    }
        //    else if (name.Equals("2065WAA-8-Y16-P2"))
        //    {
        //        return new Device_2065WAA_8_Y16_P2();
        //    }
        //    else if (name.Equals("2065WEB-12-16-00"))
        //    {
        //        return new Device_2065WEB_12_16_00();
        //    }
        //    else if (name.Equals("2065WEB-12-16-01"))
        //    {
        //        return new Device_2065WEB_12_16_01();
        //    }
        //    else if (name.Equals("THEMIS-8-8-00P"))
        //    {
        //        return new Device_THEMIS_8_8_00P();
        //    }
        //    else if (name.Equals("ICNC66-12-8-01P"))
        //    {
        //        return new Device_ICNC66_12_8_01P();
        //    }
        //    else if (name.Equals("2012WRA-12-16-00"))
        //    {
        //        return new Device_2012WRA_12_16_00();
        //    }
        //    else if (name.Equals("2053WMA-8-16-CP1"))
        //    {
        //        return new Device_2053WMA_8_16_CP1();
        //    }
        //    else if (name.Equals("2053WMA-8-Y16-P1"))
        //    {
        //        return new Device_2053WMA_8_Y16_P1();
        //    }
        //    else if (name.Equals("2053WMA-8-16-CP2"))
        //    {
        //        return new Device_2053WMA_8_16_CP2();
        //    }
        //    else if (name.Equals("2053WMA-8-Y16-P2"))
        //    {
        //        return new Device_2053WMA_8_Y16_P2();
        //    }
        //    else if (name.Equals("HS5154-8-8-00P"))
        //    {
        //        return new Device_HS5154_8_8_00P();
        //    }
        //    else if (name.Equals("CPS4038-8-32-00P"))
        //    {
        //        return new Device_CPS4038_8_32_00P();
        //    }
        //    else if (name.Equals("CPS4019-8-32-01P"))
        //    {
        //        return new Device_CPS4019_8_32_01P();
        //    }

        //    return null;

        }
    }
}
