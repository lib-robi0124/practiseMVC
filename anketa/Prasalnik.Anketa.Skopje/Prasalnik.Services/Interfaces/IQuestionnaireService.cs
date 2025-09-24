using Prasalnik.Domain.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        IEnumerable<Questionnaire> GetAllQuestionnaires();
        Questionnaire GetQuestionnaireById(int id);
        Questionnaire GetByUserId(int userId);
        void CreateQuestionnaire(Questionnaire questionnaire);
        void UpdateQuestionnaire(Questionnaire questionnaire);
        void DeleteQuestionnaire(int id);
    }
}
