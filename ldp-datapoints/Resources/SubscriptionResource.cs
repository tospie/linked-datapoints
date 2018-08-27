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

using LDPDatapoints.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints.Resources
{
    public abstract class SubscriptionResource<T> : Resource
    {
        protected Graph RDFGraph { get; set; }
        protected Graph SubscriptionDescription { get; private set; }
        protected T _value;

        protected List<ISubscription> Subscriptions { get; }
        protected abstract void NotifySubscriptions(object sender, EventArgs e);
        protected string typeRoute { get; }

        public virtual T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                // this way, ValueResource would not need to override this setter
                // but this does likely not comply with the implementation of CollectionResource(?)
                // NotifySubscriptions(this, new EventArgs());
            }
        }

        public SubscriptionResource(T value, string route) : base(route)
        {
            InitializeRDFGraphs();
            Subscriptions = new List<ISubscription>();
            try
            {
                var routeUri = new Uri(route);
                var baseroute = routeUri.Scheme + "://" + routeUri.Host + ":" + routeUri.Port + "/";
                typeRoute = baseroute + "types/" + typeof(T).transformTypeToString() + "/";
                new TypeResource(typeof(T), typeRoute);
            }
            catch (HttpListenerException)
            {
                //Console.Error.WriteLine("Route already taken. TypeResource should exist.");
            }
        }

        public void Subscribe(ISubscription subscription)
        {
            Subscriptions.Add(subscription);
            describeSubscription(subscription);
        }

        private void InitializeRDFGraphs()
        {
            RDFGraph = new Graph();
            SubscriptionDescription = new Graph();
            SubscriptionDescription.NamespaceMap.AddNamespace("sub", new Uri("http://www.dfki.de/linked-datapoints/subscriptions/#"));
            SubscriptionDescription.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
        }

        private void describeSubscription(ISubscription subscription)
        {
            var subscriptionEndpointNode = SubscriptionDescription.CreateUriNode(new Uri(Route));
            var SUB_PROTOCOL = SubscriptionDescription.CreateUriNode("sub:protocol");
            var RDF_FORMAT = SubscriptionDescription.CreateUriNode("rdf:format");
            if (subscription is WebsocketSubscription)
            {
                var wsSubscription = subscription as WebsocketSubscription;
                var wsEndpointNode = SubscriptionDescription.CreateUriNode(new Uri(wsSubscription.Route));
                SubscriptionDescription.Assert(new Triple(
                        subscriptionEndpointNode,
                        SubscriptionDescription.CreateUriNode("sub:endpoint"),
                        wsEndpointNode
                    ));
                SubscriptionDescription.Assert(new Triple(
                        wsEndpointNode,
                        SUB_PROTOCOL,
                        SubscriptionDescription.CreateUriNode("sub:websocket")
                    ));
                SubscriptionDescription.Assert(new Triple(
                        wsEndpointNode,
                        RDF_FORMAT,
                        SubscriptionDescription.CreateUriNode("sub:TBD")
                    ));
            }
        }

        protected override void onOptions(object sender, HttpEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
