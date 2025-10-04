namespace Anketa.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string QuestionType { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
    }
}
