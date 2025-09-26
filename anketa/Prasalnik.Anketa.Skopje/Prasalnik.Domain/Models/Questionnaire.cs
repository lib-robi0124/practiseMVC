namespace Prasalnik.Domain.Models
{
    public class Questionnaire : BaseEntity
    {
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public ICollection<QuestionItem> QuestionItems { get; set; }
        public string Status { get; set; } // Answered, Skipped
        public Questionnaire()
        {
            QuestionItems = new HashSet<QuestionItem>();
        }
    }
}
