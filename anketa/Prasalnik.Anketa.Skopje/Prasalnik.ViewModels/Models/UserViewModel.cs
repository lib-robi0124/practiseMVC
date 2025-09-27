namespace Prasalnik.ViewModels.Models
{
    public class UserViewModel
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; }
        public string Role { get; set; } = string.Empty;
        public object Id { get; set; }
    }
}
