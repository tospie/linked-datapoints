/*
Copyright 2018 T.Spieldenner, DFKI GmbH

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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

        public string Route { get; private set; }

        public WebsocketSubscription(string websocketUri)
        {
            Route = websocketUri;
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
            Console.WriteLine("[WEBSOCKET]\n_______________________\n\n " + message + "\n\n");
        }
    }
}
