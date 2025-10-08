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
            throw new NotImplementedException();
        }

        public int DeleteQuestionAsync(int questionId)
        {
            throw new NotImplementedException();
        }

        public List<QuestionVM> GetAllQuestions()
        {
            throw new NotImplementedException();
        }

        public QuestionVM GetQuestionById(int questionId)
        {
            throw new NotImplementedException();
        }

        public void UpdateQuestion(RegisterQuestionVM registerQuestionVM)
        {
            throw new NotImplementedException();
        }
    }
}
