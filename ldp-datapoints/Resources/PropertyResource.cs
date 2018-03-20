using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints.Resources
{
    class PropertyResource<T> : Resource<T> where T : INotifyPropertyChanged
    {
        public PropertyResource(T value, string route) : base(value, route)
        {
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            base.NotifySubscriptions(sender, e);
        }
    }
}
