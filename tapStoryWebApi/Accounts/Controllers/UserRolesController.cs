using System.Linq;
using System.Net.Http;
using System.Web.OData;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Controllers
{
    public class UserRolesController : ODataController
    {
        private ApplicationDbContext GetDbContext()
        {
            return Request.GetOwinContext().Get<ApplicationDbContext>();
        }

        [EnableQuery]
        public IQueryable<ApplicationUserRole> GetUserRoles([FromODataUri] int key)
        {
            return UserRoleService.GetRoles(GetDbContext(), key);
        }

        //[EnableQuery]
        //public SingleResult<ApplicationUserRole> GetUserRole([FromODataUri] int key)
        //{
        //    //return SingleResult.Create(RoleService.GetRole(GetDbContext(), key));
        //    return null;
        //}

    }
}