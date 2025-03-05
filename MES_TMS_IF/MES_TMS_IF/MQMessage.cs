using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_TMS_IF
{
    public class MQMessage
    {
        private MQMessage() { }
        public Header HEADER { get; set; }
        public Dictionary<string, object> BODY { get; set; }
        public Return RETURN { get; set; }
        public static MQMessage GetMessage(IFSystem system)
        {
            var msg = new MQMessage();
            msg.HEADER = new Header();
            msg.HEADER.FAC_ID = "JSE";
            switch (system)
            {
                case IFSystem.MES:
                    msg.HEADER.SOURCE_SUBJECT = "MES";
                    break;
                case IFSystem.TMS:
                    msg.HEADER.SOURCE_SUBJECT = "TMS";
                    break;
                default:
                    break;
            }

            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1990, 1, 1)); // 当地时区
            long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds;
            msg.HEADER.LANGUAGE = "CN";
            msg.HEADER.TXN_ID = timeStamp.ToString();


            msg.BODY = new Dictionary<string, object>();
            msg.RETURN = new Return();
            msg.RETURN.RETURN_CODE = "0";
            msg.RETURN.RETURN_MSG = "成功";
            return msg;
        }
    }

    public class Header
    {
        public string SERVICE_ID { get; set; }
        public string FAC_ID { get; set; }
        public string SOURCE_SUBJECT { get; set; }
        public string TARGET_SUBJECT { get; set; }
        public string REPLY_SUBJECT { get; set; }
        public string REPLY_TYPE { get; set; }
        public string REPLY_QUEUE_NAME { get; set; }
        public string LANGUAGE { get; set; }
        public string SYSTEM_ID { get; set; }
        public string SYSTEM_VERSION { get; set; }
        public string IP_ADDRESS { get; set; }
        public string SESSION_ID { get; set; }
        public string TXN_ID { get; set; }
        public string EVENT_USER { get; set; }
        public string EVENT_MSG { get; set; }
    }

    public class Return
    {
        public string RETURN_CODE { get; set; }
        public string RETURN_MSG { get; set; }
    }

}