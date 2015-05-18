using System.Linq;
using tapStoryWebApi.Common.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Accounts.Services
{
    public class UserRoleService : IDataService
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