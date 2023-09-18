using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class ProfileConfiguration : EntityTypeConfiguration<Profile>
    {
        public ProfileConfiguration()
        {
            ToTable(nameof(Profile));
            HasKey(p => p.Id);

            Property(p => p.Name)
                .HasColumnName(nameof(Profile.Name))
                .IsRequired()
                .HasMaxLength(50);

            Property(u => u.Active)
                .HasColumnName(nameof(Profile.Active))
                .HasColumnType("CHAR")
                .IsRequired()
                .HasMaxLength(1);

            Property(p => p.Id)
                .HasColumnName(nameof(Profile.Id))
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
