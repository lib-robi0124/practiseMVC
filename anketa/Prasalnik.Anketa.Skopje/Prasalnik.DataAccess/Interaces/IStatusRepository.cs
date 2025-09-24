using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IStatusRepository : IRepository<Status>
    {
        Status GetByName(string name);
    }
}
