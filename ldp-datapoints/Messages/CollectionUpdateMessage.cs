using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Messages
{
    class CollectionUpdateMessage : SubscriptionMessage
    {
        /// <summary>
        /// ObjectAdded = true => IndexChanged: Index of new Object, newObject = object that was added
        /// ObjectAdded = false, newObject = null : IndexChanged = Index of Object that was removed
        /// ObjectAdded = false, newObject != null : Property of object at index was changed; newObject is new object state
        /// </summary>
        public bool ObjectAdded;
        public int IndexChanged;
        public object newObject;
    }
}
