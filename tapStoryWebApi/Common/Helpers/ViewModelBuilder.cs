using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using tapStoryWebApi.Accounts.ViewModels;
using tapStoryWebApi.Relationships.ViewModels;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Common.Helpers
{
    public static class ViewModelBuilder
    {
        public static Expression<Func<UserRelationship, ChildRelationshipViewModel>> GetChildRelationshipViewModel = ur => new ChildRelationshipViewModel()
        {
            Id = ur.Id,
            ParentId = ur.PrimaryMemberId,
            ChildId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            Parent = new[] { ur.PrimaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault(),
            Child = new[] { ur.SecondaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault()
        };

        public static Expression<Func<UserRelationship, FriendRelationshipViewModel>> GetFriendRelationshipViewModel = ur => new FriendRelationshipViewModel()
        {
            Id = ur.Id,
            SourceFriendId = ur.PrimaryMemberId,
            TargetFriendId = ur.SecondaryMemberId,
            RelationshipStatus = ur.RelationshipStatus,
            SourceFriend = new [] { ur.PrimaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault(),
            TargetFriend = new [] { ur.SecondaryMember }.AsQueryable().Select(GetApplicationUserViewModel).FirstOrDefault()
        };

        public static Expression<Func<UserRelationship, GuardianRelationshipViewModel>> GetGuardianRelationshipViewModel = ur => new GuardianRelationshipViewModel()
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