namespace Questionnaire.Domain.Models
{
    public class User : BaseEntity
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; } // Organizational Unit
        public string RoleKey { get; set; }
        public Role Role { get; set; }
        public virtual ICollection<QuestionForm> QuestionForms { get; set; }
        public User()
        {
            QuestionForms = new HashSet<QuestionForm>();
        }
    }
}
