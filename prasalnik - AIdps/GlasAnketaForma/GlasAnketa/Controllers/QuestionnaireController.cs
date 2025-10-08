using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlasAnketa.Controllers
{
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
}