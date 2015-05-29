using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Relationships.ViewModels;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.Services
{
    public class UserRelationshipService : IDataService
    {

        private readonly AuditService _auditService;
        private readonly ApplicationDbContext _ctx;

        private readonly Expression<Func<UserRelationship, ChildRelationshipViewModel>> _getChildViewModel = ur => new ChildRelationshipViewModel()
        {
            Id = ur.Id,
            ParentId = ur.PrimaryMemberId,
            ChildId = ur.SecondaryMemberId,
            RelationshipStatus =  ur.RelationshipStatus,
            Parent = ur.PrimaryMember,
            Child = ur.SecondaryMember
        };

        private readonly Expression<Func<UserRelationship, FriendRelationshipViewModel>> _getFriendViewModel = ur => new FriendRelationshipViewModel()
        {
            Id = ur.Id,
            SourceFriendId = ur.PrimaryMemberId,
            TargetFriendId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            SourceFriend = ur.PrimaryMember,
            TargetFriend = ur.SecondaryMember
        };

        private readonly Expression<Func<UserRelationship, GuardianRelationshipViewModel>> _getGuardianViewModel = ur => new GuardianRelationshipViewModel()
        {
            Id = ur.Id,
            ParentId = ur.PrimaryMemberId,
            ChildId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            Parent = ur.PrimaryMember,
            Child = ur.SecondaryMember
        };

        public UserRelationshipService(ApplicationDbContext ctx, AuditService auditService)
        {
            _auditService = auditService;
            _ctx = ctx;
        }

        public IQueryable<ChildRelationshipViewModel> GetChildRelationships(int? parentId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == parentId || parentId == null) && ur.RelationshipType == RelationshipType.Child).Select(_getChildViewModel);
        }

        public IQueryable<GuardianRelationshipViewModel> GetGuardianRelationships(int? childId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.SecondaryMemberId == childId || childId == null) && ur.RelationshipType == RelationshipType.Child).Select(_getGuardianViewModel);
        }

        public IQueryable<FriendRelationshipViewModel> GetFriendRelationships(int? sourceFriendId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == sourceFriendId || sourceFriendId == null) && ur.RelationshipType == RelationshipType.Child).Select(_getFriendViewModel);
        }

        public IQueryable<UserRelationship> GetUserRelationships(int? primaryMemberId = null, int? secondaryMemberId = null, RelationshipType? relType = null)
        {
            return _ctx.UserRelationships.Where(ur => (ur.PrimaryMemberId == primaryMemberId || primaryMemberId == null) 
                && (ur.SecondaryMemberId == secondaryMemberId || secondaryMemberId == null)
                && (ur.RelationshipType == relType || relType == null));
        }

    }
}