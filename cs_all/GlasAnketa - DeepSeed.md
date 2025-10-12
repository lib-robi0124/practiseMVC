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
      public User()
      {
          Answers = new HashSet<Answer>();
      }
  }
 public class Role
 {
     public int Id { get; set; }
     public string Name { get; set; }
     public virtual ICollection<User> Users { get; set; }
     public Role()     {    Users = new HashSet<User>();  }
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
      public Question()    {   Answers = new HashSet<Answer>(); }
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
   public class QuestionType
 {
     public int Id { get; set; }
     public string Name { get; set; } // "Scale", "Text"
     public string Description { get; set; }
     public ICollection<Question> Questions { get; set; }
     public QuestionType()  {  Questions = new HashSet<Question>();    }
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
  public class AnswerSubmissionResult
 {
     public bool Success { get; set; }
     public List<string> Errors { get; set; } = new List<string>();
     public int SubmittedAnswersCount { get; set; }
     public DateTime SubmissionDate { get; set; }
 }
  public class AnswerValidationResult
 {
     public bool IsValid { get; set; }
     public List<string> Errors { get; set; } = new List<string>();
 }
  public class AnswerSummary
 {
     public int QuestionId { get; set; }
     public string QuestionText { get; set; }
     public string QuestionType { get; set; }
     public int TotalResponses { get; set; }
     public double? AverageScaleValue { get; set; }
     public Dictionary<int, int> ScaleValueDistribution { get; set; } = new Dictionary<int, int>();
     public List<string> TextResponses { get; set; } = new List<string>();
 }
---
## 3) DataAccess.Repository DataSeed, AppDbContext

```csharp
  public class AppDbContext : DbContext //no need Identity
 {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)   {  }
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
 public static class DataSeedExtensions
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
      
        // Seed Question Types
        modelBuilder.Entity<QuestionType>().HasData(
            new QuestionType { Id = 1, Name = "Scale", Description = "1-10 Scale" },
            new QuestionType { Id = 2, Name = "Text", Description = "Text Answer" } );
        // Seed Question Forms with OU
        modelBuilder.Entity<QuestionForm>().HasData(
            new QuestionForm { Id = 1, Title = "Општо задоволство", Description = "Overall Satisfaction" },
            new QuestionForm { Id = 2, Title = "Обврска кон компанијата", Description = "Commitment to the Company" });
          // Seed Questions (UserId = 1 for Admin user, No OU)
        modelBuilder.Entity<Question>().HasData(
            // Form 1: Општо задоволство (Scale questions)
            new Question { Id = 1, Text = "Задоволен сум од мојата моментална работа", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
            new Question { Id = 2, Text = "Чувствувам дека мојата работа е ценета во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
            new Question { Id = 3, Text = "Се чувствувам мотивиран да одам на работа секој ден", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },

            // Form 2: Обврска кон компанијата (Scale questions)
            new Question { Id = 4, Text = "Се чувствувам горд што работам за оваа компанија", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },
            new Question { Id = 5, Text = "Со задоволство ја препорачувам оваа компанија како работно место на пријателите и семејството", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },
            new Question { Id = 6, Text = "Се гледам себеси како долгорочно работам во оваа компанија", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 });
        // Seed Users with OU
        modelBuilder.Entity<User>().HasData( 
            new User { Id = 186, CompanyId = 20621, FullName = "Todorka Ristovska", OU = "CEO office", OU2 = "CEO office", Password = "20621", RoleId = 2 },
            new User { Id = 285, CompanyId = 20975, FullName = "Robert Ristovski", OU = "Projects and IT", OU2 = "Maintenance Progress", Password = "20975", RoleId = 1 });
        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Administrator" },
            new Role { Id = 2, Name = "Employee" }        );    
}}
 public interface IUserRepository
{
        User GetByCompanyId(int companyId);
        User GetById(int id); // Added this method
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
      // Existing synchronous methods
    int InsertQuestionForm(QuestionForm questionForm);
    void UpdateQuestionForm(QuestionForm questionForm);
    void DeleteQuestionForm(int id);
    QuestionForm GetQuestionFormById(int id);
    List<QuestionForm> GetAllFormQuestions();

    // New methods needed by service
    List<QuestionForm> GetActiveForms();
    QuestionForm GetFormWithQuestions(int formId);
    Task<int> InsertFormAsync(QuestionForm questionForm);
    Task<bool> UpdateFormAsync(QuestionForm questionForm);
    Task<bool> ToggleFormStatusAsync(int formId, bool isActive);
}
    public interface IAnswerRepository
  {
            // Single answer operations
        Task<int> InsertAnswerAsync(Answer answer);
        Task<int> InsertAnswersAsync(List<Answer> answers);
        Task<bool> UpdateAnswerAsync(Answer answer);
        Task<bool> DeleteAnswerAsync(int answerId);
         // Get operations
        Task<Answer> GetAnswerByIdAsync(int answerId);
        Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId);
        Task<List<Answer>> GetAnswersByUserIdAsync(int userId);
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId);
        // Check operations
        Task<bool> HasUserSubmittedFormAsync(int userId, int questionFormId);
        Task<bool> HasUserAnsweredQuestionAsync(int userId, int questionId);
        // Analytics and summaries
        Task<Dictionary<int, AnswerSummary>> GetAnswerSummariesAsync(int formId);
        Task<List<Answer>> GetFormAnswersWithDetailsAsync(int formId);
        Task<int> GetFormResponseCountAsync(int formId);
        // Bulk operations
        Task<int> GetUserAnswerCountAsync(int userId);
        Task<List<Answer>> GetRecentAnswersAsync(int count = 50);
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
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)   {  }
    public User GetByCompanyId(int companyId)
    {
        return _appDbContext.Users
            .Include(u => u.Role) // Make sure Role is included for mapping
            .AsNoTracking() // Optional: better performance for read operations
            .FirstOrDefault(x => x.CompanyId == companyId);
    }
    // Add this missing method
    public User GetById(int id)
    {
        return _appDbContext.Users
            .Include(u => u.Role)
            .FirstOrDefault(x => x.Id == id);
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
    public QuestionRepository(AppDbContext appDbContext) : base(appDbContext)   {    }
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
    public QuestionFormRepository(AppDbContext appDbContext) : base(appDbContext) { }

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

    public List<QuestionForm> GetAllFormQuestions()
    {
        return _appDbContext.QuestionForms
            .Include(qf => qf.Questions)
                .ThenInclude(q => q.QuestionType)
            .Include(qf => qf.Questions)
                .ThenInclude(q => q.User)
            .ToList();
    }

    public QuestionForm GetQuestionFormById(int id)
    {
        return _appDbContext.QuestionForms
            .FirstOrDefault(q => q.Id == id);
    }

    public int InsertQuestionForm(QuestionForm questionForm)
    {
        _appDbContext.QuestionForms.Add(questionForm);
        _appDbContext.SaveChanges();
        return questionForm.Id;
    }

    public void UpdateQuestionForm(QuestionForm questionForm)
    {
        if (!_appDbContext.QuestionForms.Any(x => x.Id == questionForm.Id))
        {
            throw new Exception($"QuestionForm with id {questionForm.Id} was not found");
        }
        _appDbContext.QuestionForms.Update(questionForm);
        _appDbContext.SaveChanges();
    }

    // New implementations for service methods
    public List<QuestionForm> GetActiveForms()
    {
        return _appDbContext.QuestionForms
            .Where(qf => qf.IsActive)
            .Include(qf => qf.Questions)
            .OrderBy(qf => qf.Id)
            .ToList();
    }

    public QuestionForm GetFormWithQuestions(int formId)
    {
        return _appDbContext.QuestionForms
            .Include(qf => qf.Questions)
                .ThenInclude(q => q.QuestionType)
            .Include(qf => qf.Questions)
                .ThenInclude(q => q.User)
            .FirstOrDefault(qf => qf.Id == formId);
    }

    public async Task<int> InsertFormAsync(QuestionForm questionForm)
    {
        _appDbContext.QuestionForms.Add(questionForm);
        await _appDbContext.SaveChangesAsync();
        return questionForm.Id;
    }

    public async Task<bool> UpdateFormAsync(QuestionForm questionForm)
    {
        var existingForm = await _appDbContext.QuestionForms
            .FirstOrDefaultAsync(qf => qf.Id == questionForm.Id);

        if (existingForm == null)
            return false;

        _appDbContext.Entry(existingForm).CurrentValues.SetValues(questionForm);
        await _appDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleFormStatusAsync(int formId, bool isActive)
    {
        var form = await _appDbContext.QuestionForms
            .FirstOrDefaultAsync(qf => qf.Id == formId);

        if (form == null)
            return false;

        form.IsActive = isActive;
        await _appDbContext.SaveChangesAsync();
        return true;
    }
}
public class AnswerRepository : BaseRepository, IAnswerRepository
{
    public AnswerRepository(AppDbContext appDbContext) : base(appDbContext)  {    }
    // Single answer operations
    public async Task<int> InsertAnswerAsync(Answer answer)
    {
        _appDbContext.Answers.Add(answer);
        await _appDbContext.SaveChangesAsync();
        return answer.Id;
    }
    public async Task<int> InsertAnswersAsync(List<Answer> answers)
    {
        await _appDbContext.Answers.AddRangeAsync(answers);
        return await _appDbContext.SaveChangesAsync();
    }
    public async Task<bool> UpdateAnswerAsync(Answer answer)
    {
        var existingAnswer = await _appDbContext.Answers
            .FirstOrDefaultAsync(a => a.Id == answer.Id);

        if (existingAnswer == null)
            return false;

        _appDbContext.Entry(existingAnswer).CurrentValues.SetValues(answer);
        await _appDbContext.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAnswerAsync(int answerId)
    {
        var answer = await _appDbContext.Answers
            .FirstOrDefaultAsync(a => a.Id == answerId);

        if (answer == null)
            return false;

        _appDbContext.Answers.Remove(answer);
        await _appDbContext.SaveChangesAsync();
        return true;
    }
    // Get operations with includes for related data
    public async Task<Answer> GetAnswerByIdAsync(int answerId)
    {
        return await _appDbContext.Answers
            .Include(a => a.User)
            .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
            .Include(a => a.QuestionForm)
            .FirstOrDefaultAsync(a => a.Id == answerId);
    }
    public async Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId)
    {
        return await _appDbContext.Answers
            .Where(a => a.QuestionFormId == questionFormId)
            .Include(a => a.User)
            .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
            .OrderByDescending(a => a.AnsweredDate)
            .ToListAsync();
    }
    public async Task<List<Answer>> GetAnswersByUserIdAsync(int userId)
    {
        return await _appDbContext.Answers
            .Where(a => a.UserId == userId)
            .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
            .Include(a => a.QuestionForm)
            .OrderByDescending(a => a.AnsweredDate)
            .ToListAsync();
    }
    public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
    {
        return await _appDbContext.Answers
            .Where(a => a.QuestionId == questionId)
            .Include(a => a.User)
            .Include(a => a.QuestionForm)
            .OrderByDescending(a => a.AnsweredDate)
            .ToListAsync();
    }
    public async Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId)
    {
        return await _appDbContext.Answers
            .FirstOrDefaultAsync(a => a.UserId == userId &&
                                    a.QuestionId == questionId &&
                                    a.QuestionFormId == questionFormId);
    }
    // Check operations
    public async Task<bool> HasUserSubmittedFormAsync(int userId, int questionFormId)
    {
        return await _appDbContext.Answers
            .AnyAsync(a => a.UserId == userId && a.QuestionFormId == questionFormId);
    }
    public async Task<bool> HasUserAnsweredQuestionAsync(int userId, int questionId)
    {
        return await _appDbContext.Answers
            .AnyAsync(a => a.UserId == userId && a.QuestionId == questionId);
    }
    // Analytics and summaries
    public async Task<Dictionary<int, AnswerSummary>> GetAnswerSummariesAsync(int formId)
    {
        var summaries = new Dictionary<int, AnswerSummary>();
        // Get all questions for this form
        var questions = await _appDbContext.Questions
            .Where(q => q.QuestionFormId == formId)
            .Include(q => q.QuestionType)
            .ToListAsync();
        foreach (var question in questions)
        {
            var answers = await _appDbContext.Answers
                .Where(a => a.QuestionId == question.Id && a.QuestionFormId == formId)
                .ToListAsync();
            var summary = new AnswerSummary
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                QuestionType = question.QuestionType.Name,
                TotalResponses = answers.Count,
                TextResponses = answers.Where(a => !string.IsNullOrEmpty(a.TextValue))
                                    .Select(a => a.TextValue)
                                    .ToList()
            };
            // Calculate average for scale questions
            if (question.QuestionType.Name == "Scale" && answers.Any())
            {
                var scaleAnswers = answers.Where(a => a.ScaleValue.HasValue)
                                        .Select(a => a.ScaleValue.Value)
                                        .ToList();
                summary.AverageScaleValue = scaleAnswers.Average();
                summary.ScaleValueDistribution = scaleAnswers
                    .GroupBy(v => v)
                    .ToDictionary(g => g.Key, g => g.Count());
            }

            summaries.Add(question.Id, summary);
        }

        return summaries;
    }
    public async Task<List<Answer>> GetFormAnswersWithDetailsAsync(int formId)
    {
        return await _appDbContext.Answers
            .Where(a => a.QuestionFormId == formId)
            .Include(a => a.User)
            .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
            .OrderBy(a => a.User.OU)
            .ThenBy(a => a.User.FullName)
            .ThenBy(a => a.QuestionId)
            .ToListAsync();
    }
    public async Task<int> GetFormResponseCountAsync(int formId)
    {
        return await _appDbContext.Answers
            .Where(a => a.QuestionFormId == formId)
            .Select(a => a.UserId)
            .Distinct()
            .CountAsync();
    }
    // Bulk operations
    public async Task<int> GetUserAnswerCountAsync(int userId)
    {
        return await _appDbContext.Answers
            .Where(a => a.UserId == userId)
            .CountAsync();
    }
    public async Task<List<Answer>> GetRecentAnswersAsync(int count = 50)
    {
        return await _appDbContext.Answers
            .Include(a => a.User)
            .Include(a => a.Question)
            .Include(a => a.QuestionForm)
            .OrderByDescending(a => a.AnsweredDate)
            .Take(count)
            .ToListAsync();
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
 public class UserFormAnswersVM
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; }
    public string QuestionType { get; set; }
    public int? ScaleValue { get; set; }
    public string TextValue { get; set; }
    public DateTime AnsweredDate { get; set; }
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
 public class RoleVM
{
    public int RoleId { get; set; }
    public string Name { get; set; }
}
 public class ThankYouVM
{
    public int SubmittedCount { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string Message => SubmittedCount > 0
        ? $"Thank you for submitting {SubmittedCount} answers!"
        : "Thank you for your participation!";
}
 public class ResultsVM
 {
     public List<AnswerVM> Answers { get; set; }
     public Dictionary<int, AnswerSummaryVM> Summaries { get; set; }
 }
 public class QuestionVM
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public int QuestionFormId { get; set; }
    public bool IsRequired { get; set; }
    public string QuestionType { get; set; }
}
public class RegisterQuestionVM
{
    public int QuestionFormId { get; set; }
    public string Text { get; set; }
    public int QuestionTypeId { get; set; }
}
public class QuestionTypeVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
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
  public class FormSummaryVM
 {
     public int Id { get; set; }
     public string Title { get; set; }
     public string Description { get; set; }
     public bool IsActive { get; set; }
     public DateTime CreatedDate { get; set; }
     public int QuestionCount { get; set; }
     public int ResponseCount { get; set; }
     public double CompletionRate { get; set; }
 }
 public class FormSubmissionVM
{
    public QuestionFormVM QuestionForm { get; set; }
    public List<AnswerVM> Answers { get; set; }
}
 public class FormAnalyticsVM
 {
     public int FormId { get; set; }
     public int TotalResponses { get; set; }
     public Dictionary<int, AnswerSummaryVM> AnswerSummaries { get; set; } = new Dictionary<int, AnswerSummaryVM>();
     public List<AnswerVM> RecentSubmissions { get; set; } = new List<AnswerVM>();
     public DateTime AnalyticsDate { get; set; }
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
   public class AnswerSummaryVM
 {
     public int QuestionId { get; set; }
     public string QuestionText { get; set; }
     public double AverageScaleValue { get; set; }
     public int ResponseCount { get; set; }
     public List<string> TextResponses { get; set; }
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
public static class InjectionExtensions
{
    public static void InjectDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));
    }
    public static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IQuestionFormRepository, QuestionFormRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IQuestionFormService, QuestionFormService>();
        services.AddScoped<IAnswerService, AnswerService>();
    }
    public static void InjectAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
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
         CreateMap<User, UserVM>()
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
             .ReverseMap();
         CreateMap<User, RegisterUserVM>().ReverseMap();
     }
 }
  public class RoleMappingProfile : Profile
 {
     public RoleMappingProfile()   { CreateMap<Role, RoleVM>().ReverseMap();  }
 }
  public class QuestionTypeMappingProfile : Profile
 {
     public QuestionTypeMappingProfile()  { CreateMap<QuestionType, QuestionTypeVM>().ReverseMap(); }
 }
public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile() 
    {
        CreateMap<Question, QuestionVM>()
        .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
        .ReverseMap();
        CreateMap<Question, RegisterQuestionVM>().ReverseMap();
    }
}
 public class QuestionFormMappingProfile : Profile
 {
     public QuestionFormMappingProfile() 
     {
         CreateMap<QuestionForm, QuestionFormVM>()
         .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
         .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count))
         .ReverseMap();
         CreateMap<QuestionForm, FormSubmissionVM>().ReverseMap();
     }
 }
 public class AnswerMappingProfile : Profile
 {
     public AnswerMappingProfile()
     { 
         CreateMap<Answer, AnswerVM>().ReverseMap();
         CreateMap<Answer, AnswerSummary>().ReverseMap();
         CreateMap<Answer, AnswerSummaryVM>().ReverseMap();
         CreateMap<Answer, ResultsVM>().ReverseMap();
         CreateMap<Answer, UserFormAnswersVM>()
         .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.Text))
         .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Question.QuestionType.Name));
     }
 }
public interface IUserService
{
    UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM);
    UserVM GetUserById(int id);
    UserVM ValidateUser(UserCredentialsVM userCredentialsViewModel);
}
 public interface IQuestionService
 {
     QuestionVM GetQuestionById(int questionId);
     // Admin methods
     void CreateQuestion(RegisterQuestionVM registerQuestionVM);
     int RemoveQuestion(int questionId);
     void UpdateQuestion(RegisterQuestionVM registerQuestionVM);
 }
  public interface IQuestionFormService
  {
      // For employees - get active forms
      Task<List<QuestionFormVM>> GetActiveFormsAsync();
      Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);
      // For admins - form management
      Task<List<QuestionFormVM>> GetAllFormsAsync();
      Task<QuestionFormVM> GetFormByIdAsync(int formId);
      Task<int> CreateFormAsync(QuestionFormVM formVm);
      Task<bool> UpdateFormAsync(QuestionFormVM formVm);
      Task<bool> DeleteFormAsync(int formId);
      Task<bool> ToggleFormStatusAsync(int formId, bool isActive);
      // Additional useful methods
      Task<bool> FormExistsAsync(int formId);
      Task<int> GetFormQuestionCountAsync(int formId);
  }
 public interface IAnswerService
 {
     // Answer submission
     Task<AnswerSubmissionResult> SubmitAnswersAsync(List<AnswerVM> answers, int userId);
     Task<bool> SubmitSingleAnswerAsync(List<AnswerVM> answer, int userId);
     // Retrieval methods
     Task<List<AnswerVM>> GetUserAnswersAsync(int userId);
     Task<List<AnswerVM>> GetFormAnswersAsync(int formId);
     Task<List<AnswerVM>> GetQuestionAnswersAsync(int questionId);
     Task<AnswerVM> GetAnswerByIdAsync(int answerId);
     // User-specific queries
     Task<List<UserFormAnswersVM>> GetUserFormAnswersAsync(int userId, int formId);
     Task<bool> HasUserSubmittedFormAsync(int userId, int formId);
     Task<DateTime?> GetUserLastSubmissionDateAsync(int userId, int formId);
     // Analytics and reporting
     Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId);
     Task<FormAnalyticsVM> GetFormAnalyticsAsync(int formId);
     Task<List<AnswerVM>> GetRecentAnswersAsync(int count = 50);
     // Admin operations
     Task<bool> DeleteAnswerAsync(int answerId);
     Task<bool> UpdateAnswerAsync(AnswerVM answer);
     Task<int> GetFormResponseCountAsync(int formId);
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

     public UserVM ValidateUser(UserCredentialsVM userCredentials)
     {
         var user = _userRepository.GetByCompanyId(userCredentials.CompanyId);
         if (user == null || user.Password != userCredentials.Password)
             return null;
        
             return _mapper.Map<UserVM>(user);
        
     }
     public UserVM GetUserById(int id)
     {
         var user = _userRepository.GetByCompanyId(id);
         return _mapper.Map<UserVM>(user);
     }

     public UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM)
     {
         var user = _mapper.Map<User>(registerUserViewModel);
         PasswordHasherHelper.HashPassword(user, registerUserViewModel.Password); //?? do I need this???
         user.RoleId = roleVM.Name == "Administrator" ? 1 : 2;
         var userId = _userRepository.Insert(user);

         if (userId <= 0)
         {
             throw new Exception("User not saved.");
         }

         return GetUserById(userId);
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
    public void CreateQuestion(RegisterQuestionVM registerQuestionVM)
    {
        var question = _mapper.Map<Question>(registerQuestionVM);
        _questionRepository.InsertQuestion(question);
    }
    public int RemoveQuestion(int questionId)
    {
        _questionRepository.DeleteQuestion(questionId);
        return questionId;
    }
    public QuestionVM GetQuestionById(int questionId)
    {
        var question = _questionRepository.GetQuestionById(questionId);
        return _mapper.Map<QuestionVM>(question);
    }
    public void UpdateQuestion(RegisterQuestionVM registerQuestionVM)
    {
        var question = _mapper.Map<Question>(registerQuestionVM);
        _questionRepository.UpdateQuestion(question);
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
    public async Task<List<QuestionFormVM>> GetActiveFormsAsync()
    {
        var forms = _questionFormRepository.GetActiveForms();
        return _mapper.Map<List<QuestionFormVM>>(forms);
    }
    public async Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId)
    {
        var form = _questionFormRepository.GetFormWithQuestions(formId);
        if (form == null)
            throw new Exception($"Form with id {formId} not found.");
        return _mapper.Map<QuestionFormVM>(form);
    }
    public async Task<List<QuestionFormVM>> GetAllFormsAsync()
    {
        var forms = _questionFormRepository.GetAllFormQuestions();
        return _mapper.Map<List<QuestionFormVM>>(forms);
    }
    public async Task<QuestionFormVM> GetFormByIdAsync(int formId)
    {
        var form = _questionFormRepository.GetQuestionFormById(formId);
        if (form == null)
            throw new Exception($"Form with id {formId} not found.");
        return _mapper.Map<QuestionFormVM>(form);
    }
    public async Task<int> CreateFormAsync(QuestionFormVM formVm)
    {
        var form = _mapper.Map<QuestionForm>(formVm);
        form.CreatedDate = DateTime.UtcNow;
        form.IsActive = true;
        return await _questionFormRepository.InsertFormAsync(form);
    }
    public async Task<bool> UpdateFormAsync(QuestionFormVM formVm)
    {
        var existingForm = _questionFormRepository.GetQuestionFormById(formVm.Id);
        if (existingForm == null)
            return false;
        _mapper.Map(formVm, existingForm);
        return await _questionFormRepository.UpdateFormAsync(existingForm);
    }
    public async Task<bool> DeleteFormAsync(int formId)
    {
        try
        {
            _questionFormRepository.DeleteQuestionForm(formId);
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error deleting form {formId}: {ex.Message}");
            return false;
        }
    }
    public async Task<bool> ToggleFormStatusAsync(int formId, bool isActive)
    {
        return await _questionFormRepository.ToggleFormStatusAsync(formId, isActive);
    }
    public async Task<bool> FormExistsAsync(int formId)
    {
        var form = _questionFormRepository.GetQuestionFormById(formId);
        return form != null;
    }
    public async Task<int> GetFormQuestionCountAsync(int formId)
    {
        var form = _questionFormRepository.GetFormWithQuestions(formId);
        return form?.Questions?.Count ?? 0;
    }
}
public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;
    public AnswerService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IMapper mapper)
    {
        _answerRepository = answerRepository;
        _questionRepository = questionRepository;
        _mapper = mapper;
    }
    public async Task<AnswerSubmissionResult> SubmitAnswersAsync(List<AnswerVM> answers, int userId)
    {
        var result = new AnswerSubmissionResult();
        if (answers == null || !answers.Any())
        {
            result.Success = false;
            result.Errors.Add("No answers provided.");
            return result;
        }
        try
        {
            // Validate required questions
            var validationResult = await ValidateAnswersAsync(answers, userId);
            if (!validationResult.IsValid)
            {
                result.Success = false;
                result.Errors.AddRange(validationResult.Errors);
                return result;
            }
            // Check if user already submitted this form
            var formId = answers.First().QuestionFormId;
            var hasSubmitted = await _answerRepository.HasUserSubmittedFormAsync(userId, formId);
            if (hasSubmitted)
            {
                result.Success = false;
                result.Errors.Add("You have already submitted answers for this form.");
                return result;
            }
            // Map and prepare answers
            var answerEntities = _mapper.Map<List<Answer>>(answers);
            foreach (var answer in answerEntities)
            {
                answer.UserId = userId;
                answer.AnsweredDate = DateTime.UtcNow;

                // Validate answer based on question type
                var question = _questionRepository.GetQuestionById(answer.QuestionId);
                if (question != null)
                {
                    if (question.QuestionType.Name == "Scale" && !answer.ScaleValue.HasValue)
                    {
                        result.Errors.Add($"Scale value required for question: {question.Text}");
                    }
                    else if (question.QuestionType.Name == "Text" && string.IsNullOrWhiteSpace(answer.TextValue))
                    {
                        if (question.IsRequired)
                        {
                            result.Errors.Add($"Text answer required for question: {question.Text}");
                        }
                    }
                }
            }
            if (result.Errors.Any())
            {
                result.Success = false;
                return result;
            }
            // Save answers
            var savedCount = await _answerRepository.InsertAnswersAsync(answerEntities);
            result.Success = savedCount > 0;
            result.SubmittedAnswersCount = savedCount;
            result.SubmissionDate = DateTime.UtcNow;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add($"An error occurred while submitting answers: {ex.Message}");
            return result;
        }
    }
    public async Task<bool> SubmitSingleAnswerAsync(List<AnswerVM> answer, int userId)
    {
        try
        {
            var answerEntity = _mapper.Map<Answer>(answer);
            answerEntity.UserId = userId;
            answerEntity.AnsweredDate = DateTime.UtcNow;

            var answerId = await _answerRepository.InsertAnswerAsync(answerEntity);
            return answerId > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<List<AnswerVM>> GetUserAnswersAsync(int userId)
    {
        var answers = await _answerRepository.GetAnswersByUserIdAsync(userId);
        return _mapper.Map<List<AnswerVM>>(answers);
    }

    public async Task<List<AnswerVM>> GetFormAnswersAsync(int formId)
    {
        var answers = await _answerRepository.GetFormAnswersWithDetailsAsync(formId);
        return _mapper.Map<List<AnswerVM>>(answers);
    }
    public async Task<List<AnswerVM>> GetQuestionAnswersAsync(int questionId)
    {
        var answers = await _answerRepository.GetAnswersByQuestionIdAsync(questionId);
        return _mapper.Map<List<AnswerVM>>(answers);
    }
    public async Task<AnswerVM> GetAnswerByIdAsync(int answerId)
    {
        var answer = await _answerRepository.GetAnswerByIdAsync(answerId);
        return _mapper.Map<AnswerVM>(answer);
    }
    public async Task<List<UserFormAnswersVM>> GetUserFormAnswersAsync(int userId, int formId)
    {
        var answers = await _answerRepository.GetAnswersByUserIdAsync(userId);
        var formAnswers = answers.Where(a => a.QuestionFormId == formId).ToList();

        return _mapper.Map<List<UserFormAnswersVM>>(formAnswers);
    }
    public async Task<bool> HasUserSubmittedFormAsync(int userId, int formId)
    {
        return await _answerRepository.HasUserSubmittedFormAsync(userId, formId);
    }

    public async Task<DateTime?> GetUserLastSubmissionDateAsync(int userId, int formId)
    {
        var answers = await _answerRepository.GetAnswersByUserIdAsync(userId);
        return answers.Where(a => a.QuestionFormId == formId)
                     .OrderByDescending(a => a.AnsweredDate)
                     .FirstOrDefault()?.AnsweredDate;
    }
    public async Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId)
    {
        var summaries = await _answerRepository.GetAnswerSummariesAsync(formId);
        return _mapper.Map<Dictionary<int, AnswerSummaryVM>>(summaries);
    }
    public async Task<FormAnalyticsVM> GetFormAnalyticsAsync(int formId)
    {
        var responseCount = await _answerRepository.GetFormResponseCountAsync(formId);
        var summaries = await GetAnswerSummariesAsync(formId);
        var recentAnswers = await GetRecentAnswersAsync(10);
        return new FormAnalyticsVM
        {
            FormId = formId,
            TotalResponses = responseCount,
            AnswerSummaries = summaries,
            RecentSubmissions = recentAnswers,
            AnalyticsDate = DateTime.UtcNow
        };
    }
    public async Task<List<AnswerVM>> GetRecentAnswersAsync(int count = 50)
    {
        var answers = await _answerRepository.GetRecentAnswersAsync(count);
        return _mapper.Map<List<AnswerVM>>(answers);
    }
    public async Task<bool> DeleteAnswerAsync(int answerId)
    {
        return await _answerRepository.DeleteAnswerAsync(answerId);
    }
    public async Task<bool> UpdateAnswerAsync(AnswerVM answer)
    {
        var answerEntity = _mapper.Map<Answer>(answer);
        return await _answerRepository.UpdateAnswerAsync(answerEntity);
    }
    public async Task<int> GetFormResponseCountAsync(int formId)
    {
        return await _answerRepository.GetFormResponseCountAsync(formId);
    }
    // Private helper methods
    private async Task<AnswerValidationResult> ValidateAnswersAsync(List<AnswerVM> answers, int userId)
    {
        var result = new AnswerValidationResult();

        foreach (var answer in answers)
        {
            var question = _questionRepository.GetQuestionById(answer.QuestionId);
            if (question == null)
            {
                result.Errors.Add($"Invalid question ID: {answer.QuestionId}");
                continue;
            }
            // Check required questions
            if (question.IsRequired)
            {
                if (question.QuestionType.Name == "Scale" && !answer.ScaleValue.HasValue)
                {
                    result.Errors.Add($"Required question not answered: {question.Text}");
                }
                else if (question.QuestionType.Name == "Text" && string.IsNullOrWhiteSpace(answer.TextValue))
                {
                    result.Errors.Add($"Required question not answered: {question.Text}");
                }
            }
            // Validate scale range
            if (question.QuestionType.Name == "Scale" && answer.ScaleValue.HasValue)
            {
                if (answer.ScaleValue < 1 || answer.ScaleValue > 10)
                {
                    result.Errors.Add($"Invalid scale value for question: {question.Text}. Must be between 1-10.");
                }
            }
        }
        result.IsValid = !result.Errors.Any();
        return result;
    }
}
```
## 6) Razor view snippets

**Views/Account/Login.cshtml** // add + changed
```html
@model UserCredentialsVM

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Employee Satisfaction Survey - Login</title>
    <link rel="stylesheet" href="~/css/site.css" />
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

            <form asp-action="Login" method="post" id="loginForm">
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
```

**Views/Questionnaire/Form.cshtml** // add-changed
```html
@model FormSubmissionVM

<div class="questionnaire-container">
    <div class="questionnaire-header fade-in">
        <h1>@Model.QuestionForm.Title</h1>
        <p>@Model.QuestionForm.Description</p>
        
        <div class="progress-container">
            <div class="progress-bar">
                <div class="progress-fill" style="width: 0%"></div>
            </div>
            <small class="text-muted">Progress: <span id="progress-text">0%</span></small>
        </div>
    </div>

    <div class="questionnaire-form slide-in">
        <form asp-action="SubmitForm" method="post" id="questionnaireForm">
            <input type="hidden" name="QuestionFormId" value="@Model.QuestionForm.Id" />
            <input type="hidden" id="current-form" value="@Model.QuestionForm.Id" />
            <input type="hidden" id="total-forms" value="@ViewBag.TotalForms" />
            
            @for (int i = 0; i < Model.QuestionForm.Questions.Count; i++)
            {
                var question = Model.QuestionForm.Questions[i];
                <div class="question-item" data-required="@question.IsRequired.ToString().ToLower()">
                    <h4>@question.Text @(question.IsRequired ? "*" : "")</h4>
                    <input type="hidden" asp-for="Answers[i].QuestionId" value="@question.Id" />
                    <input type="hidden" asp-for="Answers[i].QuestionFormId" value="@Model.QuestionForm.Id" />
                    
                    @if (question.QuestionType == "Scale")
                    {
                        <div class="scale-answers">
                            @for (int j = 1; j <= 10; j++)
                            {
                                <label>
                                    <input type="radio" asp-for="Answers[i].ScaleValue" value="@j" />
                                    <span>@j</span>
                                </label>
                            }
                        </div>
                        <div class="scale-labels">
                            <span>Strongly Disagree</span>
                            <span>Strongly Agree</span>
                        </div>
                    }
                    else
                    {
                        <textarea asp-for="Answers[i].TextValue" class="text-answer form-control" 
                                  placeholder="Please enter your response here..."></textarea>
                        <span asp-validation-for="Answers[i].TextValue" class="text-danger"></span>
                    }
                </div>
            }
            
            <div class="navigation-buttons">
                @if (Model.QuestionForm.Id > 1)
                {
                    <a href="@Url.Action("Form", new { id = Model.QuestionForm.Id - 1 })" 
                       class="btn btn-prev">← Previous</a>
                }
                else
                {
                    <div></div> <!-- Empty div for flex spacing -->
                }
                
                <button type="submit" class="btn btn-success" id="submitBtn">
                    <span class="btn-text">
                        @(Model.QuestionForm.Id == ViewBag.TotalForms ? "Submit All Answers" : "Next →")
                    </span>
                    <div class="loading-spinner" style="display: none;"></div>
                </button>
            </div>
        </form>
    </div>
</div>
@section Scripts {
    <script>
        // Initialize questionnaire manager
        document.addEventListener('DOMContentLoaded', function() {
            window.questionnaireManager = new QuestionnaireManager();
        });
    </script>
}
```
**Views/Questionnaire/ThankYou.cshtml (Enhanced)** 
```html
@{
    ViewData["Title"] = "Thank You";
}

<div class="thank-you-container fade-in">
    <div class="alert-success">
        <div class="success-icon" style="font-size: 4em; margin-bottom: 20px;">✅</div>
        <h2>Thank You for Your Feedback!</h2>
        <p>Your responses have been successfully recorded and will help us improve the workplace environment.</p>
        <p>We truly value your input and commitment to making our company better.</p>

        <div class="mt-4">
            <a href="@Url.Action("Logout", "Account")" class="btn btn-primary">Return to Login</a>
        </div>
    </div>
</div>

<script>
    // Celebrate completion with confetti
    document.addEventListener('DOMContentLoaded', function() {
        if (typeof confetti === 'function') {
            confetti({
                particleCount: 100,
                spread: 70,
                origin: { y: 0.6 }
            });
        }

        // Clear all saved progress
        Object.keys(localStorage).forEach(key => {
            if (key.startsWith('questionnaire_progress_')) {
                localStorage.removeItem(key);
            }
        });
    });
</script>
```
**update _Layout.cshtml**
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Employee Satisfaction Survey</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <!-- Confetti.js -->
    <script src="https://cdn.jsdelivr.net/npm/canvas-confetti@1.9.2/dist/confetti.browser.min.js"></script>
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container-fluid">
                <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">GlasAnketa</a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2025 - Employee Satisfaction Survey - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```
## 7) Controllers 
```csharp
public class AccountController : Controller
{
    private readonly IUserService _userService;
    public AccountController(IUserService userService)
    {
        _userService = userService;
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
            if (user.Role.Name == "Administrator")
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("Index", "Questionnaire");
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
// QuestionnaireController.cs
public class QuestionnaireController : Controller
{
    private readonly IQuestionFormService _formService;
    private readonly IAnswerService _answerService;
    public QuestionnaireController(IQuestionFormService formService, IAnswerService answerService)
    {
        _formService = formService;
        _answerService = answerService;
    }
    public async Task<IActionResult> Index()
    {
        var forms = await _formService.GetActiveFormsAsync();
        return View(forms);
    }
    [HttpGet]
    public async Task<IActionResult> Form(int id)
    {
        var form = await _formService.GetFormWithQuestionsAsync(id);
        if (form == null)
            return NotFound();

        var viewModel = new FormSubmissionVM
        {
            QuestionForm = form,
            Answers = form.Questions.Select(q => new AnswerVM
            {
                QuestionId = q.Id,
                QuestionFormId = id
            }).ToList()
        };
        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitForm(FormSubmissionVM model)
    {
        if (!ModelState.IsValid)
            return View("Form", model);
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Account");
        try
        {
            var result = await _answerService.SubmitAnswersAsync(model.Answers, userId.Value);
            if (result.Success)
            {
                // Clear any saved progress for this form
                var formId = model.Answers.FirstOrDefault()?.QuestionFormId;
                if (formId.HasValue)
                {
                    // Clear localStorage progress if using client-side storage
                    TempData["SubmissionSuccess"] = true;
                }
                return RedirectToAction("ThankYou");
            }
            else
            {
                // Add errors to ModelState for display
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                // Log the submission attempt
                TempData["ErrorMessage"] = "There were errors with your submission. Please review and try again.";
                return View("Form", model);
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            ModelState.AddModelError("", "An unexpected error occurred while submitting your answers. Please try again.");
            TempData["ErrorMessage"] = "A system error occurred. Please contact support if the problem persists.";
            return View("Form", model);
        }
    }
    public IActionResult ThankYou()
    {
        // Check if this is a genuine successful submission
        if (TempData["SubmissionSuccess"] == null)
        {
            return RedirectToAction("Index");
        }
        var viewModel = new ThankYouVM
        {
            SubmittedCount = TempData["SubmittedCount"] as int? ?? 0,
            SubmissionDate = DateTime.Now
        };
        return View(viewModel);
    }
}
 [Authorize(Roles = "Administrator")]
 public class AdminController : Controller
 {
     private readonly IQuestionService _questionService;
     private readonly IQuestionFormService _formService;
     private readonly IAnswerService _answerService;
     public AdminController(IQuestionService questionService, IQuestionFormService formService, IAnswerService answerService)
     {
         _questionService = questionService;
         _formService = formService;
         _answerService = answerService;
     }
     public IActionResult Index()
     {
         return View();
     }
     [HttpGet]
     public async Task<IActionResult> ManageQuestions()
     {
         // Implementation for question management
         return View();
     }
     [HttpGet]
     public async Task<IActionResult> ViewResults(int formId)
     {
         var answers = await _answerService.GetFormAnswersAsync(formId);
         var summaries = await _answerService.GetAnswerSummariesAsync(formId);
         var viewModel = new ResultsVM
         {
             Answers = answers,
             Summaries = summaries
         };
         return View(viewModel);
     }
 }
```
