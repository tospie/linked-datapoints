using LDPDatapoints.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Subscriptions
{
    class WebHookSubscription<T> : ISubscription
    {
        HttpClient sender;
        List<string> callbackUris;

        public WebHookSubscription(string endpointUri)
        {
            sender = new HttpClient();
            callbackUris = new List<string>();
        }

        public void SendMessage(SubscriptionMessage message)
        {
            StringContent messageAsString = new StringContent(JsonConvert.SerializeObject(message));
            foreach (string uri in callbackUris)
            {
                var r = sender.PostAsync(uri, messageAsString);
            }
        }

        private void registerWebhook(string uri)
        {
            callbackUris.Add(uri);
        }
    }
}
