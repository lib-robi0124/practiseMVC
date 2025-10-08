namespace GlasAnketa.ViewModels.Models
{
    public class UserVM
    {
            public int Id { get; set; }
            public int CompanyId { get; set; }
            public string FullName { get; set; }
            public string Password { get; set; }
            public string RoleKey { get; set; }
            public RoleVM Role { get; set; }
        
    }
}
