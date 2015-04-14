using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Security.OAuth;

namespace tapStoryWebApi
{
    class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {

            var cors = new EnableCorsAttribute("*", "*", "*");
            configuration.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            configuration.SuppressDefaultHostAuthentication();
            configuration.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
        }
    }
}