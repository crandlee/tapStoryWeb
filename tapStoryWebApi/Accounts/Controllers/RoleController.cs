using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.DTO;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Attributes;

namespace tapStoryWebApi.Accounts.Controllers
{
    [Authorize]
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // POST api/Role/
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> AddRole(AddRoleBindingModel model)
        {
            try
            {
                var result = await RoleService.AddRoleToUserAsync(Request.GetOwinContext().Get<ApplicationUserManager>(), model.UserId,
                    model.RoleName);

                return (result != null && !result.Succeeded) ? GetErrorResult(result) : Ok();
            } 
            catch (Exception e)
            {
                Logger.Error("AddRole(POST): Error thrown: {0}", e.ToString());
                throw;

            }

        }

        // POST api/Role/
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> RemoveRole(AddRoleBindingModel model)
        {
            try
            {
                var result = await RoleService.RemoveRoleFromUserAsync(Request.GetOwinContext().Get<ApplicationUserManager>(), model.UserId,
                    model.RoleName);

                return (result != null && !result.Succeeded) ? GetErrorResult(result) : Ok();
            }
            catch (Exception e)
            {
                Logger.Error("RemoveRole(POST): Error thrown: {0}", e.ToString());
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
