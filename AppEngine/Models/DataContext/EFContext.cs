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
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AppEngine.Models.DataContext
{
    public class EFContext : IdentityDbContext<Person> 
    {
        public EFContext()
            : base("name=DefaultConnection", throwIfV1Schema: false)
        {
            base.Configuration.ProxyCreationEnabled = false;
            if (!Database.Exists())
            {
                Database.Initialize(true);
                //new Configuration().CallSeed(this);
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
        public DbSet<ProfileGroup2Organization> GroupsInOrganizations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Remove the tight dependency on the entity framework that 
            //wants to take control of the database. EF by nature wants
            //to drive the database so that the database changes conform
            //to the model changes in the application. This will remove the
            //control from the EF and leave the changes to the database admin
            //side so that it continues to be in sync with the model.
            Database.SetInitializer<EFContext>(null);

            //Remove the default pluaralization of model names
            //This will allow us to work with database table names that are singular
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 

            modelBuilder.Entity<IdentityUser>().ToTable("Person", "dbo");
            modelBuilder.Entity<Person>().ToTable("Person", "dbo");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");
        }

    }
}
