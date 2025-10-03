using Anketa.Domain.Models;

namespace Anketa.DataAccess.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
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
