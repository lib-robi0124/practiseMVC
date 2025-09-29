using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;
using VideoMovieRent.Services.Interfaces;

namespace VideoMovieRent.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(User user)
        {
            _userRepository.CreateUser(user);
        }

        public User GetUserByCardNumber(string cardNumber)
        {
            return _userRepository.GetUserByCardNumber(cardNumber);
        }
        public void Login(string cardNumber)
        {
            _userRepository.Login(cardNumber);
        }
    }
}
