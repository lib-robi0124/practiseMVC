using GlasAnketa.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace GlasAnketa.ViewModels.Models
{
    public class QuestionFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public int QuestionCount { get; set; }
        public int ResponseCount { get; set; }

        // Navigation properties
        public List<QuestionVM> Questions { get; set; } = new List<QuestionVM>();
        public List<AnswerVM> Answers { get; set; } = new List<AnswerVM>();
    }
}
