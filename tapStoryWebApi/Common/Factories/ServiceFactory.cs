using System.Net.Http;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Files.Services;
using tapStoryWebApi.Relationships.Services;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Common.Factories
{
    public static class ServiceFactory
    {

        public static UserRelationshipService GetUserRelationshipService(ApplicationDbContext ctx, HttpRequestMessage request, IPrincipal principal)
        {
            var auditService = GetAuditService(ctx, request, principal);
            return new UserRelationshipService(ctx, auditService);
        }

        public static FileDataService GetFileService(ApplicationDbContext ctx, HttpRequestMessage request, IPrincipal principal)
        {
            var auditService = GetAuditService(ctx, request, principal);
            return new FileDataService(ctx, auditService);
        }

        public static BookService GetBookService(ApplicationDbContext ctx, HttpRequestMessage request, IPrincipal principal)
        {
            var auditService = GetAuditService(ctx, request, principal);
            return new BookService(ctx, auditService);
        }

        private static AuditService GetAuditService(ApplicationDbContext ctx, HttpRequestMessage request, IPrincipal principal)
        {
            if (principal == null || principal.Identity == null) return new AuditService(ctx, null);

            var userManager = request.GetOwinContext().Get<ApplicationUserManager>();
            var user = userManager.FindById(principal.Identity.GetUserId<int>());
            return new AuditService(ctx, user);
        }


        public static T GetDbContext<T>(HttpRequestMessage request)
        {
            return request.GetOwinContext().Get<T>();
        }

    }
}