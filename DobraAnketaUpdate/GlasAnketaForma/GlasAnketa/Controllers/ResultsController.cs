using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlasAnketa.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IQuestionFormService _formService;
        private readonly IAnswerService _answerService;
        private readonly IUserService _userService;

        public ResultsController(IQuestionService questionService,
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

        [HttpGet]
        public async Task<IActionResult> OU2Analytics(int formId = 1)
        {
            try
            {
                var analyticsData = await _answerService.GetOU2AnalyticsAsync(formId);
                
                ViewBag.SelectedFormId = formId;
                var forms = await _formService.GetAllFormsAsync();
                ViewBag.Forms = new SelectList(forms, "Id", "Title", formId);

                return View(analyticsData);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading OU2 analytics: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SatisfactionComparison(int questionId = 1)
        {
            try
            {
                var comparisonData = await _answerService.GetSatisfactionComparisonAsync(questionId);
                
                // Get all scale questions for the dropdown
                var forms = await _formService.GetAllFormsAsync();
                var allQuestions = new List<QuestionVM>();
                foreach (var form in forms)
                {
                    var scaleQuestions = form.Questions.Where(q => q.QuestionType == "Scale").ToList();
                    allQuestions.AddRange(scaleQuestions);
                }

                ViewBag.SelectedQuestionId = questionId;
                ViewBag.Questions = new SelectList(allQuestions, "Id", "Text", questionId);

                comparisonData.AvailableQuestions = allQuestions;
                return View(comparisonData);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading satisfaction comparison: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> QuestionComparison(int formId = 1)
        {
            try
            {
                var comparisonData = await _answerService.GetQuestionComparisonAsync(formId);

                ViewBag.SelectedFormId = formId;
                var forms = await _formService.GetAllFormsAsync();
                ViewBag.Forms = new SelectList(forms, "Id", "Title", formId);

                return View(comparisonData);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading question comparison: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> OU2Comparison(int questionId = 1)
        {
            try
            {
                var question = await _questionService.GetQuestionById(questionId);
                var comparisonData = await _answerService.GetSatisfactionComparisonAsync(questionId);

                // Group satisfaction data by OU2 for better visualization
                var ou2Groups = comparisonData.UserSatisfactions
                    .Where(u => u.HasResponded && u.ScaleValue.HasValue)
                    .GroupBy(u => u.OU2)
                    .Select(g => new OU2ScoreVM
                    {
                        OU2Name = g.Key,
                        AverageScore = g.Average(u => u.ScaleValue.Value),
                        ResponseCount = g.Count(),
                        AllScores = g.Select(u => u.ScaleValue.Value).OrderBy(s => s).ToList()
                    })
                    .OrderByDescending(g => g.AverageScore)
                    .ToList();

                // Calculate median for each OU2
                foreach (var ou2Score in ou2Groups)
                {
                    ou2Score.MedianScore = CalculateMedian(ou2Score.AllScores);
                }

                var overallAverage = ou2Groups.Any() ? ou2Groups.Average(g => g.AverageScore) : 0;
                var standardDeviation = CalculateStandardDeviation(ou2Groups.Select(g => g.AverageScore).ToList());

                var ou2ComparisonModel = new OU2ComparisonVM
                {
                    QuestionText = question?.Text ?? "Unknown Question",
                    QuestionId = questionId,
                    QuestionType = "Scale",
                    OU2Scores = ou2Groups,
                    OverallAverage = overallAverage,
                    StandardDeviation = standardDeviation
                };

                // Get all scale questions for the dropdown
                var forms = await _formService.GetAllFormsAsync();
                var allQuestions = new List<QuestionVM>();
                foreach (var form in forms)
                {
                    var scaleQuestions = form.Questions.Where(q => q.QuestionType == "Scale").ToList();
                    allQuestions.AddRange(scaleQuestions);
                }

                ViewBag.SelectedQuestionId = questionId;
                ViewBag.Questions = new SelectList(allQuestions, "Id", "Text", questionId);

                return View(ou2ComparisonModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading OU2 comparison: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportResults(int formId, string format = "csv")
        {
            try
            {
                var analyticsData = await _answerService.GetOU2AnalyticsAsync(formId);
                
                if (format.ToLower() == "csv")
                {
                    var csvContent = GenerateCSVReport(analyticsData);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
                    return File(bytes, "text/csv", $"survey_results_form_{formId}_{DateTime.Now:yyyyMMdd}.csv");
                }

                return RedirectToAction("OU2Analytics", new { formId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error exporting results: {ex.Message}";
                return RedirectToAction("OU2Analytics", new { formId });
            }
        }

        private string GenerateCSVReport(OU2AnalyticsVM analyticsData)
        {
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("OU2,Question,Question Type,Average Score,Response Count,Response Rate %");

            foreach (var ou2Result in analyticsData.OU2Results)
            {
                foreach (var questionResult in ou2Result.QuestionResults)
                {
                    csv.AppendLine($"\"{ou2Result.OU2Name}\",\"{questionResult.QuestionText}\",\"{questionResult.QuestionType}\",{questionResult.AverageScore:F2},{questionResult.ResponseCount},{ou2Result.ResponseRate:F1}");
                }
            }

            return csv.ToString();
        }

        private double CalculateMedian(List<int> values)
        {
            if (!values.Any()) return 0;
            
            var sortedValues = values.OrderBy(x => x).ToList();
            var count = sortedValues.Count();
            if (count % 2 == 0)
            {
                return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2.0;
            }
            return sortedValues[count / 2];
        }

        private double CalculateStandardDeviation(List<double> values)
        {
            if (!values.Any()) return 0;
            var average = values.Average();
            var sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / values.Count);
        }
    }
}