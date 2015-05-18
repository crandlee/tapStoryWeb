using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using tapStoryWebData.EF.Models;

namespace tapStoryWebData.EF.Contexts
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
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new {r.UserId, r.RoleId});
            modelBuilder.Entity<ApplicationUserRole>().Ignore(r => r.UserName);
            modelBuilder.Entity<ApplicationUserRole>().Ignore(r => r.RoleName);
            modelBuilder.Entity<UserRelationship>().HasRequired(i => i.PrimaryMember).WithMany(u => u.PrimaryRelationships).WillCascadeOnDelete(false);
            modelBuilder.Entity<UserRelationship>().HasRequired(i => i.SecondaryMember).WithMany(u => u.SecondaryRelationships).WillCascadeOnDelete(false);
        }

        public DbSet<UserRelationship> UserRelationships { get; set; }
        public DbSet<FileGroup> FileGroups { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<UserFileGroup> UserFileGroups { get; set; }
        public DbSet<Audit> AuditRecords { get; set; }
    }

}
