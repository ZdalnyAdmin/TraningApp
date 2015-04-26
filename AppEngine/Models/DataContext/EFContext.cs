using AppEngine.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web; 

namespace AppEngine.Models.DataContext
{
    public class EFContext : DbContext 
    {
        public EFContext()
            : base("name=DefaultConnection")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileGroup> Groups { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TrainingResult> TrainingResults { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<ProfileGroup2Person> PeopleInGroups { get; set; }

    }
}
