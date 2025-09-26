using Prasalnik.Domain.Enums;

namespace Prasalnik.Domain.Models
{
    public class QuestionItem : BaseEntity
    {
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public string QuestionText { get; set; }
        public QuestionTypeEnum Type { get; set; }
    }
}
