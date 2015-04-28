using AppEngine.Models.Common;
using AppEngine.Models.DataObject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AppEngine.Models.DataContext
{
    public class EFContext : IdentityDbContext<Person> 
    {
        public EFContext()
            : base("name=DefaultConnection", throwIfV1Schema: false)
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public static EFContext Create()
        {
            return new EFContext();
        }

        //public DbSet<Person> Persons { get; set; } // We acctually have context for this DbSet it's named Users.
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileGroup> Groups { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TrainingResult> TrainingResults { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<ProfileGroup2Person> PeopleInGroups { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<ProfileGroup2Trainings> TrainingInGroups { get; set; }
        public DbSet<AppLog> Logs { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<IdentityUser>().ToTable("Person", "dbo");
            modelBuilder.Entity<Person>().ToTable("Person", "dbo");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");

        }

    }
}
