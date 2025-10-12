namespace GlasAnketa.ViewModels.Models
{
    public class AnswerSummaryVM
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public double AverageScaleValue { get; set; }
        public int ResponseCount { get; set; }
        public List<string> TextResponses { get; set; }
    }
}
