using System;
using System.Linq;
using System.Linq.Expressions;
using tapStoryWebApi.Accounts.DTO;
using tapStoryWebApi.Relationships.DTO;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Common.Helpers
{
    public static class DtoBuilder
    {
        public static Expression<Func<UserRelationship, ChildRelationshipModel>> GetChildRelationshipViewModel = ur => new ChildRelationshipModel()
        {
            Id = ur.Id,
            ParentId = ur.PrimaryMemberId,
            ChildId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            Parent = new[] { ur.PrimaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault(),
            Child = new[] { ur.SecondaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault()
        };

        public static Expression<Func<UserRelationship, FriendRelationshipModel>> GetFriendRelationshipViewModel = ur => new FriendRelationshipModel()
        {
            Id = ur.Id,
            SourceFriendId = ur.PrimaryMemberId,
            TargetFriendId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            SourceFriend = new [] { ur.PrimaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault(),
            TargetFriend = new [] { ur.SecondaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault()
        };

        public static Expression<Func<UserRelationship, GuardianRelationshipModel>> GetGuardianRelationshipViewModel = ur => new GuardianRelationshipModel()
        {
            Id = ur.Id,
            ParentId = ur.PrimaryMemberId,
            ChildId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            Parent = new[] { ur.PrimaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault(),
            Child = new[] { ur.SecondaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault()
        };

        public static Expression<Func<ApplicationUser, ApplicationUserViewModel>> GetApplicationUserViewModel = u => new ApplicationUserViewModel()
        {
            EMail = u.Email,
            FirstName = u.FirstName,
            Id = u.Id,
            LastName = u.LastName,
            UserName = u.UserName,
        }; 

    }
}