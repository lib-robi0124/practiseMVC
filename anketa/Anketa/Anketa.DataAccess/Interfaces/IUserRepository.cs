using Anketa.Domain.Models;

namespace Anketa.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByCompanyIdAsync(int companyId);
        Task<User> GetUserWithRoleAsync(int userId);
        Task<User> AuthenticateAsync(int companyId, string fullName, string password);
        Task<string> GetUserOUAsync(int userId);
    }
}
