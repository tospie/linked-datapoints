using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints
{
    public static class TypeSupplements
    {
        public static Dictionary<Type, string> typeNames = new Dictionary<Type, string>{
            { typeof(int), "xsd:int"},
            { typeof(short), "xsd:short"},
            { typeof(long), "xsd:long"},
            { typeof(byte), "xsd:byte"},
            { typeof(bool), "xsd:boolean"},
            { typeof(double), "xsd:double"},
            { typeof(float), "xsd:float"},
            { typeof(string), "xsd:string"},
            { typeof(uint), "xsd:unsignedInt"},
            { typeof(ushort), "xsd:unsignedShort"},
            { typeof(ulong), "xsd:unsignedLong"},
            { typeof(object), "xsd:anyURI"}
        };

        public static string transformTypeToString(this Type type)
        {
            if (typeNames.ContainsKey(type))
            {
                return typeNames[type];
            }
            else if (typeof(IDictionary).IsAssignableFrom(type))
            {
                var genericKeyType = transformTypeToString(type.GenericTypeArguments[0]);
                var genericValueType = transformTypeToString(type.GenericTypeArguments[1]);
                return "DictionaryFrom(" + genericKeyType + ")To(" + genericValueType + ")";
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                if (type.GenericTypeArguments.Length > 0)
                {
                    var genericListType = transformTypeToString(type.GenericTypeArguments[0]);
                    return "ListOf(" + genericListType + ")";
                }
            }
            return type.Name;
        }
    }
}
