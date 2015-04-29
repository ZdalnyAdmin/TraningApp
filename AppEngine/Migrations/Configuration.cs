namespace AppEngine.Migrations
{
    using AppEngine.Models.Common;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AppEngine.Models.DataContext.EFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppEngine.Models.DataContext.EFContext context)
        {
            context.Organizations.AddOrUpdate(new Organization() { Name = "My First Organization", CreateDate = DateTime.Now });
        }
    }
}
