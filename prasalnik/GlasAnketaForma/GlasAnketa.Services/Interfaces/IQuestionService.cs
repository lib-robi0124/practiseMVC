using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionService
    {
        List<Question> GetQuestionById(int questionId);
        List<Question> GetAllQuestions();
        // Admin methods
        QuestionVM CreateQuestion(RegisterQuestionVM registerQuestionVM);
        int DeleteQuestionAsync(int questionId);
        QuestionVM UpdateQuestion(RegisterQuestionVM questionVM);
    }
}
