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
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Submit(int formId, IFormCollection form)
        //{
        //    Console.WriteLine($"=== FORM SUBMISSION STARTED ===");
        //    Console.WriteLine($"Form ID: {formId}");
        //    Console.WriteLine($"Form keys: {string.Join(", ", form.Keys)}");

        //    if (form == null || !form.Any())
        //    {
        //        Console.WriteLine("No form data received");
        //        TempData["Error"] = "No answers provided.";
        //        return RedirectToAction("Answer", "Form", new { formId });
        //    }

        //    try
        //    {
        //        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        //        Console.WriteLine($"User ID: {userId}");

        //        // ✅ FIXED: Properly parse form data
        //        var answers = ParseFormAnswers(form);
        //        Console.WriteLine($"Parsed {answers.Count} answers: {string.Join(", ", answers.Select(a => $"{a.Key}: {a.Value}"))}");

        //        if (!answers.Any())
        //        {
        //            Console.WriteLine("No valid answers after parsing");
        //            TempData["Error"] = "No valid answers provided.";
        //            return RedirectToAction("Answer", "Form", new { formId });
        //        }

        //        var success = await _answerService.SubmitAnswersAsync(userId, formId, answers);
        //        Console.WriteLine($"Submit result: {success}");

        //        if (success)
        //        {
        //            TempData["Success"] = "Thank you for submitting your answers!";
        //            return RedirectToAction("Thanks", "Form");
        //        }

        //        TempData["Error"] = "Error submitting your answers. Please try again.";
        //        return RedirectToAction("Answer", "Form", new { formId });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging
        //        Console.WriteLine($"Error submitting answers: {ex.Message}");
        //        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        //        TempData["Error"] = "An error occurred while submitting your answers.";
        //        return RedirectToAction("Answer", "Form", new { formId });
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(IFormCollection form)
        {
            Console.WriteLine($"=== SERVER: Form submission started ===");

            try
            {
                // Extract formId from form collection
                if (!int.TryParse(form["formId"], out int formId))
                {
                    Console.WriteLine("SERVER: Invalid formId");
                    TempData["Error"] = "Invalid form data.";
                    return RedirectToAction("Index", "Form");
                }

                Console.WriteLine($"SERVER: Form ID: {formId}");

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                Console.WriteLine($"SERVER: User ID: {userId}");

                // ✅ FIXED: Parse form data properly
                var answers = new Dictionary<int, object>();

                Console.WriteLine("SERVER: Processing form data:");
                foreach (var key in form.Keys)
                {
                    Console.WriteLine($"  Key: {key}, Value: {form[key]}");

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
                                    Console.WriteLine($"  → SCALE ANSWER: Q{questionId} = {scaleValue}");
                                }
                                else
                                {
                                    answers[questionId] = value;
                                    Console.WriteLine($"  → TEXT ANSWER: Q{questionId} = {value}");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"SERVER: Total answers parsed: {answers.Count}");

                if (!answers.Any())
                {
                    Console.WriteLine("SERVER: No valid answers found");
                    TempData["Error"] = "No valid answers provided.";
                    return RedirectToAction("Answer", "Form", new { formId });
                }

                var success = await _answerService.SubmitAnswersAsync(userId, formId, answers);

                Console.WriteLine($"SERVER: Submit result: {success}");

                if (success)
                {
                    Console.WriteLine($"SERVER: Answers submitted successfully!");
                    TempData["Success"] = "Thank you for submitting your answers!";
                    return RedirectToAction("Thanks", "Form");
                }

                Console.WriteLine($"SERVER: Database submission failed");
                TempData["Error"] = "Error submitting your answers. Please try again.";
                return RedirectToAction("Answer", "Form", new { formId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SERVER EXCEPTION: {ex.Message}");
                Console.WriteLine($"SERVER STACK TRACE: {ex.StackTrace}");

                TempData["Error"] = "An error occurred while submitting your answers.";
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