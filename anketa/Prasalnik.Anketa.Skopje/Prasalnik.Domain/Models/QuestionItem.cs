using Prasalnik.Domain.Enums;

namespace Prasalnik.Domain.Models
{
    public class QuestionItem : BaseEntity
    {
        public string QuestionText { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public int QuestionnaireId { get; set; }
    }
}
