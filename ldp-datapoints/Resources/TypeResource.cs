using LDPDatapoints;
using LDPDatapoints.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Writing;

namespace LDPDatapoints.Resources
{
    class TypeResource : Resource
    {
        string typeinfo;

        public TypeResource(Type t, string route) : base(route)
        {
            typeinfo = buildTypeInfo(t);
        }

        protected override void onGet(object sender, HttpEventArgs e)
        {
            e.response.OutputStream.Write(Encoding.UTF8.GetBytes(typeinfo), 0, typeinfo.Length);
            e.response.Close();
        }

        protected string buildTypeInfo(Type t)
        {
            return "TODO";
        }
    }
}
