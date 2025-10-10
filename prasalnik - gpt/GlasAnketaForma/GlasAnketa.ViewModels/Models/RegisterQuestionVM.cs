namespace GlasAnketa.ViewModels.Models
{
    public class RegisterQuestionVM
    {
        public readonly int CreatedByUserId;

        public int Id { get; set; }
        public int QuestionFormId { get; set; }
        public string Text { get; set; }
        public int QuestionTypeId { get; set; }

    }
}
