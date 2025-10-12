using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionVM> GetQuestionById(int questionId);
        Task<List<QuestionVM>> GetAllQuestions();
        // Admin methods
        void CreateQuestion(RegisterQuestionVM registerQuestionVM);
        Task<bool> RemoveQuestion(int questionId);
        void UpdateQuestion(RegisterQuestionVM registerQuestionVM);
    }
}
