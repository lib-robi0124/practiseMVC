using AutoMapper;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _repo;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<AnswerViewModel> GetAll()
        {
            var answers = _repo.GetAll();
            return _mapper.Map<IEnumerable<AnswerViewModel>>(answers);
        }

        public AnswerViewModel GetById(int id)
        {
            var answer = _repo.GetById(id);
            return _mapper.Map<AnswerViewModel>(answer);
        }

        public void Create(AnswerViewModel vm)
        {
            var entity = _mapper.Map<Answer>(vm);
            _repo.Create(entity);
        }

        public void Update(AnswerViewModel vm)
        {
            var entity = _mapper.Map<Answer>(vm);
            _repo.Update(entity);
        }

        public void Delete(int id) => _repo.Delete(id);
    }
}
