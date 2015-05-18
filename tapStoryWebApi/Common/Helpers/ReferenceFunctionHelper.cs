using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.Routing;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using tapStoryWebApi.Attributes;
using tapStoryWebApi.Common.Services;

namespace tapStoryWebApi.Common.Helpers
{
    public static class ReferenceFunctionHelper
    {
        public static void CallReferenceFunction<TKey>(string sourceReference, string targetReference,
            ReferenceServiceFunctionType refFunctionType, IDataService targetService, TKey sourceKey, HttpRequestMessage request, Uri link, object[] nonKeyParams = null)
        {

            var methods = Assembly.GetExecutingAssembly().GetTypes().SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof (ReferenceServiceFunction), false).Length > 0);

            methods = methods.Where(m => m.CustomAttributes.Any() && m.CustomAttributes.ToArray()[0].ConstructorArguments.Count() == 3);

            methods = methods.Where(m => m.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[0].Value.ToString() == sourceReference);
            methods = methods.Where(m => m.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[1].Value.ToString() == targetReference);
            methods =
                methods.Where(
                    m =>
                        (ReferenceServiceFunctionType)(m.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[2].Value) ==
                        refFunctionType);

            var method = methods.FirstOrDefault();
            if (method == null) return;
            if (nonKeyParams == null) nonKeyParams = new object[]{};
            var keyParams = new object[] {sourceKey, GetKeyFromUri<TKey>(request, link)};
            var parms = new ArrayList();
            parms.AddRange(keyParams);
            parms.AddRange(nonKeyParams);
            method.Invoke(targetService, parms.ToArray());
        }


        public static TKey GetKeyFromUri<TKey>(HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            var urlHelper = request.GetUrlHelper() ?? new UrlHelper(request);

            var serviceRoot = urlHelper.CreateODataLink(
                request.ODataProperties().RouteName,
                request.ODataProperties().PathHandler, new List<ODataPathSegment>());
            var odataPath = request.ODataProperties().PathHandler.Parse(
                request.ODataProperties().Model,
                serviceRoot, uri.LocalPath);

            var keySegment = odataPath.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Value, ODataVersion.V4);
            return (TKey)value;
        }
    }


}