using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IQuestionFormRepository
    {
        // Existing synchronous methods
        int InsertQuestionForm(QuestionForm questionForm);
        void UpdateQuestionForm(QuestionForm questionForm);
        void DeleteQuestionForm(int id);
        QuestionForm GetQuestionFormById(int id);
        List<QuestionForm> GetAllFormQuestions();

        // New methods needed by service
        List<QuestionForm> GetActiveForms();
        QuestionForm GetFormWithQuestions(int formId);
        Task<int> InsertFormAsync(QuestionForm questionForm);
        Task<bool> UpdateFormAsync(QuestionForm questionForm);
        Task<bool> ToggleFormStatusAsync(int formId, bool isActive);
    }
}
