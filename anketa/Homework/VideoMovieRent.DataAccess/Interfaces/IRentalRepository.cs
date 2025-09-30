
using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Interfaces
{
    public interface IRentalRepository
    {
        IEnumerable<Rental> GetRentalsByUserId(int userId);
        public bool MarkAsReturned(int rentalId, int userId);
        bool Rent(int movieId, int userId);
    }
}
