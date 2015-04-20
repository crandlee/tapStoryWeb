using System.Linq;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Services
{
    public class UserRoleService
    {
        public static IQueryable<ApplicationUserRole> GetUserRole(ApplicationDbContext ctx, int roleId, int userId)
        {
            return ctx.Users.Where(u => u.Id == userId).SelectMany(u => u.Roles.Where(ur => ur.RoleId == roleId));
            
        }

        public static IQueryable<ApplicationUserRole> GetRoles(ApplicationDbContext ctx, int userId)
        {
            return ctx.Users.Where(u => u.Id == userId).SelectMany(u => u.Roles);
        }

    }
}