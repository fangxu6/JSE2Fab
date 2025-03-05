using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_TMS_IF
{
    public class MessageEventArgs
    {
        private string _message;
        public MessageEventArgs(string message)
        {
            this._message = message;
        }

        public string Message { get => _message; set => _message = value; }
    }
    public class RabbitTriggerEvent
    {
        public delegate void GetInfoEventHandler(object sender, MessageEventArgs e);
        public event GetInfoEventHandler infoEvent;

        //存储信息变量
        //public string Message = "";

        //编写引发事件的函数(在程序任意域使用)
        public void OnMessage(string message)
        {
            if (infoEvent != null)
            {
                //发送信息
                infoEvent(this, new MessageEventArgs(message));
            }
        }
    }
}
