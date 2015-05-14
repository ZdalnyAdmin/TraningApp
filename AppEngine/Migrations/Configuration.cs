namespace AppEngine.Migrations
{
    using AppEngine.Models.Common;
    using AppEngine.Models.DataObject;
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
            //context.Organizations.AddOrUpdate(new Organization() { Name = "My First Organization", CreateDate = DateTime.Now });

            var user = context.Users.FirstOrDefault(x => x.Id == "9154c72e-a195-4a70-8de2-61927df9bd9d");
            if (user == null)
            {
                context.Users.AddOrUpdate(new Person()
                {
                    Id = "9154c72e-a195-4a70-8de2-61927df9bd9d",
                    Email = "email@mail.pl",
                    PasswordHash = "AGE09SnBYFYs5KB409zx1/T5mIf6Z1gef2QD6mMyr8DIYtpyMtT9vMYWP/Mpyj7JgQ==", // Admin1!
                    SecurityStamp = "fd2e32d1-5280-48d4-8963-57f48b35dbd7",
                    TwoFactorEnabled = true,
                    UserName = "admin",
                    DisplayName = "admin",
                    Profile = Models.DataBusiness.ProfileEnum.Superuser,
                    Status = Models.DataBusiness.StatusEnum.Active,
                    RegistrationDate = DateTime.Now,
                    InvitationDate = DateTime.Now//,
                    //InviterID = "9154c72e-a195-4a70-8de2-61927df9bd9d"//,
                    //OrganizationID = 1
                });
                context.SaveChanges();
            }

            context.Groups.AddOrUpdate(new ProfileGroup()
            {
                Name = "Wszyscy",
                IsDeleted = false,
                CreateUserID = "9154c72e-a195-4a70-8de2-61927df9bd9d",
                CreateDate = DateTime.Now
            });

            if (!context.AppSettings.Any(x => x.IsDefault))
            {
                context.AppSettings.AddOrUpdate(new AppSetting()
                {
                    AllowUserToChangeName = true,
                    AllowUserToChangeMail = true,
                    SpaceDisk = 50,
                    MaxAssignedUser = 10,
                    IsGlobalAvailable = true,
                    IsTrainingAvailableForAll = true,
                    MaxActiveTrainings = 5,
                    DefaultEmail = "support@kenpro.pl",
                    DefaultName = "Kenpro",
                    IsDefault = true
                });
            }

            context.SaveChanges();
        }

        internal void CallSeed(Models.DataContext.EFContext eFContext)
        {
            Seed(eFContext);
        }
    }
}
