using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Services
{
    public class RoleService
    {
        public static ApplicationRole GetRoleByName(ApplicationRoleManager roleManager, string roleName)
        {
            return roleManager.FindByName(roleName);
        }


        public async static Task<IdentityResult> AddRole(ApplicationRoleManager roleManager, string roleName, string description = null)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null) return null;
            if (description == null) description = roleName;
            return await roleManager.CreateAsync(new ApplicationRole(roleName, description));
        }

        public async static Task<IdentityResult> AddRoleToUser(ApplicationUserManager userManager, ApplicationUser user, ApplicationRole role)
        {
            var rolesForUser = await userManager.GetRolesAsync(user.Id);
            return await (!rolesForUser.Contains(role.Name) ? userManager.AddToRoleAsync(user.Id, role.Name) : Task.FromResult<IdentityResult>(null));

        }

        public async static Task<IdentityResult> AddRoleToUserAsync(ApplicationUserManager userManager, int userId, string role)
        {
            var rolesForUser = await userManager.GetRolesAsync(userId);
            return await (!rolesForUser.Contains(role) ? userManager.AddToRoleAsync(userId, role) : Task.FromResult<IdentityResult>(null));
        }

        public async static Task<IdentityResult> RemoveRoleFromUserAsync(ApplicationUserManager userManager, int userId, string role)
        {
            var rolesForUser = await userManager.GetRolesAsync(userId);
            return await
                    (rolesForUser.Contains(role)
                        ? userManager.RemoveFromRoleAsync(userId, role)
                        : Task.FromResult<IdentityResult>(null));

        }

        public async static Task<IEnumerable<string>> GetRolesAsync(ApplicationUserManager userManager, int userId)
        {
            return await userManager.GetRolesAsync(userId);
        }

        public async static Task<bool> UserHasRoleAsync(ApplicationUserManager userManager, int userId, string role)
        {
            var rolesForUser = await userManager.GetRolesAsync(userId);
            return rolesForUser.Contains(role);
        }

        public async static Task<bool> UserHasRolesAsync(ApplicationUserManager userManager, int userId, string[] roles)
        {
            var rolesForUser = await userManager.GetRolesAsync(userId);
            return RoleCheck(rolesForUser, roles);
        }

        public static bool UserHasRoles(ApplicationUserManager userManager, int userId, string[] roles)
        {
            var rolesForUser = userManager.GetRoles(userId);
            return RoleCheck(rolesForUser, roles);
        }

        private static bool RoleCheck(ICollection<string> sourceRoles, IReadOnlyCollection<string> testRoles)
        {
            var countRolesMatched = sourceRoles.Sum(role => (sourceRoles.Contains(role)) ? 1 : 0);
            return countRolesMatched > 0 && countRolesMatched == testRoles.Count;            
        }
    }
}