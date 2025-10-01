using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Questionnaire.Domain.Models;

namespace Questionnaire.DataAccess.ModelsConfig
{
    public class QuestionFormConfig : IEntityTypeConfiguration<QuestionForm>
    {
        public void Configure(EntityTypeBuilder<QuestionForm> builder)
        {
            builder.ToTable("QuestionForms");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            // foreign key to User
            builder.HasOne(x => x.User)
                .WithMany(x => x.QuestionForms)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction) //koga ke se izbrise user, da ne se izbriseat i questionForms
                .HasConstraintName("FK_QuestionForm_User");
        }
    }
}
