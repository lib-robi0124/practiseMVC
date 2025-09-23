using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IAnswer : IRepository<Answer>
    {
        Answer GetByUserId(int userId);
    }
}
