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
        protected T _value;

        protected List<ISubscription> Subscriptions { get; }
        protected abstract void NotifySubscriptions(object sender, EventArgs e);

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
            RDFGraph = new Graph();
            Subscriptions = new List<ISubscription>();
            try
            {
                var typeRoute = "http://127.0.0.1:3333/types/" + typeof(T).Name.ToString() + "/";
                new TypeResource(typeof(T), typeRoute);
                Console.WriteLine("Route: " + typeRoute);
            }
            catch (HttpListenerException)
            {
                Console.Error.WriteLine("Route already taken. TypeResource should exist.");
            }
        }

        public void Subscribe(ISubscription subscription)
        {
            Subscriptions.Add(subscription);
        }

        // this maybe is a bad idea
        //protected abstract Graph buildGraph(T value);

    }
}
