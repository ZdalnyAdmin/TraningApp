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
            //context.Organizations.AddOrUpdate(new Organization() { Name = "My First Organization", CreateDate = DateTime.Now });
            context.Users.AddOrUpdate(new Person() { Id = "9154c72e-a195-4a70-8de2-61927df9bd9d",
                                                     Email = "email@mail.pl",
                                                     PasswordHash = "AGE09SnBYFYs5KB409zx1/T5mIf6Z1gef2QD6mMyr8DIYtpyMtT9vMYWP/Mpyj7JgQ==", // Admin1!
                                                     SecurityStamp =  "fd2e32d1-5280-48d4-8963-57f48b35dbd7",
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
        }
    }
}
