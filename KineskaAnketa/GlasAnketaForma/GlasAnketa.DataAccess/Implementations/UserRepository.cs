using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)   {  }
        public User GetByCompanyId(int companyId)
        {
            return _appDbContext.Users
                .Include(u => u.Role) // Make sure Role is included for mapping
                .AsNoTracking() // Optional: better performance for read operations
                .FirstOrDefault(x => x.CompanyId == companyId);
        }
        // Add this missing method
        public User GetById(int id)
        {
            return _appDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(x => x.Id == id);
        }
        public User GetByOU(string ou)
        {
            return _appDbContext.Users.Include(x => x.CompanyId).Include(x => x.OU2).FirstOrDefault(x => x.OU == ou);
        }

        public int Insert(User user)
        {
            _appDbContext.Users.Add(user); //Add method marks entity as Added in the context
            _appDbContext.SaveChanges(); //SaveChanges commits changes to the database
            return user.Id; //after SaveChanges, EF Core populates the Id property with the generated value
        }
    }

}
