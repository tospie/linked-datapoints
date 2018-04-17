/*
Copyright 2018 T.Spieldenner, DFKI GmbH

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
