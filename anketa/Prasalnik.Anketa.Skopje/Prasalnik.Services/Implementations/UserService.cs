using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;

namespace Prasalnik.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();
        public User GetUserById(int id) => _userRepository.GetById(id);
        public User Login(int companyId, string fullName) => _userRepository.LoginUser(companyId, fullName);
        public void CreateUser(User user) => _userRepository.Create(user);
        public void UpdateUser(User user) => _userRepository.Update(user);
        public void DeleteUser(int id) => _userRepository.Delete(id);
    }
}
