using System.ComponentModel.DataAnnotations;

namespace Anketa.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
