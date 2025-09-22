using Domain.Enums;

namespace Domain.Models
{
    public class User : BaseEntity
    {
        public int CompanyId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public RoleEnum Role { get; set; } = RoleEnum.Employee;
    }
}
