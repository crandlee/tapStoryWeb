using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Controllers
{
    public class UsersController : ODataController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private ApplicationDbContext GetDbContext()
        {
            return Request.GetOwinContext().Get<ApplicationDbContext>();
        }

        [EnableQuery]        
        public IQueryable<ApplicationUser> Get()
        {
            return UserService.GetUsers(GetDbContext());
        }

        [EnableQuery]
        public SingleResult<ApplicationUser> Get([FromODataUri] int key)
        {
            return SingleResult.Create(UserService.GetUser(GetDbContext(), key));
        }

        [EnableQuery]
        public IQueryable<ApplicationUserRole> GetRoles([FromODataUri] int key)
        {
            return UserService.GetUserRolesForUser(GetDbContext(), key);
        }

        [EnableQuery]
        public IQueryable<UserRelationship> GetPrimaryRelationships([FromODataUri] int key)
        {
            var ur  = UserService.GetPrimaryRelationshipsForUser(GetDbContext(), key);
            return ur;
        }

        [EnableQuery]
        public IQueryable<UserRelationship> GetSecondaryRelationships([FromODataUri] int key)
        {
            var ur = UserService.GetSecondaryRelationshipsForUser(GetDbContext(), key);
            return ur;
        }

    }
}
