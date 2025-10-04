//LoginViewModel.cs
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
//FormViewModel.cs
    public class FormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }
// ViewModels/QuestionViewModel.cs
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string QuestionType { get; set; }
        public bool IsRequired { get; set; }
    }
//AnswerViewModel.cs
    public class AnswerViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        
        [Range(1, 10, ErrorMessage = "Scale value must be between 1 and 10")]
        public int? ScaleValue { get; set; }
        
        [StringLength(500, ErrorMessage = "Text answer cannot exceed 500 characters")]
        public string TextValue { get; set; }
    }

    public class FormAnswersViewModel
    {
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public List<AnswerViewModel> Answers { get; set; } = new();
    }
//QuestionMapper.cs
    public static class QuestionMapper
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

        public static FormViewModel ToViewModel(this QuestionForm form)
        {
            return new FormViewModel
            {
                Id = form.Id,
                Title = form.Title,
                Description = form.Description,
                Questions = form.Questions?.Select(q => q.ToViewModel())
                .ToList() ?? new List<QuestionViewModel>()
            };
        }
    }
//UserController.cs
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

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
                new Claim("CompanyId", user.CompanyId),
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
//FormController.cs
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
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

            var viewModel = new FormAnswersViewModel
            {
                FormId = formId,
                FormTitle = form.Title,
                Answers = form.Questions.Select(q => new AnswerViewModel
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    QuestionType = q.QuestionType.Name
                }).ToList()
            };

            return View(viewModel);
        }
    }
//AnswerController.cs

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
                TempData["Error"] = "No answers provided";
                return RedirectToAction("Answer", "Form", new { formId });
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var success = await _answerService.SubmitAnswersAsync(userId, formId, answers);
            
            if (success)
            {
                TempData["Success"] = "Thank you for submitting your answers!";
                return RedirectToAction("Index", "Form");
            }
            else
            {
                TempData["Error"] = "There was an error submitting your answers. Please try again.";
                return RedirectToAction("Answer", "Form", new { formId });
            }
        }
    }
//Views/User/Login.cshtml
@model Anketa.ViewModels.LoginViewModel

@{
    ViewData["Title"] = "Employee Satisfaction Survey - Login";
}

<div class="row justify-content-center">
    <div class="col-md-6">
        <div class="card">
            <div class="card-header">
                <h4 class="text-center">Employee Satisfaction Survey</h4>
            </div>
            <div class="card-body">
                <form asp-action="Login" method="post">
                    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                    
                    <div class="form-group mb-3">
                        <label asp-for="CompanyId" class="form-label"></label>
                        <input asp-for="CompanyId" class="form-control" />
                        <span asp-validation-for="CompanyId" class="text-danger"></span>
                    </div>
                    
                    <div class="form-group mb-3">
                        <label asp-for="FullName" class="form-label"></label>
                        <input asp-for="FullName" class="form-control" />
                        <span asp-validation-for="FullName" class="text-danger"></span>
                    </div>
                    
                    <div class="form-group mb-3">
                        <label asp-for="Password" class="form-label"></label>
                        <input asp-for="Password" type="password" class="form-control" />
                        <span asp-validation-for="Password" class="text-danger"></span>
                    </div>
                    
                    <div class="form-group">
                        <button type="submit" class="btn btn-primary w-100">Login</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>
//Views/Form/Index.cshtml
@model List<Anketa.ViewModels.FormViewModel>

@{
    ViewData["Title"] = "Available Surveys";
}

<div class="container">
    <h2>Available Satisfaction Surveys</h2>
    
    @if (TempData["Success"] != null)
    {
        <div class="alert alert-success">@TempData["Success"]</div>
    }
    @if (TempData["Message"] != null)
    {
        <div class="alert alert-info">@TempData["Message"]</div>
    }

    <div class="row">
        @foreach (var form in Model)
        {
            <div class="col-md-6 mb-4">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">@form.Title</h5>
                        <p class="card-text">@form.Description</p>
                        <p class="card-text">
                            <small class="text-muted">@form.Questions.Count questions</small>
                        </p>
                        <a href="@Url.Action("Answer", new { formId = form.Id })" class="btn btn-primary">Start Survey</a>
                    </div>
                </div>
            </div>
        }
    </div>
</div>
Anketa.Web/
├── Controllers/
│   ├── UserController.cs ✓
│   ├── FormController.cs ✓
│   └── AnswerController.cs ✓
├── Views/
│   ├── User/
│   │   └── Login.cshtml ✓
│   ├── Form/
│   │   ├── Index.cshtml ✓
│   │   └── Answer.cshtml (TO CREATE)
│   └── Shared/
│       └── _Layout.cshtml (TO CREATE)
├── ViewModels/ ✓
├── Mappers/ ✓
└── Program.cs (UPDATE FOR AUTHENTICATION)