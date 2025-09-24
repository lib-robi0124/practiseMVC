using Prasalnik.DataAccess.DataContext;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Implementations
{
    public class QuestionItemRepository : IQuestionItemRepository
    {
        private readonly AppDbContext _context;

        public QuestionItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(QuestionItem entity)
        {
            _context.QuestionItems.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.QuestionItems.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Question item not found");
            _context.QuestionItems.Remove(item);
            _context.SaveChanges();
        }

        public IEnumerable<QuestionItem> GetAll()
        {
            return _context.QuestionItems.ToList();
        }

        public QuestionItem GetById(int id)
        {
            return _context.QuestionItems.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Question item not found");
        }

        public QuestionItem GetByType(Type type)
        {
            throw new NotSupportedException("Use QuestionTypeEnum instead of System.Type");
        }

        public void Update(QuestionItem entity)
        {
            _context.QuestionItems.Update(entity);
            _context.SaveChanges();
        }
    }

}
