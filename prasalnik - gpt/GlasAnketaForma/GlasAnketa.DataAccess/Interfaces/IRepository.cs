using System.Linq.Expressions;

namespace GlasAnketa.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task Remove(T entity);
    }
}
