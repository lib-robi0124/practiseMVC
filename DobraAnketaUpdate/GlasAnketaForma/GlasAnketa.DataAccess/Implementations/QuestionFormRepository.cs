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
                        .OrderBy(f => f.Id)
                        .ToListAsync();
        }
        public async Task<QuestionForm?> GetNextActiveFormAsync(int currentFormId)
        {
            // Find the next active form strictly after the current form by Id.
            // Do NOT wrap around; if none exists, return null (end of sequence).
            var nextForm = await _context.QuestionForms
                .AsNoTracking()
                .Where(f => f.IsActive && f.Id > currentFormId)
                .OrderBy(f => f.Id)
                .Include(f => f.Questions)
                    .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync();

            return nextForm; // will be null if we are at the last active form
        }
        public async Task<QuestionForm> GetQuestionFormByIdAsync(int id)
        {
            return await _context.QuestionForms
                .Include(f => f.Questions)
                .ThenInclude(q => q.QuestionType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ToggleFormStatusAsync(int formId, bool isActive)
        {
            var form = await _context.QuestionForms
                .FirstOrDefaultAsync(qf => qf.Id == formId);

            if (form == null)
                return false;

            form.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

