using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MES_TMS_IF
{
    public class Xml_ConfigAccess
    {

        private IFSystem _system;
        private XmlDocument _doc;
        private string _filePath;
        public Xml_ConfigAccess(IFSystem system)
        {
            if (!File.Exists(Environment.CurrentDirectory + "\\RabbitMQ_Config.xml"))
            {
                throw new Exception($"RabbitMQ_Config.xml doesn't exsit in {Environment.CurrentDirectory + "\\RabbitMQ_Config.xml"}!");
            }
            else
            {
                _filePath = Environment.CurrentDirectory + "\\RabbitMQ_Config.xml";
                _doc = new XmlDocument();
                this._system = system;
            }
        }

        public string ReadFactoryXml(string node)
        {
            string result = "";
            //加载要读取的XML
            _doc.Load(_filePath);
            //获得根节点
            XmlElement xml = _doc.DocumentElement;

            //获得某一类特定的子节点
            XmlNodeList xnl = xml.SelectNodes($"/RabbitMQ/{_system}/{node}");


            foreach (XmlNode item in xnl)
            {
                result = item.InnerText;
            }
            return result;
        }






    }
}
