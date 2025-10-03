// ViewModels/LoginViewModel.cs
public class LoginViewModel
{
[Required]
public int CompanyId { get; set; }


[Required]
[StringLength(200)]
public string FullName { get; set; }


[Required]
[DataType(DataType.Password)]
public string Password { get; set; }
}


// ViewModels/QuestionViewModel.cs
public class QuestionViewModel
{
public int Id { get; set; }
public string Text { get; set; }
public string QuestionType { get; set; }
public bool IsRequired { get; set; }
}


// ViewModels/QuestionFormViewModel.cs
public class QuestionFormViewModel
{
public int Id { get; set; }
public string Title { get; set; }
public string Description { get; set; }
public List<QuestionViewModel> Questions { get; set; } = new();
}
// ViewModels/LoginViewModel.cs
public class LoginViewModel
{
[Required]
public int CompanyId { get; set; }


[Required]
[StringLength(200)]
public string FullName { get; set; }


[Required]
[DataType(DataType.Password)]
public string Password { get; set; }
}


// ViewModels/QuestionViewModel.cs
public class QuestionViewModel
{
public int Id { get; set; }
public string Text { get; set; }
public string QuestionType { get; set; }
public bool IsRequired { get; set; }
}


// ViewModels/QuestionFormViewModel.cs
public class QuestionFormViewModel
{
public int Id { get; set; }
public string Title { get; set; }
public string Description { get; set; }
public List<QuestionViewModel> Questions { get; set; } = new();
}


// ViewModels/AnswerInputViewModel.cs
public class AnswerInputViewModel
{
public int QuestionId { get; set; }
// ScaleValue OR TextValue depending on question type
public int? ScaleValue { get; set; }
public string TextValue { get; set; }
}


// ViewModels/SubmitFormViewModel.cs
public class SubmitFormViewModel
{
[Required]
public int FormId { get; set; }


[Required]
public int UserId { get; set; }


[Required]
public List<AnswerInputViewModel> Answers { get; set; } = new();
}
Mappers (simple helper to map Domain ↔ ViewModel)
public static class Mappers
{
public static QuestionViewModel ToViewModel(this Question q) => new()
{
Id = q.Id,
Text = q.Text,
QuestionType = q.QuestionType?.Name ?? string.Empty,
IsRequired = q.IsRequired
};


public static QuestionFormViewModel ToViewModel(this QuestionForm f)
{
return new QuestionFormViewModel
{
Id = f.Id,
Title = f.Title,
Description = f.Description,
Questions = f.Questions?.Select(q => q.ToViewModel()).ToList() ?? new List<QuestionViewModel>()
};
}}
//Helpers — Authentication & Password Hashing 
public static class AuthHelper
{
private const int SaltSize = 16; // 128 bit
private const int KeySize = 32; // 256 bit
private const int Iterations = 10000;
public static string HashPassword(string password)
{
using var rng = RandomNumberGenerator.Create();
var salt = new byte[SaltSize];
rng.GetBytes(salt);
using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
var key = pbkdf2.GetBytes(KeySize);
var hashBytes = new byte[1 + SaltSize + KeySize];
hashBytes[0] = 0; // version
Buffer.BlockCopy(salt, 0, hashBytes, 1, SaltSize);
Buffer.BlockCopy(key, 0, hashBytes, 1 + SaltSize, KeySize);
return Convert.ToBase64String(hashBytes);
}

public static bool VerifyPassword(string password, string hashed)
{
var hashBytes = Convert.FromBase64String(hashed);
if (hashBytes[0] != 0) return false;
var salt = new byte[SaltSize];
Buffer.BlockCopy(hashBytes, 1, salt, 0, SaltSize);
var key = new byte[KeySize];
Buffer.BlockCopy(hashBytes, 1 + SaltSize, key, 0, KeySize);
using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
var computed = pbkdf2.GetBytes(KeySize);

return CryptographicOperations.FixedTimeEquals(computed, key);
}}
Important: Update your seed data to store Password = AuthHelper.HashPassword("16130") etc. Storing plain companyId as password is insecure.
//Controllers (skeletons)
// Controllers/AccountController.cs
var user = await _userService.AuthenticateAsync(vm.CompanyId, vm.FullName, vm.Password);
if (user == null)
{
ModelState.AddModelError(string.Empty, "Invalid credentials");
return View(vm);
}
// Create simple cookie/session (adjust to your auth mechanism)
HttpContext.Session.SetInt32("UserId", user.Id);
HttpContext.Session.SetString("FullName", user.FullName);
return RedirectToAction("Index", "Form");
}
public IActionResult Logout()
{
HttpContext.Session.Clear();
return RedirectToAction("Login");
}}
// Controllers/FormController.cs
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
var vm = forms.Select(f => f.ToViewModel()).ToList();
return View(vm);
}

public async Task<IActionResult> Take(int id)
{
var form = await _questionService.GetFormWithQuestionsAsync(id);
if (form == null) return NotFound();
var vm = form.ToViewModel();
return View(vm);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Submit(SubmitFormViewModel vm)
{
if (!ModelState.IsValid) return BadRequest(ModelState);

// map to Dictionary<int, object>
var answersDict = vm.Answers.ToDictionary(a => a.QuestionId, a => (object)(a.QuestionTypeIsScale() ? (object)a.ScaleValue.GetValueOrDefault() : (object)a.TextValue ?? string.Empty));

var ok = await _answerService.SubmitAnswersAsync(vm.UserId, vm.FormId, answersDict);
if (!ok) return StatusCode(500, "Could not save answers");

return RedirectToAction("Thanks");
}

public IActionResult Thanks() => View();
}
Note: AnswerInputViewModel may need a helper QuestionTypeIsScale() — you can load question types client-side or verify server side by checking question meta before saving.
//Admin controller (create/edit questions)
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
}}
//Razor view snippets
//Views/Account/Login.cshtml
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
//Views/Form/Take.cshtml
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