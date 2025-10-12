using AutoMapper;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
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

        public void CreateQuestion(RegisterQuestionVM registerQuestionVM)
        {
            var question = _mapper.Map<Question>(registerQuestionVM);
            _questionRepository.AddAsync(question);
        }

        public async Task<List<QuestionVM>> GetAllQuestions()
        {
            var questions = await _questionRepository.GetAllAsync();
            return _mapper.Map<List<QuestionVM>>(questions);
        }

        public async Task<QuestionVM> GetQuestionById(int questionId)
        {
            var question = await _questionRepository.GetActiveAsync(questionId);
            return _mapper.Map<QuestionVM>(question);
        }

        public async Task<bool> RemoveQuestion(int questionId)
        {
            try
            {
                var question = await _questionRepository.GetActiveAsync(questionId);
                if (question == null)
                    return false;

                await _questionRepository.Remove(question);
                return true;
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return false;
            }
        }

       public void UpdateQuestion(RegisterQuestionVM registerQuestionVM)
        {
            var question = _mapper.Map<Question>(registerQuestionVM);
            _questionRepository.Update(question);
        }
    }
}
