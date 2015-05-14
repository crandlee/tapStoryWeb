using System.Data.Entity;
using System.Net.Http;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Common.Factories
{
    public static class ServiceFactory
    {
        public static FileService GetFileService(ApplicationDbContext ctx, HttpRequestMessage request, IPrincipal principal)
        {
            ApplicationUser user = null;
            if (principal != null && principal.Identity != null)
            {
                var userManager = request.GetOwinContext().Get<ApplicationUserManager>();
                user = userManager.FindById(principal.Identity.GetUserId<int>());
            }
            var auditService = new AuditService(ctx, user);
            return new FileService(ctx, auditService);

        }

        public static T GetDbContext<T>(HttpRequestMessage request)
        {
            return request.GetOwinContext().Get<T>();
        }

    }
}