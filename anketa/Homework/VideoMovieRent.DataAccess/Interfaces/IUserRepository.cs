using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User GetUserByCardNumber(string cardNumber);
        void Login(string cardNumber);
    }
}
