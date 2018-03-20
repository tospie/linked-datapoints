using LDPDatapoints.Resources;
using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

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
            ValueResource<XmlInt> testResource = new ValueResource<XmlInt>(new XmlInt(3), "http://127.0.0.1:3333/int/");
            ValueResource<XmlBool> test2 = new ValueResource<XmlBool>(new XmlBool(true), "http://127.0.0.1:3333/bool/");
            Console.WriteLine("Eingabe beendet Programm...");
            Console.ReadKey();
        }
    }
}
