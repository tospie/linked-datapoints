using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Resources
{
    public class CollectionResource<T> : Resource<T> where T : INotifyCollectionChanged
    {
        protected CollectionResource(T value, string route) : base(value, route)

        private void handleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var _valueAsCollection = _value as Collection<U>;
            if (collectionHasPropertyElements)
            {
                foreach (var i in e.NewItems)
                {
                    var item = i as INotifyPropertyChanged;
                    item.PropertyChanged += (o, p) =>
                    {
                        var index = _valueAsCollection.IndexOf((U)o);
                        handleCollectionItemPropertyChanged((U)o, index, p.PropertyName);
                    };
                }
            }
            NotifySubscriptions(sender, e);
        }

        private void handleCollectionItemPropertyChanged(U sender, int index, string property)
        {
            var collection = _value as ICollection<U>;
            CollectionUpdateMessage m = new CollectionUpdateMessage();
            m.IndexChanged = index;
            m.newObject = sender;
            m.ObjectAdded = false;
            foreach (ISubscription s in Subscriptions)
            {
                s.SendMessage(m);
            }
        }

        {
            value.CollectionChanged += (o,e) => { };
        }
    }
}
