using LDPDatapoints.Subscriptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDPDatapoints.Resources
{
    public class ValueResource<T> : SubscriptionResource<T> where T : IXmlSerializable, new()
    {

        protected XmlSerializer xmlSerializer;

        public override T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifySubscriptions(this, new EventArgs());
            }
        }

        public ValueResource(T value, string route) : base(value, route)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer = new XmlSerializer(typeof(T));
        protected override void onGet(object sender, HttpEventArgs e)
        {
            HttpListenerRequest request = e.request;
            HttpListenerResponse response = e.response;

            // use JSON-LD representation only if explicitly requested
            if (request.AcceptTypes.Contains("application/ld+json") && !(request.AcceptTypes.Contains("text/turtle")))
            {
                // TODO: json-ld representation
                response.OutputStream.Write(Encoding.UTF8.GetBytes("NOT YET IMPLEMENTED"), 0, "NOT YET IMPLEMENTED".Length);
            }
            else
            {
                System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                CompressingTurtleWriter ttlWriter = new CompressingTurtleWriter();
                ttlWriter.Save(RDFGraph, stringWriter);
                string graph = stringWriter.ToString();
                response.OutputStream.Write(Encoding.UTF8.GetBytes(graph), 0, graph.Length);
            }
            response.Close();
        }
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
