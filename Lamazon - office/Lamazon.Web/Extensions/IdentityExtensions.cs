using Lamazon.Domain.Constants;
using System.Security.Claims;

namespace Lamazon.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static string DisplayName(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        }
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.IsInRole(Roles.Admin);
        }
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var primarySid = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimaryGroupSid)?.Value ?? string.Empty;
            if(string.IsNullOrEmpty(primarySid))
            {
                return 0;
            }
            return int.Parse(primarySid);
        }
    }
}
