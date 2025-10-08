namespace GlasAnketa.ViewModels.Models
{
    public class ThankYouVM
    {
        public int SubmittedCount { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Message => SubmittedCount > 0
            ? $"Thank you for submitting {SubmittedCount} answers!"
            : "Thank you for your participation!";
    }
}
