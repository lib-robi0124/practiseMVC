using System.ComponentModel.DataAnnotations;

namespace GlasAnketa.ViewModels.Models
{
    public class CreateQuestionFormVM
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Initial questions
        public List<RegisterQuestionVM> Questions { get; set; } = new List<RegisterQuestionVM>();
    }
}