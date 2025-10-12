namespace GlasAnketa.Domain.Models
{
    public class AnswerSummary
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int TotalResponses { get; set; }
        public double? AverageScaleValue { get; set; }
        public Dictionary<int, int> ScaleValueDistribution { get; set; } = new Dictionary<int, int>();
        public List<string> TextResponses { get; set; } = new List<string>();
    }
}
