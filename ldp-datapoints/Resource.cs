using LDPDatapoints.Subscriptions;
using System;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints
{
    public abstract class Resource<T>
    {
        Graph RDFGraph { get; }
        CompressingTurtleWriter TtlWriter { get; }
        HttpRequestListener RequestListener { get; }
        ISubscription<T>[] subscriptions { get; }

        private T _value;
        public T Value
        {
            get { return _value; }
            set { _value = value; /* .. trigger update event */ }
        }

        protected Resource(T value, string route)
        {
            // Validate(route);
            TtlWriter = new CompressingTurtleWriter();
            RequestListener = new HttpRequestListener(route);
            RequestListener.OnGet += (o, e) => { };
            var t = _value.GetType();
        }
    }
}
