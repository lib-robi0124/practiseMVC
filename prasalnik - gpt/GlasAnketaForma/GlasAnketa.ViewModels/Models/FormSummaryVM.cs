namespace GlasAnketa.ViewModels.Models
{
    public class FormSummaryVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QuestionCount { get; set; }
        public int ResponseCount { get; set; }
        public double CompletionRate { get; set; }
    }
}
