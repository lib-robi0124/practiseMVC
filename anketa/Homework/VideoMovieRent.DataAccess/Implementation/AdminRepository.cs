using VideoMovieRent.DataAccess.Interfaces;
using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly VideoMovieRentDbContext _db;

        public AdminRepository(VideoMovieRentDbContext db)
        {
            _db = db;
        }
        public Admin? Login(string username, string password)
        {
            return _db.Admins.FirstOrDefault(a =>
                a.Username == username && a.Password == password);
        }
         public void Create(Movie entity)
        {
            _db.Movies.Add(entity);
            _db.SaveChanges();
        }
            
        public void Update(Movie entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            var movie = _db.Movies.FirstOrDefault(x => x.Id == id);
            _db.Movies.Remove(movie);
            _db.SaveChanges();
        }
    }
}
