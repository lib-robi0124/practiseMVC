using GlasAnketa.DataAccess.Extensions;
using GlasAnketa.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.DataContext
{
    public class AppDbContext : DbContext //no need Identity
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=DbAnketaDeepSeek;Trusted_Connection=True;TrustServerCertificate=True");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedData();

            // Configure relationships and constraints if needed
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)                //Each Answer belongs to one User.
                .WithMany(u => u.Answers)           //Each User can have many Answers.
                .HasForeignKey(a => a.UserId)       //The UserId in Answer is a foreign key to the Users table.
                .OnDelete(DeleteBehavior.NoAction); //When a User is deleted, EF must decide what to do with that user’s related Answers.

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

            // Additional configurations can be added here Questions, QuestionTypes, QuestionForms, Users
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
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuestionForm> QuestionForms { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}
