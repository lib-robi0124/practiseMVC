using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlasAnketa.Controllers
{
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionFormService _questionFormService;

        public QuestionnaireController(IQuestionFormService questionFormService)
        {
            _questionFormService = questionFormService;
        }
        // GET: Questionnaire/Form/{id}
        [HttpGet]
        public async Task<IActionResult> ShowForm(int? formId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Load all active forms ordered by Id for deterministic navigation
            var activeForms = await _questionFormService.GetAllActiveFormsAsync();
            activeForms = activeForms.OrderBy(f => f.Id).ToList();

            if (activeForms.Count == 0)
                return RedirectToAction("ThankYou");

            QuestionFormVM? form;
            if (formId.HasValue)
            {
                form = activeForms.FirstOrDefault(f => f.Id == formId.Value);
                // If requested form is not active, move to the next active one after it; if none, end
                if (form == null)
                {
                    form = activeForms.FirstOrDefault(f => f.Id > formId.Value);
                    if (form == null)
                        return RedirectToAction("ThankYou");
                }
            }
            else
            {
                form = activeForms.First(); // first active form
            }

            // Determine navigation info
            var idx = activeForms.FindIndex(f => f.Id == form.Id);
            var nextFormId = (idx >= 0 && idx < activeForms.Count - 1) ? activeForms[idx + 1].Id : (int?)null;

            // Populate the VM
            var vm = new FormSubmissionVM
            {
                QuestionForm = form,
                QuestionFormId = form.Id,
                NextFormId = nextFormId,
                Answers = form.Questions.Select(q => new AnswerVM
                {
                    QuestionId = q.Id,
                    QuestionFormId = form.Id,
                    UserId = userId.Value
                }).ToList()
            };

            // Provide counters for UI (progress display)
            ViewBag.CurrentFormNumber = idx + 1;
            ViewBag.TotalForms = activeForms.Count;
            ViewBag.IsLastForm = nextFormId == null;
            ViewBag.NextFormId = nextFormId;

            return View("Form", vm);
        }
        
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}