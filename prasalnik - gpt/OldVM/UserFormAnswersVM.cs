namespace GlasAnketa.ViewModels.Models
{
    public class UserFormAnswersVM
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int? ScaleValue { get; set; }
        public string TextValue { get; set; }
        public DateTime AnsweredDate { get; set; }
    }
}
