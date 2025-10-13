# GlasAnketa Application - Complete Architecture Flow Documentation

**Date:** October 13, 2025  
**Project:** Employee Survey System - Request Lifecycle Analysis  
**Location:** `C:\qinshift\practiseMVC\DobraAnketaUpdate\GlasAnketaForma`  
**Type:** Educational Flow Documentation

---

## 📚 **Teaching Guide: Application Request Flow**

This document provides a step-by-step walkthrough of how a user request flows through the entire GlasAnketa application, from initial startup to final logout. Each step includes detailed explanations of what happens behind the scenes.

---

## 🚀 **PHASE 1: Application Startup & Bootstrap**

### **Step 1: Application Initialization (Program.cs)**

```csharp
// 🟢 APPLICATION STARTS HERE
var builder = WebApplication.CreateBuilder(args);

// 📋 Services Registration
builder.Services.AddControllersWithViews();     // ← MVC Pattern Support
builder.Services.AddSession();                  // ← Session State Management
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔧 Dependency Injection Setup
builder.Services.InjectRepositories();          // ← Data Access Layer
builder.Services.InjectServices();              // ← Business Logic Layer
builder.Services.AddAutoMapper(typeof(Program)); // ← Object Mapping

var app = builder.Build();

// 🛡️ Middleware Pipeline Configuration
app.UseHttpsRedirection();  // ← Force HTTPS
app.UseStaticFiles();       // ← Serve CSS/JS/Images
app.UseRouting();           // ← URL Routing
app.UseSession();           // ← Session Management
app.UseAuthentication();    // ← Authentication
app.UseAuthorization();     // ← Authorization

// 🎯 Default Route Configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
    //       ↑ DEFAULT ENTRY POINT

app.Run(); // ← Start Web Server
```

**🎓 Teacher Notes:**
- Application starts with `Program.cs`
- Default route points to `Account/Login` - this is why login page appears first
- Dependency Injection container is configured here
- Middleware pipeline processes requests in order

---

## 🔐 **PHASE 2: User Authentication Flow**

### **Step 2: Initial Request → Login Page**

```
🌐 User Browser Request: GET https://localhost:5001/
                        ↓
🎯 Routing Engine: No specific controller/action specified
                        ↓
📍 Default Route Applied: {controller=Account}/{action=Login}
                        ↓
🎮 AccountController.Login() [HttpGet] is invoked
```

### **Step 3: AccountController.Login() [GET] Method**

```csharp
[HttpGet]
public IActionResult Login()
{
    // 📊 INPUTS: None (initial page load)
    
    // 🔄 PROCESSING:
    // - No authentication check needed (this IS the login page)
    // - No business logic required
    // - Simply return the login view
    
    // 📤 OUTPUT: Login.cshtml view
    return View(); // ← Returns Views/Account/Login.cshtml
}
```

### **Step 4: Login.cshtml View Rendering**

```html
@model UserCredentialsVM  <!-- 📋 Strongly typed view model -->

<form asp-action="Login" method="post">
    <!-- 🛡️ CSRF Protection -->
    <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>
    
    <!-- 📝 Input Fields -->
    <input asp-for="CompanyId" class="form-control" placeholder="Enter Employee ID" />
    <input asp-for="Password" type="password" class="form-control" placeholder="Enter Password" />
    
    <!-- ✅ Submit Button -->
    <button type="submit" class="btn btn-login">
        <i class="fas fa-sign-in-alt"></i>Login
    </button>
</form>
```

**🎓 Teacher Notes:**
- View is strongly typed with `UserCredentialsVM`
- Form posts back to same controller/action but with [HttpPost]
- Razor syntax generates proper HTML with validation

---

## 🔄 **PHASE 3: Login Form Submission**

### **Step 5: User Submits Login Form**

```
🖱️ User clicks "Login" button
                        ↓
📨 HTTP POST Request: POST /Account/Login
    Content-Type: application/x-www-form-urlencoded
    Body: CompanyId=12345&Password=mypassword
                        ↓
🎯 MVC Routing: Routes to AccountController.Login() [HttpPost]
```

### **Step 6: AccountController.Login() [POST] Method**

```csharp
[HttpPost]
public async Task<IActionResult> Login(UserCredentialsVM model)
{
    // 📊 INPUTS: 
    // - model.CompanyId (from form)
    // - model.Password (from form)
    
    // 🔍 STEP 6.1: Model Validation
    if (!ModelState.IsValid)
        return View(model); // ← Return with validation errors
    
    // 🔍 STEP 6.2: User Authentication via Service Layer
    var user = await _userService.ValidateUser(model);
    //                      ↓
    // This calls: UserService.ValidateUser() →
    //              UserRepository.GetUserByCredentials() →
    //              Database Query: SELECT * FROM Users WHERE CompanyId = @id AND Password = @pwd
    
    if (user == null)
    {
        // ❌ AUTHENTICATION FAILED
        ModelState.AddModelError("", "Invalid Company ID or Password");
        return View(model); // ← Return to login with error
    }
    
    // ✅ AUTHENTICATION SUCCESS - Store user in session
    // 🔒 STEP 6.3: Session Management
    HttpContext.Session.SetInt32("UserId", user.Id);
    HttpContext.Session.SetString("UserRole", user.Role.Name);
    HttpContext.Session.SetString("UserName", user.FullName);
    HttpContext.Session.SetInt32("CompanyId", user.CompanyId);
    HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString());
    
    // 🎯 STEP 6.4: Role-based Routing Decision
    if (user.Role.Name == "Administrator")
        return RedirectToAction("Index", "Admin"); // ← Admin dashboard
    
    // 👥 Regular User Flow
    var form = await _formService.GetActiveFormAsync();
    if (form != null)
    {
        // 📋 Redirect to questionnaire
        return RedirectToAction("ShowForm", "Questionnaire", new { formId = 1 });
    }
    else
    {
        // ⚠️ No active forms available
        TempData["ErrorMessage"] = "No active questionnaires available.";
        return RedirectToAction("Login", "Account");
    }
}
```

**🎓 Teacher Notes:**
- Model binding automatically converts form data to `UserCredentialsVM`
- Service layer handles business logic (authentication)
- Session stores user information for subsequent requests
- Role-based routing directs users to appropriate areas

---

## 📋 **PHASE 4: Questionnaire Display Flow**

### **Step 7: Redirect to Questionnaire**

```
🔄 RedirectToAction("ShowForm", "Questionnaire", new { formId = 1 })
                        ↓
📨 HTTP GET Request: GET /Questionnaire/ShowForm/1
                        ↓
🎯 Routes to: QuestionnaireController.ShowForm(int? formId)
```

### **Step 8: QuestionnaireController.ShowForm() Method**

```csharp
[HttpGet]
public async Task<IActionResult> ShowForm(int? formId)
{
    // 📊 INPUTS:
    // - formId = 1 (from route parameter)
    
    // 🔒 STEP 8.1: Authentication Check
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return RedirectToAction("Login", "Account"); // ← Redirect if not logged in
    
    // 📋 STEP 8.2: Load Form Data from Database
    var form = formId.HasValue
        ? await _questionFormService.GetFormByIdAsync(formId.Value)
        : await _questionFormService.GetActiveFormAsync();
    //                                      ↓
    // This triggers: QuestionFormService.GetFormByIdAsync() →
    //                QuestionFormRepository.GetQuestionFormByIdAsync() →
    //                Database Query: SELECT f.*, q.*, qt.* 
    //                               FROM QuestionForms f
    //                               JOIN Questions q ON f.Id = q.QuestionFormId
    //                               JOIN QuestionTypes qt ON q.QuestionTypeId = qt.Id
    //                               WHERE f.Id = @formId
    
    if (form == null)
        return RedirectToAction("ThankYou"); // ← No more forms
    
    // 🏗️ STEP 8.3: Build ViewModel
    var vm = new FormSubmissionVM
    {
        QuestionForm = form,            // ← Form with questions
        QuestionFormId = form.Id,       // ← Form identifier
        Answers = form.Questions.Select(q => new AnswerVM
        {
            QuestionId = q.Id,          // ← Each question gets answer slot
            QuestionFormId = form.Id,
            UserId = userId.Value       // ← From session
        }).ToList()
    };
    
    // 📤 OUTPUT: Form.cshtml view with FormSubmissionVM
    return View("Form", vm);
}
```

**🎓 Teacher Notes:**
- Session-based authentication check on every request
- Service layer loads complex data (form + questions + question types)
- ViewModel is constructed to provide all data needed by the view
- Each question gets a pre-built answer object for form binding

---

## 🎨 **PHASE 5: Form Rendering**

### **Step 9: Form.cshtml View Rendering**

```html
@model FormSubmissionVM

<!DOCTYPE html>
<html>
<head>
    <title>@ViewData["Title"] - Employee Survey</title>
    <link rel="stylesheet" href="~/css/modern-questionnaire.css" />
</head>
<body>
    <!-- 🎯 Progress Indicator -->
    <div class="progress-indicator">
        <div class="progress-bar" style="width: 0%"></div>
    </div>
    
    <div class="questionnaire-container">
        <!-- 📊 Header Section -->
        <div class="questionnaire-header">
            <div class="user-info">
                Company ID: <strong>@Context.Session.GetInt32("CompanyId")</strong>
            </div>
            <div class="logout-section">
                <!-- 🚪 Logout Form -->
                <form asp-controller="Account" asp-action="Logout" method="post">
                    @Html.AntiForgeryToken()
                    <button type="submit" class="btn btn-outline-danger">Logout</button>
                </form>
            </div>
            
            <h1>@Model.QuestionForm.Title</h1>
            <p>@Model.QuestionForm.Description</p>
        </div>

        <!-- 📋 Main Form -->
        <div class="questionnaire-form">
            <form asp-action="SubmitAnswers" asp-controller="Answer" method="post" id="questionnaireForm">
                @Html.AntiForgeryToken()
                <input type="hidden" asp-for="QuestionFormId" />

                <!-- 🔄 Loop through questions -->
                @for (int i = 0; i < Model.QuestionForm.Questions.Count; i++)
                {
                    var question = Model.QuestionForm.Questions[i];
                    <div class="question-item">
                        <h4>@question.Text @(question.IsRequired ? "*" : "")</h4>

                        <!-- Hidden fields for model binding -->
                        <input type="hidden" name="Answers[@i].QuestionId" value="@question.Id" />
                        <input type="hidden" name="Answers[@i].QuestionFormId" value="@Model.QuestionForm.Id" />

                        @if (question.QuestionType == "Scale")
                        {
                            <!-- 📊 Scale Questions (1-10) -->
                            <div class="scale-answers">
                                @for (int j = 1; j <= 10; j++)
                                {
                                    <label class="scale-option">
                                        <input type="radio" name="Answers[@i].ScaleValue" value="@j" />
                                        <span class="scale-number">@j</span>
                                    </label>
                                }
                            </div>
                            <div class="scale-labels">
                                <span>1 - Very Unsatisfied</span>
                                <span>10 - Very Satisfied</span>
                            </div>
                        }
                        else if (question.QuestionType == "Text")
                        {
                            <!-- ✍️ Text Questions -->
                            <textarea name="Answers[@i].TextValue" class="text-answer form-control"
                                      placeholder="Please enter your response here..."></textarea>
                        }
                    </div>
                }

                <!-- ✅ Submit Button -->
                <div class="navigation-buttons">
                    <button type="submit" class="btn btn-success" id="submitBtn">
                        <span class="btn-text">Save & Continue →</span>
                        <div class="loading-spinner" style="display: none;"></div>
                    </button>
                </div>
            </form>
        </div>
    </div>

    <!-- 🎨 JavaScript for interactivity -->
    <script>
        // Progress tracking, validation, animations, etc.
    </script>
</body>
</html>
```

**🎓 Teacher Notes:**
- Strongly typed view with `FormSubmissionVM`
- Complex form with nested objects (`Answers[i].QuestionId`)
- Session data displayed in header
- Form posts to `Answer/SubmitAnswers`
- JavaScript enhances user experience

---

## 💾 **PHASE 6: Answer Submission Flow**

### **Step 10: User Submits Answers**

```
🖱️ User fills form and clicks "Save & Continue"
                        ↓
📨 HTTP POST Request: POST /Answer/SubmitAnswers
    Content-Type: application/x-www-form-urlencoded
    Body: QuestionFormId=1&Answers[0].QuestionId=1&Answers[0].ScaleValue=8&Answers[1].QuestionId=2&Answers[1].TextValue=Great%20experience
                        ↓
🎯 Routes to: AnswerController.SubmitAnswers(FormSubmissionVM model)
```

### **Step 11: AnswerController.SubmitAnswers() Method**

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SubmitAnswers(FormSubmissionVM model)
{
    // 📊 INPUTS:
    // - model.QuestionFormId = 1
    // - model.Answers[0].QuestionId = 1, ScaleValue = 8
    // - model.Answers[1].QuestionId = 2, TextValue = "Great experience"
    
    // 🔒 STEP 11.1: Authentication Check
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return RedirectToAction("Login", "Account");
    
    // 🔄 STEP 11.2: Transform Data for Service Layer
    var answers = model.Answers.ToDictionary(
        a => a.QuestionId,                           // Key: Question ID
        a => (object)(a.ScaleValue ?? (object)a.TextValue)  // Value: Answer (scale or text)
    );
    // Results in: { 1 => 8, 2 => "Great experience" }
    
    // 💾 STEP 11.3: Save to Database via Service Layer
    await _answerService.SubmitAnswersAsync(userId.Value, model.QuestionFormId, answers);
    //                                      ↓
    // This triggers complex chain:
    // AnswerService.SubmitAnswersAsync() →
    // AnswerRepository.SubmitAnswersAsync() →
    // Multiple Database Operations:
    //   1. DELETE existing answers for this user/form
    //   2. INSERT INTO Answers (UserId, QuestionId, ScaleValue, TextValue, AnsweredDate)
    //      VALUES (123, 1, 8, NULL, '2025-10-13 18:00:00')
    //   3. INSERT INTO Answers (UserId, QuestionId, ScaleValue, TextValue, AnsweredDate)
    //      VALUES (123, 2, NULL, 'Great experience', '2025-10-13 18:00:00')
    
    // 🎯 STEP 11.4: Determine Next Form
    var nextForm = await _questionFormService.GetNextActiveFormAsync(model.QuestionFormId);
    
    if (nextForm == null)
    {
        // ✅ All forms completed
        return RedirectToAction("ThankYou", "Questionnaire");
    }
    
    // 📋 Continue to next form
    return RedirectToAction("ShowForm", "Questionnaire", new { formId = nextForm.Id });
}
```

**🎓 Teacher Notes:**
- Complex model binding converts form data to objects
- Data transformation prepares it for service layer
- Database operations are transactional (delete + insert)
- Flow continues to next form or completion page

---

## 📊 **PHASE 7: Database Operations Deep Dive**

### **Step 12: AnswerRepository.SubmitAnswersAsync() - Database Layer**

```csharp
public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        // 🗑️ STEP 12.1: Clean Previous Answers
        var existingAnswers = _context.Answers
            .Where(a => a.UserId == userId && 
                       a.Question.QuestionFormId == formId);
        
        _context.Answers.RemoveRange(existingAnswers);
        //                    ↓
        // SQL: DELETE FROM Answers 
        //      WHERE UserId = @userId 
        //        AND QuestionId IN (SELECT Id FROM Questions WHERE QuestionFormId = @formId)
        
        // 💾 STEP 12.2: Insert New Answers
        foreach (var answerPair in answers)
        {
            var answer = new Answer
            {
                UserId = userId,
                QuestionId = answerPair.Key,
                ScaleValue = answerPair.Value is int scale ? scale : (int?)null,
                TextValue = answerPair.Value is string text ? text : null,
                AnsweredDate = DateTime.UtcNow
            };
            
            _context.Answers.Add(answer);
            //                    ↓
            // SQL: INSERT INTO Answers (UserId, QuestionId, ScaleValue, TextValue, AnsweredDate)
            //      VALUES (@userId, @questionId, @scaleValue, @textValue, @answeredDate)
        }
        
        // 💾 STEP 12.3: Commit Transaction
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        
        return true;
    }
    catch (Exception ex)
    {
        // ❌ Rollback on error
        await transaction.RollbackAsync();
        throw;
    }
}
```

**🎓 Teacher Notes:**
- Database transaction ensures data consistency
- Previous answers are deleted before inserting new ones
- Entity Framework generates SQL automatically
- Error handling with transaction rollback

---

## 🎯 **PHASE 8: Multi-Form Navigation**

### **Step 13: Next Form Logic**

```csharp
// In QuestionFormService.GetNextActiveFormAsync()
public async Task<QuestionFormVM> GetNextActiveFormAsync(int currentFormId)
{
    // 📋 STEP 13.1: Get all active forms ordered by sequence
    var forms = await _context.QuestionForms
        .Where(f => f.IsActive)
        .OrderBy(f => f.Id)  // Or whatever ordering logic you have
        .ToListAsync();
    //                    ↓
    // SQL: SELECT * FROM QuestionForms 
    //      WHERE IsActive = 1 
    //      ORDER BY Id
    
    // 🔍 STEP 13.2: Find current form index
    var currentIndex = forms.FindIndex(f => f.Id == currentFormId);
    
    // 🎯 STEP 13.3: Return next form or null if last
    return currentIndex >= 0 && currentIndex < forms.Count - 1
        ? _mapper.Map<QuestionFormVM>(forms[currentIndex + 1])
        : null;
}
```

### **Step 14: Form Completion or Continuation**

```
📋 If nextForm != null:
    → Redirect to ShowForm with nextForm.Id
    → User continues with next questionnaire
    → Process repeats from Step 7

✅ If nextForm == null:
    → Redirect to ThankYou page
    → Survey completion flow
```

---

## 🏁 **PHASE 9: Survey Completion**

### **Step 15: ThankYou Page**

```csharp
// QuestionnaireController.ThankYou()
public IActionResult ThankYou()
{
    // 📊 INPUTS: None (completion page)
    
    // 🔄 PROCESSING: 
    // - Could log completion
    // - Could send notifications
    // - Could update user status
    
    // 📤 OUTPUT: ThankYou.cshtml view
    return View();
}
```

---

## 🚪 **PHASE 10: Logout Flow**

### **Step 16: User Clicks Logout**

```
🖱️ User clicks "Logout" button (available on any page)
                        ↓
📨 HTTP POST Request: POST /Account/Logout
    + Anti-forgery token
                        ↓
🎯 Routes to: AccountController.Logout() [HttpPost]
```

### **Step 17: AccountController.Logout() Method**

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Logout()
{
    // 📊 INPUTS: None (just the logout request)
    
    // 🔒 STEP 17.1: Clear Session Data
    HttpContext.Session.Clear();
    //                    ↓
    // This removes all session data:
    // - UserId
    // - UserRole  
    // - UserName
    // - CompanyId
    // - LastActivity
    
    // 🎯 STEP 17.2: Redirect to Login
    return RedirectToAction("Login");
    //                    ↓
    // HTTP 302 Redirect: Location: /Account/Login
    // Browser automatically follows redirect
    // Process starts over from Step 2
}
```

---

## 🔄 **Complete Request Flow Summary**

```
🚀 STARTUP
Program.cs → Dependency Injection → Middleware Pipeline → Default Route

🔐 AUTHENTICATION  
Browser Request → AccountController.Login[GET] → Login.cshtml → 
User Input → AccountController.Login[POST] → UserService.ValidateUser → 
Database Query → Session Storage → Role-based Redirect

📋 QUESTIONNAIRE
QuestionnaireController.ShowForm → QuestionFormService.GetFormById → 
Database Query → ViewModel Construction → Form.cshtml → User Interaction

💾 ANSWER SUBMISSION
AnswerController.SubmitAnswers → Data Transformation → 
AnswerService.SubmitAnswersAsync → AnswerRepository → Database Transaction → 
Next Form Logic → Redirect or Complete

🏁 COMPLETION
ThankYou Page or Next Form → Continue Loop

🚪 LOGOUT
AccountController.Logout → Session.Clear() → Redirect to Login → Cycle Repeats
```

---

## 🎓 **Key Learning Points**

### **1. MVC Pattern Implementation**
- **Models**: ViewModels (UserCredentialsVM, FormSubmissionVM), Domain Models (User, Question, Answer)
- **Views**: Razor templates with strongly typed models
- **Controllers**: Request handling, business logic coordination, response generation

### **2. Dependency Injection & Service Layer**
- Controllers depend on Services (IUserService, IQuestionFormService)  
- Services depend on Repositories (IUserRepository, IAnswerRepository)
- Repositories handle data access via Entity Framework

### **3. Session Management**
- User state maintained across requests via HttpContext.Session
- Authentication check on every protected action
- Session cleared on logout for security

### **4. Data Flow Patterns**
- **Request**: Browser → Controller → Service → Repository → Database
- **Response**: Database → Repository → Service → Controller → View → Browser

### **5. Security Considerations**
- Anti-forgery tokens on all forms
- Session-based authentication checks
- Role-based access control
- Input validation and model binding

### **6. Database Transaction Management**
- Entity Framework handles most transactions automatically
- Manual transactions for complex operations (answer submission)
- Rollback capabilities for error scenarios

---

## 📊 **Performance & Scalability Notes**

### **Database Efficiency**
- Async/await pattern for all database operations
- Entity Framework query optimization
- Proper indexing on frequently queried columns

### **Session State Considerations**
- Session stored in server memory (for single server)
- Consider Redis/SQL Server session state for scale-out
- Session timeout and cleanup policies

### **Caching Opportunities**
- Form definitions (rarely change)
- User roles and permissions  
- Static content (CSS, JavaScript)

---

**Generated on:** October 13, 2025  
**Documentation Type:** Educational Flow Analysis  
**Complexity Level:** Intermediate to Advanced  
**Target Audience:** Developers learning ASP.NET Core MVC

*This documentation provides a complete understanding of how requests flow through the GlasAnketa survey application, serving as both a technical reference and educational guide.*