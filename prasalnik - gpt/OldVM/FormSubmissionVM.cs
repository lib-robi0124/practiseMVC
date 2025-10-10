namespace GlasAnketa.ViewModels.Models
{
    public class FormSubmissionVM
    {
        public QuestionFormVM QuestionForm { get; set; }
        public List<AnswerVM> Answers { get; set; }
        public int QuestionFormId { get; set; } // Add this property
    }
}
