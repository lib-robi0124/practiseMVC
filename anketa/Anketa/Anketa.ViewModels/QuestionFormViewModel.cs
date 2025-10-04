namespace Anketa.ViewModels
{
    public class QuestionFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new();
    }
}
