using AutoMapper;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<List<QuestionVM>> GetAllQuestions()
        {
            var questions = await _questionRepository.GetAllAsync();
            return _mapper.Map<List<QuestionVM>>(questions);
        }

        public async Task<QuestionVM> GetQuestionById(int questionId)
        {
            var question = await _questionRepository.GetFormWithQuestionsAsync(questionId);
            return _mapper.Map<QuestionVM>(question);
        }
    }
}
