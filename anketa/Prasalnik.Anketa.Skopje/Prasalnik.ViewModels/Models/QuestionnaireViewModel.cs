namespace Prasalnik.ViewModels.Models
{
    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Use a collection here (previously it was a single QuestionItemViewModel)
        public List<QuestionItemViewModel> QuestionItems { get; set; } = new();
    }
}
