using Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class ProfilePermissionConfiguration : EntityTypeConfiguration<ProfilePermission>
    {
        public ProfilePermissionConfiguration()
        {
            ToTable(nameof(ProfilePermission));
            HasKey(a => new { a.IdProfile, a.IdPermission});

            Property(a => a.IdProfile)
                .HasColumnName(nameof(ProfilePermission.IdProfile))
                .IsRequired();

            Property(a => a.IdPermission)
                .HasColumnName(nameof(ProfilePermission.IdPermission))
                .IsRequired();

            Property(a => a.IdUser)
                .HasColumnName(nameof(ProfilePermission.IdUser));

            HasRequired(a => a.Profile)
                .WithMany(p => p.ProfilePermissions)
                .HasForeignKey(a => a.IdProfile)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Permission)
                .WithMany(pe => pe.ProfilePermissions)
                .HasForeignKey(a => a.IdPermission)
                .WillCascadeOnDelete(false);
        }

    }
}
