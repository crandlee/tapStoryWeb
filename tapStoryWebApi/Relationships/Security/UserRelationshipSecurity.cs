using System;
using System.Linq;
using System.Security.Principal;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Relationships.DTO;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Relationships.Security
{
    public class UserRelationshipSecurity: SecurityService
    {

        public UserRelationshipSecurity(ApplicationDbContext ctx, IPrincipal currentPrincipal) : base(ctx, currentPrincipal)
        {
        }

        public IQueryable<ChildRelationshipModel> SecureChildrenQuery(IQueryable<ChildRelationshipModel> childRelationships)
        {
            if (childRelationships == null) throw new ArgumentException("childRelationships must be valid", "childRelationships");
            return childRelationships.Where(c => CurrentUserIsAdmin || CurrentUserIsActive && c.ParentId == CurrentUserId);

        }

        public IQueryable<FriendRelationshipModel> SecureFriendQuery(IQueryable<FriendRelationshipModel> friendRelationships)
        {
            if (friendRelationships == null) throw new ArgumentException("friendRelationships must be valid", "friendRelationships");
            return friendRelationships.Where(c => CurrentUserIsAdmin || (CurrentUserIsActive && (c.SourceFriendId == CurrentUserId || c.TargetFriendId == CurrentUserId)));

        }

        public IQueryable<GuardianRelationshipModel> SecureGuardianQuery(IQueryable<GuardianRelationshipModel> guardianRelationships)
        {
            if (guardianRelationships == null) throw new ArgumentException("guardianRelationships must be valid", "guardianRelationships");
            return guardianRelationships.Where(c => CurrentUserIsAdmin || (CurrentUserIsActive && (c.ChildId == CurrentUserId)));
        }

        public IQueryable<ChildRelationshipModel> SecureGuardianshipQuery(IQueryable<ChildRelationshipModel> childRelationships)
        {

            if (childRelationships == null) throw new ArgumentException("childRelationships must be valid", "childRelationships");
            return childRelationships.Where(c => CurrentUserIsAdmin || (CurrentUserIsActive && (c.ParentId == CurrentUserId)));

        }

        public bool CanCreatePendingFriendship(int sourceUserId)
        {
            return CurrentUserIsAdultAndSelf(sourceUserId) || CurrentUserIsStrictGuardian(sourceUserId);
        }

        public bool CanAcceptPendingFriendship(int sourceUserId)
        {
            return CurrentUserIsAdultAndSelf(sourceUserId) || CurrentUserIsStrictGuardian(sourceUserId);
        }

        public bool CanUnfriend(int sourceUserId)
        {
            return (CurrentUserIsAdmin  || CurrentUserIsAdultAndSelf(sourceUserId) ||
                    CurrentUserIsNonStrictGuardian(sourceUserId));

        }
    }
}