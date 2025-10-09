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
            var questionVMs = new List<QuestionVM>();
            await _questionRepository.GetAllAsync();
            return questionVMs;
        }

        public async Task<QuestionVM> GetQuestionById(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            return _mapper.Map<QuestionVM>(question);
        }
    }
}
