using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IAnswerService
    {
        IEnumerable<AnswerViewModel> GetAll();
        AnswerViewModel GetById(int id);
        void Create(AnswerViewModel answer);
        void Update(AnswerViewModel answer);
        void Delete(int id);
    }
}
