using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        // submit Answer to database
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        // Get Operations
        Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId);
        Task<List<Answer>> GetAnswersByUserIdAsync(int userId);
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId);
    }
}
