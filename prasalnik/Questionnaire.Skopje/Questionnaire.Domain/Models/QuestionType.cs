namespace Questionnaire.Domain.Models
{
    public class QuestionType : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }
    }
}
