using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace LDPDatapoints.Subscriptions.WebSocket
{
    public class WSSubscriptionServer
    {

        public static WSSubscriptionServer Instance { get; set; }

        private WebSocketServer server;

        public WSSubscriptionServer(string host, int port)
        {
            try
            {
                server = new WebSocketServer(System.Net.IPAddress.Parse(host), port);
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to open WebSocket Server under {0}, {1} . Reason: {2} ", host, port, e.Message);
            }
        }

        public void AddSubscriptionRoute(string path, WebsocketSubscription subscription)
        {
            try
            {
                server.AddWebSocketService(path, () => new WsBehaviour(subscription));
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add WebSocketService for path {0} : {1}", path, e.Message);
            }
        }
    }
}
