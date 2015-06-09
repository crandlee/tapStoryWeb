using System.Web.OData.Builder;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.OData.Edm;
using tapStoryWebApi.Files.DTO;
using tapStoryWebApi.Relationships.DTO;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.ODataConfiguration
{
    public class ODataEdm
    {
        public static IEdmModel GetModel()
        {

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder = AddApplicationUserConfiguration(builder);
            builder = AddUserRelationshipConfiguration(builder);
            builder = AddFileConfiguration(builder);
            return builder.GetEdmModel();

        }

        private static ODataModelBuilder AddFileConfiguration(ODataModelBuilder builder)
        {
            builder.EntitySet<FileGroupModel>("OdFileGroups");
            builder.EntitySet<UserStoryModel>("OdUserStories");
            builder.EntitySet<BookModel>("OdBooks");
            builder.EntitySet<FileModel>("OdFiles");
            return builder;
        }

        private static ODataModelBuilder AddApplicationUserConfiguration(ODataModelBuilder builder)
        {

            var ent = builder.EntitySet<ApplicationUser>("Users");
            ent.EntityType.Ignore(e => e.EmailConfirmed);
            ent.EntityType.Ignore(e => e.PasswordHash);
            ent.EntityType.Ignore(e => e.PhoneNumber);
            ent.EntityType.Ignore(e => e.PhoneNumberConfirmed);
            ent.EntityType.Ignore(e => e.SecurityStamp);
            ent.EntityType.Ignore(e => e.TwoFactorEnabled);
            ent.EntityType.Ignore(e => e.LockoutEndDateUtc);
            ent.EntityType.Ignore(e => e.LockoutEnabled);
            ent.EntityType.Ignore(e => e.AccessFailedCount);
            ent.EntityType.Ignore(e => e.Logins);
            builder.EntitySet<ApplicationUserClaim>("Claims");
            builder.EntityType<IdentityUserRole<int>>().HasKey(urole => new { urole.RoleId, urole.UserId });
            builder.EntitySet<ApplicationUserRole>("Roles");
            return builder;
        }

        private static ODataModelBuilder AddUserRelationshipConfiguration(ODataModelBuilder builder)
        {
            builder.EntitySet<UserRelationship>("UserRelationships");
            //var urGetFunc = builder.Function("RetrieveUserRelationships");                
            //urGetFunc.Parameter<int>("PrimaryMemberId");
            //urGetFunc.Parameter<int>("SecondaryMemberId");
            //urGetFunc.ReturnsCollectionFromEntitySet<UserRelationship>("UserRelationships");
            builder.EntitySet<ChildRelationshipModel>("OdChildRelationships");
            builder.EntitySet<GuardianRelationshipModel>("OdGuardianships");
            builder.EntitySet<FriendRelationshipModel>("OdFriendships");
            return builder;
        }

    }
}