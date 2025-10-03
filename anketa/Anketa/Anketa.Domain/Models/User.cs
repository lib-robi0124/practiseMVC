namespace Anketa.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; } // Organizational Unit
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<Answer> Answers { get; set; }
    }
}
