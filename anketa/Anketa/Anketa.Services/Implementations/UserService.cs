using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Anketa.Services.Interfaces;

namespace Anketa.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateAsync(int companyId, string fullName, string password)
        {
            return await _userRepository.AuthenticateAsync(companyId, fullName, password);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserWithRoleAsync(id);
        }

        public async Task<string> GetUserOUAsync(int userId)
        {
            return await _userRepository.GetUserOUAsync(userId);
        }
    }
}
