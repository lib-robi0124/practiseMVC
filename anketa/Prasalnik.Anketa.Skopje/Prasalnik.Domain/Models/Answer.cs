namespace Prasalnik.Domain.Models
{
    public class Answer : BaseEntity
    {
        public int QuestionnaireId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public string Response { get; set; }
    }
}
