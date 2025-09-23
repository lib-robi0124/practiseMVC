using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prasalnik.Domain.Models;

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
        }
    }
}
