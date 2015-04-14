using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace tapStoryWebApi.Routing
{
    public class RouteConfig
    {

        public static HttpConfiguration ConfigureRoutes()
        {
            var configuration = new HttpConfiguration();

            configuration.MapHttpAttributeRoutes();

            configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return configuration;
        }

    }
}