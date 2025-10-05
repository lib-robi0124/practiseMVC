Company needs MVC app as Questionnaire for employee satisfactory on different area of workplace. Idea is , Domain.Models to be:
User , Role , Question, QuestionType, QuestionForm, Answer
User are Administrators to be able to add, modify(update), delete Questions as per Roles, Users as Employees are Answer the QuestionForm with Question as Type of Scale from 1 to 10, or fill with Text. Answer has to be stored in database for future reporting, with UserId, QuestionFormId, QuestionId.
Main page will have login screen with CopmanyId and FullName of User
After successfully login, enter to QuestionForm with 3 Questions , like:
QuestionForm Title "Општо задоволство"
QuestionId 1 "Задоволен сум од мојата моментална работа"
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionId 2 "Чувствувам дека мојата работа е ценета во рамките на компанијата." 
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionId 3 "Се чувствувам мотивиран да одам на работа секој ден."
Answer is int from Scale 1 2 3 4 5 6 7 8 9 10
QuestionForm Title "Отворени прашања"
QuestionId 1 "Што најмногу ви се допаѓа на вашето сегашно работно место? "
Answer is string Text (500)
QuestionId 2 "Кои се најголемите предизвици со кои се соочувате на работа?" 
Answer is string Text (500)
QuestionId 3 "Какви предлози имате за подобрување на работната средина или процесите на компанијата?" 
Answer is string Text (500)
Client Browser → MVC Controllers → Services Layer → DataAccess Layer → Database
     ↓              ↓                  ↓               ↓               ↓
   Views       ViewModels        Business Logic     EF Context     SQL Server
     ↓              ↓                  ↓               ↓               ↓
   HTML         Data Transfer    Validation &      Repository      Tables:
   Forms         Objects         Processing        Pattern        Users, Roles, 
                                                               Questions, Forms, 
                                                               Answers
📋 ARCHITECTURE DIAGRAM

Client Browser 
     ↓
MVC Controllers (UserController, FormController, AnswerController)
     ↓
ViewModels (Data Transfer Objects with Validation)
     ↓
Services Layer (Business Logic & Validation)
     ↓
Repository Pattern (Data Access Abstraction)
     ↓
Entity Framework Context
     ↓
SQL Server Database
✅
Good Separation of Concerns - Clear layers (Domain, DataAccess, Services)

Repository Pattern - Proper abstraction of data access

Dependency Injection - Well implemented with extension methods

Comprehensive Seed Data - Realistic user data with organizational units

Proper Async/Await - Good use of asynchronous programming
📊 Domain Models
namespace Anketa.Domain.Models
{
    public class User
  {
      public int Id { get; set; }
      public int CompanyId { get; set; }
      public string FullName { get; set; }
      public string OU { get; set; } // Organizational Unit
      public string Password { get; set; }
      public int RoleId { get; set; }
      public Role Role { get; set; }
      public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
      public ICollection<Answer> Answers { get; set; }
  }
}
namespace Anketa.Domain.Models
{
   public class Role
 {
     public int Id { get; set; }
     public string Name { get; set; }
     public ICollection<User> Users { get; set; }
 }
}
namespace Anketa.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; } // Admin user who created the question
        public User User { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public bool IsRequired { get; set; } = true;
        public ICollection<Answer> Answers { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class QuestionType
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Scale", "Text"
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class QuestionForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
namespace Anketa.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public int? ScaleValue { get; set; }
        public string TextValue { get; set; }
        public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
    }
}
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
            // Apply seed data
            modelBuilder.SeedData();

            // Configure relationships
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
                .WithMany(f => f.Answers)
                .HasForeignKey(a => a.QuestionFormId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure other relationships
            modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionForm)
                .WithMany(f => f.Questions)
                .HasForeignKey(q => q.QuestionFormId)
                .OnDelete(DeleteBehavior.Cascade); // This can stay as Cascade

            modelBuilder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionType)
                .WithMany(qt => qt.Questions)
                .HasForeignKey(q => q.QuestionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
namespace Anketa.DataAccess
{
    public static class DataSeedExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "Employee" }
            );
            // Seed Question Types
            modelBuilder.Entity<QuestionType>().HasData(
                new QuestionType { Id = 1, Name = "Scale", Description = "1-10 Scale" },
                new QuestionType { Id = 2, Name = "Text", Description = "Text Answer" }
            );
            // Seed Question Forms with OU
            modelBuilder.Entity<QuestionForm>().HasData(
                new QuestionForm { Id = 1, Title = "Општо задоволство", Description = "Overall Satisfaction" },
                new QuestionForm { Id = 2, Title = "Обврска кон компанијата", Description = "Commitment to the Company" },
                
                new QuestionForm { Id = 12, Title = "Отворени прашања", Description = "Open Questions" }
            );
            // Seed Questions (UserId = 1 for Admin user, No OU)
            modelBuilder.Entity<Question>().HasData(
                    // Form 1: Општо задоволство (Scale questions)
                    new Question { Id = 1, Text = "Задоволен сум од мојата моментална работа", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
                    new Question { Id = 2, Text = "Чувствувам дека мојата работа е ценета во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
                    new Question { Id = 3, Text = "Се чувствувам мотивиран да одам на работа секој ден", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },

                   
                    // Form 11: "Конечна евалуација" (Scale and text questions)
                    new Question { Id = 32, Text = "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", QuestionTypeId = 1, QuestionFormId = 11, UserId = 1 },
                    new Question { Id = 33, Text = "Како ја гледате вашата иднина во оваа компанија во следните 2-3 години?", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },
                    new Question { Id = 34, Text = "разно", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },

                    // Form 12: Отворени прашања (Text questions)
                    new Question { Id = 35, Text = "Што најмногу ви се допаѓа на вашето сегашно работно место?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
                    new Question { Id = 36, Text = "Кои се најголемите предизвици со кои се соочувате на работа?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
                    new Question { Id = 37, Text = "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 }
                );
            // Seed Users with OU
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, CompanyId = 16130, FullName = "Vasko Gjorgiev", OU = "Production", Password = "16130", RoleId = 2 },
                new User { Id = 2, CompanyId = 16684, FullName = "Zoran Stojanovski", OU = "Production", Password = "16684", RoleId = 2 },
                
              
                new User { Id = 411, CompanyId = 21315, FullName = "Hristina Jovanovska", OU = "Projects and IT", Password = "21315", RoleId = 2 },
                new User { Id = 412, CompanyId = 21316, FullName = "Marjan Georgiev", OU = "Production", Password = "21316", RoleId = 2 }
                );
        }
    }
}
namespace Anketa.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
namespace Anketa.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByCompanyIdAsync(int companyId);
        Task<User> GetUserWithRoleAsync(int userId);
        Task<User> AuthenticateAsync(int companyId, string fullName, string password);
        Task<string> GetUserOUAsync(int userId);
    }
}
namespace Anketa.DataAccess.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<QuestionForm>> GetActiveFormsAsync();
        Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        // Admin methods
        Task<Question> CreateQuestionAsync(Question question);
        Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId);
    }
}
namespace Anketa.DataAccess.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId);
        Task<bool> HasUserCompletedFormAsync(int userId, int formId);
        Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId);
    }
}
namespace Anketa.DataAccess.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }
        public virtual void Update(T entity)
        {
            _entities.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
namespace Anketa.DataAccess.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.CompanyId == companyId);
        }

        public async Task<User> GetUserWithRoleAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<User> AuthenticateAsync(int companyId, string fullName, string password)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.CompanyId == companyId &&
                                        u.FullName == fullName &&
                                        u.Password == password);
        }

        public async Task<string> GetUserOUAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.OU)
                .FirstOrDefaultAsync();
        }
    }
}
namespace Anketa.DataAccess.Implementations
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionForm>> GetActiveFormsAsync()
        {
            return await _context.QuestionForms
                .Where(f => f.IsActive)
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }
       
        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _context.QuestionForms
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.QuestionForm)
                .Include(q => q.User)
                .OrderBy(q => q.QuestionFormId)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }
        // New method for admin to create questions
        public async Task<Question> CreateQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        // New method to get questions by form
        public async Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId)
        {
            return await _context.Questions
                .Where(q => q.QuestionFormId == formId)
                .Include(q => q.QuestionType)
                .ToListAsync();
        }
    }
}
namespace Anketa.DataAccess.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var answer in answers)
                {
                    var question = await _context.Questions
                        .Include(q => q.QuestionType)
                        .FirstOrDefaultAsync(q => q.Id == answer.Key);

                    if (question == null) continue;

                    var existingAnswer = await _context.Answers
                        .FirstOrDefaultAsync(a => a.UserId == userId &&
                                                a.QuestionId == answer.Key &&
                                                a.QuestionFormId == formId);

                    if (existingAnswer != null)
                    {
                        // Update existing answer
                        UpdateAnswerValue(existingAnswer, question.QuestionType.Name, answer.Value);
                        Update(existingAnswer);
                    }
                    else
                    {
                        // Create new answer
                        var newAnswer = new Answer
                        {
                            UserId = userId,
                            QuestionId = answer.Key,
                            QuestionFormId = formId,
                            AnsweredDate = DateTime.UtcNow
                        };
                        UpdateAnswerValue(newAnswer, question.QuestionType.Name, answer.Value);
                        await AddAsync(newAnswer);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
     private void UpdateAnswerValue(Answer answer, string questionType, object value)
        {
            if (questionType == "Scale" && value is int scaleValue)
            {
                answer.ScaleValue = scaleValue;
                answer.TextValue = null;
            }
            else if (questionType == "Text" && value is string textValue)
            {
                answer.TextValue = textValue;
                answer.ScaleValue = null;
            }
        }

        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _context.Answers
                .Where(a => a.UserId == userId && a.QuestionFormId == formId)
                .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }

        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _context.Answers
                .AnyAsync(a => a.UserId == userId && a.QuestionFormId == formId);
        }
        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _context.Answers
                .Where(a => a.QuestionFormId == formId)
                .Include(a => a.Question)
                .Include(a => a.User)
                .ToListAsync();
        }
    }
}
namespace Anketa.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(int companyId, string fullName, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<string> GetUserOUAsync(int userId);
    }
}
namespace Anketa.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionForm>> GetActiveFormsAsync();
        Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        // Admin methods
        Task<Question> CreateQuestionAsync(Question question);
        Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId);
    }
}
namespace Anketa.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId);
        Task<bool> HasUserCompletedFormAsync(int userId, int formId);
        Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId);
    }
}
namespace Anketa.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(int companyId, string fullName, string password)
        {
            return await _userRepository.AuthenticateAsync(companyId, fullName, password);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserWithRoleAsync(id);
        }

        public async Task<string> GetUserOUAsync(int userId)
        {
            return await _userRepository.GetUserOUAsync(userId);
        }
    }
}
namespace Anketa.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            return await _questionRepository.CreateQuestionAsync(question);
        }

        public async Task<IEnumerable<QuestionForm>> GetActiveFormsAsync()
        {
            return await _questionRepository.GetActiveFormsAsync();
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllQuestionsAsync();
        }

        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _questionRepository.GetFormWithQuestionsAsync(formId);
        }
               
        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _questionRepository.GetQuestionByIdAsync(questionId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId)
        {
            return await _questionRepository.GetQuestionsByFormAsync(formId);
        }
    }
}
namespace Anketa.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            return await _answerRepository.SubmitAnswersAsync(userId, formId, answers);
        }

        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _answerRepository.GetUserAnswersAsync(userId, formId);
        }
        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _answerRepository.HasUserCompletedFormAsync(userId, formId);
        }

        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _answerRepository.GetFormAnswersAsync(formId);
        }
    }
}
namespace Anketa.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));
        }
        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();
        }
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
        }
    }
}
namespace Anketa.Services.Mappers
{
    public static class Mappers
    {
        public static QuestionViewModel ToViewModel(this Question question)
        {
            return new QuestionViewModel
            {
                Id = question.Id,
                Text = question.Text,
                QuestionType = question.QuestionType?.Name,
                IsRequired = question.IsRequired
            };
        }

        public static QuestionFormViewModel ToViewModel(this QuestionForm form)
        {
            return new QuestionFormViewModel
            {
                Id = form.Id,
                Title = form.Title,
                Description = form.Description,
                Questions = form.Questions?.Select(q => q.ToViewModel())
                .ToList() ?? new List<QuestionViewModel>()
            };
        }
    }
}
namespace Anketa.Services.Mappers
{

    public static class PasswordHasherHelper
    {
        private static PasswordHasher<User> _passwordHasher = new();

        public static void HashPassword(User user, string password)
        {
            user.Password = _passwordHasher.HashPassword(user, password);
        }

        public static PasswordVerificationResult VerifyHashedPassword(User user, string password)
        {
            return _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        }
    }
}
namespace Anketa.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
namespace Anketa.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string QuestionType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
    }
}
namespace Anketa.ViewModels
{
    public class QuestionFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }
}
namespace Anketa.ViewModels
{
    public class FormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
namespace Anketa.ViewModels
{
    public class AdminFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<QuestionViewModel> Questions { get; set; } = new();
    }
}
namespace Anketa.ViewModels
{
    public class AdminQuestionCreateViewModel
    {
        [Required(ErrorMessage = "Question text is required")]
        [Display(Name = "Question Text")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a question type")]
        [Display(Name = "Question Type")]
        public string QuestionType { get; set; } = "Text"; // Default

        [Display(Name = "Required?")]
        public bool IsRequired { get; set; } = true;
    }
}
namespace Anketa.ViewModels
{
    public class AnswerInputViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Scale value must be between 1 and 10")]
        public int? ScaleValue { get; set; }

        [StringLength(500, ErrorMessage = "Text answer cannot exceed 500 characters")]
        public string TextValue { get; set; }
        public bool QuestionTypeIsScale() =>
            QuestionType.Equals("Scale", StringComparison.OrdinalIgnoreCase);
    }
}
namespace Anketa.ViewModels
{
    public class SubmitFormViewModel : FormViewModel
    {
        [Required]
        public int FormId { get; set; }

        public string FormTitle { get; set; } = string.Empty;

        public int UserId { get; set; }

        [Required]
        public List<AnswerInputViewModel> Answers { get; set; } = new();
    }
}
//Views/User/Login.cshtml
@model LoginViewModel

@{
    ViewData["Title"] = "Најава - Анкета за Задоволство";
    Layout = null;
}
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">

    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">

    <style>
        body {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            align-items: center;
        }

        .login-card {
            background: white;
            border-radius: 15px;
            box-shadow: 0 15px 35px rgba(0, 0, 0, 0.1);
            overflow: hidden;
        }

        .login-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 2rem;
            text-align: center;
        }

        .login-body {
            padding: 2rem;
        }

        .btn-login {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            color: white;
            padding: 12px;
            font-weight: 600;
        }

            .btn-login:hover {
                transform: translateY(-2px);
                box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
            }
    </style>
</head>
<body>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-5">
                <div class="login-card">
                    <div class="login-header">
                        <h2><i class="fas fa-chart-bar me-2"></i>Анкета за Задоволство</h2>
                        <p class="mb-0">Внесете ги вашите податоци за најава</p>
                    </div>
                    <div class="login-body">
                        <form asp-action="Login" method="post">
                            <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

                            <div class="mb-3">
                                <label asp-for="CompanyId" class="form-label">
                                    <i class="fas fa-id-card me-1"></i>ID на Вработениот
                                </label>
                                <input asp-for="CompanyId" class="form-control form-control-lg"
                                       placeholder="Внесете го вашиот ID" />
                                <span asp-validation-for="CompanyId" class="text-danger small"></span>
                            </div>

                            <div class="mb-3">
                                <label asp-for="FullName" class="form-label">
                                    <i class="fas fa-user me-1"></i>Име и Презиме
                                </label>
                                <input asp-for="FullName" class="form-control form-control-lg"
                                       placeholder="Внесете го вашето име и презиме" />
                                <span asp-validation-for="FullName" class="text-danger small"></span>
                            </div>

                            <div class="mb-4">
                                <label asp-for="Password" class="form-label">
                                    <i class="fas fa-lock me-1"></i>Лозинка
                                </label>
                                <input asp-for="Password" type="password" class="form-control form-control-lg"
                                       placeholder="Внесете ја вашата лозинка" />
                                <span asp-validation-for="Password" class="text-danger small"></span>
                            </div>

                            <div class="d-grid">
                                <button type="submit" class="btn btn-login btn-lg">
                                    <i class="fas fa-sign-in-alt me-2"></i>Најави се
                                </button>
                            </div>
                        </form>

                        <div class="text-center mt-4">
                            <small class="text-muted">
                                За помош контактирајте го ИТ одделот
                            </small>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap & jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Validation Scripts -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.3/jquery.validate.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.12/jquery.validate.unobtrusive.min.js"></script>
</body>
</html>
//Views/Form/Index.cshtml

@model List<QuestionFormViewModel>


@{
    ViewData["Title"] = "Анкети за Задоволство на Вработените";
}

<div class="container">
    <div class="row">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h2><i class="fas fa-clipboard-list text-primary"></i> Достапни Анкети</h2>
                <div>
                    <span class="badge bg-primary fs-6">@Model.Count анкети</span>
                </div>
            </div>

            @if (TempData["Success"] != null)
            {
                <div class="alert alert-success alert-dismissible fade show" role="alert">
                    <i class="fas fa-check-circle"></i> @TempData["Success"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            }

            @if (TempData["Message"] != null)
            {
                <div class="alert alert-info alert-dismissible fade show" role="alert">
                    <i class="fas fa-info-circle"></i> @TempData["Message"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            }

            @if (TempData["Error"] != null)
            {
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                    <i class="fas fa-exclamation-triangle"></i> @TempData["Error"]
                    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            }

            @if (!Model.Any())
            {
                <div class="text-center py-5">
                    <i class="fas fa-inbox fa-4x text-muted mb-3"></i>
                    <h4 class="text-muted">Нема достапни анкети</h4>
                    <p class="text-muted">Во моментов нема активни анкети. Ве молиме проверете подоцна.</p>
                </div>
            }
            else
            {
                <div class="row">
                    @foreach (var form in Model)
                    {
                        <div class="col-md-6 col-lg-4 mb-4">
                            <div class="card h-100 shadow-sm">
                                <div class="card-body d-flex flex-column">
                                    <div class="d-flex justify-content-between align-items-start mb-3">
                                        <h5 class="card-title text-primary">@form.Title</h5>
                                        <span class="badge bg-success">@form.Questions.Count прашања</span>
                                    </div>
                                    <p class="card-text flex-grow-1">@form.Description</p>
                                    <div class="mt-auto">
                                        <div class="d-grid gap-2">
                                            <a href="@Url.Action("Answer", new { formId = form.Id })" 
                                               class="btn btn-primary btn-lg">
                                                <i class="fas fa-play-circle"></i> Започни Анкета
                                            </a>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-footer bg-transparent">
                                    <small class="text-muted">
                                        <i class="fas fa-question-circle"></i> @form.Questions.Count прашања
                                    </small>
                                </div>
                            </div>
                        </div>
                    }
                </div>
            }
        </div>
    </div>
</div>

@section Scripts {
    <script>
        $(document).ready(function () {
            // Auto-dismiss alerts after 5 seconds
            setTimeout(function () {
                $('.alert').alert('close');
            }, 5000);

            // Add hover effects to cards
            $('.card').hover(
                function () {
                    $(this).css('transform', 'translateY(-5px)');
                    $(this).css('box-shadow', '0 8px 15px rgba(0,0,0,0.1)');
                },
                function () {
                    $(this).css('transform', 'translateY(0)');
                    $(this).css('box-shadow', '0 4px 6px rgba(0,0,0,0.1)');
                }
            );
        });
    </script>
}
//Views/Form/Answer.cshtml
@model SubmitFormViewModel

@{
    ViewData["Title"] = Model.FormTitle;
}

<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-10">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">@Model.FormTitle</h4>
                </div>
                <div class="card-body">
                    @if (TempData["Error"] != null)
                    {
                        <div class="alert alert-danger">@TempData["Error"]</div>
                    }

                    <form asp-action="Submit" asp-controller="Answer" method="post">
                        <input type="hidden" name="formId" value="@Model.FormId" />
                        
                        @for (int i = 0; i < Model.Answers.Count; i++)
                        {
                            <div class="question-group mb-4 p-3 border rounded">
                                <h5 class="question-text">@(i + 1). @Model.Answers[i].QuestionText</h5>
                                
                                @if (Model.Answers[i].QuestionType == "Scale")
                                {
                                    <div class="scale-answers">
                                        <div class="btn-group btn-group-toggle d-flex flex-wrap" data-toggle="buttons">
                                            @for (int scale = 1; scale <= 10; scale++)
                                            {
                                                <label class="btn btn-outline-primary scale-option m-1">
                                                    <input type="radio" 
                                                           name="answers[@Model.Answers[i].QuestionId]" 
                                                           value="@scale" 
                                                           required 
                                                           class="scale-input">
                                                    @scale
                                                </label>
                                            }
                                        </div>
                                        <div class="scale-labels d-flex justify-content-between mt-2">
                                            <small class="text-muted">1 - Многу незадоволен</small>
                                            <small class="text-muted">10 - Многу задоволен</small>
                                        </div>
                                    </div>
                                }
                                else if (Model.Answers[i].QuestionType == "Text")
                                {
                                    <div class="text-answer">
                                        <textarea class="form-control" 
                                                  name="answers[@Model.Answers[i].QuestionId]" 
                                                  rows="4" 
                                                  maxlength="500"
                                                  placeholder="Внесете го вашиот одговор овде (макс. 500 карактери)..."
                                                  required></textarea>
                                        <small class="text-muted float-end"><span class="char-count">0</span>/500 карактери</small>
                                    </div>
                                }
                                
                                @if (!string.IsNullOrEmpty(ViewData.ModelState[$"answers[{Model.Answers[i].QuestionId}]"]?.Errors?.FirstOrDefault()?.ErrorMessage))
                                {
                                    <span class="text-danger">
                                        @ViewData.ModelState[$"answers[{Model.Answers[i].QuestionId}]"]?.Errors?.FirstOrDefault()?.ErrorMessage
                                    </span>
                                }
                            </div>
                        }
                        
                        <div class="form-group mt-4">
                            <button type="submit" class="btn btn-success btn-lg w-100">
                                <i class="fas fa-paper-plane"></i> Поднеси ги одговорите
                            </button>
                            <a href="@Url.Action("Index")" class="btn btn-secondary w-100 mt-2">
                                <i class="fas fa-arrow-left"></i> Назад кон листата
                            </a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        $(document).ready(function () {
            // Scale answer selection styling
            $('.scale-input').change(function () {
                $(this).closest('.scale-option').removeClass('btn-outline-primary').addClass('btn-primary');
                $(this).closest('.btn-group').find('.scale-option').not(this).removeClass('btn-primary').addClass('btn-outline-primary');
            });

            // Character counter for text areas
            $('textarea').on('input', function () {
                var length = $(this).val().length;
                $(this).siblings('.char-count').text(length);
                
                if (length > 450) {
                    $(this).siblings('.text-muted').addClass('text-warning');
                } else {
                    $(this).siblings('.text-muted').removeClass('text-warning');
                }
            });

            // Initialize character counts
            $('textarea').each(function () {
                var length = $(this).val().length;
                $(this).siblings('.char-count').text(length);
            });

            // Form validation
            $('form').on('submit', function () {
                var isValid = true;
                $('.question-group').each(function () {
                    var hasAnswer = false;
                    var questionType = $(this).find('.scale-input').length > 0 ? 'scale' : 'text';
                    
                    if (questionType === 'scale') {
                        hasAnswer = $(this).find('.scale-input:checked').length > 0;
                    } else {
                        hasAnswer = $(this).find('textarea').val().trim().length > 0;
                    }
                    
                    if (!hasAnswer) {
                        isValid = false;
                        $(this).addClass('border-danger');
                    } else {
                        $(this).removeClass('border-danger');
                    }
                });
                
                if (!isValid) {
                    alert('Ве молам одговорете на сите прашања пред да поднесете.');
                    return false;
                }
                
                return true;
            });
        });
    </script>

    <style>
        .question-group {
            background-color: #f8f9fa;
            border-left: 4px solid #007bff;
            transition: all 0.3s ease;
        }

        .question-group:hover {
            background-color: #e9ecef;
        }

        .question-text {
            color: #2c3e50;
            font-weight: 600;
            margin-bottom: 15px;
        }

        .scale-option {
            min-width: 45px;
            text-align: center;
        }

        .scale-option:hover {
            transform: translateY(-2px);
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
        }

        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
        }

        .border-danger {
            border-left-color: #dc3545 !important;
            border-color: #dc3545 !important;
        }

        .char-count {
            font-weight: bold;
        }

        .text-warning {
            color: #ffc107 !important;
            font-weight: bold;
        }

        .card {
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            border: none;
            border-radius: 10px;
        }

        .card-header {
            border-radius: 10px 10px 0 0 !important;
        }

        .btn-success {
            background-color: #28a745;
            border-color: #28a745;
            padding: 12px;
            font-size: 18px;
            font-weight: 600;
        }

        .btn-success:hover {
            background-color: #218838;
            border-color: #1e7e34;
            transform: translateY(-1px);
        }
    </style>
}
//Views/Shared/_Layout.cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Анкета за Задоволство</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/Anketa.Skopje.styles.css" asp-append-version="true" />

   
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-dark bg-dark">
            <div class="container">
                <a class="navbar-brand" asp-controller="Form" asp-action="Index">
                    <i class="fas fa-chart-bar me-2"></i>Анкета за Задоволство
                </a>

                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>

                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav me-auto">
                        @if (User.Identity.IsAuthenticated)
                        {
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Form" asp-action="Index">
                                    <i class="fas fa-home me-1"></i>Анкети
                                </a>
                            </li>
                        }
                    </ul>

                    <ul class="navbar-nav">
                        @if (User.Identity.IsAuthenticated)
                        {
                            <li class="nav-item">
                                <span class="nav-link">
                                    <i class="fas fa-user me-1"></i>@User.Identity.Name
                                </span>
                            </li>
                            <li class="nav-item">
                                <form asp-controller="User" asp-action="Logout" method="post" class="d-inline">
                                    <button type="submit" class="btn btn-outline-light btn-sm">
                                        <i class="fas fa-sign-out-alt me-1"></i>Одјава
                                    </button>
                                </form>
                            </li>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    </header>

    <main role="main" class="pb-3">
        @RenderBody()
    </main>

    <footer class="footer mt-auto py-3 bg-dark text-light">
        <div class="container text-center">
            <span>&copy; 2024 - Анкета за Задоволство на Вработените</span>
        </div>
    </footer>

    <!-- Bootstrap & jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    
    @await RenderSectionAsync("Scripts", required: false)
</ body >
</ html >
namespace Anketa.Skopje.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.AuthenticateAsync(model.CompanyId, model.FullName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("CompanyId", user.CompanyId.ToString()),
                new Claim("OU", user.OU ?? "")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Form");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
namespace Anketa.Skopje.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;

        public FormController(IQuestionService questionService, IAnswerService answerService)
        {
            _questionService = questionService;
            _answerService = answerService;
        }

        public async Task<IActionResult> Index()
        {
            var forms = await _questionService.GetActiveFormsAsync();
            var viewModels = forms.Select(f => f.ToViewModel()).ToList();
            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Answer(int formId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var form = await _questionService.GetFormWithQuestionsAsync(formId);
            if (form == null)
            {
                TempData["Error"] = "Form not found.";
                return RedirectToAction("Index");
            }

            if (await _answerService.HasUserCompletedFormAsync(userId, formId))
            {
                TempData["Message"] = "You have already completed this form.";
                return RedirectToAction("Index");
            }

            var vm = new SubmitFormViewModel
            {
                FormId = formId,
                FormTitle = form.Title,
                Answers = form.Questions.Select(q => new AnswerInputViewModel
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    QuestionType = q.QuestionType.Name
                }).ToList()
            };

            return View(vm);
        }
        public IActionResult Thanks() => View();
    }
}
namespace Anketa.Skopje.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int formId, Dictionary<int, object> answers)
        {
            if (answers == null || !answers.Any())
            {
                TempData["Error"] = "No answers provided.";
                return RedirectToAction("Answer", "Form", new { formId });
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var success = await _answerService.SubmitAnswersAsync(userId, formId, answers);

            if (success)
            {
                TempData["Success"] = "Thank you for submitting your answers!";
                return RedirectToAction("Thanks", "Form");
            }

            TempData["Error"] = "Error submitting your answers. Please try again.";
            return RedirectToAction("Answer", "Form", new { formId });
        }
    }
}
## Program.cs
// Add services to the container.
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.InjectDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.InjectRepositories();
builder.Services.InjectServices();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}"); // Default to Login page

app.Run();
