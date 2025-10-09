using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly AppDbContext _context;
        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Question> GetByUserIdAsync(int userId)
        {
            return await _context.Questions
                        .Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.UserId == userId);
        }
        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _context.QuestionForms
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(f => f.Id == formId);
        }
    }
}