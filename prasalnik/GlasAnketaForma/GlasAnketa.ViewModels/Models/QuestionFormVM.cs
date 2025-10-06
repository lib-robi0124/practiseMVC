using GlasAnketa.Domain.Models;

namespace GlasAnketa.ViewModels.Models
{
    public class QuestionFormVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<QuestionVM>? Questions { get; set; }
        public List<AnswerVM>? Answers { get; set; }
        
    }
}
