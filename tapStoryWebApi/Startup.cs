using Microsoft.Owin;
using Owin;
using tapStoryWebApi;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Middleware;
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

            //Initialize Authorization Server
            AuthorizationService.ConfigureAuthorization(app);

            //Add Middleware Here
            app.Use(typeof (AuthorizationInitializer));

            app.UseWebApi(configuration);
        }

    }
}