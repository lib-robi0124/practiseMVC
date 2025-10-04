using Anketa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Anketa.Skopje.Controllers
{
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
                TempData["Error"] = "No answers provided.";
                return RedirectToAction("Answer", "Form", new { formId });
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var success = await _answerService.SubmitAnswersAsync(userId, formId, answers);

            if (success)
            {
                TempData["Success"] = "Thank you for submitting your answers!";
                return RedirectToAction("Thanks", "Form");
            }

            TempData["Error"] = "Error submitting your answers. Please try again.";
            return RedirectToAction("Answer", "Form", new { formId });
        }
    }
}
