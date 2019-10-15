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

using LDPDatapoints.Events;
using LDPDatapoints.Subscriptions;
using Newtonsoft.Json;
using System;
using System.IO;
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

        public event EventHandler<EventArgs> ValueChanged;

        public override T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                buildGraph();
                NotifySubscriptions(this, new EventArgs());
                ValueChanged?.Invoke(this, new ValueChangedEventArgs(_value));
            }
        }

        public ValueResource(T value, string route) : base(value, route)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer = new XmlSerializer(typeof(T));
            xmlWriter = XmlWriter.Create(stringWriter);

            Value = value;
        }

        public override void onGet(object sender, HttpEventArgs e)
        {
            HttpListenerRequest request = e.request;
            HttpListenerResponse response = e.response;
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            string valueAsString = "";
            if (request.AcceptTypes.Contains("application/xml"))
            {
                xmlSerializer.Serialize(stringWriter, _value);
                valueAsString = stringWriter.ToString();
                response.ContentType = "application/xml";
            }
            else if (request.AcceptTypes.Contains("application/json"))
            {
                valueAsString = JsonConvert.SerializeObject(_value);
                response.ContentType = "application/json";
            }
            else if (request.AcceptTypes.Contains("text/turtle"))
            {
                CompressingTurtleWriter ttlWriter = new CompressingTurtleWriter();
                ttlWriter.Save(RDFGraph, stringWriter);
                valueAsString = stringWriter.ToString();
                response.ContentType = "text/turtle";
            }
            response.OutputStream.Write(Encoding.UTF8.GetBytes(valueAsString), 0, valueAsString.Length);
            response.Close();
        }

        protected void buildGraph()
        {
            var graph = new Graph();
            graph.NamespaceMap.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            var literalValue = Value == null ? null : Value.ToString();
            var o = graph.CreateLiteralNode(literalValue, new Uri(typeRoute));
            var p = graph.CreateUriNode("rdf:value");
            var s = graph.CreateUriNode(new Uri(Route));
            graph.Assert(new Triple(s, p, o));
            RDFGraph = graph;
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, _value);
            string valueAsXmlString = stringWriter.ToString();
            foreach (Subscription s in Subscriptions)
            {
                s.SendMessage(valueAsXmlString);
            }
        }

        public override void onPut(object sender, HttpEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void onPost(object sender, HttpEventArgs e)
        {
            HttpListenerRequest request = e.request;

            try
            {
                if (request.ContentType.Equals("application/json"))
                {
                    using (Stream input = request.InputStream)
                    {
                        using (StreamReader reader = new StreamReader(input, Encoding.UTF8))
                        {
                            string receivedData = reader.ReadToEnd();
                            var deserialized = JsonConvert.DeserializeObject<T>(receivedData);
                            Value = deserialized;
                        }
                    }
                }
                else if (request.ContentType.Equals("application/xml"))
                {
                    Value = (T)xmlSerializer.Deserialize(request.InputStream);
                }
                e.response.StatusCode = 204;
                e.response.Close();
            }
            catch (Exception ex)
            {
                e.response.StatusCode = 500;
                string genericError = "Something went wrong when processing the request.";
                e.response.OutputStream.Write(Encoding.UTF8.GetBytes(genericError), 0, genericError.Length);
                e.response.Close();
            }
        }
    }
}
