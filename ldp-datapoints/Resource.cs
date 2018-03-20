using LDPDatapoints.Subscriptions;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Text;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints
{
    public class Resource<T>
    {
        Graph RDFGraph { get; }
        CompressingTurtleWriter TtlWriter { get; }
        HttpRequestListener RequestListener { get; }
        ISubscription<T>[] subscriptions { get; }
        string route { get; }

        private T _value;
        public T Value
        {
            get { return _value; }
            set {  /* .. trigger update event */ }
        }

        public Resource(T value, string route)
        {
            this.route = route;
            TtlWriter = new CompressingTurtleWriter();
            RequestListener = new HttpRequestListener(route);
            RequestListener.OnGet += onGet;
            RDFGraph = new Graph();
            RDFGraph.NamespaceMap.AddNamespace("owl", new Uri("http://www.w3.org/2002/07/owl#"));
            RDFGraph = buildGraph(value);
        }

        protected virtual void onGet(object sender, HttpEventArgs e)
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
                System.IO.StringWriter sw = new System.IO.StringWriter();
                TtlWriter.Save(RDFGraph, sw);
                string graph = sw.ToString();
                response.OutputStream.Write(Encoding.UTF8.GetBytes(graph), 0, graph.Length);
            }

            response.Close();
        }

        protected virtual Graph buildGraph(T value)
        {
            var graph = new Graph();
            graph.CreateLiteralNode(value.ToString());
            return graph;
        }
    }

    public abstract class EventResource<T> : Resource<T>
    {
        protected EventResource(T value, string route) : base(value, route) { }

        public abstract void onChanged(object sender, EventArgs e);

    }

    public class ValueResource<T> : EventResource<T> where T : INotifyPropertyChanged
    {
        public ValueResource(T value, string route) : base(value, route)
        {
            value.PropertyChanged += onChanged;
        }

        public override void onChanged(object sender, EventArgs e)
        {
            PropertyChangedEventArgs eventArgs = (PropertyChangedEventArgs)e;
            throw new NotImplementedException();
        }
    }

    public class CollectionResource<T> : EventResource<T> where T : INotifyCollectionChanged
    {
        public CollectionResource(T value, string route) : base(value, route)
        {
            value.CollectionChanged += onChanged;
        }

        public override void onChanged(object sender, EventArgs e)
        {
            NotifyCollectionChangedEventArgs eventArgs = (NotifyCollectionChangedEventArgs)e;
            throw new NotImplementedException();
        }
    }
}
