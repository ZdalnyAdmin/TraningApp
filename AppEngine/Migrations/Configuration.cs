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
            context.Profiles.AddOrUpdate(new Profile() { ProfileID = 1, Name = "Administrator", CreateDate = DateTime.Now },
                                         new Profile() { ProfileID = 2, Name = "Manager", CreateDate = DateTime.Now },
                                         new Profile() { ProfileID = 3, Name = "Twórca", CreateDate = DateTime.Now },
                                         new Profile() { ProfileID = 4, Name = "Opiekun", CreateDate = DateTime.Now },
                                         new Profile() { ProfileID = 5, Name = "Użytkownik", CreateDate = DateTime.Now });

            context.Status.AddOrUpdate(new Status() { StatusID = 1, Name = "Aktywny" },
                                       new Status() { StatusID = 2, Name = "Zablokowany" },
                                       new Status() { StatusID = 3, Name = "Usunięty" },
                                       new Status() { StatusID = 4, Name = "Oczekuje" });

            context.Organizations.AddOrUpdate(new Organization() { Name = "My First Organization", CreateDate = DateTime.Now });
        }
    }
}
