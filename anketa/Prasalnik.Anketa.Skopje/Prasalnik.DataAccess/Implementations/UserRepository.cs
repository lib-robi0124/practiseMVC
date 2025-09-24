using Prasalnik.DataAccess.DataContext;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        public AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Create(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id) ?? throw new Exception("User not found");
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id) ?? throw new Exception("User not found");
        }

        public User GetUserByCompanyId(int companyId)
        {
            return _context.Users.FirstOrDefault(u => u.CompanyId == companyId) ?? throw new Exception("User not found");
        }

        public User LoginUser(int companyId, string fullName)
        {
            return _context.Users.FirstOrDefault(u => u.CompanyId == companyId && u.FullName == fullName) ?? throw new Exception("User not found");
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
