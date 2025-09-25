﻿using AutoMapper;
using Lamazon.DataAccess.Interfaces;
using Lamazon.DataAccess.Migrations;
using Lamazon.Domain.Constants;
using Lamazon.Domain.Entities;
using Lamazon.Services.Helpers;
using Lamazon.Services.Interfaces;
using Lamazon.ViewModels.Models;

namespace Lamazon.Services.Implementations
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
        public UserViewModel GetUserById(int id)
        {
            var user = _userRepository.GetById(id);

            return _mapper.Map<UserViewModel>(user);
        }

        public UserViewModel RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            var user = _mapper.Map<User>(registerUserViewModel);
            PasswordHasherHelper.HashPassword(user, registerUserViewModel.Password);
            user.RoleKey = Roles.User;
            var userId = _userRepository.Insert(user);
            if(userId <=0)
            {
               throw new Exception("Error registering user");
            }
            return GetUserById(userId);
        }

        public UserViewModel ValidateUser(UserCredentialsViewModel userCredentialsViewModel)
        {
            var user = _userRepository.GetByEmail(userCredentialsViewModel.Email);
            if (user is null)
            {
                return null;
            }
            var result = PasswordHasherHelper.VerifyHashedPassword(user, userCredentialsViewModel.Password);
            if(result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                {
                return new UserViewModel();
            }
            return _mapper.Map<UserViewModel>(user);
        }

    }
}
