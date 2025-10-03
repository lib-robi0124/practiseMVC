namespace Anketa.Domain.Models
{
    public class QuestionForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
