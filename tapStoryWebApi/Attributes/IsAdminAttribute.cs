using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.Services;

namespace tapStoryWebApi.Attributes
{
    public class IsAdminAttribute : AuthorizeAttribute
    {

        private readonly bool _isSelf;
        private readonly string _idField;

        public IsAdminAttribute(bool isSelf = false, string userIdField = "UserId")
        {
            _isSelf = isSelf;
            _idField = userIdField;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal;

            if (user == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            } 

            //Check for matching userId property if this functionality was enabled by attribute properties.
            if (!AllowedUserId(actionContext, user.Identity.GetUserId<int>()))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            //Validate admin roles
            //NOTE:  Made this synchronous because making it async conflicts with the potential async calls that the controller might use.  Cannot queue up two async calls with EF
            var hasRoles = RoleService.UserHasRoles(actionContext.Request.GetOwinContext().Get<ApplicationUserManager>(),
                        user.Identity.GetUserId<int>(), new[] { tapStoryWebData.Identity.Models.Roles.Admin.ToString(), tapStoryWebData.Identity.Models.Roles.SuperAdmin.ToString() });
            if (hasRoles)
            {
                base.OnAuthorization(actionContext);                
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
            
        }

        private bool AllowedUserId(HttpActionContext actionContext, int currentUserId)
        {

            if (!_isSelf) return true;
            if (actionContext.ActionArguments[_idField] == null) return false;
            int userId;
            if (!Int32.TryParse(actionContext.ActionArguments[_idField].ToString(), out userId)) return false;
            return userId == currentUserId;

        }
    }
}