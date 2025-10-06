namespace GlasAnketa.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; } // Admin user who created the question
        public User User { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public bool IsRequired { get; set; } = true;
        public ICollection<Answer> Answers { get; set; }
    }
}