using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionRepository : BaseRepository, IQuestionRepository
    {
        public QuestionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public void DeleteQuestion(int id)
        {
            var question = _appDbContext.Questions.FirstOrDefault(c => c.Id == id);
            if (question is null)
            {
                throw new Exception($"Question with id {id} not found.");
            }
            _appDbContext.Questions.Remove(question);
            _appDbContext.SaveChanges();
        }

        public Question GetQuestionById(int id)
        {
            return _appDbContext.Questions.Include(q => q.User)
                                          .Include(q => q.QuestionForm)
                                          .FirstOrDefault(q => q.Id == id);
        }

        public int InsertQuestion(Question question)
        {
            _appDbContext.Questions.Add(question);
            _appDbContext.SaveChanges();
            return question.Id;
        }

        public void UpdateQuestion(Question question)
        {
            if (!_appDbContext.Questions
                 .Any(x => x.Id == question.Id))
            {
                throw new Exception($"Question with id {question.Id} was not found");
            }
            _appDbContext.Questions.Update(question);
            _appDbContext.SaveChanges();
        }
    }
}