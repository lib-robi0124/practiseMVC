using Prasalnik.Domain.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        User Login(int companyId, string fullName);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }
}

