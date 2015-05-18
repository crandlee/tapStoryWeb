using System.Linq;
using tapStoryWebApi.Common.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.Services
{
    public class UserRelationshipService : IDataService
    {
        public static IQueryable<UserRelationship> GetUserRelationships(ApplicationDbContext ctx)
        {
            return ctx.UserRelationships;
        }

        public static IQueryable<UserRelationship> GetUserRelationshipById(ApplicationDbContext ctx, int id)
        {
            return ctx.UserRelationships.Where(ur => ur.Id == id);
        }

        public static IQueryable<UserRelationship> GetUserRelationshipById(ApplicationDbContext ctx, int primaryMemberId, int secondaryMemberId)
        {
            return ctx.UserRelationships.Where(ur => ur.PrimaryMemberId == primaryMemberId && ur.SecondaryMemberId == secondaryMemberId);
        }

    }
}