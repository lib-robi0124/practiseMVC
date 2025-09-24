using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IQuestionnaireRepository : IRepository<Questionnaire>
    {
        Questionnaire GetbyUserId(int userId);
    }
}
