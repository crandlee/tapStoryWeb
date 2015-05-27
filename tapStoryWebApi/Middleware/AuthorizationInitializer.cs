using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Middleware
{
    public class AuthorizationInitializer : OwinMiddleware
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AuthorizationInitializer(OwinMiddleware next)
            : base(next)
        {
        }

        public async override Task Invoke(IOwinContext context)
        {
            try
            {
                var identityDbContext = context.Get<ApplicationDbContext>();

                if (identityDbContext != null && identityDbContext.Users.FirstOrDefault() == null)
                {

                    Logger.Trace("Invoke: Invoking AuthorizationInitializer MiddleWare");
                    InitializeUserManagement(context);
                    Logger.Trace("Invoke: Invoked AuthorizationInitializer MiddleWare");

                }

                await Next.Invoke(context);

            } 
            catch (Exception e)
            {
                Logger.Error("Invoke threw an exception: {0}", e.ToString());
                throw;
            }

        }

        public async static void InitializeUserManagement(IOwinContext context)
        {
            var userManager = context.GetUserManager<ApplicationUserManager>();
            var roleManager = context.Get<ApplicationRoleManager>();

            var roles = new[] { "user", "admin", "superadmin" };
            foreach (var roleName in roles)
            {
                var roleResult = await RoleService.AddRole(roleManager, roleName);
                if (roleResult == null)
                {
                    Logger.Trace("InitializeUserManagement: Role {0} currently exists. Will not add.", roleName);
                }
                else
                {
                    if (roleResult == IdentityResult.Success)
                        Logger.Trace("InitializeUserManagement: Added role {0}.", roleName);
                    else
                        Logger.Error("InitializeUserManagement: Could not add role {0}", roleName);
                }
            }

            const string userName = "Admin";
            var userResult = UserService.AddUser(userManager, userName, "admin@tapStory.com", "tapStory", "Admin", "rE4$g*1W#*iuo");
            if (userResult == null)
            {
                Logger.Trace("InitializeUserManagement: User {0} currently exists. Will not add.", userName);
            }
            else
            {
                if (userResult == IdentityResult.Success)
                    Logger.Trace("InitializeUserManagement: Added user {0}.", userName);
                else
                    Logger.Error("InitializeUserManagement: Could not add user {0}", userName);

            }

            var roleUserRes = await RoleService.AddRoleToUser(userManager, UserService.GetUserByName(userManager, userName), RoleService.GetRoleByName(roleManager, Roles.Admin.ToString()));
            if (roleUserRes == IdentityResult.Success) {
                Logger.Trace("InitializeUserManagement: Added admin role to {0}.", userName);
                roleUserRes = await RoleService.AddRoleToUser(userManager, UserService.GetUserByName(userManager, userName), RoleService.GetRoleByName(roleManager, Roles.User.ToString()));
                if (roleUserRes == IdentityResult.Success)
                {
                    Logger.Trace("InitializeUserManagement: Added user role to {0}.", userName);
                }
                else
                {
                    Logger.Error("InitializeUserManagement: Could not add user role to user {0}", userName);
                }
            }
            else
                Logger.Error("InitializeUserManagement: Could not add admin role to user {0}", userName);

        }


    }
}