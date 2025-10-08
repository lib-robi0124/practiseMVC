using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IAnswerRepository
    {
        List<Answer> GetAnswersByQuestionFormId(int questionFormId);
        List<Answer> GetAnswersByUserId(int userId);
        List<Answer> GetAnswersByQuestionId(int questionId);
        Answer GetAllAnswers(int userId, int questionId, int questionFormId);
        Task<int> InsertAnswerAsync(Answer answer);
    }
}
