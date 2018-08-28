using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LDPDatapoints.Subscriptions.WebSocket
{
    public class WsBehaviour : WebSocketBehavior
    {
        private WebsocketSubscription subscription;

        public WsBehaviour(WebsocketSubscription subscription)
        {
            this.subscription = subscription;
            subscription.Behaviour = this;
        }

        internal void Broadcast(string message)
        {
            Sessions.Broadcast(message);
        }

        internal void Broadcast(byte[] message)
        {
            Sessions.Broadcast(message);
        }

        protected override Task OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Websocket connection closed on {0} ", subscription.Route);
            return base.OnClose(e);
        }

        protected override Task OnOpen()
        {
            Console.WriteLine("Received a new Websocket connection on {0} ", subscription.Route);
            return base.OnOpen();
        }
    }
}
