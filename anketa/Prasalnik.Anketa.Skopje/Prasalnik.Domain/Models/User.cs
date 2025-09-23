
using Prasalnik.Domain.Enums;

namespace Prasalnik.Domain.Models
{
    public class User : BaseEntity
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string OU { get; set; } // Organizational Unit
        public RoleEnum Role { get; set; }
    }
}
