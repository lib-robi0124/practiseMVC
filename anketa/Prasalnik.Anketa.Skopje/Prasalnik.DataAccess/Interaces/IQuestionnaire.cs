using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IQuestionnaire : IRepository<Questionnaire>
    {
        Questionnaire GetbyUserId(int userId);
    }
}
