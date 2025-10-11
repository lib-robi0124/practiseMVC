using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IAnswerService
    {
        // Answer submission
        Task<AnswerSubmissionResult> SubmitAnswersAsync(List<AnswerVM> answers, int userId);
        //Task<bool> SubmitSingleAnswerAsync(List<AnswerVM> answer, int userId);

        // Retrieval methods
        Task<List<AnswerVM>> GetUserAnswersAsync(int userId);
        Task<List<AnswerVM>> GetFormAnswersAsync(int formId);
        Task<List<AnswerVM>> GetQuestionAnswersAsync(int questionId);
        Task<AnswerVM> GetAnswerByIdAsync(int answerId);

        // User-specific queries
        Task<List<UserFormAnswersVM>> GetUserFormAnswersAsync(int userId, int formId);
        Task<bool> HasUserSubmittedFormAsync(int userId, int formId);
        Task<DateTime?> GetUserLastSubmissionDateAsync(int userId, int formId);

        // Analytics and reporting
        Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId);
        Task<List<AnswerVM>> GetRecentAnswersAsync(int count = 50);

        // Admin operations
        Task<bool> DeleteAnswerAsync(int answerId);
        Task<bool> UpdateAnswerAsync(AnswerVM answer);
        Task<int> GetFormResponseCountAsync(int formId);
    }
}
