namespace Prasalnik.ViewModels.Models
{
    public class QuestionItemViewModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int QuestionnaireId { get; set; }
        public bool IsRequired { get; set; }
    }
}
