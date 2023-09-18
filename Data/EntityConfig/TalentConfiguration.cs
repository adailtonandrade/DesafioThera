using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class TalentConfiguration : EntityTypeConfiguration<Talent>
    {
        public TalentConfiguration()
        {
            ToTable(nameof(Talent));
            HasKey(p => p.Id);

            Property(a => a.FullName)
                .HasColumnName(nameof(Talent.FullName))
                .HasMaxLength(50)
                .IsRequired();

            Property(a => a.Email)
                .HasColumnName(nameof(Talent.Email))
                .HasMaxLength(50)
                .IsRequired();

            Property(a => a.ResumeFileName)
                .HasColumnName(nameof(Talent.ResumeFileName))
                .HasMaxLength(50)
                .IsRequired();

            Property(a => a.ResumeUniqueName)
                .HasColumnName(nameof(Talent.ResumeUniqueName))
                .HasMaxLength(200)
                .IsRequired();

            Property(a => a.Active)
                .HasColumnName(nameof(Talent.Active))
                .HasColumnType("CHAR")
                .IsRequired()
                .HasMaxLength(1);

            Property(a => a.CreatedAt)
                .HasColumnName(nameof(Talent.CreatedAt));

            Property(a => a.UpdatedAt)
                .HasColumnName(nameof(Talent.UpdatedAt));

            HasOptional(a => a.UserWhoUpdated)
                .WithMany(u => u.Applicants)
                .HasForeignKey(u => u.UpdatedBy)
                .WillCascadeOnDelete(false);

            Property(p => p.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
