using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Implementations
{
    public class QuestionRepository : BaseRepository, IQuestionRepository
    {
        public QuestionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public void DeleteQuestion(int id)
        {
            throw new NotImplementedException();
        }

        public Question GetQuestionById(int id)
        {
            throw new NotImplementedException();
        }

        public int InsertQuestion(Question question)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(Question question)
        {
            throw new NotImplementedException();
        }
    }
}
