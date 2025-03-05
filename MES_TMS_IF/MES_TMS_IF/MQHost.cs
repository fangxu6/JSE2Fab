using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_TMS_IF
{
    public class MQHost
    {
        public IFSystem System { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangeName { get; set; }
        public string ReplyToName { get; set; }

        public int XMessageTtl{ get; set; }
    }
}
