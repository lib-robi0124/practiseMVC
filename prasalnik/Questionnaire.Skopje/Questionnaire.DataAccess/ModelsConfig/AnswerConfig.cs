using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.Domain.Models;

namespace Questionnaire.DataAccess.ModelsConfig
{
    public class AnswerConfig : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Response).HasMaxLength(500);
           
            // foreign key to Question
            builder.HasOne(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.NoAction) 
                .HasConstraintName("FK_Answer_Question");
            // foreign key to QuestionForm
            builder.HasOne(x => x.QuestionForm)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionFormId)
                .OnDelete(DeleteBehavior.NoAction) 
                .HasConstraintName("FK_Answer_QuestionForm");
        }

    }
}
