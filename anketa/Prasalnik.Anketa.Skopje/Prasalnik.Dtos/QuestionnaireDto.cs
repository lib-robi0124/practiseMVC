namespace Prasalnik.Dtos
{
    public class QuestionnaireDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public List<QuestionItemDto> QuestionItems { get; set; } = new();
    }
}
