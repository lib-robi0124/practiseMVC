using Anketa.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Anketa.Controllers
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
        public async Task<IActionResult> Submit(IFormCollection form)
        {

            try
            {
                // Extract formId from form collection
                if (!int.TryParse(form["formId"], out int formId))
                {
                    return RedirectToAction("Index", "Form");
                }


                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                // ✅ FIXED: Parse form data properly
                var answers = new Dictionary<int, object>();

                foreach (var key in form.Keys)
                {

                    if (key.StartsWith("answers["))
                    {
                        // Extract question ID from "answers[123]"
                        var questionIdStr = key.Replace("answers[", "").Replace("]", "");
                        if (int.TryParse(questionIdStr, out int questionId))
                        {
                            var value = form[key].ToString();

                            if (!string.IsNullOrEmpty(value))
                            {
                                // For scale questions, convert to int
                                if (int.TryParse(value, out int scaleValue))
                                {
                                    answers[questionId] = scaleValue;
                                }
                                else
                                {
                                    answers[questionId] = value;
                                }
                            }
                        }
                    }
                }


                if (!answers.Any())
                {
                    return RedirectToAction("Answer", "Form", new { formId });
                }

                var success = await _answerService.SubmitAnswersAsync(userId, formId, answers);


                if (success)
                {
                    return RedirectToAction("Thanks", "Form");
                }

                return RedirectToAction("Answer", "Form", new { formId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Form");
            }
        }
        // ✅ NEW: Parse form collection to dictionary with proper types
        private Dictionary<int, object> ParseFormAnswers(IFormCollection form)
        {
            var answers = new Dictionary<int, object>();

            foreach (var key in form.Keys)
            {
                // Look for keys that start with "answers[" 
                if (key.StartsWith("answers[") && key.EndsWith("]"))
                {
                    // Extract question ID from key name "answers[123]"
                    var questionIdStr = key.Substring(8, key.Length - 9); // Remove "answers[" and "]"

                    if (int.TryParse(questionIdStr, out int questionId))
                    {
                        var value = form[key].ToString();

                        if (!string.IsNullOrEmpty(value))
                        {
                            // ✅ Determine if this is a scale or text answer
                            if (IsScaleAnswer(questionId))
                            {
                                if (int.TryParse(value, out int scaleValue))
                                {
                                    answers[questionId] = scaleValue;
                                }
                            }
                            else
                            {
                                answers[questionId] = value;
                            }
                        }
                    }
                }
            }

            return answers;
        }

        // ✅ Helper method to determine answer type (you might want to cache this)
        private bool IsScaleAnswer(int questionId)
        {
            // You could query the database here, but for performance, 
            // we'll assume scale questions have IDs 1-33 and text questions 34-36
            // Alternatively, modify your view model to include this info
            return questionId <= 33; // Adjust based on your actual question IDs
        }
    }
}