using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Implementation
{
    public class RentalRepository : IRentalRepository
    {
        private readonly VideoMovieRentDbContext _db;
        public RentalRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Rental> GetRentalsByUserId(int userId)
        {
            return _db.Rentals.Where(r => r.UserId == userId && r.ReturnedOn == DateTime.MinValue).ToList();
        }

        public bool MarkAsReturned(int rentalId, int userId)
        {
            var rental = _db.Rentals.FirstOrDefault(r => r.Id == rentalId && r.UserId == userId && r.ReturnedOn == DateTime.MinValue);
            if (rental == null)
            {
                return false; // Rental not found or already returned
            }
            rental.ReturnedOn = DateTime.UtcNow;
            var movie = _db.Movies.FirstOrDefault(m => m.Id == rental.MovieId);
            if (movie != null)
            {
                movie.Quantity++; // Increment the quantity of the movie
                if (movie.Quantity > 0)
                {
                    movie.IsAvailable = true; // Mark the movie as available
                }
                    
            }
            _db.SaveChanges();
            return true; // Rental marked as returned successfully
        }

        public bool Rent(int movieId, int userId)
        {
            var movie = _db.Movies.SingleOrDefault(x => x.Id == movieId);

            if (movie == null || movie.Quantity <= 0)
            {
                return false; // Movie not available for rent
            }
            var rental = new Rental
            {
                MovieId = movieId,
                Title = movie.Title,
                UserId = userId,
                RentedOn = DateTime.UtcNow,
                ReturnedOn = DateTime.MinValue // Not returned yet
            };
            _db.Rentals.Add(rental);
            movie.Quantity--; // Decrement the quantity of the movie
            if (movie.Quantity == 0)
            {
                movie.IsAvailable = false; // Mark the movie as not available
            }
            _db.SaveChanges();
            return true; // Rental created successfully
        }
 
    }
}
