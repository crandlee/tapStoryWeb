using System;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Common.Services
{
    public class SecurityService
    {

        private readonly ApplicationDbContext _ctx;

        public SecurityService(ApplicationDbContext ctx, IPrincipal currentPrincipal)
        {
            CurrentPrincipal = currentPrincipal;
            if (currentPrincipal == null) throw new ArgumentException("currentPrincipal must be valid", "currentPrincipal");
            CurrentUserId = currentPrincipal.Identity.GetUserId<int>();
            CurrentUser = ctx.Users.FirstOrDefault(x => x.Id == CurrentUserId);
            CurrentUserIsActive = (CurrentUser != null) && (CurrentUser.IsActive);
            CurrentUserIsAdmin = CurrentUserIsActive && currentPrincipal.IsInRole("admin") || currentPrincipal.IsInRole("superadmin");
            CurrentUserIsSuperAdmin = CurrentUserIsActive && currentPrincipal.IsInRole("superadmin");
            _ctx = ctx;
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

        public bool CurrentUserIsStrictGuardian(int targetUserId)
        {
            return CurrentUserIsAdmin || (CurrentUserIsGuardian(targetUserId));
        }

        public bool CurrentUserIsNonStrictGuardian(int targetUserId)
        {
            return CurrentUserIsAdmin || (CurrentUserIsGuardian(targetUserId));
        }

        public bool CurrentUserIsGuardian(int targetUserId)
        {

            var currentUserIsMinor = _ctx.UserRelationships.Any(
                ur =>
                    ur.RelationshipType == RelationshipType.Guardian &&
                    ur.RelationshipStatus == RelationshipStatus.Active && ur.PrimaryMemberId ==
                        CurrentUserId && ur.SecondaryMemberId == targetUserId);

            //If target stops being minor, then guardianship ends.  Not sure how the changing of this flag gets
            //determined yet.
            var targetIsMinor = _ctx.Users.Any(u => u.Id == targetUserId && u.IsMinor);

            return currentUserIsMinor && targetIsMinor;
        }

        public bool CurrentUserIsAdultAndSelf(int targetUserId)
        {
            return CurrentUserIsAdult && (CurrentUserId == targetUserId);
        }

        public bool CurrentUserIsAdminAndTargetAdult(int targetUserId)
        {
            return CurrentUserIsAdmin && (CurrentUserId == targetUserId);
        }

    }
}
