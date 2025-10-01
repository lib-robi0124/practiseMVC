using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.Domain.Models;

namespace Questionnaire.DataAccess.ModelsConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.OU).HasMaxLength(128);

            builder.HasIndex(x => x.CompanyId).IsUnique();
            //foreign key to UserRole
            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleKey)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise role, da ne se izbriseat i user-ite
                .HasConstraintName("FK_User_UserRole");
        }
    }
}
