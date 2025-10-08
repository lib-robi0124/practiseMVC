using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlasAnketa.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionFormService _formService;
        private readonly IAnswerService _answerService;
        public AdminController(IQuestionService questionService, IQuestionFormService formService, IAnswerService answerService)
        {
            _questionService = questionService;
            _formService = formService;
            _answerService = answerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ManageQuestions()
        {
            // Implementation for question management
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ViewResults(int formId)
        {
            var answers = await _answerService.GetFormAnswersAsync(formId);
            var summaries = await _answerService.GetAnswerSummariesAsync(formId);
            var viewModel = new ResultsVM
            {
                Answers = answers,
                Summaries = summaries
            };
            return View(viewModel);
        }
    }
}
