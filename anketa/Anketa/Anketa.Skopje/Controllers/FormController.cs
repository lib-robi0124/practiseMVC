using Anketa.Services.Interfaces;
using Anketa.Services.Mappers;
using Anketa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Anketa.Skopje.Controllers
{
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

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

            var vm = new SubmitFormViewModel
            {
                FormId = formId,
                FormTitle = form.Title,
                Answers = form.Questions.Select(q => new AnswerInputViewModel
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    QuestionType = q.QuestionType.Name
                }).ToList()
            };

            return View(vm);
        }

        public IActionResult Thanks() => View();
    }
}
