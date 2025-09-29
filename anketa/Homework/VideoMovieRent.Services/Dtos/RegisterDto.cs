using System.ComponentModel.DataAnnotations;

namespace VideoMovieRent.Services.Dtos
{
    public class RegisterDto
    {
        [Required] public string FullName { get; set; }
        [Required] public string CardNumber { get; set; }
        [Required]
        [Range(18,80, ErrorMessage = "Age must be between 18 and 80.")]
        public int Age { get; set; }
    }
}
