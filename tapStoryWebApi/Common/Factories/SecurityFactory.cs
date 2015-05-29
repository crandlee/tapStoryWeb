using System.Security.Principal;
using tapStoryWebApi.Relationships.Security;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Common.Factories
{
    public static class SecurityFactory
    {
        public static UserRelationshipSecurity GetUserRelationshipSecurity(ApplicationDbContext ctx, IPrincipal principal)
        {
            return new UserRelationshipSecurity(ctx, principal);
        }

    }
}