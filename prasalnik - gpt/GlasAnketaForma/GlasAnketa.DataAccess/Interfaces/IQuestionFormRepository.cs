using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IQuestionFormRepository : IRepository<QuestionForm>
    {
        Task<List<QuestionForm>> GetAllFormQuestionsAsync();
    }
}
