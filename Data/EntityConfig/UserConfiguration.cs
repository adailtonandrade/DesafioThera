using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable(nameof(User));
            HasKey(u => u.Id);

            Property(u => u.ProfileId)
                .HasColumnName(nameof(User.ProfileId))
                .IsRequired();

            Property(u => u.Cpf)
                .HasColumnName(nameof(User.Cpf))
                .HasMaxLength(11)
                .IsRequired();

            Property(u => u.Name)
                .HasColumnName(nameof(User.Name))
                .HasMaxLength(100)
                .IsRequired();

            Property(u => u.Email)
                .HasColumnName(nameof(User.Email))
                .HasMaxLength(50)
                .IsRequired();

            Property(u => u.Active)
                .HasColumnName(nameof(User.Active))
                .HasColumnType("CHAR")
                .IsRequired()
                .HasMaxLength(1);

            Property(u => u.Username)
                .HasColumnName(nameof(User.Username))
                .HasMaxLength(50)
                .IsRequired();

            Property(u => u.NickName)
                .HasColumnName(nameof(User.NickName))
                .HasMaxLength(15)
                .IsRequired();

            Property(u => u.EmailConfirmed)
                .HasColumnName(nameof(User.EmailConfirmed))
                .IsRequired();

            Property(u => u.PasswordHash)
                .HasColumnName(nameof(User.PasswordHash))
                .IsRequired();

            Property(u => u.SecurityStamp)
                .HasColumnName(nameof(User.SecurityStamp));

            Property(u => u.PhoneNumber)
                .HasColumnName(nameof(User.PhoneNumber))
                .HasMaxLength(50);

            Property(u => u.PhoneNumberConfirmed)
                .HasColumnName(nameof(User.PhoneNumberConfirmed))
                .IsRequired();

            Property(u => u.TwoFactorEnabled)
                .HasColumnName(nameof(User.TwoFactorEnabled))
                .IsRequired();

            Property(u => u.LockoutEndDateUTC)
                .HasColumnName(nameof(User.LockoutEndDateUTC));

            Property(u => u.LockoutEnabled)
                .HasColumnName(nameof(User.LockoutEnabled))
                .IsRequired();

            Property(u => u.AccessFailedCount)
                .HasColumnName(nameof(User.AccessFailedCount))
                .IsRequired()
                .HasColumnType("int");

            Property(u => u.CreatedAt)
                .HasColumnName(nameof(User.CreatedAt));

            HasRequired(u => u.Profile)
                .WithMany(p => p.UserList)
                .HasForeignKey(u => u.ProfileId)
                .WillCascadeOnDelete(false);

            Property(u => u.Id)
                .HasColumnName(nameof(User.Id))
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
