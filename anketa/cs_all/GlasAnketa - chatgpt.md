## 1) DomainModels
```csharp
 public class User
 {
     public int Id { get; set; }
     public int CompanyId { get; set; }
     public string FullName { get; set; }
     public string OU { get; set; } // Organizational Unit
     public string OU2 { get; set; } // Secondary Level Organizational Unit
     public string Password { get; set; }
     public int RoleId { get; set; }
     public Role Role { get; set; }
     public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
     public ICollection<Answer> Answers { get; set; }
     public User()  {  Answers = new HashSet<Answer>();  }
 }
 public class Role
 {
     public int Id { get; set; }
     public string Name { get; set; }
     public virtual ICollection<User> Users { get; set; }
     public Role()  {  Users = new HashSet<User>();  }
 }
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
     public Question() { Answers = new HashSet<Answer>();   }
 }
  public class QuestionType
 {
     public int Id { get; set; }
     public string Name { get; set; } // "Scale", "Text"
     public string Description { get; set; }
     public ICollection<Question> Questions { get; set; }
     public QuestionType()  { Questions = new HashSet<Question>();   }
 }
     public class QuestionForm
   {
       public int Id { get; set; }
       public string Title { get; set; }
       public string Description { get; set; }
       public bool IsActive { get; set; } = true;
       public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
       public ICollection<Question> Questions { get; set; }
       public ICollection<Answer> Answers { get; set; }
       public QuestionForm()
            {
                Questions = new HashSet<Question>();
                Answers = new HashSet<Answer>();
            }
   }
     public class Answer
  {
      public int Id { get; set; }
      public int CompanyId { get; set; }
      public int UserId { get; set; }
      public User User { get; set; }
      public int QuestionId { get; set; }
      public Question Question { get; set; }
      public int QuestionFormId { get; set; }
      public QuestionForm QuestionForm { get; set; }
      public int? ScaleValue { get; set; } // For scale-based questions, nulable if not applicable
      public string? TextValue { get; set; } // For text-based questions
      public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
  }
---
## 3) DataAccess.Repository DataSeed, AppDbContext
```csharp
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
        modelBuilder.Entity<Answer>()
            .HasIndex(a => new { a.UserId, a.QuestionId, a.QuestionFormId })
            .IsUnique();
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
 public static class DataSeedExtensions
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
           // Seed Question Types
        modelBuilder.Entity<QuestionType>().HasData(
            new QuestionType { Id = 1, Name = "Scale", Description = "1-10 Scale" },
            new QuestionType { Id = 2, Name = "Text", Description = "Text Answer" }
        );
        // Seed Question Forms with OU
        modelBuilder.Entity<QuestionForm>().HasData(
            new QuestionForm { Id = 1, Title = "Општо задоволство", Description = "Overall Satisfaction" },
            new QuestionForm { Id = 2, Title = "Обврска кон компанијата", Description = "Commitment to the Company" },
            new QuestionForm { Id = 3, Title = "Отворени прашања", Description = "Open Questions" }
        );
        // Seed Questions (UserId = 1 for Admin user, No OU)
        modelBuilder.Entity<Question>().HasData(
            // Form 1: Општо задоволство (Scale questions)
            new Question { Id = 1, Text = "Задоволен сум од мојата моментална работа", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
            new Question { Id = 2, Text = "Чувствувам дека мојата работа е ценета во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
            new Question { Id = 3, Text = "Се чувствувам мотивиран да одам на работа секој ден", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
               // Form 11: "Конечна евалуација" (Scale and text questions)
            new Question { Id = 31, Text = "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", QuestionTypeId = 1, QuestionFormId = 11, UserId = 1 },
            new Question { Id = 32, Text = "Како ја гледате вашата иднина во оваа компанија во следните 2-3 години?", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },
            new Question { Id = 33, Text = "разно", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },
           // Form 12: Отворени прашања (Text questions)
            new Question { Id = 34, Text = "Што најмногу ви се допаѓа на вашето сегашно работно место?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
            new Question { Id = 35, Text = "Кои се најголемите предизвици со кои се соочувате на работа?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
            new Question { Id = 36, Text = "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 }
        );
        // Seed Users with OU
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, CompanyId = 16130, FullName = "Vasko Gjorgiev", OU = "Production", OU2 = "Rolling Unit", Password = "16130", RoleId = 2 },
            new User { Id = 2, CompanyId = 16684, FullName = "Zoran Stojanovski", OU = "Production", OU2 = "Rolling Unit", Password = "16684", RoleId = 2 },
            new User { Id = 3, CompanyId = 16695, FullName = "Pane Naskovski", OU = "Production", OU2 = "Rolling Unit", Password = "16695", RoleId = 2 },
            new User { Id = 4, CompanyId = 16720, FullName = "Tome Trajanov", OU = "Projects and IT", OU2 = "Crane Maintenance & Internal Transport", Password = "16720", RoleId = 2 },
            new User { Id = 5, CompanyId = 16827, FullName = "Zoran Boshkovski", OU = "Production", OU2 = "Rolling Unit", Password = "16827", RoleId = 1 });
                // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Administrator" },
            new Role { Id = 2, Name = "Employee" }
        );
}  }   
 public interface IRepository<T> where T : class
 {
     Task<T> GetByIdAsync(int id);
     Task<IEnumerable<T>> GetAllAsync();
     Task<T> AddAsync(T entity);
     Task Update(T entity);
     Task Remove(T entity);
 }    
 public interface IUserRepository : IRepository<User>
 {
     Task<User> GetByCompanyIdAsync(int companyId);
     Task<User> AuthenticateAsync(int companyId, string password);
     Task<string> GetUserOUAsync(int userId);
 }
 public interface IQuestionRepository : IRepository<Question>
 {
    Task<Question> GetByUserIdAsync (int userId);
    Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
 }
public interface IQuestionFormRepository : IRepository<QuestionForm>
{
    Task<List<QuestionForm>> GetAllFormQuestionsAsync();
}
 public interface IAnswerRepository : IRepository<Answer>
 {
     // submit Answer to database
     Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
     // Get Operations
     Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId);
     Task<List<Answer>> GetAnswersByUserIdAsync(int userId);
     Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
     Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId);
 }
  public class Repository<T> : IRepository<T> where T : class
 {
     protected readonly AppDbContext _context;
     protected readonly DbSet<T> _entities;

     public Repository(AppDbContext context)
     {
         _context = context;
         _entities = context.Set<T>();
     }

     public virtual async Task<T> GetByIdAsync(int id)
         => await _entities.FindAsync(id);

     public virtual async Task<IEnumerable<T>> GetAllAsync()
         => await _entities.ToListAsync();

     public virtual async Task<T> AddAsync(T entity)
     {
         await _entities.AddAsync(entity);
         await _context.SaveChangesAsync();
         return entity;
     }

     public virtual async Task Update(T entity)
     {
         _entities.Update(entity);
         await _context.SaveChangesAsync();
     }

     public virtual async Task Remove(T entity)
     {
         _entities.Remove(entity);
         await _context.SaveChangesAsync();
     }
 }
public class UserRepository : Repository<User>, IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<User> AuthenticateAsync(int companyId, string password)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.CompanyId == companyId && u.Password == password);
    }
    public async Task<User> GetByCompanyIdAsync(int companyId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.CompanyId == companyId);
    }
    public async Task<string> GetUserOUAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.OU)
            .FirstOrDefaultAsync();
    }
}
public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    private readonly AppDbContext _context;
    public QuestionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Question> GetByUserIdAsync(int userId)
    {
        return await _context.Questions
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
    }
    public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
    {
        return await _context.QuestionForms
            .Include(f => f.Questions)
            .ThenInclude(q => q.QuestionType)
            .FirstOrDefaultAsync(f => f.Id == formId);
    }
}
public class QuestionFormRepository : Repository<QuestionForm>, IQuestionFormRepository
{
    private readonly AppDbContext _context;
    public QuestionFormRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<QuestionForm>> GetAllFormQuestionsAsync()
    {
        return await _context.QuestionForms
            .Include(f => f.Questions)
            .ThenInclude(q => q.QuestionType)
            .ToListAsync();
    }
}
public class AnswerRepository : Repository<Answer>, IAnswerRepository
{
    private readonly AppDbContext _context;
    public AnswerRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId)
    {
        return await _context.Answers
                    .Where(a => a.QuestionFormId == questionFormId)
                    .ToListAsync();
    }
    public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
    {
        return await _context.Answers
                    .Where(a => a.QuestionId == questionId)
                    .ToListAsync();
    }
    public async Task<List<Answer>> GetAnswersByUserIdAsync(int userId)
    {
        return await _context.Answers
                    .Where(a => a.UserId == userId)
                    .ToListAsync();
    }
    public async Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId)
    {
        return await _context.Answers
                .FirstOrDefaultAsync(a => a.UserId == userId &&
                                          a.QuestionId == questionId &&
                                          a.QuestionFormId == questionFormId);
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
                if (question == null)
                {
                    continue;
                }
                // Check for existing answer
                var existingAnswer = await _context.Answers
                    .FirstOrDefaultAsync(a => a.UserId == userId &&
                                            a.QuestionId == answer.Key &&
                                            a.QuestionFormId == formId);
                if (existingAnswer != null)
                {
                    UpdateAnswerValue(existingAnswer, question.QuestionType.Name, answer.Value);
                    Update(existingAnswer);
                }
                else
                {
                    var userCompanyId = await _context.Users
                                    .Where(u => u.Id == userId)
                                    .Select(u => u.CompanyId)
                                    .FirstOrDefaultAsync();
                    var newAnswer = new Answer
                    {
                        UserId = userId,
                        CompanyId = userCompanyId,
                        QuestionId = answer.Key,
                        QuestionFormId = formId,
                        AnsweredDate = DateTime.UtcNow
                    };
                    UpdateAnswerValue(newAnswer, question.QuestionType.Name, answer.Value);
                    await AddAsync(newAnswer);
                }
            }

            var saveResult = await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return saveResult > 0;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // Log inner exception if exists
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            return false;
        }
    }
    private void UpdateAnswerValue(Answer answer, string questionType, object value)
    {
        if (questionType == "Scale")
        {
            if (value is int scaleValue)
            {
                answer.ScaleValue = scaleValue;
                answer.TextValue = null;
            }
            else if (value is string stringValue && int.TryParse(stringValue, out int parsedValue))
            {
                answer.ScaleValue = parsedValue;
                answer.TextValue = null;
            }
            else
            {
                Console.WriteLine($"❌ Invalid scale value: {value} (Type: {value.GetType()})");
            }
        }
        else if (questionType == "Text")
        {
            answer.TextValue = value?.ToString();
            answer.ScaleValue = null;
        }
        else
        {
            Console.WriteLine($"❌ Unknown question type: {questionType}");
        }
    }
}
```
## 4) ViewModels

```csharp
 public class UserVM
 {
         public int Id { get; set; }
         public int CompanyId { get; set; }
         public string FullName { get; set; }
         public string OU { get; set; }
         public string OU2 { get; set; }
         public RoleVM Role { get; set; }
 }
public class RoleVM
{
    public int RoleId { get; set; }
    public string Name { get; set; }
}
  public class UserCredentialsVM
 {
     [Required(ErrorMessage = "Company ID is required")]
     [Display(Name = "Company ID")]
     public int CompanyId { get; set; }

     [Required(ErrorMessage = "Password is required")]
     [DataType(DataType.Password)]
     public string Password { get; set; }
 }
 public class RegisterUserVM
 {
     public int CompanyId { get; set; }
     public string FullName { get; set; }
     public string OU { get; set; }
     public string OU2 { get; set; }
     public string Password { get; set; }
 }
 public class CreateQuestionFormVM
 {
     [Required(ErrorMessage = "Title is required")]
     [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
     public string Title { get; set; }

     [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
     public string Description { get; set; }

     public bool IsActive { get; set; } = true;

     // Initial questions
     public List<RegisterQuestionVM> Questions { get; set; } = new List<RegisterQuestionVM>();
 }
 public class RegisterQuestionVM
 {
     public int Id { get; set; }
     public int QuestionFormId { get; set; }
     public string Text { get; set; }
     public int QuestionTypeId { get; set; }
 }
    public class QuestionFormVM
   {
       public int Id { get; set; }

       [Required(ErrorMessage = "Title is required")]
       [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
       public string Title { get; set; }

       [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
       public string Description { get; set; }

       public bool IsActive { get; set; } = true;
       public DateTime CreatedDate { get; set; }
       public int QuestionCount { get; set; }
       public int ResponseCount { get; set; }

       // Navigation properties
       public List<QuestionVM> Questions { get; set; } = new List<QuestionVM>();
       public List<AnswerVM> Answers { get; set; } = new List<AnswerVM>();
   }
```
## 5) Services /interfaces - implementations - automappers - extensions - helpers..

```csharp
 public class UserMappingProfile : Profile
 {
     public UserMappingProfile()
     {
         CreateMap<User, UserVM>()
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
             .ReverseMap();
         CreateMap<User, RegisterUserVM>().ReverseMap();
     }
 }
  public class RoleMappingProfile : Profile
 {
     public RoleMappingProfile() { CreateMap<Role, RoleVM>().ReverseMap(); }
 }
 public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile() 
    {
        CreateMap<Question, QuestionVM >().ReverseMap();
    }
}
public class QuestionFormMappingProfile : Profile
{
    public QuestionFormMappingProfile() { CreateMap<QuestionForm, QuestionFormVM>().ReverseMap(); }
}
 public interface IUserService
 {
     Task<UserVM> RegisterUser(RegisterUserVM registerUserVM, RoleVM roleVM);
     Task<QuestionVM> RegisterQuestion(RegisterQuestionVM registerQuestionVM, RoleVM roleVM);
     Task<QuestionFormVM> CreateQuestionForm(CreateQuestionFormVM createQuestionFormVM, RoleVM roleVM);
     Task<QuestionFormVM> UpdateFormAsync(QuestionFormVM formVm, RoleVM roleVM);
     Task<UserVM> GetUserById(int id);
     Task<UserVM> ValidateUser(UserCredentialsVM userCredentialsVM);
 }
  public interface IQuestionFormService
 {
     Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);
     
 }
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuestionFormRepository _questionFormRepository;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IQuestionRepository questionRepository,
        IQuestionFormRepository questionFormRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _questionRepository = questionRepository;
        _questionFormRepository = questionFormRepository;
        _mapper = mapper;
    }

    // ✅ Register a new employee or admin user
    public async Task<UserVM> RegisterUser(RegisterUserVM registerUserVM, RoleVM roleVM)
    {
        var user = _mapper.Map<User>(registerUserVM);
        user.Password = registerUserVM.Password;
        user.RoleId = roleVM.Name == "Administrator" ? 1 : 2;
        user.CreatedDate = DateTime.UtcNow;

        await _userRepository.AddAsync(user);

        var createdUser = await _userRepository.GetByCompanyIdAsync(user.CompanyId);
        return _mapper.Map<UserVM>(createdUser);
    }

    // ✅ Get user by ID
    public async Task<UserVM> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserVM>(user);
    }

    // ✅ Validate login credentials
    public async Task<UserVM> ValidateUser(UserCredentialsVM userCredentialsVM)
    {
        var user = await _userRepository.AuthenticateAsync(
            userCredentialsVM.CompanyId,
            userCredentialsVM.Password
        );

        if (user == null)
            throw new InvalidOperationException("❌ Invalid Company ID or Password");

        return _mapper.Map<UserVM>(user);
    }

    // ✅ Admin creates a new question (linked to form)
    public async Task<QuestionVM> RegisterQuestion(RegisterQuestionVM registerQuestionVM, RoleVM roleVM)
    {
        var question = _mapper.Map<Question>(registerQuestionVM);
        question.UserId = roleVM.RoleId; // Use Role.Id, not RoleId from VM
        question.IsRequired = true;

        await _questionRepository.AddAsync(question);
        return _mapper.Map<QuestionVM>(question);
    }

    // ✅ Admin creates a new QuestionForm with optional questions
    public async Task<QuestionFormVM> CreateQuestionForm(CreateQuestionFormVM createQuestionFormVM, RoleVM roleVM)
    {
        var questionForm = _mapper.Map<QuestionForm>(createQuestionFormVM);
        questionForm.CreatedDate = DateTime.UtcNow;
        questionForm.IsActive = true;

        await _questionFormRepository.AddAsync(questionForm);

        // Add questions if provided
        if (createQuestionFormVM.Questions?.Any() == true)
        {
            foreach (var q in createQuestionFormVM.Questions)
            {
                var newQuestion = new Question
                {
                    Text = q.Text,
                    QuestionFormId = questionForm.Id,
                    QuestionTypeId = q.QuestionTypeId,
                    UserId = roleVM.RoleId // Admin who creates
                };

                await _questionRepository.AddAsync(newQuestion);
            }
        }

        return _mapper.Map<QuestionFormVM>(questionForm);
    }

    // ✅ Update existing QuestionForm
    public async Task<QuestionFormVM> UpdateFormAsync(QuestionFormVM formVm, RoleVM roleVM)
    {
        var qForm = _mapper.Map<QuestionForm>(formVm);
        qForm.CreatedDate = DateTime.UtcNow;
        qForm.IsActive = true;

        await _questionFormRepository.Update(qForm);
        return _mapper.Map<QuestionFormVM>(qForm);
    }
}
 public class QuestionFormService : IQuestionFormService
 {
     private readonly IQuestionFormRepository _questionFormRepository;
     private readonly IMapper _mapper;

     public QuestionFormService(IQuestionFormRepository questionFormRepository, IMapper mapper)
     {
         _questionFormRepository = questionFormRepository;
         _mapper = mapper;
     }

     public async Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId)
     {
         var qForm = await _questionFormRepository.GetByIdAsync(formId);
         return _mapper.Map<QuestionFormVM>(qForm);
     }
 }
public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<List<QuestionVM>> GetAllQuestions()
    {
        var questionVMs = new List<QuestionVM>();
        await _questionRepository.GetAllAsync();
        return questionVMs;
    }

    public async Task<QuestionVM> GetQuestionById(int questionId)
    {
        var question = await _questionRepository.GetByIdAsync(questionId);
        return _mapper.Map<QuestionVM>(question);
    }
}

```
## 6) Razor view snippets
**Views/User/Login.cshtml** // login as admin -CRUD for users and questions, as user (companyId and password)- QuestionForm
**Views/User/Admin.cshtml**
_Layout.cshtml (put in Views/Shared/_Layout.cshtml)
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>@ViewData["Title"] - VoiceEmployee</title>

    <!-- Bootstrap 5 CDN -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Optional icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />

    <!-- Site CSS -->
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container">
            <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">VoiceEmployee</a>
            <div class="collapse navbar-collapse">
                <ul class="navbar-nav ms-auto">
                    @if (Context.Session.GetInt32("UserId") != null)
                    {
                        <li class="nav-item">
                            <a class="nav-link">Hello, @Context.Session.GetString("FullName")</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" asp-controller="Account" asp-action="Logout">Logout</a>
                        </li>
                    }
                    else
                    {
                        <li class="nav-item">
                            <a class="nav-link" asp-controller="Account" asp-action="Login">Login</a>
                        </li>
                    }
                </ul>
            </div>
        </div>
    </nav>

    <div class="container my-4">
        @RenderBody()
    </div>

    <footer class="bg-light text-center py-3 mt-auto">
        <div class="container small">© @DateTime.UtcNow.Year - VoiceEmployee</div>
    </footer>

    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <script src="~/js/site.js"></script>
    @RenderSection("Scripts", required: false)
</body>
</html>

**Views/Account/Login.cshtml**  // add from-gpt
```html
@model UserCredentialsVM
@{
    ViewData["Title"] = "Login";
}

<div class="row justify-content-center">
    <div class="col-md-5">
        <div class="card shadow-sm">
            <div class="card-body">
                <h4 class="card-title mb-3">Sign in</h4>

                <form asp-action="Login" method="post" novalidate>
                    <div class="mb-3">
                        <label asp-for="CompanyId" class="form-label">Company ID</label>
                        <input asp-for="CompanyId" class="form-control" />
                        <span asp-validation-for="CompanyId" class="text-danger"></span>
                    </div>

                    <div class="mb-3">
                        <label asp-for="Password" class="form-label">Password</label>
                        <input asp-for="Password" type="password" class="form-control" />
                        <span asp-validation-for="Password" class="text-danger"></span>
                    </div>

                    <button type="submit" class="btn btn-primary w-100">Login</button>
                </form>
            </div>
        </div>
    </div>
</div>
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```
**Views/QForm/Index.cshtml** //view only one QuestionForm in page
```html
@model QuestionFormVM
@{
    ViewData["Title"] = Model.Title;
}

<div class="card">
    <div class="card-header">
        <h4>@Model.Title</h4>
        <p class="mb-0">@Model.Description</p>
    </div>
    <div class="card-body">
        <form asp-action="SubmitAnswers" method="post" id="questionForm">
            <input type="hidden" name="questionFormId" value="@Model.Id" />

            @if (Model.Questions == null || !Model.Questions.Any())
            {
                <div class="alert alert-info">No questions defined for this form.</div>
            }
            else
            {
                <div class="row g-3">
                    @for (int i = 0; i < Model.Questions.Count; i++)
                    {
                        var q = Model.Questions[i];
                        <div class="col-12">
                            <div class="card mb-2">
                                <div class="card-body">
                                    <label class="form-label fw-bold">@q.Text
                                        @if (q.IsRequired) { <span class="text-danger">*</span> }
                                    </label>

                                    <input type="hidden" name="answers[@i].QuestionId" value="@q.Id" />
                                    <input type="hidden" name="answers[@i].QuestionFormId" value="@Model.Id" />

                                    @if (q.QuestionTypeId == 1)  {
                                        <div class="d-flex align-items-center gap-3">
                                            <input class="form-range" type="range" min="1" max="10" step="1" 
                                                   name="answers[@i].ScaleValue" id="range_@q.Id" oninput="updateRangeLabel(@q.Id,this.value)" />
                                            <div><span id="rangeVal_@q.Id">5</span>/10</div>
                                        </div>
                                    } else {
                                        <textarea class="form-control" name="answers[@i].TextValue" rows="3" 
                                                  placeholder="Write your answer here..."></textarea>
                                    }
                                </div>
                            </div>
                        </div>
                    }
                </div>

                <div class="mt-3">
                    <button type="submit" class="btn btn-success">Submit Answers</button>
                </div>
            }
        </form>
    </div>
</div>

@section Scripts {
    <script>
        // initialize range labels
        document.querySelectorAll('input[type="range"]').forEach(function(r){
            var id = r.id;
            var parts = id.split('_');
            var qid = parts.length>1 ? parts[1] : null;
            if(qid){
                var valSpan = document.getElementById('rangeVal_' + qid);
                if(valSpan) valSpan.textContent = Math.round(r.value || 5);
            }
        });

        function updateRangeLabel(qid, val) {
            var el = document.getElementById('rangeVal_' + qid);
            if (el) el.textContent = val;
        }
    </script>
}
```
**Views/Admin/Questions.cshtml** 
```html
@model QuestionFormVM
@{
    ViewData["Title"] = "Questions - " + Model.Title;
}

<div class="d-flex justify-content-between mb-3">
    <h3>Questions for: @Model.Title</h3>
    <a asp-action="CreateQuestion" asp-route-formId="@Model.Id" class="btn btn-primary">New Question</a>
</div>

<table class="table table-striped table-hover">
    <thead class="table-light">
        <tr>
            <th>#</th>
            <th>Question</th>
            <th>Type</th>
            <th>Required</th>
            <th class="text-end">Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var q in Model.Questions)
        {
            <tr>
                <td>@q.Id</td>
                <td>@q.Text</td>
                <td>@(q.QuestionTypeId == 1 ? "Scale (1-10)" : "Text")</td>
                <td>@(q.IsRequired ? "Yes" : "No")</td>
                <td class="text-end">
                    <a asp-action="EditQuestion" asp-route-id="@q.Id" class="btn btn-sm btn-outline-secondary me-1">
                        <i class="fa fa-edit"></i>
                    </a>
                    <form asp-action="DeleteQuestion" asp-route-id="@q.Id" method="post" style="display:inline" onsubmit="return confirm('Delete question?');">
                        <button class="btn btn-sm btn-outline-danger"><i class="fa fa-trash"></i></button>
                    </form>
                </td>
            </tr>
        }
    </tbody>
</table>
```
**Views/Question/Edit.cshtml**

@model Anketa.Application.ViewModels.RegisterQuestionVM
@{
    ViewData["Title"] = "Edit Question";
}

<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" />

<div class="container mt-5">
    <div class="card shadow-lg p-4 rounded-4">
        <h2 class="mb-4 text-center text-primary">Edit Question</h2>

        <form asp-action="Edit" method="post">
            <div class="mb-3">
                <label asp-for="Text" class="form-label fw-bold"></label>
                <input asp-for="Text" class="form-control" placeholder="Enter question text..." />
                <span asp-validation-for="Text" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Category" class="form-label fw-bold"></label>
                <input asp-for="Category" class="form-control" placeholder="Category name..." />
                <span asp-validation-for="Category" class="text-danger"></span>
            </div>

            <div class="d-flex justify-content-between">
                <a asp-action="Index" class="btn btn-outline-secondary">Back</a>
                <button type="submit" class="btn btn-success">Save Changes</button>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    <script>
        document.querySelector('form').addEventListener('submit', function () {
            const btn = document.querySelector('button[type="submit"]');
            btn.disabled = true;
            btn.innerText = "Saving...";
        });
    </script>
}
**Views/Question/Delete.cshtml**

@model Anketa.Application.ViewModels.QuestionVM
@{
    ViewData["Title"] = "Delete Question";
}

<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" />

<div class="container mt-5">
    <div class="card shadow p-4 rounded-4 border-danger">
        <h2 class="text-danger text-center mb-4">Confirm Deletion</h2>

        <div class="alert alert-warning text-center">
            <strong>Are you sure you want to delete this question?</strong>
        </div>

        <dl class="row">
            <dt class="col-sm-3">Question:</dt>
            <dd class="col-sm-9">@Model.Text</dd>

            <dt class="col-sm-3">Category:</dt>
            <dd class="col-sm-9">@Model.Category</dd>
        </dl>

        <form asp-action="Delete" method="post">
            <input type="hidden" asp-for="Id" />
            <div class="d-flex justify-content-between mt-4">
                <a asp-action="Index" class="btn btn-outline-secondary">Cancel</a>
                <button type="submit" class="btn btn-danger">Delete</button>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    <script>
        document.querySelector('button.btn-danger').addEventListener('click', function () {
            return confirm("Are you absolutely sure you want to delete this question?");
        });
    </script>
}
**site.css (wwwroot/css/site.css)**
/* minimal theme tweaks */
body {
    background: #f7f9fb;
    color: #222;
    font-family: "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
}

.card {
    border-radius: 0.6rem;
}

.navbar .navbar-brand {
    font-weight: 700;
    letter-spacing: .3px;
}

/* range input appearance */
input[type="range"] {
    width: 220px;
}

/* small helpers */
.small-muted { color: #6c757d; font-size: .9rem; }
**site.js (wwwroot/js/site.js)**
// small helpers for UI
window.confirmAction = function(message) {
    return confirm(message || 'Are you sure?');
};

// optional: simple client-side validation helper
window.simpleValidateRequired = function(selector) {
    var el = document.querySelector(selector);
    if (!el) return true;
    if (!el.value || el.value.trim() === '') {
        el.classList.add('is-invalid');
        return false;
    }
    el.classList.remove('is-invalid');
    return true;
};
## 7) Controllers 
**AdminController**(CRUD for users and questions)
```csharp
public class AdminController : Controller
{
    private readonly IQuestionService _questionService;
    private readonly IQuestionFormService _formService;
    public AdminController(IQuestionService questionService, IQuestionFormService formService)
    {
        _questionService = questionService;
        _formService = formService;
    }

    public IActionResult Questions(int formId = 1)
    {
        var qf = _formService.GetFormById(formId);
        return View(qf);
    }

    [HttpGet]
    public IActionResult CreateQuestion(int formId)
    {
        var vm = new RegisterQuestionVM { QuestionFormId = formId };
        return View(vm);
    }

    [HttpPost]
    public IActionResult CreateQuestion(RegisterQuestionVM model)
    {
        if (!ModelState.IsValid) return View(model);
        _questionService.CreateQuestion(model);
        return RedirectToAction("Questions", new { formId = model.QuestionFormId });
    }

    // Edit/Delete similar...
}
```
**UserController** view Form, Get answers, send to Db
```csharp
public class AccountController : Controller
{
    private readonly IUserService _userService;
    public AccountController(IUserService userService) { _userService = userService; }

    [HttpGet]
    public IActionResult Login() => View(new UserCredentialsVM());

    [HttpPost]
    public IActionResult Login(UserCredentialsVM model)
    {
        if(!ModelState.IsValid) return View(model);
        var user = _userService.ValidateUser(model);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid CompanyId or Password");
            return View(model);
        }

        // simple session (or use cookie auth)
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("FullName", user.FullName);
        // RoleKey available if mapped
        return RedirectToAction("Index", "Form");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
```
**QFormController** one page one QForm, btn Next for next QForm
```csharp
public class FormController : Controller
{
    private readonly IQuestionFormService _formService;
    private readonly IAnswerService _answerService;
    private readonly IQuestionRepository _questionRepo;

    public FormController(IQuestionFormService formService, IAnswerService answerService, IQuestionRepository questionRepo)
    {
        _formService = formService;
        _answerService = answerService;
        _questionRepo = questionRepo;
    }

    public IActionResult Index(int id = 1) // form id
    {
        var formVM = _formService.GetFormById(id);
        if (formVM == null) return NotFound();
        return View(formVM); // view will iterate Questions and create inputs
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswers(List<AnswerVM> answers)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        foreach(var a in answers)
        {
            a.UserId = userId.Value;
            // server validation: required questions
            var q = _questionRepo.GetQuestionById(a.QuestionId);
            if (q == null) continue;
            if (q.IsRequired)
            {
                if (q.QuestionTypeId == 1 && a.ScaleValue == null) ModelState.AddModelError("", $"Question {q.Id} is required.");
                if (q.QuestionTypeId == 2 && string.IsNullOrWhiteSpace(a.TextValue)) ModelState.AddModelError("", $"Question {q.Id} is required.");
            }
            if (ModelState.IsValid)
            {
                await _answerService.SubmitAnswerAsync(a);
            }
        }

        if(!ModelState.IsValid) return BadRequest(ModelState);

        return RedirectToAction("Index", new { id = answers.FirstOrDefault()?.QuestionFormId });
    }
}
```
**QuestionController**
```csharp
public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // GET: Question/Edit/5
        public IActionResult Edit(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
            {
                return NotFound();
            }

            var model = new RegisterQuestionVM
            {
                Id = question.Id,
                Text = question.Text,
                Category = question.Category
            };

            return View(model);
        }

        // POST: Question/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RegisterQuestionVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _questionService.UpdateQuestion(model);
            TempData["Success"] = "Question updated successfully!";
            return RedirectToAction("Index", "Question");
        }

        // GET: Question/Delete/5
        public IActionResult Delete(int id)
        {
            var question = _questionService.GetQuestionById(id);
            if (question == null)
                return NotFound();

            return View(question);
        }

        // POST: Question/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _questionService.DeleteQuestionAsync(id);
            TempData["Success"] = "Question deleted successfully!";
            return RedirectToAction("Index", "Question");
        }
    }
}
```
