using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MES_TMS_IF
{
    public partial class Form1 : Form
    {
        //RabbitMQ mq;

        public Form1()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            //InitImage();
            this.btnStartMes.Image = InitImage(Color.Red);
            this.btnStartMes.ForeColor = Color.Red;
            this.btnStartMes.Font = new Font(this.Font, FontStyle.Bold);

            this.btnStartTms.Image = InitImage(Color.Red);
            this.btnStartTms.ForeColor = Color.Red;
            this.btnStartTms.Font = new Font(this.Font, FontStyle.Bold);
            RabbitMQ.Init();

            ContextMenuStrip cmnuStrip = new ContextMenuStrip();
            cmnuStrip.Items.Add("Clear");
            cmnuStrip.ItemClicked += CmnuStrip_ItemClicked;
            txtSendToMesMsg.ContextMenuStrip = cmnuStrip;
            txtSendToTmsMsg.ContextMenuStrip = cmnuStrip;
            txtMesMsg.ContextMenuStrip = cmnuStrip;
            txtTmsMsg.ContextMenuStrip = cmnuStrip;

        }

        private void CmnuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Clear")
            {
                (sender as ContextMenuStrip).SourceControl.Text = null;
            }
            //if(e.ClickedItem == "")
        }

        private Bitmap InitImage(Color color)
        {
            //InitImage
            Bitmap bitmap = new Bitmap(64, 64);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.TranslateTransform(40, 40);
            g.FillEllipse(new SolidBrush(color), 120, 120, 200, 200);
            g.TranslateTransform(100, 100);
            Pen pen = new Pen(Color.Gold, 2);
            g.DrawEllipse(pen, 10, 10, 200, 200);
            g.Dispose();
            return bitmap;
        }

        RabbitTriggerEvent tmsEvent = null;
        RabbitTriggerEvent mesEvent = null;

        RabbitTriggerEvent tmsReplyEvent = null;
        RabbitTriggerEvent mesReplyEvent = null;
        private void btnStartMes_Click(object sender, EventArgs e)
        {
            if (mesEvent == null)
            {
                mesEvent = new RabbitTriggerEvent();
                mesEvent.infoEvent += MesEvent_infoEvent;
                var host = RabbitMQ.GetMQHost(IFSystem.MES);
                RabbitMQ.Create_Consumer_PubSub(host.System, host.ExchangeName, new List<string>() { string.Empty }, host.ExchangeName, mesEvent);

                mesReplyEvent = new RabbitTriggerEvent();
                mesReplyEvent.infoEvent += MesReplyEvent_infoEvent;
                RabbitMQ.Create_Cosumer_WorkQueues(IFSystem.MES, host.ReplyToName, mesReplyEvent);
                this.btnStartMes.ForeColor = Color.Green;
            }
        }


        private void btnStartTms_Click(object sender, EventArgs e)
        {
            if (tmsEvent == null)
            {
                tmsEvent = new RabbitTriggerEvent();
                tmsEvent.infoEvent += TmsEvent_infoEvent;
                var host = RabbitMQ.GetMQHost(IFSystem.TMS);
                RabbitMQ.Create_Consumer_PubSub(host.System, host.ExchangeName, new List<string>() { string.Empty }, host.ExchangeName, tmsEvent);

                tmsReplyEvent = new RabbitTriggerEvent();
                tmsReplyEvent.infoEvent += TmsReplyEvent_infoEvent;
                RabbitMQ.Create_Cosumer_WorkQueues(IFSystem.TMS, host.ReplyToName, tmsReplyEvent);

                this.btnStartTms.ForeColor = Color.Green;
            }
        }

        private void MesReplyEvent_infoEvent(object sender, MessageEventArgs e)
        {
            RenderMessage(txtMesMsg, e);
        }

        private void MesEvent_infoEvent(object sender, MessageEventArgs e)
        {
            RenderMessage(txtMesMsg, e);
        }
        private void TmsEvent_infoEvent(object sender, MessageEventArgs e)
        {
            RenderMessage(txtTmsMsg, e);
        }

        private void TmsReplyEvent_infoEvent(object sender, MessageEventArgs e)
        {
            RenderMessage(txtTmsMsg, e);
        }

        private void RenderMessage(TextBox txt, MessageEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        txt.Text = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(e.Message), Formatting.Indented);
                    }
                    catch (Exception)
                    {
                        txt.Text = e.Message;
                    }
                    txt.Text = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(e.Message), Formatting.Indented);
                }));
            }
            else
            {
                try
                {
                    txt.Text = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(e.Message), Formatting.Indented);
                }
                catch (Exception)
                {
                    txt.Text = e.Message;
                }
            }
        }
        private void btnToTmsSend_Click(object sender, EventArgs e)
        {
            MQMessage message = null;
            try
            {
                message = JsonConvert.DeserializeObject<MQMessage>(txtSendToTmsMsg.Text);
                message.HEADER.REPLY_QUEUE_NAME = RabbitMQ.GetMQHost(IFSystem.MES).ReplyToName;
                txtTmsServiceId.Text = message.HEADER.SERVICE_ID;
            }
            catch { }

            if (message == null)
            {
                message = MQMessage.GetMessage(IFSystem.MES);
                message.BODY.Add("TEST", txtSendToTmsMsg.Text);
                message.HEADER.SERVICE_ID = txtTmsServiceId.Text;
            }
            string Contentjson = JsonConvert.SerializeObject(message);

            RabbitMQ.Publish_WorkQueues(IFSystem.TMS, message.HEADER.SERVICE_ID, Contentjson);
        }

        private void btnToMesSned_Click(object sender, EventArgs e)
        {
            MQMessage message = null;
            try
            {
                message = JsonConvert.DeserializeObject<MQMessage>(txtSendToMesMsg.Text);
                //message.HEADER.REPLY_QUEUE_NAME = RabbitMQ.GetMQHost(IFSystem.TMS).ReplyToName;
                txtMesServiceId.Text = message.HEADER.SERVICE_ID;
            }
            catch { }

            if (message == null)
            {
                message = MQMessage.GetMessage(IFSystem.TMS);

                message.BODY.Add("TEST", txtSendToMesMsg.Text);
                message.HEADER.SERVICE_ID = txtMesServiceId.Text;
            }
            string Contentjson = JsonConvert.SerializeObject(message);
            if (string.IsNullOrEmpty(txtMesServiceId.Text) && !string.IsNullOrEmpty(message.HEADER.SERVICE_ID))
            {
                txtMesServiceId.Text = message.HEADER.SERVICE_ID;
            }
            RabbitMQ.Publish_WorkQueues(IFSystem.MES, message.HEADER.SERVICE_ID, Contentjson);
        }

        private void btnFormatSendToTmsMsg_Click(object sender, EventArgs e)
        {
            try
            {
                txtSendToTmsMsg.Text = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(txtSendToTmsMsg.Text), Formatting.Indented);
            }
            catch (Exception)
            {
            }
        }

        private void btnFormatSendToMesMsg_Click(object sender, EventArgs e)
        {
            try
            {
                txtSendToMesMsg.Text = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(txtSendToMesMsg.Text), Formatting.Indented);
            }
            catch (Exception)
            {
            }
        }
    }
}
