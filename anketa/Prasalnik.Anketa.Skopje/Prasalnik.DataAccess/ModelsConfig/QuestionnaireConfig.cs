using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.ModelsConfig
{
    public class QuestionnaireConfig : IEntityTypeConfiguration<Questionnaire>
    {
        public void Configure(EntityTypeBuilder<Questionnaire> builder)
        {
            builder.ToTable("Questionnaires");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Status).IsRequired().HasMaxLength(50);
        }
    }
}
