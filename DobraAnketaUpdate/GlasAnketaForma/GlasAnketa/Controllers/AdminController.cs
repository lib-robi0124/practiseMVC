using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlasAnketa.Controllers
{
    public class AdminController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionFormService _formService;
        private readonly IAnswerService _answerService;
        private readonly IUserService _userService;

        public AdminController(IQuestionService questionService,
                             IQuestionFormService formService,
                             IAnswerService answerService,
                             IUserService userService)
        {
            _questionService = questionService;
            _formService = formService;
            _answerService = answerService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageQuestions()
        {
            var forms = await _formService.GetAllFormsAsync();
            ViewBag.QuestionForms = new SelectList(forms, "Id", "Title");

            // Get all questions from all forms
            var allQuestions = new List<QuestionVM>();
            foreach (var form in forms)
            {
                allQuestions.AddRange(form.Questions);
            }

            return View(allQuestions);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            // This will be implemented when we have user management
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ManageForms()
        {
            var forms = await _formService.GetAllFormsAsync();
            return View(forms);
        }

        [HttpGet]
        public async Task<IActionResult> CreateQuestion()
        {
            var forms = await _formService.GetAllFormsAsync();
            ViewBag.QuestionForms = new SelectList(forms, "Id", "Title");

            // Mock question types - in real app, get from database
            var questionTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Scale (1-10)" },
                new SelectListItem { Value = "2", Text = "Text" }
            };
            ViewBag.QuestionTypes = new SelectList(questionTypes, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(RegisterQuestionVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _questionService.CreateQuestion(model);
                    TempData["SuccessMessage"] = "Question created successfully!";
                    return RedirectToAction("ManageQuestions");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating question: {ex.Message}");
                }
            }

            // Repopulate dropdowns if validation fails
            var forms = await _formService.GetAllFormsAsync();
            ViewBag.QuestionForms = new SelectList(forms, "Id", "Title");

            var questionTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Scale (1-10)" },
                new SelectListItem { Value = "2", Text = "Text" }
            };
            ViewBag.QuestionTypes = new SelectList(questionTypes, "Value", "Text");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditQuestion(int id)
        {
            try
            {
                var question = await _questionService.GetQuestionById(id);
                if (question == null)
                {
                    TempData["ErrorMessage"] = "Question not found.";
                    return RedirectToAction("ManageQuestions");
                }

                var forms = await _formService.GetAllFormsAsync();
                ViewBag.QuestionForms = new SelectList(forms, "Id", "Title");

                var questionTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Scale (1-10)", Selected = question.QuestionType == "Scale" },
                    new SelectListItem { Value = "2", Text = "Text", Selected = question.QuestionType == "Text" }
                };
                ViewBag.QuestionTypes = new SelectList(questionTypes, "Value", "Text");

                var editModel = new RegisterQuestionVM
                {
                    Id = question.Id,
                    QuestionFormId = question.QuestionFormId,
                    Text = question.Text,
                    QuestionTypeId = question.QuestionType == "Scale" ? 1 : 2
                };

                return View(editModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading question: {ex.Message}";
                return RedirectToAction("ManageQuestions");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(RegisterQuestionVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _questionService.UpdateQuestion(model);
                    TempData["SuccessMessage"] = "Question updated successfully!";
                    return RedirectToAction("ManageQuestions");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating question: {ex.Message}");
                }
            }

            // Repopulate dropdowns
            var forms = await _formService.GetAllFormsAsync();
            ViewBag.QuestionForms = new SelectList(forms, "Id", "Title");

            var questionTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Scale (1-10)" },
                new SelectListItem { Value = "2", Text = "Text" }
            };
            ViewBag.QuestionTypes = new SelectList(questionTypes, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                _questionService.RemoveQuestion(id);
                TempData["SuccessMessage"] = "Question deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting question: {ex.Message}";
            }

            return RedirectToAction("ManageQuestions");
        }

        [HttpGet]
        public async Task<IActionResult> CreateForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForm(QuestionFormVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _formService.CreateFormAsync(model);
                    TempData["SuccessMessage"] = "Form created successfully!";
                    return RedirectToAction("ManageForms");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating form: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditForm(int id)
        {
            try
            {
                var form = await _formService.GetFormByIdAsync(id);
                if (form == null)
                {
                    TempData["ErrorMessage"] = "Form not found.";
                    return RedirectToAction("ManageForms");
                }

                return View(form);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading form: {ex.Message}";
                return RedirectToAction("ManageForms");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditForm(QuestionFormVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _formService.UpdateFormAsync(model);
                    if (success)
                    {
                        TempData["SuccessMessage"] = "Form updated successfully!";
                        return RedirectToAction("ManageForms");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update form.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating form: {ex.Message}");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                var success = await _formService.DeleteFormAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Form deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete form.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting form: {ex.Message}";
            }

            return RedirectToAction("ManageForms");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFormStatus(int id, bool isActive)
        {
            try
            {
                var success = await _formService.ToggleFormStatusAsync(id, isActive);
                if (success)
                {
                    TempData["SuccessMessage"] = $"Form {(isActive ? "activated" : "deactivated")} successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update form status.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating form status: {ex.Message}";
            }

            return RedirectToAction("ManageForms");
        }

        [HttpGet]
        public async Task<IActionResult> ViewResults(int formId = 1)
        {
            try
            {
                var answers = await _answerService.GetFormAnswersAsync(formId);
                var summaries = await _answerService.GetAnswerSummariesAsync(formId);

                var viewModel = new ResultsVM
                {
                    Answers = answers,
                    Summaries = summaries
                };

                ViewBag.SelectedFormId = formId;
                var forms = await _formService.GetAllFormsAsync();
                ViewBag.Forms = new SelectList(forms, "Id", "Title", formId);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewResults: {ex.Message}");
                return View(new ResultsVM { Answers = new List<AnswerVM>(), Summaries = new Dictionary<int, AnswerSummaryVM>() });
            }
        }
    }
}
