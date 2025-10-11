using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionFormService
    {
        // For employees - get active forms
        Task<List<QuestionFormVM>> GetActiveFormsAsync();
        Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);

        // For admins - form management
        Task<List<QuestionFormVM>> GetAllFormsAsync();
        Task<QuestionFormVM> GetFormByIdAsync(int formId);
        Task<int> CreateFormAsync(QuestionFormVM formVm);
        Task<bool> UpdateFormAsync(QuestionFormVM formVm);
        Task<bool> DeleteFormAsync(int formId);
        Task<bool> ToggleFormStatusAsync(int formId, bool isActive);

        // Additional useful methods
        Task<bool> FormExistsAsync(int formId);
        Task<int> GetFormQuestionCountAsync(int formId);
    }
}

