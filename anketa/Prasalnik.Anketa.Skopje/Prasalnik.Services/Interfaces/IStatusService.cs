using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IStatusService
    {
        IEnumerable<StatusViewModel> GetAll();
        StatusViewModel GetById(int id);
    }
}
