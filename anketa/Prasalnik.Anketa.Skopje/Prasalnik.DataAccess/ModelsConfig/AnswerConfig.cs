using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.ModelsConfig
{
    public class AnswerConfig : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Response).HasMaxLength(255);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Answer_User");

            builder.HasOne<Questionnaire>()
                .WithMany()
                .HasForeignKey(x => x.QuestionnaireId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Answer_Questionnaire");

            builder.HasOne<QuestionItem>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Answer_QuestionItem");
        }
    }
}
