using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.Domain.Models;

namespace Questionnaire.DataAccess.ModelsConfig
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.Key);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        }
    }
    
}
