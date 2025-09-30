using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;
using VideoMovieRent.Services.Interfaces;

namespace VideoMovieRent.Services.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        public RentalService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public IEnumerable<Rental> GetRentalsByUserId(int userId)
        {
            return _rentalRepository.GetRentalsByUserId(userId);
        }

        public bool RentMovie(int userId, int movieId)
        {
            return _rentalRepository.Rent(userId, movieId);
        }

        public bool MarkAsReturned(int userId, int movieId)
        {
            return _rentalRepository.MarkAsReturned(userId, movieId);
        }
    }
}
