using AutoMapper;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;
using Prasalnik.Domain.Enums;

namespace Prasalnik.Services.Implementations
{
    public class QuestionItemService : IQuestionItemService
    {
        private readonly IQuestionItemRepository _repo;
        private readonly IMapper _mapper;

        public QuestionItemService(IQuestionItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<QuestionItemViewModel> GetAll()
        {
            var items = _repo.GetAll();
            return _mapper.Map<IEnumerable<QuestionItemViewModel>>(items);
        }

        public QuestionItemViewModel GetById(int id)
        {
            var item = _repo.GetById(id);
            return _mapper.Map<QuestionItemViewModel>(item);
        }

        public IEnumerable<QuestionItemViewModel> GetByType(QuestionTypeEnum type)
        {
            QuestionItem items = _repo.GetByType(type);
            return _mapper.Map<IEnumerable<QuestionItemViewModel>>(items);
        }

        public void Create(QuestionItemViewModel vm)
        {
            var entity = _mapper.Map<QuestionItem>(vm);
            _repo.Create(entity);
        }

        public void Update(QuestionItemViewModel vm)
        {
            var entity = _mapper.Map<QuestionItem>(vm);
            _repo.Update(entity);
        }

        public void Delete(int id) => _repo.Delete(id);
    }
}
