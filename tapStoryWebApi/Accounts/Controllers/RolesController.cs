using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Accounts.Controllers
{
    public class RolesController : ODataController
    {

        private ApplicationDbContext GetDbContext()
        {
            return Request.GetOwinContext().Get<ApplicationDbContext>();
        }

        [EnableQuery]
        public IQueryable<ApplicationRole> Get()
        {
            return RoleService.GetRoles(GetDbContext());
        }

        //[EnableQuery]
        //public SingleResult<ApplicationRole> Get([FromODataUri] int roleId, [FromODataUri] int userId)
        //{
        //    return SingleResult.Create(new ApplicationRole());
        //}

        [EnableQuery]
        public IQueryable<ApplicationUserRole> GetUsers([FromODataUri] int key)
        {
            var ur = RoleService.GetUserRolesForRole(GetDbContext(), key);
            return ur;
        }

        [AcceptVerbs("POST")]
        public IHttpActionResult Post(ApplicationUserRole userRole)
        {
            return Created("OK");
        }

    }
}