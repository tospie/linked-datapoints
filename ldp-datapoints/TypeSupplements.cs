using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDPDatapoints
{
    static class TypeSupplements
    {
        public static Dictionary<Type, string> typeNames = new Dictionary<Type, string>{
            { typeof(int), "int"},
            { typeof(short), "short"},
            { typeof(long), "long"},
            { typeof(byte), "byte"},
            { typeof(bool), "boolean"},
            { typeof(double), "double"},
            { typeof(float), "float"},
            { typeof(string), "string"},
            { typeof(uint), "unsignedInt"},
            { typeof(ushort), "unsignedShort"},
            { typeof(ulong), "unsignedLong"},
            { typeof(object), "anyURI"} // TODO: is this right?
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
