using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionFormService
    {
        Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);
        Task<List<QuestionFormVM>> GetAllActiveFormsAsync();
        Task<QuestionFormVM> GetActiveFormAsync();
        Task<QuestionFormVM> GetFormByIdAsync(int id);
        Task<QuestionFormVM?> GetNextActiveFormAsync(int currentFormId);

    }
}
