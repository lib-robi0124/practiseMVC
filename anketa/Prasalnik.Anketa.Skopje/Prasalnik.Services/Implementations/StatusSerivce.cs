using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;

namespace Prasalnik.Services.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public IEnumerable<Status> GetAllStatuses() => _statusRepository.GetAll();
        public Status GetStatusById(int id) => _statusRepository.GetById(id);
        public Status GetByName(string name) => _statusRepository.GetByName(name);
        public void CreateStatus(Status status) => _statusRepository.Create(status);
        public void UpdateStatus(Status status) => _statusRepository.Update(status);
        public void DeleteStatus(int id) => _statusRepository.Delete(id);
    }
}
