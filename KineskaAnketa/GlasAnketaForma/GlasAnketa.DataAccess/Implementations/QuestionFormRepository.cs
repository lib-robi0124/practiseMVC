using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionFormRepository : BaseRepository, IQuestionFormRepository
    {
        public QuestionFormRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public void DeleteQuestionForm(int id)
        {
            var questionForm = _appDbContext.QuestionForms.FirstOrDefault(c => c.Id == id);
            if (questionForm is null)
            {
                throw new Exception($"QuestionForm with id {id} not found.");
            }
            _appDbContext.QuestionForms.Remove(questionForm);
            _appDbContext.SaveChanges();
        }

        public List<QuestionForm> GetAllFormQuestions()
        {
            return _appDbContext.QuestionForms
                .Include(qf => qf.Questions)
                    .ThenInclude(q => q.QuestionType)
                .Include(qf => qf.Questions)
                    .ThenInclude(q => q.User)
                .ToList();
        }

        public QuestionForm GetQuestionFormById(int id)
        {
            return _appDbContext.QuestionForms
                .FirstOrDefault(q => q.Id == id);
        }

        public int InsertQuestionForm(QuestionForm questionForm)
        {
            _appDbContext.QuestionForms.Add(questionForm);
            _appDbContext.SaveChanges();
            return questionForm.Id;
        }

        public void UpdateQuestionForm(QuestionForm questionForm)
        {
            if (!_appDbContext.QuestionForms.Any(x => x.Id == questionForm.Id))
            {
                throw new Exception($"QuestionForm with id {questionForm.Id} was not found");
            }
            _appDbContext.QuestionForms.Update(questionForm);
            _appDbContext.SaveChanges();
        }

        // New implementations for service methods
        public List<QuestionForm> GetActiveForms()
        {
            return _appDbContext.QuestionForms
                .Where(qf => qf.IsActive)
                .Include(qf => qf.Questions)
                .OrderBy(qf => qf.Id)
                .ToList();
        }

        public QuestionForm GetFormWithQuestions(int formId)
        {
            try
            {
                Console.WriteLine($"GetFormWithQuestions called for form {formId}");

                var form = _appDbContext.QuestionForms
                    .Include(qf => qf.Questions)
                        .ThenInclude(q => q.QuestionType)
                    .Include(qf => qf.Questions)
                        .ThenInclude(q => q.User)
                    .FirstOrDefault(qf => qf.Id == formId);

                Console.WriteLine($"Database query completed - Form found: {form != null}");

                if (form != null)
                {
                    Console.WriteLine($"Form: {form.Title}, Questions count: {form.Questions?.Count}");
                }

                return form;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in GetFormWithQuestions: {ex.Message}");
                throw;
            }
        }

        public async Task<int> InsertFormAsync(QuestionForm questionForm)
        {
            _appDbContext.QuestionForms.Add(questionForm);
            await _appDbContext.SaveChangesAsync();
            return questionForm.Id;
        }

        public async Task<bool> UpdateFormAsync(QuestionForm questionForm)
        {
            var existingForm = await _appDbContext.QuestionForms
                .FirstOrDefaultAsync(qf => qf.Id == questionForm.Id);

            if (existingForm == null)
                return false;

            _appDbContext.Entry(existingForm).CurrentValues.SetValues(questionForm);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleFormStatusAsync(int formId, bool isActive)
        {
            var form = await _appDbContext.QuestionForms
                .FirstOrDefaultAsync(qf => qf.Id == formId);

            if (form == null)
                return false;

            form.IsActive = isActive;
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
