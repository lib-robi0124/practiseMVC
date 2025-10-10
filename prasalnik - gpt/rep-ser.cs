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
     Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
     Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId);
     Task<List<Answer>> GetAnswersByUserIdAsync(int userId, int formId1);
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
     public async Task<List<Answer>> GetAnswersByUserIdAsync(int userId, int formId1)
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
 public interface IUserService
{
    Task<UserVM> RegisterUser(RegisterUserVM registerUserVM, RoleVM roleVM);
    Task<QuestionVM> RegisterQuestion(RegisterQuestionVM registerQuestionVM, RoleVM roleVM);
    Task<QuestionFormVM> CreateQuestionForm(CreateQuestionFormVM createQuestionFormVM, RoleVM roleVM);
    Task<QuestionFormVM> UpdateFormAsync(QuestionFormVM formVm, RoleVM roleVM);
    Task<UserVM> GetUserById(int id);
    Task<UserVM> ValidateUser(UserCredentialsVM userCredentialsVM);
}
public interface IQuestionService
{
    Task<QuestionVM> GetQuestionById(int questionId);
    Task<List<QuestionVM>> GetAllQuestions();
}
public interface IQuestionFormService
{
    Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);
    Task<List<QuestionFormVM>> GetAllActiveFormsAsync();
    Task<QuestionFormVM> GetActiveFormAsync();

}
public interface IAnswerService
{
    Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
    Task<AnswerVM> GetUserAnswersAsync(int userId, int formId1);
    Task<AnswerVM> GetFormAnswersAsync(int formId);
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
    public async Task<UserVM> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserVM>(user);
    }
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
    public async Task<QuestionVM> RegisterQuestion(RegisterQuestionVM registerQuestionVM, RoleVM roleVM)
    {
        var question = _mapper.Map<Question>(registerQuestionVM);
        question.UserId = roleVM.RoleId; // Use Role.Id, not RoleId from VM
        question.IsRequired = true;

        await _questionRepository.AddAsync(question);
        return _mapper.Map<QuestionVM>(question);
    }
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
    public async Task<QuestionFormVM> UpdateFormAsync(QuestionFormVM formVm, RoleVM roleVM)
    {
        var qForm = _mapper.Map<QuestionForm>(formVm);
        qForm.CreatedDate = DateTime.UtcNow;
        qForm.IsActive = true;

        await _questionFormRepository.Update(qForm);
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
 public class QuestionFormService : IQuestionFormService
 {
     private readonly IQuestionFormRepository _questionFormRepository;
     private readonly IMapper _mapper;
     public QuestionFormService(IQuestionFormRepository questionFormRepository, IMapper mapper)
     {
         _questionFormRepository = questionFormRepository;
         _mapper = mapper;
     }
     public async Task<QuestionFormVM> GetActiveFormAsync()
     {
         var forms = await _questionFormRepository.GetAllFormQuestionsAsync();
         var activeForm = forms.FirstOrDefault(f => f.IsActive);
         return _mapper.Map<QuestionFormVM>(activeForm);
     }
     public async Task<List<QuestionFormVM>> GetAllActiveFormsAsync()
     {
         var forms = await _questionFormRepository.GetAllFormQuestionsAsync();
         var active = forms.Where(f => f.IsActive).ToList();
         return _mapper.Map<List<QuestionFormVM>>(active);
     }
     public async Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId)
     {
         var qForm = await _questionFormRepository.GetByIdAsync(formId);
         return _mapper.Map<QuestionFormVM>(qForm);
     }
 }
 public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IMapper _mapper;
    public AnswerService(IAnswerRepository answerRepository, IMapper mapper)
    {
        _answerRepository = answerRepository;
        _mapper = mapper;
    }
    public async Task<AnswerVM> GetFormAnswersAsync(int formId)
    {
        var answers = await _answerRepository.GetAnswersByQuestionFormIdAsync(formId);
        return _mapper.Map<AnswerVM>(answers);
    }
    public async Task<AnswerVM> GetUserAnswersAsync(int userId, int formId1)
    {
        var answers = await _answerRepository.GetAnswersByUserIdAsync(userId, formId1);
        return _mapper.Map<AnswerVM>(answers);

    }
    public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
    {
        return await _answerRepository.SubmitAnswersAsync(userId, formId, answers);
    }
}
 public class UserVM
 {
         public int Id { get; set; }
         public int CompanyId { get; set; }
         public string FullName { get; set; }
         public string OU { get; set; }
         public string OU2 { get; set; }
         public RoleVM Role { get; set; }
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
   public class RoleVM
 {
     public int RoleId { get; set; }
     public string Name { get; set; }
 }
  public class RegisterUserVM
 {
     public int CompanyId { get; set; }
     public string FullName { get; set; }
     public string OU { get; set; }
     public string OU2 { get; set; }
     public string Password { get; set; }
 }
  public class RegisterQuestionVM
 {
     public int Id { get; set; }
     public int QuestionFormId { get; set; }
     public string Text { get; set; }
     public int QuestionTypeId { get; set; }
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
      public List<QuestionVM> Questions { get; set; } = new List<QuestionVM>();
      public List<AnswerVM> Answers { get; set; } = new List<AnswerVM>();
  }
   public class CreateQuestionFormVM
 {
     [Required(ErrorMessage = "Title is required")]
     [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
     public string Title { get; set; }
     [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
     public string Description { get; set; }
     public bool IsActive { get; set; } = true;
     public List<RegisterQuestionVM> Questions { get; set; } = new List<RegisterQuestionVM>();
 }
  public class AnswerVM
 {
     public int UserId { get; set; }
     public int QuestionId { get; set; }
     public int QuestionFormId { get; set; }
     public int? ScaleValue { get; set; } 
     public string? TextValue { get; set; } 
     public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
 }