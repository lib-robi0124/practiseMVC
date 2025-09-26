using Prasalnik.ViewModels.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserViewModel> GetAllUsers();
        UserViewModel GetUserById(int id);
        UserViewModel Login(UserCredentialsViewModel creds);
        void CreateUser(RegisterUserViewModel registerVm);
        void UpdateUser(UserViewModel userVm);
        void DeleteUser(int id);
    }
}
