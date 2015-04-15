using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Services
{
    public class RoleService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static ApplicationRole GetRoleByName(ApplicationRoleManager roleManager, string roleName)
        {
            return roleManager.FindByName(roleName);
        }


        public static IdentityResult AddRole(ApplicationRoleManager roleManager, string roleName, string description = null)
        {
            try
            {
                Logger.Trace("AddRole: Searching for Role {0}", roleName);
                var role = roleManager.FindByName(roleName);
                Logger.Trace("AddRole: Role {0} found = {1}", roleName, role != null);
                if (role != null) return null;
                Logger.Trace("AddRole: Adding role {0}", roleName);
                if (description == null) description = roleName;
                role = new ApplicationRole(roleName, description);
                var res = roleManager.Create(role);
                Logger.Trace("AddRole: Added role {0}", roleName);
                return res;
            }
            catch (Exception e)
            {
                Logger.Error("AddRole: Error thrown: {0}", e.ToString());
                throw;
            }
        }

        public static IdentityResult AddRoleToUser(ApplicationUserManager userManager, ApplicationUser user, ApplicationRole role)
        {
            try
            {
                var rolesForUser = userManager.GetRoles(user.Id);
                return !rolesForUser.Contains(role.Name) ? userManager.AddToRole(user.Id, role.Name) : null;
            }
            catch (Exception e)
            {
                Logger.Error("AddRoleToUser: Error thrown: {0}", e.ToString());
                throw;
            }

        }

        public async static Task<IdentityResult> AddRoleToUser(ApplicationUserManager userManager, int userId, string role)
        {
            try
            {
                var rolesForUser = await userManager.GetRolesAsync(userId);
                return await (!rolesForUser.Contains(role) ? userManager.AddToRoleAsync(userId, role) : Task.FromResult<IdentityResult>(null));

            }
            catch (Exception e)
            {
                Logger.Error("AddRoleToUser: Error thrown: {0}", e.ToString());
                throw;
            }


        }


    }
}