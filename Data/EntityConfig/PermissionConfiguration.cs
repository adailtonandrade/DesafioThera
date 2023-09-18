using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class PermissionConfiguration : EntityTypeConfiguration<Permission>
    {
        //usar padrão de nomenclatura, manter palavras minúsculas
        public PermissionConfiguration()
        {
            ToTable(nameof(Permission));
            HasKey(p => p.Id);

            Property(p => p.ClaimType)
                .HasColumnName("ClaimType")
                .HasMaxLength(100);

            Property(p => p.ClaimValue)
                .HasColumnName("ClaimValue")
                .HasMaxLength(100);

            Property(p => p.IdUser)
                .HasColumnName("IdUser");

            Property(p => p.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
