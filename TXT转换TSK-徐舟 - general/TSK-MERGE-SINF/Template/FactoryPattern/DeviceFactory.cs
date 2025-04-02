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
                MessageBox.Show("该型号未定义，请联系IT并告知批次号。");
                throw new Exception("该型号不支持");
            }
            object obj = Activator.CreateInstance(o, true);//根据类型创建实例
            return (IncomingFileToTskTemplate)obj;
        }
    }
}
