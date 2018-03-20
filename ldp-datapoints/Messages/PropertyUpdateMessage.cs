using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Messages
{
    class PropertyUpdateMessage : SubscriptionMessage
    {
        public string PropertyName { get; private set; }
        public object PropertyValue { get; private set; }

        public PropertyUpdateMessage(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

    }
}
