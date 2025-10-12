using GlasAnketa.ViewModels.Models;

namespace GlasAnketa.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserVM> RegisterUser(RegisterUserVM registerUserVM, RoleVM roleVM);
        Task<QuestionVM> RegisterQuestion(RegisterQuestionVM registerQuestionVM, RoleVM roleVM);
        Task<QuestionFormVM> CreateQuestionForm(CreateQuestionFormVM createQuestionFormVM, RoleVM roleVM);
        Task<QuestionFormVM> UpdateFormAsync(QuestionFormVM formVm, RoleVM roleVM);
        Task<UserVM> GetUserById(int id);
        Task<UserVM> ValidateUser(UserCredentialsVM userCredentialsVM);
    }
}
