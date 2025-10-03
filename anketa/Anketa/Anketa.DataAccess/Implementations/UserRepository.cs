using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Anketa.DataAccess.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.CompanyId == companyId);
        }

        public async Task<User> GetUserWithRoleAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<User> AuthenticateAsync(int companyId, string fullName, string password)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.CompanyId == companyId &&
                                        u.FullName == fullName &&
                                        u.Password == password);
        }

        public async Task<string> GetUserOUAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.OU)
                .FirstOrDefaultAsync();
        }
    }
}
