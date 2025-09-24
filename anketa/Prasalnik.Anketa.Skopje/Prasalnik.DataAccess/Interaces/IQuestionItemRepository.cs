using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IQuestionItemRepository : IRepository<QuestionItem>
    {
        QuestionItem GetByType(Type type);
    }
}
