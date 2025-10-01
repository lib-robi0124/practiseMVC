using Lamazon.Services.Middlewaires;
using Microsoft.AspNetCore.Builder;

namespace Lamazon.Services.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDebugIpAddressMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DebugIpAddressMiddleware>();
        }
    }
}
