using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlasAnketa.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionFormService _questionFormService;
        private readonly IAnswerService _answerService;

        public QuestionnaireController(IQuestionFormService questionFormService, IAnswerService answerService)
        {
            _questionFormService = questionFormService;
            _answerService = answerService;
        }
        // GET: Questionnaire/Form/{id}
        [HttpGet]
        public async Task<IActionResult> Form(int id)
        {
            try
            {
                // Get logged in user
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                    return RedirectToAction("Login", "Account");

                // Load the active form
                var questionForm = await _questionFormService.GetFormByIdAsync(id);
                if (questionForm == null)
                {
                    TempData["ErrorMessage"] = "Form not found or inactive.";
                    return RedirectToAction("Login", "Account");
                }
                // Prepare VM for the view
                var model = new FormSubmissionVM
                {
                    QuestionFormId = questionForm.Id,
                    QuestionForm = questionForm,
                    Answers = questionForm.Questions
                        .Select(q => new AnswerVM
                        {
                            QuestionId = q.Id,
                            QuestionFormId = questionForm.Id,
                            UserId = userId.Value
                        })
                        .ToList()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                // Graceful error handling
                ViewBag.Error = $"An unexpected error occurred: {ex.Message}";
                return View("Error");
            }
        }
        // POST: Questionnaire/SubmitForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm(FormSubmissionVM model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Set UserId for all answers before saving
            foreach (var answer in model.Answers)
            {
                answer.UserId = userId.Value;
            }

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
            //var nextVm = new FormSubmissionVM
            //{
            //    QuestionForm = nextForm,
            //    Answers = nextForm.Questions.Select(q => new AnswerVM
            //    {
            //        QuestionId = q.Id,
            //        QuestionFormId = nextForm.Id,
            //        UserId = userId.Value
            //    }).ToList()
            //};
            var nextVm = new FormSubmissionVM
            {
                QuestionFormId = nextForm.Id,
                QuestionForm = nextForm,
                Answers = new List<AnswerVM>(), // Empty list for new form
                NextFormId = (await _questionFormService.GetNextActiveFormAsync(nextForm.Id))?.Id
            };

            return View("Form", nextVm);
        }
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}