using Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class ProfilePermissionConfiguration : EntityTypeConfiguration<ProfilePermission>
    {
        public ProfilePermissionConfiguration()
        {
            ToTable(nameof(ProfilePermission));
            HasKey(a => new { a.ProfileId, a.PermissionId});

            Property(a => a.ProfileId)
                .HasColumnName(nameof(ProfilePermission.ProfileId))
                .IsRequired();

            Property(a => a.PermissionId)
                .HasColumnName(nameof(ProfilePermission.PermissionId))
                .IsRequired();

            Property(a => a.IdUser)
                .HasColumnName(nameof(ProfilePermission.IdUser));

            HasRequired(a => a.Profile)
                .WithMany(p => p.ProfilePermissions)
                .HasForeignKey(a => a.ProfileId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Permission)
                .WithMany(pe => pe.ProfilePermissions)
                .HasForeignKey(a => a.PermissionId)
                .WillCascadeOnDelete(false);
        }

    }
}
