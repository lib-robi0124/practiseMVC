using System.Collections.Generic;
using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Implementation
{
    public class MovieRepository : IRepository<Movie>
    {
        private readonly VideoMovieRentDbContext _db;

        public MovieRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Movie> GetAll()
        {
            return _db.Movies.ToList();
        } 
        public Movie GetById(int id)
        {
            return _db.Movies.SingleOrDefault(x => x.Id == id);
        }
        public void Create(Movie entity)
        {
            _db.Movies.Add(entity);
            _db.SaveChanges();
        }
        public void Delete(int id)
        {
            var movie = _db.Movies.SingleOrDefault(x => x.Id == id);

            if (movie != null)
            {
                _db.Movies.Remove(movie);
                _db.SaveChanges();
            }
        }
        public void Update(Movie entity)
        {
            _db.Movies.Update(entity);
            _db.SaveChanges();
        }
      
    }
}
