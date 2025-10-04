using System.ComponentModel.DataAnnotations;

namespace Anketa.ViewModels
{
    public class AdminQuestionCreateViewModel
    {
        [Required(ErrorMessage = "Question text is required")]
        [Display(Name = "Question Text")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a question type")]
        [Display(Name = "Question Type")]
        public string QuestionType { get; set; } = "Text"; // Default

        [Display(Name = "Required?")]
        public bool IsRequired { get; set; } = true;
    }
}
