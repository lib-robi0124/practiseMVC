namespace GlasAnketa.Domain.Models
{
    public class QuestionType
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Scale", "Text"
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
        public QuestionType()
        {
            Questions = new HashSet<Question>();
        }
    }
}