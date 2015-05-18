using System.Data.Entity.Migrations;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebData.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            
        }

    }
}
