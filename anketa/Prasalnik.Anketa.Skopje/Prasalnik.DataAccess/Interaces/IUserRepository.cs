using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByCompanyId(int companyId);
        User LoginUser(int companyId, string fullName);

    }
}
