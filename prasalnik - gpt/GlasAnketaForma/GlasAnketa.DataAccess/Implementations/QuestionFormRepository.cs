using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionFormRepository : Repository<QuestionForm>, IQuestionFormRepository
    {
        private readonly AppDbContext _context;
        public QuestionFormRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<QuestionForm>> GetAllFormQuestionsAsync()
        {
            return await _context.QuestionForms
                        .Include(f => f.Questions)
                        .ThenInclude(q => q.QuestionType)
                        .ToListAsync();
        }
    }
}
