using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlasAnketa.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionFormService _questionFormService;

        public AnswerController(IAnswerService answerService, IQuestionFormService questionFormService)
        {
            _answerService = answerService;
            _questionFormService = questionFormService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAnswers(FormSubmissionVM model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Save the submitted answers
            var answers = model.Answers.ToDictionary(
                a => a.QuestionId,
                a => (object)(a.ScaleValue ?? (object)a.TextValue)
            );

            await _answerService.SubmitAnswersAsync(userId.Value, model.QuestionFormId, answers);

            // Get next form
            var nextForm = await _questionFormService.GetNextActiveFormAsync(model.QuestionFormId);

            if (nextForm == null)
                return RedirectToAction("ThankYou", "Questionnaire");

            // Redirect instead of returning View (to avoid losing state)
            return RedirectToAction("ShowForm", "Questionnaire", new { formId = nextForm.Id });
        }
    }

}
