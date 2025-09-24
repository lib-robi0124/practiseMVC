using Microsoft.EntityFrameworkCore;
using Prasalnik.DataAccess.DataContext;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Implementations
{
    public class QuestionnaireRepository : IQuestionnaireRepository
    {
        private readonly AppDbContext _context;

        public QuestionnaireRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Questionnaire entity)
        {
            _context.Questionnaires.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var q = _context.Questionnaires.FirstOrDefault(x => x.Id == id) ?? throw new Exception("Questionnaire not found");
            _context.Questionnaires.Remove(q);
            _context.SaveChanges();
        }

        public IEnumerable<Questionnaire> GetAll()
        {
            return _context.Questionnaires.Include(q => q.QuestionItems).ToList();
        }

        public Questionnaire GetById(int id)
        {
            return _context.Questionnaires.Include(q => q.QuestionItems).FirstOrDefault(x => x.Id == id)
                   ?? throw new Exception("Questionnaire not found");
        }

        public Questionnaire GetbyUserId(int userId)
        {
            return _context.Answers
                .Where(a => a.UserId == userId)
                .Select(a => a.QuestionnaireId)
                .Distinct()
                .Select(id => _context.Questionnaires.Include(q => q.QuestionItems).FirstOrDefault(q => q.Id == id))
                .FirstOrDefault() ?? throw new Exception("No questionnaire found for this user");
        }

        public void Update(Questionnaire entity)
        {
            _context.Questionnaires.Update(entity);
            _context.SaveChanges();
        }
    }

}
