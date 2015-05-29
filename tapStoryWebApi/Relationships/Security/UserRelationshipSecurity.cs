using System;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using tapStoryWebApi.Relationships.ViewModels;

namespace tapStoryWebApi.Relationships.Security
{
    public class UserRelationshipSecurity
    {
        private readonly IPrincipal _currentUser;

        public UserRelationshipSecurity(IPrincipal currentUser)
        {
            _currentUser = currentUser;
            if (_currentUser == null) throw new ArgumentException("currentUser must be valid", "currentUser");
        }

        public IQueryable<ChildRelationshipViewModel> SecureChildrenQuery(IQueryable<ChildRelationshipViewModel> childRelationships)
        {
            var id = _currentUser.Identity.GetUserId<int>();
            if (childRelationships == null) throw new ArgumentException("childRelationships must be valid", "childRelationships");
            return childRelationships.Where(c => c.ParentId == id);

        }

        public IQueryable<FriendRelationshipViewModel> SecureFriendQuery(IQueryable<FriendRelationshipViewModel> friendRelationships)
        {
            var id = _currentUser.Identity.GetUserId<int>();
            if (friendRelationships == null) throw new ArgumentException("friendRelationships must be valid", "friendRelationships");
            return friendRelationships.Where(c => c.SourceFriendId == id || c.TargetFriendId == id);

        }

        public IQueryable<GuardianRelationshipViewModel> SecureGuardianQuery(IQueryable<GuardianRelationshipViewModel> guardianRelationships)
        {

            var id = _currentUser.Identity.GetUserId<int>();
            if (guardianRelationships == null) throw new ArgumentException("guardianRelationships must be valid", "guardianRelationships");
            return guardianRelationships.Where(c => c.ChildId == id);

        }

    }
}