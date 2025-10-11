using System.ComponentModel.DataAnnotations;

namespace GlasAnketa.ViewModels.Models
{
    public class UserCredentialsVM
    {
        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company ID")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
