namespace GlasAnketa.Domain.Models
{
    public class AnswerValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
