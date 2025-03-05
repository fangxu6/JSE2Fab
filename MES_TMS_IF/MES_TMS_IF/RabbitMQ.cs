using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES_TMS_IF
{
    public class RabbitMQ
    {
        private RabbitMQ() { }
        public static MQHost MES_MQ { get; set; }

        public static MQHost TMS_MQ { get; set; }
        #region init
        public static void Init()
        {
            try
            {
                if (MES_MQ == null)
                {
                    Xml_ConfigAccess config = new Xml_ConfigAccess(IFSystem.MES);
                    MES_MQ = new MQHost();
                    MES_MQ.System = IFSystem.MES;
                    MES_MQ.HostName = config.ReadFactoryXml("HostName");
                    MES_MQ.Port = Convert.ToInt32(config.ReadFactoryXml("Port"));
                    MES_MQ.UserName = config.ReadFactoryXml("UserName");
                    MES_MQ.Password = config.ReadFactoryXml("Password");
                    MES_MQ.VirtualHost = config.ReadFactoryXml("VirtualHost");
                    MES_MQ.ExchangeName = config.ReadFactoryXml("ExchangeName");
                    MES_MQ.ReplyToName = config.ReadFactoryXml("ReplyToName");
                    MES_MQ.XMessageTtl = Convert.ToInt32(config.ReadFactoryXml("X_MESSAGE_TTL"));

                }

                if (TMS_MQ == null)
                {
                    Xml_ConfigAccess config = new Xml_ConfigAccess(IFSystem.TMS);
                    TMS_MQ = new MQHost();
                    TMS_MQ.System = IFSystem.TMS;
                    TMS_MQ.HostName = config.ReadFactoryXml("HostName");
                    TMS_MQ.Port = Convert.ToInt32(config.ReadFactoryXml("Port"));
                    TMS_MQ.UserName = config.ReadFactoryXml("UserName");
                    TMS_MQ.Password = config.ReadFactoryXml("Password");
                    TMS_MQ.VirtualHost = config.ReadFactoryXml("VirtualHost");
                    TMS_MQ.ExchangeName = config.ReadFactoryXml("ExchangeName");
                    TMS_MQ.ReplyToName = config.ReadFactoryXml("ReplyToName");
                    TMS_MQ.XMessageTtl = Convert.ToInt32(config.ReadFactoryXml("X_MESSAGE_TTL"));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static ConnectionFactory GetFactory(IFSystem system)
        {
            ConnectionFactory factory = new ConnectionFactory();
            var mQHost = GetMQHost(system);
            factory.HostName = mQHost.HostName;
            factory.Port = mQHost.Port;
            factory.UserName = mQHost.UserName;
            factory.Password = mQHost.Password;
            factory.VirtualHost = mQHost.VirtualHost;
            
            return factory;
        }
        #endregion

        #region 队列模式
        /// <summary>
        /// 工作队列模式下发布消息
        /// </summary>
        /// <param name="queueName">写入消息的队列名称</param>
        /// <param name="replyTo">回复消息的队列名称</param>
        /// <param name="message">要发布的消息</param>
        /// <exception cref="Exception"></exception>
        public static void Publish_WorkQueues(IFSystem system, string queueName, string replyTo, string message)
        {
            ConnectionFactory factory = GetFactory(system);
            var mQHost = GetMQHost(system);
            if (queueName == "")
            {
                throw new Exception("RabbitMQ procuder find no queue name!");
            }

            if (message == "")
            {
                throw new Exception("RabbitMQ procuder find no message into the queue!");
            }

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() { ["x-message-ttl"] = mQHost.XMessageTtl });
                    var body = Encoding.UTF8.GetBytes(message);

                    //消息持久化
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    props.ReplyTo = replyTo;
                    channel.BasicPublish("", queueName, props, body);
                }
            }
        }

        /// <summary>
        /// 工作队列模式下发布消息(定制)
        /// </summary>
        /// <param name="queueName">写入消息的队列名称</param>
        /// <param name="replyTo">回复消息的队列名称</param>
        /// <param name="serviceId">接口名字</param>
        /// <param name="messageId">消息ID</param> 
        /// <param name="message">要发布的消息</param>
        /// <exception cref="Exception"></exception>
        public static void Publish_WorkQueues(IFSystem system, string queueName,/* string replyTo,*/ string serviceId, string messageId, string message)
        {
            ConnectionFactory factory = GetFactory(system);
            var mQHost = GetMQHost(system);

            if (queueName == "")
            {
                throw new Exception("RabbitMQ procuder find no queue name!");
            }

            if (message == "")
            {
                throw new Exception("RabbitMQ procuder find no message into the queue!");
            }

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() { ["x-message-ttl"] = mQHost.XMessageTtl });
                    var body = Encoding.UTF8.GetBytes(message);

                    //消息持久化
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    //props.ReplyTo = replyTo;
                    props.Headers = new Dictionary<string, object>()
                    {
                        ["ServiceId"] = serviceId,
                        ["MessageId"] = messageId
                    };
                    channel.BasicPublish("", queueName, props, body);
                }
            }
        }


        public static void Reply(IFSystem system, MQMessage message, IBasicProperties properties)
        {
            if (string.IsNullOrEmpty(properties.ReplyTo)) return;

            ConnectionFactory factory = GetFactory(system);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //props.Persistent = true;
                    //props.ReplyTo = replyTo;
                    //properties.Headers = new Dictionary<string, object>()
                    //{
                    //    ["ServiceId"] = serviceId,
                    //    ["MessageId"] = messageId
                    //};
                    var retMsg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                    //打印Log
                    Console.WriteLine(JsonConvert.SerializeObject(message, Formatting.Indented));
                    channel.BasicPublish("", properties.ReplyTo, properties, retMsg);
                }
            }
        }

        /// <summary>
        /// 工作队列模式下发布消息(XML配置版)
        /// </summary>
        /// <param name="serviceId">接口名字</param>
        /// <param name="message">要发布的消息</param>
        /// <exception cref="Exception"></exception>
        public static void Publish_WorkQueues(IFSystem system, string serviceId, string message)
        {
            ConnectionFactory factory = GetFactory(system);
            var mQHost = GetMQHost(system);
            string queueName = mQHost.ExchangeName;
            string replyTo = mQHost.ReplyToName;

            if (queueName == "")
            {
                throw new Exception("RabbitMQ procuder find no queue name!");
            }

            if (message == "")
            {
                throw new Exception("RabbitMQ procuder find no message into the queue!");
            }

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() {/* ["x-message-ttl"] = mQHost.XMessageTtl*/ });
                    var body = Encoding.UTF8.GetBytes(message);

                    //消息持久化
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    props.ReplyTo = replyTo;
                    props.Headers = new Dictionary<string, object>()
                    {
                        ["ServiceId"] = serviceId,
                        ["MessageId"] = Guid.NewGuid().ToString("D")
                    };
                    channel.BasicPublish(queueName, "", props, body);
                }
            }
        }

        /// <summary>
        /// 工作队列模式下创建消费者
        /// </summary>
        /// <param name="queueName">取消息的队列名称</param>
        /// <param name="triggerEvent">消费者绑定的触发事件</param>
        /// 

        public static void Create_Cosumer_WorkQueues(IFSystem system, string queueName, RabbitTriggerEvent triggerEvent)
        {
            if (queueName == "")
            {
                throw new Exception("RabbitMQ consumer found no queue name!");
            }

            if (triggerEvent == null)
            {
                throw new Exception("RabbitMQ consumer found no binding triggerEvent!");
            }
            ConnectionFactory factory = GetFactory(system);
            var mQHost = GetMQHost(system);
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() { ["x-message-ttl"] = mQHost.XMessageTtl });
            channel.BasicQos(0, 1, false);
            var a = channel.CreateBasicProperties();

            EventingBasicConsumer consumers = new EventingBasicConsumer(channel);
            consumers.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //触发事件，由外部方法执行
                triggerEvent.OnMessage(message);
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, false, consumers);
        }

        /// <summary>
        /// 工作队列模式下创建消费者(XML配置版)
        /// </summary>
        /// <param name="queueName">取消息的队列名称</param>
        /// <param name="triggerEvent">消费者绑定的触发事件</param>
        public static void Create_Cosumer_WorkQueues(IFSystem system, RabbitTriggerEvent triggerEvent)
        {
            var mQHost = GetMQHost(system);
            string queueName = mQHost.ReplyToName;
            //string queueName = mQHost.ExchangeName;
            if (queueName == "")
            {
                throw new Exception("RabbitMQ consumer found no queue name!");
            }

            if (triggerEvent == null)
            {
                throw new Exception("RabbitMQ consumer found no binding triggerEvent!");
            }
            ConnectionFactory factory = GetFactory(system);
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
             
            channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() { ["x-message-ttl"] = mQHost.XMessageTtl });
            channel.BasicQos(0, 1, false);
            var a = channel.CreateBasicProperties();
            

            EventingBasicConsumer consumers = new EventingBasicConsumer(channel);
            consumers.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var msgMessage = Encoding.UTF8.GetString(body);
                //触发事件，由外部方法执行
                triggerEvent.OnMessage(msgMessage);
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queueName, false, consumers);
        }
        #endregion

        #region 订阅模式
        /// <summary>
        /// 订阅模式下发布消息
        /// </summary>
        /// <param name="exchangeName">写到交换机的名字</param>
        /// <param name="routingKey">写到哪个队列的路由key，不写为广播模式</param>
        /// <param name="message">要发布的消息</param>
        public static void Publish_PubSub(IFSystem system, string exchangeName, string routingKey, string message)
        {
            if (exchangeName == "")
            {
                throw new Exception("RabbitMQ procuder found no exchangeName name!");
            }
            if (message == "")
            {
                throw new Exception("RabbitMQ procuder found no message!");
            }

            ConnectionFactory factory = GetFactory(system);

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    //消息持久化
                    var props = channel.CreateBasicProperties();
                    props.Persistent = true;
                    //将消息和routingkey发布到指定交换机，由交换机根据队列绑定的routingKey来分配消息
                    channel.BasicPublish(exchangeName, routingKey, props, body);
                }
            }
        }



        /// <summary>
        /// 订阅模式下创建消费者
        /// </summary>
        /// <param name="exchangeName">连接交换机的名字</param>
        /// <param name="routingKeys">消费者拥有的routingKey</param>
        /// <param name="queueName">消费者绑定队列的名字</param>
        /// <param name="triggerEvent">消费者绑定的触发事件</param>
        /// <exception cref="Exception"></exception>
        public static void Create_Consumer_PubSub(IFSystem system, string exchangeName, List<string> routingKeys, string queueName, RabbitTriggerEvent triggerEvent)
        {
            if (queueName == "")
            {
                throw new Exception("RabbitMQ consumer found no queue name!");
            }

            if (triggerEvent == null)
            {
                throw new Exception("RabbitMQ consumer found no binding triggerEvent!");
            }
            if (exchangeName == "")
            {
                throw new Exception("RabbitMQ consumer  found no exchangeName name!");
            }

            ConnectionFactory factory = GetFactory(system);
            var mQHost = GetMQHost(system);
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //当routingKey没有时，连接广播式的交换机，当有时，连接对点对的交换机
            if (routingKeys.Count == 0)
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
                channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() { /*["x-message-ttl"] = mQHost.XMessageTtl */});

                channel.QueueBind(queueName, exchangeName, "");
            }
            else
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
                channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>() { /*["x-message-ttl"] = mQHost.XMessageTtl*/ });

                //给消费者队列绑定多个routingKey
                foreach (var routingKey in routingKeys)
                {
                    channel.QueueBind(queueName, exchangeName, routingKey);
                }
            }

            channel.BasicQos(0, 1, false);

            EventingBasicConsumer consumers = new EventingBasicConsumer(channel);

            consumers.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                triggerEvent.OnMessage(message);
                try
                {
                    var msg = JsonConvert.DeserializeObject<MQMessage>(message);
                    //var retMsg = null;
                    //if (msg.HEADER.SERVICE_ID == "CPINDataReceive")
                    if (!string.IsNullOrWhiteSpace(msg.HEADER.SERVICE_ID))
                    {
                        msg = SampleDataGen.GetRefMethod(msg.HEADER.SERVICE_ID, msg);
                        //msg.RETURN.RETURN_CODE =  
                    }
                    Reply(IFSystem.MES, msg, ea.BasicProperties);
                }
                catch (Exception)
                {


                }

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumers);
        }

        #endregion

        public static MQHost GetMQHost(IFSystem system)
        {
            MQHost mQHost = null;
            switch (system)
            {
                case IFSystem.MES:
                    mQHost = MES_MQ;
                    break;
                case IFSystem.TMS:
                    mQHost = TMS_MQ;
                    break;
                default:
                    throw new Exception("The system is not defined");
            }
            return mQHost;
        }

    }
}
