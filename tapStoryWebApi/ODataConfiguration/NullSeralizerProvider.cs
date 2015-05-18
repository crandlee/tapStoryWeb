using System;
using System.Net;
using System.Net.Http;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using Microsoft.Owin;

namespace tapStoryWebApi.ODataConfiguration
{
    public class NullSerializerProvider : DefaultODataSerializerProvider
    {
        private readonly NullEntityTypeSerializer _nullEntityTypeSerializer;

        public NullSerializerProvider()
        {
            _nullEntityTypeSerializer = new NullEntityTypeSerializer(this);
        }

        public override ODataSerializer GetODataPayloadSerializer(IEdmModel model, Type type, HttpRequestMessage request)
        {
            var serializer = base.GetODataPayloadSerializer(model, type, request);
            if (serializer != null) return serializer;
            var response = request.GetOwinContext().Response;
            response.OnSendingHeaders(state =>
            {
                //Workaround for NULL IQueryables returned from SingleResult.Create (causing a 500 Internal Error without)
                ((IOwinResponse)state).StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.ReasonPhrase = "Not Found";
            }, response);                
            return _nullEntityTypeSerializer;
        }
    }


}