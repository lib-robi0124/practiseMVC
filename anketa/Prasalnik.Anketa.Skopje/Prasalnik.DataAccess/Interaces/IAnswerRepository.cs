using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Answer GetByUserId(int userId);
    }
}
