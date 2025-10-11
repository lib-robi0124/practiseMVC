using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByCompanyIdAsync(int companyId);
        Task<User> AuthenticateAsync(int companyId, string password);
        Task<string> GetUserOUAsync(int userId);
    }
}
