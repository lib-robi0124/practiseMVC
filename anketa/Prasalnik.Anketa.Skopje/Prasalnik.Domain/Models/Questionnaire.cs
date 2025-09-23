namespace Prasalnik.Domain.Models
{
    public class Questionnaire : BaseEntity
    {
        public string Title { get; set; }
        public List<QuestionItem> QuestionItems { get; set; } = new();
        public string Status { get; set; } // Draft, Published, Closed
    }
}
