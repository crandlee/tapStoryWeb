using System;
using System.Linq;
using System.Security.Principal;
using tapStoryWebApi.Common.BaseClasses;
using tapStoryWebApi.Relationships.ViewModels;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Relationships.Security
{
    public class UserRelationshipSecurity: SecurityService
    {

        public UserRelationshipSecurity(ApplicationDbContext ctx, IPrincipal currentPrincipal) : base(ctx, currentPrincipal)
        {
        }

        public IQueryable<ChildRelationshipViewModel> SecureChildrenQuery(IQueryable<ChildRelationshipViewModel> childRelationships)
        {
            if (childRelationships == null) throw new ArgumentException("childRelationships must be valid", "childRelationships");
            return childRelationships.Where(c => CurrentUserIsAdmin || CurrentUserIsActive && c.ParentId == CurrentUserId);

        }

        public IQueryable<FriendRelationshipViewModel> SecureFriendQuery(IQueryable<FriendRelationshipViewModel> friendRelationships)
        {
            if (friendRelationships == null) throw new ArgumentException("friendRelationships must be valid", "friendRelationships");
            return friendRelationships.Where(c => CurrentUserIsAdmin || (CurrentUserIsActive && (c.SourceFriendId == CurrentUserId || c.TargetFriendId == CurrentUserId)));

        }

        public IQueryable<GuardianRelationshipViewModel> SecureGuardianQuery(IQueryable<GuardianRelationshipViewModel> guardianRelationships)
        {

            if (guardianRelationships == null) throw new ArgumentException("guardianRelationships must be valid", "guardianRelationships");
            return guardianRelationships.Where(c => CurrentUserIsAdmin || (CurrentUserIsActive && c.ChildId == CurrentUserId));

        }

        public bool CanCreatePendingFriendship()
        {
            return (CurrentUserIsAdmin || CurrentUserIsAdult);
        }

    }
}