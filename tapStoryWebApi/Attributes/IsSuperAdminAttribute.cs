using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.Services;

namespace tapStoryWebApi.Attributes
{
    public class IsSuperAdminAttribute : AuthorizeAttribute
    {
        public async override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal;

            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            var hasRole = await RoleService.UserHasRoleAsync(actionContext.Request.GetOwinContext().Get<ApplicationUserManager>(),
                        user.Identity.GetUserId<int>(), tapStoryWebData.Identity.Models.Roles.SuperAdmin.ToString());
            if (hasRole)
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }

        }
    }
}