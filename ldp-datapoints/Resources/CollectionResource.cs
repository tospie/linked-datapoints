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
        {
        }
    }
}
