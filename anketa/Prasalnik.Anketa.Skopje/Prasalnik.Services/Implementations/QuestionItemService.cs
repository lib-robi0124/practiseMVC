using Prasalnik.DataAccess.Interaces;
using Prasalnik.Domain.Enums;
using Prasalnik.Domain.Models;
using Prasalnik.Services.Interfaces;

namespace Prasalnik.Services.Implementations
{
    public class QuestionItemService : IQuestionItemService
    {
        private readonly IQuestionItemRepository _questionItemRepository;
        public QuestionItemService(IQuestionItemRepository questionItemRepository)
        {
            _questionItemRepository = questionItemRepository;
        }

        public IEnumerable<QuestionItem> GetAllItems() => _questionItemRepository.GetAll();
        public QuestionItem GetItemById(int id) => _questionItemRepository.GetById(id);
        public QuestionItem GetByType(QuestionTypeEnum type) => _questionItemRepository.GetByType(type.GetType());
        public void CreateItem(QuestionItem item) => _questionItemRepository.Create(item);
        public void UpdateItem(QuestionItem item) => _questionItemRepository.Update(item);
        public void DeleteItem(int id) => _questionItemRepository.Delete(id);
    }
}

