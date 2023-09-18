using Identity.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Identity.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, ProfilePermission, CustomClaim>
    {
        public ApplicationDbContext()
            : base("DBContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.Id).HasColumnName("Id");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.IdProfile).HasColumnName("IdProfile");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.Cpf).HasColumnName("Cpf");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.Name).HasColumnName("Name");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.Email).HasColumnName("Email");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.PasswordHash).HasColumnName("PasswordHash");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.SecurityStamp).HasColumnName("SecurityStamp");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.Active).HasColumnName("Active");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.EmailConfirmed).HasColumnName("EmailConfirmed");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.LockoutEnabled).HasColumnName("LockoutEnabled");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.AccessFailedCount).HasColumnName("AccessFailedCount");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.UserName).HasColumnName("UserName");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").Property(x => x.CreatedAt).HasColumnName("CreatedAt");
            modelBuilder.Entity<ApplicationUser>().ToTable("USER").HasRequired(u => u.Profile)
                .WithMany(p => p.UserList)
                .HasForeignKey(u => u.IdProfile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomRole>().ToTable("PROFILE").Property(x => x.Id).HasColumnName("Id");
            modelBuilder.Entity<CustomRole>().ToTable("PROFILE").Property(x => x.Name).HasColumnName("Name");

            modelBuilder.Entity<ProfilePermission>().ToTable("ACCESS").Property(x => x.IdPermission).HasColumnName("IdPermission");
            modelBuilder.Entity<ProfilePermission>().ToTable("ACCESS").Property(x => x.RoleId).HasColumnName("IdProfile");
            modelBuilder.Entity<ProfilePermission>().ToTable("ACCESS").Property(x => x.UserId).HasColumnName("IdUser");
            modelBuilder.Entity<ProfilePermission>().ToTable("ACCESS").HasRequired(u => u.Permission)
                .WithMany(p => p.ProfilePermissions)
                .HasForeignKey(u => u.IdPermission)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ProfilePermission>().ToTable("ACCESS").HasRequired(u => u.Profile)
                .WithMany(p => p.ProfilePermissions)
                .HasForeignKey(u => u.RoleId)
                .WillCascadeOnDelete(false);
            //modelBuilder.Entity<CustomPermissionRole>().ToTable("ACCESS").HasKey(r => new { r.UserId, r.RoleId });

            modelBuilder.Entity<CustomClaim>().ToTable("PERMISSION").Property(x => x.Id).HasColumnName("Id");
            modelBuilder.Entity<CustomClaim>().ToTable("PERMISSION").Property(x => x.ClaimType).HasColumnName("ClaimType");
            modelBuilder.Entity<CustomClaim>().ToTable("PERMISSION").Property(x => x.ClaimValue).HasColumnName("ClaimValue");
            modelBuilder.Entity<CustomClaim>().ToTable("PERMISSION").Property(x => x.UserId).HasColumnName("IdUser");

            modelBuilder.Entity<CustomUserLogin>().ToTable("USERLOGIN").Property(x => x.UserId).HasColumnName("IdUser");
            modelBuilder.Entity<CustomUserLogin>().ToTable("USERLOGIN").Property(x => x.LoginProvider).HasColumnName("LoginProvider");
            modelBuilder.Entity<CustomUserLogin>().ToTable("USERLOGIN").Property(x => x.ProviderKey).HasColumnName("ProviderKey");
            modelBuilder.Entity<CustomUserLogin>().ToTable("USERLOGIN").HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
        }
    }
}
