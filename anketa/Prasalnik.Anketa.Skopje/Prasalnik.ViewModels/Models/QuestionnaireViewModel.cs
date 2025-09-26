namespace Prasalnik.ViewModels.Models
{
    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public QuestionItemViewModel QuestionItems { get; set; }
    }
}
