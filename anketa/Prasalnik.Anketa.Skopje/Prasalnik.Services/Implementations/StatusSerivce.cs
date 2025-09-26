using AutoMapper;
using Prasalnik.DataAccess.Interaces;
using Prasalnik.Services.Interfaces;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repo;
        private readonly IMapper _mapper;

        public StatusService(IStatusRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<StatusViewModel> GetAll()
        {
            var statuses = _repo.GetAll();
            return _mapper.Map<IEnumerable<StatusViewModel>>(statuses);
        }

        public StatusViewModel GetById(int id)
        {
            var status = _repo.GetById(id);
            return _mapper.Map<StatusViewModel>(status);
        }
    }
}
