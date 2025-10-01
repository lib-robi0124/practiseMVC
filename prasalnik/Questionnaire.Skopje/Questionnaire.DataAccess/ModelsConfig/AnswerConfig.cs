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
            builder.Property(x => x.QuestionName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Response).IsRequired().HasMaxLength(1000);
           
            // foreign key to Question
            builder.HasOne(x => x.Question)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise question, da ne se izbriseat i answers
                .HasConstraintName("FK_Answer_Question");
            // foreign key to QuestionForm
            builder.HasOne(x => x.Questionnaire)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionnaireId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise questionForm, da ne se izbriseat i answers
                .HasConstraintName("FK_Answer_QuestionForm");
        }

    }
}
