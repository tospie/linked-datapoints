using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace LDPDatapoints.Subscriptions
{
    public class WebsocketSubscription<T> : ISubscription<T>
    {
        private WebSocket webSocket;

        public WebsocketSubscription(string websocketUri)
        {
            webSocket = new WebSocket(websocketUri);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(ISubscriptionMessage message)
        {
            webSocket.Send(JsonConvert.SerializeObject(message));
        }
    }
}
