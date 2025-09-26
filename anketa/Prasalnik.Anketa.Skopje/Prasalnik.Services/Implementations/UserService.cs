using AutoMapper;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        public UserViewModel GetUserById(int id)
        {
            var user = _userRepository.GetById(id);
            return _mapper.Map<UserViewModel>(user);
        }

        public UserViewModel Login(UserCredentialsViewModel creds)
        {
            var user = _userRepository.LoginUser(creds.CompanyId, creds.FullName);
            return _mapper.Map<UserViewModel>(user);
        }

        public void CreateUser(RegisterUserViewModel registerVm)
        {
            var user = _mapper.Map<User>(registerVm);
            _userRepository.Create(user);
        }

        public void UpdateUser(UserViewModel userVm)
        {
            var user = _mapper.Map<User>(userVm);
            _userRepository.Update(user);
        }

        public void DeleteUser(int id) => _userRepository.Delete(id);
    }
}
