using Lamazon.DataAccess.DataContext;
using Lamazon.DataAccess.Interfaces;
using Lamazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lamazon.DataAccess.Implementations
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public User GetByEmail(string email)
        {
            return _applicationDbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email.ToLower() == email.ToLower()); //if condition is string comparison,
                                                                            //use ToLower to avoid case sensitivity issues
        }

        public User GetById(int id)
        {
            return _applicationDbContext.Users
                   .Include(x => x.Role) //if we want to include related Role entity
                   .FirstOrDefault(x => x.Id == id); //FirstOrDefault returns first element or null if not found
        }

        public int Insert(User user)
        {
            _applicationDbContext.Users.Add(user); //Add method marks entity as Added in the context
            _applicationDbContext.SaveChanges(); //SaveChanges commits changes to the database
            return user.Id; //after SaveChanges, EF Core populates the Id property with the generated value
        }
    }
}
