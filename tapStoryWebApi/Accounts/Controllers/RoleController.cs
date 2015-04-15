using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Accounts.ViewModels;

namespace tapStoryWebApi.Accounts.Controllers
{
    [Authorize]
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ApplicationUserManager _userManager;

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
            try
            {
                var result = await RoleService.AddRoleToUser(Request.GetOwinContext().Get<ApplicationUserManager>(), model.UserId,
                    model.RoleName);

                return !result.Succeeded ? GetErrorResult(result) : Ok();
            } 
            catch (Exception e)
            {
                Logger.Error("AddRole(POST): Error thrown: {0}", e.ToString());
                throw;

            }

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
