using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionService
    {
        List<QuestionVM> GetAllQuestions();
        QuestionVM GetQuestionById(int questionId);

        // Admin methods
        void CreateQuestion(RegisterQuestionVM registerQuestionVM);
        int RemoveQuestion(int questionId);
        void UpdateQuestion(RegisterQuestionVM registerQuestionVM);
    }
}
