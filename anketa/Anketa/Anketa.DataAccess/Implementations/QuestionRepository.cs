using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Anketa.DataAccess.Implementations
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuestionForm>> GetActiveFormsAsync()
        {
            return await _context.QuestionForms
                .Where(f => f.IsActive)
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }
       
        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _context.QuestionForms
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.QuestionForm)
                .Include(q => q.User)
                .OrderBy(q => q.QuestionFormId)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }
        // New method for admin to create questions
        public async Task<Question> CreateQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            return question;
        }

        // New method to get questions by form
        public async Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId)
        {
            return await _context.Questions
                .Where(q => q.QuestionFormId == formId)
                .Include(q => q.QuestionType)
                .ToListAsync();
        }
    }
}
