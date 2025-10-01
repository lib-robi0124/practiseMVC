using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Questionnaire.DataAccess.DataContext;

namespace Questionnaire.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
