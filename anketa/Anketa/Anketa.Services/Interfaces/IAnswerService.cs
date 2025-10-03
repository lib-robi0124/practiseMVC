using Anketa.Domain.Models;

namespace Anketa.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers);
        Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId);
        Task<bool> HasUserCompletedFormAsync(int userId, int formId);
        Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId);
    }
}
