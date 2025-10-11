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
            _questionRepository.InsertQuestion(question);
        }

        public int RemoveQuestion(int questionId)
        {
            _questionRepository.DeleteQuestion(questionId);
            return questionId;
        }

        public QuestionVM GetQuestionById(int questionId)
        {
            var question = _questionRepository.GetQuestionById(questionId);
            return _mapper.Map<QuestionVM>(question);
        }
        public List<QuestionVM> GetAllQuestions()
        {
            Question questions = _questionRepository.GetAllQuestions(); // You'll need to implement this
            return _mapper.Map<List<QuestionVM>>(questions);
        }
        public void UpdateQuestion(RegisterQuestionVM registerQuestionVM)
        {
            var question = _mapper.Map<Question>(registerQuestionVM);
            _questionRepository.UpdateQuestion(question);
        }
    }
}
