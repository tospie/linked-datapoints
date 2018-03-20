using LDPDatapoints.Resources;
using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using LDPDatapoints.Subscriptions;
using System.Collections.ObjectModel;

namespace SimpleResourceServer
{
    public class Program
    {
        public class XmlInt : IXmlSerializable
        {
            int value;
            public XmlInt(int value)
            {
                this.value = value;
            }

            public XmlInt() { }

            public XmlSchema GetSchema()
            {
                return new XmlSchema();
            }

            public void ReadXml(XmlReader reader)
            {
                value = int.Parse(reader.ReadContentAsString());
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteString(value.ToString());
            }

            public override string ToString()
            {
                return value.ToString();
            }
        }

        public class XmlBool : IXmlSerializable
        {
            bool value;
            public XmlBool(bool value)
            {
                this.value = value;
            }

            public XmlBool() { }

            public XmlSchema GetSchema()
            {
                return new XmlSchema();
            }

            public void ReadXml(XmlReader reader)
            {
                value = reader.ReadContentAsString() == "true";
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteString(value.ToString());
            }

            public override string ToString()
            {
                return value.ToString();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ValueResource<int> testResource = new ValueResource<int>(3, "http://127.0.0.1:3333/int/");
            ValueResource<XmlBool> test2 = new ValueResource<XmlBool>(new XmlBool(true), "http://127.0.0.1:3333/bool/");

            WebsocketSubscription wsSubscription = new WebsocketSubscription("ws://stuff");
            WebHookSubscription whSubscription = new WebHookSubscription("http://test");
            ObservableCollection<int> c = new ObservableCollection<int>();
            CollectionResource<ObservableCollection<int>, int> r = new CollectionResource<ObservableCollection<int>, int>(c, "http://localhost:12345/c/");

            testResource.Subscribe(whSubscription);
            testResource.Subscribe(wsSubscription);

            Console.WriteLine("Any key to change int to 12 ...");
            Console.ReadKey();
            testResource.Value = 12;

            r.Subscribe(whSubscription);
            r.Subscribe(wsSubscription);

            Console.WriteLine("Any key to add 12 to Collection...");
            Console.ReadKey();
            r.Value.Add(12);

            Console.WriteLine("Any key to close application...");
            Console.ReadKey();
        }
    }
}
