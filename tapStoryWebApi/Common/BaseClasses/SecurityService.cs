using System;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Common.BaseClasses
{
    public class SecurityService
    {

        public SecurityService(ApplicationDbContext ctx,IPrincipal currentPrincipal)
        {
            CurrentPrincipal = currentPrincipal;
            if (currentPrincipal == null) throw new ArgumentException("currentPrincipal must be valid", "currentPrincipal");
            CurrentUserId = currentPrincipal.Identity.GetUserId<int>();
            CurrentUser = ctx.Users.FirstOrDefault(x => x.Id == CurrentUserId);
            CurrentUserIsActive = (CurrentUser != null) && (CurrentUser.IsActive);
            CurrentUserIsAdmin = CurrentUserIsActive && currentPrincipal.IsInRole("admin") || currentPrincipal.IsInRole("superadmin");
            CurrentUserIsSuperAdmin = CurrentUserIsActive && currentPrincipal.IsInRole("superadmin");
            
        }

        public IPrincipal CurrentPrincipal { get; private set; }
        public ApplicationUser CurrentUser { get; private set; }
        public int CurrentUserId { get; private set; }
        public bool CurrentUserIsActive { get; private set; }
        public bool CurrentUserIsAdmin { get; private set; }
        public bool CurrentUserIsSuperAdmin { get; private set; }

        public bool CurrentUserIsAdult
        {
            get
            {
                return (CurrentUser != null) && CurrentUserIsActive && (!CurrentUser.IsMinor);                
            }
        }


    }
}
