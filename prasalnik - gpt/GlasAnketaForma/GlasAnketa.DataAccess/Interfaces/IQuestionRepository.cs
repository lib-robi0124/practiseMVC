using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IQuestionRepository
    {
        int InsertQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(int id);
        Question GetQuestionById(int id);
    }
}
