namespace Questionnaire.Domain.Models
{
    public class QuestionForm : BaseEntity
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public QuestionForm()
        {
            Answers = new HashSet<Answer>();
            Questions = new HashSet<Question>();
        }
    }
}
