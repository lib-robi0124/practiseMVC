**QuestionFormService**

public async Task<QuestionFormVM?> GetNextActiveFormAsync(int currentFormId)
{
    // Find the next active form ID
    var nextForm = await _questionFormRepository.GetNextActiveFormAsync(currentFormId);
    if (nextForm == null)
        return null;

    // Map fresh form with questions (loaded directly from DB)
    return _mapper.Map<QuestionFormVM>(nextForm);
}
**QuestionFormRepository * *
public async Task<List<QuestionForm>> GetAllFormQuestionsAsync()
{
    return await _context.QuestionForms
        .Include(f => f.Questions)
        .ThenInclude(q => q.QuestionType)
        .OrderBy(f => f.Id)
        .ToListAsync();
}
**QFormRep_V1* *
public async Task<QuestionForm> GetNextActiveFormAsync(int currentFormId)
{
    return await _context.QuestionForms
        .Where(f => f.IsActive && f.Id > currentFormId)
        .Include(f => f.Questions)
        .ThenInclude(q => q.QuestionType)
        .OrderBy(f => f.Id)
        .FirstOrDefaultAsync();
}

**QuestionnaireController * *
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SubmitForm(FormSubmissionVM model)
{
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null) return RedirectToAction("Login", "Account");

    // Save answers
    var answers = model.Answers.ToDictionary(
        a => a.QuestionId,
        a => (object)(a.ScaleValue ?? (object)a.TextValue)
    );

    var result = await _answerService.SubmitAnswersAsync(userId.Value, model.QuestionFormId, answers);

    if (!result)
    {
        TempData["Error"] = "Failed to save your answers. Please try again.";
        return RedirectToAction("Form", new { id = model.QuestionFormId });
    }

    // ✅ Get next form
    var nextForm = await _questionFormService.GetNextActiveFormAsync(model.QuestionFormId);

    if (nextForm == null)
        return RedirectToAction("ThankYou");

    // ✅ Load next form page
    var nextVm = new FormSubmissionVM
    {
        QuestionForm = nextForm,
        Answers = nextForm.Questions.Select(q => new AnswerVM
        {
            QuestionId = q.Id,
            QuestionFormId = nextForm.Id,
            UserId = userId.Value
        }).ToList()
    };

    return View("Form", nextVm);
}
public IActionResult ThankYou()
{
    return View();
}
**QContr_V2**
[HttpPost]
public async Task<IActionResult> SubmitForm(FormSubmissionVM model)
{
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null) return RedirectToAction("Login", "Account");

    // Save answers
    var answers = model.Answers.ToDictionary(
        a => a.QuestionId,
        a => (object)(a.ScaleValue ?? (object)a.TextValue)
    );
    await _answerService.SubmitAnswersAsync(userId.Value, model.QuestionFormId, answers);

    // Get next form
    var nextForm = await _questionFormService.GetNextActiveFormAsync(model.QuestionFormId);
    if (nextForm == null)
        return RedirectToAction("ThankYou");

    // Load next form’s questions
    var nextVm = new FormSubmissionVM
    {
        QuestionForm = nextForm,
        Answers = nextForm.Questions.Select(q => new AnswerVM
        {
            QuestionId = q.Id,
            QuestionFormId = nextForm.Id,
            UserId = userId.Value
        }).ToList()
    };

    return View("Form", nextVm);
}


