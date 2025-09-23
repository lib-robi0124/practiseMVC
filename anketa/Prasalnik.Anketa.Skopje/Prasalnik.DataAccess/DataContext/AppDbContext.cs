using Microsoft.EntityFrameworkCore;
using Prasalnik.DataAccess.ModelsConfig;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionItem> QuestionItems { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new QuestionnaireConfig());
            modelBuilder.ApplyConfiguration(new QuestionItemConfig());
            modelBuilder.ApplyConfiguration(new AnswerConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
