using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;

namespace tapStoryWebApi.ODataConfiguration
{
    public class NullEntityTypeSerializer : ODataEntityTypeSerializer
    {
        public NullEntityTypeSerializer(ODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        { }

        public override void WriteObjectInline(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
        {
            if (graph != null)
            {
                base.WriteObjectInline(graph, expectedType, writer, writeContext);
            }
        }
    }

}