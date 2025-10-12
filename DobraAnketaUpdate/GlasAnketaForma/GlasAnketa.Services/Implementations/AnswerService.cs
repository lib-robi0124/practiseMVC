using AutoMapper;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<Dictionary<int, AnswerSummaryVM>> GetAnswerSummariesAsync(int formId)
        {
            var summaries = await _answerRepository.GetAnswerSummariesAsync(formId);
            return _mapper.Map<Dictionary<int, AnswerSummaryVM>>(summaries);
        }
        public async Task<List<AnswerVM>> GetFormAnswersAsync(int formId)
        {
            var answers = await _answerRepository.GetAnswersByQuestionFormIdAsync(formId);
            return _mapper.Map<List<AnswerVM>>(answers);
        }

        public async Task<List<AnswerVM>> GetUserAnswersAsync(int userId, int formId)
        {
            var answers = await _answerRepository.GetAnswersByUserIdAsync(userId, formId);
            return _mapper.Map<List<AnswerVM>>(answers);

        }

        public async Task SaveAnswersAsync(List<AnswerVM> answers)
        {
            var mappedAnswers = _mapper.Map<List<Answer>>(answers);
            await _answerRepository.SaveAnswersAsync(mappedAnswers);
        }

        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            return await _answerRepository.SubmitAnswersAsync(userId, formId, answers);
        }
    }
}
