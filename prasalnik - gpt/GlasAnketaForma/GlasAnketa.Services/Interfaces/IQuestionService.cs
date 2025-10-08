using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionService
    {
        QuestionVM GetQuestionById(int questionId);
        List<QuestionVM> GetAllQuestions();
        // Admin methods
        void CreateQuestion(RegisterQuestionVM registerQuestionVM);
        int DeleteQuestionAsync(int questionId);
        void UpdateQuestion(RegisterQuestionVM registerQuestionVM);
    }
}
