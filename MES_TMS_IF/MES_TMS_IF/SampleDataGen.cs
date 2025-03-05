using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_TMS_IF
{
    public class SampleDataGen
    {
        static Random random = new Random();
        public static MQMessage CPINDataReceive(MQMessage message)
        {
            message.RETURN.RETURN_CODE = ReturnData.Success;
            message.BODY.Clear();
            message.BODY["CARD_ID"] = "LOT001";
            return message;
        }

        public static MQMessage CPCheckIsExistTestProgram(MQMessage message)
        {
            message.RETURN.RETURN_CODE = ReturnData.Success;
            message.BODY.Clear();
            message.BODY["IS_EXIST"] = "Y";
            return message;
        }

        public static MQMessage CPTrackInData(MQMessage message)
        {
            message.RETURN.RETURN_CODE = ReturnData.Success;
            message.BODY.Clear();
            return message;
        }

        public static MQMessage CPTrackOutData(MQMessage message)
        {
            message.RETURN.RETURN_CODE = ReturnData.Success;
            message.BODY.Clear();
            return message;
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static MQMessage GetRefMethod(string method, MQMessage message)
        {
            SampleDataGen gen = new SampleDataGen();
            var m = gen.GetType().GetMethod(method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (m == null) return null;
            //MQMessage message = MQMessage.GetMessage(IFSystem.MES);
            var r = (MQMessage)m.Invoke(gen, new object[] { message });
            return r;
        }
    }
}
