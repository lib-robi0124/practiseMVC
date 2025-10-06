using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionFormRepository : BaseRepository, IQuestionFormRepository
    {
        public QuestionFormRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

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

        public QuestionForm GetQuestionFormById(int id)
        {
            return _appDbContext.QuestionForms.FirstOrDefault(q => q.Id == id);
        }

        public int InsertQuestionForm(QuestionForm questionForm)
        {
            _appDbContext.QuestionForms.Add(questionForm);
            _appDbContext.SaveChanges();
            return questionForm.Id;
        }

        public void UpdateQuestionForm(QuestionForm questionForm)
        {
            if (!_appDbContext.QuestionForms
                 .Any(x => x.Id == questionForm.Id))
            {
                throw new Exception($"QuestionForm with id {questionForm.Id} was not found");
            }
            _appDbContext.QuestionForms.Update(questionForm);
            _appDbContext.SaveChanges();

        }
    }
}
