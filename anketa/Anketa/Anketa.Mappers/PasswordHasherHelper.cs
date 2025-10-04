using Anketa.Domain.Models;
using Microsoft.AspNetCore.Identity;
namespace Anketa.Mappers
{
    
        public static class PasswordHasherHelper
        {
            private static PasswordHasher<User> _passwordHasher = new();

            public static void HashPassword(User user, string password)
            {
                user.Password = _passwordHasher.HashPassword(user, password);
            }

            public static PasswordVerificationResult VerifyHashedPassword(User user, string password)
            {
                return _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            }
        }
}
