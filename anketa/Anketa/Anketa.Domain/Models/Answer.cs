namespace Anketa.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public int? ScaleValue { get; set; }
        public string TextValue { get; set; }
        public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
    }
}
