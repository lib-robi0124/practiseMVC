using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionFormService
    {
        Task<QuestionFormVM> GetFormWithQuestionsAsync(int formId);
        
    }
}
