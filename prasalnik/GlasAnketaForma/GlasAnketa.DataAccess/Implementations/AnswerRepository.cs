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
            return _appDbContext.Answers
                                .Where(a => a.QuestionFormId == questionFormId)
                                .ToList();
        }

        public List<Answer> GetAnswersByUserId(int userId)
        {
            return _appDbContext.Answers
                                .Where(a => a.UserId == userId)
                                .ToList();
        }

        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            return _appDbContext.Answers
                                .Where(a => a.QuestionId == questionId)
                                .ToList();
        }

        public Answer GetAllAnswers(int userId, int questionId, int questionFormId)
        {
            return _appDbContext.Answers
                                .FirstOrDefault(a => a.UserId == userId && a.QuestionId == questionId && a.QuestionFormId == questionFormId);
        }

        public Task<int> InsertAnswerAsync(Answer answer)
        {
            //Answer provide from user in question form like scale value or text value, has to be submitted to database
            _appDbContext.Answers.Add(answer);
            return _appDbContext.SaveChangesAsync();
        }
    }
}
