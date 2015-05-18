using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Extensions;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Accounts.Controllers
{
    public class UsersController : ODataController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private ApplicationDbContext GetDbContext()
        {
            return Request.GetOwinContext().Get<ApplicationDbContext>();
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<ApplicationUser> Get()
        {
            return UserService.GetUsers(GetDbContext());
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public SingleResult<ApplicationUser> Get([FromODataUri] int key)
        {
            return SingleResult.Create(UserService.GetUser(GetDbContext(), key));
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<ApplicationUserRole> GetRoles([FromODataUri] int key)
        {
            return UserService.GetUserRolesForUser(GetDbContext(), key);
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<ApplicationUserClaim> GetClaims([FromODataUri] int key)
        {
            var x = GetDbContext().Users.Where(u => u.Id == key).SelectMany(s => s.Claims);
            return x;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<UserRelationship> GetPrimaryRelationships([FromODataUri] int key)
        {
            return UserService.GetPrimaryRelationshipsForUser(GetDbContext(), key);
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<UserRelationship> GetSecondaryRelationships([FromODataUri] int key)
        {
            return UserService.GetSecondaryRelationshipsForUser(GetDbContext(), key);
        }

    }
}
