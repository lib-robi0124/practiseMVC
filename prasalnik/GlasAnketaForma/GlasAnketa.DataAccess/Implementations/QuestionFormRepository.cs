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
            throw new NotImplementedException();
        }

        public QuestionForm GetQuestionFormById(int id)
        {
            throw new NotImplementedException();
        }

        public int InsertQuestionForm(QuestionForm questionForm)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestionForm(QuestionForm questionForm)
        {
            throw new NotImplementedException();
        }
    }
}
