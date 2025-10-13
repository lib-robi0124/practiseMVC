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
            // Ensure currentFormId stays in the hardcoded 1..13 range
            if (currentFormId < 1 || currentFormId > 13)
            {
                currentFormId = 1;
            }

            // Preload active forms within the hardcoded range to avoid multiple DB round-trips
            var activeForms = await _context.QuestionForms
                .AsNoTracking()
                .Where(f => f.IsActive && f.Id >= 1 && f.Id <= 13)
                .Include(f => f.Questions)
                    .ThenInclude(q => q.QuestionType)
                .ToListAsync();

            if (!activeForms.Any())
                return null; // none active in 1..13

            // Try next ids in sequence: currentFormId+1 .. 13, then wrap 1 .. currentFormId
            // Use modular arithmetic to produce the sequence of 12 possible "next" ids (or fewer if you prefer)
            for (int offset = 1; offset <= 12; offset++)
            {
                int candidateId = ((currentFormId - 1 + offset) % 13) + 1; // maps into 1..13
                var match = activeForms.FirstOrDefault(f => f.Id == candidateId);
                if (match != null)
                    return match;
            }

            // If we get here, there was no active form found in 1..13
            return null;
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

