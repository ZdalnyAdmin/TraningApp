using AppEngine.Models.Common;
using AppEngine.Models.DataObject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AppEngine.Migrations;

namespace AppEngine.Models.DataContext
{
    public class EFContext : IdentityDbContext<Person> 
    {
        public static bool firsLog = true;


        public EFContext()
            : base("name=DefaultConnection", throwIfV1Schema: false)
        {
            base.Configuration.ProxyCreationEnabled = false;
            if (!Database.Exists())
            {
                Database.Initialize(true);
            }  

            if(firsLog)
            {
                firsLog = false;
                new Configuration().CallSeed(this);
            }
        }

        public static EFContext Create()
        {
            return new EFContext();
        }

        //public DbSet<Person> Persons { get; set; } // We acctually have context for this DbSet it's named Users.
        public DbSet<ProfileGroup> Groups { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<TrainingResult> TrainingResults { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<ProfileGroup2Person> PeopleInGroups { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<ProfileGroup2Trainings> TrainingInGroups { get; set; }
        public DbSet<AppLog> Logs { get; set; }
        public DbSet<TrainingAnswer> TrainingAnswers { get; set; }
        public DbSet<TrainingDetail> TrainingDetails { get; set; }
        public DbSet<TrainingQuestion> TrainingQuestons { get; set; }
        public DbSet<Trainings2Organizations> TrainingsInOrganizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
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
