namespace Anketa.ViewModels
{
    public class AdminFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<QuestionViewModel> Questions { get; set; } = new();
    }
}
