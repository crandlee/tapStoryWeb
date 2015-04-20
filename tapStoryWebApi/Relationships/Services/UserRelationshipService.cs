using System.Linq;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Relationships.Services
{
    public class UserRelationshipService
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