using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using System.Web.OData.Formatter.Deserialization;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using tapStoryWebApi;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Attributes;
using tapStoryWebApi.Middleware;
using tapStoryWebApi.ODataConfiguration;
using tapStoryWebApi.Routing;

[assembly: OwinStartup(typeof(Startup))]
namespace tapStoryWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //Routing
            var configuration = RouteConfig.ConfigureRoutes();

            //Global Filters
            configuration.Filters.Add(new ValidateViewModelAttribute());


            var cors = new EnableCorsAttribute("*", "*", "*");
            configuration.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //configuration.SuppressDefaultHostAuthentication();
            //configuration.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            //Configure OData
            configuration.MapODataServiceRoute("odata", "odata", ODataEdm.GetModel());
            
            //API/ODATA Formatters
            var odataFormatters = ODataMediaTypeFormatters.Create(new NullSerializerProvider(), new DefaultODataDeserializerProvider());
            var apiFormatter = configuration.Formatters.JsonFormatter;
            configuration.Formatters.Clear();
            configuration.Formatters.AddRange(odataFormatters);
            configuration.Formatters.Add(apiFormatter);
            //configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //Initialize Authorization Server
            AuthorizationService.ConfigureAuthorization(app);

            //Add Middleware Here
            app.Use(typeof (AuthorizationInitializer));

            app.UseWebApi(configuration);
        }


    }
}