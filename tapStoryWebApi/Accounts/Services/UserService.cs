using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Common.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Accounts.Services
{
    public class UserService : IDataService  
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static ApplicationUser GetUserByName(ApplicationUserManager userManager, string userName)
        {
            return userManager.FindByName(userName);
        }

        public static IQueryable<ApplicationUser> GetUser(ApplicationDbContext ctx, int id)
        {
            return ctx.Users.Where(u => u.Id == id);
        }

        public static IQueryable<ApplicationUser> GetUsers(ApplicationDbContext ctx)
        {
            return ctx.Users;
        }

        public static IQueryable<ApplicationUserRole> GetUserRolesForUser(ApplicationDbContext ctx, int id)
        {
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(ctx));
            var user = ctx.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);
            if (user == null) return new ApplicationUserRole[] {}.AsQueryable();
            return user.Roles.Select(r =>
            {
                var role = roleManager.FindById(r.RoleId);
                if (role != null) r.RoleName = role.Name;
                r.UserName = user.UserName;
                return r;
            }).AsQueryable();
        }

        public static IQueryable<UserRelationship> GetPrimaryRelationshipsForUser(ApplicationDbContext ctx, int id)
        {
            var usr = ctx.Users.Where(u => u.Id == id).Include("PrimaryRelationships");
            return usr.SelectMany(s => s.PrimaryRelationships);
        }

        public static IQueryable<UserRelationship> GetSecondaryRelationshipsForUser(ApplicationDbContext ctx, int id)
        {
            var usr = ctx.Users.Where(u => u.Id == id).Include("SecondaryRelationships");
            return usr.SelectMany(s => s.SecondaryRelationships);
        }

        public static IdentityResult AddUser(ApplicationUserManager userManager, string userName, string email, string firstName, string lastName, string password, bool enableLockout = true)
        {
            try
            {
                Logger.Trace("AddUser: Searching for User {0}", userName);
                var user = userManager.FindByName(userName);
                Logger.Trace("AddUser: User {0} found = {1}", userName, user != null);
                if (user != null) return null;
                Logger.Trace("AddUser: Adding user with userName {0} and email {1}", userName, email);
                var newUser = new ApplicationUser { UserName = userName, Email = email, FirstName = firstName, LastName = lastName };
                var res = userManager.Create(newUser, password);
                if (res != IdentityResult.Success) return res;
                Logger.Trace("AddUser: Added user with userName {0} and email {1}", userName, email);
                //var lockoutResult = userManager.SetLockoutEnabled(newUser.Id, enableLockout);
                //if (lockoutResult != IdentityResult.Success) Logger.Error("AddUser: Could not set lockout result for user {0}", userName);
                Logger.Trace("AddUser: Set lockout {0} for user {1}", enableLockout, userName);
                return res;
            }
            catch (Exception e)
            {
                Logger.Error("AddUser: Error thrown: {0}", e.ToString());
                throw;
            }
        }
    }
}