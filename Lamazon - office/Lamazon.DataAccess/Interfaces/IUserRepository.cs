using Lamazon.Domain.Entities;

namespace Lamazon.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByEmail(string email);
        int Insert(User user);
    }
}
