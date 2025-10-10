using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

public class QuestionnaireController : Controller
{
    private readonly IQuestionFormService _formService;
    private readonly IAnswerRepository _answerRepository;

    public QuestionnaireController(IQuestionFormService formService, IAnswerRepository answerRepository)
    {
        _formService = formService;
        _answerRepository = answerRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Form(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Account");

        var form = await _formService.GetFormWithQuestionsAsync(id);
        if (form == null)
        {
            TempData["ErrorMessage"] = "Form not found.";
            return RedirectToAction("Login", "Account");
        }

        var vm = new FormSubmissionVM
        {
            QuestionForm = form,
            QuestionFormId = form.Id,
            Answers = form.Questions.Select(q => new AnswerVM
            {
                QuestionId = q.Id,
                QuestionFormId = form.Id,
                UserId = userId.Value
            }).ToList()
        };

        return View(vm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitForm(FormSubmissionVM model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            await RepopulateForm(model);
            return View("Form", model);
        }

        var answerDict = new Dictionary<int, object>();
        foreach (var a in model.Answers)
        {
            if (a.ScaleValue.HasValue)
                answerDict[a.QuestionId] = a.ScaleValue.Value;
            else if (!string.IsNullOrEmpty(a.TextValue))
                answerDict[a.QuestionId] = a.TextValue;
        }

        var success = await _answerRepository.SubmitAnswersAsync(userId.Value, model.QuestionFormId, answerDict);

        if (!success)
        {
            ModelState.AddModelError("", "Error saving answers. Please try again.");
            await RepopulateForm(model);
            return View("Form", model);
        }

        TempData["SuccessMessage"] = "✅ Answers submitted successfully!";
        return RedirectToAction("ThankYou");
    }

    private async Task RepopulateForm(FormSubmissionVM model)
    {
        model.QuestionForm = await _formService.GetFormWithQuestionsAsync(model.QuestionFormId);
    }

    [HttpGet]
    public IActionResult ThankYou()
    {
        return View();
    }
}
