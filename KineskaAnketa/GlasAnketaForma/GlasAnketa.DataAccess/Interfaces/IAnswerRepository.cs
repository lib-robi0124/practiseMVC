using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IAnswerRepository
    {
        // Single answer operations
        Task<int> InsertAnswersAsync(List<Answer> answers);
        Task<bool> UpdateAnswerAsync(Answer answer);
        Task<bool> DeleteAnswerAsync(int answerId);

        // Get operations
        Task<Answer> GetAnswerByIdAsync(int answerId);
        Task<List<Answer>> GetAnswersByQuestionFormIdAsync(int questionFormId);
        Task<List<Answer>> GetAnswersByUserIdAsync(int userId);
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        Task<Answer> GetUserAnswerForQuestionAsync(int userId, int questionId, int questionFormId);

        // Check operations
        Task<bool> HasUserSubmittedFormAsync(int userId, int questionFormId);
        Task<bool> HasUserAnsweredQuestionAsync(int userId, int questionId);

        // Analytics and summaries
        Task<Dictionary<int, AnswerSummary>> GetAnswerSummariesAsync(int formId);
        Task<List<Answer>> GetFormAnswersWithDetailsAsync(int formId);
        Task<int> GetFormResponseCountAsync(int formId);

        // Bulk operations
        Task<int> GetUserAnswerCountAsync(int userId);
        Task<List<Answer>> GetRecentAnswersAsync(int count = 50);
    }
}
