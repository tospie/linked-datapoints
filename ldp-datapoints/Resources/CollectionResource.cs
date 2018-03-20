using LDPDatapoints.Messages;
using LDPDatapoints.Subscriptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace LDPDatapoints.Resources
{
    /// <summary>
    /// A Resource that wraps a collection type object. The collection needs to implement the INotifyCollectionChanged
    /// interface, so that Subscriptions are notified about changes in the collection.
    /// </summary>
    /// <typeparam name="T">The collection type that this Resource should represent.</typeparam>
    /// <typeparam name="U">The generic type of the collection elements.</typeparam>
    public class CollectionResource<T, U> : SubscriptionResource<T> where T : ICollection<U>, INotifyCollectionChanged
    {
        private bool collectionHasPropertyElements = false;

        public CollectionResource(T value, string route) : base(value, route)
        {
            _value.CollectionChanged += (o, e) => NotifySubscriptions(o, e);
            collectionHasPropertyElements = typeof(U).IsAssignableFrom(typeof(INotifyPropertyChanged));
        }

        private void handleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var _valueAsCollection = _value as Collection<U>;
            if (collectionHasPropertyElements)
            {
                var count = 0;
                foreach (var i in e.NewItems)
                {
                    var item = i as INotifyPropertyChanged;
                    item.PropertyChanged += (o, p) =>
                    {
                        var index = _valueAsCollection.IndexOf((U)o);
                        handleCollectionItemPropertyChanged((U)o, e.NewStartingIndex + count, p.PropertyName);
                    };
                    count++;
                }
            }
            NotifySubscriptions(sender, e);
        }

        private void handleCollectionItemPropertyChanged(U sender, int index, string property)
        {
            CollectionUpdateMessage m = new CollectionUpdateMessage();
            m.IndexChanged = index;
            m.newObject = sender;
            m.ObjectAdded = false;
            foreach (ISubscription s in Subscriptions)
            {
                s.SendMessage(m.ToString());
            }
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            ObservableCollection<T> o = new ObservableCollection<T>();
            var args = e as NotifyCollectionChangedEventArgs;
        }

        protected override void onGet(object sender, HttpEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
