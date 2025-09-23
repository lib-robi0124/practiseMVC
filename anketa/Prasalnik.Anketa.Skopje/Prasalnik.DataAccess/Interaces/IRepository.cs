using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);

    }
}
