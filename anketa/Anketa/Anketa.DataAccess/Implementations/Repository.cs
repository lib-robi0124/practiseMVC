using Anketa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Anketa.DataAccess.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }
        public virtual void Update(T entity)
        {
            _entities.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }
    }
}
