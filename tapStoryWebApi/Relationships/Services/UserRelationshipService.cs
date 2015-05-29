using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using tapStoryWebApi.Common.Helpers;
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


        public UserRelationshipService(ApplicationDbContext ctx, AuditService auditService)
        {
            _auditService = auditService;
            _ctx = ctx;
        }

        public IQueryable<ChildRelationshipViewModel> GetChildRelationships(int? parentId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == parentId || parentId == null) && ur.RelationshipType == RelationshipType.Child).Select(ViewModelBuilder.GetChildRelationshipViewModel);
        }

        public IQueryable<GuardianRelationshipViewModel> GetGuardianRelationships(int? childId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == childId || childId == null) && ur.RelationshipType == RelationshipType.Guardian).Select(ViewModelBuilder.GetGuardianRelationshipViewModel);
        }

        public IQueryable<FriendRelationshipViewModel> GetFriendRelationships(int? sourceFriendId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == sourceFriendId || sourceFriendId == null) && ur.RelationshipType == RelationshipType.Friend).Select(ViewModelBuilder.GetFriendRelationshipViewModel);
        }

        public async Task<FriendRelationshipViewModel> CreatePendingFriendship(int sourceFriendId, int targetFriendId)
        {
            var existingActiveRelationship = await _ctx.UserRelationships.Where(ur => 
                ((ur.PrimaryMemberId == sourceFriendId && ur.SecondaryMemberId == targetFriendId) || (ur.SecondaryMemberId == sourceFriendId && ur.PrimaryMemberId == targetFriendId))
                && ur.RelationshipStatus != RelationshipStatus.Inactive).AnyAsync();

            if (existingActiveRelationship) return null;
            var newRel = new UserRelationship()
            {
                PrimaryMemberId = sourceFriendId,
                SecondaryMemberId = targetFriendId,
                RelationshipStatus = RelationshipStatus.Pending,
                RelationshipType = RelationshipType.Friend
            };
            _ctx.UserRelationships.Add(newRel);
            _ctx.UserRelationships.Add(new UserRelationship()
            {
                PrimaryMemberId = targetFriendId,
                SecondaryMemberId = sourceFriendId,
                RelationshipStatus = RelationshipStatus.PendingAck,
                RelationshipType = RelationshipType.Friend
            });
            return ViewModelBuilder.GetFriendRelationshipViewModel.Invoke(newRel);
        }

        

    }
}