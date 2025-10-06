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
 public class AppDbContext : IdentityDbContext
 {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
     {
     }
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
         optionsBuilder.UseSqlServer("Server=.;Database=VoiceEmployeeDb;Trusted_Connection=True;TrustServerCertificate=True");

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
  }
    public interface IQuestionFormRepository
  {
      int InsertQuestionForm(QuestionForm questionForm);
      void UpdateQuestionForm(QuestionForm questionForm);
      void DeleteQuestionForm(int id);
      QuestionForm GetQuestionFormById(int id);
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
    protected readonly AppDbContext _appDbcontext;
    public BaseRepository(AppDbContext appDbContext)
    {
        _appDbcontext = appDbContext;
    }
}
 public class UserRepository : BaseRepository, IUserRepository
 {
     public UserRepository(AppDbContext appDbContext) : base(appDbContext)
     {
     }
     public User GetByCompanyId(int companyId)
     {
         return _appDbContext.Users.Include(x => x.OU).Include(x => x.OU2).FirstOrDefault(x => x.CompanyId == companyId);

     }

     public User GetByOU(string ou)
     {
         return _appDbContext.Users.Include(x => x.CompanyId).Include(x => x.OU2).FirstOrDefault(x => x.OU == ou);
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
}
public class QuestionFormRepository : BaseRepository, IQuestionFormRepository
{
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
        return _appDbContext.QuestionForms.FirstOrDefault(q => q.Id == id);
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
}
public class AnswerRepository : BaseRepository, IAnswerRepository
{
    public AnswerRepository(DataContext.AppDbContext appDbContext) : base(appDbContext)
    {
    }
    public List<Answer> GetAnswersByQuestionFormId(int questionFormId)
    {
        return _appDbContext.Answers
                            .Where(a => a.QuestionFormId == questionFormId)
                            .ToList();
    }

    public List<Answer> GetAnswersByUserId(int userId)
    {
        return _appDbContext.Answers
                            .Where(a => a.UserId == userId)
                            .ToList();
    }

    public List<Answer> GetAnswersByQuestionId(int questionId)
    {
        return _appDbContext.Answers
                            .Where(a => a.QuestionId == questionId)
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
        return _appDbContext.SaveChangesAsync();
    }
}
```

---

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
   public class QuestionVM
 {
     public string Text { get; set; }
     public int UserId { get; set; }
     public int QuestionFormId { get; set; }

 }
   public class RegisterQuestionVM
 {
     public int QuestionFormId { get; set; }
     public string Text { get; set; }
     public int QuestionTypeId { get; set; }

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
```
## 6) Admin controller (create/edit questions)

```csharp
public class AdminController : Controller
{
    private readonly IQuestionService _questionService;

    public AdminController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    public async Task<IActionResult> Questions()
    {
        var questions = await _questionService.GetAllQuestionsAsync();
        return View(questions); // Map to VM as appropriate
    }

    [HttpGet]
    public IActionResult CreateQuestion() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateQuestion(Question q)
    {
        if (!ModelState.IsValid) return View(q);
        await _questionService.CreateQuestionAsync(q);
        return RedirectToAction("Questions");
    }
}
```

---

## 7) Razor view snippets

**Views/Account/Login.cshtml**

```html
@model LoginViewModel
<form asp-action="Login" method="post">
    <div>
        <label>CompanyId</label>
        <input asp-for="CompanyId" />
    </div>
    <div>
        <label>FullName</label>
        <input asp-for="FullName" />
    </div>
    <div>
        <label>Password</label>
        <input asp-for="Password" type="password" />
    </div>
    <button type="submit">Login</button>
</form>
```

**Views/Form/Take.cshtml**

```html
@model QuestionFormViewModel
<h2>@Model.Title</h2>
<form asp-action="Submit" method="post">
    <input type="hidden" asp-for="Id" name="FormId" />
    <input type="hidden" name="UserId" value="@HttpContext.Session.GetInt32("UserId")" />

    @for (int i = 0; i < Model.Questions.Count; i++) {
        var q = Model.Questions[i];
        <div>
            <p>@q.Text</p>
            @if(q.QuestionType == "Scale") {
                <select name="Answers[@i].ScaleValue" required="@(q.IsRequired)">
                    <option value="">--</option>
                    @for (int s = 1; s <= 10; s++) {
                        <option value="@s">@s</option>
                    }
                </select>
                <input type="hidden" name="Answers[@i].QuestionId" value="@q.Id" />
            }
            else {
                <textarea name="Answers[@i].TextValue" maxlength="500"></textarea>
                <input type="hidden" name="Answers[@i].QuestionId" value="@q.Id" />
            }
        </div>
    }
    <button type="submit">Submit</button>
</form>
```

---

## 8) Notes / Security / Migration

1. **Passwords**: modify seed to hash passwords with `AuthHelper.HashPassword(...)` and optionally create a migration to update existing seeded values.
2. **Sessions / Auth**: for production use ASP.NET Core Identity or cookie authentication. Session-based approach above is minimal for PoC.
3. **Validation**: check required scale answers server-side; reject if missing.
4. **Prevent double submissions**: `IAnswerRepository.HasUserCompletedFormAsync` can be used before creating answers — or allow updates.
5. **Transactions**: `SubmitAnswersAsync` uses a DB transaction — good.
6. **Model binding for lists**: Ensure form field naming matches asp.net model binder (Answers[0].QuestionId etc.).
7. **Seed data**: do not keep admin password in plaintext. Consider creating an admin default with known secure password and require reset.

---

## 9) Next steps / Todo

- Replace seed plaintext passwords with hashed ones and create EF migration.
- Add server-side checks ensuring `QuestionType` is fetched server-side before accepting each answer.
- Add a `SubmittedForms` table if you prefer to mark a form as completed per user (denormalization for reporting).
- Add reporting endpoints (aggregate average per question, counts, free-text exports).

---

## 10) How to apply: Migration commands

```bash
# from project root
dotnet ef migrations add InitAnketa
dotnet ef database update
```

---

*If you want, I can generate real complete controller files, fully typed viewmodels with DataAnnotations, or update the DataSeedExtensions to write hashed passwords. Tell me which one you want next and I'll add it to the repo.*

