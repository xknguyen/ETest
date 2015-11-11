using ETest.DAL;
using System.Data.Entity.Migrations;
namespace ETest.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ETestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ETest.DAL.ETestDbContext";
        }

        protected override void Seed(ETestDbContext context)
        {
            AccountSeeder.Seed(context);
        }
    }
}