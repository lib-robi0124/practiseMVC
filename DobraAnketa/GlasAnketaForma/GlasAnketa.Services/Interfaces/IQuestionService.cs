using GlasAnketa.Domain.Models;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionVM> GetQuestionById(int questionId);
        Task<List<QuestionVM>> GetAllQuestions();
    }
}
