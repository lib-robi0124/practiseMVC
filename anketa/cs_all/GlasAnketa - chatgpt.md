# Company needs MVC app as Questionnaire for employee satisfactory on different area of workplace. Idea is , Domain.Models to be:
User , Role , Question, QuestionType, QuestionForm, Answer
User as Administrators to be able to add, modify(update), delete Questions , Users as Employees are Answer the QuestionForm with Question Type of Scale from 1 to 10, or fill with Text. Answer has to be stored in database for future reporting, with UserId, QuestionFormId, QuestionId.
Main page will have login screen with CopmanyId and Password

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


## 1) High-level architecture (flow)

Client Browser (Razor Views / HTML Forms)
↓ (POST/GET)
**Controllers** (AccountController, FormController, AnswerController, AdminController)
↓
**ViewModels** (LoginViewModel, FormViewModel, QuestionViewModel, AnswerViewModel)
↓
**Services** (IUserService, IQuestionService, IAnswerService) — business validation and orchestration
↓
**Repositories** (IUserRepository, IQuestionRepository, IAnswerRepository) — repository pattern using AppDbContext
↓
**EF Core DbContext (AppDbContext)** → SQL Server

Notes:
- Controllers map incoming ViewModels → call Services → Services call Repositories → Repositories use EF.
- Services should contain business rules (prevent duplicate submissions, check required answers, map types).
- Always validate ViewModels server-side and use Data Annotations.

---

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
  }
   public class Role
 {
     public int Id { get; set; }
     public string Name { get; set; }
     public ICollection<User> Users { get; set; }
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
   }
     public class QuestionType
  {
      public int Id { get; set; }
      public string Name { get; set; } // "Scale", "Text"
      public string Description { get; set; }
      public ICollection<Question> Questions { get; set; }
  }
    public class Answer
  {
      public int Id { get; set; }
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
public class AppDbContext : DbContext // use DbContext unless you use Identity types
 {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)     {     }
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
        if (!optionsBuilder.IsConfigured)
        {
         optionsBuilder.UseSqlServer("Server=.;Database=VoiceEmployeeDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
     }
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         modelBuilder.SeedData();

         // Configure relationships and constraints if needed
         modelBuilder.Entity<Answer>()
             .HasOne(a => a.User)
             .WithMany(u => u.Answers)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.Restrict);

         modelBuilder.Entity<Answer>()
             .HasOne(a => a.Question)
             .WithMany(q => q.Answers)
             .HasForeignKey(a => a.QuestionId)
             .OnDelete(DeleteBehavior.Restrict);

         modelBuilder.Entity<Answer>()
             .HasOne(a => a.QuestionForm)
             .WithMany(qf => qf.Answers)
             .HasForeignKey(a => a.QuestionFormId)
             .OnDelete(DeleteBehavior.Restrict);

         // Additional configurations can be added here Questions, QuestionTypes, QuestionForms, Users
         modelBuilder.Entity<Question>()
             .HasOne(q => q.QuestionForm)
             .WithMany(f => f.Questions)
             .HasForeignKey(q => q.QuestionFormId)
             .OnDelete(DeleteBehavior.Restrict); 

         modelBuilder.Entity<Question>()
             .HasOne(q => q.User)
             .WithMany()
             .HasForeignKey(q => q.UserId)
             .OnDelete(DeleteBehavior.Restrict);

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
     }
     public DbSet<User> Users { get; set; }
     public DbSet<Role> Roles { get; set; }
     public DbSet<Question> Questions { get; set; }
     public DbSet<QuestionType> QuestionTypes { get; set; }
     public DbSet<QuestionForm> QuestionForms { get; set; }
     public DbSet<Answer> Answers { get; set; }
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
            new QuestionForm { Id = 12, Title = "Отворени прашања", Description = "Open Questions" }
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
}     }          
 public interface IUserRepository
{
    User GetByCompanyId(int companyId);
    User GetByOU(string ou);
    int Insert(User user);
}
  public interface IQuestionRepository
  {
      int InsertQuestion(Question question);
      void UpdateQuestion(Question question);
      void DeleteQuestion(int id);
      Question GetQuestionById(int id);
      List<Question> GetAllQuestions(); // NEW
  }
    public interface IQuestionFormRepository
  {
      int InsertQuestionForm(QuestionForm questionForm);
      void UpdateQuestionForm(QuestionForm questionForm);
      void DeleteQuestionForm(int id);
      QuestionForm GetQuestionFormById(int id);
      List<QuestionForm> GetAllFormQuestions();
}
    public interface IAnswerRepository
  {
      List<Answer> GetAnswersByQuestionFormId(int questionFormId);
      List<Answer> GetAnswersByUserId(int userId);
      List<Answer> GetAnswersByQuestionId(int questionId);
      Answer GetAllAnswers(int userId, int questionId, int questionFormId);
      Task<int> InsertAnswerAsync(Answer answer);
  }
  public abstract class BaseRepository
{
    protected readonly AppDbContext _appDbContext;
    public BaseRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
}
 public class UserRepository : BaseRepository, IUserRepository
 {
     public UserRepository(AppDbContext appDbContext) : base(appDbContext)     {     }
     public User GetByCompanyId(int companyId)
     {
         return _appDbContext.Users.Include(u => u.Role).FirstOrDefault(x => x.CompanyId == companyId); //changed

     }
     public User GetByOU(string ou)
     {
         return _appDbContext.Users.Include(u => u.Role).FirstOrDefault(x => x.OU == ou); // changed
     }
     public int Insert(User user)
     {
         _appDbContext.Users.Add(user); //Add method marks entity as Added in the context
         _appDbContext.SaveChanges(); //SaveChanges commits changes to the database
         return user.Id; //after SaveChanges, EF Core populates the Id property with the generated value
     }
 }
 public class QuestionRepository : BaseRepository, IQuestionRepository
{
    public QuestionRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
    public void DeleteQuestion(int id)
    {
        var question = _appDbContext.Questions.FirstOrDefault(c => c.Id == id);
        if (question is null)
        {
            throw new Exception($"Question with id {id} not found.");
        }
        _appDbContext.Questions.Remove(question);
        _appDbContext.SaveChanges();
    }
    public Question GetQuestionById(int id)
    {
        return _appDbContext.Questions.Include(q => q.User)
                                      .Include(q => q.QuestionForm)
                                      .Include(q => q.QuestionType) // changed
                                      .FirstOrDefault(q => q.Id == id);
    }
    public int InsertQuestion(Question question)
    {
        _appDbContext.Questions.Add(question);
        _appDbContext.SaveChanges();
        return question.Id;
    }
    public void UpdateQuestion(Question question)
    {
        if (!_appDbContext.Questions
             .Any(x => x.Id == question.Id))
        {
            throw new Exception($"Question with id {question.Id} was not found");
        }
        _appDbContext.Questions.Update(question);
        _appDbContext.SaveChanges();
    }
     public List<Question> GetAllQuestions() // add changed
    {
        return _appDbContext.Questions
            .Include(q => q.QuestionType)
            .Include(q => q.QuestionForm)
            .Include(q => q.User)
            .OrderBy(q => q.QuestionFormId)
            .ThenBy(q => q.Id)
            .ToList();
    }
}
public class QuestionFormRepository : BaseRepository, IQuestionFormRepository
{   // QuestionFormRepository.cs (minor rename of GetAllQuestions -> GetAllQuestionForms)
    public QuestionFormRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public void DeleteQuestionForm(int id)
    {
        var questionForm = _appDbContext.QuestionForms.FirstOrDefault(c => c.Id == id);
        if (questionForm is null)
        {
            throw new Exception($"QuestionForm with id {id} not found.");
        }
        _appDbContext.QuestionForms.Remove(questionForm);
        _appDbContext.SaveChanges();
    }

    public QuestionForm GetQuestionFormById(int id)
    {
        return _appDbContext.QuestionForms.Include(qf => qf.Questions).FirstOrDefault(q => q.Id == id); //changed
    }

    public int InsertQuestionForm(QuestionForm questionForm)
    {
        _appDbContext.QuestionForms.Add(questionForm);
        _appDbContext.SaveChanges();
        return questionForm.Id;
    }

    public void UpdateQuestionForm(QuestionForm questionForm)
    {
        if (!_appDbContext.QuestionForms
             .Any(x => x.Id == questionForm.Id))
        {
            throw new Exception($"QuestionForm with id {questionForm.Id} was not found");
        }
        _appDbContext.QuestionForms.Update(questionForm);
        _appDbContext.SaveChanges();

    }
     public List<QuestionForm> GetAllFormQuestions() => _appDbContext.QuestionForms.Include(qf => qf.Questions).ToList(); // changed
}
public class AnswerRepository : BaseRepository, IAnswerRepository
{       // AnswerRepository.cs - FIXES: constructor type + async Save
    public AnswerRepository(DataContext.AppDbContext appDbContext) : base(appDbContext)    {    }
    public List<Answer> GetAnswersByQuestionFormId(int questionFormId)
    {
        return _appDbContext.Answers
                            .Where(a => a.QuestionFormId == questionFormId)
                            .Include(a => a.User)           // changed
                            .Include(a => a.Question)       // changed
                            .ToList();
    }
    public List<Answer> GetAnswersByUserId(int userId)
    {
        return _appDbContext.Answers
                            .Where(a => a.UserId == userId)
                            .Include(a => a.Question) // changed
                            .ToList();
    }
    public List<Answer> GetAnswersByQuestionId(int questionId)
    {
        return _appDbContext.Answers
                            .Where(a => a.QuestionId == questionId)
                            .Include(a => a.User)       // changed
                            .ToList();
    }
    public Answer GetAllAnswers(int userId, int questionId, int questionFormId)
    {
        return _appDbContext.Answers
                            .FirstOrDefault(a => a.UserId == userId && a.QuestionId == questionId && a.QuestionFormId == questionFormId);
    }
    public Task<int> InsertAnswerAsync(Answer answer)
    {
        //Answer provide from user in question form like scale value or text value, has to be submitted to database
        _appDbContext.Answers.Add(answer);
        return await _appDbContext.SaveChangesAsync(); // changed
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
         public string Password { get; set; }
         public string RoleKey { get; set; }
         public RoleVM Role { get; set; }
 }
  public class RoleVM
 {
     public string Key { get; set; }
     public string Name { get; set; }
 }
  public class UserCredentialsVM
 {
     public int CompanyId { get; set; }
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
   public class QuestionVM // changed
 {
     public int Id { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public int QuestionFormId { get; set; }
    public int QuestionTypeId { get; set; }
    public bool IsRequired { get; set; }

 }
   public class RegisterQuestionVM // changed
 {
    public int? Id { get; set; }                // for edits
    public int QuestionFormId { get; set; }
    public string Text { get; set; }
    public int QuestionTypeId { get; set; }
    public bool IsRequired { get; set; } = true;

 }
   public class QuestionFormVM
  {
      public string Title { get; set; }
      public string Description { get; set; }
      public bool IsActive { get; set; } = true;
      public List<QuestionVM>? Questions { get; set; }
      public List<AnswerVM>? Answers { get; set; }
  }
   public class AnswerVM
 {
     public int UserId { get; set; }
     public int QuestionId { get; set; }
     public int QuestionFormId { get; set; }
     public int? ScaleValue { get; set; } // For scale-based questions, nulable if not applicable
     public string? TextValue { get; set; } // For text-based questions
     public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
 }
```
## 5) Services /interfaces - implementations - automappers - extensions - helpers..

```csharp
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
  public class UserMappingProfile : Profile
 {
     public UserMappingProfile()
     {
         CreateMap<User, UserVM>().ReverseMap();
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
        CreateMap<Question, RegisterQuestionVM>().ReverseMap();
    }
}
public class QuestionFormMappingProfile : Profile
{
    public QuestionFormMappingProfile() { CreateMap<QuestionForm, QuestionFormVM>().ReverseMap(); }
}
public class AnswerMappingProfile : Profile
{
    public AnswerMappingProfile() { CreateMap<Answer, AnswerVM>().ReverseMap(); }
}
 public interface IUserService
 {
     UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM = null); //changed
     UserVM GetUserById(int id);
     UserVM ValidateUser(UserCredentialsVM userCredentialsViewModel);
 }
  public interface IQuestionService // edit - change
 {
      List<QuestionVM> GetAllQuestions();
    QuestionVM GetQuestionById(int questionId);

    // Admin methods
    int CreateQuestion(RegisterQuestionVM registerQuestionVM);
    int DeleteQuestion(int questionId);
    void UpdateQuestion(RegisterQuestionVM registerQuestionVM);
 }
 public interface IQuestionFormService //provide me with code
{
    List<QuestionFormVM> GetAllForms();
    QuestionFormVM GetFormById(int id);
}
public interface IAnswerService // provide me with code
{
    Task<int> SubmitAnswerAsync(AnswerVM answerVM);
    bool HasUserAnswered(int userId, int questionId, int questionFormId);
    List<AnswerVM> GetAnswersByFormId(int formId);
}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public UserVM GetUserById(int id)
    {
        var user = _userRepository.GetByCompanyId(id);
        return _mapper.Map<UserVM>(user);
    }

    public UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM)
    {
        var user = _mapper.Map<User>(registerUserViewModel);
        PasswordHasherHelper.HashPassword(user, registerUserViewModel.Password);
        user.RoleId = roleVM.RoleId; // FIX: Assign RoleId property directly from RoleVM
        var userId = _userRepository.Insert(user);
        if (userId <= 0)
        {
            throw new Exception("User not saved.");
        }
        return GetUserById(userId);
    }
    public UserVM ValidateUser(UserCredentialsVM userCredentialsViewModel)
    {
        var user = _userRepository.GetByCompanyId(userCredentialsViewModel.CompanyId);
        if (user == null)
        {
            return null;
        }
        var result = PasswordHasherHelper.VerifyHashedPassword(user, userCredentialsViewModel.Password);
        if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
        {
            return new UserVM();
        }
        return _mapper.Map<UserVM>(user);
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
    public int CreateQuestion(RegisterQuestionVM registerQuestionVM) // edit -create
    {
        if (registerQuestionVM == null) throw new ArgumentNullException(nameof(registerQuestionVM));
        if (string.IsNullOrWhiteSpace(registerQuestionVM.Text)) throw new ArgumentException("Text is required.");
        if (registerQuestionVM.QuestionFormId <= 0) throw new ArgumentException("QuestionFormId is required.");
        if (registerQuestionVM.QuestionTypeId <= 0) throw new ArgumentException("QuestionTypeId is required.");

        var entity = _mapper.Map<Question>(registerQuestionVM);

        // ensure default values (if any)
        // entity.UserId = ... (optional: set admin user)
        var newId = _questionRepository.InsertQuestion(entity);
        return newId;
    }
    public int DeleteQuestionAsync(int questionId)
    {
        if (questionId <= 0) throw new ArgumentException("Invalid question id.");

        var existing = _questionRepository.GetQuestionById(questionId);
        if (existing == null) throw new KeyNotFoundException($"Question with id {questionId} not found.");

        _questionRepository.DeleteQuestion(questionId);
        return questionId;
    }
     public List<QuestionVM> GetAllQuestions()
    {
        var list = _questionRepository.GetAllQuestions();
        return _mapper.Map<List<QuestionVM>>(list);
    }
    public QuestionVM GetQuestionById(int questionId)
    {
       if (questionId <= 0) return null;
        var q = _questionRepository.GetQuestionById(questionId);
        return q == null ? null : _mapper.Map<QuestionVM>(q);
    }
    public void UpdateQuestion(RegisterQuestionVM registerQuestionVM)
    {
         if (registerQuestionVM == null) throw new ArgumentNullException(nameof(registerQuestionVM));
        if (!registerQuestionVM.Id.HasValue || registerQuestionVM.Id.Value <= 0)
            throw new ArgumentException("Id is required for update.");

        var existing = _questionRepository.GetQuestionById(registerQuestionVM.Id.Value);
        if (existing == null) throw new KeyNotFoundException($"Question with id {registerQuestionVM.Id.Value} not found.");

        // map allowed fields (prevent overwriting navigation props unexpectedly)
        existing.Text = registerQuestionVM.Text?.Trim();
        existing.QuestionFormId = registerQuestionVM.QuestionFormId;
        existing.QuestionTypeId = registerQuestionVM.QuestionTypeId;
        existing.IsRequired = registerQuestionVM.IsRequired;

        _questionRepository.UpdateQuestion(existing);
    }
}
public class QuestionFormService : IQuestionFormService
{
    private readonly IQuestionFormRepository _repo;
    private readonly IMapper _mapper;
    public QuestionFormService(IQuestionFormRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public List<QuestionFormVM> GetAllForms() => _repo.GetAllQuestions().Select(qf => _mapper.Map<QuestionFormVM>(qf)).ToList();
    public QuestionFormVM GetFormById(int id) => _mapper.Map<QuestionFormVM>(_repo.GetQuestionFormById(id));
}
public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _repo;
    private readonly IMapper _mapper;
    public AnswerService(IAnswerRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<int> SubmitAnswerAsync(AnswerVM answerVM)
    {
        // Validation: required fields, check duplicate
        if (answerVM == null) throw new ArgumentNullException(nameof(answerVM));
        if (answerVM.QuestionId <= 0 || answerVM.UserId <= 0) throw new ArgumentException("Missing keys.");

        var existing = _repo.GetAllAnswers(answerVM.UserId, answerVM.QuestionId, answerVM.QuestionFormId);
        if (existing != null) // prevent duplicate response (business decision: update vs ignore)
            throw new InvalidOperationException("Answer already submitted.");

        var answer = _mapper.Map<Answer>(answerVM);
        return await _repo.InsertAnswerAsync(answer);
    }

    public bool HasUserAnswered(int userId, int questionId, int questionFormId) =>
        _repo.GetAllAnswers(userId, questionId, questionFormId) != null;

    public List<AnswerVM> GetAnswersByFormId(int formId) =>
        _repo.GetAnswersByQuestionFormId(formId).Select(a => _mapper.Map<AnswerVM>(a)).ToList();
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
