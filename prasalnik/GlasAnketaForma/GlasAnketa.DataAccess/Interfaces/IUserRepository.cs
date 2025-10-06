using GlasAnketa.Domain.Models;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        User GetByCompanyId(int companyId);
        User GetByOU(string ou);
        int Insert(User user);
    }
}
