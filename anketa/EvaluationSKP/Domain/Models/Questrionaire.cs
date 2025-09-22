namespace Domain.Models
{
    public class Questrionaire : BaseEntity
    {
        public int QuestionaireNumber { get; set; }
        public string TitlePage { get; set; } = string.Empty;

    }
}
