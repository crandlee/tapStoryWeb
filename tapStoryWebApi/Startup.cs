using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding.Binders;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using Microsoft.OData.Edm;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using tapStoryWebApi;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Accounts.ViewModels;
using tapStoryWebApi.Attributes;
using tapStoryWebApi.Middleware;
using tapStoryWebApi.Relationships.ViewModels;
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
            configuration.MapODataServiceRoute("odata", "odata", GetModel());
            
            //API/ODATA Formatters
            var odataFormatters = ODataMediaTypeFormatters.Create();
            var apiFormatter = configuration.Formatters.JsonFormatter;
            configuration.Formatters.Clear();
            configuration.Formatters.AddRange(odataFormatters);
            configuration.Formatters.Add(apiFormatter);
            //configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            //Initialize Authorization Server
            AuthorizationService.ConfigureAuthorization(app);

            //Add Middleware Here
            app.Use(typeof (AuthorizationInitializer));

            app.UseWebApi(configuration);
        }

        public static IEdmModel GetModel()
        {

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<ApplicationUserViewModel>("Users");
            builder.EntitySet<UserRelationshipViewModel>("UserRelationships");
            return builder.GetEdmModel();

        }

    }
}