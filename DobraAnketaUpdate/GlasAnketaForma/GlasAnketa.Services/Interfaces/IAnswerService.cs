using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<List<AnswerVM>> GetUserAnswersAsync(int userId, int formId);
        Task<List<AnswerVM>> GetFormAnswersAsync(int formId);
        Task SaveAnswersAsync(List<AnswerVM> answers);
        Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId);
        Task<OU2AnalyticsVM> GetOU2AnalyticsAsync(int formId);
        Task<SatisfactionComparisonVM> GetSatisfactionComparisonAsync(int questionId);
        Task<QuestionComparisonVM> GetQuestionComparisonAsync(int formId);
    }
}
