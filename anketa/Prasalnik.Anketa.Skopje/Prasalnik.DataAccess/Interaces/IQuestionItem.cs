using Prasalnik.Domain.Models;

namespace Prasalnik.DataAccess.Interaces
{
    public interface IQuestionItem : IRepository<QuestionItem>
    {
        QuestionItem GetByType(Type type);
    }
}
