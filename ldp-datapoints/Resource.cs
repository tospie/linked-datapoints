using LDPDatapoints.Subscriptions;
using System;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints
{
    public abstract class Resource<T> where T: IObservable<T>
    {
        Graph RDFGraph { get; }
        CompressingTurtleWriter TtlWriter { get; }
        HttpRequestListener RequestListener { get; }
        ISubscription<T>[] subscriptions { get; }

        private T _value;
        public T Value {
            get { return _value; }
            set {  /* .. trigger update event */ }
        }

        protected Resource(T value, string route)
        {
            // Validate(route);
            Value.Subscribe(new WebHookSubscription<T>("blabla"));
            TtlWriter = new CompressingTurtleWriter();
            RequestListener = new HttpRequestListener(route);
            RequestListener.OnGet += (o, e) => { };
        }
    }
}
