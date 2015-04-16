using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Accounts.ViewModels;
using tapStoryWebData.Identity.Contexts;

namespace tapStoryWebApi.Accounts.Controllers
{
    public class UsersController : ODataController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private ApplicationIdentityDbContext GetDbContext()
        {
            return Request.GetOwinContext().Get<ApplicationIdentityDbContext>();
        }

        [EnableQuery]
        public IQueryable<ApplicationUserViewModel> GetUsers()
        {
            return UserService.GetUsers(GetDbContext());
        }

        [EnableQuery]
        public SingleResult<ApplicationUserViewModel> GetUser([FromODataUri] int key)
        {
            return SingleResult.Create(UserService.GetUser(GetDbContext(), key));
        }
    }
}
