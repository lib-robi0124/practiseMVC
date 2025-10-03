using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Anketa.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EmployeeAnketaDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<QuestionForm> QuestionForms { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.QuestionForm)
                .WithMany(f => f.Answers)
                .HasForeignKey(a => a.QuestionFormId);
        }
    }
}
