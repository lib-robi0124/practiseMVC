using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;

namespace Prasalnik.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public IEnumerable<Answer> GetAllAnswers() => _answerRepository.GetAll();
        public Answer GetAnswerById(int id) => _answerRepository.GetById(id);
        public Answer GetByUserId(int userId) => _answerRepository.GetByUserId(userId);
        public void CreateAnswer(Answer answer) => _answerRepository.Create(answer);
        public void UpdateAnswer(Answer answer) => _answerRepository.Update(answer);
        public void DeleteAnswer(int id) => _answerRepository.Delete(id);
    }
}
