using Prasalnik.Domain.Enums;
using Prasalnik.Domain.Models;

namespace Prasalnik.Services.Interfaces
{
    public interface IQuestionItemService
    {
        IEnumerable<QuestionItem> GetAllItems();
        QuestionItem GetItemById(int id);
        QuestionItem GetByType(QuestionTypeEnum type);
        void CreateItem(QuestionItem item);
        void UpdateItem(QuestionItem item);
        void DeleteItem(int id);
    }
}

