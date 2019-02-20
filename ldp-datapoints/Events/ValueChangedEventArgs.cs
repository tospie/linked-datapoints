using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Events
{
    public class ValueChangedEventArgs : EventArgs
    {
        public object Value { get; private set; }

        public ValueChangedEventArgs(object value)
        {
            Value = value;
        }
    }
}
