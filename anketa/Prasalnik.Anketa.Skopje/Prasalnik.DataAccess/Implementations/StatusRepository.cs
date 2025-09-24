using Prasalnik.DataAccess.DataContext;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Implementations
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AppDbContext _context;

        public StatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Status entity)
        {
            _context.Statuses.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var s = _context.Statuses.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Status not found");
            _context.Statuses.Remove(s);
            _context.SaveChanges();
        }

        public IEnumerable<Status> GetAll()
        {
            return _context.Statuses.ToList();
        }

        public Status GetById(int id)
        {
            return _context.Statuses.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Status not found");
        }

        public Status GetByName(string name)
        {
            return _context.Statuses.FirstOrDefault(x => x.Name == name) ?? throw new Exception("Status not found");
        }

        public void Update(Status entity)
        {
            _context.Statuses.Update(entity);
            _context.SaveChanges();
        }
    }

}
