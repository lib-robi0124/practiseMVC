using Prasalnik.ViewModels.Models;
using Prasalnik.Domain.Enums;

namespace Prasalnik.Services.Interfaces
{
    public interface IQuestionItemService
    {
        IEnumerable<QuestionItemViewModel> GetAll();
        QuestionItemViewModel GetById(int id);
        IEnumerable<QuestionItemViewModel> GetByType(QuestionTypeEnum type);
        void Create(QuestionItemViewModel questionItem);
        void Update(QuestionItemViewModel questionItem);
        void Delete(int id);
    }
}
