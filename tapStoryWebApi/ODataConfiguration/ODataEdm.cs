using System.Web.OData.Builder;
using Microsoft.OData.Edm;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.ODataConfiguration
{
    public class ODataEdm
    {
        public static IEdmModel GetModel()
        {

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder = AddApplicationUserConfiguration(builder);
            builder = AddApplicationRoleConfiguration(builder);
            builder = AddUserRelationshipConfiguration(builder);
            return builder.GetEdmModel();

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

            return builder;
        }

        private static ODataModelBuilder AddApplicationRoleConfiguration(ODataModelBuilder builder)
        {
            builder.EntitySet<ApplicationRole>("Roles");
            var ur = builder.EntitySet<ApplicationUserRole>("UserRoles");
            ur.EntityType.HasKey(urole => new { urole.RoleId, urole.UserId });
            return builder;
        }

        private static ODataModelBuilder AddUserRelationshipConfiguration(ODataModelBuilder builder)
        {
            builder.EntitySet<UserRelationship>("UserRelationships");
            var urGetFunc = builder.Function("RetrieveUserRelationships");                
            urGetFunc.Parameter<int>("PrimaryMemberId");
            urGetFunc.Parameter<int>("SecondaryMemberId");
            urGetFunc.ReturnsCollectionFromEntitySet<UserRelationship>("UserRelationships");
            return builder;
        }

    }
}