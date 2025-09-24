using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;

namespace Prasalnik.Services.Implementations
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;
        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository)
        {
            _questionnaireRepository = questionnaireRepository;
        }

        public IEnumerable<Questionnaire> GetAllQuestionnaires() => _questionnaireRepository.GetAll();
        public Questionnaire GetQuestionnaireById(int id) => _questionnaireRepository.GetById(id);
        public Questionnaire GetByUserId(int userId) => _questionnaireRepository.GetbyUserId(userId);
        public void CreateQuestionnaire(Questionnaire questionnaire) => _questionnaireRepository.Create(questionnaire);
        public void UpdateQuestionnaire(Questionnaire questionnaire) => _questionnaireRepository.Update(questionnaire);
        public void DeleteQuestionnaire(int id) => _questionnaireRepository.Delete(id);
    }
}
