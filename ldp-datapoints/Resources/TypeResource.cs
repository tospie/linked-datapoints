using LDPDatapoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Writing;

namespace ldp_datapoints
{
    class TypeResource
    {
        Graph RDFGraph { get; }
        CompressingTurtleWriter ttlWriter { get; }
        HttpRequestListener httpListener { get; }

    }
}
