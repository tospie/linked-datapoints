using LDPDatapoints.Subscriptions;
using System;
using System.Net;
using System.Text;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints
{
    public class Resource<T>
    {
        protected Graph RDFGraph { get; }
        protected CompressingTurtleWriter TtlWriter { get; }
        protected HttpRequestListener RequestListener { get; }
        protected ISubscription[] Subscriptions { get; }

        protected T _value;
        public T Value
        {
            get { return _value; }
            set {  /* .. trigger update event */ }
        }

        public Resource(T value, string route)
        {
            TtlWriter = new CompressingTurtleWriter();
            RequestListener = new HttpRequestListener(route);
            RequestListener.OnGet += onGet;
        }

        protected virtual void onGet(object sender, HttpEventArgs e)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            TtlWriter.Save(RDFGraph, sw);
            string graph = sw.ToString();

            HttpListenerRequest request = e.request;
            HttpListenerResponse response = e.response;
            response.OutputStream.Write(Encoding.UTF8.GetBytes(graph), 0, graph.Length);
            response.Close();
        }

        protected virtual void NotifySubscriptions(this object sender, EventArgs e)
        {
            throw new NotImplementedException("Instance of Resource<T> does not implement notification of subscriptions");
        }
    }
}
