using LDPDatapoints.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace LDPDatapoints.Subscriptions
{
    public class WebsocketSubscription : ISubscription
    {
        private WebSocket webSocket;

        public WebsocketSubscription(string websocketUri)
        {
            webSocket = new WebSocket(websocketUri);
        }

        public void SendMessage(SubscriptionMessage message)
        {
            webSocket.Send(JsonConvert.SerializeObject(message));
        }

        public void SendData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
