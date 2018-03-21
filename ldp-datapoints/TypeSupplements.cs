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
            { typeof(int), "i32"},
            { typeof(short), "i16"},
            { typeof(long), "i64"},
            { typeof(byte), "byte"},
            { typeof(bool), "bool"},
            { typeof(double), "double"},
            { typeof(float), "float"},
            { typeof(string), "string"},
            { typeof(uint), "u32"},
            { typeof(ushort), "u16"},
            { typeof(ulong), "u64"},
            { typeof(object), "any"}
        };

        public static string transformTypeToString(Type type)
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
