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

            //foreign key relationship with User
            builder.HasOne<User>()
                   .WithMany(u => u.Questionnaires)
                   .HasForeignKey(q => q.CreatedByUserId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("FK_Questionnaire_User");

            builder.HasData(
                new Questionnaire { Id = 1, Title = "Customer Satisfaction Survey", Status = "Answered" },
                new Questionnaire { Id = 2, Title = "Employee Feedback Form", Status = "Skipped" });
        }
    }
}
