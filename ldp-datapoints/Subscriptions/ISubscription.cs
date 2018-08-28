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
using LDPDatapoints.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace LDPDatapoints.Subscriptions
{
    public abstract class Subscription
    {
        /// <summary>
        /// Route to the endpoint that can be called to subscribe to updates of the datum
        /// </summary>
        public string Route { get; protected set; }

        protected IUriNode SUB_ENDPOINT;
        protected IUriNode SUB_PROTOCOL;
        protected IUriNode RDF_FORMAT;
        protected IUriNode SUBSCRIPTION_ROUTE;

        /// <summary>
        /// RDF Graph that contains a description of the subscription endpoint that can be processed by a connecting client.
        /// MUST contain with predicate sub:endpoint, object <see cref="Route""/>, subject Route to value datapoint,
        /// MUST contain triples with <see cref="Route"/> as subject, sub:protocol and rdf:format respectiveley, and
        /// MUST use in these triples specify as objects sub:[protocol] with [protocol] e.g. websocket or webhook,
        /// and a JSON-Schema or XML-Schema representation of the used message format
        /// </summary>
        public Graph DescriptionGraph { get; protected set; }

        public Subscription(string Route)
        {
            this.Route = Route;
            InitializeDescriptionGraph();
        }

        private void InitializeDescriptionGraph()
        {
            DescriptionGraph = new Graph();
            RegisterNamespaces();
            InitializeUriNodes();
        }

        private void RegisterNamespaces()
        {
            DescriptionGraph.NamespaceMap.AddNamespace("sub", new Uri("http://www.dfki.de/linked-datapoints/subscriptions/#"));
            DescriptionGraph.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
        }

        private void InitializeUriNodes()
        {
            SUBSCRIPTION_ROUTE = DescriptionGraph.CreateUriNode(new Uri(Route));
            SUB_ENDPOINT = DescriptionGraph.CreateUriNode("sub:endpoint");
            SUB_PROTOCOL = DescriptionGraph.CreateUriNode("sub:protocol");
            RDF_FORMAT = DescriptionGraph.CreateUriNode("rdf:format");
        }

        public abstract void SendMessage(string message);

        public abstract void SendData(byte[] data);

        public virtual void BuildGraph(Resource datapoint)
        {
            IUriNode dataPointURI = DescriptionGraph.CreateUriNode(new Uri(datapoint.Route));
            DescriptionGraph.Assert(new Triple(dataPointURI, SUB_ENDPOINT, SUBSCRIPTION_ROUTE));
        }

    }
}
