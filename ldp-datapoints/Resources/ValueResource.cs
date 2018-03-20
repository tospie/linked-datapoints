using LDPDatapoints.Subscriptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDPDatapoints.Resources
{
    public class ValueResource<T> : Resource<T> where T : IXmlSerializable
    {

        protected XmlSerializer xmlSerializer;

        public override T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifySubscriptions(this, new EventArgs());
            }
        }

        public ValueResource(T value, string route) : base(value, route)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer = new XmlSerializer(typeof(T));
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, _value);
            string valueAsXmlString = stringWriter.ToString();
            foreach (ISubscription s in Subscriptions)
            {
                s.SendMessage(valueAsXmlString);
            }
        }
    }
}
