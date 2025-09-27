using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Anketa.Controllers
{
    [Route("Questionnaire")]
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IMapper _mapper;

        public QuestionnaireController(IQuestionnaireService questionnaireService, IMapper mapper)
        {
            _questionnaireService = questionnaireService;
            _mapper = mapper;
        }

        [HttpGet("Index")] // explicitly mapped
        public IActionResult Index()
        {
            var questionnaire = _questionnaireService.GetAll().FirstOrDefault();
            if (questionnaire == null) return View("Empty");

            var vm = _mapper.Map<QuestionnaireViewModel>(questionnaire);
            return View(vm);
        }

        [HttpGet("SubmitAnswers")]
        public IActionResult SubmitAnswers()
        {
            return View();
        }

        [HttpPost("SubmitAnswers")]
        public IActionResult SubmitAnswers(AnswerViewModel model)
        {
            // process answers
            return RedirectToAction("ThankYou");
        }

        [HttpGet("ThankYou")]
        public IActionResult ThankYou()
        {
            return View();
        }
    }

}

