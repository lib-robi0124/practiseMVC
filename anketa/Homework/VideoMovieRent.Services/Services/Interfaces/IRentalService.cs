using VideoMovieRent.Domain;

namespace VideoMovieRent.Services.Interfaces
{
    public interface IRentalService
    {
        IEnumerable<Rental> GetRentalsByUserId(int userId);
        public bool MarkAsReturned(int rentalId, int userId);
        bool RentMovie(int movieId, int userId);
    }
}
