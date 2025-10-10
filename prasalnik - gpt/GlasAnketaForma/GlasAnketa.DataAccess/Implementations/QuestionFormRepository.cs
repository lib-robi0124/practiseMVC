using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionFormRepository : Repository<QuestionForm>, IQuestionFormRepository
    {
        
        public QuestionFormRepository(AppDbContext context) : base(context) { }
        public async Task<List<QuestionForm>> GetAllFormQuestionsAsync()
        {
            return await _context.QuestionForms
                        .Include(f => f.Questions)
                        .ThenInclude(q => q.QuestionType)
                        .ToListAsync();
        }
        public async Task<QuestionForm> GetNextActiveFormAsync(int currentFormId)
        {
            return await _context.QuestionForms
                        .Where(f => f.IsActive && f.Id > currentFormId)
                        .Include(f => f.Questions)
                        .ThenInclude(q => q.QuestionType)
                        .OrderBy(f => f.Id)
                        .FirstOrDefaultAsync();
        }
        public async Task<QuestionForm> GetQuestionFormByIdAsync(int id)
        {
            return await _context.QuestionForms
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
