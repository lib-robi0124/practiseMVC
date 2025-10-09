namespace GlasAnketa.ViewModels.Models
{
    public class ResultsVM
    {
        public List<AnswerVM> Answers { get; set; }
        public Dictionary<int, AnswerSummaryVM> Summaries { get; set; }
    }
}
