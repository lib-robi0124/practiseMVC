using Anketa.Domain.Models;

namespace Anketa.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(int companyId, string fullName, string password);
        Task<User> GetUserByIdAsync(int id);
        Task<string> GetUserOUAsync(int userId);
    }
}
