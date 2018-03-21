using LDPDatapoints.Subscriptions;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints.Resources
{

    public class ValueResource<T> : SubscriptionResource<T>
    {
        XmlSerializer xmlSerializer;
        XmlWriter xmlWriter;

        public override T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                buildGraph();
                NotifySubscriptions(this, new EventArgs());
            }
        }

        public ValueResource(T value, string route) : base(value, route)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer = new XmlSerializer(typeof(T));
            xmlWriter = XmlWriter.Create(stringWriter);

            Value = value;
        }

        protected override void onGet(object sender, HttpEventArgs e)
        {
            HttpListenerRequest request = e.request;
            HttpListenerResponse response = e.response;
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            CompressingTurtleWriter ttlWriter = new CompressingTurtleWriter();
            ttlWriter.Save(RDFGraph, stringWriter);
            string graph = stringWriter.ToString();
            response.OutputStream.Write(Encoding.UTF8.GetBytes(graph), 0, graph.Length);
            response.Close();
        }

        protected void buildGraph()
        {
            var graph = new Graph();
            graph.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));            
            var o = graph.CreateLiteralNode(Value.ToString(), new Uri("http://localhost:3333/todo"));
            var p = graph.CreateUriNode("rdf:value");
            var s = graph.CreateUriNode(new Uri(route));
            graph.Assert(new Triple(s, p, o));
            RDFGraph = graph;
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, _value);
            string valueAsXmlString = stringWriter.ToString();
            foreach (ISubscription s in Subscriptions)
            {
                s.SendMessage(valueAsXmlString);
            }
        }
    }
}
