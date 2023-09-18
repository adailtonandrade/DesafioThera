using Data.EntityConfig;
using Domain.Entities;
using System.Data.Entity;

namespace Data.Context
{
    public class ModelContext : DbContext
    {
        public ModelContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public ModelContext() : base("DBContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<ProfilePermission> ProfilePermission { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<Talent> Talent { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ProfileConfiguration());
            modelBuilder.Configurations.Add(new ProfilePermissionConfiguration());
            modelBuilder.Configurations.Add(new PermissionConfiguration());
            modelBuilder.Configurations.Add(new UserLoginConfiguration());
            modelBuilder.Configurations.Add(new TalentConfiguration());
        }
    }
}
