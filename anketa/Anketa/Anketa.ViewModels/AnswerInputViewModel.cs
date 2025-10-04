using System.ComponentModel.DataAnnotations;

namespace Anketa.ViewModels
{
    public class AnswerInputViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Scale value must be between 1 and 10")]
        public int? ScaleValue { get; set; }

        [StringLength(500, ErrorMessage = "Text answer cannot exceed 500 characters")]
        public string TextValue { get; set; }
        public bool QuestionTypeIsScale() =>
            QuestionType.Equals("Scale", StringComparison.OrdinalIgnoreCase);
    }
}
