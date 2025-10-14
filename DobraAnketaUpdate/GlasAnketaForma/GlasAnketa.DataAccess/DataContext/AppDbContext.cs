using GlasAnketa.DataAccess.Extensions;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {  }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuestionForm> QuestionForms { get; set; }
        public DbSet<Answer> Answers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedData();

           modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.QuestionForm)
                .WithMany(qf => qf.Answers)
                .HasForeignKey(a => a.QuestionFormId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Answer>()
                        .Property(a => a.CompanyId)
                        .IsRequired();

            // Allow multiple answers over time for the same user/question/form
            modelBuilder.Entity<Answer>()
                .HasIndex(a => new { a.UserId, a.QuestionId, a.QuestionFormId });

           modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionForm)
                .WithMany(f => f.Questions)
                .HasForeignKey(q => q.QuestionFormId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionType)
                .WithMany(qt => qt.Questions)
                .HasForeignKey(q => q.QuestionTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
      
    }
}
