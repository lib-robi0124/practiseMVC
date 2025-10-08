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

        // REMOVE Index action - we don't need it anymore
        // Users go directly to first form

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
                    // If this form is submitted, redirect to next form or thank you
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
                Console.WriteLine($"SubmitForm called for form {model.QuestionFormId}, User: {userId}");

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid");
                    await RepopulateFormData(model);
                    return View("Form", model);
                }

                // Validate that we have answers
                if (model.Answers == null || !model.Answers.Any())
                {
                    ModelState.AddModelError("", "No answers provided.");
                    await RepopulateFormData(model);
                    return View("Form", model);
                }

                Console.WriteLine($"Processing {model.Answers.Count} answers");

                // Prepare answers for submission - include ALL answers
                var answersToSubmit = new List<AnswerVM>();

                foreach (var answer in model.Answers)
                {
                    // For Scale questions, include even if ScaleValue is 0 (not selected)
                    // For Text questions, include even if empty
                    answersToSubmit.Add(new AnswerVM
                    {
                        UserId = userId.Value,
                        QuestionId = answer.QuestionId,
                        QuestionFormId = model.QuestionFormId,
                        ScaleValue = answer.ScaleValue,
                        TextValue = answer.TextValue
                    });
                }

                Console.WriteLine($"Submitting {answersToSubmit.Count} answers to database");

                // Submit answers to service
                var result = await _answerService.SubmitAnswersAsync(answersToSubmit, userId.Value);

                Console.WriteLine($"Submit result: Success={result.Success}, Count={result.SubmittedAnswersCount}, Errors: {string.Join(", ", result.Errors)}");

                if (result.Success)
                {
                    TempData["SubmissionSuccess"] = true;
                    TempData["SubmittedCount"] = result.SubmittedAnswersCount;

                    // Get next form ID
                    var nextFormId = await GetNextFormId(model.QuestionFormId);

                    if (nextFormId.HasValue)
                    {
                        Console.WriteLine($"Redirecting to next form: {nextFormId}");
                        return RedirectToAction("Form", new { id = nextFormId.Value });
                    }
                    else
                    {
                        Console.WriteLine("All forms completed, redirecting to ThankYou");
                        return RedirectToAction("ThankYou");
                    }
                }
                else
                {
                    Console.WriteLine($"Submission failed: {string.Join(", ", result.Errors)}");
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
                Console.WriteLine($"Exception in SubmitForm: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
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

            return null; // No more forms
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

        // Debug method to check what's happening
        [HttpGet]
        public async Task<IActionResult> DebugInfo()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var activeForms = await _formService.GetActiveFormsAsync();

            return Json(new
            {
                UserId = userId,
                SessionUserId = HttpContext.Session.GetInt32("UserId"),
                SessionUserRole = HttpContext.Session.GetString("UserRole"),
                ActiveFormsCount = activeForms.Count,
                Forms = activeForms.Select(f => new { f.Id, f.Title, f.QuestionCount }),
                NextFormId = await GetNextFormId(1) // Test with first form
            });
        }
    }
}