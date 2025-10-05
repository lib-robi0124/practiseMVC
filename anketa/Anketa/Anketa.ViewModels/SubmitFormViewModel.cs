using System.ComponentModel.DataAnnotations;

namespace Anketa.ViewModels
{
    public class SubmitFormViewModel : FormViewModel
    {
        [Required]
        public int FormId { get; set; }

        public string FormTitle { get; set; } = string.Empty;

        public int UserId { get; set; }

        [Required]
        public List<AnswerInputViewModel> Answers { get; set; } = new();
    }
}
