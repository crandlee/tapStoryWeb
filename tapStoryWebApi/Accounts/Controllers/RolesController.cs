using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Attributes;

namespace tapStoryWebApi.Accounts.Controllers
{
    [Authorize]
    [RoutePrefix("api/Roles")]
    public class RolesController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // GET api/Roles/
        [AcceptVerbs("GET")]
        [IsAdmin]
        public async Task<IHttpActionResult> GetRoles(int userId)
        {
            try
            {
                var userManager = Request.GetOwinContext().Get<ApplicationUserManager>();
                var roles =
                    await RoleService.GetRolesAsync(userManager, userId);
                return Ok(new { Roles = roles });

            }
            catch (Exception e)
            {
                Logger.Error("GetRoles(GET): Error thrown: {0}", e.ToString());
                throw;

            }

        }

    }
}