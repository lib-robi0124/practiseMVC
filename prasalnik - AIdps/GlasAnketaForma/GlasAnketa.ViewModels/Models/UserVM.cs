namespace GlasAnketa.ViewModels.Models
{
    public class UserVM
    {
            public int Id { get; set; }
            public int CompanyId { get; set; }
            public string FullName { get; set; }
            public string OU { get; set; }
            public string OU2 { get; set; }
            public RoleVM Role { get; set; }
        
    }
}
