using VideoMovieRent.Domain;

namespace VideoMovieRent.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int Id);
        //object GetById(Movie movie);
    }

}
