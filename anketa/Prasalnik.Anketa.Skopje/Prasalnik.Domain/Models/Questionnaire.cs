namespace Prasalnik.Domain.Models
{
    public class Questionnaire : BaseEntity
    {
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public ICollection<QuestionItem> QuestionItems { get; set; }

        // Use FK to Status table
        public int StatusId { get; set; }
        public Status Status { get; set; }  // navigation property

        public Questionnaire()
        {
            QuestionItems = new HashSet<QuestionItem>();
        }
    }
}
