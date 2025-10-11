using AutoMapper;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using GlasAnketa.Services.Interfaces;
using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Implementations
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

        public UserVM ValidateUser(UserCredentialsVM userCredentials)
        {
            try
            {
                Console.WriteLine($"Validating user: CompanyId={userCredentials.CompanyId}");

                var user = _userRepository.GetByCompanyId(userCredentials.CompanyId);

                if (user == null)
                {
                    Console.WriteLine("User not found in database");
                    return null;
                }

                Console.WriteLine($"Found user: {user.FullName}, Password in DB: {user.Password}, Input password: {userCredentials.Password}");

                // Simple password comparison (check your seeded data)
                if (user.Password != userCredentials.Password)
                {
                    Console.WriteLine("Password mismatch");
                    return null;
                }

                // Make sure role is loaded
                if (user.Role == null)
                {
                    Console.WriteLine("User role is null, loading role...");
                    user = _userRepository.GetById(user.Id); // Reload with role
                }

                Console.WriteLine($"User validated: {user.FullName}, Role: {user.Role?.Name}");
                return _mapper.Map<UserVM>(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateUser: {ex.Message}");
                return null;
            }
        }
        public UserVM GetUserById(int id)
        {
            var user = _userRepository.GetByCompanyId(id);
            return _mapper.Map<UserVM>(user);
        }

        public UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM)
        {
            var user = _mapper.Map<User>(registerUserViewModel);
            user.Password = registerUserViewModel.Password;
            user.RoleId = roleVM.Name == "Administrator" ? 1 : 2;
            var userId = _userRepository.Insert(user);

            if (userId <= 0)
            {
                throw new Exception("User not saved.");
            }

            return GetUserById(userId);
        }
    }
}