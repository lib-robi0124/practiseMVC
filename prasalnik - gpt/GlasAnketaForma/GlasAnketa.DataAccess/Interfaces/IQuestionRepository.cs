using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IQuestionRepository : IRepository<Question>
    {
       Task<List<Question>> GetByUserIdAsync (int userId);
       Task<QuestionForm> GetFormWithQuestionsAsync(int formId);
    }
}
