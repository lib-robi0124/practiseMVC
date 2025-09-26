using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        IEnumerable<QuestionnaireViewModel> GetAll();
        QuestionnaireViewModel GetById(int id);
        void Create(QuestionnaireViewModel questionnaire);
        void Update(QuestionnaireViewModel questionnaire);
        void Delete(int id);
    }
}
