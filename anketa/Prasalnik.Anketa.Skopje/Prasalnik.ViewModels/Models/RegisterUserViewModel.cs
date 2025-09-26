namespace Prasalnik.ViewModels.Models
{
    public class RegisterUserViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
