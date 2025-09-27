using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prasalnik.Domain.Models;
using Prasalnik.Domain.Enums;

namespace Prasalnik.DataAccess.ModelsConfig
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
            builder.Property(x => x.Role).IsRequired();

            builder.HasIndex(x => x.CompanyId).IsUnique(); // Assuming CompanyId is unique for each user

            builder.HasData(
            new User { Id = 1, CompanyId = 12345, FullName = "Sara Sara", OU = "HR", Role = RoleEnum.Admin },
            new User { Id = 2, CompanyId = 24680, FullName = "Robi Robi", OU = "IT", Role = RoleEnum.Manager },
            new User { Id = 3, CompanyId = 13579, FullName = "Ada Ada", OU = "Finance", Role = RoleEnum.Employee }
);        }
    }
}
