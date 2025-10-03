using Anketa.Domain.Models;

namespace Anketa.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionForm>> GetActiveFormsAsync();
        Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task<IEnumerable<Question>> GetAllQuestionsAsync();
        // Admin methods
        Task<Question> CreateQuestionAsync(Question question);
        Task<IEnumerable<Question>> GetQuestionsByFormAsync(int formId);
    }
}
