using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Anketa.Services.Interfaces;

namespace Anketa.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            return await _questionRepository.CreateQuestionAsync(question);
        }

        public async Task<IEnumerable<QuestionForm>> GetActiveFormsAsync()
        {
            return await _questionRepository.GetActiveFormsAsync();
        }

        public async Task<IEnumerable<Question>> GetAllQuestionsAsync()
        {
            return await _questionRepository.GetAllQuestionsAsync();
        }

        public async Task<QuestionForm> GetFormWithQuestionsAsync(int formId)
        {
            return await _questionRepository.GetFormWithQuestionsAsync(formId);
        }
               
        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _questionRepository.GetQuestionByIdAsync(questionId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId)
        {
            return await _questionRepository.GetQuestionsByFormAsync(formId);
        }
    }
}
