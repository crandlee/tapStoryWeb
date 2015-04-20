using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebData.Identity.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ApplicationUserRole>()
            //    .HasRequired(ur => ur.Role)
            //    .WithMany()
            //    .HasForeignKey(ur => ur.RoleId);


            modelBuilder.Entity<UserRelationship>().HasRequired(i => i.PrimaryMember).WithMany(u => u.PrimaryRelationships).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserRelationship>().HasRequired(i => i.SecondaryMember).WithMany(u => u.SecondaryRelationships).WillCascadeOnDelete(false);
        }

        public DbSet<UserRelationship> UserRelationships { get; set; }
    }

}
