using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TSK_MERGE_SINF.Template;

namespace TSK_MERGE_SINF.Template
{
    public class DeviceFactory
    {
        public static IncomingFileToTskTemplate GetDeviceFromTsk(String name)
        {
            //后期if else改为反射 //命名空间.类型名,程序集
            String clazzNmae = "TSK_MERGE_SINF.Template." + "Device_" + name.Replace("-", "_");
            Type o = Type.GetType(clazzNmae);
            if (o == null)
            {
                clazzNmae = "TSK_MERGE_SINF.Template." + "Device_BZ1610_8_16_00P";
                o = Type.GetType(clazzNmae);
            }
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (IncomingFileToTskTemplate)obj;

        }
    }
}
