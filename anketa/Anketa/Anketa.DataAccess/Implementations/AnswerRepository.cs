using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Anketa.DataAccess.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context) : base(context)
        {
            _context = context;
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

                    if (question == null) continue;

                    var existingAnswer = await _context.Answers
                        .FirstOrDefaultAsync(a => a.UserId == userId &&
                                                a.QuestionId == answer.Key &&
                                                a.QuestionFormId == formId);

                    if (existingAnswer != null)
                    {
                        // Update existing answer
                        UpdateAnswerValue(existingAnswer, question.QuestionType.Name, answer.Value);
                        Update(existingAnswer);
                    }
                    else
                    {
                        // Create new answer
                        var newAnswer = new Answer
                        {
                            UserId = userId,
                            QuestionId = answer.Key,
                            QuestionFormId = formId,
                            AnsweredDate = DateTime.UtcNow
                        };
                        UpdateAnswerValue(newAnswer, question.QuestionType.Name, answer.Value);
                        await AddAsync(newAnswer);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
     private void UpdateAnswerValue(Answer answer, string questionType, object value)
        {
            if (questionType == "Scale" && value is int scaleValue)
            {
                answer.ScaleValue = scaleValue;
                answer.TextValue = null;
            }
            else if (questionType == "Text" && value is string textValue)
            {
                answer.TextValue = textValue;
                answer.ScaleValue = null;
            }
        }

        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _context.Answers
                .Where(a => a.UserId == userId && a.QuestionFormId == formId)
                .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }

        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _context.Answers
                .AnyAsync(a => a.UserId == userId && a.QuestionFormId == formId);
        }
        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _context.Answers
                .Where(a => a.QuestionFormId == formId)
                .Include(a => a.Question)
                .Include(a => a.User)
                .ToListAsync();
        }
    }
}
