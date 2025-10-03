using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Anketa.Services.Interfaces;

namespace Anketa.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            return await _answerRepository.SubmitAnswersAsync(userId, formId, answers);
        }

        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _answerRepository.GetUserAnswersAsync(userId, formId);
        }
        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _answerRepository.HasUserCompletedFormAsync(userId, formId);
        }

        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _answerRepository.GetFormAnswersAsync(formId);
        }
    }
}
