using System;
using Microsoft.AspNet.Identity;
using NLog;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Services
{
    public class UserService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static ApplicationUser GetUserByName(ApplicationUserManager userManager, string userName)
        {
            return userManager.FindByName(userName);
        }

        public static IdentityResult AddUser(ApplicationUserManager userManager, string userName, string email, string password, bool enableLockout = true)
        {
            try
            {
                Logger.Trace("AddUser: Searching for User {0}", userName);
                var user = userManager.FindByName(userName);
                Logger.Trace("AddUser: User {0} found = {1}", userName, user != null);
                if (user != null) return null;
                Logger.Trace("AddUser: Adding user with userName {0} and email {1}", userName, email);
                var newUser = new ApplicationUser { UserName = userName, Email = email };
                var res = userManager.Create(newUser, password);
                if (res != IdentityResult.Success) return res;
                Logger.Trace("AddUser: Added user with userName {0} and email {1}", userName, email);
                var lockoutResult = userManager.SetLockoutEnabled(newUser.Id, enableLockout);
                if (lockoutResult != IdentityResult.Success) Logger.Error("AddUser: Could not set lockout result for user {0}", userName);
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