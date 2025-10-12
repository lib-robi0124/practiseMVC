using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId);
        Task<List<Answer>> GetAnswersByUserIdAsync(int userId, int formId1);
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId);
        Task SaveAnswersAsync(List<Answer> answers);
        Task<Dictionary<int, AnswerSummary>> GetAnswerSummariesAsync(int formId);
    }
}
