using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext context) : base(context)   {    }
        public async Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId)
        {
            return await _context.Answers
                        .Where(a => a.QuestionFormId == questionFormId)
                        .ToListAsync();
        }
        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _context.Answers
                        .Where(a => a.QuestionId == questionId)
                        .ToListAsync();
        }
        public async Task<List<Answer>> GetAnswersByUserIdAsync(int userId, int formId1)
        {
            return await _context.Answers
                        .Where(a => a.UserId == userId)
                        .ToListAsync();
        }
        public async Task<Dictionary<int, AnswerSummary>> GetAnswerSummariesAsync(int formId)
        {
            var summaries = new Dictionary<int, AnswerSummary>();

            // Get all questions for this form
            var questions = await _context.Questions
                .Where(q => q.QuestionFormId == formId)
                .Include(q => q.QuestionType)
                .ToListAsync();

            foreach (var question in questions)
            {
                var answers = await _context.Answers
                    .Where(a => a.QuestionId == question.Id && a.QuestionFormId == formId)
                    .ToListAsync();

                var summary = new AnswerSummary
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text,
                    QuestionType = question.QuestionType.Name,
                    TotalResponses = answers.Count,
                    TextResponses = answers.Where(a => !string.IsNullOrEmpty(a.TextValue))
                                        .Select(a => a.TextValue)
                                        .ToList()
                };
                // Calculate average for scale questions
                if (question.QuestionType.Name == "Scale" && answers.Any())
                {
                    var scaleAnswers = answers.Where(a => a.ScaleValue.HasValue)
                                            .Select(a => a.ScaleValue.Value)
                                            .ToList();
                    summary.AverageScaleValue = scaleAnswers.Average();
                    summary.ScaleValueDistribution = scaleAnswers
                        .GroupBy(v => v)
                        .ToDictionary(g => g.Key, g => g.Count());
                }
                summaries.Add(question.Id, summary);
            }
            return summaries;
        }
        public async Task<Answer?> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId)
        {
            return await _context.Answers
                    .FirstOrDefaultAsync(a => a.UserId == userId &&
                                              a.QuestionId == questionId &&
                                              a.QuestionFormId == questionFormId);
        }
        public async Task SaveAnswersAsync(List<Answer> answers)
        {
            foreach (var ans in answers)
            {
                var existing = await _context.Answers
                    .FirstOrDefaultAsync(a => a.UserId == ans.UserId &&
                                              a.QuestionId == ans.QuestionId &&
                                              a.QuestionFormId == ans.QuestionFormId);

                if (existing == null)
                {
                    _context.Answers.Add(ans);
                }
                else
                {
                    existing.TextValue = ans.TextValue;
                    existing.ScaleValue = ans.ScaleValue;
                    existing.AnsweredDate = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Resolve company once per submission
            var companyId = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.CompanyId)
                .FirstOrDefaultAsync();

            foreach (var (questionId, value) in answers)
            {
                var question = await _context.Questions
                    .Include(q => q.QuestionType)
                    .FirstOrDefaultAsync(q => q.Id == questionId);

                if (question == null) continue;

                // Always create a new answer record (keep history)
                var newAnswer = new Answer
                {
                    UserId = userId,
                    CompanyId = companyId,
                    QuestionId = questionId,
                    QuestionFormId = formId,
                    AnsweredDate = DateTime.UtcNow
                };

                UpdateAnswerValue(newAnswer, question.QuestionType.Name, value);
                _context.Answers.Add(newAnswer);
            }

            var result = await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result > 0;
        }
        private void UpdateAnswerValue(Answer answer, string questionType, object value)
        {
            if (questionType == "Scale")
            {
                if (value == null)
                {
                    // Explicitly clear any previous value if user left it blank this time
                    answer.ScaleValue = null;
                    answer.TextValue = null;
                    return;
                }

                if (value is int scaleValue)
                {
                    answer.ScaleValue = scaleValue;
                    answer.TextValue = null;
                    return;
                }
                if (value is string stringValue && int.TryParse(stringValue, out int parsedValue))
                {
                    answer.ScaleValue = parsedValue;
                    answer.TextValue = null;
                    return;
                }

                // Fallback: clear if unusable value
                answer.ScaleValue = null;
                answer.TextValue = null;
            }
            else if (questionType == "Text")
            {
                if (value == null)
                {
                    answer.TextValue = null;
                    answer.ScaleValue = null;
                    return;
                }

                var text = value.ToString();
                if (string.IsNullOrWhiteSpace(text))
                {
                    answer.TextValue = null;
                    answer.ScaleValue = null;
                }
                else
                {
                    answer.TextValue = text;
                    answer.ScaleValue = null;
                }
            }
        }

        public async Task<List<Answer>> GetAnswersWithUsersByFormIdAsync(int formId)
        {
            return await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
                .Where(a => a.QuestionFormId == formId)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetAnswersWithUsersByQuestionIdAsync(int questionId)
        {
            return await _context.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
                .Where(a => a.QuestionId == questionId)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctOU2ValuesAsync()
        {
            return await _context.Users
                .Select(u => u.OU2)
                .Distinct()
                .OrderBy(ou2 => ou2)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetUserCountsByOU2Async()
        {
            return await _context.Users
                .GroupBy(u => u.OU2)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
    }
}
