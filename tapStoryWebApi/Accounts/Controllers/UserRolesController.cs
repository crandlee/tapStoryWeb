using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Microsoft.AspNet.Identity.Owin;
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
        public IQueryable<ApplicationUserRole> Get()
        {
            return GetDbContext().Users.Where(u => u.Id == 1).SelectMany(u => u.Roles);
        }

    }
}