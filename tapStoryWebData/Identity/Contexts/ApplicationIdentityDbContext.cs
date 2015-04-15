using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebData.Identity.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationIdentityDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationIdentityDbContext Create()
        {
            return new ApplicationIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUserRole>()
                .HasRequired(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);
        }

        public bool SeedCalled { get; set; }
    }

}
