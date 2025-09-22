using Lamazon.DataAccess.DataContext;
using Lamazon.DataAccess.Implementacija;
using Lamazon.DataAccess.Interfaces;
using Lamazon.Services.Implementations;
using Lamazon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lamazon.Services.Extensions
{
    public static class InjectionExtensions
    {
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            // Here you can add your service injections, for example:
            // services.AddScoped<IYourService, YourServiceImplementation>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        public static void InjectRepositories(this IServiceCollection services)
        {
            // Here you can add your repository injections, for example:
            // services.AddScoped<IYourRepository, YourRepositoryImplementation>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
        public static void InjectServices(this IServiceCollection services)
        {
            // Here you can add your service injections, for example:
            // services.AddScoped<IYourService, YourServiceImplementation>();
            // services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductService, ProductService>();
        }
        public static void InjectAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
    
