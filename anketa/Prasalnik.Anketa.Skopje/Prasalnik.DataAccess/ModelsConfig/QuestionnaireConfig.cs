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

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            // FK relationship with User
            builder.HasOne<User>()
                   .WithMany(u => u.Questionnaires)
                   .HasForeignKey(q => q.CreatedByUserId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("FK_Questionnaire_User");

            // FK relationship with Status
            builder.HasOne(q => q.Status)
                   .WithMany()
                   .HasForeignKey(q => q.StatusId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("FK_Questionnaire_Status");

            // Seed initial Questionnaires
            builder.HasData(
                new Questionnaire { Id = 1, Title = "Customer Satisfaction Survey", StatusId = 1, CreatedByUserId = 1 },
                new Questionnaire { Id = 2, Title = "Employee Feedback Form", StatusId = 2, CreatedByUserId = 2 }
            );
        }
    }

}
