namespace GlasAnketa.Domain.Models
{
    public class AnswerSubmissionResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public int SubmittedAnswersCount { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}