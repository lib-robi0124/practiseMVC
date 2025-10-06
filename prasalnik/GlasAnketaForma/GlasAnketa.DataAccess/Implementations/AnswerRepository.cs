using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Implementations
{
    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(DataContext.AppDbContext appDbContext) : base(appDbContext)
        {
        }
        public List<Answer> GetAnswersByQuestionFormId(int questionFormId)
        {
            throw new NotImplementedException();
        }

        public List<Answer> GetAnswersByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            throw new NotImplementedException();
        }

        public Answer GetAllAnswers(int userId, int questionId, int questionFormId)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAnswerAsync(Answer answer)
        {
            throw new NotImplementedException();
        }
    }
}
