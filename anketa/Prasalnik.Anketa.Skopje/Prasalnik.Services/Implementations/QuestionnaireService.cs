using AutoMapper;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Implementations
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _repo;
        private readonly IMapper _mapper;

        public QuestionnaireService(IQuestionnaireRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<QuestionnaireViewModel> GetAll()
        {
            var questionnaires = _repo.GetAll();
            return _mapper.Map<IEnumerable<QuestionnaireViewModel>>(questionnaires);
        }

        public QuestionnaireViewModel GetById(int id)
        {
            var questionnaire = _repo.GetById(id);
            return _mapper.Map<QuestionnaireViewModel>(questionnaire);
        }

        public void Create(QuestionnaireViewModel questionnaireVm)
        {
            var entity = _mapper.Map<Questionnaire>(questionnaireVm);
            _repo.Create(entity);
        }

        public void Update(QuestionnaireViewModel questionnaireVm)
        {
            var entity = _mapper.Map<Questionnaire>(questionnaireVm);
            _repo.Update(entity);
        }

        public void Delete(int id) => _repo.Delete(id);
    }
}
