using LDPDatapoints.Messages;
using LDPDatapoints.Subscriptions;
using System;
using System.ComponentModel;

namespace LDPDatapoints.Resources
{
    class PropertyResource<T> : Resource<T> where T : INotifyPropertyChanged
    {
        public PropertyResource(T value, string route) : base(value, route)
        {
            _value.PropertyChanged += NotifySubscriptions;
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            var args = e as PropertyChangedEventArgs;
            object newValue = null;
            _value.GetType().GetProperty(args.PropertyName).GetValue(newValue);
            string message = new PropertyUpdateMessage(args.PropertyName, newValue).ToString();
            foreach (ISubscription s in Subscriptions)
            {
                s.SendMessage(message);
            }
        }
    }
}
