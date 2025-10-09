using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GlasAnketa.DataAccess.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly AppDbContext _context;
        public AnswerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
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
        public async Task<List<Answer>> GetAnswersByUserIdAsync(int userId)
        {
            return await _context.Answers
                        .Where(a => a.UserId == userId)
                        .ToListAsync();
        }
        public async Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId)
        {
            return await _context.Answers
                    .FirstOrDefaultAsync(a => a.UserId == userId &&
                                              a.QuestionId == questionId &&
                                              a.QuestionFormId == questionFormId);
        }
        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
          using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var answer in answers)
                {
                         var question = await _context.Questions
                        .Include(q => q.QuestionType)
                        .FirstOrDefaultAsync(q => q.Id == answer.Key);
                    if (question == null)
                    {
                        continue;
                    }
                    // Check for existing answer
                    var existingAnswer = await _context.Answers
                        .FirstOrDefaultAsync(a => a.UserId == userId &&
                                                a.QuestionId == answer.Key &&
                                                a.QuestionFormId == formId);
                    if (existingAnswer != null)
                    {
                        UpdateAnswerValue(existingAnswer, question.QuestionType.Name, answer.Value);
                        Update(existingAnswer);
                    }
                    else
                    {
                        var userCompanyId = await _context.Users
                                        .Where(u => u.Id == userId)
                                        .Select(u => u.CompanyId)
                                        .FirstOrDefaultAsync();
                        var newAnswer = new Answer
                        {
                            UserId = userId,
                            CompanyId = userCompanyId,
                            QuestionId = answer.Key,
                            QuestionFormId = formId,
                            AnsweredDate = DateTime.UtcNow
                        };
                        UpdateAnswerValue(newAnswer, question.QuestionType.Name, answer.Value);
                        await AddAsync(newAnswer);
                    }
                }

                var saveResult = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return saveResult > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log inner exception if exists
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        private void UpdateAnswerValue(Answer answer, string questionType, object value)
        {
            if (questionType == "Scale")
            {
                if (value is int scaleValue)
                {
                    answer.ScaleValue = scaleValue;
                    answer.TextValue = null;
                }
                else if (value is string stringValue && int.TryParse(stringValue, out int parsedValue))
                {
                    answer.ScaleValue = parsedValue;
                    answer.TextValue = null;
                }
                else
                {
                    Console.WriteLine($"❌ Invalid scale value: {value} (Type: {value.GetType()})");
                }
            }
            else if (questionType == "Text")
            {
                answer.TextValue = value?.ToString();
                answer.ScaleValue = null;
            }
            else
            {
                Console.WriteLine($"❌ Unknown question type: {questionType}");
            }
        }
    }
}
