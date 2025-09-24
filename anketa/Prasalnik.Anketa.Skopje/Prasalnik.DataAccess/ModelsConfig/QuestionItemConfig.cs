using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prasalnik.Domain.Enums;
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

            
            builder.HasData(
                     new QuestionItem { Id = 1, QuestionText = "How satisfied are you with our service?", Type = QuestionTypeEnum.Scale, QuestionnaireId = 1 },
                     new QuestionItem { Id = 2, QuestionText = "Would you recommend us to others?", Type = QuestionTypeEnum.Radio, QuestionnaireId = 1 },
                     new QuestionItem { Id = 3, QuestionText = "What can we improve?", Type = QuestionTypeEnum.Text, QuestionnaireId = 2 },
                     new QuestionItem { Id = 4, QuestionText = "How do you rate the work environment?", Type = QuestionTypeEnum.Scale, QuestionnaireId = 2 }
                 );
        }
    }
}
