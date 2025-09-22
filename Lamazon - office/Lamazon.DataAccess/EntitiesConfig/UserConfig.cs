using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lamazon.DataAccess.EntitiesConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);

            builder.HasIndex(x => x.Email).IsUnique(); //email da bide unikaten
            //foreign key to UserRole
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleKey)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise role, da ne se izbriseat i user-ite
                .HasConstraintName("FK_User_UserRole");

        }
    }
}
