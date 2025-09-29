
using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly VideoMovieRentDbContext _db;

        public UserRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public User GetUserByCardNumber(string cardNumber)
        {
            return _db.Users.FirstOrDefault(x => x.CardNumber == cardNumber);
        }

        public void Login(string cardNumber)
        {
            var user = _db.Users.FirstOrDefault(x => x.CardNumber == cardNumber);
        }

        public void CreateUser(User user)
        {
            _db.Users.Add(user);
        }
    }
}
