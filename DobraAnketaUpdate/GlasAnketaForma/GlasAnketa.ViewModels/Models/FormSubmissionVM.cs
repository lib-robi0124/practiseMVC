namespace GlasAnketa.ViewModels.Models
{
    public class FormSubmissionVM
    {
        public QuestionFormVM QuestionForm { get; set; }
        public List<AnswerVM> Answers { get; set; } = new List<AnswerVM>(); // Initialize
        public int QuestionFormId { get; set; }
        public int? NextFormId { get; set; } // Add this to track next form
    }
}
