using Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class UserLoginConfiguration : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginConfiguration()
        {
            ToTable(nameof(UserLogin));
            HasKey(ul => new { ul.IdUser, ul.LoginProvider, ul.ProviderKey });

            Property(u => u.IdUser)
                .HasColumnName(nameof(UserLogin.IdUser))
                .IsRequired();

            Property(ul => ul.LoginProvider)
                .HasColumnName(nameof(UserLogin.LoginProvider))
                .HasMaxLength(128);

            Property(ul => ul.ProviderKey)
                .HasColumnName(nameof(UserLogin.ProviderKey))
                .HasMaxLength(128);

            HasRequired(ul => ul.User)
                .WithMany(u => u.UserLoginList)
                .HasForeignKey(ul => ul.IdUser)
                .WillCascadeOnDelete(false);
        }
    }
}
