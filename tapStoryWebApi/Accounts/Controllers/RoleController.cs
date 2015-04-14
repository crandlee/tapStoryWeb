using System.Net.Http;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.ViewModels;
using RoleService = tapStoryWebApi.Accounts.Services.RoleService;

namespace tapStoryWebApi.Accounts.Controllers
{
    [Authorize]
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {

        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public RoleController()
        {
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return Request.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // POST api/Role/
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> AddRole(AddRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await RoleService.AddRoleToUser(Request.GetOwinContext().Get<ApplicationUserManager>(), model.UserId,
                model.RoleName);

            return !result.Succeeded ? GetErrorResult(result) : Ok();

            return Ok();
        }

        #region Helpers
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded) return null;
            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {
                // No ModelState errors are available to send, so just return an empty BadRequest.
                return BadRequest();
            }

            return BadRequest(ModelState);
        }
        #endregion

    }
}
