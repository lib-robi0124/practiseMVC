using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<int> InsertAnswersAsync(List<Answer> answers)
        {
            if (answers == null || !answers.Any())
            {
                Console.WriteLine("InsertAnswersAsync: No answers to insert");
                return 0;
            }

            try
            {
                Console.WriteLine($"InsertAnswersAsync: Starting to insert {answers.Count} answers");

                // Log each answer before saving
                foreach (var answer in answers)
                {
                    Console.WriteLine($"  Answer to insert - User{answer.UserId}, Q{answer.QuestionId}, Form{answer.QuestionFormId}, Scale: {answer.ScaleValue}, Text: '{answer.TextValue}'");
                }

                await _appDbContext.Answers.AddRangeAsync(answers);
                Console.WriteLine("Entities added to context, calling SaveChangesAsync...");

                var result = await _appDbContext.SaveChangesAsync();

                Console.WriteLine($"SaveChangesAsync completed. Rows affected: {result}");

                // Verify the answers were saved
                if (result > 0)
                {
                    var savedAnswers = _appDbContext.Answers
                        .Where(a => a.UserId == answers.First().UserId && a.QuestionFormId == answers.First().QuestionFormId)
                        .ToList();
                    Console.WriteLine($"Verified: {savedAnswers.Count} answers in database for this user/form");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR in InsertAnswersAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<bool> UpdateAnswerAsync(Answer answer)
        {
            var existingAnswer = await _appDbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == answer.Id);

            if (existingAnswer == null)
                return false;

            _appDbContext.Entry(existingAnswer).CurrentValues.SetValues(answer);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAnswerAsync(int answerId)
        {
            var answer = await _appDbContext.Answers
                .FirstOrDefaultAsync(a => a.Id == answerId);

            if (answer == null)
                return false;

            _appDbContext.Answers.Remove(answer);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        // Get operations with includes for related data
        public async Task<Answer> GetAnswerByIdAsync(int answerId)
        {
            return await _appDbContext.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                    .ThenInclude(q => q.QuestionType)
                .Include(a => a.QuestionForm)
                .FirstOrDefaultAsync(a => a.Id == answerId);
        }

        public async Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId)
        {
            return await _appDbContext.Answers
                .Where(a => a.QuestionFormId == questionFormId)
                .Include(a => a.User)
                .Include(a => a.Question)
                    .ThenInclude(q => q.QuestionType)
                .OrderByDescending(a => a.AnsweredDate)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetAnswersByUserIdAsync(int userId)
        {
            return await _appDbContext.Answers
                .Where(a => a.UserId == userId)
                .Include(a => a.Question)
                    .ThenInclude(q => q.QuestionType)
                .Include(a => a.QuestionForm)
                .OrderByDescending(a => a.AnsweredDate)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _appDbContext.Answers
                .Where(a => a.QuestionId == questionId)
                .Include(a => a.User)
                .Include(a => a.QuestionForm)
                .OrderByDescending(a => a.AnsweredDate)
                .ToListAsync();
        }

        public async Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId)
        {
            return await _appDbContext.Answers
                .FirstOrDefaultAsync(a => a.UserId == userId &&
                                        a.QuestionId == questionId &&
                                        a.QuestionFormId == questionFormId);
        }

        // Check operations
        public async Task<bool> HasUserSubmittedFormAsync(int userId, int questionFormId)
        {
            return await _appDbContext.Answers
                .AnyAsync(a => a.UserId == userId && a.QuestionFormId == questionFormId);
        }

        public async Task<bool> HasUserAnsweredQuestionAsync(int userId, int questionId)
        {
            return await _appDbContext.Answers
                .AnyAsync(a => a.UserId == userId && a.QuestionId == questionId);
        }

        // Analytics and summaries
        public async Task<Dictionary<int, AnswerSummary>> GetAnswerSummariesAsync(int formId)
        {
            var summaries = new Dictionary<int, AnswerSummary>();

            // Get all questions for this form
            var questions = await _appDbContext.Questions
                .Where(q => q.QuestionFormId == formId)
                .Include(q => q.QuestionType)
                .ToListAsync();

            foreach (var question in questions)
            {
                var answers = await _appDbContext.Answers
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

        public async Task<List<Answer>> GetFormAnswersWithDetailsAsync(int formId)
        {
            return await _appDbContext.Answers
                .Where(a => a.QuestionFormId == formId)
                .Include(a => a.User)
                .Include(a => a.Question)
                    .ThenInclude(q => q.QuestionType)
                .OrderBy(a => a.User.OU)
                .ThenBy(a => a.User.FullName)
                .ThenBy(a => a.QuestionId)
                .ToListAsync();
        }

        public async Task<int> GetFormResponseCountAsync(int formId)
        {
            return await _appDbContext.Answers
                .Where(a => a.QuestionFormId == formId)
                .Select(a => a.UserId)
                .Distinct()
                .CountAsync();
        }

        // Bulk operations
        public async Task<int> GetUserAnswerCountAsync(int userId)
        {
            return await _appDbContext.Answers
                .Where(a => a.UserId == userId)
                .CountAsync();
        }

        public async Task<List<Answer>> GetRecentAnswersAsync(int count = 50)
        {
            return await _appDbContext.Answers
                .Include(a => a.User)
                .Include(a => a.Question)
                .Include(a => a.QuestionForm)
                .OrderByDescending(a => a.AnsweredDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
