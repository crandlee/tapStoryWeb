using System.Security.Principal;
using tapStoryWebApi.Relationships.Security;

namespace tapStoryWebApi.Common.Factories
{
    public static class SecurityFactory
    {
        public static UserRelationshipSecurity GetUserRelationshipSecurity(IPrincipal principal)
        {
            return new UserRelationshipSecurity(principal);
        }

    }
}