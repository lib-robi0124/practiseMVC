using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IQuestionFormRepository
    {
        int InsertQuestionForm(QuestionForm questionForm);
        void UpdateQuestionForm(QuestionForm questionForm);
        void DeleteQuestionForm(int id);
        QuestionForm GetQuestionFormById(int id);
    }
}
