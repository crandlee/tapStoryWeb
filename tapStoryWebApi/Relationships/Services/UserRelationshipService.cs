using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNet.Identity;
using tapStoryWebApi.Accounts.Configuration;
using tapStoryWebApi.Accounts.DTO;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebApi.Common.Helpers;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Relationships.DTO;
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

        public IQueryable<ChildRelationshipModel> GetChildRelationships(int? parentId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == parentId || parentId == null) && ur.RelationshipType == RelationshipType.Child).Select(DtoBuilder.GetChildRelationshipViewModel);
        }

        public IQueryable<GuardianRelationshipModel> GetGuardianRelationships(int? childId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == childId || childId == null) && ur.RelationshipType == RelationshipType.Guardian).Select(DtoBuilder.GetGuardianRelationshipViewModel);
        }

        public IQueryable<FriendRelationshipModel> GetFriendRelationships(int? sourceFriendId = null)
        {
            return _ctx.UserRelationships.AsQueryable().Include(ur => ur.PrimaryMember).Include(ur => ur.SecondaryMember)
                .Where(ur => (ur.PrimaryMemberId == sourceFriendId || sourceFriendId == null) && ur.RelationshipType == RelationshipType.Friend).Select(DtoBuilder.GetFriendRelationshipViewModel);
        }

        public async Task<FriendRelationshipModel> CreatePendingFriendship(int sourceFriendId, int targetFriendId)
        {

            var sourceRelationship = await _ctx.UserRelationships.Where(ur => (ur.PrimaryMemberId == sourceFriendId && ur.SecondaryMemberId == targetFriendId)).FirstOrDefaultAsync();
            var targetRelationship = await _ctx.UserRelationships.Where(ur => (ur.SecondaryMemberId == targetFriendId && ur.PrimaryMemberId == sourceFriendId)).FirstOrDefaultAsync();

            if (sourceRelationship == null)
            {
                sourceRelationship = new UserRelationship()
                {
                    PrimaryMemberId = sourceFriendId,
                    SecondaryMemberId = targetFriendId,
                    RelationshipStatus = RelationshipStatus.Pending,
                    RelationshipType = RelationshipType.Friend
                };
                _ctx.UserRelationships.Add(sourceRelationship);
            }
            if (sourceRelationship.RelationshipStatus == RelationshipStatus.Inactive) sourceRelationship.RelationshipStatus = RelationshipStatus.Active;
            if (targetRelationship == null)
            {
                targetRelationship = new UserRelationship()
                {
                    PrimaryMemberId = targetFriendId,
                    SecondaryMemberId = sourceFriendId,
                    RelationshipStatus = RelationshipStatus.PendingAck,
                    RelationshipType = RelationshipType.Friend
                };
                _ctx.UserRelationships.Add(targetRelationship);
            }
            if (targetRelationship.RelationshipStatus == RelationshipStatus.Inactive) targetRelationship.RelationshipStatus = RelationshipStatus.Active;

            _auditService.AddAuditRecord(AuditTable.UserRelationship,  AuditRecordType.Created, String.Format("({0}, {1})", sourceFriendId, targetFriendId), "Friend Request Pending");
            _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Created, String.Format("({0}, {1})", targetFriendId, sourceFriendId), "Friend Request Pending Ack");
            return DtoBuilder.GetFriendRelationshipViewModel.Invoke(sourceRelationship);
        }

        public async Task<FriendRelationshipModel> AcceptPendingFriendship(int sourceFriendId, int targetFriendId)
        {
            var existingPendingAckFriendship = await _ctx.UserRelationships.Where(ur =>
                ((ur.PrimaryMemberId == targetFriendId && ur.SecondaryMemberId == sourceFriendId))
                && ur.RelationshipStatus == RelationshipStatus.PendingAck && ur.RelationshipType == RelationshipType.Friend).FirstOrDefaultAsync();
            var existingPendingFriendship = await _ctx.UserRelationships.Where(ur =>
                ((ur.PrimaryMemberId == sourceFriendId && ur.SecondaryMemberId == targetFriendId))
                && ur.RelationshipStatus == RelationshipStatus.Pending && ur.RelationshipType == RelationshipType.Friend).FirstOrDefaultAsync();

            if (existingPendingAckFriendship == null || existingPendingFriendship == null)
            {
                //If one of the relationship records doesn't exist, then we should assume that friendship is invalid and set the records inactive
                if (existingPendingFriendship != null)
                {
                    existingPendingFriendship.RelationshipStatus = RelationshipStatus.Inactive;
                    _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", sourceFriendId, targetFriendId), "Friend Relationship Not Activated");
                }
                if (existingPendingAckFriendship == null) return null;
                existingPendingAckFriendship.RelationshipStatus = RelationshipStatus.Inactive;
                _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", targetFriendId, sourceFriendId), "Friend Relationship Not Activated");
                return null;
            }

            existingPendingAckFriendship.RelationshipStatus = RelationshipStatus.Active;
            existingPendingFriendship.RelationshipStatus = RelationshipStatus.Active;
            _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", sourceFriendId, targetFriendId), "Friend Relationship Active");
            _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", targetFriendId, sourceFriendId), "Friend Relationship Active");

            return DtoBuilder.GetFriendRelationshipViewModel.Invoke(existingPendingAckFriendship);
        }

        public async Task Unfriend(int sourceFriendId, int targetFriendId)
        {
            var sourceRelationship = await _ctx.UserRelationships.Where(ur =>
                ((ur.PrimaryMemberId == targetFriendId && ur.SecondaryMemberId == sourceFriendId))
                 && ur.RelationshipType == RelationshipType.Friend).FirstOrDefaultAsync();
            var targetRelationship = await _ctx.UserRelationships.Where(ur =>
                ((ur.PrimaryMemberId == sourceFriendId && ur.SecondaryMemberId == targetFriendId))
                && ur.RelationshipType == RelationshipType.Friend).FirstOrDefaultAsync();

                //If one of the relationship records doesn't exist, then we should assume that friendship is invalid and set the records inactive
            if (sourceRelationship != null)
                {
                    sourceRelationship.RelationshipStatus = RelationshipStatus.Inactive;
                    _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", sourceFriendId, targetFriendId), "Friend Relationship De-Activated");
                }
            if (targetRelationship != null){
                targetRelationship.RelationshipStatus = RelationshipStatus.Inactive;
                    _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", targetFriendId, sourceFriendId), "Friend Relationship De-Activated");
            }
        }

        public async Task<GuardianRelationshipModel> AddGuardian(int childId, int newGuardianId)
        {

            var sourceRelationship = await _ctx.UserRelationships.Where(ur => (ur.PrimaryMemberId == newGuardianId && ur.SecondaryMemberId == childId)).FirstOrDefaultAsync();
            var targetRelationship = await _ctx.UserRelationships.Where(ur => (ur.SecondaryMemberId == childId && ur.PrimaryMemberId == newGuardianId)).FirstOrDefaultAsync();

            if (sourceRelationship == null) { 
                sourceRelationship = new UserRelationship()
                {
                    PrimaryMemberId = newGuardianId,
                    SecondaryMemberId = childId,
                    RelationshipStatus = RelationshipStatus.Active,
                    RelationshipType = RelationshipType.Guardian
                };
                _ctx.UserRelationships.Add(sourceRelationship);
            }
            if (sourceRelationship.RelationshipStatus == RelationshipStatus.Inactive) sourceRelationship.RelationshipStatus = RelationshipStatus.Active;
            if (targetRelationship == null)
            {
                targetRelationship = new UserRelationship()
                {
                    PrimaryMemberId = childId,
                    SecondaryMemberId = newGuardianId,
                    RelationshipStatus = RelationshipStatus.Active,
                    RelationshipType = RelationshipType.Child
                };
                _ctx.UserRelationships.Add(targetRelationship);
            }
            if (targetRelationship.RelationshipStatus == RelationshipStatus.Inactive) targetRelationship.RelationshipStatus = RelationshipStatus.Active;
            
            _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Created, String.Format("({0}, {1})", newGuardianId, childId), "Guardian Relationship Activated");
            _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Created, String.Format("({0}, {1})", childId, newGuardianId), "Child Relationship Activated");

            return DtoBuilder.GetGuardianRelationshipViewModel.Invoke(sourceRelationship);
        }

        public async Task RemoveGuardian(int childId, int existingGuardianId)
        {
            var sourceRelationship = await _ctx.UserRelationships.Where(ur =>
                ((ur.PrimaryMemberId == existingGuardianId && ur.SecondaryMemberId == childId))
                 && ur.RelationshipType == RelationshipType.Guardian).FirstOrDefaultAsync();
            var targetRelationship = await _ctx.UserRelationships.Where(ur =>
                ((ur.PrimaryMemberId == childId && ur.SecondaryMemberId == existingGuardianId))
                && ur.RelationshipType == RelationshipType.Child).FirstOrDefaultAsync();

            if (sourceRelationship != null)
            {
                sourceRelationship.RelationshipStatus = RelationshipStatus.Inactive;
                _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", existingGuardianId, childId), "Gaurdian Relationship De-Activated");
            }
            if (targetRelationship != null)
            {
                targetRelationship.RelationshipStatus = RelationshipStatus.Inactive;
                _auditService.AddAuditRecord(AuditTable.UserRelationship, AuditRecordType.Modified, String.Format("({0}, {1})", childId, existingGuardianId), "Child Relationship De-Activated");
            }

        }

        public async Task<GuardianRelationshipModel> AddChild(RoleService roleService, ApplicationUserManager userManager, int sourceUserId, RegisterBindingModel newChild)
        {
            var childUser = new ApplicationUser() { UserName = newChild.Email, Email = newChild.Email, FirstName = newChild.FirstName, LastName = newChild.LastName, IsActive = true, IsMinor = true };

            var result = await userManager.CreateAsync(childUser, newChild.Password);

            if (result.Succeeded)
            {
                result = await RoleService.AddRoleToUserAsync(userManager, childUser.Id, Roles.User.ToString());
                if (result.Succeeded)
                {
                    return await AddGuardian(childUser.Id, sourceUserId);
                }
            }
            throw new ApplicationException(String.Join(",", result.Errors.ToArray()));

        }


    }
}