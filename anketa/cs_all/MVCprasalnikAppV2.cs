** These are Domain Models, the “core truth” of your app.
**They represent database tables (User, Questionnaire, etc.).
**Enums (RoleEnum, QuestionTypeEnum) are strongly typed alternatives to raw strings.
**Relationships (User → Questionnaires, etc.) allow EF Core navigation properties
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
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
        public virtual ICollection<QuestionItem> QuestionItems { get; set; }
        public User()
        {
            Questionnaires = new HashSet<Questionnaire>();
            QuestionItems = new HashSet<QuestionItem>();
        }
    }
}
namespace Prasalnik.Domain.Models
{
    public class Questionnaire : BaseEntity
    {
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public ICollection<QuestionItem> QuestionItems { get; set; }
        public string Status { get; set; } // Answered, Skipped
        public Questionnaire()
        {
            QuestionItems = new HashSet<QuestionItem>();
        }
    }
}
namespace Prasalnik.Domain.Models
{
    public class QuestionItem : BaseEntity
    {
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypeEnum Type { get; set; }
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
🔹 DataAccess (DbContext + Configurations + Repositories)
** AppDbContext is the bridge between code and DB.
** Each DbSet<T> maps to a table.
** OnModelCreating applies Fluent API configs (constraints, FK, seeds).
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

            builder.HasIndex(x => x.CompanyId).IsUnique(); // Assuming CompanyId is unique for each user

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
** Repository Pattern = abstraction over EF queries.
** Keeps controllers/services free of LINQ clutter.
** Specialized repos (UserRepository, AnswerRepository) add custom queries.
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
** ViewModels are shaped for UI (forms, pages).
** Unlike Domain Models, they don’t need all DB fields.
** Example: UserCredentialsViewModel is only for login form.
namespace Prasalnik.ViewModels.Models
{
    public class UserViewModel
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
namespace Prasalnik.ViewModels.Models
{
    public class UserCredentialsViewModel
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; }
    }
}
namespace Prasalnik.ViewModels.Models
{
    public class RegisterUserViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
namespace Prasalnik.ViewModels.Models
{
    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public QuestionItemViewModel QuestionItems { get; set; }
    }
}
namespace Prasalnik.ViewModels.Models
{
    public class QuestionItemViewModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int QuestionnaireId { get; set; }
        public bool IsRequired { get; set; }
    }
}
namespace Prasalnik.ViewModels.Models
{
    public class AnswerViewModel
    {
        public int QuestionId { get; set; }
        public string Response { get; set; }
    }
}
** AutoMapper converts between Domain Models ↔ DTOs/ViewModels.
** Keeps controllers clean (no manual mapping).
** Example: DB User.Role = RoleEnum.Manager → VM "Manager".
namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Domain -> ViewModel
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            // ViewModel -> Domain
              CreateMap<UserViewModel, User>()
                .ForMember(d => d.Role, opt => opt.MapFrom<RoleResolver>());

            // RegisterUserViewModel -> User
            CreateMap<RegisterUserViewModel, User>()
                .ForMember(d => d.Role, opt => opt.MapFrom((src, dest, destMember, context) =>
                    Enum.TryParse<RoleEnum>(src.Role, out var role) ? role : RoleEnum.Employee));

            // User -> RegisterUserViewModel
            CreateMap<User, RegisterUserViewModel>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role.ToString()));

            // Credentials mapping
            CreateMap<UserCredentialsViewModel, User>()
                .ForMember(d => d.Role, opt => opt.Ignore()); // credentials don't provide role
        }
    }
}
namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class QuestionnaireMappingProfile : Profile
    {
        public QuestionnaireMappingProfile()
        {
            // Domain -> ViewModel (including child items)
            CreateMap<Questionnaire, QuestionnaireViewModel>()
                .ForMember(d => d.QuestionItems, opt => opt.MapFrom(s => s.QuestionItems));

            // ViewModel -> Domain
            CreateMap<QuestionnaireViewModel, Questionnaire>()
                .ForMember(d => d.QuestionItems, opt => opt.MapFrom(s => s.QuestionItems));
        }
    }
}
namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class QuestionItemMappingProfile : Profile
    {
        public QuestionItemMappingProfile()
        {
            // Domain -> ViewModel: convert enum to int
            CreateMap<QuestionItem, QuestionItemViewModel>()
                .ForMember(d => d.QuestionType, opt => opt.MapFrom(s => (int)s.Type))
                .ForMember(d => d.QuestionnaireId, opt => opt.MapFrom(s => s.QuestionnaireId));

            // ViewModel -> Domain: convert int back to enum safely
            CreateMap<QuestionItemViewModel, QuestionItem>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s =>
                    Enum.IsDefined(typeof(QuestionTypeEnum), s.QuestionType)
                        ? (QuestionTypeEnum)s.QuestionType
                        : QuestionTypeEnum.Text))
                .ForMember(d => d.QuestionnaireId, opt => opt.MapFrom(s => s.QuestionnaireId));
        }
    }
}
namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class AnswerMappingProfile : Profile
    {
        public AnswerMappingProfile()
        {
            CreateMap<Answer, AnswerViewModel>().ReverseMap();
        }
    }
}
namespace Prasalnik.Mappers.AutoMapperProfiles
{
    public class StatusMappingProfile : Profile
    {
        public StatusMappingProfile()
        {
            CreateMap<Status, StatusViewModel>().ReverseMap();
        }
    }
}
** Services = business logic layer.
** They consume repositories and add validation, rules, or transactions.
** Good that you don’t call repositories directly from controllers.
**⚠️ Note: You currently return Domain Models from services.
**👉 Better practice: return DTOs/ViewModels so controllers don’t leak DB entities.
namespace Prasalnik.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User Login(int companyId, string fullName);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}
namespace Prasalnik.Services.Interfaces
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAllStatuses();
        Status GetStatusById(int id);
        Status GetByName(string name);
        void CreateStatus(Status status);
        void UpdateStatus(Status status);
        void DeleteStatus(int id);
    }
}
namespace Prasalnik.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        IEnumerable<Questionnaire> GetAllQuestionnaires();
        Questionnaire GetQuestionnaireById(int id);
        Questionnaire GetByUserId(int userId);
        void CreateQuestionnaire(Questionnaire questionnaire);
        void UpdateQuestionnaire(Questionnaire questionnaire);
        void DeleteQuestionnaire(int id);
    }
}
namespace Prasalnik.Services.Interfaces
{
    public interface IQuestionItemService
    {
        IEnumerable<QuestionItem> GetAllItems();
        QuestionItem GetItemById(int id);
        QuestionItem GetByType(QuestionTypeEnum type);
        void CreateItem(QuestionItem item);
        void UpdateItem(QuestionItem item);
        void DeleteItem(int id);
    }
}
namespace Prasalnik.Services.Interfaces
{
    public interface IAnswerService
    {
        IEnumerable<Answer> GetAllAnswers();
        Answer GetAnswerById(int id);
        Answer GetByUserId(int userId);
        void CreateAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(int id);
    }
}
namespace Prasalnik.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();
        public User GetUserById(int id) => _userRepository.GetById(id);
        public User Login(int companyId, string fullName) => _userRepository.LoginUser(companyId, fullName);
        public void CreateUser(User user) => _userRepository.Create(user);
        public void UpdateUser(User user) => _userRepository.Update(user);
        public void DeleteUser(int id) => _userRepository.Delete(id);
    }
}
namespace Prasalnik.Services.Implementations
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;
        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository)
        {
            _questionnaireRepository = questionnaireRepository;
        }

        public IEnumerable<Questionnaire> GetAllQuestionnaires() => _questionnaireRepository.GetAll();
        public Questionnaire GetQuestionnaireById(int id) => _questionnaireRepository.GetById(id);
        public Questionnaire GetByUserId(int userId) => _questionnaireRepository.GetbyUserId(userId);
        public void CreateQuestionnaire(Questionnaire questionnaire) => _questionnaireRepository.Create(questionnaire);
        public void UpdateQuestionnaire(Questionnaire questionnaire) => _questionnaireRepository.Update(questionnaire);
        public void DeleteQuestionnaire(int id) => _questionnaireRepository.Delete(id);
    }
}
namespace Prasalnik.Services.Implementations
{
    public class QuestionItemService : IQuestionItemService
    {
        private readonly IQuestionItemRepository _questionItemRepository;
        public QuestionItemService(IQuestionItemRepository questionItemRepository)
        {
            _questionItemRepository = questionItemRepository;
        }

        public IEnumerable<QuestionItem> GetAllItems() => _questionItemRepository.GetAll();
        public QuestionItem GetItemById(int id) => _questionItemRepository.GetById(id);
        public QuestionItem GetByType(QuestionTypeEnum type) => _questionItemRepository.GetByType(type.GetType());
        public void CreateItem(QuestionItem item) => _questionItemRepository.Create(item);
        public void UpdateItem(QuestionItem item) => _questionItemRepository.Update(item);
        public void DeleteItem(int id) => _questionItemRepository.Delete(id);
    }
}
namespace Prasalnik.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }
        public IEnumerable<Answer> GetAllAnswers() => _answerRepository.GetAll();
        public Answer GetAnswerById(int id) => _answerRepository.GetById(id);
        public Answer GetByUserId(int userId) => _answerRepository.GetByUserId(userId);
        public void CreateAnswer(Answer answer) => _answerRepository.Create(answer);
        public void UpdateAnswer(Answer answer) => _answerRepository.Update(answer);
        public void DeleteAnswer(int id) => _answerRepository.Delete(id);
    }
}
namespace Prasalnik.Services.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }
        public IEnumerable<Status> GetAllStatuses() => _statusRepository.GetAll();
        public Status GetStatusById(int id) => _statusRepository.GetById(id);
        public Status GetByName(string name) => _statusRepository.GetByName(name);
        public void CreateStatus(Status status) => _statusRepository.Create(status);
        public void UpdateStatus(Status status) => _statusRepository.Update(status);
        public void DeleteStatus(int id) => _statusRepository.Delete(id);
    }
}
namespace Prasalnik.Mapers
{
    public class RoleResolver : IValueResolver<UserViewModel, User, RoleEnum>
    {
        public RoleEnum Resolve(UserViewModel source, User destination, RoleEnum destMember, ResolutionContext context)
        {
            var roleRaw = source.Role?.Trim();
            if (string.IsNullOrEmpty(roleRaw))
                return RoleEnum.Employee;

            // Try parse name or number
            if (Enum.TryParse<RoleEnum>(roleRaw, ignoreCase: true, out var parsed)
                && Enum.IsDefined(typeof(RoleEnum), parsed))
            {
                return parsed;
            }

            // Optional: log unexpected value (context.Items or ILogger via DI)
            // throw new ArgumentException($"Invalid role value: {source.Role}");

            return RoleEnum.Employee;
        }
    }
}
** Controllers = entry point from HTTP → Service Layer.
** They accept ViewModels, call Services, then return Views.
** Good: You use Session to keep logged user.
namespace Prasalnik.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserCredentialsViewModel());
        }

        [HttpPost]
        public IActionResult Login(UserCredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
                return View(credentials);

            var user = _userService.Login(credentials);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View(credentials);
            }

            // Store in session
            HttpContext.Session.SetString("UserId", user.CompanyId.ToString());
            HttpContext.Session.SetString("UserName", user.FullName);

            return RedirectToAction("Index", "Questionnaire");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
** Simple Razor form bound to UserCredentialsViewModel.
** Posts back to UserController.Login().
** After login → redirect to Questionnaire page.
** Views\User\Login.cshtml
@model Prasalnik.ViewModels.Models.UserCredentialsViewModel

@{
    ViewData["Title"] = "Login";
}

<h2>Login</h2>

<form asp-action="Login" method="post">
    <div class="form-group">
        <label asp-for="CompanyId"></label>
        <input asp-for="CompanyId" class="form-control" />
        <span asp-validation-for="CompanyId" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="FullName"></label>
        <input asp-for="FullName" class="form-control" />
        <span asp-validation-for="FullName" class="text-danger"></span>
    </div>
    <button type="submit" class="btn btn-primary">Login</button>
</form>

@if (ViewBag.Error != null)
{
    <div class="alert alert-danger">@ViewBag.Error</div>
}
** Views\Questionnaire\Index.cshtml
@model IEnumerable<Prasalnik.ViewModels.Models.QuestionnaireViewModel>

@{
    ViewData["Title"] = "Questionnaires";
}

<h2>Available Questionnaires</h2>

@if (!Model.Any())
{
    <p>No questionnaires available.</p>
}
else
{
    < ul >
        @foreach(var q in Model)
        {
            < li >
                < a asp - controller = "Questionnaire" asp - action = "Details" asp - route - id = "@q.Id" >
                    @q.Title - Status: @q.Status
                </ a >
            </ li >
        }
    </ ul >
}
** Views\Questionnaire\Details.cshtml
@model Prasalnik.ViewModels.Models.QuestionnaireViewModel

@{
    ViewData["Title"] = "Questionnaire Details";
}

<h2>@Model.Title</h2>

<form asp-action="SubmitAnswers" method="post">
    <input type="hidden" name="QuestionnaireId" value="@Model.Id" />

    @foreach (var question in Model.QuestionItems)
    {
        <div class="form-group">
            <label>@question.QuestionText</label>

            <!-- Use question.Id as key in Answers[] -->
            <input type="text" name="Answers[@question.Id]" class="form-control" />
        </div>
    }

    <button type="submit" class="btn btn-success" > Submit </ button >
</ form >
# Programs.cs
var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

// ... add others
IServiceCollection serviceCollection = builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
builder.Services.AddSession();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>();
builder.Services.AddScoped<IQuestionItemRepository, QuestionItemRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();
builder.Services.AddScoped<IQuestionItemService, QuestionItemService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IStatusService, StatusService>();

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
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();