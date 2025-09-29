using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Interfaces
{
    public interface IAdminRepository
    {
        Admin? Login(string username, string password);
        void Create(Movie entity);
        void Update(Movie entity);
        void Delete(int id);
    }
}
