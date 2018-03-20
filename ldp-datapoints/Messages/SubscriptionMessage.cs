using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LDPDatapoints.Messages
{

    public class SubscriptionMessage
    {
        protected XmlSerializer xmlSerializer;
        protected StringWriter stringWriter;

        public SubscriptionMessage()
        {
            xmlSerializer = new XmlSerializer(this.GetType());
            stringWriter = new StringWriter();
        }
        public override string ToString()
        {
            xmlSerializer.Serialize(stringWriter, this);
            return stringWriter.ToString();
        }
    }
}
