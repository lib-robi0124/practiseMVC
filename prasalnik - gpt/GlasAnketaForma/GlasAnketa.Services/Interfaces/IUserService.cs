using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IUserService
    {
        UserVM RegisterUser(RegisterUserVM registerUserViewModel, RoleVM roleVM);

        UserVM GetUserById(int id);

        UserVM ValidateUser(UserCredentialsVM userCredentialsViewModel);


    }
}
