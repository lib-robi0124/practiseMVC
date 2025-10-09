namespace GlasAnketa.ViewModels.Models
{
    public class QuestionVM
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int UserId { get; set; }
        public int QuestionFormId { get; set; }
        public bool IsRequired { get; set; }
        public string QuestionType { get; set; }

    }
}