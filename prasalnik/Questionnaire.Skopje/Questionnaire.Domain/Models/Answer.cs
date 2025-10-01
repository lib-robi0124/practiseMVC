namespace Questionnaire.Domain.Models
{
    public class Answer : BaseEntity
    {
        public int QuestionFormId { get; set; }
        public QuestionForm QuestionForm { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string Response { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Answer()
        {
            Response = string.Empty;
        }

    }
}