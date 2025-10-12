using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionFormService
    {
        Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);
        Task<List<QuestionFormVM>> GetAllActiveFormsAsync();
        Task<QuestionFormVM> GetActiveFormAsync();
        Task<QuestionFormVM?> GetNextActiveFormAsync(int currentFormId);
		// For admins - form management
        Task<List<QuestionFormVM>> GetAllFormsAsync();
        Task<QuestionFormVM> GetFormByIdAsync(int formId);
        Task<int> CreateFormAsync(QuestionFormVM formVm);
        Task<bool> UpdateFormAsync(QuestionFormVM formVm);
        Task<bool> DeleteFormAsync(int formId);
        Task<bool> ToggleFormStatusAsync(int formId, bool isActive);

    }
}
