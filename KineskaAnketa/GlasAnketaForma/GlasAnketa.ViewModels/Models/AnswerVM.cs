using GlasAnketa.Domain.Models;

namespace GlasAnketa.ViewModels.Models
{
    public class AnswerVM
    {
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionFormId { get; set; }
        public int? ScaleValue { get; set; } // For scale-based questions, nulable if not applicable
        public string? TextValue { get; set; } // For text-based questions
        public DateTime AnsweredDate { get; set; } = DateTime.UtcNow;
    }
}