using Prasalnik.Domain.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAllStatuses();
        Status GetStatusById(int id);
        Status GetByName(string name);
        void CreateStatus(Status status);
        void UpdateStatus(Status status);
        void DeleteStatus(int id);
    }
}
