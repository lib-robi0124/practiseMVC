using Prasalnik.Domain.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IAnswerService
    {
        IEnumerable<Answer> GetAllAnswers();
        Answer GetAnswerById(int id);
        Answer GetByUserId(int userId);
        void CreateAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void DeleteAnswer(int id);
    }
}
