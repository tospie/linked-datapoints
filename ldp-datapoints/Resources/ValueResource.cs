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
        protected System.IO.StringWriter stringWriter;

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
            xmlSerializer = new XmlSerializer(typeof(T));
            stringWriter = new System.IO.StringWriter();
        }

        protected override void NotifySubscriptions(object sender, EventArgs e)
        {
            xmlSerializer.Serialize(stringWriter, _value);
            string valueAsXmlString = stringWriter.ToString();
            foreach(ISubscription s in Subscriptions)
            {
                s.SendMessage(valueAsXmlString);
            }
        }
    }
}
