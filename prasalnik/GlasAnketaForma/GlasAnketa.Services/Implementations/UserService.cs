using AutoMapper;
using GlasAnketa.DataAccess.Implementations;
using GlasAnketa.DataAccess.Interfaces;
using GlasAnketa.Domain.Models;
using GlasAnketa.Services.Helpers;
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
        public UserVM GetUserById(int id)
        {
            var user = _userRepository.GetByCompanyId(id);
            return _mapper.Map<UserVM>(user);
        }

        public UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM)
        {
            var user = _mapper.Map<User>(registerUserViewModel);
            PasswordHasherHelper.HashPassword(user, registerUserViewModel.Password);
            user.RoleId = roleVM.RoleId; // FIX: Assign RoleId property directly from RoleVM
            var userId = _userRepository.Insert(user);

            if (userId <= 0)
            {
                throw new Exception("User not saved.");
            }

            return GetUserById(userId);
        }

        public UserVM ValidateUser(UserCredentialsVM userCredentialsViewModel)
        {
            var user = _userRepository.GetByCompanyId(userCredentialsViewModel.CompanyId);
            if (user == null)
            {
                return null;
            }
            var result = PasswordHasherHelper.VerifyHashedPassword(user, userCredentialsViewModel.Password);
            if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
            {
                return new UserVM();
            }
            return _mapper.Map<UserVM>(user);
        }
    }
}