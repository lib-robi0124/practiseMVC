namespace Questionnaire.Domain.Models
{
    public class Question : BaseEntity
    {
        public string Name { get; set; }
        public string QuestionText { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new HashSet<Answer>();
        }
    }
}
