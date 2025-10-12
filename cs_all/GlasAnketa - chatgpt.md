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
@model UserCredentialsVM

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Employee Satisfaction Survey - Login</title>
    <link rel="stylesheet" href="~/css/site.css" />
    <link rel="stylesheet" href="~/css/login.css" />
</head>
<body>
    <div class="login-container">
        <div class="login-header">
            <h2>👥 Employee Satisfaction Survey</h2>
            <p>Please enter your credentials to continue</p>
        </div>

        @if (!ViewData.ModelState.IsValid)
        {
            <div class="validation-summary">
                <ul>
                    @foreach (var error in ViewData.ModelState.Values.SelectMany(v => v.Errors))
                    {
                        <li>@error.ErrorMessage</li>
                    }
                </ul>
            </div>
        }

        <form asp-action="Login" method="post" id="loginForm" class="login-form">
            <div class="form-group">
                <label asp-for="CompanyId" class="form-label">🏢 Company ID</label>
                <input asp-for="CompanyId" class="form-control" placeholder="Enter your company ID" />
                <span asp-validation-for="CompanyId" class="text-danger"></span>
            </div>

            <div class="form-group">
                <label asp-for="Password" class="form-label">🔒 Password</label>
                <input asp-for="Password" type="password" class="form-control" placeholder="Enter your password" />
                <span asp-validation-for="Password" class="text-danger"></span>
            </div>

            <button type="submit" class="btn-login" id="loginBtn">
                <span id="btnText">Login</span>
                <div id="loadingSpinner" style="display: none;">Loading...</div>
            </button>
        </form>

        <div style="text-align: center; margin-top: 20px;">
            <small style="color: #7f8c8d;">
                Having trouble logging in? Contact your administrator.
            </small>
        </div>
    </div>

    <script>
        document.getElementById('loginForm').addEventListener('submit', function(e) {
            const btn = document.getElementById('loginBtn');
            const btnText = document.getElementById('btnText');
            const spinner = document.getElementById('loadingSpinner');

            btnText.style.display = 'none';
            spinner.style.display = 'block';
            btn.disabled = true;
        });
    </script>
</body>
</html>
``````````
@model FormSubmissionVM
@{
    // Add null check for Model.QuestionForm
    if (Model?.QuestionForm == null)
    {
        <div class="alert alert-danger">
            <h2>Error Loading Form</h2>
            <p>The questionnaire form could not be loaded. Please try again or contact support.</p>
            <a href="@Url.Action("Login", "Account")" class="btn btn-primary">Return to Login</a>
        </div>
        return;
    }

    ViewData["Title"] = Model.QuestionForm.Title;
    var nextFormId = ViewBag.NextFormId as int?;
    var isLastForm = ViewBag.IsLastForm as bool? ?? false;
    var currentFormNumber = ViewBag.CurrentFormNumber as int? ?? 1;
    var totalForms = ViewBag.TotalForms as int? ?? 1;
}

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Employee Satisfaction Survey</title>
    <link rel="stylesheet" href="~/css/site.css" />
    <link rel="stylesheet" href="~/css/questionnaire.css" />
</head>
<body>
    <div class="questionnaire-container">
        <div class="questionnaire-header fade-in">
            <div class="form-progress">
                <div class="progress-info">
                    <h1>@Model.QuestionForm.Title</h1>
                    <p>@Model.QuestionForm.Description</p>
                    <div class="form-counter">
                        Questionnaire @currentFormNumber of @totalForms
                    </div>
                </div>

                <div class="progress-container">
                    <div class="progress-bar">
                        <div class="progress-fill" id="progressFill" style="width: 0%"></div>
                    </div>
                    <small class="text-muted">Progress: <span id="progress-text">0%</span></small>
                </div>
            </div>
        </div>

        <!-- Rest of your form code remains the same -->
        <div class="questionnaire-form slide-in">
            <form asp-action="SubmitForm" method="post" id="questionnaireForm">
                @Html.AntiForgeryToken()
                <input type="hidden" asp-for="QuestionFormId" />

                @for (int i = 0; i < Model.QuestionForm.Questions.Count; i++)
                {
                    var question = Model.QuestionForm.Questions[i];
                    <div class="question-item" data-required="@question.IsRequired.ToString().ToLower()" data-question-type="@question.QuestionType">
                        <h4>@question.Text @(question.IsRequired ? "*" : "")</h4>

                        <input type="hidden" name="Answers[@i].QuestionId" value="@question.Id" />
                        <input type="hidden" name="Answers[@i].QuestionFormId" value="@Model.QuestionForm.Id" />
                        <input type="hidden" name="Answers[@i].UserId" value="@Model.Answers[i].UserId" />

                        @if (question.QuestionType == "Scale")
                        {
                            <div class="scale-answers">
                                @for (int j = 1; j <= 10; j++)
                                {
                                    <label class="scale-option">
                                        <input type="radio"
                                               name="Answers[@i].ScaleValue"
                                               value="@j"
                                               class="scale-radio" />
                                        <span class="scale-number">@j</span>
                                    </label>
                                }
                            </div>
                            <div class="scale-labels">
                                <span>Strongly Disagree (1)</span>
                                <span>Strongly Agree (10)</span>
                            </div>
                        }
                        else if (question.QuestionType == "Text")
                        {
                            <textarea name="Answers[@i].TextValue"
                              class="text-answer form-control"
                              placeholder="Please enter your response here..."></textarea>
                        }
                    </div>
                }

                <div class="navigation-buttons">
                    <button type="submit" class="btn btn-success" id="submitBtn">
                        <span class="btn-text">
                            @(isLastForm ? "Submit All Answers" : "Save & Continue →")
                        </span>
                        <div class="loading-spinner" style="display: none;"></div>
                    </button>
                </div>
            </form>
        </div>
    </div>

    <script>
        // Your existing JavaScript code
        document.addEventListener('DOMContentLoaded', function() {
            console.log("Form loaded successfully");
            // ... rest of your JavaScript
        });
    </script>
</body>
</html>
````````````````
```html
@model List<QuestionFormVM>
@{
    ViewData["Title"] = "Available Questionnaires";
}

<div class="container mt-4">
    <div class="row">
        <div class="col-12">
            <div class="welcome-header text-center mb-4">
                <h1>Welcome, @Context.Session.GetString("UserName")! 👋</h1>
                <p class="lead">Please select a questionnaire to complete</p>
            </div>

            @if (!Model.Any())
            {
                <div class="alert alert-info">
                    <h4>No active questionnaires available</h4>
                    <p>There are currently no active questionnaires. Please check back later or contact your administrator.</p>
                </div>
            }
            else
            {
                <div class="row">
                    @foreach (var form in Model)
                    {
                        <div class="col-md-6 mb-4">
                            <div class="card questionnaire-card h-100">
                                <div class="card-body">
                                    <h5 class="card-title">@form.Title</h5>
                                    <p class="card-text">@form.Description</p>

                                    <div class="questionnaire-meta">
                                        <small class="text-muted">
                                            <i class="fas fa-question-circle"></i>
                                            @form.QuestionCount question@(form.QuestionCount != 1 ? "s" : "")
                                        </small>
                                        <br>
                                        <small class="text-muted">
                                            <i class="fas fa-users"></i>
                                            @form.ResponseCount response@(form.ResponseCount != 1 ? "s" : "")
                                        </small>
                                    </div>
                                </div>
                                <div class="card-footer bg-transparent">
                                    <a href="@Url.Action("Form", new { id = form.Id })"
                                       class="btn btn-primary btn-block">
                                        Start Questionnaire →
                                    </a>
                                </div>
                            </div>
                        </div>
                    }
                </div>
            }

            <div class="text-center mt-4">
                <div class="user-info-card p-3">
                    <small class="text-muted">
                        Logged in as: <strong>@Context.Session.GetString("UserName")</strong> |
                        Role: <strong>@Context.Session.GetString("UserRole")</strong> |
                        <a href="@Url.Action("Logout", "Account")" class="text-danger">Logout</a>
                    </small>
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    .welcome-header {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 2rem;
        border-radius: 10px;
        margin-bottom: 2rem;
    }

    .questionnaire-card {
        border: none;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        transition: transform 0.2s ease-in-out;
    }

        .questionnaire-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 15px rgba(0, 0, 0, 0.2);
        }

    .questionnaire-meta {
        margin: 1rem 0;
    }

    .user-info-card {
        background-color: #f8f9fa;
        border-radius: 5px;
        border-left: 4px solid #007bff;
    }

    .btn-block {
        width: 100%;
    }
</style>
````
## Controllers
```csharp
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IQuestionFormService _formService;

        public AccountController(IUserService userService, IQuestionFormService formService)
        {
            _userService = userService;
            _formService = formService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            // Clear any existing session
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = _userService.ValidateUser(model);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid Company ID or Password");
                    return View(model);
                }

                // Store user in session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role.Name);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetInt32("CompanyId", user.CompanyId);

                // Set last activity time
                HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString());

                // Debug: Check role and redirect
                Console.WriteLine($"User Role: {user.Role.Name}, Redirecting...");

                if (user.Role.Name == "Administrator")
                {
                    Console.WriteLine("Redirecting to Admin Index");
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    Console.WriteLine("Redirecting to FIRST questionnaire form");
                    // Get active forms and redirect to first one
                    var forms = await _formService.GetActiveFormsAsync();
                    if (forms.Any())
                    {
                        return RedirectToAction("Form", "Questionnaire", new { id = forms.First().Id });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No active questionnaires available.";
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
```
public class QuestionnaireController : Controller
{
    private readonly IQuestionFormService _formService;
    private readonly IAnswerService _answerService;

    public QuestionnaireController(IQuestionFormService formService, IAnswerService answerService)
    {
        _formService = formService;
        _answerService = answerService;
    }

    [HttpGet]
    public async Task<IActionResult> Form(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Account");

        try
        {
            // Get the specific form with questions
            var currentForm = await _formService.GetFormWithQuestionsAsync(id);
            if (currentForm == null)
            {
                TempData["ErrorMessage"] = "Questionnaire not found.";
                return RedirectToAction("Login", "Account");
            }

            // Check if user already submitted this form
            var hasSubmitted = await _answerService.HasUserSubmittedFormAsync(userId.Value, id);
            if (hasSubmitted)
            {
                return await RedirectToNextForm(id);
            }

            var viewModel = new FormSubmissionVM
            {
                QuestionForm = currentForm,
                QuestionFormId = currentForm.Id,
                Answers = currentForm.Questions.Select(q => new AnswerVM
                {
                    QuestionId = q.Id,
                    QuestionFormId = id,
                    UserId = userId.Value
                }).ToList()
            };

            // Get next form info for navigation
            var activeForms = await _formService.GetActiveFormsAsync();
            var currentIndex = activeForms.FindIndex(f => f.Id == id);
            var nextFormId = await GetNextFormId(id);

            ViewBag.NextFormId = nextFormId;
            ViewBag.IsLastForm = nextFormId == null;
            ViewBag.CurrentFormNumber = currentIndex + 1;
            ViewBag.TotalForms = activeForms.Count;

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error loading questionnaire. Please try again.";
            return RedirectToAction("Login", "Account");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitForm(FormSubmissionVM model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Account");

        try
        {
            Console.WriteLine($"=== FORM SUBMISSION STARTED ===");
            Console.WriteLine($"User: {userId}, Form: {model.QuestionFormId}");

            if (!ModelState.IsValid)
            {
                await RepopulateFormData(model);
                return View("Form", model);
            }

            if (model.Answers == null || !model.Answers.Any())
            {
                ModelState.AddModelError("", "Please answer at least one question.");
                await RepopulateFormData(model);
                return View("Form", model);
            }

            var answersWithValues = model.Answers.Where(a =>
                (a.ScaleValue.HasValue && a.ScaleValue > 0) ||
                !string.IsNullOrWhiteSpace(a.TextValue)).ToList();

            if (!answersWithValues.Any())
            {
                ModelState.AddModelError("", "Please answer at least one question.");
                await RepopulateFormData(model);
                return View("Form", model);
            }

            foreach (var answer in answersWithValues)
            {
                answer.UserId = userId.Value;
            }

            var result = await _answerService.SubmitAnswersAsync(answersWithValues, userId.Value);

            if (result.Success)
            {
                TempData["SubmissionSuccess"] = true;
                TempData["SubmittedCount"] = result.SubmittedAnswersCount;

                var nextFormId = await GetNextFormId(model.QuestionFormId);

                if (nextFormId.HasValue)
                {
                    return RedirectToAction("Form", new { id = nextFormId.Value });
                }
                else
                {
                    return RedirectToAction("ThankYou");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                await RepopulateFormData(model);
                return View("Form", model);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while submitting your answers. Please try again.");
            await RepopulateFormData(model);
            return View("Form", model);
        }
    }

    public IActionResult ThankYou()
    {
        if (TempData["SubmissionSuccess"] == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var viewModel = new ThankYouVM
        {
            SubmittedCount = TempData["SubmittedCount"] as int? ?? 0,
            SubmissionDate = DateTime.Now
        };

        return View(viewModel);
    }

    private async Task<int?> GetNextFormId(int currentFormId)
    {
        var activeForms = await _formService.GetActiveFormsAsync();
        var currentIndex = activeForms.FindIndex(f => f.Id == currentFormId);

        if (currentIndex < activeForms.Count - 1)
        {
            return activeForms[currentIndex + 1].Id;
        }

        return null;
    }

    private async Task<IActionResult> RedirectToNextForm(int currentFormId)
    {
        var nextFormId = await GetNextFormId(currentFormId);

        if (nextFormId.HasValue)
        {
            return RedirectToAction("Form", new { id = nextFormId.Value });
        }
        else
        {
            return RedirectToAction("ThankYou");
        }
    }

    private async Task RepopulateFormData(FormSubmissionVM model)
    {
        try
        {
            var currentForm = await _formService.GetFormWithQuestionsAsync(model.QuestionFormId);
            var nextFormId = await GetNextFormId(model.QuestionFormId);

            model.QuestionForm = currentForm;
            ViewBag.NextFormId = nextFormId;
            ViewBag.IsLastForm = nextFormId == null;

            var activeForms = await _formService.GetActiveFormsAsync();
            var currentIndex = activeForms.FindIndex(f => f.Id == model.QuestionFormId);
            ViewBag.CurrentFormNumber = currentIndex + 1;
            ViewBag.TotalForms = activeForms.Count;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RepopulateFormData: {ex.Message}");
        }
    }
}