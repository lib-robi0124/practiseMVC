using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.ModelsConfig
{
    public class QuestionItemConfig : IEntityTypeConfiguration<QuestionItem>
    {
        public void Configure(EntityTypeBuilder<QuestionItem> builder)
        {
            builder.ToTable("QuestionItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.QuestionText).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Type).IsRequired();

            builder.HasOne<Questionnaire>()
                .WithMany(q => q.QuestionItems)
                .HasForeignKey("QuestionnaireId")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_QuestionItem_Questionnaire");
        }
    }
}
