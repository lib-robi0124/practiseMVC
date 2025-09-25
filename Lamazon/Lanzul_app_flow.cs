using Prasalnik.Domain.Models;

namespace Prasalnik.Domain.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
    }
}
namespace Prasalnik.Domain.Models
{
    public class User : BaseEntity
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; } // Organizational Unit
        public RoleEnum Role { get; set; }
    }
}
namespace Prasalnik.Domain.Models
{
    public class Questionnaire : BaseEntity
    {
        public string Title { get; set; }
        public List<QuestionItem> QuestionItems { get; set; } = new();
        public string Status { get; set; } // Draft, Published, Closed
    }
}
namespace Prasalnik.Domain.Models
{
    public class QuestionItem : BaseEntity
    {
        public string QuestionText { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public int QuestionnaireId { get; set; } 
    }
}
namespace Prasalnik.Domain.Models
{
    public class Answer : BaseEntity
    {
        public int QuestionnaireId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Response { get; set; }
    }
}
namespace Prasalnik.Domain.Models
{
    public class Status : BaseEntity
    {
        public string Name { get; set; }
    }
}
namespace Prasalnik.Domain.Enums
{
    public enum RoleEnum
    {
        Admin = 1,
        Manager,
        Employee
    }
}
namespace Prasalnik.Domain.Enums
{
    public enum QuestionTypeEnum
    {
        Text = 1,   // 255 char
        Radio,      // Yes/No
        Scale       // 1-10
    }
}
namespace Prasalnik.DataAccess.ModelsConfig
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.OU).HasMaxLength(128);
            builder.Property(x => x.Role).IsRequired();

            builder.HasData(
                 new User { Id = 1, CompanyId = 12345, FullName = "Alice Johnson", OU = "HR", Role = (RoleEnum)1 },
                new User { Id = 2, CompanyId = 12345, FullName = "Bob Smith", OU = "IT", Role = (RoleEnum)2 },
                new User { Id = 3, CompanyId = 12345, FullName = "Charlie Brown", OU = "Finance", Role = (RoleEnum)3 });
        }
    }
}
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

            builder.HasData(
                new Questionnaire { Id = 1, Title = "Customer Satisfaction Survey", Status = "Answered" },
                new Questionnaire { Id = 2, Title = "Employee Feedback Form", Status = "Skipped" });
        }
    }
}
namespace Prasalnik.DataAccess.ModelsConfig
{
    public class QuestionItemConfig : IEntityTypeConfiguration<QuestionItem>
    {
        public void Configure(EntityTypeBuilder<QuestionItem> builder)
        {
            builder.ToTable("QuestionItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.QuestionText).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Type).IsRequired();

            builder.HasOne<Questionnaire>()
                .WithMany(q => q.QuestionItems)
                .HasForeignKey("QuestionnaireId")
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_QuestionItem_Questionnaire");
        
            builder.HasData(
                new QuestionItem { Id = 1, QuestionText = "How satisfied are you with our service?", Type = QuestionTypeEnum.Radio, QuestionnaireId = 1 },
                new QuestionItem { Id = 2, QuestionText = "Would you recommend us to others?", Type = QuestionTypeEnum.Scale, QuestionnaireId = 1 },
                new QuestionItem { Id = 3, QuestionText = "What can we improve?", Type = QuestionTypeEnum.Text, QuestionnaireId = 2 },
                new QuestionItem { Id = 4, QuestionText = "How do you rate the work environment?", Type = QuestionTypeEnum.Scale, QuestionnaireId = 2 } );
        }
    }
}
namespace Prasalnik.DataAccess.ModelsConfig
{
    public class StatusConfigure : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            // Seed initial data
            builder.HasData(
                new Status { Id = 1, Name = "Answered" },
                new Status { Id = 2, Name = "Skipped" }
            );
        }
    }
}
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
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Answer_Questionnaire");

            builder.HasOne<QuestionItem>()
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Answer_QuestionItem");
        }
    }
}
namespace Prasalnik.DataAccess.Interaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);

    }
}
namespace Prasalnik.DataAccess.Interaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByCompanyId(int companyId);
        User LoginUser(int companyId, string fullName);

    }
}
namespace Prasalnik.DataAccess.Interaces
{
    public interface IQuestionnaireRepository : IRepository<Questionnaire>
    {
        Questionnaire GetbyUserId(int userId);
    }
}
namespace Prasalnik.DataAccess.Interaces
{
    public interface IQuestionItemRepository : IRepository<QuestionItem>
    {
        QuestionItem GetByType(Type type);
    }
}
namespace Prasalnik.DataAccess.Interaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Answer GetByUserId(int userId);
    }
}
namespace Prasalnik.DataAccess.Interaces
{
    public interface IStatusRepository : IRepository<Status>
    {
        Status GetByName(string name);
    }
}
namespace Prasalnik.DataAccess.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionItem> QuestionItems { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new QuestionnaireConfig());
            modelBuilder.ApplyConfiguration(new QuestionItemConfig());
            modelBuilder.ApplyConfiguration(new AnswerConfig());
            modelBuilder.ApplyConfiguration(new StatusConfigure());

            base.OnModelCreating(modelBuilder);
        }
    }
}
namespace Prasalnik.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        public AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Create(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id) ?? throw new Exception("User not found");
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id) ?? throw new Exception("User not found");
        }

        public User GetUserByCompanyId(int companyId)
        {
            return _context.Users.FirstOrDefault(u => u.CompanyId == companyId) ?? throw new Exception("User not found");
        }

        public User LoginUser(int companyId, string fullName)
        {
            return _context.Users.FirstOrDefault(u => u.CompanyId == companyId && u.FullName == fullName) ?? throw new Exception("User not found");
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
namespace Prasalnik.DataAccess.Implementations
{
    public class QuestionnaireRepository : IQuestionnaireRepository
{
    private readonly AppDbContext _context;

    public QuestionnaireRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Questionnaire entity)
    {
        _context.Questionnaires.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var q = _context.Questionnaires.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Questionnaire not found");
        _context.Questionnaires.Remove(q);
        _context.SaveChanges();
    }

    public IEnumerable<Questionnaire> GetAll()
    {
        return _context.Questionnaires.Include(q => q.QuestionItems).ToList();
    }

    public Questionnaire GetById(int id)
    {
        return _context.Questionnaires.Include(q => q.QuestionItems).FirstOrDefault(x => x.Id == id)
               ?? throw new Exception("Questionnaire not found");
    }

    public Questionnaire GetbyUserId(int userId)
    {
        return _context.Answers
            .Where(a => a.UserId == userId)
            .Select(a => a.QuestionnaireId)
            .Distinct()
            .Select(id => _context.Questionnaires.Include(q => q.QuestionItems).FirstOrDefault(q => q.Id == id))
            .FirstOrDefault() ?? throw new Exception("No questionnaire found for this user");
    }

    public void Update(Questionnaire entity)
    {
        _context.Questionnaires.Update(entity);
        _context.SaveChanges();
    }
}

}
namespace Prasalnik.DataAccess.Implementations
{
    public class QuestionItemRepository : IQuestionItemRepository
{
    private readonly AppDbContext _context;

    public QuestionItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(QuestionItem entity)
    {
        _context.QuestionItems.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = _context.QuestionItems.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Question item not found");
        _context.QuestionItems.Remove(item);
        _context.SaveChanges();
    }

    public IEnumerable<QuestionItem> GetAll()
    {
        return _context.QuestionItems.ToList();
    }

    public QuestionItem GetById(int id)
    {
        return _context.QuestionItems.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Question item not found");
    }

    public QuestionItem GetByType(Type type)
    {
        throw new NotSupportedException("Use QuestionTypeEnum instead of System.Type");
    }

    public void Update(QuestionItem entity)
    {
        _context.QuestionItems.Update(entity);
        _context.SaveChanges();
    }
}

}
// this Answers will be fill up by logged user
namespace Prasalnik.DataAccess.Implementations
{
    public class AnswerRepository : IAnswerRepository
{
    private readonly AppDbContext _context;

    public AnswerRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Answer entity)
    {
        _context.Answers.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var ans = _context.Answers.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Answer not found");
        _context.Answers.Remove(ans);
        _context.SaveChanges();
    }

    public IEnumerable<Answer> GetAll()
    {
        return _context.Answers.ToList();
    }

    public Answer GetById(int id)
    {
        return _context.Answers.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Answer not found");
    }

    public Answer GetByUserId(int userId)
    {
        return _context.Answers.FirstOrDefault(x => x.UserId == userId) ?? throw new Exception("No answers found for user");
    }

    public void Update(Answer entity)
    {
        _context.Answers.Update(entity);
        _context.SaveChanges();
    }
}

}
// this Status will depends from logged user input
namespace Prasalnik.DataAccess.Implementations
{
public class StatusRepository : IStatusRepository
{
    private readonly AppDbContext _context;

    public StatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Status entity)
    {
        _context.Statuses.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var s = _context.Statuses.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Status not found");
        _context.Statuses.Remove(s);
        _context.SaveChanges();
    }

    public IEnumerable<Status> GetAll()
    {
        return _context.Statuses.ToList();
    }

    public Status GetById(int id)
    {
        return _context.Statuses.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Status not found");
    }

    public Status GetByName(string name)
    {
        return _context.Statuses.FirstOrDefault(x => x.Name == name) ?? throw new Exception("Status not found");
    }

    public void Update(Status entity)
    {
        _context.Statuses.Update(entity);
        _context.SaveChanges();
    }
}

}
// appsettings.json 
"DefaultConnection": "Server=.;Database=LibertyHrDb;Trusted_Connection=True;TrustServerCertificate=True"
// Program.cs 
var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();