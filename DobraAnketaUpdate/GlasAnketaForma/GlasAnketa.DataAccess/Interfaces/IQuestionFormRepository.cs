using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IQuestionFormRepository : IRepository<QuestionForm>
    {
        Task<List<QuestionForm>> GetAllFormQuestionsAsync();
        Task<QuestionForm> GetQuestionFormByIdAsync(int id);
        Task<QuestionForm?> GetNextActiveFormAsync(int currentFormId);
        Task<bool> ToggleFormStatusAsync(int formId, bool isActive);
    }
}
