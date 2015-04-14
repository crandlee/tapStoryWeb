using System.Data.Entity.Migrations;
using tapStoryWebData.Identity.EF;

namespace tapStoryWebData.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationIdentityDbContext context)
        {
            context.SeedCalled = true;
        }

    }
}
