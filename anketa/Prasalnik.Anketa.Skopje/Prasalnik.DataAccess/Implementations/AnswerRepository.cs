using Prasalnik.DataAccess.DataContext;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Answer entity)
        {
            _context.Answers.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var ans = _context.Answers.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Answer not found");
            _context.Answers.Remove(ans);
            _context.SaveChanges();
        }

        public IEnumerable<Answer> GetAll()
        {
            return _context.Answers.ToList();
        }

        public Answer GetById(int id)
        {
            return _context.Answers.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Answer not found");
        }

        public Answer GetByUserId(int userId)
        {
            return _context.Answers.FirstOrDefault(x => x.UserId == userId) ?? throw new Exception("No answers found for user");
        }

        public void Update(Answer entity)
        {
            _context.Answers.Update(entity);
            _context.SaveChanges();
        }
    }

}
