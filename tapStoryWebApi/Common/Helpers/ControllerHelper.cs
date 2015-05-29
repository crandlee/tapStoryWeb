using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace tapStoryWebApi.Common.Helpers
{
    public static class ControllerHelper
    {
        public static string GetQueryStringValue(HttpActionContext ac, string key)
        {
            var queryString = ac.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            return queryString.ContainsKey(key) ? queryString[key] : String.Empty;            
        }
    }
}