using GlasAnketa.DataAccess.DataContext;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
        public async Task<User> AuthenticateAsync(int companyId, string password)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.CompanyId == companyId && u.Password == password);
        }
        public async Task<User> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.CompanyId == companyId);
        }
        public async Task<string> GetUserOUAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.OU)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.OU2)
                .ThenBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<Dictionary<string, List<User>>> GetUsersByOU2Async()
        {
            var users = await GetAllUsersAsync();
            return users.GroupBy(u => u.OU2)
                       .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
